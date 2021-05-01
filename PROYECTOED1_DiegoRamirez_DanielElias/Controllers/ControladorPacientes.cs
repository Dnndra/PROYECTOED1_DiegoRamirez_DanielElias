﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Microsoft.VisualBasic.FileIO;


using PROYECTOED1_DiegoRamirez_DanielElias.Models.Data;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Controllers
{
    
    public class ControladorPacientes : Controller
    {
        //PARÁMETROS MODIFICABLES
        public static DateTime fecha = new DateTime(2021,3,10,8,0,0);
        public static int CantidadAVacunar = 3;

        public static int contador = 0;
        public static string csvPacientes = "";
        public static bool cargaInicial = false;
        public static bool calendarizado = false;
        //hosting environment
        IWebHostEnvironment hostingEnvironment;
        public ControladorPacientes(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

        }

        public void LeerTablaPacientes()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines($"{hostingEnvironment.WebRootPath}\\csv\\pacientes.csv");
                TextReader reader = new StreamReader($"{hostingEnvironment.WebRootPath}\\csv\\pacientes.csv");
                TextFieldParser csvReader = new TextFieldParser(reader);
                csvReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                csvReader.SetDelimiters(",");
                csvReader.HasFieldsEnclosedInQuotes = true;
                string[] fields;
                while (!csvReader.EndOfData)
                {
                    try
                    {
                        fields = csvReader.ReadFields();
                        var nuevoPaciente = new Paciente();
                        var nuevoPaciente2 = new Paciente();
                        var nuevoPaciente3 = new Paciente();
                        var nuevoPaciente4 = new Paciente();
                       
                        var node = new Models.Data.PriorityNode<Models.Data.Paciente>();
                        nuevoPaciente.Nombre = fields[0];
                        nuevoPaciente2.Nombre = fields[0]; 
                        nuevoPaciente.Apellido = fields[1];
                        nuevoPaciente3.Apellido = fields[1];
                        nuevoPaciente.DPI = fields[2];                     
                        nuevoPaciente4.DPI = fields[2];
                        nuevoPaciente.Departamento = fields[3];                    
                        nuevoPaciente.Municipio = fields[4];                       
                        nuevoPaciente.Profesion = fields[5];             
                        nuevoPaciente.Edad = Convert.ToInt32(fields[6]);                  
                        nuevoPaciente.FechaDeVacunacion = Convert.ToDateTime(fields[7]);
                        nuevoPaciente.Prioridad = Convert.ToInt32(fields[8]);
                        

                        nuevoPaciente.Vacunado = Convert.ToBoolean(fields[9]);
                        
                        if (nuevoPaciente.Vacunado == false)
                        {
                            Singleton.Instance.MinheapPacientes.Add(node);
                        }
                        else
                        {
                            Singleton.Instance.ListaDeVacunados.AddLast(nuevoPaciente);
                        }
                        node.Data = nuevoPaciente;
                        node.prioridad = nuevoPaciente.Prioridad;
                        Singleton.Instance.TablaHashPacientes.Add(nuevoPaciente.DPI, nuevoPaciente);

                        csvPacientes += $"{fields[0]},{fields[1]},{fields[2]},{fields[3]},{fields[4]},{fields[5]},{fields[6]},{fields[7]},{fields[8]},{fields[9]}\n";               
                        Singleton.Instance.Buscarpaciente.AddTo(nuevoPaciente4, nuevoPaciente.Nombre, nuevoPaciente.Apellido, Singleton.Instance.Buscarpaciente.Root, nuevoPaciente.DPI,""); ;
                        Singleton.Instance.BuscarNombre.AddTo(nuevoPaciente2, nuevoPaciente.Nombre, nuevoPaciente.Apellido, Singleton.Instance.BuscarNombre.Root, nuevoPaciente.DPI,"");
                        Singleton.Instance.BuscarApellido.AddTo(nuevoPaciente3, nuevoPaciente2.Nombre, nuevoPaciente3.Apellido, Singleton.Instance.BuscarApellido.Root, nuevoPaciente.DPI,"");
                     

                    }
                    catch
                    {

                    }
                }
                Singleton.Instance.MinheapPacientes.Heapify();
                reader.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        void EscribirTablaPacientes(Paciente paciente)
        {

            TextWriter writer = new StreamWriter($"{hostingEnvironment.WebRootPath}\\csv\\pacientes.csv");

            csvPacientes += $"{paciente.Nombre},{paciente.Apellido},{paciente.DPI},{paciente.Departamento},{paciente.Municipio},{paciente.Profesion},{paciente.Edad},{paciente.FechaDeVacunacion},{paciente.Prioridad},{paciente.Vacunado}\n";
            writer.Write(csvPacientes);
            writer.Close();
        }

        void ActualizarTablaPacientes()
        {
            TextWriter writer = new StreamWriter($"{hostingEnvironment.WebRootPath}\\csv\\pacientes.csv");
            csvPacientes = "";
            var listaDeEspera = Singleton.Instance.ListaDeEspera;
            var listaDeVacunados = Singleton.Instance.ListaDeVacunados;
            foreach (Paciente paciente in listaDeEspera)
            {
                csvPacientes += $"{paciente.Nombre},{paciente.Apellido},{paciente.DPI},{paciente.Departamento},{paciente.Municipio},{paciente.Profesion},{paciente.Edad},{paciente.FechaDeVacunacion},{paciente.Prioridad},{paciente.Vacunado}\n";

            }
            foreach (Paciente paciente in listaDeVacunados)
            {
                csvPacientes += $"{paciente.Nombre},{paciente.Apellido},{paciente.DPI},{paciente.Departamento},{paciente.Municipio},{paciente.Profesion},{paciente.Edad},{paciente.FechaDeVacunacion},{paciente.Prioridad},{paciente.Vacunado}\n";

            }
            writer.Write(csvPacientes);
            writer.Close();
            
        }
        // GET: ControladorPacientes
        public ActionResult Index()
        {
            if (cargaInicial == false)
            {
                LeerTablaPacientes();
                cargaInicial = true;
            }
            
            return View();
        }

        public ActionResult ListaDePacientes()
        {

            var lista = Singleton.Instance.ListaDeEspera;
            
            var heap = Singleton.Instance.MinheapPacientes;
            var hashtable = Singleton.Instance.TablaHashPacientes;

            foreach (Paciente paciente1 in lista)
            {
                lista.Remove(0);
            }

            for (int i = 0; i < heap.elements.Count; i++)
            {

                string key = heap.elements[i].Data.DPI;
                var paciente = new Models.Data.Paciente();
                paciente = hashtable.GetNode(key);
                paciente.FechaDeVacunacion = fecha;
                if (paciente.Vacunado == false)
                {
                    lista.AddLast(paciente);
                }
               

               
             
            }
          
            return View(lista);
        }

        public ActionResult ListaDeVacunados()
        {
            
            var lista = Singleton.Instance.ListaDeVacunados;
         

            return View(lista);
        }
        public ActionResult BuscarPersona(string dev)
        {
            ViewData["GetDev"] = dev;
            var lista = Singleton.Instance.ListaDeEspera;
            var paciente1 = new Models.Data.Paciente();
            var paciente3 = new Models.Data.Paciente();
            var paciente4 = new Models.Data.Paciente();
           
 
            paciente1.DPI = dev;
            paciente3.Apellido = dev;
            paciente4.Nombre = dev;
            Paciente selectedDPI;
            Paciente selectedName;

        

            if (!string.IsNullOrEmpty(dev))

            {
                foreach (Paciente paciente5 in lista)
                {
                    lista.Remove(0);
                }
                //búsqueda por DPI
                var selectedbuscadordpi= Singleton.Instance.Buscarpaciente.find(paciente1, "-1", Singleton.Instance.Buscarpaciente.Root );
                if (selectedbuscadordpi.DPI != "-1")
                {
                
                        selectedDPI = Singleton.Instance.TablaHashPacientes.GetNode(selectedbuscadordpi.DPI);
                        lista.AddLast(selectedDPI);

                    return View(lista);
                 
                }
                // búsqueda por apellido

                var selectedbuscadorapellido = Singleton.Instance.BuscarApellido.find(paciente3, "-1", Singleton.Instance.BuscarApellido.Root);
                if (selectedbuscadorapellido.DPI != "-1")
                {
                    selectedName = Singleton.Instance.TablaHashPacientes.GetNode(selectedbuscadorapellido.DPI);

                    lista.AddLast(selectedName);
                    return View(lista);
                }
                    //busquedapor  nombre
                var selectedbuscadornombre = Singleton.Instance.BuscarNombre.find(paciente4, "-1", Singleton.Instance.BuscarNombre.Root);
                if (selectedbuscadornombre.DPI != "-1")
                {
                    selectedName = Singleton.Instance.TablaHashPacientes.GetNode(selectedbuscadornombre.DPI);

                    lista.AddLast(selectedName);
                    return View(lista);
                }
          

            }
            return View(lista);
        }
        public ActionResult CalendarizarVacunacion()
        {
            csvPacientes = "";
            var lista = Singleton.Instance.ListaDeEspera;
            foreach (Paciente paciente1 in lista)
            {
                lista.Remove(0);
            }

            var heap = Singleton.Instance.MinheapPacientes;
            var hashtable = Singleton.Instance.TablaHashPacientes;
            int contadorPacientes = 0;
            for (int i = 0; i < heap.elements.Count; i++)
            {
                string key = heap.elements[i].Data.DPI;
                var paciente = new Models.Data.Paciente();
                paciente = hashtable.GetNode(key);
                paciente.FechaDeVacunacion = fecha;
                contadorPacientes++;

                if (contadorPacientes == CantidadAVacunar)
                {
                    fecha = fecha.AddDays(1);
                    contadorPacientes = 0;
                }
                lista.AddLast(paciente);
                EscribirTablaPacientes(paciente);
            }

            calendarizado = true;



            return View(lista);
        }

        public ActionResult AgregarPaciente()
        {
            
          


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPaciente(IFormCollection collection)
        {

            try
            {

                var paciente = new Models.Data.Paciente();
                var paciente2 = new Models.Data.Paciente();
                var paciente3 = new Models.Data.Paciente();
                var paciente4 = new Models.Data.Paciente();
                var node = new Models.Data.PriorityNode<Models.Data.Paciente>();
                {

                    paciente.Nombre = collection["Nombre"];
                    paciente2.Nombre = collection["Nombre"];
                    paciente.Apellido = collection["Apellido"];
                    paciente3.Apellido = collection["Apellido"];
                    paciente.DPI = collection["DPI"];
                    paciente4.DPI = collection["DPI"];
                    paciente.Departamento = collection["Departamento"];
                    paciente.Municipio = collection["Municipio"];
                    paciente.Edad = Convert.ToInt32(collection["Edad"]);
                    paciente.Profesion = collection["Profesion"];
                    node.Data = paciente;
                    node.prioridad = calcularprioridad(paciente.Profesion, paciente.Edad);
                    paciente.Prioridad = node.prioridad;
               
                }
                if (Singleton.Instance.TablaHashPacientes.GetNodeByKey(paciente.DPI) != (null,null))
                {
                    MostrarDialogo("El DPI ingresado no es válido");
                    return View();
                }
                else
                {
                    Singleton.Instance.TablaHashPacientes.Add(paciente.DPI, paciente);
                    Singleton.Instance.MinheapPacientes.Add(node);
                    Singleton.Instance.Buscarpaciente.AddTo(paciente4,paciente.Nombre, paciente.Apellido, Singleton.Instance.Buscarpaciente.Root, paciente.DPI, "");
                    Singleton.Instance.BuscarNombre.AddTo(paciente2, paciente2.Nombre, paciente.Apellido, Singleton.Instance.BuscarNombre.Root, paciente.DPI, "");
                    Singleton.Instance.BuscarApellido.AddTo(paciente3, paciente2.Nombre, paciente3.Apellido, Singleton.Instance.BuscarApellido.Root, paciente.DPI, "");
                    EscribirTablaPacientes(paciente);
                    calendarizado = false;


                    return RedirectToAction(nameof(Index));

                }


            }
            catch
            {
                return View();
            }

        }

        public ActionResult VacunarPersonas()
        {
            if (calendarizado == false)
            {
                MostrarDialogo("Debe calendarizar la vacunación de primero");
                calendarizado = true;
                return RedirectToAction(nameof(Index));
            }
            var heap = Singleton.Instance.MinheapPacientes;
            var listaDeEspera = Singleton.Instance.ListaDeEspera;
            var listaDeVacunados = Singleton.Instance.ListaDeVacunados;
            var node = new PriorityNode<Paciente>();
            foreach (Paciente paciente1 in listaDeEspera)
            {
                listaDeEspera.Remove(0);
            }

            for (int i = 0; i < heap.elements.Count; i++)
            {

                var paciente = heap.elements[i].Data;
                listaDeEspera.AddLast(paciente);

            }

            try
            {
                for (int i = 0; i < CantidadAVacunar; i++)
                {
                    var temp = listaDeEspera.ElementAt(0);


                    if (probabilidadDeAusencia() == 1)
                    {
                        //CASO SI EL PACIENTE NO SE PRESENTA A LA VACUNACION 
                        temp.FechaDeVacunacion = fecha;
                        temp.Prioridad = 12;
                        heap.elements[0].prioridad = 12;
                        heap.listPriority();
                       // MostrarDialogo("Se ha reagendado la vacunación para " + temp.Nombre + " " + temp.Apellido + " para el dia " + temp.FechaDeVacunacion + "debido a que no se presentó");

                        foreach (Paciente paciente1 in listaDeEspera)
                        {
                            listaDeEspera.Remove(0);
                        }

                        for (int j = 0; j < heap.elements.Count; j++)
                        {

                            var paciente = heap.elements[j].Data;
                            listaDeEspera.AddLast(paciente);

                        }

                    }
                    else
                    {
                        //EL PACIENTE SI SE PRESENTA A LA VACUNACION
                        int index = 0;
                        listaDeEspera.Remove(0);
                        temp.Vacunado = true;
                        listaDeVacunados.AddLast(temp);
                        Singleton.Instance.TablaHashPacientes.GetNode(temp.DPI).Vacunado = true;

                        if (heap.GetNode(0).Data == temp) {
                            heap.PopMin();
                        }
                        else
                        {
                            while (heap.GetNode(index).Data != temp)
                            {
                                index++;
                            }
                            heap.Delete(heap.GetNode(index));
                        }
                      
                        
                    }

                }
                ActualizarTablaPacientes();
            }
            catch
            {
                return RedirectToAction(nameof(ListaDeVacunados));
            }


            return RedirectToAction(nameof(ListaDeVacunados));
        }

     
        //METODOS

        public void MostrarDialogo(string message)
        {
            TempData["alertMessage"] = message;
            
        }

        public int probabilidadDeAusencia()
        {
            //ESTE METODO SIMULA LA PROBABILIDAD DE QUE UN PACIENTE NO SE PRESENTE 
            //ES DE PARAMETROS AJUSTABLES
            Random random = new Random();
            return random.Next(1, 25);
        }

        public ActionResult porcentajeDeVacunados()
        {
            var listaDePacientes = Singleton.Instance.ListaDeEspera.Count();
            var listaDeVacunados = Singleton.Instance.ListaDeVacunados.Count();
            double total = listaDeVacunados + listaDePacientes;
            double porcentaje;

            porcentaje = (listaDeVacunados / total) * 100;
            MostrarDialogo("El porcentaje de vacunados es: " + Math.Round(porcentaje,2) + "%");

            return RedirectToAction(nameof(Index));
        }
        public int calcularprioridad(string  profesion, int Edad)
        {
            if (profesion == "Trabajador de salud")
            {
                return 1;
            }
            else if(profesion == "Estudiantes de salud" ) {
                return 2; 

            }
            else if  (profesion == "Cuerpos de socorro")
            {
                return 3;
            }
            else if (profesion == "Trabajador de funeraria")
            {
                return 3; 
            }
            else if(profesion == "Cuidador de adultos mayores") {
                return 3; 
            }
            else if (profesion == "Seguridad nacional" && Edad < 50)
            {
                return 6; 
            }
            else if (profesion == "Trabajador de municipalidad" && Edad < 50)
            {
                return 7; 
            }
            else if (profesion == "Trabajador del sector educación" && Edad < 50 )
            {
                return 8; 
            }
            else if (profesion == "Trabajador del sector justicia" && Edad < 50)
            {
                return 9; 
            }
            else
            {
                if (Edad >= 70)
                {
                    return 4; 
                }
                else  if  (Edad >= 50 && Edad <= 69)
                {
                    return 5; 
                }
                else if (Edad >= 40 && Edad <= 49)
                {
                    return 10;
                }
                else if  (Edad >=1 && Edad <= 39)
                {
                    return 11;
                }
            }
            return 0; 
        }

      

    }
}

