using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>
    public partial class Template : Page
    {
        public Template()
        {
            InitializeComponent();
            _PageContent.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/InputFormula.xaml", UriKind.RelativeOrAbsolute));
            menu.Cursor = Cursors.Hand;
            navBar.Opacity = 0;
            logoFooter.Opacity = 0;
        }




        private void menu_Checked(object sender, RoutedEventArgs e)
        {
            _PageContent.IsHitTestVisible = false;
            _PageContent.Effect = new BlurEffect() { Radius = 30, KernelType = KernelType.Gaussian };
            menuIcon.SetResourceReference(System.Windows.Shapes.Path.DataProperty, "EXIT_ICON");
            menu.Margin = new Thickness(0, 12, 16, 0);
            menu.HorizontalAlignment = HorizontalAlignment.Right;
            navBar.Opacity = 1;
            logoFooter.Opacity = 1;

        }

        private void menu_Unchecked(object sender, RoutedEventArgs e)
        {
            _PageContent.IsHitTestVisible=true;
            _PageContent.Effect = null;

            menuIcon.SetResourceReference(System.Windows.Shapes.Path.DataProperty, "MENU_ICON");
            menu.Margin = new Thickness(0, 20, 0, 0);
            menu.HorizontalAlignment = HorizontalAlignment.Center;
            navBar.Opacity = 0;
            logoFooter.Opacity = 0;
        }

       

        private void toHomePage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Home.xaml", UriKind.Absolute));

        }

        private void toWebSite_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C Website\\src\\index.html";
            process.StartInfo = startInfo;
            process.Start();
        }

        

        private void toDocumentation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _PageContent.IsHitTestVisible = true;
            _PageContent.Effect = null;
            _PageContent.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Documentation.xaml", UriKind.RelativeOrAbsolute));
            

        }
    }
}
