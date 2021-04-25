using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();

        public HashTable<Paciente> TablaHashPacientes;
        private Singleton()
        {
            TablaHashPacientes = new HashTable<Paciente>(10);
        }


        public static Singleton Instance
        {
            get { return _instance; }
        }
    }
}
