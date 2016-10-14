using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Alumnos;
using Digitalizacion.Models.Carreras;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Digitalizacion.ViewModels
{
    class AdmisionContext : BaseViewModel
    {
        string carnet;
        string nombre;
        string carrera;
        string requisito;
        ObservableCollection<Obtenercarrerasxcarnet_Result> carreras = new ObservableCollection<Obtenercarrerasxcarnet_Result>();
        ObservableCollection<Requisitosadmision_Result> requisitos = new ObservableCollection<Requisitosadmision_Result>();

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
                setRequisitos();
            }
        }

        public string Requisito
        {
            get
            {
                return requisito;
            }
            set
            {
                requisito = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(requisito) || string.IsNullOrEmpty(carrera) || string.IsNullOrEmpty(carnet))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARNET, carnet));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARRERA, carrera));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.REQUISITO, requisito));

            ctx.setEtiquetas("EX", 10, lst);
        }

        private async void setNombre()
        {
            this.carreras.Clear();

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
            }
        }

        private async void setRequisitos()
        {
            requisitos.Clear();

            try
            {
                var lst = await AlumnosModel.GetRequisitosAdmision();

                foreach (var fila in lst)
                {
                    this.requisitos.Add(fila);
                }

                Alumnos_GetSolicitudesBindingModel model2 = new Alumnos_GetSolicitudesBindingModel();
                model2.ID = carnet;
                model2.Carrera = carrera;
                model2.Tramite = 102;
            }
            catch (Exception ex)
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

        public ObservableCollection<Requisitosadmision_Result> Requisitos
        {
            get
            {
                return requisitos;
            }
        }
    }
}
