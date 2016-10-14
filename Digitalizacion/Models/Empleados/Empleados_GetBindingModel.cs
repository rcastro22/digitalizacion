using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Empleados
{
    public class Empleados_GetBindingModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public string ID { get; set; }

        public string QueryString()
        {
            return ID;
        }
    }
}
