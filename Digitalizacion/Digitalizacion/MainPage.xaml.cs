using Digitalizacion.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Digitalizacion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public string LaunchParam { get; set; }
        partial void GetScenarioIdForLaunch(string launchParam, ref int index);
        public static string Usuario;

        public MainPage()
        {
            this.InitializeComponent();
            SampleTitle.Text = FEATURE_NAME;

            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Common.APIClient.rootPage = MainPage.Current;

            try
            {
                IEnumerable<int> Autorizados = await Models.UsuariosModel.GetVinculos();

                // Populate the scenario list from the SampleConfiguration.cs file
                ScenarioControl.ItemsSource = from a in scenarios
                                              join b in Autorizados on a.Number equals b
                                              select a;

                // If we have saved state return to the previously selected scenario  
                if (SuspensionManager.SessionState.ContainsKey("SelectedScenarioIndex") && String.IsNullOrEmpty(e.Parameter.ToString()))
                {
                    ScenarioControl.SelectedIndex = Convert.ToInt32(SuspensionManager.SessionState["SelectedScenarioIndex"]);
                    ScenarioControl.ScrollIntoView(ScenarioControl.SelectedItem);
                }
                else if (e.Parameter != null && !String.IsNullOrEmpty(e.Parameter.ToString()))
                {
                    this.LaunchParam = e.Parameter.ToString();
                    int index = 0;
                    this.GetScenarioIdForLaunch(this.LaunchParam, ref index);
                    ScenarioControl.SelectedIndex = index;
                }
            }
            catch (Exception)
            {
                // No hay menu
                NotifyUser("Usted no tiene autorizacion para usar esta aplicacion.", NotifyType.ErrorMessage);
            }
        }

        /// <summary>
        /// Called whenever the user changes selection in the scenarios list. This method will navigate to the respective
        /// sample scenario page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the status block when navigating scenarios.
            NotifyUser(String.Empty, NotifyType.StatusMessage);

            ListBox scenarioListBox = sender as ListBox;
            Scenario s = scenarioListBox.SelectedItem as Scenario;
            if (s != null)
            {
                SuspensionManager.SessionState["SelectedScenarioIndex"] = scenarioListBox.SelectedIndex;

                try
                {
                    if (s.Scanner)
                    {
                        ScenarioFrame.Navigate(typeof(ScannerPage), s.ClassType);
                    }
                    else
                    {
                        ScenarioFrame.Navigate(s.ClassType);
                    }
                }
                catch (Exception ex)
                {
                    NotifyUser(ex.Message, NotifyType.ErrorMessage);
                }
            }
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (ScenarioFrame.CanGoBack)
            {
                // Clear the status block when navigating
                NotifyUser(String.Empty, NotifyType.StatusMessage);

                ScenarioFrame.GoBack();

                //Indicate the back button press is handled so the app does not exit
                e.Handled = true;
            }
        }

        /// <summary>
        /// List of Scenarios will be included in this page.
        /// </summary>
        public List<Scenario> Scenarios
        {
            get { return this.scenarios; }
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

        void Footer_Click(object sender, RoutedEventArgs e)
        {
            ShowLogin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = (Splitter.IsPaneOpen == true) ? false : true;
            StatusBorder.Visibility = Visibility.Collapsed;
        }

        public void ShowLogin()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(LoginPage));
        }
    }
}
