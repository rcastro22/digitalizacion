using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Alumnos;
using Digitalizacion.Models.Carreras;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Digitalizacion.ViewModels
{
    class ExpedienteContext : BaseViewModel
    {
        string carnet;
        string nombre;
        string carrera;
        string caja;

        Boolean etiquetasFill = false;
        Boolean vefCheck = false;
        Boolean existsFile = false;
        EscanerDataContext model;

        ObservableCollection<Obtenercarrerasxcarnet_Result> carreras = new ObservableCollection<Obtenercarrerasxcarnet_Result>();

        public ExpedienteContext()
        {

        }

        public string Carnet
        {
            get
            {
                return carnet;
            }
            set
            {
                carnet = value;
                OnPropertyChanged();                
                setNombre();
            }
        }        

        public string Nombre
        {
            get
            {
                return nombre;
            }
        }

        public bool IsAlumnoEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(nombre);
            }
        }

        public bool IsEtiquetasFill
        {
            get
            {
                return etiquetasFill;
            }
        }

        public bool IsCajaFill
        {
            get
            {
                return !string.IsNullOrEmpty(caja);
            }
        }

        public string IsFileExist
        {
            get
            {
                if (existsFile)
                    return "Visible";
                else
                    return "Collapsed";
            }
        }

        public string Caja
        {
            get
            {
                return caja;
            }
            set
            {
                caja = value;
                OnPropertyChanged();
                OnPropertyChanged("IsCajaFill");
            }
        }

        public string Carrera
        {
            get
            {
                return carrera;
            }
            set
            {
                carrera = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        public bool ValidExistsFile
        {
            get
            {
                return vefCheck;
            }
            set
            {
                vefCheck = value;
                OnPropertyChanged();                
                verifyExistsFile();                                       
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(carrera))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Models.Etiquetas.Etiquetas> lst = new List<Models.Etiquetas.Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARNET, carnet));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARRERA, carrera));

            ctx.setEtiquetas("EX", 1, lst);

            etiquetasFill = true;
            OnPropertyChanged("IsEtiquetasFill");
            model = ctx;           
        }

        private async void setNombre()
        {
            this.carreras.Clear();
            etiquetasFill = false;
            existsFile = false;
            vefCheck = false;

            try
            {
                if (carnet != String.Empty)
                {
                    Alumnos_GetBindingModel model = new Alumnos_GetBindingModel();
                    model.ID = carnet;

                    this.nombre = await AlumnosModel.GetNombre(model);

                    var lst = await CarrerasModel.Get(model);

                    foreach (var fila in lst)
                    {
                        this.carreras.Add(fila);
                    }
                }
                else
                {
                    this.nombre = String.Empty;
                }
            }
            catch (Exception ex)
            {
                this.nombre = string.Empty;
            }
            finally
            {
                OnPropertyChanged("Nombre");
                OnPropertyChanged("IsAlumnoEnabled");

                OnPropertyChanged("IsEtiquetasFill");
                OnPropertyChanged("ValidExistsFile");
                OnPropertyChanged("IsFileExist");
            }
        }

        private async void verifyExistsFile()
        {
            if (vefCheck)
            {
                if (model.Etiquetas.Count() > 0)
                {
                    long IDArchivo = 0;
                    string cajaGet;
                    try
                    {
                        model.ScenarioRunning = true;

                        Models.Etiquetas.Etiquetas_GetArchivosBindingModel mdl = new Models.Etiquetas.Etiquetas_GetArchivosBindingModel();
                        mdl.Aplicacion = model.Aplicacion;
                        mdl.Categoria = model.Categoria;
                        mdl.Etiquetas = model.Etiquetas;

                        IDArchivo = await Models.EtiquetasModel.Get(mdl);

                        Models.Archivos.Archivos_GetEtiquetasBindingModel modelArchivo = new Models.Archivos.Archivos_GetEtiquetasBindingModel();
                        modelArchivo.IDArchivo = IDArchivo;

                        cajaGet = await Models.ArchivosModel.GetCaja(modelArchivo);

                        if (cajaGet == "null") {
                            this.caja = string.Empty;
                        }
                        else {
                            this.caja = cajaGet.Replace("\"","");
                        }
                        
                    }
                    catch (Exception)
                    {
                        // No pasa nada
                    }
                    finally
                    {
                        model.ScenarioRunning = false;

                        if (IDArchivo != 0)
                        {
                            existsFile = true;
                        }
                        else
                        {
                            existsFile = false;
                            vefCheck = false;
                            MainPage.Current.NotifyUser("No se ha encontrado archivo de expediente guardado", NotifyType.ErrorMessage);
                        }                        
                    }
                }
                else
                {
                    MainPage.Current.NotifyUser("Debe ingresar primero los datos", NotifyType.ErrorMessage);
                }
            }
            else
            {
                existsFile = false;
            }
            OnPropertyChanged("IsFileExist");
            OnPropertyChanged("ValidExistsFile");
            OnPropertyChanged("Caja");
            OnPropertyChanged("IsCajaFill");
        }

        public ObservableCollection<Obtenercarrerasxcarnet_Result> Carreras
        {
            get
            {
                return carreras;
            }
        }
    }
}
