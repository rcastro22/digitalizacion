using Digitalizacion.Common;
using Digitalizacion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Scanners;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Digitalizacion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScannerPage : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;
        CancellationTokenSource cancellationToken;
        EscanerDataContext model;

        public ScannerPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Type s = (Type)e.Parameter;

            FragmentFrame.Navigate(s);

            // Scanner
            model = new EscanerDataContext();

            if (!model.ScannerDataContext.WatcherStarted)
            {
                model.ScannerDataContext.StartScannerWatcher();
            }

            model.ClearFileList();

            rootPage.DataContext = model;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            CancelScanning();
            model.UnLoad();
        }

        private async void StartScenario_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DisplayImage.Source = null;

            rootPage.NotifyUser("Escaneando", NotifyType.StatusMessage);
            // Making scenario running as true before start of scenario
            model.ScenarioRunning = true;
            ScanToFolder(model.ScannerDataContext.CurrentScannerDeviceId, model.DestinationFolder);
        }

        private void CancelScenario_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CancelScanning();
        }

        private async void btnFinalizar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                Type tipoPage = FragmentFrame.SourcePageType;

                StorageFolder localFolder = await Utils.BaseFolder.CreateFolderAsync(tipoPage.Name.Replace("Page", string.Empty), CreationCollisionOption.OpenIfExists);
                StorageFolder destinationFolder = await localFolder.CreateFolderAsync(Guid.NewGuid().ToString(), CreationCollisionOption.GenerateUniqueName);
                StorageFolder pdfFolder = await localFolder.CreateFolderAsync("Digitalizados", CreationCollisionOption.OpenIfExists);

                // Encabezado
                StorageFile file = await destinationFolder.CreateFileAsync("Metadata.dat", CreationCollisionOption.FailIfExists);

                await FileIO.WriteTextAsync(file, model.StringifyEtiquetas + Environment.NewLine);

                IEnumerable<string> query = from a in model.FileList
                                            select a.Name;

                await FileIO.AppendLinesAsync(file, query);

                List<Stream> Fotos = new List<Stream>();
                List<IBuffer> lst = new List<IBuffer>();

                // Detalle
                foreach (FileItem fila in model.FileList)
                {
                    StorageFile filepage = await model.DestinationFolder.GetFileAsync(fila.Name);

                    IBuffer buf = await FileIO.ReadBufferAsync(filepage);

                    lst.Add(buf);
                    Fotos.Add(buf.AsStream());

                    await filepage.MoveAsync(destinationFolder);
                }

                ////////////////////
                Models.Archivos.Archivos_PostBindingModel model2 = new Models.Archivos.Archivos_PostBindingModel();
                model2.Aplicacion = model.Aplicacion;
                model2.Categoria = model.Categoria;
                model2.Etiquetas = model.Etiquetas;

                IEnumerable<IBuffer> archivos = lst;

                IBuffer archivo = await ArchivosModel.PostArchivoPdf(model2, archivos);
                byte[] st = archivo.ToArray();

                var queryTags = from a in model2.Etiquetas
                                select a.Valor;
                string NombreArchivo = string.Join("-", queryTags) + ".pdf";
                try
                {
                    await obtenerArchivoGuardado(st, pdfFolder.Path + @"\" + NombreArchivo);
                }
                catch { }
                ////////////////////

                LimpiarVista();

                rootPage.NotifyUser("El documento fue guardado exitosamente. Puede continuar.", NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(ex.Message, NotifyType.ErrorMessage);
            }
        }

        public async Task obtenerArchivoGuardado(byte[] buffer, string path)
        {
            await Task.Run(() =>
            {
                Task.Yield();
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            });
        }

        private void LimpiarVista()
        {
            model.ClearFileList();

            Type s = (Type)FragmentFrame.Content.GetType();

            FragmentFrame.Navigate(s);
        }

        private async void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            foreach (FileItem Fila in model.FileList)
            {
                StorageFile Archivo = await model.DestinationFolder.GetFileAsync(Fila.Name);
                await Archivo.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }

            LimpiarVista();

            rootPage.NotifyUser(String.Empty, NotifyType.StatusMessage);
        }

        /// <summary>
        /// Scans all the images from Feeder source of the scanner
        /// The scanning is allowed only if the selected scanner is equipped with a Feeder
        /// </summary>
        /// <param name="deviceId">scanner device id</param>
        /// <param name="folder">the folder that receives the scanned files</param>        
        public async void ScanToFolder(string deviceId, StorageFolder folder)
        {
            try
            {
                // Get the scanner object for this device id
                ImageScanner myScanner = await ImageScanner.FromIdAsync(deviceId);
                // Check to see if the use has already cancelled the scenario
                if (model.ScenarioRunning)
                {
                    if (myScanner.IsScanSourceSupported(ImageScannerScanSource.Feeder))
                    {
                        // Set MaxNumberOfPages to zero to scanning all the pages that are present in the feeder
                        myScanner.FeederConfiguration.MaxNumberOfPages = 0;
                        myScanner.FeederConfiguration.Duplex = tggDuplex.IsOn;
                        myScanner.FeederConfiguration.PageSize = (Windows.Graphics.Printing.PrintMediaSize)model.ScannerDataContext.CurrentSize;

                        cancellationToken = new CancellationTokenSource();
                        rootPage.NotifyUser("Escaneando", NotifyType.StatusMessage);

                        // Scan API call to start scanning from the Feeder source of the scanner.
                        var operation = myScanner.ScanFilesToFolderAsync(ImageScannerScanSource.Feeder, folder);
                        operation.Progress = new AsyncOperationProgressHandler<ImageScannerScanResult, uint>(ScanProgress);
                        var result = await operation.AsTask<ImageScannerScanResult, UInt32>(cancellationToken.Token);

                        // Number of scanned files should be zero here since we already processed during scan progress notifications all the files that have been scanned
                        rootPage.NotifyUser("Escaneo completado.", NotifyType.StatusMessage);
                        model.ScenarioRunning = false;
                    }
                    else
                    {
                        model.ScenarioRunning = false;
                        rootPage.NotifyUser("El escáner seleccionado indica no tener Feeder.", NotifyType.ErrorMessage);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Utils.DisplayScanCancelationMessage();
            }
            catch (Exception ex)
            {
                Utils.OnScenarioException(ex, model);
            }
        }

        /// <summary>
        /// Event Handler for progress of scanning 
        /// </summary>
        /// <param name="operation">async operation for scanning</param>
        /// <param name="numberOfFiles">The Number of files scanned so far</param>
        private async void ScanProgress(IAsyncOperationWithProgress<ImageScannerScanResult, UInt32> operation, UInt32 numberOfScannedFiles)
        {
            ImageScannerScanResult result = null;

            try
            {
                result = operation.GetResults();
            }
            catch (OperationCanceledException)
            {
                // The try catch is placed here for scenarios in which operation has already been cancelled when progress call is made
                Utils.DisplayScanCancelationMessage();
            }

            if (result != null && result.ScannedFiles.Count > 0)
            {
                IReadOnlyList<StorageFile> fileStorageList = result.ScannedFiles;
                await MainPage.Current.Dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        StorageFile file = fileStorageList[0];
                        Utils.SetImageSourceFromFile(file, DisplayImage);

                        rootPage.NotifyUser("Escaneo en progreso...", NotifyType.StatusMessage);
                        Utils.UpdateFileListData(fileStorageList, model);

                        ScanerListView.SelectedItem = model.FileList.Last();
                    }
                ));
            }
        }

        /// <summary>
        /// Cancels the current scanning task.
        /// </summary>
        void CancelScanning()
        {
            if (model.ScenarioRunning)
            {
                if (cancellationToken != null)
                {
                    cancellationToken.Cancel();
                }
                DisplayImage.Source = null;
                model.ScenarioRunning = false;
                model.ClearFileList();
            }
        }

        private async void ScanerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileItem s = ScanerListView.SelectedItem as FileItem;

            if (s != null)
            {
                StorageFile file = await model.DestinationFolder.GetFileAsync(s.Name);
                Utils.SetImageSourceFromFile(file, DisplayImage);
            }
            else
            {
                DisplayImage.Source = null;
            }
        }

        private async void btnCombinar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (model.Etiquetas.Count() > 0)
            {
                try
                {
                    model.ScenarioRunning = true;

                    Models.Etiquetas.Etiquetas_GetArchivosBindingModel mdl = new Models.Etiquetas.Etiquetas_GetArchivosBindingModel();
                    mdl.Aplicacion = model.Aplicacion;
                    mdl.Categoria = model.Categoria;
                    mdl.Etiquetas = model.Etiquetas;

                    long IDArchivo = await Models.EtiquetasModel.Get(mdl);

                    Models.Archivos.Archivos_GetBindingModel model2 = new Models.Archivos.Archivos_GetBindingModel();
                    model2.ID = IDArchivo;

                    IBuffer buffer = await Models.ArchivosModel.Get(model2);

                    Windows.Data.Pdf.PdfDocument pdf = await Windows.Data.Pdf.PdfDocument.LoadFromStreamAsync(buffer.AsStream().AsRandomAccessStream());

                    for (uint i = 0; i < pdf.PageCount; i++)
                    {
                        Windows.Data.Pdf.PdfPage page = pdf.GetPage(i);

                        StorageFile bmpFile = await model.DestinationFolder.CreateFileAsync(string.Format("Pagina {0}.bmp", i + 1), CreationCollisionOption.GenerateUniqueName);

                        IRandomAccessStream stream = await bmpFile.OpenAsync(FileAccessMode.ReadWrite);

                        await page.RenderToStreamAsync(stream);

                        await stream.FlushAsync();

                        stream.Dispose();
                        page.Dispose();

                        Utils.SetImageSourceFromFile(bmpFile, DisplayImage);

                        rootPage.NotifyUser("Apertura en progreso...", NotifyType.StatusMessage);
                        Utils.UpdateFileData(bmpFile, model);

                        ScanerListView.SelectedItem = model.FileList.Last();
                    }

                    rootPage.NotifyUser(String.Empty, NotifyType.StatusMessage);
                }
                catch (Exception)
                {
                    // No pasa nada
                }
                finally
                {
                    model.ScenarioRunning = false;
                }
            }
            else
            {
                rootPage.NotifyUser("Debe ingresar primero los datos", NotifyType.ErrorMessage);
            }
        }

        private async void ScanerListView_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Delete && ScanerListView.SelectedItem != null)
            {
                int Indice = ScanerListView.SelectedIndex;
                var archivo = (FileItem)ScanerListView.SelectedItem;

                StorageFile file = await model.DestinationFolder.GetFileAsync(archivo.Name);

                model.FileList.Remove(archivo);

                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

                if (model.FileListSize > 0)
                {
                    if (Indice >= model.FileList.Count)
                    {
                        Indice = model.FileList.Count - 1;
                    }

                    ScanerListView.SelectedItem = model.FileList.ElementAt(Indice);
                }
            }
        }

        private async void ScanerListView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            if (ScanerListView.SelectedItem != null)
            {
                int Indice = ScanerListView.SelectedIndex;
                var archivo = (FileItem)ScanerListView.SelectedItem;

                StorageFile file = await model.DestinationFolder.GetFileAsync(archivo.Name);

                model.FileList.Remove(archivo);

                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

                if (model.FileListSize > 0)
                {
                    if (Indice >= model.FileList.Count)
                    {
                        Indice = model.FileList.Count - 1;
                    }

                    ScanerListView.SelectedItem = model.FileList.ElementAt(Indice);
                }
            }
        }
    }
}
