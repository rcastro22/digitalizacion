using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Alumnos
{
    public class Alumnos_GetBindingModel
    {
        [Required]
        [MaxLength(8)]
        public string ID { get; set; }

        public string QueryString()
        {
            return ID;
        }
    }
}
