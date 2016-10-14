using Digitalizacion.Common;
using Digitalizacion.Models.Archivos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Digitalizacion.Models
{
    class ArchivosModel : APIClient
    {
        public static async Task<IBuffer> Get(Archivos_GetBindingModel model)
        {
            IBuffer buffer = await GetAPIBuffer("api/Archivos" + model.QueryString());

            return buffer;
        }

        public static async Task<IEnumerable<Etiquetasarchivo_Result>> GetEtiquetas(Archivos_GetEtiquetasBindingModel model)
        {
            string json = await GetAPI("api/Archivos/Etiquetas" + model.QueryString());

            return null;
        }

        public static async Task<IHttpContent> Post(Archivos_PostBindingModel model, IEnumerable<IBuffer> files)
        {
            string json = model.Stringify();

            return await PostAPI("api/Archivos", json, files);

        }

        public static async Task<IEnumerable<ObtenerResolucionesCompletas_Result>> GetResoluciones(Archivos_GetResolucionesBindingModel model)
        {
            string json = await GetAPI("api/Archivos/Resoluciones" + model.QueryString());

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<ObtenerResolucionesCompletas_Result> lst = new List<ObtenerResolucionesCompletas_Result>();

                foreach (IJsonValue fila in jsonArray)
                {
                    ObtenerResolucionesCompletas_Result i = new ObtenerResolucionesCompletas_Result(fila);

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

        public static async Task PostCaja(Archivos_PostCajaBindingModel model)
        {
            //await GetAPI("api/Archivos/CajaExpediente" + model.QueryString());
            string json = model.Stringify();

            await PostAPI("api/Archivos/CajaExpediente", json);
        }

        public static async Task<string> GetCaja(Archivos_GetEtiquetasBindingModel model)
        {
            return await GetAPI("api/Archivos/GetNoCaja" + model.QueryString());
        }

        public static async Task PostPergamino(Archivos_PostPergaminoBindingModel model)
        {
            string json = model.Stringify();
            await PostAPI("api/Archivos/PergaminoExpediente", json);
        }
    }
}
