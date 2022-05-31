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
using dnf;
using System.Windows.Media;
using DetectionErreurs;
using System.Windows.Media.Effects;
using System.Diagnostics;

namespace ArchimedeFront.Pages

{
    /// <summary>
    /// Interaction logic for InputFormula.xaml
    /// </summary>
    public partial class InputFormula : Page
    {
        int caretPosition = 0;
        bool activePopUp = false;
        public InputFormula()
        {
            InitializeComponent();
            SimplificationPopUP.Visibility = Visibility.Collapsed;
            TransformationPopUP.Visibility = Visibility.Collapsed;
            errorsContainer.Children.Clear();
            enableButtons();
            numberOfVariablesInput.Width = new GridLength(0, GridUnitType.Star);
            guidePopUp.Visibility = Visibility.Collapsed;
            expression.Text = "A.B + !A.B.C";


            if (Data.saveexpressionlitterale == null)
            {
                Data.saveexpressionlitterale = expression.Text;
            }
            else
                expression.Text = Data.saveexpressionlitterale;

            caretPosition =expression.Text.Length -1;
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

            //Initialiser les paramètres de synthèse
            Data.nb_and = 16;
            Data.nb_or = 16;
            Data.nb_nand = 2;
            Data.nb_nor = 2;
            Data.nb_xor = 2;
            Data.nb_xnor = 2;
        }

        private void simplifyButton_Click(object sender, RoutedEventArgs e)
        {
           errorsContainer.Children.Clear();
           Data.resete();
           Data.expression = expression.Text.Replace(" ","");
            expression.Text = Data.expression;
            
            if ( numerique.IsChecked == true)
            {
                Data.literal = false;


                List<string> errorMessages = new List<string>();
                errorMessages = Erreurs.detectionErreurs(expression.Text, false);
                if (errorMessages.Count > 0)
                {
                    disableButtons();
                    foreach (string error in errorMessages)
                    {
                        errorsContainer.Children.Add(generateNewError(error));
                    }
                    return;
                }



                Boolean nbVariableAuto  = nbVariables.Text.Replace(" ","").Equals("");

                if (!nbVariableAuto) {
                    try
                    {
                        Data.nbVariables = int.Parse(nbVariables.Text);
                    }
                    catch (Exception)
                    {
                        disableButtons();
                        errorsContainer.Children.Add(generateNewError("Nombre de variable invalide ."));
                        return;
                    }
                }
               
                
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

                if (nbVariableAuto) Data.nbVariables = Minterme.maxNbVariables;
                if (Minterme.maxNbVariables > Data.nbVariables)
                {
                    errorsContainer.Children.Add(generateNewErrorNumerique(String.Format("La liste de mintermes introduite dépasse le nombre maximal de variables introduit,\nLe nombre de variables minimal pour cette liste est de : {0}", Minterme.maxNbVariables)));
                    disableButtons();
                    return;
                }
            }
            else
            {
                Data.literal = true;
                List<string> errorMessages = new List<string>();
                errorMessages= Erreurs.detectionErreurs(expression.Text,true);
                if(errorMessages.Count > 0)
                {
                    disableButtons();
                    foreach(string error in errorMessages)
                    {
                        errorsContainer.Children.Add(generateNewError(error));
                    }
                    return;
                }
               
            }

            pageContent.IsHitTestVisible = false;
            activePopUp = true;
            pageContent.Effect = new BlurEffect() { Radius = 30, KernelType = KernelType.Gaussian };
            SimplificationPopUP.Visibility = Visibility.Visible;
        }

