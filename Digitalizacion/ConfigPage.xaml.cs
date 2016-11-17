using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Digitalizacion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public ConfigPage()
        {
            this.InitializeComponent();
            bool value = Convert.ToBoolean(localSettings.Values["SaveLocalFile"]);
            tggSaveFile.IsOn = value;
        }

        private void tggSaveFile_Toggled(object sender, RoutedEventArgs e)
        {
            bool saveFile = tggSaveFile.IsOn;            
            localSettings.Values["SaveLocalFile"] = saveFile;
        }
    }
}
