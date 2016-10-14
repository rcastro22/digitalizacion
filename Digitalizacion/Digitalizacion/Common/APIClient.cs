using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Cryptography.Certificates;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Digitalizacion.Common
{
    class APIClient : IDisposable
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        public static MainPage rootPage = MainPage.Current;
        private const string AddressField = "http://dg.galileo.edu/dg/";
        private static HttpBaseProtocolFilter filter;
        private static HttpClient httpClient;
        private static CancellationTokenSource cts;
        private static bool Repetir;
        public static string Usuario;



        public APIClient()
        {
            // In this scenario we just create an HttpClient instance with default settings. I.e. no custom filters. 
            // For examples on how to use custom filters see other scenarios.
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            filter.MaxVersion = HttpVersion.Http20;

            // Most Recent (always hits the network, but if it is already in the
            // cache, If - Modified - Since header is added automatically and if
            // server does not have a newer version, the server will reply with
            // 304 and network bandwidth will be saved)
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            // No Cache (never stored in the cache)
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

            // ---------------------------------------------------------------------------
            // WARNING: Only test applications should ignore SSL errors.
            // In real applications, ignoring server certificate errors can lead to MITM
            // attacks (while the connection is secure, the server is not authenticated).
            //
            // The SetupServer script included with this sample creates a server certificate that is self-signed
            // and issued to fabrikam.com, and hence we need to ignore these errors here. 
            // ---------------------------------------------------------------------------
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);
        }

        protected async static Task<string> GetAPI(string API)
        {
            cts = new CancellationTokenSource();

            Uri resourceUri = new Uri(AddressField + API);

            rootPage.NotifyUser("En progreso", NotifyType.StatusMessage);

            try
            {
                HttpResponseMessage response;

                do
                {
                    Repetir = false;

                    httpClient = await Helpers.CreateHttpClient(httpClient);

                    response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

                    if (!response.IsSuccessStatusCode)
                    {
                        await MostrarErrorAPI(response);
                    }
                }
                while (Repetir);

                string result = await Helpers.GetTextResultAsync(response, cts.Token);

                rootPage.NotifyUser("Completado.", NotifyType.StatusMessage);

                return result;
            }
            catch (TaskCanceledException)
            {
                rootPage.NotifyUser("Solicitud cancelada.", NotifyType.ErrorMessage);

                throw;
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser("Error: " + ex.Message, NotifyType.ErrorMessage);

                throw;
            }
        }

        protected async static Task<IBuffer> GetAPIBuffer(string API)
        {
            cts = new CancellationTokenSource();

            Uri resourceUri = new Uri(AddressField + API);

            rootPage.NotifyUser("En progreso", NotifyType.StatusMessage);

            try
            {
                HttpResponseMessage response;

                do
                {
                    Repetir = false;

                    httpClient = await Helpers.CreateHttpClient(httpClient);

                    response = await httpClient.GetAsync(resourceUri).AsTask(cts.Token);

                    if (!response.IsSuccessStatusCode)
                    {
                        await MostrarErrorAPI(response);
                    }
                }
                while (Repetir);

                IBuffer result = await Helpers.GetStreamResultAsync(response, cts.Token);

                rootPage.NotifyUser("Completado.", NotifyType.StatusMessage);

                return result;
            }
            catch (TaskCanceledException)
            {
                rootPage.NotifyUser("Solicitud cancelada.", NotifyType.ErrorMessage);

                throw;
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser("Error: " + ex.Message, NotifyType.ErrorMessage);

                throw;
            }
        }

        protected async static Task<string> PostAPI(string API, string data)
        {
            cts = new CancellationTokenSource();

            Uri resourceUri = new Uri(AddressField + API);

            //rootPage.NotifyUser("En progreso", NotifyType.StatusMessage);

            try
            {
                HttpResponseMessage response;

                do
                {
                    Repetir = false;

                    httpClient = await Helpers.CreateHttpClient(httpClient);

                    HttpStringContent form = new HttpStringContent(data);

                    form.Headers.ContentType = Windows.Web.Http.Headers.HttpMediaTypeHeaderValue.Parse("application/json");

                    response = await httpClient.PostAsync(resourceUri, form).AsTask(cts.Token);

                    if (!response.IsSuccessStatusCode)
                    {
                        await MostrarErrorAPI(response);
                    }
                }
                while (Repetir);

                string result = await Helpers.GetTextResultAsync(response, cts.Token);

                //rootPage.NotifyUser("Completado.", NotifyType.StatusMessage);

                return result;
            }
            catch (TaskCanceledException)
            {
                //rootPage.NotifyUser("Solicitud cancelada.", NotifyType.ErrorMessage);

                throw;
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser("Error: " + ex.Message, NotifyType.ErrorMessage);

                throw;
            }
        }

        protected async static Task<IHttpContent> PostAPI(string API, string data, IEnumerable<IBuffer> files)
        {
            cts = new CancellationTokenSource();

            Uri resourceUri = new Uri(AddressField + API);

            rootPage.NotifyUser("En progreso", NotifyType.StatusMessage);

            try
            {
                HttpResponseMessage response;

                do
                {
                    Repetir = false;

                    httpClient = await Helpers.CreateHttpClient(httpClient);

                    HttpMultipartFormDataContent form = new HttpMultipartFormDataContent();

                    form.Add(new HttpStringContent(data), "model");
                    form.First().Headers.ContentType = Windows.Web.Http.Headers.HttpMediaTypeHeaderValue.Parse("application/json");

                    for (int i = 0; i < files.Count(); i++)
                    {
                        form.Add(new HttpBufferContent(files.ElementAt(i), 0, files.ElementAt(i).Length), "image" + i, "Imagen" + i + ".bmp");
                        form.ElementAt(i + 1).Headers.ContentType = Windows.Web.Http.Headers.HttpMediaTypeHeaderValue.Parse("image/bmp");
                    }

                    response = await httpClient.PostAsync(resourceUri, form).AsTask(cts.Token);

                    if (!response.IsSuccessStatusCode)
                    {
                        await MostrarErrorAPI(response);
                    }
                }
                while (Repetir);

                rootPage.NotifyUser("Completado.", NotifyType.StatusMessage);

                return response.Content;
            }
            catch (TaskCanceledException)
            {
                rootPage.NotifyUser("Solicitud cancelada.", NotifyType.ErrorMessage);

                throw;
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser("Error: " + ex.Message, NotifyType.ErrorMessage);

                throw;
            }
        }

        private static async Task MostrarErrorAPI(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                rootPage.NotifyUser("La sesión ha expirado, intentando de nuevo...", NotifyType.StatusMessage);

                Repetir = true;
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                rootPage.NotifyUser("No está autorizado para usar esta aplicación.", NotifyType.ErrorMessage);
                rootPage.ShowLogin();
            }
            else
            {
                string json = await response.Content.ReadAsStringAsync();

                JsonObject jsonObject = JsonObject.Parse(json);

                throw new Exception(jsonObject.GetNamedString("Message"));
            }
        }

        public void Close()
        {
            cts.Cancel();
            cts.Dispose();

            // Re-create the CancellationTokenSource.
            cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            if (filter != null)
            {
                filter.Dispose();
                filter = null;
            }

            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }

            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }
        }
    }
}
