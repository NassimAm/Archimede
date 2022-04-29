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

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Interaction logic for Step1.xaml
    /// </summary>
    public partial class Step1 : Page
    {
        static int stepNumber = 1;

        public Step1()
        {
            InitializeComponent();
        }

        private void nextStepButton_click(object sender, RoutedEventArgs e)
        {
            switch (stepNumber)
            {
                case 1:
                    _NextStep2.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step2.xaml", UriKind.RelativeOrAbsolute));

                    break;
                case 2:
                    _NextStep3.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step3.xaml", UriKind.RelativeOrAbsolute));

                    break;
                case 3:
                    _NextStep4.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step4.xaml", UriKind.RelativeOrAbsolute));

                    break;
                default:
                    break;
            }

            stepNumber++;


        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
