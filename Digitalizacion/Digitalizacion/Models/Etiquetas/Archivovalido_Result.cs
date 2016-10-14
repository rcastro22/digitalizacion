using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Etiquetas
{
    public class Archivovalido_Result
    {
        public decimal Idarchivo { get; set; }
        public string Nombreapp { get; set; }
        public string Nombrecat { get; set; }
        public string Aplicacion { get; set; }
        public short Categoria { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Extension { get; set; }
        public DateTime? Fecha { get; set; }
        public short? Automatizado { get; set; }

        public Archivovalido_Result(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            Idarchivo = Convert.ToDecimal(jsonObject.GetNamedNumber("Idarchivo"));
            Nombreapp = jsonObject.GetNamedString("Nombreapp");
            Nombrecat = jsonObject.GetNamedString("Nombrecat");
            Aplicacion = jsonObject.GetNamedString("Aplicacion");
            Categoria = Convert.ToInt16(jsonObject.GetNamedNumber("Categoria"));
            Nombre = jsonObject.GetNamedString("Nombre");
            Usuario = jsonObject.GetNamedString("Usuario");
            Extension = jsonObject.GetNamedString("Extension");
            Fecha = Convert.ToDateTime(jsonObject.GetNamedString("Fecha"));
            Automatizado = Convert.ToInt16(jsonObject.GetNamedNumber("Automatizado"));
        }
    }
}
