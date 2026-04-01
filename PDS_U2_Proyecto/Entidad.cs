using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_U2_Proyecto
{
    // Producto
    public abstract class Entidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public Entidad(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }
    }
    // Productos concretos
    public class Objeto : Entidad
    {
        public bool Agarrable { get; set; }
        public int Coste { get; set; }
        
        // Añadir : base(args) indica que se está llamando al constructor de la clase padre
        // Como los tipos de entidad comparten ciertas propiedades, permite ahorrar unas cuantas líneas de código
        public Objeto(string nombre, string descripcion, bool agarrable, int coste) : base(nombre, descripcion)
        {
            Agarrable = agarrable;
            Coste = coste;
        }
    }
    
    public class Enemigo : Entidad
    {
        public int Vida { get; set; }
        public int Fuerza { get; set; }
        public Enemigo(string nombre, string descripcion, int vida, int fuerza) : base(nombre, descripcion)
        {
            Vida = vida;
            Fuerza = fuerza;
        }
    }
    
    public class Personaje : Entidad
    {
        public string Texto { get; set; }        
        public Personaje(string nombre, string descripcion, string texto) : base(nombre, descripcion)
        {
            Texto = texto;
        }
    }
}
