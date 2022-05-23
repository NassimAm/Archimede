using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading;
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
using Archimède;

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Logique d'interaction pour SynthesePage.xaml
    /// </summary>
    public partial class SynthesePage : Page
    {
        public SynthesePage()
        {
            Synthese.ExprBoolNode tree;
            if (Data.literal)
            {
                tree = Synthese.To_N_ary(Data.expression, Data.nb_and, Data.nb_or, Data.nb_nand, Data.nb_nor);
            }
            else
            {
                string expression = "";
                for (int i = 0; i < Data.mintermes.Count; i++)
                {
                    expression += Mintermes.getExpressionDeBincode(Data.literal, Data.mintermes[i].bincode, Data.variables) + "+";
                }
                if (expression.Length > 0)
                    expression = expression.Substring(0, expression.Length - 1);
                tree = Synthese.To_N_ary(expression, Data.nb_and, Data.nb_or, Data.nb_nand, Data.nb_nor);
            }
            Synthese.Circuit_Visualisation(tree);
            Image syntheseImage = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.None;
            bitmap.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.UriSource = new Uri(Directory.GetCurrentDirectory() + "\\synthese.png", UriKind.Relative);
            bitmap.EndInit();
            syntheseImage.Source = bitmap;
            InitializeComponent();
            border.Child = syntheseImage;
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