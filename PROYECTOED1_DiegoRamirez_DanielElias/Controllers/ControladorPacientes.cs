using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PROYECTOED1_DiegoRamirez_DanielElias.Models.Data;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Controllers
{
    public class ControladorPacientes : Controller
    {
        // GET: ControladorPacientes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListaDePacientes()
        {
            return View(Singleton.Instance.TablaHashPacientes);
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
                var node = new Models.Data.PriorityNode<Models.Data.Paciente>();
                {

                    paciente.Nombre = collection["Nombre"];
                    paciente.Apellido = collection["Apellido"];
                    paciente.DPI = collection["DPI"];
                    paciente.Departamento = collection["Departamento"];
                    paciente.Municipio = collection["Municipio"];
                    paciente.Edad = Convert.ToInt32(collection["Edad"]);
                    paciente.Profesion = collection["Profesion"];
                    node.Data = paciente;
                    node.prioridad = calcularprioridad(paciente.Profesion, paciente.Edad); 
                }
                Singleton.Instance.TablaHashPacientes.Add(paciente.Nombre, paciente);
                Singleton.Instance.MinheapPacientes.Add(node);
                Singleton.Instance.Buscarpaciente.AddTo(paciente, Singleton.Instance.Buscarpaciente.Root);
                return RedirectToAction(nameof(ListaDePacientes));

            }
            catch
            {
                return View();
            }

        }

        // GET: ControladorPacientes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ControladorPacientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControladorPacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControladorPacientes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControladorPacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControladorPacientes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControladorPacientes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        //METODOS
        public int calcularprioridad(string  profesion, int Edad)
        {
            if (profesion == "Trabajador de  salud")
            {
                return 1;
            }
            else if(profesion == "Estudiantes de salud" ) {
                return 2; 

            }
            else if  (profesion == "Cuerpos de  socorro")
            {
                return 3;
            }
            else if (profesion == "Trabajador de  funeraria")
            {
                return 3; 
            }
            else if(profesion == "Cuidador de  adultos mayores") {
                return 3; 
            }
            else if (profesion == "Seguridad nacional")
            {
                return 6; 
            }
            else if (profesion == "Trabajador de municipalidad")
            {
                return 7; 
            }
            else if (profesion == "Trabajador del sector educación")
            {
                return 8; 
            }
            else if (profesion == "Trabajador del secto justicia")
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
