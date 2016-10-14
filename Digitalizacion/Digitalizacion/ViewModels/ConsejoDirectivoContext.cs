using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class ConsejoDirectivoContext : BaseViewModel
    {
        DateTime fecha = DateTime.Now;
        DateTime anio = DateTime.Now;
        long? noacta;

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

        public DateTime Anio
        {
            get
            {
                return anio;
            }
            set
            {
                anio = value;
                OnPropertyChanged();
            }
        }

        public long? NoActa
        {
            get
            {
                return noacta;
            }
            set
            {
                noacta = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (noacta.HasValue == false)
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.FECHA, Convert.ToString(fecha)));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.NOACTA, Convert.ToString(noacta)+"-"+Convert.ToString(anio.Year)));

            ctx.setEtiquetas("AC", 58, lst);
        }
    }
}
