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

            List<string> mintermes = new List<string>();
            List<string> bincodes = new List<string>();
            mintermes.Add("!A!BC!D");
            bincodes.Add("0010");
            mintermes.Add("!AB!C!D");
            bincodes.Add("0100");
            mintermes.Add("!AB!CD");
            bincodes.Add("0101");
            mintermes.Add("!ABC!D");
            bincodes.Add("0110");
            mintermes.Add("!ABCD");
            bincodes.Add("0111");
            mintermes.Add("A!B!CD");
            bincodes.Add("1001");
            mintermes.Add("AB!CD");
            bincodes.Add("1101");

            WrapPanel wrappanel;
            for (int i = 0;i < mintermes.Count;i++)
            {
                wrappanel = new WrapPanel();
                wrappanel.Orientation = Orientation.Vertical;
                wrappanel.VerticalAlignment = VerticalAlignment.Top;
                wrappanel.Margin = new Thickness(10, 0, 10, 10);
                wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(0, 0, 0, 10), Text = mintermes[i]});
                wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(0, 0, 0, 0), Text = bincodes[i]});
                mintermesList.Children.Add(wrappanel);

                if(i != mintermes.Count-1)
                {
                    wrappanel = new WrapPanel();
                    wrappanel.Orientation = Orientation.Vertical;
                    wrappanel.VerticalAlignment = VerticalAlignment.Top;
                    wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(0, 0, 0, 0), Text = "+"});
                    mintermesList.Children.Add(wrappanel);
                }
            }
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
                case 4:
                    _NextStep5.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step5.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 5:
                    _NextStep6.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step6.xaml", UriKind.RelativeOrAbsolute));
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
