using Digitalizacion.Common;
using Digitalizacion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Digitalizacion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransferirPage : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;

        public TransferirPage()
        {
            this.InitializeComponent();
        }

        private void chkTodos_Checked(object sender, RoutedEventArgs e)
        {
            lstArchivos.SelectAll();
        }

        private void chkTodos_Unchecked(object sender, RoutedEventArgs e)
        {
            lstArchivos.SelectedItems.Clear();
        }

        private async void btnTransferir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CambiarVisibilidad(Visibility.Collapsed);

                Models.Archivos.Archivos_PostBindingModel model = new Models.Archivos.Archivos_PostBindingModel();

                foreach (var fila in lstArchivos.SelectedItems.Reverse())
                {
                    TransferirModel myFolder = (TransferirModel)fila;
                    myFolder.Enviando = true;

                    try
                    {
                        model = await TransferirContext.getModel(myFolder.Nombre);
                        model.Agregar = true;

                        IEnumerable<IBuffer> archivos = await TransferirContext.getFolderFiles(myFolder.Nombre);

                        string IdArchivo;
                        IdArchivo = Convert.ToString(await ArchivosModel.Post(model, archivos));
                        /*IdArchivo = Convert.ToString(IdArchivo).Remove(IdArchivo.IndexOf("."));

                        Models.Archivos.Archivos_GetBindingModel model2 = new Models.Archivos.Archivos_GetBindingModel();
                        model2.ID = Convert.ToInt64(IdArchivo);

                        IBuffer archivo = await ArchivosModel.Get(model2);
                        byte[] st = archivo.ToArray();

                        string path = @"C:\Users\DynamicsAdmin\Pictures\ARCHIVOS" + @"\" + "arch" + "." + "pdf";
                        //path = @"C:\Users\Luis\Documents\" + IDArchivo + "." + Extension;
                        await obtenerArchivoGuardad(st, path);*/


                        await TransferirContext.RemoverFolder(myFolder);
                    }
                    catch (Exception ex)
                    {
                        myFolder.Mensaje = ex.Message;
                        myFolder.Enviando = false;
                    }
                }
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
            }
            finally
            {
                CambiarVisibilidad(Visibility.Visible);
            }
        }

        public async Task obtenerArchivoGuardad(byte[] buffer, string path)
        {
            await Task.Run(() =>
            {
                Task.Yield();
                using (FileStream stream = new FileStream(path, FileMode.CreateNew))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            });
        }

        private void lstArchivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TransferirContext.SelectedArchivos = lstArchivos.SelectedItems.Count;
        }

        private void CambiarVisibilidad(Visibility visibilidad)
        {
            stkBuscar.Visibility = visibilidad;
            chkTodos.Visibility = visibilidad;
            stkCategoria.Visibility = visibilidad;
        }
    }
}
