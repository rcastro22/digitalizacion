using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Etiquetas;
using Digitalizacion.Models.Facultades;
using Digitalizacion.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Digitalizacion.ViewModels
{
    class ConveniosContext : BaseViewModel
    {
        DateTime fecha = DateTime.Now;
        string nombre;
        string facultad;
        string convenio;
        ObservableCollection<Facultad_Result> facultades = new ObservableCollection<Facultad_Result>();
        ObservableCollection<TipoConvenio_Result> tipoConvenios = new ObservableCollection<TipoConvenio_Result>();

        public DateTime Fecha
        {
            get
            {
                return fecha;
            }
            set
            {
                fecha = value;
                OnPropertyChanged();                
            }
        }

        public String Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
                OnPropertyChanged();
                setTipoConvenio();
                setFacultades();
                LlenarEtiquetas();
            }
        }

        public String Facultad
        {
            get
            {
                return facultad;
            }
            set
            {
                facultad = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        public String Convenio
        {
            get
            {
                return convenio;
            }
            set
            {
                convenio = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(convenio) || string.IsNullOrEmpty(facultad))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.FACULTAD, Convert.ToString(facultad)));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.NOMBRE, Convert.ToString(nombre)));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.CATEGORIA, Convert.ToString(convenio)));            
            lst.Add(new Etiquetas((short)Common.Utils.Tags.FECHA, Convert.ToString(fecha.Day)+ Convert.ToString(fecha.Month)+ Convert.ToString(fecha.Year)));            

            ctx.setEtiquetas("CO", 17, lst);
        }

        private async void setFacultades()
        {
            facultades.Clear();

            try
            {
                var lst = await FacultadesModel.Get();

                foreach (var fila in lst)
                {
                    this.facultades.Add(fila);
                }
            }
            catch (Exception) { }
        }

        private async void setTipoConvenio()
        {
            tipoConvenios.Clear();

            try
            {
                var lst = await ConveniosModel.Get();

                foreach (var fila in lst)
                {
                    this.tipoConvenios.Add(fila);
                }
            }
            catch (Exception) { }
        }

        public ObservableCollection<Facultad_Result> Facultades
        {
            get
            {
                return facultades;
            }
        }

        public ObservableCollection<TipoConvenio_Result> Convenios
        {
            get
            {
                return tipoConvenios;
            }
        }
    }
}
