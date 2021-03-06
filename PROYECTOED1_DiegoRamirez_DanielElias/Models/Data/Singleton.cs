using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibreriaProyecto;
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
        public Manual_List<String> muncipios;
        public Manual_List<Paciente> Listabuscar;  
        public Manual_List<Paciente> ListaAuxiliar;
        


        private Singleton()
        {
            ListaDeEspera = new Manual_List<Paciente>();
            ListaDeVacunados = new Manual_List<Paciente>();
            TablaHashPacientes = new HashTable<Paciente>(10); 
            MinheapPacientes = new MinHeap<Paciente>();
            Buscarpaciente = new AVLTree<Paciente>();
            BuscarNombre = new AVLTree<Paciente>();
            BuscarApellido = new AVLTree<Paciente>();
            muncipios = new Manual_List<String>();
            Listabuscar = new Manual_List<Paciente>();
            ListaAuxiliar = new Manual_List<Paciente>();
        }


        public static Singleton Instance
        {
            get { return _instance; }
        }
    }
}
