using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Carreras
{
    public class Carreras_GetTitulosBindingModel : Carreras_GetBindingModel
    {
        [Required]
        [Range(0, 2)]
        public short Titulo { get; set; }

        public string QueryString()
        {
            return string.Format("?Titulo={0}&Carrera={1}", this.Titulo, this.Carrera);
        }
    }
}
