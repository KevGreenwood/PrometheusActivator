using PrometheusActivator.Utilities;
using PrometheusActivator.Utilities.Activators;
using System.Windows.Controls;

namespace PrometheusActivator.Pages
{
    /// <summary>
    /// Lógica de interacción para WindowsPage.xaml
    /// </summary>
    
    public partial class WindowsPage : Page
    {
        public List<string> Servers => KMS.Servers;

        public WindowsPage()
        {
            DataContext = this;

            InitializeComponent();

            Logo.Source = WindowsHandler.Logo;
            ProductName.Text = WindowsHandler.GetAllInfo;
            Version.Text = WindowsHandler.Version;
        }

        private void MethodCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MethodCombo.SelectedIndex <= 1 )
            {
                LicensesCard.Visibility = System.Windows.Visibility.Visible;
                ServerCard.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                LicensesCard.Visibility = System.Windows.Visibility.Collapsed;
                ServerCard.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
