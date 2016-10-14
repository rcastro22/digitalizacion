using Digitalizacion.Common;
using Digitalizacion.Models.Facultades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class FacultadesModel : APIClient
    {
        public static async Task<IEnumerable<Facultad_Result>> Get()
        {
            string json = await GetAPI("api/Facultades");

            try
            {
                List<Facultad_Result> lst = new List<Facultad_Result>();
                JsonArray jsonArray = JsonArray.Parse(json);

                foreach (IJsonValue fila in jsonArray)
                {
                    Facultad_Result i = new Facultad_Result(fila);

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
