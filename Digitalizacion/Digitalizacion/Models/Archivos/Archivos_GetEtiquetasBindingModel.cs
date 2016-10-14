﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models.Archivos
{
    public class Archivos_GetEtiquetasBindingModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long IDArchivo { get; set; }

        public string QueryString()
        {
            return string.Format("?IDArchivo={0}", IDArchivo);
        }
    }
}
