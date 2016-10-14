using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Archivos
{
    public class ObtenerResolucionesCompletas_Result
    {
        public string Resolucion { get; set; }
        public string Nombre { get; set; }

        public ObtenerResolucionesCompletas_Result(IJsonValue jsonValue)
        {
            /*JsonObject jsonObject = jsonValue.GetObject();

            Resolucion = jsonObject.GetNamedString("Valor");
            Nombre = jsonObject.GetNamedString("Valor");*/

            Resolucion = jsonValue.GetString();
            Nombre = jsonValue.GetString();

        }
    }
}
