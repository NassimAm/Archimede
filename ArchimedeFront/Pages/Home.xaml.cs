using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Template.xaml", UriKind.Absolute));

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Width >= 1400)
            {
                logoArchimede.MaxWidth = 500;
                logoArchimede.Margin = new Thickness(80, 0, 80, 0);

            }
            else
            {
                logoArchimede.MaxWidth = 300;
                logoArchimede.Margin = new Thickness(20, 0, 20, 0);
 
            }
            if ( e.NewSize.Width >= 750 && e.NewSize.Width <= 900) {
                bienvenueText.FontSize = 64;
            }
            else if (e.NewSize.Width > 900 && e.NewSize.Width < 1400 || e.NewSize.Width<750)
            {
                bienvenueText.FontSize = 74;
            }
            else
            {
                bienvenueText.FontSize = 90;
            }
           
            if (e.NewSize.Width <= 750)
            {
                
                logoSection.Width = new GridLength(0 , GridUnitType.Pixel);
            }
           else
            {
                logoSection.Width = new GridLength(3, GridUnitType.Star); 
            }
            
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
    }
}
