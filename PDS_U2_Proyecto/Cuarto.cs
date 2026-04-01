using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_U2_Proyecto
{
    public class Cuarto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        // Permitirá adjutar cuartos a otros cuartos y su desplazamiento
        public Dictionary<string, Cuarto> Salidas { get; } = new Dictionary<string, Cuarto>();
        // Indica objetos, enemigos y/o personajes en el cuarto
        public List<Entidad> Entidades { get; } = new List<Entidad>();
        
        public Cuarto(string nombre, string descripcion) 
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }
        
        public void AgregarSalida(string sentido, Cuarto cuarto)
        {
            //Salidas.Add(sentido, cuarto);
            // También se puede:
            Salidas[sentido] = cuarto;
            Console.WriteLine($"Nueva salida: {Nombre}-->{sentido}-->{cuarto.Nombre} {cuarto.GetHashCode()}");
        }
        
        public void AgregarEntidad(Entidad entidad)
        {
            // Se agrega una entidad al cuarto.
            Entidades.Add(entidad);
        }
    }
    // Constructor de cuartos
    public class CuartoBuilder
    {
        private readonly Cuarto _cuarto;
        
        public CuartoBuilder(string nombre, string descripcion)
        {
            _cuarto = new Cuarto(nombre,descripcion);
            Console.WriteLine($"Cuarto creado: {nombre}. Código hash del cuarto: {_cuarto.GetHashCode()}");
        }
        
        public CuartoBuilder AgregarEntidad(Entidad entidad)
        {
            _cuarto.AgregarEntidad(entidad);
            /* Permite encadenar funciones
             * Algo como
             * constructor = new CuartoBuilder("Algo", "Aquí)
             * .AddEntity(algo)
             * .AddEntity(aquí)
             * .Construir()
            */
            return this;
        }
        
        public Cuarto Construir()
        {
            Console.WriteLine($"Construyendo el cuarto con hash {_cuarto.GetHashCode()}");
            return _cuarto;
        }
    }
}
