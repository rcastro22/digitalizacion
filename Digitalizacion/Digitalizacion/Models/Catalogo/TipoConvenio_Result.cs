using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Catalogo
{
    public class TipoConvenio_Result
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }

        public TipoConvenio_Result(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            Codigo = Convert.ToInt32(jsonObject.GetNamedNumber("Codigo"));
            Nombre = jsonObject.GetNamedString("Nombre");

        }
    }
}
