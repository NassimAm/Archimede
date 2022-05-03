using System;
using System.Collections.Generic;
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

            _PageContent.Effect = new BlurEffect() { Radius = 14, KernelType = KernelType.Gaussian };
            menuIcon.SetResourceReference(Path.DataProperty, "EXIT_ICON");
            menu.Margin = new Thickness(0, 12, 16, 0);
            menu.HorizontalAlignment = HorizontalAlignment.Right;
            navBar.Opacity = 1;
            logoFooter.Opacity = 1;

        }

        private void menu_Unchecked(object sender, RoutedEventArgs e)
        {
            _PageContent.Effect = null;

            menuIcon.SetResourceReference(Path.DataProperty, "MENU_ICON");
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

        }

        

        private void toDocumentation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
