using Windows.UI.Xaml.Controls;
using Digitalizacion.Common;
using Digitalizacion.Models;
using System;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Digitalizacion.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExpedientePage : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;
        EscanerDataContext model;

        public ExpedientePage()
        {
            this.InitializeComponent();
        }

        private async void btnUpdate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            model = (EscanerDataContext)rootPage.DataContext;
            string valorCaja = txtCaja.Text;

            // Codig de actualizacion de no. de caja
            long IDArchivo = 0;
            if (model.Etiquetas.Count() > 0)
            {
                
                try
                {
                    Models.Etiquetas.Etiquetas_GetArchivosBindingModel mdl = new Models.Etiquetas.Etiquetas_GetArchivosBindingModel();
                    mdl.Aplicacion = model.Aplicacion;
                    mdl.Categoria = model.Categoria;
                    mdl.Etiquetas = model.Etiquetas;

                    IDArchivo = await EtiquetasModel.Get(mdl);
                }
                catch (Exception) { }
            }

            try
            {
                Models.Archivos.Archivos_PostCajaBindingModel modelCaja = new Models.Archivos.Archivos_PostCajaBindingModel();
                modelCaja.IDArchivo = IDArchivo;
                modelCaja.Caja = valorCaja;

                await ArchivosModel.PostCaja(modelCaja);
            }
            catch (Exception) { }
            //

            model.ClearEtiquetas();
            LimpiarVista();
        }

        private void LimpiarVista()
        {
            txtCaja.Text = String.Empty;
            txtCarne.Text = String.Empty;
        }
    }
}
