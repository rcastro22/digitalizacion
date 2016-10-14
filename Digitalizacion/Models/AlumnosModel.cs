using Digitalizacion.Common;
using Digitalizacion.Models.Alumnos;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class AlumnosModel : APIClient
    {
        public static async Task<string> GetNombre(Alumnos_GetBindingModel model)
        {
            string json = await GetAPI("api/Alumnos/" + model.QueryString());

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

        public static async Task<IEnumerable<Requisitosadmision_Result>> GetRequisitosAdmision()
        {
            string json = await GetAPI("api/Alumnos/RequisitosAdmision");

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<Requisitosadmision_Result> lst = new List<Requisitosadmision_Result>();

                foreach (IJsonValue fila in jsonArray)
                {
                    Requisitosadmision_Result i = new Requisitosadmision_Result(fila);

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

        public static async Task<IEnumerable<long>> GetSolicitudesPendientes(Alumnos_GetSolicitudesBindingModel model)
        {
            string json = await GetAPI("api/Alumnos/SolicitudesPendientes" + model.QueryString());

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<long> lst = new List<long>();

                foreach (IJsonValue fila in jsonArray)
                {
                    long i = Convert.ToInt64(fila.GetNumber());

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