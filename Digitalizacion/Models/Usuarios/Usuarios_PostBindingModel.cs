using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Usuarios
{
    class Usuarios_PostBindingModel
    {
        [Required]
        [MaxLength(32)]
        public string Usuario { get; set; }

        [Required]
        [MaxLength(50)]
        public string Contrasena { get; set; }

        public string Stringify()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject.Add("Usuario", JsonValue.CreateStringValue(Usuario));
            jsonObject.Add("Contrasena", JsonValue.CreateStringValue(Contrasena));

            return jsonObject.Stringify();
        }
    }
}
