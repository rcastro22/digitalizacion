using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Alumnos
{
    public class Requisitosadmision_Result
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Codigonombre { get; set; }

        public Requisitosadmision_Result(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            Codigo = jsonObject.GetNamedString("Codigo");
            Nombre = jsonObject.GetNamedString("Nombre");
            Codigonombre = jsonObject.GetNamedString("Codigonombre");
        }
    }
}
