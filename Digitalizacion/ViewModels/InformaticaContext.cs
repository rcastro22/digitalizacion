﻿using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Docentes;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;

namespace Digitalizacion.ViewModels
{
    class InformaticaContext : BaseViewModel
    {
        string codpers;
        long? correlativo;
        string nombre;

        public InformaticaContext()
        {

        }

        public string Codpers
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

            if (string.IsNullOrWhiteSpace(codpers) || correlativo.HasValue == false)
            {
                ctx.ClearEtiquetas();

                return;
            }

            List<Etiquetas> lst = new List<Etiquetas>();

            lst.Add(new Etiquetas((short)Common.Utils.Tags.CODPERS, codpers));
            lst.Add(new Etiquetas((short)Common.Utils.Tags.NOSOLICITUD, Convert.ToString(correlativo)));

            ctx.setEtiquetas("IT", 8, lst);
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
