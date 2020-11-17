using System;
using System.Collections.Generic;

namespace ejerc1
{
    enum EstadoAsiento
    {
        Libre,
        Ocupado
    }

    class Persona
    {
        public string Nombre;
        public string Apellido;

        public Persona(string n, string a)
        {
            Nombre = n;
            Apellido = a;
        }
    }

    class Película
    {
        public string Título;
        public string Título_Original;
        public Persona Director;
        public Dictionary<Persona, string> Reparto;
        public TimeSpan Duración = new TimeSpan();
        public string Sinopsis;

        public Película(string titulo, string titulo_original, Persona dire, Dictionary<Persona, string> reparto, TimeSpan duracion, string sino)
        {
            Título = titulo;
            Título_Original = titulo_original;
            Director = dire;
            Reparto = reparto;
            Duración = duracion;
            Sinopsis = sino;
        }
    }

    class Asiento
    {
        public int Fila;
        public int Columna;
        public bool EsVip;

        public Asiento(int f, int c)
        {
            Fila = f;
            Columna = c;
        }
    }

    class Sala
    {
        public int Numero;
        public Asiento[,] Asientos;

        public Sala(int n, int x, int y)
        {
            Numero = n;
            Asientos = new Asiento[x, y];

            for (int i = 0; i < Asientos.GetLength(0); i++)
            {
                for (int j = 0; j < Asientos.GetLength(1); j++)
                {
                    Asientos[i, j] = new Asiento(i, j);
                }
            }
        }
    }

