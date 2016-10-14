using Digitalizacion.Common;
using Digitalizacion.Models;
using Digitalizacion.Models.Alumnos;
using Digitalizacion.Models.Carreras;
using Digitalizacion.Models.Etiquetas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Digitalizacion.ViewModels
{
    class PergaminoContext : BaseViewModel
    {
        string caja;
        string pergamino;

        Boolean etiquetasFill = false;
        Boolean vefCheck = false;
        Boolean existsFile = false;
        EscanerDataContext model;

        ObservableCollection<Obtenercarrerasxcarnet_Result> carreras = new ObservableCollection<Obtenercarrerasxcarnet_Result>();

        public PergaminoContext()
        {

        }

        public bool IsEtiquetasFill
        {
            get
            {
                return etiquetasFill;
            }
        }

        public bool IsCajaFill
        {
            get
            {
                return !(string.IsNullOrEmpty(caja) || string.IsNullOrEmpty(pergamino));
            }
        }

        public string IsFileExist
        {
            get
            {
                if (existsFile)
                    return "Visible";
                else
                    return "Collapsed";
            }
        }

        public string Caja
        {
            get
            {
                return caja;
            }
            set
            {
                caja = value;
                OnPropertyChanged();
                OnPropertyChanged("IsCajaFill");
            }
        }

        public string Pergamino
        {
            get
            {
                return pergamino;
            }
            set
            {
                pergamino = value;
                OnPropertyChanged();
                OnPropertyChanged("IsCajaFill");
            }
        }
                                   
    }
}
