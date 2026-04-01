using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDS_U2_Proyecto
{
    public enum Tipo
    {
        Objeto,
        Enemigo,
        Personaje
    }
    public class Fab_Entidad
    {
        // Creadora
        public static Entidad CreadorEntidad(Tipo tipo, string nombre)
        {
            switch (tipo)
            {
                case Tipo.Objeto:
                    return CrearObjeto(nombre);
                case Tipo.Enemigo:
                    return CrearEnemigo(nombre);
                case Tipo.Personaje:
                    return CrearPersonaje(nombre);
                default:
                    throw new ArgumentException("Entidad no válida", nameof(tipo));
                      
            }
        }
        // Fábricas concretas
        public static Entidad CrearObjeto(string nombre)
        {
            switch(nombre.ToLower())
            {
                case "espada":
                    return new Objeto("Espada", "Una espada que, aunque parezca mágica, sigue siendo una espada normal.",
                        true, 15);
                case "pocima":
                    return new Objeto("Pócima", "Esta pócima tiene la capacidad de restaurar a alguien al máximo " +
                        "aunque esté en lecho de muerte.", true, 20);
                case "dardo":
                    return new Objeto("Dardo", "Un proyectil tan sencillo de entender que hasta un mono puede usarlo",
                        true, 5);
                case "escudo":
                    return new Objeto("Escudo", "Este escudo brinda protección al usuario.", true, 30);
                case "superescudo":
                    return new Objeto("Escudo mágico", "Este escudo protege al usuario de cualquier ataque, pero solo una vez.",
                        true, 50);
                case "cofre":
                    return new Objeto("Cofre", "Un cofre de apariencia dañada. ¿Qué contendrá?", false, 100);
                default:
                    return new Objeto(nombre, "¿Qué es esto? No parece nada conocido.", false, 1);
            }
        }
        
        public static Entidad CrearEnemigo(string nombre)
        {
            switch (nombre)
            {
                case "limo":
                    return new Enemigo("Limo", "Una criatura pequeña de apariencia gelatinosa, parece amistosa.",
                        30, 10);
                case "murcielago":
                    return new Enemigo("Murciélago", "Un monstruo con colmillos y alas. Muerde en su primera oportunidad.",
                        50, 15);
                case "armadura":
                    return new Enemigo("Armadura viviente", "Armadura que cobró vida propia de alguna manera.",
                        75, 20);
                case "dragon":
                    return new Enemigo("Dragón", "Un reptil verde y enorme que puede escupir fuego. Es veloz y feroz.",
                        150, 40);
                case "superdragon":
                    return new Enemigo("Dragón rojo", "¡Un dragón aún más fuerte! No descansa hasta acabar con su presa",
                        250, 75);
                default:
                    return new Enemigo(nombre, "Un ser de aspecto misterioso.",
                        10, 5);
            }
        }
        
        public static Entidad CrearPersonaje(string nombre)
        {
            switch (nombre)
            {
                case "mercader":
                    return new Personaje("Mercader", "Una persona con una mochila llena de artículos",
                        "¡Hola, viajero! Tome el tiempo de revisar mis productos");
                case "viejo":
                    return new Personaje("Viejo", "Una persona de edad avanzada. ¿Qué hace aquí?",
                        "Ten cuidado, este lugar es peligroso");
                case "cadaver":
                    return new Personaje("Cadáver", "Una persona muerta", "No hay respuesta. Es solo un cadáver");
                case "pepe":
                    return new Personaje("Pepe", "José Luis Morales Hernández", "¿Y luego?");
                default:
                    return new Personaje(nombre, "Una persona desconocida.", "Yo no debería estar aquí jejeje.");
            }
        }
    }
}
