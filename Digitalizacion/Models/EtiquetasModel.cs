using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class EtiquetasModel : APIClient
    {
        public static async Task<IEnumerable<Etiquetas_Result>> Get()
        {
            string json = await GetAPI("api/Usuarios/Vinculos");

            return null;
        }

        public static async Task<long> Get(Etiquetas_GetArchivosBindingModel model)
        {
            string json = await GetAPI("api/Etiquetas/Archivo" + model.QueryString());

            try
            {
                JsonValue jsonValue = JsonValue.Parse(json);

                return Convert.ToInt64(jsonValue.GetNumber());
            }
            catch (Exception)
            {
                rootPage.NotifyUser("Error al procesar los datos obtenidos", NotifyType.ErrorMessage);

                throw;
            }
        }
    }
}
