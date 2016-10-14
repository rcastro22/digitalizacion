using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Facultades
{
    public class Facultad_Result
    {
        public string Entidad { get; set; }
        public string Descripcion_Corta { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion_Larga { get; set; }

        public Facultad_Result(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            Entidad = jsonObject.GetNamedString("Entidad");
            Descripcion_Corta = jsonObject.GetNamedString("Descripcion_Corta");
            Descripcion = jsonObject.GetNamedString("Descripcion");
            Descripcion_Larga = jsonObject.GetNamedString("Descripcion_Larga");
        }
    }
}
