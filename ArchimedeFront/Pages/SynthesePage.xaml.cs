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
    /// Logique d'interaction pour SynthesePage.xaml
    /// </summary>
    public partial class SynthesePage : Page
    {
        public SynthesePage()
        {

            Synthese.ExprBoolNode tree = Synthese.To_N_ary(Data.expression.Replace(" ",""), Data.nb_and, Data.nb_or);
            Synthese.Circuit_Visualisation(tree);
            InitializeComponent();
        }

        private void returnButton_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/InputFormula.xaml", UriKind.Absolute));
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
