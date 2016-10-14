using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class BecasContext : BaseViewModel
    {
        string convenio;

        public BecasContext()
        {

        }

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
                LlenarEtiquetas();
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

            lst.Add(new Etiquetas((short)Common.Utils.Tags.NOMBRE, convenio));

            ctx.setEtiquetas("AF", 13, lst);
        }
    }
}
