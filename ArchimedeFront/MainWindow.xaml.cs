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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace ArchimedeFront
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _PageContent.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/InputFormula.xaml", UriKind.RelativeOrAbsolute) );
            menu.Cursor = Cursors.Hand;
            navBar.Opacity = 0;
            logoFooter.Opacity = 0;

        

         }

        private void menu_Checked(object sender, RoutedEventArgs e)
        {

            menuIcon.SetResourceReference(Path.DataProperty, "EXIT_ICON");
            menu.Margin = new Thickness(0, 12, 16, 0);
            menu.HorizontalAlignment = HorizontalAlignment.Right;
            navBar.Opacity = 1;
            logoFooter.Opacity = 1;

        }

        private void menu_Unchecked(object sender, RoutedEventArgs e)
        {
            menuIcon.SetResourceReference(Path.DataProperty, "MENU_ICON");
            menu.Margin = new Thickness(0, 20, 0, 0);
            menu.HorizontalAlignment = HorizontalAlignment.Center;
            navBar.Opacity = 0;
            logoFooter.Opacity = 0;
        }






    }
}
