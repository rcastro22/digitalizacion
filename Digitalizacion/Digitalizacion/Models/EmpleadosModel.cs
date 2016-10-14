using Digitalizacion.Common;
using Digitalizacion.Models.Empleados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class EmpleadosModel : APIClient
    {
        public static async Task<string> GetNombre(Empleados_GetBindingModel model)
        {
            string json = await GetAPI("api/Empleados/" + model.QueryString());

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