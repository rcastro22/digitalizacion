using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Archivos;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Digitalizacion.ViewModels
{
    class AdendaContext : BaseViewModel
    {
        DateTime? fecha;
        long? numero;
        string resolucionBusqueda;
        string resolucion;
        ObservableCollection<ObtenerResolucionesCompletas_Result> resoluciones = new ObservableCollection<ObtenerResolucionesCompletas_Result>();

        public DateTime? Fecha
        {
            get
            {
                return fecha;
            }
            set
            {
                fecha = value;
                OnPropertyChanged();
                //LlenarEtiquetas();
            }
        }

        public long? Numero
        {
            get
            {
                return numero;
            }
            set
            {
                numero = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        public string ResolucionBusqueda
        {
            get
            {
                return resolucionBusqueda;
            }
            set
            {
                resolucionBusqueda = value;
                OnPropertyChanged();
                //LlenarEtiquetas();
                setResoluciones();
            }
        }

        public string Resolucion
        {
            get
            {
                return resolucion;
            }
            set
            {
                resolucion = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrWhiteSpace(fecha.ToString()))
            {
                fecha = DateTime.Today;
            }

            if (!numero.HasValue || string.IsNullOrEmpty(resolucion))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.NORESOLUCION, Convert.ToString(resolucion)));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.ADENDA, Convert.ToString(fecha.Value.Year)+"-"+Convert.ToString(numero)));
            

            ctx.setEtiquetas("RS", 44, lst);
        }

        private async void setResoluciones() {
            resoluciones.Clear();

            try {
                Archivos_GetResolucionesBindingModel model = new Archivos_GetResolucionesBindingModel();
                model.resolucion = Convert.ToString(resolucionBusqueda);

                var lst = await ArchivosModel.GetResoluciones(model);

                foreach (var fila in lst)
                {
                    this.resoluciones.Add(fila);
                }


            }
            catch (Exception) { }
        }

        public ObservableCollection<ObtenerResolucionesCompletas_Result> Resoluciones
        {
            get
            {
                return resoluciones;
            }
        }
    }
}
