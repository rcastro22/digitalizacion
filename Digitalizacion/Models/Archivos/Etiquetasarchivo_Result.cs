using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Archivos
{
    public class Etiquetasarchivo_Result
    {
        public decimal Idarchivo { get; set; }
        public string Aplicacion { get; set; }
        public string Nombreaplicacion { get; set; }
        public short Categoria { get; set; }
        public string Nombrecategoria { get; set; }
        public short Etiqueta { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
