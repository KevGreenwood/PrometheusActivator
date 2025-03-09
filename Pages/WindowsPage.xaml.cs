using PrometheusActivator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrometheusActivator.Pages
{
    /// <summary>
    /// Lógica de interacción para WindowsPage.xaml
    /// </summary>
    public partial class WindowsPage : Page
    {

        public WindowsPage()
        {
            InitializeComponent();

            ShellBox.Text = PowershellHandler.RunCommand("Get-Process");
        }


    }
}
