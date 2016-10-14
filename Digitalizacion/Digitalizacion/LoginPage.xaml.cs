using System;
using Windows.ApplicationModel;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Digitalizacion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();

            // get the app version number
            PackageVersion pv = Package.Current.Id.Version;

            txtVersion.Text = $"{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}";
        }

        private async void Launch_Click(object sender, RoutedEventArgs e)
        {
            Launch.IsEnabled = false;

            this.NotifyUser("Autenticando", NotifyType.StatusMessage);

            try
            {
                Models.Usuarios.Usuarios_PostBindingModel model = new Models.Usuarios.Usuarios_PostBindingModel();
                model.Usuario = txtUsuario.Text.ToUpper();
                model.Contrasena = txtContrasena.Password;

                Common.APIClient.Usuario = model.Usuario;

                bool Autenticado = await Models.UsuariosModel.PostAutenticar(model);

                if (Autenticado)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e);
                }
                else
                {
                    this.NotifyUser("El usuario o la contraseña son incorrectos.", NotifyType.ErrorMessage);
                }
            }
            catch (Exception Error)
            {
                this.NotifyUser(Error.Message, NotifyType.ErrorMessage);
            }
            finally
            {
                Launch.IsEnabled = true;
            }
        }

        async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void txtContrasena_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Launch_Click(sender, e);
            }
        }
    }
}
