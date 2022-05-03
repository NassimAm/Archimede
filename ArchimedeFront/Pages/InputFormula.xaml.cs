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
using ArchimedeFront.Styles;
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
            remove_error();
            numberOfVariablesInput.Width = new GridLength(0, GridUnitType.Star);


            AlignableWrapPanel buttons = new AlignableWrapPanel();
            Button operatorButton ;
            string[] operators = { "ET", "OU", "NON", "NAND", "NOR", "XOR", "XNOR", "( )" };

            buttons.HorizontalContentAlignment = HorizontalAlignment.Center;
            buttons.MaxWidth = 500;
            foreach(string o in operators)
            {
                operatorButton = new Button() { Style = FindResource("operatorButton") as Style, Margin = new Thickness(14, 6, 14, 6)  , Content = o , HorizontalAlignment = HorizontalAlignment.Center};
                operatorButton.Click += new RoutedEventHandler( operator_Click );
                buttons.Children.Add(operatorButton);
            }
            
            operatorButtonsContainer.Children.Add(buttons);
        
        }

        private void simplifyButton_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step1.xaml", UriKind.Absolute));

        }

        private void operator_Click(object sender, RoutedEventArgs e)
        {
            string res ;

            switch(((Button)sender).Content)
            {
                
                case "ET":
                    res = ".";
                    break;
                case "OU":
                    res = "+";
                    break;
                case "NON":
                    res = "!";
                    break;
                case "NAND":
                    res = "↑";
                    break;
                case "NOR":
                    res = "↓";
                    break;
                case "XOR":
                    res = "⊕";
                    break;
                case "XNOR":
                    res = "⊙";
                    break;
                case "( )":
                    res = "()";
                    break;
                default:
                    return;
                   
               
            }

            expression.Text = expression.Text + res;
            if((string) ((Button)sender).Content == "( )")
            {
                expression.CaretIndex = expression.Text.Length-1;
            }
            else
            {
                expression.CaretIndex = expression.Text.Length;
            }

            expression.ScrollToEnd(); 
            expression.Focus();
        }

        private void simplifyButton_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void show_error(string message)
        {
            buttonsContainer.Opacity = 0.3;
            buttonsContainer.IsHitTestVisible = false;
            errorMessage.Text = message;
            errorContainer.Visibility = Visibility.Visible;
        }
        private void remove_error()
        {
            buttonsContainer.Opacity = 1;
            buttonsContainer.IsHitTestVisible = true;
            errorContainer.Visibility = Visibility.Collapsed;
            
        }

        private void literale_Checked(object sender, RoutedEventArgs e)
        {
            if (numberOfVariablesInput == null) return;
            numberOfVariablesInput.Width = new GridLength(0, GridUnitType.Star);
            expression.Text = "A.B + !A.B.C";
            operatorButtonsContainer.Visibility = Visibility.Visible;
            buttonsContainer.Margin = new Thickness(0, 24, 0, 24);
        }

        private void numerique_Checked(object sender, RoutedEventArgs e)
        {
            if (numberOfVariablesInput == null) return;
            numberOfVariablesInput.Width = new GridLength(60, GridUnitType.Pixel);
            expression.Text = "0,1,2,3,10";
            
            operatorButtonsContainer.Visibility = Visibility.Collapsed;
            buttonsContainer.Margin = new Thickness(0, 64, 0, 24);
        }
    }



}
