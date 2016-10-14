using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Digitalizacion.ViewModels
{
    class LoginContext : BaseViewModel
    {
        string usuario;
        string contrasena;

        public string Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                usuario = value;
                OnPropertyChanged();
                OnPropertyChanged("IsIngresarEnabled");
            }
        }

        public string Contrasena
        {
            get
            {
                return contrasena;
            }
            set
            {
                contrasena = value;
                OnPropertyChanged();
                OnPropertyChanged("IsIngresarEnabled");
            }
        }

        public bool IsIngresarEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(contrasena);
            }
        }
    }
}
