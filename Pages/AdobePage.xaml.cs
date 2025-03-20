using PrometheusActivator.Utilities;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace PrometheusActivator.Pages
{
    /// <summary>
    /// Lógica de interacción para WindowsPage.xaml
    /// </summary>
    public partial class AdobePage : Page
    {
        public List<AdobeProduct> Products => AdobeHandler.Products;


        public AdobePage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch && toggleSwitch.DataContext is AdobeProduct product)
            {
                AdobeHandler.AddFirewallBlockRule(product);
            }
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch && toggleSwitch.DataContext is AdobeProduct product)
            {
                AdobeHandler.RemoveFirewallRule(product);
            }
        }
    }
}
