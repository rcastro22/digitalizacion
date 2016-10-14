using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Carreras
{
    public class Obtenertitulosxcarrera_Result
    {
        public string Titulo { get; set; }
        public string Nombre { get; set; }

        public Obtenertitulosxcarrera_Result(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            Titulo = jsonObject.GetNamedString("Titulo");
            Nombre = jsonObject.GetNamedString("Nombre");
        }
    }
}
