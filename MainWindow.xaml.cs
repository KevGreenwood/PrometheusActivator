using PrometheusActivator.Pages;
using PrometheusActivator.Utilities;
using Wpf.Ui.Controls;


namespace PrometheusActivator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        DataContext = this;

        Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        InitializeComponent();

        Loaded += (_, _) => RootNavigation.Navigate(typeof(WindowsPage));
        WindowsHandler.InitializeAsync();


    }
}