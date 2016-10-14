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
    public sealed partial class PergaminoPage : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;
        EscanerDataContext model;

        public PergaminoPage()
        {
            this.InitializeComponent();
        }

        private async void btnUpdate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            model = (EscanerDataContext)rootPage.DataContext;
            string valorCaja = txtCaja.Text;
            string valorPergamino = txtPergamino.Text;

            // Codig de actualizacion de no. de pergamino según el no. de caja            
            try
            {
                Models.Archivos.Archivos_PostPergaminoBindingModel modelPergamino = new Models.Archivos.Archivos_PostPergaminoBindingModel();
                modelPergamino.Caja = valorCaja;
                modelPergamino.Pergamino = valorPergamino;

                await ArchivosModel.PostPergamino(modelPergamino);

                MainPage.Current.NotifyUser("Datos guardados exitosamente", NotifyType.StatusMessage);
            }
            catch (Exception ex) {
                MainPage.Current.NotifyUser(ex.Message, NotifyType.ErrorMessage);
            }
            //

            model.ClearEtiquetas();
            LimpiarVista();
        }

        private void LimpiarVista()
        {            
            txtCaja.Text = String.Empty;
            txtPergamino.Text = String.Empty;
        }
    }
}
