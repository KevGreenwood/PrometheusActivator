using PrometheusActivator.Utilities;
using System.Diagnostics;
using System.Windows;

namespace PrometheusActivator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void NavigationViewControl_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer != null && args.InvokedItemContainer.Tag != null)
        {
            string item = args.InvokedItemContainer.Tag.ToString();
            if (item != null)
            {
                switch (item)
                {
                    case "Pages.HomePage":
                        //ContentFrame.Navigate(typeof(Pages.HomePage));
                        break;
                    case "Pages.WindowsPage":
                        ContentFrame.Navigate(typeof(Pages.WindowsPage));
                        break;
                    case "Pages.OfficePage":
                        //ContentFrame.Navigate(typeof(Pages.OfficePage));
                        break;
                    case "Pages.AdobePage":
                        ContentFrame.Navigate(typeof(Pages.AdobePage));
                        break;
                    default:
                        // Manejar caso desconocido o mostrar mensaje
                        MessageBox.Show($"Página no encontrada: {item}", "Error de Navegación");
                        break;
                }
            }
        }
    }
}
