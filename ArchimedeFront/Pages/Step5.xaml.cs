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
using System.IO;

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

            string? path = Directory.GetCurrentDirectory() + "\\step5.txt";
            File.WriteAllText(path, "");
            File.AppendAllText(path, "Table d'impliquants essentiaux\n");
            File.AppendAllText(path, "V:Vide  R:Représente  E:Essentiel\n\n");

            Mintermes.getListImpliquantsEssentiaux(ref Data.impliquantsEssentiels, Data.stringListMinterm, Data.impliquantsPremiers, Data.impliquantsType);
            /*Generation du premier rang ===========================================================*/
            WrapPanel wrappanel = new WrapPanel();
            wrappanel.Orientation = Orientation.Horizontal;
            wrappanel.VerticalAlignment = VerticalAlignment.Center;
            wrappanel.Margin = new Thickness(0, 20, 0, 0);
            TopLeftCase.Children.Add(new Border() { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 0, 2, 2), Width = 16 * Data.nbVariables + 42, Height = 35, Child = null });

            string topTableString = " " + new string(' ', Data.nbVariables) + " |";
            for (int i = 0; i < Data.stringListMinterm.Count; i++)
            {
                topTableString += " " + Data.stringListMinterm[i] + " |";
                TextBlock tb = new TextBlock() { Style = FindResource("paragraphe") as Style, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 28, Width = 16 * Data.nbVariables + 40, Text = Data.stringListMinterm[i] };
                /*Style st = new Style();
                for(int j=0;j<Data.impliquantsPremiers.Count;j++)
                {
                    DataTrigger tg = new DataTrigger() { Binding = new Binding() { Path = new PropertyPath("IsMouseOver"), ElementName = String.Format("Wrappanel{0}{1}", i, j) } };
                    tg.Setters.Add(new Setter()
                    {
                        Property = Control.BackgroundProperty,
                        Value = Brushes.Green
                    });
                    st.Triggers.Add(tg);
                }
                
                tb.Style = st;*/
                wrappanel.Children.Add(tb);
            }
            Border border = new Border() { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 0, 0, 2), Child = wrappanel };

            topTableString += "\n";
            File.AppendAllText(path, topTableString);


            MintermeTable.Children.Add(border);

            /*Generation des autres rangs du tableau*/
            WrapPanel wrappanelinterne;

            for (int i = 0; i < Data.impliquantsPremiers.Count; i++)
            {
                wrappanelinterne = new WrapPanel();
                wrappanelinterne.Orientation = Orientation.Horizontal;
                wrappanelinterne.VerticalAlignment = VerticalAlignment.Center;
                wrappanelinterne.Margin = new Thickness(0, 10, 0, 10);
                wrappanelinterne.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, HorizontalAlignment = HorizontalAlignment.Center, Width = 16 * Data.nbVariables + 40, FontSize = 28, Text = Data.impliquantsPremiers[i].bincode });
                border = new Border() { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 0, 2, 0), Child = wrappanelinterne };

                ImpliquantPremiersTable.Children.Add(border);

                wrappanel = new WrapPanel() { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 7.5, 0, 7.5) };

                string rowTableString = " " + Data.impliquantsPremiers[i].bincode + " |";
                for (int j = 0; j < Data.stringListMinterm.Count; j++)
                {
                    rowTableString += " " + new string(' ', Convert.ToInt32((Data.nbVariables - 1) / 2));
                    if (Data.impliquantsType[i][j] == 'E')
                    {
                        rowTableString += "E";
                        wrappanel.Children.Add(generateImpliquantEssentielCase(i, j));
                    }
                    else
                    {
                        if (Data.impliquantsType[i][j] == 'R')
                        {
                            rowTableString += "R";
                            wrappanel.Children.Add(generateImpliquantCase(i, j));
                        }
                        else
                        {
                            rowTableString += "V";
                            wrappanel.Children.Add(generateCaseVide(i, j));
                        }
                    }
                    rowTableString += new string(' ', Convert.ToInt32((Data.nbVariables - 1) / 2)) + " |";
                }
                rowTableString += "\n";
                File.AppendAllText(path, rowTableString);

                ImpliquantsEssentiauxTable.Children.Add(wrappanel);
            }
        }
        private WrapPanel generateImpliquantCase(int i, int j)
        {
            WrapPanel wrappanel = new WrapPanel();
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Width = Data.nbVariables * 16 + 40;
            panel.Height = 15;
            panel.Margin = new Thickness(0, 10, 0, 10);
            panel.Children.Add(new Ellipse() { Style = FindResource("ImpliquantNonEssentielCercle") as Style, Margin = new Thickness((panel.Width - 15) / 2, 0, 0, 0) });
            wrappanel.Children.Add(panel);

            Style st = new Style();
            Trigger tg = new Trigger() { Property = Control.IsMouseOverProperty, Value = true };
            tg.Setters.Add(new Setter()
            {
                Property = Control.BackgroundProperty,
                Value = Brushes.Green
            });

            st.Triggers.Add(tg);
            wrappanel.Style = st;
            wrappanel.Name = String.Format("Wrappanel{0}{1}", i, j);
            return wrappanel;
        }
        private WrapPanel generateImpliquantEssentielCase(int i, int j)
        {
            WrapPanel wrappanel = new WrapPanel();
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Width = Data.nbVariables * 16 + 40;
            panel.Height = 19;
            panel.Margin = new Thickness(0, 10, 0, 10);
            panel.Children.Add(new Border() { Style = FindResource("ImpliquantEssentielBordure") as Style, Margin = new Thickness((panel.Width - 15) / 2, 0, 0, 0), Child = new Ellipse() { Style = FindResource("ImpliquantEssentielCercle") as Style } });
            wrappanel.Children.Add(panel);

            Style st = new Style();
            Trigger tg = new Trigger() { Property = Control.IsMouseOverProperty, Value = true };
            tg.Setters.Add(new Setter()
            {
                Property = Control.BackgroundProperty,
                Value = Brushes.Green
            });
            st.Triggers.Add(tg);
            wrappanel.Style = st;
            wrappanel.Name = String.Format("Wrappanel{0}{1}", i, j);
            return wrappanel;
        }
        private WrapPanel generateCaseVide(int i, int j)
        {
            WrapPanel wrappanel = new WrapPanel();
            WrapPanel panel = new WrapPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Width = Data.nbVariables * 16 + 40;
            panel.Height = 15;
            panel.Margin = new Thickness(0, 10, 0, 10);
            wrappanel.Children.Add(panel);

            Style st = new Style();
            Trigger tg = new Trigger() { Property = Control.IsMouseOverProperty, Value = true };
            tg.Setters.Add(new Setter()
            {
                Property = Control.BackgroundProperty,
                Value = Brushes.Green
            });
            st.Triggers.Add(tg);
            wrappanel.Style = st;
            wrappanel.Name = String.Format("Wrappanel{0}{1}", i, j);
            return wrappanel;
        }

        public void ChangeBgColor(int i, int j)
        {
            ((WrapPanel)((Border)ImpliquantPremiersTable.Children[i]).Child).Background = Brushes.Green;
            ((WrapPanel)((Border)MintermeTable.Children[j]).Child).Background = Brushes.Green;
        }
        private void Minterme_Scroll(object sender, ScrollChangedEventArgs e)
        {
            ImpliquantsEssentiauxScroll.ScrollToHorizontalOffset(MintermeScroll.HorizontalOffset);
            MintermeScroll.UpdateLayout();
        }

        private void ImpliquantPremiers_Scroll(object sender, ScrollChangedEventArgs e)
        {
            ImpliquantsEssentiauxScroll.ScrollToVerticalOffset(ImpliquantPremiersScroll.VerticalOffset);
            ImpliquantPremiersScroll.UpdateLayout();
        }

        private void ImpliquantsEssentiaux_Scroll(object sender, ScrollChangedEventArgs e)
        {
            ImpliquantPremiersScroll.ScrollToVerticalOffset(ImpliquantsEssentiauxScroll.VerticalOffset);
            MintermeScroll.ScrollToHorizontalOffset(ImpliquantsEssentiauxScroll.HorizontalOffset);
            ImpliquantPremiersScroll.UpdateLayout();
            MintermeScroll.UpdateLayout();
        }
    }
}