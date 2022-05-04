using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using System.Windows.Media.Animation;

using ArchimedeFront.Styles;
using Archimède;
namespace ArchimedeFront.Pages

{
    /// <summary>
    /// Interaction logic for InputFormula.xaml
    /// </summary>
    public partial class InputFormula : Page
    {
        int caretPosition = 0;
        public InputFormula()
        {
            InitializeComponent();
            Data.resete();
            remove_error();
            numberOfVariablesInput.Width = new GridLength(0, GridUnitType.Star);
            guidePopUp.Visibility = Visibility.Collapsed;
            expression.Text = "A.B + !A.B.C";
            caretPosition=expression.Text.Length -1;
            Data.literal = true;


            AlignableWrapPanel buttons = new AlignableWrapPanel();
            Button operatorButton;
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
           
           Data.expression = expression.Text;
            
            if ( numerique.IsChecked == true)
            {
                Data.literal = false;
                Data.nbVariables = int.Parse(nbVariables.Text);
                Data.expression = expression.Text.Replace(" ","");
                Data.listMintermesString = Data.expression.Split(",").Distinct().ToList();
                long parsedInt;

                foreach(string minterm in Data.listMintermesString)
                {

                    try
                    {
                        parsedInt = long.Parse(minterm);
                        Data.mintermes.Add(new Minterme(parsedInt));
                    }
                    catch (OverflowException)
                    {
                        Data.mintermes.Add(new Minterme(minterm));
                    }

                }

                int maxNbUns = Minterme.maxNbUns;
                

                if (Minterme.maxNbVariables > Data.nbVariables)
                {
                    show_error("La liste de mintermes introduite depasse le nombre maximal de variables introduit ");
                    Data.resete();
                    return;
                }
            }
            else
            {
                Data.literal = true;
            }
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step1.xaml", UriKind.Absolute));
            
            
        }

        private void operator_Click(object sender, RoutedEventArgs e)
        {
            
            string res ;
            

            switch (((Button)sender).Content)
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


            caretPosition = expression.CaretIndex;
            expression.Text = expression.Text.Substring(0,caretPosition) + res + expression.Text.Substring(caretPosition);
            expression.Focus();
            expression.CaretIndex = caretPosition + 1;
            
            

            
            
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
            if(buttonsContainer != null) { 
            buttonsContainer.Opacity = 1;
            buttonsContainer.IsHitTestVisible = true;
            errorContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void literale_Checked(object sender, RoutedEventArgs e)
        {
            if (numberOfVariablesInput == null) return;
            
            numberOfVariablesInput.Width = new GridLength(0, GridUnitType.Star);
            expression.Text = "A.B + !A.B.C";
            operatorButtonsContainer.Visibility = Visibility.Visible;
            buttonsContainer.Margin = new Thickness(0, 24, 0, 24);
            guidePopUp.Visibility = Visibility.Collapsed;
        }

        private void numerique_Checked(object sender, RoutedEventArgs e)
        {
            if (numberOfVariablesInput == null) return;
            
            
            numberOfVariablesInput.Width = new GridLength(60, GridUnitType.Pixel);
            expression.Text = "0,1,2,3,10";
            
            operatorButtonsContainer.Visibility = Visibility.Collapsed;
            guidePopUp.Visibility = Visibility.Visible;
            buttonsContainer.Margin = new Thickness(0, 58, 0, 24);
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(4));
            guidePopUp.BeginAnimation(OpacityProperty, da);
        }

        private void nbVariables_SelectionChanged(object sender, RoutedEventArgs e)
        {

            remove_error();
        }

        private void expression_SelectionChanged(object sender, RoutedEventArgs e)
        {
            remove_error();
        }
    }



}