        private void syntheseButton_Click(object sender, RoutedEventArgs e)
        {
            errorsContainer.Children.Clear();
            Data.resete();
            Data.expression = expression.Text.Replace(" ", "");
            expression.Text = Data.expression;

            if (numerique.IsChecked == true)
            {
                Data.literal = false;


                  List<string> errorMessages = new List<string>();
                errorMessages = Erreurs.detectionErreurs(expression.Text, false);
                if (errorMessages.Count > 0)
                {
                    disableButtons();
                    foreach (string error in errorMessages)
                    {
                        errorsContainer.Children.Add(generateNewError(error));
                    }
                    return;
                }



                Boolean nbVariableAuto = nbVariables.Text.Replace(" ", "").Equals("");

                if (!nbVariableAuto)
                {
                    try
                    {
                        Data.nbVariables = int.Parse(nbVariables.Text);
                    }
                    catch (Exception)
                    {
                        disableButtons();
                        errorsContainer.Children.Add(generateNewError("Nombre de variable invalide ."));
                        return;
                    }
                }

                Data.expression = expression.Text.Replace(" ", "");
                Data.listMintermesString = Data.expression.Split(",").Distinct().ToList();
                long parsedInt;

                foreach (string minterm in Data.listMintermesString)
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

                if (nbVariableAuto) Data.nbVariables = Minterme.maxNbVariables;
                if (Minterme.maxNbVariables > Data.nbVariables)
                {
                    errorsContainer.Children.Add(generateNewErrorNumerique(String.Format("La liste de mintermes introduite dépasse le nombre maximal de variables introduit,\nLe nombre de variables minimal pour cette liste est de : {0}", Minterme.maxNbVariables)));
                    disableButtons();
                    return;
                }
            }
            else
            {
                Data.literal = true;
                List<string> errorMessages = new List<string>();
                errorMessages = Erreurs.detectionErreurs(expression.Text,true);
                if (errorMessages.Count > 0)
                {
                    disableButtons();
                    foreach (string error in errorMessages)
                    {
                        errorsContainer.Children.Add(generateNewError(error));
                    }
                    return;
                }

            }

            pageContent.IsHitTestVisible = false;
            pageContent.Effect = new BlurEffect() { Radius = 30, KernelType = KernelType.Gaussian };
            SynthesePopUP.Visibility = Visibility.Visible;
            activePopUp = true;
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

        
       
        private void literale_Checked(object sender, RoutedEventArgs e)

        {
            if (numberOfVariablesInput == null) return;
            errorsContainer.Children.Clear();
            enableButtons();
            transformButton.Visibility = Visibility.Visible;
            numberOfVariablesInput.Width = new GridLength(0, GridUnitType.Star);
            if (Data.saveexpressionlitterale == null)
            {
                Data.saveexpressionlitterale = expression.Text;
            }
            else
                expression.Text = Data.saveexpressionlitterale;

            operatorButtonsContainer.Visibility = Visibility.Visible;
            buttonsContainer.Margin = new Thickness(0, 24, 0, 24);
            guidePopUp.Visibility = Visibility.Collapsed;
        }

        private void numerique_Checked(object sender, RoutedEventArgs e)
        {
            if (numberOfVariablesInput == null) return;

            errorsContainer.Children.Clear();
            enableButtons();
            transformButton.Visibility = Visibility.Collapsed;
            numberOfVariablesInput.Width = new GridLength(60, GridUnitType.Pixel);
            expression.Text = "0,1,2,3,10";
            if (Data.saveexpressionnumerique == null)
            {
                Data.saveexpressionnumerique = expression.Text;
            }
            else
                expression.Text = Data.saveexpressionnumerique;

            operatorButtonsContainer.Visibility = Visibility.Collapsed;
            guidePopUp.Visibility = Visibility.Visible;
            buttonsContainer.Margin = new Thickness(0, 58, 0, 24);
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(6));
            guidePopUp.BeginAnimation(OpacityProperty, da);
        }

        
        

        private StackPanel generateNewError(string message)
        {

            //generate exclamation mark
            Border border = new Border()
            {
                BorderThickness = new Thickness(2, 2, 2, 2),
                BorderBrush = Brushes.Red ,
                Width = 24 ,
                Height = 24 ,
                CornerRadius = new CornerRadius(24) ,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = new TextBlock() { Style= FindResource("fontProductSans") as Style ,FontSize =18 , Foreground=Brushes.Red,FontWeight= FontWeights.SemiBold ,VerticalAlignment = VerticalAlignment.Center , HorizontalAlignment = HorizontalAlignment.Center , Text="!" }
                

            };
            TextBlock textBlock =
            new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 20, Margin = new Thickness(8, 0, 4, 0), Foreground = Brushes.Red, FontWeight = FontWeights.SemiBold, VerticalAlignment = VerticalAlignment.Center, Text = "Erreur signalée :" };
            StackPanel errorSignal = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Top,
            };
            errorSignal.Children.Add(border);
            errorSignal.Children.Add(textBlock);

