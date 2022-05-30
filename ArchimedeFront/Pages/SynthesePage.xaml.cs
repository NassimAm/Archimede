using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Win32;

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
            Dictionary<string, int> gates = new Dictionary<string, int>();
            if (Data.literal)
            {
                tree = Synthese.To_N_ary(Data.expression, Data.nb_and, Data.nb_or, Data.nb_nand,Data.nb_nor,Data.nb_xor,Data.nb_xnor,gates);
                Data.variables = dnf.ExprBool.getVariables(Data.expression);
            }
            else
            {
                string expression = "";
                for (int i = 0; i < Data.mintermes.Count; i++)
                {
                    expression += Mintermes.getExpressionDeBincode(Data.literal, Data.mintermes[i].bincode.PadLeft(Data.nbVariables, '0'), Data.variables) + "+";
                }
                if (expression.Length > 0)
                    expression = expression.Substring(0, expression.Length - 1);
                tree = Synthese.To_N_ary(expression, Data.nb_and, Data.nb_or, Data.nb_nand, Data.nb_nor, Data.nb_xor, Data.nb_xnor, gates);
            }
            Synthese.Circuit_Visualisation(tree,Data.variables);
            Image syntheseImage = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.None;
            bitmap.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.UriSource = new Uri(Directory.GetCurrentDirectory() + "\\Synthese\\synthese.png", UriKind.Relative);
            bitmap.EndInit();
            syntheseImage.Source = bitmap;
            InitializeComponent();
            border.Child = syntheseImage;

            foreach(String key in gates.Keys)
            {
                if(gates[key] != 0)
                {
                    TextBlock txtbloc;
                    if(gates[key] != 1)
                        txtbloc = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left, Style = FindResource("paragraphe") as Style, FontSize = 20, Text=String.Format("{0} Portes "+key,gates[key])};
                    else
                        txtbloc = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left, Style = FindResource("paragraphe") as Style, FontSize = 20, Text = String.Format("{0} Porte " + key,gates[key])};

                    numberGatesPanel.Children.Add(new Border()
                    {
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(20),
                        Padding = new Thickness(10),
                        BorderBrush = new LinearGradientBrush(new GradientStopCollection()
                        {
                            new GradientStop((Color)ColorConverter.ConvertFromString("#FF00CBBD"),0.1),
                            new GradientStop((Color)ColorConverter.ConvertFromString("#FF00E17C"),1),
                        }, new Point(0, 0), new Point(0, 1)),
                        Margin = new Thickness(10,0,10,0),
                        Child = txtbloc,
                    });
                }
            }
        }

        private void returnButton_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/InputFormula.xaml", UriKind.Absolute));
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog()
            {
                InitialDirectory = @"C:\",     
                Title = "Sauvegarder la Synthèse",
                DefaultExt = "png",
                Filter = "Png files (*.png)|*.png|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() == true)
            {
                string sourceFile = Directory.GetCurrentDirectory()+"\\Synthese\\synthese.png";
                string destinationFile = dlg.FileName;
                try
                {
                    File.Copy(sourceFile, destinationFile, true);
                }
                catch (IOException iox)
                {
                    Trace.WriteLine(iox.Message);
                }
            }
        }
    }
}