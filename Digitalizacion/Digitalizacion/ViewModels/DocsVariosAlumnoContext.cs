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
    class DocsVariosAlumnoContext : BaseViewModel
    {
        string carnet;
        string nombre;
        string carrera;
        string comentario;

        ObservableCollection<Obtenercarrerasxcarnet_Result> carreras = new ObservableCollection<Obtenercarrerasxcarnet_Result>();


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
                LlenarEtiquetas();
            }
        }

        public string Comentarios
        {
            get
            {
                return comentario;
            }
            set
            {
                comentario = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }


        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(carrera) || string.IsNullOrEmpty(carnet) || string.IsNullOrEmpty(comentario))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARNET, carnet));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARRERA, carrera));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.COMENTARIOS, comentario));

            ctx.setEtiquetas("DV", 51, lst);
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


        public ObservableCollection<Obtenercarrerasxcarnet_Result> Carreras
        {
            get
            {
                return carreras;
            }
        }

    }
}
