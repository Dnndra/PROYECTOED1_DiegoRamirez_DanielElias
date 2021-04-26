using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PROYECTOED1_DiegoRamirez_DanielElias.Models.Data
{
    public class Paciente
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
        public int Edad { get; set; }

    }
}
