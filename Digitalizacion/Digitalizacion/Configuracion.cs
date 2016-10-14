using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Digitalizacion
{
    public partial class LoginPage : Page
    {
        private string GoogleClientID = "263306096279-l51s3h6j1q0q1gh21et8s3jutjlejvtv.apps.googleusercontent.com";
        private string GoogleCallbackUrl = "urn:ietf:wg:oauth:2.0:oob";
    }

    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "Digitalización";
        internal bool Dismissed;
        internal Rect SplashImageRect;

        public List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Number=11,Title = "Resoluciones", ClassType = typeof(Views.ResolucionesPage), Scanner = true },
            new Scenario() { Number=7, Title = "Expedientes",  ClassType = typeof(Views.ExpedientePage),   Scanner = true },
            new Scenario() { Number=7, Title = "Expedientes - Clasificar",  ClassType = typeof(Views.PergaminoPage),   Scanner = false },
            new Scenario() { Number=9, Title = "Informática",  ClassType = typeof(Views.InformaticaPage),  Scanner = true },
            new Scenario() { Number=6, Title = "Diplomas",     ClassType = typeof(Views.DiplomasPage),     Scanner = true },
            new Scenario() { Number=12,Title = "Títulos",      ClassType = typeof(Views.TitulosPage),     Scanner = true },
            new Scenario() { Number=5, Title = "Curriculum docente",     ClassType = typeof(Views.DocentePage),  Scanner = true },
            new Scenario() { Number=10,Title = "Requisitos de admisión", ClassType = typeof(Views.AdmisionPage), Scanner = true },
            new Scenario() { Number=8, Title = "Ficha administrativa",   ClassType = typeof(Views.AdministrativaPage), Scanner = true },
            new Scenario() { Number=4, Title = "Convenios (becas)",      ClassType = typeof(Views.BecasPage),      Scanner = true },
            new Scenario() { Number=3, Title = "Crédito educativo (becas)", ClassType = typeof(Views.AcademicosPage), Scanner = true},
            new Scenario() { Number=17,Title = "Becas Dr. Suger", ClassType = typeof(Views.SugerPage), Scanner = true},
            new Scenario() { Number=1, Title = "Boletas de pago (recibos)", ClassType = typeof(Views.RecibosPage), Scanner = true},
            new Scenario() { Number=2, Title = "Código de barras", ClassType = typeof(Views.CodigoBarrasPage),     Scanner = true },
            new Scenario() { Number=13,Title = "Vicerrectoría",    ClassType = typeof(Views.VicerrectoriaPage),   Scanner = true },
            new Scenario() { Number=18,Title = "Adenda",           ClassType = typeof(Views.AdendaPage), Scanner= true},
            new Scenario() { Number=19,Title = "Documentos Carrera", ClassType = typeof(Views.DocsVariosCarreraPage), Scanner = true},
            new Scenario() { Number=20,Title = "Documentos Alumno por Carrera", ClassType = typeof(Views.DocsVariosAlumnoPage), Scanner = true},
            new Scenario() { Number=21,Title = "Actas de Consejo Directivo", ClassType = typeof(Views.ConsejoDirectivoPage), Scanner = true},
            new Scenario() { Number=22,Title = "Convenios Generales", ClassType = typeof(Views.ConveniosPage), Scanner = true},
            new Scenario() { Number=14,Title = "Transferir documentos", ClassType = typeof(TransferirPage), Scanner = false },            
        };

        public void SetExtendedSplashInfo(Rect splashRect, bool dismissStat)
        {
            SplashImageRect = splashRect;
            Dismissed = dismissStat;
        }
    }

    public class Scenario
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public bool Scanner { get; set; }
        public Type ClassType { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    public class MainPageSizeChangedEventArgs : EventArgs
    {
        private double width;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
