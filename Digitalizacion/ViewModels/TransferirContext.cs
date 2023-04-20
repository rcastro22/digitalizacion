using Digitalizacion.Common;
using Digitalizacion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace Digitalizacion.ViewModels
{
    class TransferirContext : BaseViewModel
    {
        Type carpeta;
        long selectedArchivos;
        bool pdfCk;
        ObservableCollection<Scenario> carpetas = new ObservableCollection<Scenario>();
        ObservableCollection<TransferirModel> archivos = new ObservableCollection<TransferirModel>();
        ObservableCollection<string> transferidos = new ObservableCollection<string>();

        public TransferirContext()
        {
            var query = from a in MainPage.Current.scenarios
                        where a.Scanner == true
                        select a;

            foreach (var fila in query)
            {
                carpetas.Add(fila);
            }
        }

        public Type Carpeta
        {
            get
            {
                return carpeta;
            }
            set
            {
                carpeta = value;
                OnPropertyChanged();
                //setArchivos();
                setArchivos();
                OnPropertyChanged("IsTransferirAllowed");
            }
        }

        public bool PdfCheck
        {
            get
            {
                return pdfCk;
            }
            set
            {
                pdfCk = value;
                OnPropertyChanged();
                if (pdfCk)
                    setArchivosPdf();
                else
                    setArchivos();
            }
        }

        public string Usuario
        {
            get
            {
                return null;
            }
        }

        public ObservableCollection<Scenario> Carpetas
        {
            get
            {
                return carpetas;
            }
        }

        public ObservableCollection<TransferirModel> Archivos
        {
            get
            {
                return archivos;
            }
        }

        public ObservableCollection<string> Transferidos
        {
            get
            {
                return transferidos;
            }
        }

        public bool IsSeleccionarAllowed
        {
            get
            {
                return Convert.ToBoolean(this.archivos.Count);
            }
        }

        public bool IsTransferirAllowed
        {
            get
            {
                return Convert.ToBoolean(this.selectedArchivos);
            }
        }

        public long SelectedArchivos
        {
            get
            {
                return selectedArchivos;
            }
            set
            {
                selectedArchivos = value;
                OnPropertyChanged();
                OnPropertyChanged("IsTransferirAllowed");
            }
        }

        public async Task<Models.Archivos.Archivos_PostBindingModel> getModel(string Carpeta)
        {
            Models.Archivos.Archivos_PostBindingModel model = new Models.Archivos.Archivos_PostBindingModel();

            try
            {
                StorageFolder localFolder = await Utils.BaseFolder.GetFolderAsync(this.carpeta.Name.Replace("Page", string.Empty));
                StorageFolder folder = await localFolder.GetFolderAsync(Carpeta);
                StorageFile metadata = await folder.GetFileAsync("Metadata.dat");

                IList<string> lines = await FileIO.ReadLinesAsync(metadata);

                JsonObject jsonObject = JsonObject.Parse(lines.First());

                model.Aplicacion = jsonObject.GetNamedString("Aplicacion");
                model.Categoria = Convert.ToInt16(jsonObject.GetNamedNumber("Categoria"));

                List<Models.Etiquetas.Etiquetas> lst = new List<Models.Etiquetas.Etiquetas>();
                JsonArray jsonArray = jsonObject.GetNamedArray("Etiquetas");

                foreach (IJsonValue jsonValue in jsonArray)
                {
                    Models.Etiquetas.Etiquetas tag = new Models.Etiquetas.Etiquetas(jsonValue);

                    lst.Add(tag);
                }

                model.Etiquetas = lst;
            }
            catch (Exception)
            {
                throw;
            }

            return model;
        }

        public async Task<IEnumerable<IBuffer>> getFolderFiles(string Carpeta)
        {
            List<IBuffer> lst = new List<IBuffer>();

            try
            {
                StorageFolder localFolder = await Utils.BaseFolder.GetFolderAsync(this.carpeta.Name.Replace("Page", string.Empty));
                StorageFolder folder = await localFolder.GetFolderAsync(Carpeta);
                StorageFile metadata = await folder.GetFileAsync("Metadata.dat");

                IList<string> lines = await FileIO.ReadLinesAsync(metadata);

                for (int i = 1; i < lines.Count; i++)
                {
                    StorageFile file = await folder.GetFileAsync(lines.ElementAt(i));

                    IBuffer buffer = await FileIO.ReadBufferAsync(file);

                    lst.Add(buffer);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return lst;
        }

        public async Task<IEnumerable<IBuffer>> getFolderFilesPdf(string Carpeta, string Archivo)
        {
            List<IBuffer> lst = new List<IBuffer>();

            try
            {
                StorageFolder localFolder = await Utils.BaseFolder.GetFolderAsync(this.carpeta.Name.Replace("Page", string.Empty));
                StorageFolder folder = await localFolder.GetFolderAsync(Carpeta);
                //StorageFile metadata = await folder.GetFileAsync("Metadata.dat");
                StorageFile metadata = await folder.GetFileAsync(Archivo);

                IBuffer buffer = await FileIO.ReadBufferAsync(metadata);

                lst.Add(buffer);                
            }
            catch (Exception)
            {
                throw;
            }

            return lst;
        }

        private async void setArchivos()
        {
            this.archivos.Clear();

            try
            {
                StorageFolder localFolder = await Utils.BaseFolder.GetFolderAsync(this.carpeta.Name.Replace("Page", string.Empty));
                IReadOnlyList<StorageFolder> destinationFolders = await localFolder.GetFoldersAsync();

                foreach (StorageFolder folder in destinationFolders)
                {
                    try
                    {
                        StorageFile file = await folder.GetFileAsync("Metadata.dat");

                        IList<string> lines = await FileIO.ReadLinesAsync(file);

                        JsonObject jsonObject = JsonObject.Parse(lines.First());
                        JsonArray jsonArray = jsonObject.GetNamedArray("Etiquetas");

                        List<Models.Etiquetas.Etiquetas> lst = new List<Models.Etiquetas.Etiquetas>();

                        foreach (IJsonValue fila in jsonArray)
                        {
                            Models.Etiquetas.Etiquetas tag = new Models.Etiquetas.Etiquetas(fila);

                            lst.Add(tag);
                        }

                        TransferirModel c = new TransferirModel()
                        {
                            Nombre = folder.Name,
                            Alias = String.Join("-", lst.Select(p => p.Valor)),
                            Enviando = false,
                            Mensaje = null
                        };

                        this.archivos.Add(c);
                    }
                    catch (Exception)
                    {
                        // No pasa nada
                    }
                }
            }
            catch (Exception)
            {
                // No pasa nada
            }
            finally
            {
                OnPropertyChanged("IsSeleccionarAllowed");
            }
        }


        private async void setArchivosPdf()
        {
            this.archivos.Clear();

            try
            {
                StorageFolder localFolder = await Utils.BaseFolder.GetFolderAsync(this.carpeta.Name.Replace("Page", string.Empty));
                IReadOnlyList<StorageFolder> destinationFolders = await localFolder.GetFoldersAsync();

                foreach (StorageFolder folder in destinationFolders)
                {
                    try
                    {
                        StorageFile file = await folder.GetFileAsync("Metadata.dat");

                        List<string> fileTypeFilter = new List<string>();
                        fileTypeFilter.Add(".pdf");
                        //fileTypeFilter.Add(".jpg");
                        //fileTypeFilter.Add(".png");
                        //fileTypeFilter.Add(".bmp");
                        //fileTypeFilter.Add(".gif");
                        var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
                        var query = KnownFolders.PicturesLibrary.CreateFileQueryWithOptions(queryOptions);
                        IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();
                        // Process results
                        int counter = 0;
                        StorageFile filePdf = null;
                        foreach (StorageFile fileSearch in fileList)
                        {
                            counter++;
                            if (counter == 1)
                                filePdf = fileSearch;
                            else
                                break;
                        }
                        //StorageFile filePdf = await folder.GetFileAsync("Archivo.pdf");

                        IList<string> lines = await FileIO.ReadLinesAsync(file);

                        JsonObject jsonObject = JsonObject.Parse(lines.First());
                        JsonArray jsonArray = jsonObject.GetNamedArray("Etiquetas");

                        List<Models.Etiquetas.Etiquetas> lst = new List<Models.Etiquetas.Etiquetas>();

                        foreach (IJsonValue fila in jsonArray)
                        {
                            Models.Etiquetas.Etiquetas tag = new Models.Etiquetas.Etiquetas(fila);

                            lst.Add(tag);
                        }

                        TransferirModel c = new TransferirModel()
                        {
                            Nombre = folder.Name,
                            //Alias = String.Join("-", lst.Select(p => p.Valor)),
                            Alias = String.Format("{0} ({1})",filePdf.Name,String.Join("-", lst.Select(p => p.Valor))),
                            Enviando = false,
                            Mensaje = null
                        };

                        this.archivos.Add(c);
                    }
                    catch (Exception)
                    {
                        // No pasa nada
                    }
                }
            }
            catch (Exception)
            {
                // No pasa nada
            }
            finally
            {
                OnPropertyChanged("IsSeleccionarAllowed");
            }
        }

        public async Task RemoverFolder(TransferirModel item)
        {
            this.archivos.Remove(item);
            this.transferidos.Add(item.Nombre);

            StorageFolder localFolder = await Utils.BaseFolder.GetFolderAsync(this.carpeta.Name.Replace("Page", string.Empty));
            StorageFolder folder = await localFolder.GetFolderAsync(item.Nombre);
            await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);

            OnPropertyChanged("IsSeleccionarAllowed");
        }
    }
}
