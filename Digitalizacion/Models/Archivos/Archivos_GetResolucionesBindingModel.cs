using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Archivos
{
    public class Archivos_GetResolucionesBindingModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public string resolucion { get; set; }

        public string QueryString()
        {
            return string.Format("?Resolucion={0}", resolucion);
        }
    }
}
