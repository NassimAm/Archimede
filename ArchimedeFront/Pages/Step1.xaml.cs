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
        static int step4Number = 0;

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
                    step1.Margin = new Thickness(0, 0, 0,82);
                   
                    stepNumber++;
                    break;
                case 2:
                    _NextStep3.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step3.xaml", UriKind.RelativeOrAbsolute));
                    _NextStep2.Margin = new Thickness(0, 0, 0, 82);
                  

                    stepNumber++;
                    break;
                case 3:
                    _NextStep4.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step4.xaml", UriKind.RelativeOrAbsolute));
                    _NextStep3.Margin = new Thickness(0, 0, 0, 82);
                 
                  
                    expandBottomButton.Style =  FindResource("expandButtonHoriz")  as Style; 
                    int nbVariables = 4;
                    List<string>[] groupes = new List<string>[4];
                    groupes[0] = new List<string> { "0000" };
                    groupes[1] = new List<string> { "0001", "1000", "0100" };
                    groupes[2] = new List<string> { "0101", "1001", "1100" };
                    groupes[3] = new List<string> { "1101", "1110", "1101", "1110", "1101", "1110", "1101", "1110" };


                    Border border;
                    StackPanel groupesTable;

                    
                        groupesTable = new StackPanel() { Margin = new Thickness(10, 30, 10, 30) };
                        foreach (List<string> groupe in groupes)
                        {
                            foreach (string bincode in groupe)
                            {
                                if (bincode.StartsWith("0"))
                                    groupesTable.Children.Add(generateCheckedImplicant(bincode));
                                else groupesTable.Children.Add(generatePrimeImplicant(bincode));



                            }

                            border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0), Width = nbVariables * 20, Child = null };
                            groupesTable.Children.Add(border);

                        }
                        groupesMatrix.Children.Add(groupesTable);
                      stepNumber = -1;

                    break;
                case -1:
                     nbVariables = 4;
                    groupes = new List<string>[4];
                    groupes[0] = new List<string> { "0000" };
                    groupes[1] = new List<string> { "0001", "1000", "0100" };
                    groupes[2] = new List<string> { "0101", "1001", "1100" };
                    groupes[3] = new List<string> { "1101", "1110", "1101", "1110", "1101", "1110", "1101", "1110" };


                     
                    groupesTable = new StackPanel() { Margin = new Thickness(10, 30, 10, 30) };
                    foreach (List<string> groupe in groupes)
                    {
                        foreach (string bincode in groupe)
                        {
                            if (bincode.StartsWith("0"))
                                groupesTable.Children.Add(generateCheckedImplicant(bincode));
                            else groupesTable.Children.Add(generatePrimeImplicant(bincode));



                        }

                        border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0), Width = nbVariables * 20, Child = null };
                        groupesTable.Children.Add(border);

                    }
                    groupesMatrix.Children.Add(groupesTable);
                    step4Number++;
                    if (step4Number == 4) {
                        expandBottomButton.Style = FindResource("expandButton") as Style;
                        stepNumber = 4;
                    }
                    
                    break;
                case 4:
                    _NextStep5.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step5.xaml", UriKind.RelativeOrAbsolute));
                    groupesTableContainer.Margin = new Thickness(0, 0, 0, 82);
               
                    stepNumber++;
                    break;
                case 5:
                    _NextStep6.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step6.xaml", UriKind.RelativeOrAbsolute));
                    _NextStep5.Margin = new Thickness(0, 0, 0, 82);
                    _NextStep6.BringIntoView();
                    expandButtons.Visibility = Visibility.Collapsed;
                    stepNumber++;
                    break;
                default:
                    break;
            }

            expandButtons.BringIntoView();

            


        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {

            _NextStep6.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step6.xaml", UriKind.RelativeOrAbsolute));
            expandButtons.Visibility = Visibility.Collapsed;
            
        }


        private StackPanel generateCheckedImplicant(string bincode)
        {
            StackPanel result = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 2, 10, 2) };
            Viewbox viewbox = new Viewbox() { Width = 24, Margin = new Thickness(0, 0, 14, 0), Child = new Path() { Data = (Geometry)FindResource("CHECKED_ICON"), Fill = (SolidColorBrush?)new BrushConverter().ConvertFrom("#C4C4C4") } };
            TextBlock text = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Text = bincode };
            result.Children.Add(viewbox);
            result.Children.Add(text);
            return result;
        }
        private StackPanel generatePrimeImplicant(string bincode)
        {
            StackPanel result = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 2, 10, 2) };
            Grid grid = new Grid() { Width = 24, Margin = new Thickness(0, 0, 14, 0) };
            grid.Children.Add(new Ellipse() { Style = FindResource("greenDot") as Style });
            TextBlock text = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Text = bincode };

            result.Children.Add(grid);
            result.Children.Add(text);

            return result;
        }

        private StackPanel generateSelectedImplicant(string bincode)
        {
            StackPanel result = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 2, 10, 2) };
            Viewbox viewbox = new Viewbox() { Width = 24, Margin = new Thickness(0, 0, 14, 0), Child = new Path() { Style = FindResource("greenIcon") as Style, Data = (Geometry)FindResource("RIGHT_ARROW_ICON") } };
            TextBlock text = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Text = bincode };
            result.Children.Add(viewbox);
            result.Children.Add(text);
            return result;
        }

    }
}
