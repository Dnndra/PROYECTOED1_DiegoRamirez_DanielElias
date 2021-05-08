using Microsoft.AspNetCore.Http;
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
using System.Text;
using LibreriaProyecto;
namespace PROYECTOED1_DiegoRamirez_DanielElias.Controllers
{
    
    public class ControladorPacientes : Controller
    {
        //PARÁMETROS MODIFICABLES
        public static DateTime fecha = new DateTime(2021,3,10,8,0,0);
        public static int CantidadAVacunar = 3;
        public static int contador = 0;
        public static string municipiologgeado = "";
        public static string csvPacientes = "";
        public static string csvMunicipios = "";
        public static bool cargaInicial = false;
        public static bool calendarizado = false;
        public static bool dialogoMostrado = false;
        public static bool reloggeado = false;
        public static bool relog = false;
        //hosting environment
        IWebHostEnvironment hostingEnvironment;
        public ControladorPacientes(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

        }
        [HttpGet]
        //metodo para cambiar de  municipio
        public ActionResult ReLoggear()
        {
   

            
            reloggeado = true;
            return RedirectToAction(nameof(LoginMunicipio));
            
        }
        //metodo para seleccionar municipio
        public ActionResult LoginMunicipio()
        {
            leerMunicipios();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginMunicipio(IFormCollection collection)
        {
            municipiologgeado = "";
            municipiologgeado = collection["Municipio"];

            if (reloggeado == true)
            {
                ActualizarTablaPacientes();
                LeerTablaPacientes();
                
            }
            
            return RedirectToAction(nameof(Index));
        }
        //MÉTODO PARA CARGAR LOS MUNICIPIOS
        public void leerMunicipios()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines($"{hostingEnvironment.WebRootPath}\\csv\\Municipios.csv");
                TextReader reader = new StreamReader($"{hostingEnvironment.WebRootPath}\\csv\\Municipios.csv" );
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

                        nuevoPaciente.Municipio = fields[0];



                        csvMunicipios += $"{fields[0]}";



                        Singleton.Instance.muncipios.AddLast(nuevoPaciente.Municipio);
                    }
                    catch
                    {

                    }
                }
          
                reader.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //METODO PARA CARGAR LAS ESTRUCTURAS CON LOS DATOS DEL CSV
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
                        //PACIENTE CON TODOS LOS  DATOS
                        var nuevoPaciente = new Paciente();
                        //PACIENTE SOLO CON NOMBRE
                        var nuevoPaciente2 = new Paciente();
                        //PACIENTE  SOLO CON APELLIDO
                        var nuevoPaciente3 = new Paciente();
                        //PACIENTE  SOLO CON DPI 
                        var nuevoPaciente4 = new Paciente();
                       
                
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
                   
                        //COMPRUEBA SI ESTA VACUNADO Y COMPRUEBA  MUNICIPIO
                        if (nuevoPaciente.Vacunado == false && nuevoPaciente.Municipio == municipiologgeado)
                        {

                            Singleton.Instance.MinheapPacientes.Add(nuevoPaciente);
                            Singleton.Instance.ListaDeEspera.AddLast(nuevoPaciente);

                        }
                        else if(nuevoPaciente.Vacunado == true && nuevoPaciente.Municipio == municipiologgeado)
                        {
                            Singleton.Instance.ListaDeVacunados.AddLast(nuevoPaciente);
                        }
                        else
                        {
                            Singleton.Instance.ListaAuxiliar.AddLast(nuevoPaciente);
                        }

                        if (reloggeado == true)
                        {
                            reloggeado = false;
                           
                        }
                        //agrega a la hashtable  y  a los arboles
                        if (relog == false)
                        {

                            Singleton.Instance.TablaHashPacientes.Add(nuevoPaciente.DPI, nuevoPaciente);
                            Singleton.Instance.Buscarpaciente.AddTo(nuevoPaciente4,  Singleton.Instance.Buscarpaciente.Root, nuevoPaciente.DPI);
                            Singleton.Instance.BuscarNombre.AddTo(nuevoPaciente2, Singleton.Instance.BuscarNombre.Root, nuevoPaciente.DPI);
                            Singleton.Instance.BuscarApellido.AddTo(nuevoPaciente3,  Singleton.Instance.BuscarApellido.Root, nuevoPaciente.DPI);
                        }






