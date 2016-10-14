using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Carreras
{
    public class Obtenercarrerasxcarnet_Result
    {
        public string Carrera { get; set; }
        public string Nombre { get; set; }

        public Obtenercarrerasxcarnet_Result(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            Carrera = jsonObject.GetNamedString("Carrera");
            Nombre = jsonObject.GetNamedString("Nombre");
        }
    }
}
