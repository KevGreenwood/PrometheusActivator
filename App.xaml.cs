using PrometheusActivator.Utilities;
using System.Windows;

namespace PrometheusActivator;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected async override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        try
        {
            await WindowsHandler.InitializeAsync();


            await ProductHandler.LoadProducts();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(1);
        }
        

    }
}

