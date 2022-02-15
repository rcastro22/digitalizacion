using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class DiplomaEventosContext : BaseViewModel
    {
        string noregistro;

        public DiplomaEventosContext()
        {

        }

        public string Noregistro
        {
            get
            {
                return noregistro;
            }
            set
            {
                noregistro = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(noregistro))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.NOREGISTRO, noregistro));

            ctx.setEtiquetas("EV", 9, lst);
        }
    }
}
