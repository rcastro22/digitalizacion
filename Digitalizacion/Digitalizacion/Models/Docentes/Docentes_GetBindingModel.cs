using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Docentes
{
    public class Docentes_GetBindingModel
    {
        [Required]
        [MaxLength(6)]
        public string ID { get; set; }

        public string QueryString()
        {
            return ID;
        }
    }
}
