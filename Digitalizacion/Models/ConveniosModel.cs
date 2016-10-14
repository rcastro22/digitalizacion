using Digitalizacion.Common;
using Digitalizacion.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class ConveniosModel : APIClient
    {
        public static async Task<IEnumerable<TipoConvenio_Result>> Get()
        {
            string json = await GetAPI("api/Convenios");

            try
            {
                List<TipoConvenio_Result> lst = new List<TipoConvenio_Result>();
                JsonArray jsonArray = JsonArray.Parse(json);

                foreach (IJsonValue fila in jsonArray)
                {
                    TipoConvenio_Result i = new TipoConvenio_Result(fila);

                    lst.Add(i);
                }

                return lst;
            }
            catch (Exception)
            {
                rootPage.NotifyUser("Error al procesar los datos obtenidos", NotifyType.ErrorMessage);

                throw;
            }
        }
    }
}
