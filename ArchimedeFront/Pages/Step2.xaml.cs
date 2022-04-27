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
    /// Interaction logic for Step2.xaml
    /// </summary>
    public partial class Step2 : Page
    {

        public Step2()
        {
            InitializeComponent();

            int nbVariables = 4;

            List<string>[] groupes = new List<string>[12];
            groupes[0] = new List<string> { "0000"};
            groupes[1] = new List<string> { "0001" , "1000" , "0100"};
            groupes[2] = new List<string> { "0101", "1001", "1100" };
            groupes[3] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1110" };
            groupes[4] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101",  "1101", "1110" };
            groupes[5] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };
            groupes[6] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };
            groupes[7] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };
            groupes[8] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };
            groupes[9] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };
            groupes[10] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };
            groupes[11] = new List<string> { "0111", "1101", "1110", "1101", "1110", "1101", "1101", "1110" };





            Border border;
            StackPanel groupeContainer;

            foreach (List<string> groupe in groupes)
            {
                foreach(string bincode in groupe)
                {
                    groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style , FontSize = 28 , Margin = new Thickness(36,2,36,2) , Text = bincode});
                }
                
                border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0) , Width = nbVariables*16 , Child = null  };
                groupesTable.Children.Add(border);
            }



        }


        private void skipButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
