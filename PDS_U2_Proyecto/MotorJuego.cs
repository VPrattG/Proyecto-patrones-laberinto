using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_U2_Proyecto
{
    public class MotorJuego
    {
        private static MotorJuego _instance;
        private static readonly object _lock = new object();
        public Jugador JugadorActual { get; private set; }
        public Nivel NivelActual { get; private set; }
        public Cuarto CuartoActual { get; private set; }
        public bool Activo { get; private set; }
        Random r = new Random();
        
        private MotorJuego() 
        {
            // Esto es por diversión
            Console.WriteLine("brrrrum brrrum");
            Thread.Sleep(1000);
            Console.Clear();
        }

        public static MotorJuego instance
        {
            get
            {
                if (_instance == null)
                {
                    // Asegura que solo haya un subproceso ejecutándolo a la vez
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MotorJuego();
                        }
                    }
                }
                return _instance; // Se devuelve la instancia
            }
        }

        public void CargarNivel(Nivel nivel)
        {
            // Para comprobar que funcione
            NivelActual = nivel;
            Console.WriteLine($"Cuarto inicial: {nivel.CuartoInicial.Nombre}. Hash: {nivel.CuartoInicial.GetHashCode()}");
            Console.WriteLine($"El nivel {nivel.Nombre} ha sido cargado");
        }

        public void CrearJugador(string nombre)
        {
            // El jugador es ingresado inmediatamente.
            JugadorActual = new Jugador(nombre, 200, 10);
            Console.WriteLine($"¡Bienvenido, valiente {nombre}!");
            Console.ReadKey();
        }

        public void IniciarJuego()
        {
            // Se asegura que todos los componentes estén inicializados
            if(NivelActual == null) { Console.WriteLine("El nivel no se ha inicializado. No se puede empezar el juego."); return; }
            if (JugadorActual == null) { Console.WriteLine("El jugador no se ha inicializado. No se puede empezar el juego."); return; }
            
            // Se declara la variable de inicio del juego
            Activo = true;
            
            // Como se debe empezar en un cuarto, se usa el cuarto inicial declarado en el nivel
            CuartoActual = NivelActual.CuartoInicial;
            CicloDeJuego();
        }

        public void DesplegarCuartoActual()
        {
            // Se limpia la pantalla para no sobrecargar al jugador de información
            Console.Clear();
            Console.WriteLine($"Usted se encuentra en: {CuartoActual.Nombre}");
            Console.WriteLine(CuartoActual.Descripcion);

            if (CuartoActual.Entidades.Count > 0)
            {
                Console.WriteLine("\n Usted observa:");
                
                for(int i = 0; i < CuartoActual.Entidades.Count; i++)
                {
                    var entidad = CuartoActual.Entidades[i];
                    Console.WriteLine($"~ {entidad.Nombre}");
                    
                    if (entidad is Enemigo e)
                    {
                        Console.WriteLine("...");
                        Thread.Sleep(1000);
                        Console.WriteLine("¡Un enemigo!");
                        Thread.Sleep(1000);
                        
                        Combate(JugadorActual, e);
                        
                        return;
                    }
                }
            }

            if (CuartoActual.Salidas == null)
            {
                Console.WriteLine("No hay salidas disponibles");
            }
            else
            {
                Console.WriteLine("Las salidas disponibles son:");
                
                foreach (var sentido in CuartoActual.Salidas)
                {
                    Console.WriteLine($"- {sentido.Key} --> {sentido.Value.Nombre}");
                }
            }
            
            Console.Write("¿Qué acción desea tomar? (Ingrese h para recibir ayuda) ");
            // ToLower permite que cualquier variación de mayúsculas y minúsculas sea aceptada como comando
            string comando = Console.ReadLine().ToLower();
            ProcesarComando(comando);
        }

        public void Hablar(string persona)
        {
            var entidad = CuartoActual.Entidades.FirstOrDefault(p => p.Nombre.Equals(persona, StringComparison.OrdinalIgnoreCase));
            
            if (entidad != null && entidad is Personaje p)
            {
                Console.WriteLine($"\"{p.Texto}\"");
            }
            else
            {
                Console.WriteLine($"La persona {persona} no se encuentra aquí.");
            }
            Console.ReadKey();
        }
        
        public void DesplegarInventario()
        {
            Console.WriteLine("Inventario actual: ");
            
            for (int i = 0; i < JugadorActual.Inventario.Count; i++)
            {
                Console.WriteLine($"* {JugadorActual.Inventario[i].Nombre}");
            }
            Console.ReadKey();
        }
        
        public void TomarObjeto(Objeto o)
        {
            Console.Write("Objeto encontrado, ¿desea tomarlo? (S/N) ");
            string respuesta = Console.ReadLine();
            
            switch (respuesta)
            {
                case "S" or "s":
                    if (o.Agarrable)
                    {
                        JugadorActual.AgarrarObjeto(o);
                        CuartoActual.Entidades.Remove(o);
            
                        Console.WriteLine("El objeto ha sido añadido al inventario");
                    }
                    else if (o.Nombre == "Cofre")
                    {
                        Console.WriteLine("El cofre es tan viejo que se desintegró.");
                        Console.WriteLine("¡Algo quedó en su lugar!");

                        string[] lista = { "espada", "pocima", "dardo", "escudo", "superescudo"};

                        CuartoActual.Entidades.Remove(o);
                        
                        CuartoActual.AgregarEntidad(Fab_Entidad.CrearObjeto(lista[r.Next(lista.Length)]));
                    }
                    else
                    {
                        Console.WriteLine("El objeto no se puede agarrar...");
                    }
                    break;
                case "N" or "n":
                    Console.WriteLine("Ha decidido no tomar el objeto.");
                    break;
                default:
                    Console.WriteLine("Comando desconocido, se ignoró el objeto.");
                    break;
            }

            // Evita muchos mensajes de continuar
            return;
        }
        
        public void Ayuda()
        {
            Console.WriteLine("Comandos disponibles:");
            
            Console.WriteLine("\'ir al (dirección)\'      : Desplaza al usuario en dicha dirección si es posible.");
            Console.WriteLine("\'examinar (entidad)\'     : Obtiene la descripción de la entidad seleccionada.");
            Console.WriteLine("                           - Si la entidad es un objeto, se puede tomar.");
            Console.WriteLine("\'hablar con (personaje)\' : Conversa con un personaje en el cuarto.");
            Console.WriteLine("\'inventario\'             : Muestra el inventario del usuario");
            Console.WriteLine("\'salir\'                  : Termina el juego.");
            
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        public void ProcesarComando(string comando)
        {
            switch (comando)
            {
                case string s when s.StartsWith("ir al "):
                    string sentido = s.Substring(6);
                    MoverAlJugador(sentido);
                    break;
                case string s when s.StartsWith("examinar "):
                    string objetivo = s.Substring(9);
                    Examinar(objetivo);
                    break;
                case string s when s.StartsWith("hablar con "):
                    string morro = s.Substring(11);
                    Hablar(morro);
                    break;
                case "inventario":
                    DesplegarInventario();
                    break;
                case "h":
                    Ayuda();
                    break;
                case "salir":
                    Activo = false;
        
                    Console.WriteLine($"{JugadorActual.Nombre} ha decidido abandonar cualquier sueño de conquistar la mazmorra... por ahora.");
                    Console.WriteLine("El juego ha finalizado.");
                    break;
                default:
                    Console.WriteLine("Comando desconocido. ¿Ingresaste la información correctamente?");
                    Console.ReadKey();
                    break;
            }
        }
        public void Combate(Jugador j, Enemigo e)
        {
            double modDefensa = 1;
            int modFuerza = j.Fuerza;
            string opcion;
            int temp;
            Objeto objeto;
            
            for(int i=0;i<j.Inventario.Count;i++)
            {
                if (j.Inventario[i].Nombre == "Espada")
                {
                    modFuerza += 15;
                }
                if (j.Inventario[i].Nombre == "Escudo")
                {
                    modDefensa += 0.25;
                }
            }
            
            while (e.Vida > 0 && j.Vida > 0)
            {
                Console.Clear();
                Console.WriteLine("==============COMBATE==============");
                Console.WriteLine($"Vida: {j.Vida} Fuerza: {modFuerza} Defensa: {modDefensa}");
                Console.WriteLine($"Vida del enemigo: {e.Vida}");

                Console.WriteLine("\n~~Opciones~~");
                Console.WriteLine("1. Atacar");
                Console.WriteLine("2. Curar");
                Console.WriteLine("3. Usar dardo");
                Console.WriteLine("4. Inventario");
                Console.WriteLine("5. Examinar");
                Console.WriteLine("6. Ayuda");
                
                Console.Write("\n ¿Qué acción desea realizar? ");
                opcion = Console.ReadLine().ToLower();
                
                switch (opcion)
                {
                    case "1": case "atacar":
                        Console.WriteLine("¡Usted ataca!");
                        
                        temp = Ataque(modFuerza);
                        Console.WriteLine($"{e.Nombre} recibe {temp} de daño");
                        e.Vida -= temp;
                        
                        if (e.Vida > 0)
                        {
                            Console.WriteLine($"¡{e.Nombre} ataca!");
                            objeto = j.Inventario.FirstOrDefault(obj => obj.Nombre.Equals("Escudo Mágico", StringComparison.OrdinalIgnoreCase));
                            
                            if (objeto!=null)
                            {
                                Console.WriteLine("¡El escudo mágico bloqueo el daño!");
                                Console.WriteLine("¡El escudo mágico se desvaneció!");
                        
                                j.Inventario.Remove(objeto);
                            }
                            else
                            {
                                temp = Ataque(e.Fuerza, modDefensa);
                                Console.WriteLine($"{j.Nombre} recibe {temp} de daño");
                            
                                j.Vida -= temp;
                            }
                        }
                        Console.ReadKey();
                        break;
                    case "2": case "curar":
                        objeto = j.Inventario.FirstOrDefault(obj => obj.Nombre.Equals("Pócima", StringComparison.OrdinalIgnoreCase));
                        
                        if (objeto != null)
                        {
                            Console.WriteLine("¡Se ha usado una poción para restaurar la vida al máximo!");
                            j.Vida = 200;
                            
                            j.Inventario.Remove(objeto);
                        }
                        else
                        {
                            Console.WriteLine("¡No hay pócimas para restaurar vida!");
                        }
                        Console.ReadKey();
                        break;
                    case "3": case "usar dardo": case "dardo":
                        objeto = j.Inventario.FirstOrDefault(obj => obj.Nombre.Equals("Dardo", StringComparison.OrdinalIgnoreCase));
                        
                        if (objeto != null)
                        {
                            Console.Write($"¡{j.Nombre} ha aventado el dardo");
                            Thread.Sleep(1000);
                            Console.WriteLine("... pero solo hace 1 punto de daño!");
                            e.Vida -= 1;

                            j.Inventario.Remove(objeto);
                        }
                        else
                        {
                            Console.WriteLine("¡No hay pócimas para restaurar vida!");
                        }
                        
                        if (e.Vida > 0)
                        {
                            Console.WriteLine($"¡{e.Nombre} ataca!");
                            
                            objeto = j.Inventario.FirstOrDefault(obj => obj.Nombre.Equals("Escudo Mágico", StringComparison.OrdinalIgnoreCase));
                            
                            if (objeto != null)
                            {
                                Console.WriteLine("¡El escudo mágico bloqueo el daño!");
                                Console.WriteLine("¡El escudo mágico se desvaneció!");
                        
                                j.Inventario.Remove(objeto);
                            }
                            else
                            {
                                temp = Ataque(e.Fuerza, modDefensa);
                                Console.WriteLine($"{j.Nombre} recibe {temp} de daño");
                                j.Vida -= temp;
                            }
                        }
                        Console.ReadKey();
                        break;
                    case "4": case "inventario":
                        DesplegarInventario();
                        break;
                    case "5": case "examinar":
                        Examinar(e.Nombre);
                        break;
                    case "6": case "ayuda": default:
                        Console.WriteLine("***Cómo funciona***");
                        Console.WriteLine("Ingrese el número o nombre de acción, y el jugador responderá.");
                        Console.WriteLine("Las opciones 2 y 3 solo funcionan si el inventario contiene un objeto de ese tipo.");
                        Console.ReadKey();
                        break;
                }
            }
            
            if (j.Vida <= 0)
            {
                Activo = false;
            
                Console.WriteLine($"{j.Nombre} ha sido derrotado...");
                Console.WriteLine("\n Fin del juego");
            }
            else
            {
                Console.WriteLine($"¡{e.Nombre} ha sido derrotado!");
                
                if(e.Nombre == "Dragón rojo")
                {
                    Activo = false;
            
                    Console.WriteLine("¡Felicitaciones! Aquella bestia no atormentará a nadie más.");
                    Console.WriteLine($"¡{j.Nombre} ha conquistado la mazmorra!");
                }
                CuartoActual.Entidades.Remove(e);
            }
            
            Console.ReadKey();
        }
        
        public int Ataque(int fuerza, double defensa = 1)
        {
            double minFuerza = fuerza * 0.8;
            double maxFuerza = fuerza * 1.2;
            double mult = r.NextDouble() * (maxFuerza - minFuerza) + minFuerza;
            return (int)Math.Round(mult / defensa);
        }
        
        public void Examinar(string objetivo)
        {
            // Se busca la primera entidad que comparta el nombre de lo escrito
            // e representa un objeto del tipo Entidad
            // Se compara el nombre de la entidad con el string objetivo sin distinguir mayúsculas de minúsculas
            var entidad = CuartoActual.Entidades.FirstOrDefault(e => e.Nombre.Equals(objetivo, StringComparison.OrdinalIgnoreCase));
            
            // Si se encontró una entidad con el mismo nombre, se escribe su descripción
            if (entidad != null)
            {
                Console.WriteLine($"\"{entidad.Descripcion}\"");
                if(entidad is Objeto o)
                {
                    TomarObjeto(o);
                }
            }
            else
            {
                Console.WriteLine($"{objetivo} no parece estar en el cuarto...");
            }
        
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
        
        public void MoverAlJugador(string sentido)
        {
            // Si hay una salida en la dirección ingresada, se cambia de ubicación al jugador.
            if(CuartoActual.Salidas.ContainsKey(sentido))
                CuartoActual = CuartoActual.Salidas[sentido];
            else
            {
                Console.WriteLine("¡El cuarto no tiene una salida en esa dirección!");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
        
        public void CicloDeJuego()
        {
            while( Activo )
            {
                DesplegarCuartoActual();
            }
        }
    }
}