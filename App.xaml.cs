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

        await WindowsHandler.InitializeAsync();


        await ProductHandler.LoadProducts();

    }
}

