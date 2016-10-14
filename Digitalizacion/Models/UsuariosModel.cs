using Digitalizacion.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class UsuariosModel : APIClient
    {
        public static async Task<IEnumerable<int>> GetVinculos()
        {
            string json = await GetAPI("api/Usuarios/Vinculos");

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<int> lst = new List<int>();

                foreach (IJsonValue fila in jsonArray)
                {
                    int i = Convert.ToInt32(fila.GetNumber());

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

        public static async Task<bool> GetAutorizar()
        {
            string json = await GetAPI("api/Usuarios/Autorizar");

            try
            {
                JsonValue jsonValue = JsonValue.Parse(json);

                return jsonValue.GetBoolean();
            }
            catch (Exception)
            {
                rootPage.NotifyUser("Error al procesar los datos obtenidos", NotifyType.ErrorMessage);

                return false;
            }
        }

        public static async Task<bool> PostAutenticar(Usuarios.Usuarios_PostBindingModel model)
        {
            string json = model.Stringify();

            json = await PostAPI("api/Usuarios/Autenticar", json);

            try
            {
                JsonValue jsonValue = JsonValue.Parse(json);

                return jsonValue.GetBoolean();
            }
            catch (Exception)
            {
                rootPage.NotifyUser("Error al procesar los datos obtenidos", NotifyType.ErrorMessage);

                return false;
            }
        }
    }
}
