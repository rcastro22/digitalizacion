using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class ResolucionesContext : BaseViewModel
    {
        DateTime? fecha;
        long? correlativo;
        string[] number = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };

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
                LlenarEtiquetas();
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

            if (string.IsNullOrWhiteSpace(fecha.ToString()))
            {
                fecha = DateTime.Today;
            }

            if (!correlativo.HasValue)
            {
                ctx.ClearEtiquetas();

                return;
            } 

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.NORESOLUCION, Convert.ToString(fecha.Value.Year) + "-" + Convert.ToString(number[Convert.ToInt32(fecha.Value.Month)-1]) + "-" + Convert.ToString(correlativo)));

            ctx.setEtiquetas("RS", 3, lst);
        }
    }
}