                        csvPacientes += $"{fields[0]},{fields[1]},{fields[2]},{fields[3]},{fields[4]},{fields[5]},{fields[6]},{fields[7]},{fields[8]},{fields[9]}\n";               
                  
                     

                    }
                    catch
                    {

                    }
                }
                relog = true;
                Singleton.Instance.MinheapPacientes.Heapify();
                reader.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //METODO QUE ESCRIBE EN EL CSV
        void EscribirTablaPacientes(Paciente paciente)
        {

            TextWriter writer = new StreamWriter($"{hostingEnvironment.WebRootPath}\\csv\\pacientes.csv");

            csvPacientes += $"{paciente.Nombre},{paciente.Apellido},{paciente.DPI},{paciente.Departamento},{paciente.Municipio},{paciente.Profesion},{paciente.Edad},{paciente.FechaDeVacunacion},{paciente.Prioridad},{paciente.Vacunado}\n";
            writer.Write(csvPacientes);
            writer.Close();
        }
        //METODO PARA REINICIAR  ESTRUCTURAS
        void ActualizarTablaPacientes()
        {
            
            csvPacientes = "";
            var listaDeEspera = Singleton.Instance.ListaDeEspera;
            var listaDeVacunados = Singleton.Instance.ListaDeVacunados;
            var listaAuxiliar = Singleton.Instance.ListaAuxiliar;
            var tabla = Singleton.Instance.TablaHashPacientes;
            var heap = Singleton.Instance.MinheapPacientes;
        
            foreach (Paciente elemento in listaDeEspera)
            {
                var paciente = tabla.GetNode(elemento.DPI);
                EscribirTablaPacientes(paciente);
                listaDeEspera.Remove(0);

            }
            foreach (Paciente elemento in listaDeVacunados)
            {
                var paciente = tabla.GetNode(elemento.DPI);
                EscribirTablaPacientes(paciente);
                listaDeVacunados.Remove(0);

            }

            foreach (Paciente elemento in listaAuxiliar)
            {
                var paciente = tabla.GetNode(elemento.DPI);
                EscribirTablaPacientes(paciente);
                listaAuxiliar.Remove(0);

            }

            int contador = heap.elementos.Count();
            for (int i = 0; i< contador; i++)
            {
                heap.elementos.Remove(0);
            }

            


          
            
        }
       
        //VISTA DEL MENU
        public ActionResult Index()
        {
            //CARGA DE  PACIENTES DESDE ARCHIVO
            if (cargaInicial == false)
            {
                LeerTablaPacientes();
                leerMunicipios();
               
                cargaInicial = true;
            }
  
            return View();
        }
        //VISTA LISTA DE  PACIENTES NO VACUNADOS
        public ActionResult ListaDePacientes()
        {

            var lista = Singleton.Instance.ListaDeEspera; 
            var heap = Singleton.Instance.MinheapPacientes;
            var hashtable = Singleton.Instance.TablaHashPacientes;

            foreach (Paciente paciente1 in lista)
            {
                lista.Remove(0);
            }



            for (int i = 0; i < heap.elementos.Count(); i++)
            {

                string key = heap.elementos.Nodeatposition(i).Data.DPI;
                var paciente = new Models.Data.Paciente();
                paciente = hashtable.GetNode(key);
                paciente.FechaDeVacunacion = fecha;
                if (paciente.Vacunado == false && paciente.Municipio == municipiologgeado)
                {
                    lista.AddLast(paciente);
                }
               

               
             
            }

     

          
            return View(heap.elementos);
        }
        //VISTA LISTA DE PACIENTES DE  VACUNADOS
        public ActionResult ListaDeVacunados()
        {
            
            var lista = Singleton.Instance.ListaDeVacunados;
         

            return View(lista);
        }
        //VISTA PARA LA  BUSQUEDA 
        public ActionResult BuscarPersona(string Buscado)
        {
            ViewData["GetDev"] = Buscado;
            var lista = Singleton.Instance.Listabuscar;
            var pacienteDpi = new Models.Data.Paciente();
            var pacienteapellido = new Models.Data.Paciente();
            var pacientenombre = new Models.Data.Paciente();
           
 
            pacienteDpi.DPI = Buscado;
            pacienteapellido.Apellido = Buscado;
            pacientenombre.Nombre = Buscado;
            Paciente selectedDPI;
            Paciente selectedName;
            Paciente selectApellido;

            foreach (Paciente paciente5 in lista)
            {
                lista.Remove(0);
            }
            if (!string.IsNullOrEmpty(Buscado))

            {
             
                //búsqueda por DPI
                var selectedbuscadordpi= Singleton.Instance.Buscarpaciente.find(pacienteDpi, "-1", Singleton.Instance.Buscarpaciente.Root );
                if (selectedbuscadordpi.DPI != "-1")
                {
              
                        selectedDPI = Singleton.Instance.TablaHashPacientes.GetNode(selectedbuscadordpi.DPI);
                    if (selectedDPI.Municipio == municipiologgeado)
                    {
                        lista.AddLast(selectedDPI);
                    }

                    return View(lista);
                 
                }
                // búsqueda por apellido

                var selectedbuscadorapellido = Singleton.Instance.BuscarApellido.find(pacienteapellido, "-1", Singleton.Instance.BuscarApellido.Root);
                if (selectedbuscadorapellido.DPI != "-1")
                {
                    foreach (string dpi in selectedbuscadorapellido.Treelist)  
                    {
                        
                        selectApellido = Singleton.Instance.TablaHashPacientes.GetNode(dpi);
                        if (selectApellido.Municipio == municipiologgeado)
                        {
                            lista.AddLast(selectApellido);
                        }
                    }
                   
                    return View(lista);
                }
                    //busqueda por  nombre
                var selectedbuscadornombre = Singleton.Instance.BuscarNombre.find(pacientenombre, "-1", Singleton.Instance.BuscarNombre.Root);
                if (selectedbuscadornombre.DPI != "-1")
                {
                    foreach (string dpi in selectedbuscadornombre.Treelist)
                    {
                        selectedName = Singleton.Instance.TablaHashPacientes.GetNode(dpi);
                        if (selectedName.Municipio == municipiologgeado)
                        {
                            lista.AddLast(selectedName);
                        }
                    }
                  
                    return View(lista);
                }
          

            }
            return View(lista);
        }
        //VISTA PARA ASIGNAR FECHA  DE VACUNACION
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
            for (int i = 0; i < heap.elementos.Count(); i++)
            {
                string key = heap.elementos.Nodeatposition(i).Data.DPI;
                var paciente = new Models.Data.Paciente();
                paciente = hashtable.GetNode(key);
                paciente.FechaDeVacunacion = fecha;
                contadorPacientes++;
                //COMPRUEBA LA CANTIDAD DE  VACUNADOS AL MISMO TIEMPO (MODIFICABLE)
                if (contadorPacientes == CantidadAVacunar)
                {
                    fecha = fecha.AddDays(1);
                    contadorPacientes = 0;
                }
                lista.AddLast(paciente);
                EscribirTablaPacientes(paciente);
            }
            foreach (Paciente paciente in Singleton.Instance.ListaAuxiliar)
            {
                EscribirTablaPacientes(paciente);
            }
            foreach (Paciente paciente in Singleton.Instance.ListaDeVacunados)
            {
                EscribirTablaPacientes(paciente);
            }

            calendarizado = true;



            return View(lista);
        }
        //VISTA PARA AGREGAR PACIENTE
        public ActionResult AgregarPaciente()
        {


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //METODO PARA AGREGAR PACIENTE
        public ActionResult AgregarPaciente(IFormCollection collection)
        {

            try
            {
                //PACIENTE CON TODA LA  INFORMACION
                var paciente = new Models.Data.Paciente();
                //PACIENTE SOLO CON NOMBRE
                var paciente2 = new Models.Data.Paciente();
                //PACIENTE SOLO CON APELLIDO
                var paciente3 = new Models.Data.Paciente();
                //PACIENTE SOLO CON DPI
                var paciente4 = new Models.Data.Paciente();
     
                {

                    paciente.Nombre = collection["Nombre"];
                    paciente2.Nombre = collection["Nombre"];
                    paciente.Apellido = collection["Apellido"];
                    paciente3.Apellido = collection["Apellido"];
                    paciente.DPI = collection["DPI"];
                    paciente4.DPI = collection["DPI"];
                    paciente.Departamento = collection["Departamento"];
                    paciente.Municipio = municipiologgeado;
                    paciente.Edad = Convert.ToInt32(collection["Edad"]);
                    paciente.Profesion = collection["Profesion"];

                    paciente.Prioridad = calcularprioridad(paciente.Profesion, paciente.Edad);
               
                }
                //COMPROBACION DE QUE NO HAYA  UN DPI REPETIDO
                if (Singleton.Instance.TablaHashPacientes.GetNodeByKey(paciente.DPI) != (null,null))
                {
                    MostrarDialogo("El DPI ingresado no es válido");
                    return View();
                }
                else
                {

                    Singleton.Instance.TablaHashPacientes.Add(paciente.DPI, paciente);


                    if (paciente.Municipio == municipiologgeado)
                    {
                        Singleton.Instance.MinheapPacientes.Add(paciente);
                        Singleton.Instance.ListaDeEspera.AddLast(paciente);
                        
                    }
                    else
                    {
                        Singleton.Instance.ListaAuxiliar.AddLast(paciente);
                    }
                    

                    Singleton.Instance.Buscarpaciente.AddTo(paciente4, Singleton.Instance.Buscarpaciente.Root, paciente.DPI);
                    Singleton.Instance.BuscarNombre.AddTo(paciente2,  Singleton.Instance.BuscarNombre.Root, paciente.DPI);
                    Singleton.Instance.BuscarApellido.AddTo(paciente3,  Singleton.Instance.BuscarApellido.Root, paciente.DPI);
                    ActualizarTablaPacientes();
                    LeerTablaPacientes();

                    calendarizado = false;


                    return RedirectToAction(nameof(Index));

                }


            }
            catch
            {
                return View();
            }

        }
        //METODO PARA SIMULACION DE  VACUNACION 
        public ActionResult VacunarPersonas()
        {
            //COMPRUEBA SI YA SE ASIGNARON FECHAS PARA LOS PACIENTES
            if (calendarizado == false)
            {
                

                //calendarizado = true;
                return RedirectToAction(nameof(Index));
            }

            var heap = Singleton.Instance.MinheapPacientes;
            var listaDeEspera = Singleton.Instance.ListaDeEspera;
            var listaDeVacunados = Singleton.Instance.ListaDeVacunados;
      //REINICIA LA LISTA DE  ESPERA PARA EVITAR DUPLICACION DE DATOS
            foreach (Paciente paciente1 in listaDeEspera)
            {
                listaDeEspera.Remove(0);
            }
            //VUELVE A LLENAR LA LISTA DE  ESPERA
            for (int i = 0; i < heap.elementos.Length; i++)
            {
                var paciente = heap.elementos.ElementAt(i);
                listaDeEspera.AddLast(paciente);
            }
            try
            {
                for (int i = 0; i < CantidadAVacunar; i++)
                {
                    var temp = listaDeEspera.ElementAt(0);
                    //CALCULA LA PROBABILIDADA DE QUE EL PACIENTE NO HAYA  ASISTIDO
                    if (probabilidadDeAusencia() == 1)
                    {
                        //CASO SI EL PACIENTE NO SE PRESENTA A LA VACUNACION 
                        temp.FechaDeVacunacion = fecha;
                        temp.Prioridad = 12;
                        heap.elementos.Nodeatposition(0).Data.Prioridad = 12;
                        heap.listPriority();
                       //RECALENDARIZA LA VACUNACION
                        foreach (Paciente paciente1 in listaDeEspera)
                        {
                            listaDeEspera.Remove(0);
                        }
                        for (int j = 0; j < heap.elementos.Length; j++)
                        {
                            var paciente = heap.elementos.ElementAt(j);
                            listaDeEspera.AddLast(paciente);
                        }
                    }
                    else
                    {
                        //EL PACIENTE SI SE PRESENTA A LA VACUNACION Y LO ELIMINA DE LA COLA DE  PRIORIDAD Y LO AGREGA A LA LISTA DE VACUNADOS
                        int index = 0;
                        listaDeEspera.Remove(0);
                        temp.Vacunado = true;
                        listaDeVacunados.AddLast(temp);
                        Singleton.Instance.TablaHashPacientes.GetNode(temp.DPI).Vacunado = true;
                        if (heap.GetNode(0).Data == temp)
                        {
                            heap.elementos.Remove(0);
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

            }
            catch
            {
                ActualizarTablaPacientes();
                LeerTablaPacientes();
                return RedirectToAction(nameof(ListaDeVacunados));
            }

            ActualizarTablaPacientes();
            LeerTablaPacientes();
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
        //METODO QUE CALCULA EL PORCENTAJE DE VACUNADOS
        public ActionResult porcentajeDeVacunados()
        {
            var listaDePacientes = Singleton.Instance.MinheapPacientes.elementos.Count();
            var listaDeVacunados = Singleton.Instance.ListaDeVacunados.Count();
            double total = listaDeVacunados + listaDePacientes;
            double porcentaje;
            //CALCULO PARA EL PORCENTAJE
            porcentaje = (listaDeVacunados / total) * 100;
            MostrarDialogo("El porcentaje de vacunados es: " + Math.Round(porcentaje,2) + "%");

            return RedirectToAction(nameof(Index));
        }
        //METODO PARA CALCULAR LAS  PRIORIDADES SEGUN PROFESION Y EDAD
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
                else if  (Edad >=18 && Edad <= 39)
                {
                    return 11;
                }
            }
            return 0; 
        }

      

    }
}

