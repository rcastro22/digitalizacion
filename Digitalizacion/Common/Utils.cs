using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Digitalizacion.Common
{
    /// <summary>
    /// An bastract class containing utility functions 
    /// </summary
    class Utils
    {
        static Utils()
        {
        }

        /// <summary>
        /// Returns a Bitmap image for a give random access stream 
        /// </summary>
        /// <param name="stream">Random access stream for which the bitmap needs to be generated</param>
        /// <return Type="BitmapImage">Bitmap for the given stream</return>
        static async public Task<BitmapImage> GetImageFromFile(IRandomAccessStream stream)
        {
            BitmapImage bmp = new BitmapImage();

            await bmp.SetSourceAsync(stream);
            return bmp;
        }

        /// <summary>
        /// Set the image source that is given to a bitmap file that is generated from the given storage file
        /// </summary>
        /// <param name="file">Storage file used to generate a bitmap image that is assigned as source to the given image object</param>
        /// <param name="img">Image for which the source needs to be set to the generated bitmap from given storage file</param>
        static public async void SetImageSourceFromFile(StorageFile file, Image img)
        {
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
            BitmapImage bitmap = await GetImageFromFile(stream);

            if ((bitmap.PixelHeight > img.Height) || (bitmap.PixelWidth > img.Width))
            {
                img.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
            }
            else
            {
                img.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
            }

            img.Source = bitmap;
        }

        /// <summary>
        /// Set the image source that is given to a bitmap file that is generated from the given random access stream
        /// </summary>
        /// <param name="stream">Random access stream used to generate a bitmap image that is assigned as source to the given image object</param>
        /// <param name="img">Image for which the source need to be set to the generated bitmap from given stream</param>
        static async public void SetImageSourceFromStream(IRandomAccessStream stream, Image img)
        {
            BitmapImage bitmap = await GetImageFromFile(stream);

            if ((bitmap.PixelHeight > img.Height) || (bitmap.PixelWidth > img.Width))
            {
                img.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
            }
            else
            {
                img.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
            }

            img.Source = bitmap;
        }

        /// <summary>
        /// Display the image of the first scanned file and output the corresponding message
        /// </summary>
        /// <param name="FileStorageList">List of storage files create by the Scan Runtime API for all the scanned images given by the scanner</param>
        /// <param name="img">Image for which the source need to be set to the generated bitmap from first storage file</param>
        public static void DisplayImageAndScanCompleteMessage(IReadOnlyList<StorageFile> FileStorageList, Image img)
        {
            StorageFile file = FileStorageList[0];
            SetImageSourceFromFile(file, img);
            if (FileStorageList.Count > 1)
            {
                MainPage.Current.NotifyUser("Escaneo completado.", NotifyType.StatusMessage);
            }
            else
            {
                MainPage.Current.NotifyUser("Escaneo completado." + file.Name, NotifyType.StatusMessage);
            }
        }

        /// <summary>
        /// Add the file names of the scanned files to the list of file names that will be displayed to the user
        /// </summary>
        /// <param name="FileStorageList">List of storage files create by the Scan Runtime API for all the scanned images given by the scanner</param>
        /// <param name="Data">ModelDataContext object which contains the current data Context </param>
        public static void UpdateFileListData(IReadOnlyList<StorageFile> FileStorageList, EscanerDataContext Data)
        {
            // populate list with the names of the files that are scanned			
            for (int i = 0; i < FileStorageList.Count; i++)
            {
                Data.AddToFileList(FileStorageList[i]);
            }
        }

        public static void UpdateFileData(StorageFile FileStorageList, EscanerDataContext Data)
        {
            // populate list with the names of the files that are scanned			

            Data.AddToFileList(FileStorageList);
        }

        /// <summary>
        /// Displays an error message when an exception is thrown during the scenario and set ScenarioRunning of the Data context to false
        /// </summary>
        public static void OnScenarioException(Exception e, EscanerDataContext Data)
        {
            DisplayExceptionErrorMessage(e);
            Data.ScenarioRunning = false;
        }

        /// <summary>
        /// Displays an error message when an exception is thrown during the scenario
        /// </summary>
        public static void DisplayExceptionErrorMessage(Exception e)
        {
            MainPage.Current.NotifyUser("Ha ocurrido un problema. Mensaje: " + e.Message, NotifyType.ErrorMessage);
        }

        /// <summary>
        /// Displays cancellation message when user cancels scanning
        /// </summary>
        public static void DisplayScanCancelationMessage()
        {
            MainPage.Current.NotifyUser("Escanero ha sido cancelado.", NotifyType.StatusMessage);
        }

        public enum Tags
        {
            CARNET = 1, CARRERA = 2, NORESOLUCION = 3, NOSOLICITUD = 4, TITULO = 5,
            DIRECTOR = 6, CORRELATIVO = 7, REQUISITO = 8, CODPERS = 9, NOMBRE = 10,
            PENSUM = 11, FACULTAD = 12, FECHA = 13, CODMOTVO = 14, RECIBO = 15,
            COMENTARIOS = 17, BOLETA = 18, HORARIO = 19, FECHAIMP = 20,
            PERIODO = 21, DPI = 22, TICKET = 23, PEOPLE = 24, FIRMADO = 25,
            DYNAMICS = 26, AULAS = 27, GES_PK = 28, EMPLID = 29, CONVENIO = 30, ADENDA = 33,
            NOACTA = 41, CATEGORIA = 42, NOREGISTRO = 60
        }

        public static StorageFolder BaseFolder = KnownFolders.PicturesLibrary;
    }
}
