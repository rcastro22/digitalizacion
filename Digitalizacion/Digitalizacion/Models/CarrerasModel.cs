using Digitalizacion.Common;
using Digitalizacion.Models.Alumnos;
using Digitalizacion.Models.Carreras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Digitalizacion.Models
{
    class CarrerasModel : APIClient
    {
        public static async Task<IEnumerable<Obtenercarrerasxcarnet_Result>> Get(Alumnos_GetBindingModel model)
        {
            string json = await GetAPI("api/Carreras/" + model.QueryString());

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<Obtenercarrerasxcarnet_Result> lst = new List<Obtenercarrerasxcarnet_Result>();

                foreach (IJsonValue fila in jsonArray)
                {
                    Obtenercarrerasxcarnet_Result i = new Obtenercarrerasxcarnet_Result(fila);

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

        public static async Task<string> GetEntidad(Carreras_GetBindingModel model)
        {
            return null;
        }

        public static async Task<IEnumerable<Obtenertitulosxcarrera_Result>> GetTitulos(Carreras_GetTitulosBindingModel model)
        {
            string json = await GetAPI("api/Carreras/Titulos" + model.QueryString());

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<Obtenertitulosxcarrera_Result> lst = new List<Obtenertitulosxcarrera_Result>();

                foreach (IJsonValue fila in jsonArray)
                {
                    Obtenertitulosxcarrera_Result i = new Obtenertitulosxcarrera_Result(fila);

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


        public static async Task<IEnumerable<Obtenercarrerasxcarnet_Result>> GetNombreCarreras(Carreras_GetCarrerasBindingModel model)
        {
            string json = await GetAPI("api/Carreras/NombreCarreras" + model.QueryString());

            try
            {
                JsonArray jsonArray = JsonArray.Parse(json);
                List<Obtenercarrerasxcarnet_Result> lst = new List<Obtenercarrerasxcarnet_Result>();

                foreach (IJsonValue fila in jsonArray)
                {
                    Obtenercarrerasxcarnet_Result i = new Obtenercarrerasxcarnet_Result(fila);

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