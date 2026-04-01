using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_U2_Proyecto
{
    public class Jugador
    {
        public string Nombre { get; set; }
        public int Vida { get; set; }
        public int Fuerza { get; set; }
        public List<Objeto> Inventario { get; } = new List<Objeto>();
        
        public Jugador(string nombre, int vida, int fuerza)
        {
            Nombre = nombre;
            Vida = vida;
            Fuerza = fuerza;
        }
        
        public void AgarrarObjeto(Objeto o)
        {
            Console.WriteLine($"Objeto adquirido: {o.Nombre}");
            Inventario.Add(o);
        }
    }
}
