using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Carreras
{
    public class Carreras_GetBindingModel
    {
        [Required]
        [MaxLength(8)]
        public string Carrera { get; set; }
    }
}
