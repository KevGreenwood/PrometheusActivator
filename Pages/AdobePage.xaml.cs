using PrometheusActivator.Utilities;
using System.Collections.ObjectModel;
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
        public ObservableCollection<AdobeProduct> AdobeProducts { get; } = new();


        public AdobePage()
        {
            DataContext = this;
            LoadAdobeProducts();


            InitializeComponent();


        }

        private async void LoadAdobeProducts()
        {
            await AdobeHandler.LoadProducts();
            foreach (var product in AdobeHandler.Products)
            {
                AdobeProducts.Add(product);
            }
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
