using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Alumnos
{
    public class Alumnos_GetSolicitudesBindingModel : Alumnos_GetBindingModel
    {
        [Required]
        [MaxLength(10)]
        public string Carrera { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long Tramite { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public int Paso { get; set; }

        public string QueryString()
        {
            return string.Format("?Carrera={0}&Tramite={1}&ID={2}&Paso={3}", this.Carrera, this.Tramite, this.ID, this.Paso);
        }
    }
}
