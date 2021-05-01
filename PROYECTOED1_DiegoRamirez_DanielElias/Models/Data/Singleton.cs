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
        public MinHeap<Paciente> MinheapPacientes;
        public AVLTree<Paciente> Buscarpaciente;
        public Manual_List<Paciente> ListaDeEspera;
        public Manual_List<Paciente> ListaDeVacunados;
        public AVLTree<Paciente> BuscarNombre;
        public AVLTree<Paciente> BuscarApellido;

        
        private Singleton()
        {
            ListaDeEspera = new Manual_List<Paciente>();
            TablaHashPacientes = new HashTable<Paciente>(10);
            MinheapPacientes = new MinHeap<Paciente>();
            Buscarpaciente = new AVLTree<Paciente>();
            BuscarNombre = new AVLTree<Paciente>();
            BuscarApellido = new AVLTree<Paciente>(); 
        }


        public static Singleton Instance
        {
            get { return _instance; }
        }
    }
}
