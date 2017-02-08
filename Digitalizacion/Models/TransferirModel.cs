using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Digitalizacion.Models
{
    public class TransferirModel : INotifyPropertyChanged
    {
        public string Nombre { get; set; }
        public string Alias { get; set; }
        private bool enviando;
        private string mensaje;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Even Handler for property change event
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool Enviando
        {
            get
            {
                return enviando;
            }
            set
            {
                enviando = value;
                OnPropertyChanged();
                OnPropertyChanged("Mensaje");
            }
        }

        public string Mensaje
        {
            get
            {
                if (Enviando)
                {
                    return "Enviando...";
                }
                else
                {
                    return mensaje;
                }
            }
            set
            {
                mensaje = value;
                OnPropertyChanged();
            }
        }
    }
}
