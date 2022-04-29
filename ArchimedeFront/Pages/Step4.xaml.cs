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
    /// Interaction logic for Step4.xaml
    /// </summary>
    public partial class Step4 : Page
    {
        public Step4()
        {
            InitializeComponent();

            int nbVariables = 4;
            List<string>[] groupes = new List<string>[4];
            groupes[0] = new List<string> { "0000" };
            groupes[1] = new List<string> { "0001", "1000", "0100" };
            groupes[2] = new List<string> { "0101", "1001", "1100" };
            groupes[3] = new List<string> { "1101", "1110", "1101", "1110", "1101", "1110", "1101", "1110" };


            Border border;
            StackPanel groupesTable;

            for (int i = 0; i < 10; i++)
            {
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
            }



            /* groupesTable.Children.Add(generatePrimeImplicant("00001"));
             groupesTable.Children.Add(generateCheckedImplicant("00001"));
             groupesTable.Children.Add(generateSelectedImplicant("00001"));*/


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
