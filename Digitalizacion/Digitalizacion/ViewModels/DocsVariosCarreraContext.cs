using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Carreras;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Digitalizacion.ViewModels
{
    class DocsVariosCarreraContext : BaseViewModel
    {
        string carrera;
        string buscarcarrera;
        string comentario;

        ObservableCollection<Obtenercarrerasxcarnet_Result> carreras = new ObservableCollection<Obtenercarrerasxcarnet_Result>();

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

        public string BuscarCarrera
        {
            get
            {
                return buscarcarrera;
            }
            set
            {
                buscarcarrera = value;
                OnPropertyChanged();
                setCarreras();
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

            if (string.IsNullOrEmpty(carrera) || string.IsNullOrEmpty(comentario))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARRERA, carrera));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.COMENTARIOS, comentario));

            ctx.setEtiquetas("DV", 50, lst);
        }

        private async void setCarreras()
        {
            this.carreras.Clear();

            try
            {
                Carreras_GetCarrerasBindingModel model = new Carreras_GetCarrerasBindingModel();
                model.BuscarCarrera = buscarcarrera;
                var lst = await CarrerasModel.GetNombreCarreras(model);

                foreach (var fila in lst)
                {
                    this.carreras.Add(fila);
                }
            }
            catch (Exception)
            {
                
            }

        }

        public ObservableCollection<Obtenercarrerasxcarnet_Result> Carreras
        {
            get
            {
                return carreras;
            }
        }


        private async void btnCombinar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
        }

    }
}
