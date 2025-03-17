using PrometheusActivator.Utilities;
using System.Windows.Controls;

namespace PrometheusActivator.Pages
{
    /// <summary>
    /// Lógica de interacción para WindowsPage.xaml
    /// </summary>
    public partial class WindowsPage : Page
    {
        public WindowsPage()
        {
            DataContext = this;

            InitializeComponent();

            Logo.Source = WindowsHandler.Logo;
            ProductsCard.Text = "Lmao";
        }

        private void MethodCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
