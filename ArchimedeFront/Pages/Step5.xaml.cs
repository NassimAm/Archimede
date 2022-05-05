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
using System.Diagnostics;
using Archimède;

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Logique d'interaction pour Step5.xaml
    /// </summary>
    public partial class Step5 : Page
    {
        public Step5()
        {
            InitializeComponent();

            Mintermes.getListImpliquantsEssentiaux(ref Data.impliquantsEssentiels, Data.stringListMinterm, Data.impliquantsPremiers);
            /*Generation du premier rang ===========================================================*/
            WrapPanel wrappanel = new WrapPanel();
            wrappanel.Orientation = Orientation.Horizontal;
            wrappanel.VerticalAlignment = VerticalAlignment.Center;
            wrappanel.Margin = new Thickness(0,20, 0, 0);
            wrappanel.Children.Add(new Border() {BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 0, 2, 0), Margin= new Thickness(16*Data.nbVariables+40, 0, 0, 0), Child = null});
            for(int i = 0; i < Data.mintermes.Count; i++)
            {
                Trace.WriteLine(Data.mintermes[i].bincode);
                wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style,HorizontalAlignment=HorizontalAlignment.Center, FontSize = 28,Width = 16 * Data.nbVariables + 40, Text = Data.mintermes[i].bincode});
            }
            Border border = new Border() {BorderBrush=Brushes.Gray, BorderThickness = new Thickness(0, 0, 0, 2), Child = wrappanel };

            ImpliquantsEssentiauxTable.Children.Add(border);

            /*Generation des autres rangs du tableau*/
            WrapPanel wrappanelinterne;

            for(int i=0;i<Data.impliquantsPremiers.Count;i++)
            {
                wrappanelinterne = new WrapPanel();
                wrappanelinterne.Orientation = Orientation.Horizontal;
                wrappanelinterne.VerticalAlignment = VerticalAlignment.Center;
                wrappanelinterne.Margin = new Thickness(0, 10, 0, 10);
                wrappanelinterne.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style,HorizontalAlignment=HorizontalAlignment.Center,Width = 16 * Data.nbVariables + 40, FontSize = 28, Text = Data.impliquantsPremiers[i].bincode});
                border = new Border() { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 0, 2, 0), Child = wrappanelinterne};

                wrappanel = new WrapPanel() {Orientation = Orientation.Horizontal, VerticalAlignment=VerticalAlignment.Center};
                wrappanel.Children.Add(border);

                for(int j=0;j<Data.mintermes.Count;j++)
                {
                    if (Data.impliquantsPremiers[i].represente(Data.mintermes[j]))
                    {
                        if(Data.impliquantsEssentiels.Contains(Data.impliquantsPremiers[i]))
                            wrappanel.Children.Add(generateImpliquantEssentielCase());
                        else
                            wrappanel.Children.Add(generateImpliquantCase());
                    }
                    else
                        wrappanel.Children.Add(generateCaseVide());
                }

                ImpliquantsEssentiauxTable.Children.Add(wrappanel);
            }
        }
        private WrapPanel generateImpliquantCase()
        {
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Width = Data.nbVariables * 16 + 40;
            panel.Height = 15;
            panel.Margin = new Thickness(0, 10, 0, 10);
            panel.Children.Add(new Ellipse() { Style = FindResource("ImpliquantNonEssentielCercle") as Style, Margin = new Thickness((panel.Width - 15) / 2, 0, 0, 0) });
            return panel;
        }
        private WrapPanel generateImpliquantEssentielCase()
        {
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Width = Data.nbVariables * 16 + 40;
            panel.Height = 19;
            panel.Margin = new Thickness(0, 10, 0, 10);
            panel.Children.Add(new Border() { Style=FindResource("ImpliquantEssentielBordure") as Style,Margin = new Thickness((panel.Width - 15) / 2, 0, 0, 0),Child = new Ellipse() {Style = FindResource("ImpliquantEssentielCercle") as Style } });
            return panel;
        }
        private WrapPanel generateCaseVide()
        {
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Width = Data.nbVariables * 16 + 40;
            panel.Height = 15;
            panel.Margin = new Thickness(0, 10, 0, 10);
            return panel;
        }
    }
}
