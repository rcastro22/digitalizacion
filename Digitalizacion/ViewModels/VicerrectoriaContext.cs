using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class VicerrectoriaContext : BaseViewModel
    {
        string convenio;

        public string Convenio
        {
            get
            {
                return convenio;
            }
            set
            {
                convenio = value;
                OnPropertyChanged();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(convenio))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARNET, convenio));

            ctx.setEtiquetas("TI", 6, lst);
        }
    }
}
