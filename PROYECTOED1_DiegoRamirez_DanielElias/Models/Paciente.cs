using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models
{
    public class Paciente : IComparable

    {
        [Required]
        public int ID { get; set; }

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
       

        public int CompareTo(object obj)
        {
            var ordertree = ((Paciente)obj).Nombre;
            return ordertree.CompareTo(Nombre);
        }
    }
}
