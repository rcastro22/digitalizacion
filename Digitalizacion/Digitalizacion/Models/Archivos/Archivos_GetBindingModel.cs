using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Archivos
{
    public class Archivos_GetBindingModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long ID { get; set; }

        public string QueryString()
        {
            return string.Format("?ID={0}", ID);
        }
    }
}
