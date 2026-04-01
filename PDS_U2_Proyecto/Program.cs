namespace PDS_U2_Proyecto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MotorJuego motocicleta = MotorJuego.instance;
            Random r = new Random();
            string[] listaObj = { "espada", "pocima", "dardo", "escudo", "superescudo" };
            string[] listaPer = { "pepe", "viejo", "cadaver" };
            string[] listaEne = { "murcielago", "armadura" };

            var nivelC = new NivelBuilder("Piso de prueba");

            // Gracias al comportamiento de la clase Cuarto, es automáticamente el piso inicial
            var entrada = nivelC.AgregarCuarto("Entrada", "Un cuarto con una puera enorme para entrar").Construir();
            entrada.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "espada"));
            entrada.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "escudo"));
            entrada.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "pocima"));
            entrada.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Personaje, listaPer[r.Next(listaPer.Length)]));

            var pasillo = nivelC.AgregarCuarto("Pasillo", "Un pasillo angosto, con pocas puertas.").Construir();
            pasillo.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Enemigo, "limo"));
            pasillo.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Personaje, "cadaver"));

            var tesoro = nivelC.AgregarCuarto("Sala del tesoro", "Un area decorada con oro y varios cofres").Construir();
            tesoro.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "cofre"));
            tesoro.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "cofre"));
            tesoro.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "cofre"));
            tesoro.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "escudo"));

            var cuarto = nivelC.AgregarCuarto("Cuarto", "Un área simple con pocas cosas de interés").Construir();
            cuarto.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Enemigo, listaEne[r.Next(listaEne.Length)]));
            cuarto.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Enemigo, listaEne[r.Next(listaEne.Length)]));
            cuarto.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "pocima"));

            var tienda = nivelC.AgregarCuarto("Tienda", "Una región de la mazmorra con un mercader... pero todo es gratis").Construir();
            tienda.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Personaje, "mercader"));
            tienda.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, listaObj[r.Next(listaObj.Length)]));
            tienda.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, listaObj[r.Next(listaObj.Length)]));
            tienda.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "escudo"));

            var cuarto2 = nivelC.AgregarCuarto("Cuarto", "Un área simple con pocas cosas de interés").Construir();
            cuarto2.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Enemigo, "dragon"));
            cuarto2.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Objeto, "espada"));
            cuarto2.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Personaje, "cadaver"));

            var guarida = nivelC.AgregarCuarto("Guarida", "Lugar donde deambula un gran dragón").Construir();
            guarida.AgregarEntidad(Fab_Entidad.CreadorEntidad(Tipo.Enemigo, "superdragon"));

            nivelC.AgregarConstructor(entrada);
            nivelC.AgregarConstructor(pasillo);
            nivelC.AgregarConstructor(tesoro);
            nivelC.AgregarConstructor(cuarto);
            nivelC.AgregarConstructor(tienda);
            nivelC.AgregarConstructor(cuarto2);
            nivelC.AgregarConstructor(guarida);

            nivelC.AgregarConexion(entrada, "norte", pasillo);
            nivelC.AgregarConexion(pasillo, "sur", entrada);
            nivelC.AgregarConexion(pasillo, "oeste", tesoro);
            nivelC.AgregarConexion(pasillo, "norte", cuarto);
            nivelC.AgregarConexion(tesoro, "este", pasillo);
            nivelC.AgregarConexion(cuarto, "sur", pasillo);
            nivelC.AgregarConexion(cuarto, "este", tienda);
            nivelC.AgregarConexion(cuarto, "norte", cuarto2);
            nivelC.AgregarConexion(tienda, "oeste", cuarto);
            nivelC.AgregarConexion(cuarto2, "sur", cuarto);
            nivelC.AgregarConexion(cuarto2, "norte", guarida);
            nivelC.AgregarConexion(guarida, "sur", cuarto2);

            motocicleta.CargarNivel(nivelC.Construir());

            Console.Clear();
            Console.Write("Ingrese su nombre: ");
            string nombre = Console.ReadLine();
            motocicleta.CrearJugador(nombre);
            
            motocicleta.IniciarJuego();
        }
    }
}