using PrometheusActivator.Pages;
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

        //Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

        Wpf.Ui.Appearance.ApplicationThemeManager.Apply(
          Wpf.Ui.Appearance.ApplicationTheme.Dark, // Theme type
          Wpf.Ui.Controls.WindowBackdropType.Auto,  // Background type
          true                                      // Whether to change accents automatically
        );


        InitializeComponent();

        Loaded += (_, _) => RootNavigation.Navigate(typeof(WindowsPage));
    }
}