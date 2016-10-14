using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models.Etiquetas
{
    public class Etiquetas
    {
        private short etiqueta;
        private string valor;

        public Etiquetas()
        {
        }

        public Etiquetas(short Etiqueta, string Valor)
        {
            etiqueta = Etiqueta;
            valor = Valor;
        }

        public Etiquetas(IJsonValue jsonValue)
        {
            JsonObject jsonObject = jsonValue.GetObject();

            etiqueta = Convert.ToInt16(jsonObject.GetNamedNumber("Etiqueta"));
            valor = jsonObject.GetNamedString("Valor");
        }

        public short Etiqueta
        {
            get
            {
                return etiqueta;
            }
            set
            {
                etiqueta = value;
            }
        }

        public string Valor
        {
            get
            {
                return valor;
            }
            set
            {
                valor = value;
            }
        }

        public JsonObject Objectify()
        {
            JsonObject jsonObject = new JsonObject();

            jsonObject.Add("Etiqueta", JsonValue.CreateNumberValue(etiqueta));
            jsonObject.Add("Valor", JsonValue.CreateStringValue(valor));

            return jsonObject;
        }
    }
}

