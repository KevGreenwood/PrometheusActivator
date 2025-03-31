using PrometheusActivator.Utilities;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace PrometheusActivator.Pages
{
    /// <summary>
    /// Lógica de interacción para WindowsPage.xaml
    /// </summary>
    public partial class FirewallPage : Page
    {
        public ObservableCollection<Product> Products => ProductHandler.Products; // Enlazar con la lista observable


        public FirewallPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch && toggleSwitch.DataContext is Product product)
            {
                FirewallManager.AddFirewallBlockRule(product);
            }
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch && toggleSwitch.DataContext is Product product)
            {
                FirewallManager.RemoveFirewallRule(product);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Configurar el diálogo de selección de archivo
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Executable", // Nombre por defecto
                DefaultExt = ".exe", // Extensión por defecto
                Filter = "Executables (.exe)|*.exe" // Filtro de archivos
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(filename);
                string productName = info.ProductName ?? Path.GetFileNameWithoutExtension(filename);


                if (ProductHandler.Products.Any(p => p.ExecutablePath == filename))
                {
                    var box = new Wpf.Ui.Controls.MessageBox();
                    box.Title = "Error";
                    box.Content = "El producto ya ha sido añadido.";
                    box.ShowDialogAsync();
                    return;
                }
                else
                {
                    Product newProduct = new()
                    {
                        Name = productName,
                        Version = info.ProductVersion ?? "1.0.0",
                        ExecutablePath = filename,
                        IsFirewallBlocked = FirewallManager.FirewallRuleExists(),
                        Icon = ProductHandler.GetIcon(filename)
                    };

                    // Agregarlo a la lista
                    ProductHandler.Products.Add(newProduct);
                }
            }
        }

    }
}
