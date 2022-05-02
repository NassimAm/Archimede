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
    /// Interaction logic for InputFormula.xaml
    /// </summary>
    public partial class InputFormula : Page
    {
        public InputFormula()
        {
            InitializeComponent();
   
        }

        private void simplifyButton_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step1.xaml", UriKind.Absolute));

        }

        private void operator_Click(object sender, RoutedEventArgs e)
        {
            string res ;

            switch(((Button)sender).Name)
            {
                
                case "et":
                    res = ".";
                    break;
                case "ou":
                    res = "+";
                    break;
                case "non":
                    res = "!";
                    break;
                case "nand":
                    res = ">";
                    break;
                case "nor":
                    res = "<";
                    break;
                case "xor":
                    res = "^";
                    break;
                case "xnor":
                    res = "*";
                    break;
                case "paranthese":
                    res = "( )";
                    break;
                default:
                    return;
                   

            }

            expression.Text = expression.Text + res;
        }

        
    }
}
