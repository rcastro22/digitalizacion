using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Archivos
{
    public class Archivos_PostPergaminoBindingModel
    {
        [Required]
        [MaxLength(20)]
        public string Caja { get; set; }

        [Required]
        [MaxLength(20)]
        public string Pergamino { get; set; }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();            

            jsonObject.Add("Caja", JsonValue.CreateStringValue(Caja));
            jsonObject.Add("Pergamino", JsonValue.CreateStringValue(Pergamino));

            return jsonObject.Stringify();
        }
    }
}
