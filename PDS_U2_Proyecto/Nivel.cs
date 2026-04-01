using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_U2_Proyecto
{
    public class Nivel
    {
        public string Nombre { get; }
        public Cuarto CuartoInicial { get; }
        private List<Cuarto> _cuartos = new List<Cuarto>();
        public Nivel(string nombre, Cuarto cuartoInicial)
        {
            // Se asigna la posición inicial del jugador, y el nombre del nivel
            Nombre = nombre;
            CuartoInicial = cuartoInicial;
        }
        
        public void AgregarCuarto(Cuarto cuarto)
        {
            // Si el cuarto no existe en el nivel, se añade a este
            if (!_cuartos.Contains(cuarto))
            {
                _cuartos.Add(cuarto);
            }
        }
    }
    public class NivelBuilder
    {
        private readonly string _nombre;
        private Cuarto _cuartoInicial;
        private readonly List<Cuarto> _cuartos = new List<Cuarto>();
        // Por cada cuarto, se le asignará las salidas
        private readonly Dictionary<Cuarto, Dictionary<string, Cuarto>> _salidas = new Dictionary<Cuarto, Dictionary<string, Cuarto>>();
        
        public NivelBuilder(string nombre = "Cuarto extraño")
        {
            _nombre = nombre;
        }
        
        public CuartoBuilder AgregarCuarto(string nombre, string descripcion = null)
        {
            descripcion = descripcion ?? $"Es el cuarto {nombre}.";

            var constructor = new CuartoBuilder(nombre, descripcion);
            
            return constructor;
        }
        
        public NivelBuilder AgregarConexion(Cuarto origen, string sentido, Cuarto destino)
        {
            // Si no existía, se añade una entrada para las conexiones del cuarto
            if (!_salidas.ContainsKey(origen))
            {
                _salidas[origen] = new Dictionary<string, Cuarto>();
                Console.WriteLine($"Conexión creada para {origen.Nombre}");
            }
            
            // Se incorpora el cuarto de destino al cuarto origen, y en su dirección especificada
            // Requiere dos llaves porque el diccionario tiene un diccionario como valor
            _salidas[origen][sentido] = destino;
            Console.WriteLine($"Nueva conexión: {origen.Nombre}--{sentido}-->{destino.Nombre} {destino.GetHashCode()}");
            return this;
        }

        public NivelBuilder AgregarConstructor(Cuarto cuarto)
        {
            _cuartos.Add(cuarto);
            if (_cuartoInicial == null)
            {
                _cuartoInicial = cuarto;
            }

            if (_salidas.ContainsKey(cuarto))
            {
                _salidas[cuarto] = new Dictionary<string, Cuarto>();
            }

            return this;
        }

        public Nivel Construir()
        {
            Console.WriteLine("Creando el nivel...");
            
            // Configuración del cuarto
            foreach (var puerta in _salidas)
            {
                Cuarto cuarto = puerta.Key;
                Console.WriteLine($"Procesando salidas para {cuarto.Nombre} {cuarto.GetHashCode()}");
                
                foreach (var salida in puerta.Value)
                {
                    Console.WriteLine($"Nueva salida creada: {salida.Key}-->{salida.Value.Nombre} {salida.GetHashCode()}");
                    cuarto.AgregarSalida(salida.Key, salida.Value);
                }
            }

            if (_cuartoInicial != null)
            {
                Console.WriteLine($"Cuarto inicial: {_cuartoInicial.Nombre} con código Hash {_cuartoInicial.GetHashCode()}");
                Console.WriteLine("Salidas: ");
                
                foreach(var salida in _cuartoInicial.Salidas)
                {
                    Console.WriteLine($"- {salida.Key} -> {salida.Value.Nombre}");
                }
            }
            else
            {
                Console.WriteLine("El cuarto inicial está vacío");
            }
            
            return new Nivel(_nombre, _cuartoInicial);
        }
    }
}
