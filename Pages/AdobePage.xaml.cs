using PrometheusActivator.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PrometheusActivator.Pages
{
    public partial class AdobePage : Page
    {
        public ObservableCollection<AdobeProduct> AdobeProducts { get; } = new();

        public AdobePage()
        {
            InitializeComponent();
            DataContext = this;
            LoadAdobeProducts();
        }

        private async void LoadAdobeProducts()
        {
            await AdobeHandler.LoadProducts();
            foreach (var product in AdobeHandler.Products)
            {
                AdobeProducts.Add(product);
            }
        }
    }

}
