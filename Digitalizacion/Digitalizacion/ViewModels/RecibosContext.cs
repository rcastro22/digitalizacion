using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class RecibosContext : BaseViewModel
    {
        DateTime fecha;
        long? correlativo;

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

        public long? Correlativo
        {
            get
            {
                return correlativo;
            }
            set
            {
                correlativo = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (correlativo.HasValue == false)
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.FECHA, Convert.ToString(fecha)));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.CORRELATIVO, Convert.ToString(correlativo)));

            ctx.setEtiquetas("BO", 18, lst);
        }
    }
}
