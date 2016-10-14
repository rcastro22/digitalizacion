using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Carreras
{
    public class Carreras_GetCarrerasBindingModel : Carreras_GetBindingModel
    {
        [Required]
        [MaxLength(60)]
        public string BuscarCarrera { get; set; }

        public string QueryString()
        {
            return string.Format("?BuscarCarrera={0}", this.BuscarCarrera);
        }
    }
}
