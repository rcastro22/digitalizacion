using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Alumnos;
using Digitalizacion.Models.Carreras;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Digitalizacion.ViewModels
{
    class DiplomasContext : BaseViewModel
    {
        string carnet;
        string nombre;
        string carrera;
        string diploma;
        long? solicitud;
        int paso = 1;
        ObservableCollection<Obtenercarrerasxcarnet_Result> carreras = new ObservableCollection<Obtenercarrerasxcarnet_Result>();
        ObservableCollection<Obtenertitulosxcarrera_Result> diplomas = new ObservableCollection<Obtenertitulosxcarrera_Result>();
        ObservableCollection<long> solicitudes = new ObservableCollection<long>();

        public DiplomasContext()
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

        public bool IsCarreraFill
        {
            get
            {
                return !string.IsNullOrWhiteSpace(carrera);
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
                setDiplomas();
            }
        }

        public string Diploma
        {
            get
            {
                return diploma;
            }
            set
            {
                diploma = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        public long? Solicitud
        {
            get
            {
                return solicitud;
            }
            set
            {
                solicitud = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        public bool PasoCheck
        {
            get
            {
                return !Convert.ToBoolean(paso);
            }
            set
            {
                paso = Convert.ToInt32(!value);
                OnPropertyChanged();
                setSolicitudes();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(diploma))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARNET, carnet));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARRERA, carrera));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.TITULO, diploma));

            if (solicitud.HasValue)
            {
                lst.Add(new Etiquetas((short)Common.Utils.Tags.NOSOLICITUD, Convert.ToString(solicitud)));
            }

            ctx.setEtiquetas("TI", 7, lst);
        }

        private async void setNombre()
        {
            this.carreras.Clear();
            paso = 1;

            try
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
            catch (Exception)
            {
                this.nombre = string.Empty;
            }
            finally
            {
                OnPropertyChanged("Nombre");
                OnPropertyChanged("IsAlumnoEnabled");
                OnPropertyChanged("PasoCheck");
                OnPropertyChanged("IsCarreraFill");
            }
        }

        private async void setDiplomas()
        {
            diplomas.Clear();
            solicitudes.Clear();

            try
            {
                Carreras_GetTitulosBindingModel model = new Carreras_GetTitulosBindingModel();
                model.Carrera = carrera;
                model.Titulo = 2;

                var lst = await CarrerasModel.GetTitulos(model);

                foreach (var fila in lst)
                {
                    this.diplomas.Add(fila);
                }

                Alumnos_GetSolicitudesBindingModel model2 = new Alumnos_GetSolicitudesBindingModel();
                model2.ID = carnet;
                model2.Carrera = carrera;
                model2.Tramite = 102;
                model2.Paso = paso;

                List<long> Tramites = new List<long>();
                Tramites.Add(102);
                Tramites.Add(108);
                Tramites.Add(435);
                Tramites.Add(115);
                Tramites.Add(116);
                Tramites.Add(117);
                Tramites.Add(119);


                IEnumerable<long> lst2 = null;

                foreach (long _tramite in Tramites)
                {
                    model2.Tramite = _tramite;
                    lst2 = await AlumnosModel.GetSolicitudesPendientes(model2);
                    if (lst2.Count() > 0)
                    {
                        foreach (var fila in lst2)
                        {
                            this.solicitudes.Add(fila);
                        }
                    }                    
                }
               
            }
            catch (Exception)
            {
                // No pasa nada
            }
            finally
            {
                OnPropertyChanged("IsCarreraFill");
            }
        }

        private async void setSolicitudes()
        {
            solicitudes.Clear();

            try
            {
                if (!string.IsNullOrWhiteSpace(carrera))
                {                    
                    Alumnos_GetSolicitudesBindingModel model2 = new Alumnos_GetSolicitudesBindingModel();
                    model2.ID = carnet;
                    model2.Carrera = carrera;
                    model2.Tramite = 102;
                    model2.Paso = paso;

                    List<long> Tramites = new List<long>();
                    Tramites.Add(102);
                    Tramites.Add(108);
                    Tramites.Add(435);
                    Tramites.Add(115);
                    Tramites.Add(116);
                    Tramites.Add(117);
                    Tramites.Add(119);

                    IEnumerable<long> lst2 = null;

                    foreach (long _tramite in Tramites)
                    {
                        model2.Tramite = _tramite;
                        lst2 = await AlumnosModel.GetSolicitudesPendientes(model2);
                        if (lst2.Count() > 0)
                        {
                            foreach (var fila in lst2)
                            {
                                this.solicitudes.Add(fila);
                            }
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                // No pasa nada
            }
        }

        public ObservableCollection<Obtenercarrerasxcarnet_Result> Carreras
        {
            get
            {
                return carreras;
            }
        }

        public ObservableCollection<long> Solicitudes
        {
            get
            {
                return solicitudes;
            }
        }

        public ObservableCollection<Obtenertitulosxcarrera_Result> Diplomas
        {
            get
            {
                return diplomas;
            }
        }
    }
}