    class Funcion
    {
        public Película Película;
        public Sala Sala;
        public DateTime Fecha_Hora;
        public Dictionary<Asiento, EstadoAsiento> Estado_Asientos;
        public bool Lleno;
        public Funcion(Película p, Sala s, DateTime fh)
        {
            Película = p;
            Sala = s;
            Fecha_Hora = fh;
            Estado_Asientos = new Dictionary<Asiento, EstadoAsiento>();

            foreach (Asiento a in Sala.Asientos)
            {
                Estado_Asientos.Add(a, EstadoAsiento.Libre);
            }

            Lleno = false;
        }
        public bool IntentarOcuparAsiento(Asiento a)
        {
            if (Estado_Asientos[a] == EstadoAsiento.Ocupado)
            {
                return false;
            }

            if (Estado_Asientos[a] == EstadoAsiento.Libre)
            {
                Estado_Asientos[a] = EstadoAsiento.Ocupado;
                return true;
            }

            if (this.Sala.Asientos[a.Fila, a.Columna] != a)
            {
                throw new Exception("El asiento no se corresponde con la sala");
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }

    class Entrada
    {
        public Funcion Funcion;
        public Asiento Asiento;
        public decimal Precio;
        public DateTime FechaEmision;
        public Entrada(Funcion f, Asiento a)
        {
            Funcion = f;
            Asiento = a;
            FechaEmision = DateTime.Now;
        }
    }
    class Cine
    {
        public string Nombre;
        public List<Película> Películas;
        public List<Sala> Salas;
        public List<Funcion> Funciones;
        public List<Entrada> Entradas;
        public decimal PrecioEntrada;
        public decimal PrecioEntradaVIP;
        public Cine(string n, decimal p, decimal pV)
        {
            Nombre = n;
            PrecioEntrada = p;
            PrecioEntradaVIP = pV;
        }
        public List<Funcion> BuscarFuncion(Película p)
        {
            List<Funcion> func = new List<Funcion>();

            if (p == null)
                throw new ArgumentNullException();

            foreach (Funcion f in Funciones)
                if (f.Película == p)
                    func.Add(f);

            return func;
        }
    }
    class DB
    {
        public List<Película> DB_Peliculas;
        public List<Sala> DB_Salas;
        public List<Funcion> DB_Funciones;
        public List<Entrada> DB_Entradas;
        public Cine Cine;
    }

    class Program
    {
        /* Carga de datos */
        public static void DB_Poblar(DB db)
        {
            /* Peliculas y personas */
            Dictionary<string, Persona> Directores = new Dictionary<string, Persona>
            {
                { "The Shawshank Redemption", new Persona("Frank", "Darabont")},
                { "Schindler's List", new Persona("Steven", "Spielberg")},
                { "The Pianist", new Persona("Roman", "Polanski")}
            };

            Dictionary<Persona, string> CastingPelicula1 = new Dictionary<Persona, string>
            {
                { new Persona("Tim", "Robbins"), "Andy Dufresne" },
                { new Persona("Morgan", "Freeman"), "Ellis Boyd 'Red' Redding" },
                { new Persona("Bob", "Gunton"), "Warden Norton" }
            };

            Dictionary<Persona, string> CastingPelicula2 = new Dictionary<Persona, string>
            {
                { new Persona("Liam", "Neeson"), "Oskar Schindler" },
                { new Persona("Ben", "Kingsley"), "Itzhak Stern" },
                { new Persona("Ralph", "Fiennes"), "Amon Goeth" }
            };

            Dictionary<Persona, string> CastingPelicula3 = new Dictionary<Persona, string>
            {
                { new Persona("Adrien", "Brody"), "Wladyslaw Szpilman" },
                { new Persona("Emilia", "Fox"), "Dorota" },
                { new Persona("Michal", "Zebrowski"), "Jurek" }
            };

            List<Película> Peliculas_Activas = new List<Película>
            {
                { new Película("Cadena perpetua", "The Shawshank Redemption", Directores["The Shawshank Redemption"], CastingPelicula1, new TimeSpan(2, 22, 00), "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.") },
                { new Película("La lista de Schindler", "Schindler's List", Directores["Schindler's List"], CastingPelicula2, new TimeSpan(3, 15, 00), "In German-occupied Poland during World War II, industrialist Oskar Schindler gradually becomes concerned for his Jewish workforce after witnessing their persecution by the Nazis.") },
                { new Película("El pianista", "The Pianist", Directores["The Pianist"], CastingPelicula3, new TimeSpan(2, 30, 00), "A Polish Jewish musician struggles to survive the destruction of the Warsaw ghetto of World War II.") }
            };

            db.DB_Peliculas = Peliculas_Activas;

            /* Salas y Asientos VIP */

            List<Sala> Salas_Activas = new List<Sala>
            {
                { new Sala(0, 9, 6) },
                { new Sala(1, 8, 6) },
                { new Sala(2, 9, 8)}
            };

            db.DB_Salas = Salas_Activas;

            db.DB_Salas[0].Asientos[8, 2].EsVip = true;
            db.DB_Salas[0].Asientos[8, 3].EsVip = true;
            db.DB_Salas[0].Asientos[8, 4].EsVip = true;
            db.DB_Salas[1].Asientos[7, 2].EsVip = true;
            db.DB_Salas[1].Asientos[7, 3].EsVip = true;
            db.DB_Salas[1].Asientos[7, 4].EsVip = true;
            db.DB_Salas[2].Asientos[8, 2].EsVip = true;
            db.DB_Salas[2].Asientos[8, 3].EsVip = true;
            db.DB_Salas[2].Asientos[8, 4].EsVip = true;

            /* Funciones */

            List<Funcion> Funciones_Activas = new List<Funcion>
            {
                { new Funcion(db.DB_Peliculas[0], db.DB_Salas[2], new DateTime(2020, 11, 19, 18, 15, 0)) },
                { new Funcion(db.DB_Peliculas[1], db.DB_Salas[0], new DateTime(2020, 11, 19, 22, 30, 0)) },
                { new Funcion(db.DB_Peliculas[2], db.DB_Salas[2], new DateTime(2020, 11, 19, 23, 15, 0)) },
                { new Funcion(db.DB_Peliculas[0], db.DB_Salas[1], new DateTime(2020, 11, 20, 16, 20, 0)) },
                { new Funcion(db.DB_Peliculas[1], db.DB_Salas[0], new DateTime(2020, 11, 20, 18, 15, 0)) },
                { new Funcion(db.DB_Peliculas[2], db.DB_Salas[1], new DateTime(2020, 11, 20, 23, 30, 0)) },
            };

            db.DB_Funciones = Funciones_Activas;

            db.DB_Funciones[0].Estado_Asientos[db.DB_Salas[2].Asientos[1, 1]] = EstadoAsiento.Ocupado;
            db.DB_Funciones[0].Estado_Asientos[db.DB_Salas[2].Asientos[1, 2]] = EstadoAsiento.Ocupado;
            db.DB_Funciones[1].Estado_Asientos[db.DB_Salas[0].Asientos[3, 5]] = EstadoAsiento.Ocupado;
            db.DB_Funciones[2].Estado_Asientos[db.DB_Salas[2].Asientos[4, 0]] = EstadoAsiento.Ocupado;
            db.DB_Funciones[3].Estado_Asientos[db.DB_Salas[1].Asientos[4, 4]] = EstadoAsiento.Ocupado;
            db.DB_Funciones[3].Estado_Asientos[db.DB_Salas[1].Asientos[4, 5]] = EstadoAsiento.Ocupado;
            db.DB_Funciones[4].Estado_Asientos[db.DB_Salas[2].Asientos[1, 1]] = EstadoAsiento.Ocupado;


            /* Entradas */

            db.DB_Entradas = new List<Entrada>();

            /* Cine */

            db.Cine = new Cine("CinemaParadise", 450, 600)
            {
                Películas = db.DB_Peliculas,
                Salas = db.DB_Salas,
                Funciones = db.DB_Funciones,
                Entradas = db.DB_Entradas
            };
        }

        /* Fin Carga de datos */

        /* Funciones */
        static void Saludar()
        {
            Console.WriteLine("Bienvenido a CinemaParadise");
            Console.WriteLine("¿Qué película quieres ver?");
            Console.WriteLine();
        }
        static void Reiniciar(DB db)
        {
            Console.Clear();
            Ejecutar_Ticketeadora_Cine(db);
        }
        static bool EsVip(Funcion f, Asiento a)
        {
            return (f.Sala.Asientos[a.Fila, a.Columna].EsVip);
        }
        static bool EsDiaPromo(DayOfWeek d)
        {
            return d == DayOfWeek.Wednesday || d == DayOfWeek.Thursday;
        }
        /* Fin Funciones */

        /* Funciones de vista de -Seleccion de Pelicula- */
        static void Mostrar_Peliculas(List<Película> lp)
        {
            for (int i = 0; i < lp.Count; i++)
            {
                Película p = lp[i];

                Console.WriteLine($"{i} - {p.Título} ({p.Título_Original})");
            }
        }
        static Película Pedir_Seleccion_Pelicula(List<Película> lp)
        {
            Console.WriteLine();
            Console.WriteLine("Su eleccion:");

            if (!Int32.TryParse(Console.ReadLine(), out int salida))
            {
                Console.WriteLine("Seleccion Incorrecta");
                Pedir_Seleccion_Pelicula(lp);
            }

            if (salida > lp.Count - 1 || salida < 0)
            {
                Console.WriteLine("Seleccion Incorrecta");
                Pedir_Seleccion_Pelicula(lp);
            }

            return lp[salida];
        }

        /* Fin Funciones de vista de -Seleccion Pelicula- */


        /* Funciones de vista de -Informacion Pelicula y Seleccion Pelicula- */
        static void Mostrar_Info_Pelicula(Película pelicula)
        {
            Console.WriteLine($"Titulo: {pelicula.Título}");
            Console.WriteLine($"Titulo Original: {pelicula.Título_Original}");
            Console.WriteLine($"Duracion: {pelicula.Duración}");
            Console.WriteLine("Reparto:");

            foreach (KeyValuePair<Persona, string> persona in pelicula.Reparto)
            {
                Console.WriteLine($"*{persona.Key.Nombre + " " + persona.Key.Apellido} como {persona.Value}");
            }

            Console.WriteLine("Sinopsis:");
            Console.WriteLine($"{pelicula.Sinopsis}");
            Console.WriteLine();
        }

        static void Mostrar_Funciones_Pelicula(Película pelicula, DB db)
        {
            Console.WriteLine("Funciones:");

            List<Funcion> Funciones_Pelicula = db.Cine.BuscarFuncion(pelicula);

            for (int i = 0; i < Funciones_Pelicula.Count; i++)
            {
                if (!Funciones_Pelicula[i].Lleno)
                {
                    Console.WriteLine($"{i} - {Funciones_Pelicula[i].Fecha_Hora}");
                }
            }
            Console.WriteLine($"{Funciones_Pelicula.Count} - Cancelar");

            Seleccion_Asiento(Pedir_Seleccion_Funcion(Funciones_Pelicula, db), db);
        }

        static Funcion Pedir_Seleccion_Funcion(List<Funcion> lf, DB db)
        {
            int salida = 0;
            Console.WriteLine();
            Console.WriteLine("Su eleccion:");

            if (!Int32.TryParse(Console.ReadLine(), out salida))
            {
                Console.WriteLine("Seleccion Incorrecta");
                Pedir_Seleccion_Funcion(lf, db);
            }

            if (salida > lf.Count || salida < 0)
            {
                Console.WriteLine("Seleccion Incorrecta");
                Pedir_Seleccion_Funcion(lf, db);
            }
            else if (salida == lf.Count)
            {
                Reiniciar(db);
            }

            return lf[salida];
        }

        /* Fin Funciones de vista de -Informacion Pelicula y Seleccion Pelicula- */

        /* Funciones de vista de -Informacion Asientos y Seleccion Asiento - */
        static void Renderizar_Sala(Funcion funcion)
        {
            Renderizar_Pantalla(funcion);

            for (int i = 0; i < funcion.Sala.Asientos.GetLength(0); i++)
            {
                Console.Write($"{i}");

                for (int j = 0; j < funcion.Sala.Asientos.GetLength(1); j++)
                {
                    Console.Write("[");

                    Asiento asiento = funcion.Sala.Asientos[i, j];

                    if (funcion.Estado_Asientos[asiento] == EstadoAsiento.Ocupado)
                    {
                        Console.Write("O");
                    }
                    else if (funcion.Sala.Asientos[i, j].EsVip && funcion.Estado_Asientos[asiento] == EstadoAsiento.Libre)
                    {
                        Console.Write("V");
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                    Console.Write("]");
                }
                Console.WriteLine();
            }

        }
        static void Renderizar_Pantalla(Funcion funcion)
        {
            Console.WriteLine();

            int TotalCaracteres = 3 * funcion.Sala.Asientos.GetLength(1) - 8;
            int CaracteresIzq = TotalCaracteres / 2;
            int CaracteresDer = TotalCaracteres / 2;

            if (TotalCaracteres % 2 != 0)
            {
                CaracteresDer++;
            }

            Console.Write(" ");

            for (int i = 0; i < CaracteresIzq; i++)
            {
                Console.Write("=");
            }

            Console.Write("PANTALLA");

            for (int i = 0; i < CaracteresDer; i++)
            {
                Console.Write("=");
            }

            Console.WriteLine();
            Console.Write(" ");
            for (int i = 0; i < funcion.Sala.Asientos.GetLength(1); i++)
            {
                Console.Write(" ");
                Console.Write($"{i}");
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        static void Renderizar_Referencias(Funcion funcion, DB db)
        {
            Console.WriteLine();

            Console.WriteLine("Referencias:");
            Console.WriteLine($"[ ] = Libre ${RenderizarPrecio(funcion, false)} {EsPromo(funcion)}");
            Console.WriteLine($"[V] = VIP Libre ${RenderizarPrecio(funcion, true)} {EsPromo(funcion)}");
            Console.WriteLine("[O] = Ocupado");

            PedirFilaColumnaDeAsiento(funcion, db);
        }
        static decimal RenderizarPrecio(Funcion f, bool VIP)
        {
            if (EsDiaPromo(f.Fecha_Hora.DayOfWeek) && !VIP)
                return 600 / 2;

            if (EsDiaPromo(f.Fecha_Hora.DayOfWeek) && VIP)
                return 800 / 2;

            if (!EsDiaPromo(f.Fecha_Hora.DayOfWeek) && !VIP)
                return 600;

            return 800;
        }
        static string EsPromo(Funcion f) // Renderiza mensaje de -promo-
        {
            DayOfWeek DiaFuncion = f.Fecha_Hora.DayOfWeek;

            if (DiaFuncion == DayOfWeek.Wednesday || DiaFuncion == DayOfWeek.Thursday)
                return "(precio promo)";
            else
                return " ";
        }
        static void PedirFilaColumnaDeAsiento(Funcion funcion, DB db)
        {
            /*-------Validacion Min y Max de input Fila y Col-------*/ // HORRIBLE
            Console.WriteLine("Ingrese Fila:");
            if (!Int32.TryParse(Console.ReadLine(), out int fila))
            {
                Console.WriteLine("Seleccion Incorrecta");
                PedirFilaColumnaDeAsiento(funcion, db);
            }

            if (fila == -1)
                Reiniciar(db);

            Console.WriteLine("Ingrese Columna:");
            if (!Int32.TryParse(Console.ReadLine(), out int columna))
            {
                Console.WriteLine("Seleccion Incorrecta");
                PedirFilaColumnaDeAsiento(funcion, db);
            }

            if (columna == -1)
                Reiniciar(db);

            if (fila < -1 || fila > funcion.Sala.Asientos.GetLength(1) + 1 || columna < -1 || columna > funcion.Sala.Asientos.GetLength(0))
            {
                Console.WriteLine("Seleccion Incorrecta");
                PedirFilaColumnaDeAsiento(funcion, db);
            }
            /*-------Fin Validacion Min y Max de input Fila y Col-------*/

            Asiento AsientoNuevo = funcion.Sala.Asientos[fila, columna];

            if (!funcion.IntentarOcuparAsiento(AsientoNuevo))
            {
                Console.WriteLine("Asiento Ocupado");
                PedirFilaColumnaDeAsiento(funcion, db);
            }

            GenerarEntrada(AsientoNuevo, funcion, db);
        }
        static void GenerarEntrada(Asiento a, Funcion f, DB db)
        {
            Entrada NuevaEntrada = new Entrada(f, a);

            NuevaEntrada.Precio = CalcularPrecioEntrada(NuevaEntrada, f);

            db.DB_Entradas.Add(NuevaEntrada);

            Mostrar_Entrada(NuevaEntrada);
        }
        static void Mostrar_Entrada(Entrada e)
        {
            Console.Clear();
            Console.WriteLine("Su entrada:");
            Console.WriteLine($"Pelicula: {e.Funcion.Película.Título}");
            Console.WriteLine($"Dia: {e.Funcion.Fecha_Hora.Date}");
            Console.WriteLine($"Horario: {e.Funcion.Fecha_Hora.TimeOfDay}");
            Console.WriteLine($"Asiento: [fila: {e.Asiento.Fila}, columna: {e.Asiento.Columna}]");
            Console.WriteLine($"Precio: ${e.Precio}");

            Continuar();
        }
        static void Mostrar_Info_Funcion(Funcion funcion, DB db)
        {
            Console.WriteLine($"Pelicula: {funcion.Película.Título}");
            Console.WriteLine($"Funcion: {funcion.Fecha_Hora}");

            Renderizar_Sala(funcion);

            Renderizar_Referencias(funcion, db);
        }
        static decimal CalcularPrecioEntrada(Entrada e, Funcion f)
        {
            if (f.Sala.Asientos[e.Asiento.Fila, e.Asiento.Columna].EsVip)
                return 800;

            return 600;
        }
        static void Continuar()
        {
            Console.WriteLine();
        }
        /* Fin Funciones de vista de -Informacion Asientos y Seleccion Asiento- */

        /* Vistas */
        static void Ejecutar_Ticketeadora_Cine(DB db)
        {
            Saludar();

            Mostrar_Peliculas(db.Cine.Películas);

            Seleccion_Funcion(Pedir_Seleccion_Pelicula(db.Cine.Películas), db);
        }

        static void Seleccion_Funcion(Película pelicula, DB db)
        {
            Console.Clear();

            Mostrar_Info_Pelicula(pelicula);

            Mostrar_Funciones_Pelicula(pelicula, db);
        }

        static void Seleccion_Asiento(Funcion funcion, DB db)
        {
            Console.Clear();

            Mostrar_Info_Funcion(funcion, db);

            Reiniciar(db);
        }

        /* Fin vistas */

        static void Main(string[] args)
        {
            DB DB_prueba = new DB();
            DB_Poblar(DB_prueba);
            Ejecutar_Ticketeadora_Cine(DB_prueba);
        }
    }
}