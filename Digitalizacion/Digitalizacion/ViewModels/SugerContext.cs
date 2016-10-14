using Digitalizacion.Common;
using Digitalizacion.Models.Etiquetas;
using System.Collections.Generic;


namespace Digitalizacion.ViewModels
{
    class SugerContext : BaseViewModel
    {
        string nombre;

        public SugerContext()
        {

        }

        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                nombre = value;
                OnPropertyChanged();
                LlenarEtiquetas();
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(nombre))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.NOMBRE, nombre));

            ctx.setEtiquetas("AF", 40, lst);
        }
    }
}
