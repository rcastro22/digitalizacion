using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Empleados;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class AdministrativaContext : BaseViewModel
    {
        string emplid;
        string nombre;

        public AdministrativaContext()
        {

        }

        public string EmplId
        {
            get
            {
                return emplid;
            }
            set
            {
                emplid = value;
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

            if (string.IsNullOrEmpty(emplid))
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.DYNAMICS, emplid));

            ctx.setEtiquetas("PR", 36, lst);
        }

        private async void setNombre()
        {
            try
            {
                Empleados_GetBindingModel model = new Empleados_GetBindingModel();
                model.ID = emplid;

                this.nombre = await EmpleadosModel.GetNombre(model);
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
