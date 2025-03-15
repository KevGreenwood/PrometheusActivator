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


        //  WINDOWS 7 FIX
        WindowsHandler.InitializeAsync();

        if (WindowsHandler.ProductName.Contains("Windows 7"))
        {
            System.Windows.MessageBox.Show("Unable to save file, try again.");

            WindowsHandler.Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/7_color.svg");
            WindowsHandler.ProductName = WindowsHandler.ProductName.Replace("Windows 7", "Windows 11");

        }
    }
}