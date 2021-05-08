using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LibreriaProyecto;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class Paciente : IComparable, IComparable<Paciente>
    {

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string DPI { get; set; }

        [Required]
       
        public string Departamento { get; set; }

        [Required]
        public string Municipio { get; set; }

        [Required]
        public string Profesion { get; set; }
        
        [Required]
        [Range(18,110)]
        public int Edad { get; set; }

        public DateTime FechaDeVacunacion { get; set; }

        public int Prioridad { get; set; }
        
        public bool Vacunado { get; set; }
        public int CompareTo(object obj)
        {
            if (obj != null && obj.GetType() != GetType())
            {
                throw new ArgumentException(string.Format("Object must be of type {0}", GetType()));
            }

            return CompareTo((Paciente)obj);


        }

        public int CompareTo(Paciente newpaciente)
        {
         
            if (this.DPI != null)
            {
                var pacientes = this.DPI.CompareTo(newpaciente.DPI);
                return pacientes;

            }
            if (this.Nombre != null) {
                var pacientes = this.Nombre.CompareTo(newpaciente.Nombre);

                return pacientes;
            }

            if (this.Apellido != null)
            {
                var pacientes = this.Apellido.CompareTo(newpaciente.Apellido);
                return pacientes;
            }
            return 0;
        }
           

          
        }


    }

