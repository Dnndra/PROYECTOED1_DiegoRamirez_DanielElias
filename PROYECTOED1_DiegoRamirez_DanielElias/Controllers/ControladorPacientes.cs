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
              
                var paciente = new Models.Paciente
                {
                    ID = Convert.ToInt32(collection["ID"]),
                    Nombre = collection["Nombre"],
                    Apellido = collection["Apellido"],
                    DPI = collection["DPI"],
                    Departamento = collection["Departamento"],
                    Municipio = collection["Municipio"]

                };

                Singleton.Instance.TablaHashPacientes.Add(paciente.Nombre, paciente);
              
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
    }
}
