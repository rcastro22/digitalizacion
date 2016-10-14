using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Archivos
{
    public class Archivos_PostCajaBindingModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long IDArchivo { get; set; }

        [Required]
        [MaxLength(20)]
        public string Caja { get; set; }

        public string QueryString()
        {
            return string.Format("?IDArchivo={0}&Caja={1}", IDArchivo, Caja);
        }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject.Add("IDArchivo", JsonValue.CreateStringValue(IDArchivo.ToString()));
            jsonObject.Add("Caja", JsonValue.CreateStringValue(Caja));

            return jsonObject.Stringify();
        }
    }
}
