using Digitalizacion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace Digitalizacion.Common
{
    internal static class Helpers
    {
        //private static async Task DisplayTextResultAsync(
        //    HttpResponseMessage response,
        //    TextBox output,
        //    CancellationToken token)
        //{
        //    string responseBodyAsText;
        //    output.Text += SerializeHeaders(response);
        //    responseBodyAsText = await response.Content.ReadAsStringAsync().AsTask(token);

        //    token.ThrowIfCancellationRequested();

        //    // Insert new lines.
        //    responseBodyAsText = responseBodyAsText.Replace("<br>", Environment.NewLine);

        //    output.Text += responseBodyAsText;
        //}

        internal static async Task<string> GetTextResultAsync(
    HttpResponseMessage response,
    CancellationToken token)
        {
            string responseBodyAsText;
            //output.Text += SerializeHeaders(response);
            responseBodyAsText = await response.Content.ReadAsStringAsync().AsTask(token);

            token.ThrowIfCancellationRequested();

            return responseBodyAsText;
        }

        internal static async Task<IBuffer> GetStreamResultAsync(
    HttpResponseMessage response,
    CancellationToken token)
        {
            IBuffer buffer = await response.Content.ReadAsBufferAsync().AsTask(token);

            token.ThrowIfCancellationRequested();

            return buffer;
        }

        internal static string SerializeHeaders(HttpResponseMessage response)
        {
            StringBuilder output = new StringBuilder();

            // We cast the StatusCode to an int so we display the numeric value (e.g., "200") rather than the
            // name of the enum (e.g., "OK") which would often be redundant with the ReasonPhrase.
            output.Append(((int)response.StatusCode) + " " + response.ReasonPhrase + "\r\n");

            SerializeHeaderCollection(response.Headers, output);
            SerializeHeaderCollection(response.Content.Headers, output);
            output.Append("\r\n");
            return output.ToString();
        }

        internal static void SerializeHeaderCollection(
            IEnumerable<KeyValuePair<string, string>> headers,
            StringBuilder output)
        {
            foreach (var header in headers)
            {
                output.Append(header.Key + ": " + header.Value + "\r\n");
            }
        }

        internal async static Task<HttpClient> CreateHttpClient(HttpClient httpClient)
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
            }

            // HttpClient functionality can be extended by plugging multiple filters together and providing
            // HttpClient with the configured filter pipeline.
            IHttpFilter filter = new HttpBaseProtocolFilter();
            filter = new PlugInFilter(filter); // Adds a custom header to every request and response message.
            httpClient = new HttpClient(filter);

            // The following line sets a "User-Agent" request header as a default header on the HttpClient instance.
            // Default headers will be sent with every request sent from this HttpClient instance.

            string Token = Convert.ToString(APIClient.Usuario); //await Helpers.DomainNameAsync();

            if (Token != null)
            {
                httpClient.DefaultRequestHeaders.Add("Token", Token);
            }

            // get the system version number
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);
            string SystemVersion = $"{v1}.{v2}.{v3}.{v4}";

            PackageVersion pv = Package.Current.Id.Version;
            EasClientDeviceInformation eas = new EasClientDeviceInformation();

            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("DeviceFamily", AnalyticsInfo.VersionInfo.DeviceFamily));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("DeviceFamilyVersion", SystemVersion));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Architecture", Package.Current.Id.Architecture.ToString()));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("Version", $"{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("DeviceManufacturer", eas.SystemManufacturer));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("DeviceModel", eas.SystemProductName));

            return httpClient;
        }

        internal static void ScenarioStarted(Button startButton, Button cancelButton, TextBox outputField)
        {
            startButton.IsEnabled = false;
            cancelButton.IsEnabled = true;
            if (outputField != null)
            {
                outputField.Text = String.Empty;
            }
        }

        internal static void ScenarioCompleted(Button startButton, Button cancelButton)
        {
            startButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
        }

        internal static void ReplaceQueryString(TextBox addressField, string newQueryString)
        {
            string resourceAddress = addressField.Text;

            // Remove previous query string.
            int questionMarkIndex = resourceAddress.IndexOf("?", StringComparison.Ordinal);
            if (questionMarkIndex != -1)
            {
                resourceAddress = resourceAddress.Substring(0, questionMarkIndex);
            }

            addressField.Text = resourceAddress + newQueryString;
        }

        internal async static Task<string> DomainNameAsync()
        {
            var users = await Windows.System.User.FindAllAsync();

            var current = users.Where(p => p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated &&
                               p.Type == UserType.LocalUser).FirstOrDefault();

            // user may have username
            string AccountName = (string)await current.GetPropertyAsync(KnownUserProperties.AccountName);
            string DomainName = (string)await current.GetPropertyAsync(KnownUserProperties.DomainName);

            //or may be authinticated using hotmail 
            if (String.IsNullOrWhiteSpace(AccountName) && !String.IsNullOrWhiteSpace(DomainName))
            {
                return DomainName;
            }
            else
            {
                return null;
            }
        }

        internal async static Task<string> DisplayNameAsync()
        {
            var users = await Windows.System.User.FindAllAsync();

            var current = users.Where(p => p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated &&
                               p.Type == UserType.LocalUser).FirstOrDefault();

            string DisplayName = (string)await current.GetPropertyAsync(KnownUserProperties.DisplayName);

            return DisplayName;
        }
    }
}
