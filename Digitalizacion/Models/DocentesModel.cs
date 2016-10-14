using Digitalizacion.Common;
using Digitalizacion.Models.Docentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class DocentesModel : APIClient
    {
        /// <summary>
        /// Obtener nombre segun CodPers
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetNombre(Docentes_GetBindingModel model)
        {
            string json = await GetAPI("api/Docentes/" + model.QueryString());

            try
            {
                JsonValue jsonValue = JsonValue.Parse(json);

                return jsonValue.GetString();
            }
            catch (Exception)
            {
                rootPage.NotifyUser("Error al procesar los datos obtenidos", NotifyType.ErrorMessage);

                throw;
            }
        }
    }
}