            TextBlock errorMessage = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 20, Margin = new Thickness(0, 0, 0, 0), VerticalAlignment = VerticalAlignment.Top, TextWrapping = TextWrapping.Wrap, Text = message};

            StackPanel error = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10, 2, 10, 8) };
            error.Children.Add(errorSignal);
            error.Children.Add(errorMessage);
            return error;
        }

        private StackPanel generateNewErrorNumerique(string message)
        {

            //generate exclamation mark
            Border border = new Border()
            {
                BorderThickness = new Thickness(2, 2, 2, 2),
                BorderBrush = Brushes.Red,
                Width = 24,
                Height = 24,
                CornerRadius = new CornerRadius(24),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = new TextBlock() { Style = FindResource("fontProductSans") as Style, FontSize = 18, Foreground = Brushes.Red, FontWeight = FontWeights.SemiBold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Text = "!" }


            };
            TextBlock textBlock =
            new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 20, Margin = new Thickness(8, 0, 4, 0), Foreground = Brushes.Red, FontWeight = FontWeights.SemiBold, VerticalAlignment = VerticalAlignment.Center, Text = "Erreur signalée :" };
            StackPanel errorSignal = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Top,
            };
            errorSignal.Children.Add(border);
            errorSignal.Children.Add(textBlock);

            TextBlock errorMessage = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 20, Margin = new Thickness(0, 0, 0, 0), VerticalAlignment = VerticalAlignment.Top, TextWrapping = TextWrapping.Wrap, Text = message };

            StackPanel error = new StackPanel() { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10, 2, 10, 8) };
            error.Children.Add(errorSignal);
            error.Children.Add(errorMessage);
            return error;
        }

        private void disableButtons()
        {
            if (buttonsContainer == null) return;
            buttonsContainer.Opacity = 0.3;
            buttonsContainer.IsHitTestVisible = false;
        }

        private void enableButtons()
        {
            if (buttonsContainer == null) return;
            buttonsContainer.Opacity = 1;
            buttonsContainer.IsHitTestVisible = true;
        }

        private void selectionChanged(object sender, RoutedEventArgs e)
        {
            enableButtons();
        }

        private void startSimplification_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < radioButtons.Children.Count; i++)
            {
                if (((RadioButton)radioButtons.Children[i]).IsChecked == true)
                {
                    Data.codeTransformation = ((RadioButton)radioButtons.Children[i]).Name[2];
                    break;
                }
            }

            if (Data.literal)
            {
                
                if(Data.codeTransformation == '1')
                {
                    ExprBool tree = ExprBool.ParseExpression(Data.expression);
                    tree = ExprBool.cnf(tree);
                    StringBuilder sb = new StringBuilder();
                    ExprBool.inorder(tree, sb);
                    Data.expressionTransforme = sb.ToString();
                    Data.variables = ExprBool.getVariables(Data.expressionTransforme).OrderBy(ch => ch).ToList();
                    Data.nbVariables = Data.variables.Count;
                    Data.stringListMinterm = ExprBool.getMaxterms(Data.expressionTransforme, Data.variables);
                }
                else
                {
                    Data.expressionTransforme = ExprBool.transformerDNF(Data.expression);
                    Data.variables = ExprBool.getVariables(Data.expressionTransforme).OrderBy(ch => ch).ToList();
                    Data.nbVariables = Data.variables.Count;
                    Data.stringListMinterm = ExprBool.getMinterms(Data.expressionTransforme, Data.variables);
                }
               

                if (Data.stringListMinterm.Count == 0)
                {
                    Data.resultatFaux = true;
                    NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step6.xaml", UriKind.Absolute));
                    return;
                }

            }
            if (activerTrace.IsChecked == false) Data.trace = false;
            else Data.trace = true;
            

            

            if (Data.trace)
            {
                NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step1.xaml", UriKind.Absolute));

            }
            else
            {
                NavigationService.Navigate(new Uri("pack://application:,,,/Pages/ResultSimpNoTrace.xaml", UriKind.Absolute));
            }

        }

        private void startTransformation_Click(object sender, RoutedEventArgs e)
        {
            
            for (int i = 0; i < radioButtonsContainer.Children.Count; i++)
            {
                if (((RadioButton)radioButtonsContainer.Children[i]).IsChecked == true)
                {
                    Data.codeTransformation = ((RadioButton)radioButtonsContainer.Children[i]).Name[1];
                    break;
                }
            }

            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/ResultTransformation.xaml", UriKind.Absolute));
        }

       
        private void startSynthese_Button_Click(object sender, RoutedEventArgs e)
        {
            Data.expression = expression.Text;
            if(!Data.syntheseAuto)
            {
                Data.nb_and = int.Parse(et_entree_input_text.Text);
                Data.nb_or = int.Parse(ou_entree_input_text.Text);
                Data.nb_nand = int.Parse(nand_entree_input_text.Text);
                Data.nb_nor = int.Parse(nand_entree_input_text.Text);
                Data.nb_xor = int.Parse(nand_entree_input_text.Text);
                Data.nb_xnor = int.Parse(nand_entree_input_text.Text);
            }
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/SynthesePage.xaml", UriKind.Absolute));
        }

        private void transformButton_Click(object sender, RoutedEventArgs e)
        {
            errorsContainer.Children.Clear();
            Data.resete();
            Data.expression = expression.Text.Replace(" ", "");
            expression.Text = Data.expression;

            Data.literal = true;
            List<string> errorMessages;
            errorMessages = Erreurs.detectionErreurs(expression.Text , true);
            if (errorMessages.Count > 0)
            {
                disableButtons();
                foreach (string error in errorMessages)
                {
                    errorsContainer.Children.Add(generateNewError(error));
                }
                return;
            }

            pageContent.IsHitTestVisible = false;
            pageContent.Effect = new BlurEffect() { Radius = 30, KernelType = KernelType.Gaussian };
            TransformationPopUP.Visibility = Visibility.Visible;
            activePopUp = true;

            


        }

        private void exitSimpPopUP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageContent.IsHitTestVisible = true;
            pageContent.Effect = null;
            SimplificationPopUP.Visibility = Visibility.Collapsed;
        }

        private void exitTransformPopUP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageContent.IsHitTestVisible = true;
            pageContent.Effect = null;
            TransformationPopUP.Visibility = Visibility.Collapsed;
        }
        public void exitSynthesePopUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pageContent.IsHitTestVisible = true;
            pageContent.Effect = null;
            SynthesePopUP.Visibility = Visibility.Collapsed;
        }

        private void ET_ilimite_Checked(object sender, RoutedEventArgs e)
        {
            if (et_entree_input == null) return;
            Data.syntheseAuto = true;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.3));


            switch (((RadioButton)sender).GroupName)
            {
                case "ET_entrees":
                    et_entree_input.BeginAnimation(OpacityProperty, da);
                    Data.nb_and = 16;
                    break;
                case "OU_entrees":
                    ou_entrees_input.BeginAnimation(OpacityProperty, da);
                    Data.nb_or = 16;
                    break;
                case "NAND_entrees":
                    nand_entrees_input.BeginAnimation(OpacityProperty, da);
                    Data.nb_nand = 2;
                    break;
                case "NOR_entrees":
                    nor_entrees_input.BeginAnimation(OpacityProperty, da);
                    Data.nb_nor = 2;
                    break;
                case "XOR_entrees":
                    xor_entrees_input.BeginAnimation(OpacityProperty, da);
                    Data.nb_xor = 2;
                    break;
                case "XNOR_entrees":
                    xnor_entrees_input.BeginAnimation(OpacityProperty, da);
                    Data.nb_xnor = 2;
                    break;
            }
        }
    

        private void ET_limite_Checked(object sender, RoutedEventArgs e)
        {
            if (et_entree_input == null) return;
            Data.syntheseAuto = false;

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.3));
           

            switch (((RadioButton)sender).GroupName)
            {
                case "ET_entrees":
                    et_entree_input.BeginAnimation(OpacityProperty, da);
                    break;
                case "OU_entrees":
                    ou_entrees_input.BeginAnimation(OpacityProperty, da);
                    break;
                case "NAND_entrees":
                    nand_entrees_input.BeginAnimation(OpacityProperty, da);
                    break;
                case "NOR_entrees":
                    nor_entrees_input.BeginAnimation(OpacityProperty, da);
                    break;
                case "XOR_entrees":
                    xor_entrees_input.BeginAnimation(OpacityProperty, da);
                    break;
                case "XNOR_entrees":
                    xnor_entrees_input.BeginAnimation(OpacityProperty, da);
                    break;
            }
        }

        private void outPopUp_MouseDown(object sender , MouseButtonEventArgs e)
        {
            if (activePopUp)
            {
                pageContent.IsHitTestVisible = true;
                pageContent.Effect = null;
                SynthesePopUP.Visibility = Visibility.Collapsed;
                TransformationPopUP.Visibility = Visibility.Collapsed;
                SimplificationPopUP.Visibility = Visibility.Collapsed;
            }
        }
    }



}
