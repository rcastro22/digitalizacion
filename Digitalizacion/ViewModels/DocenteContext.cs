using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Docentes;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class DocenteContext : BaseViewModel
    {
        string codpers;
        string nombre;

        public DocenteContext()
        {

        }

        public string CodPers
        {
            get
            {
                return codpers;
            }
            set
            {
                codpers = value;
                OnPropertyChanged();
                setNombre();
                LlenarEtiquetas();
            }
        }

        public string Nombre
        {
            get
            {
                return nombre;
            }
        }

        void LlenarEtiquetas()
        {
            EscanerDataContext ctx = (EscanerDataContext)MainPage.Current.DataContext;

            if (string.IsNullOrEmpty(codpers))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CARNET, codpers));

            ctx.setEtiquetas("DO", 11, lst);
        }

        private async void setNombre()
        {
            try
            {
                Docentes_GetBindingModel model = new Docentes_GetBindingModel();
                model.ID = codpers;

                this.nombre = await DocentesModel.GetNombre(model);
            }
            catch (Exception)
            {
                this.nombre = string.Empty;
            }
            finally
            {
                OnPropertyChanged("Nombre");
            }
        }
    }
}
