using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Archivos
{
    public class Archivos_PostBindingModel
    {
        [Required]
        [MaxLength(2)]
        public string Aplicacion { get; set; }

        [Required]
        [Range(1, short.MaxValue)]
        public short Categoria { get; set; }

        [Required]
        public IEnumerable<Models.Etiquetas.Etiquetas> Etiquetas { get; set; }

        [Required]
        public bool Agregar { get; set; }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject.Add("Aplicacion", JsonValue.CreateStringValue(Aplicacion));
            jsonObject.Add("Categoria", JsonValue.CreateNumberValue(Categoria));

            JsonArray EtiquetasArray = new JsonArray();

            foreach (var fila in Etiquetas)
            {
                EtiquetasArray.Add(fila.Objectify());
            }

            jsonObject.Add("Etiquetas", EtiquetasArray);
            jsonObject.Add("Agregar", JsonValue.CreateBooleanValue(Agregar));

            return jsonObject.Stringify();
        }
    }
}
