using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Archimède;
namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Interaction logic for Step2.xaml
    /// </summary>
    public partial class Step2 : Page
    {

        public Step2()
        {
            int nbVariables = Data.nbVariables;
            Data.groupeMintermes.GrouperListes(Data.impliquants);


            InitializeComponent();

            Border border;

            string? path = Directory.GetCurrentDirectory() + "\\step2.txt";
            File.WriteAllText(path, "");
            int cpt = 0;
            foreach (List<Impliquant> groupe in Data.groupeMintermes.groupesImpliquants)
            {
                File.AppendAllText(path, String.Format("Groupage {0} :\n",cpt));
                if (groupe.Count > 0) { 
                    foreach(Impliquant impliquant in groupe)
                    {
                        File.AppendAllText(path, "o "+impliquant.bincode+"\n");
                        groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style , FontSize = 28 , Margin = new Thickness(36,2,36,2) , Text = impliquant.bincode});
                    }
                
                    border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 8, 36, 8) , Width = nbVariables*16 , Child = null  };
                    groupesTable.Children.Add(border);
                }
                cpt += 1;
            }

            File.AppendAllText(path, "\n");

            if (Data.literal && Data.impliquantsEnAttente.Count > 0)
            {
                // linear gradiant background for impliquants en attente
                File.AppendAllText(path, "Impliquants en attente :\n");
                LinearGradientBrush LinearBrush = new LinearGradientBrush();
                LinearBrush.StartPoint = new Point(0, 0);
                LinearBrush.EndPoint = new Point(0, 1);
                LinearBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#00CBBD"), 0.1));
                LinearBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#00E17C"), 1));
                //-------

                foreach (Impliquant impliquant in Data.impliquantsEnAttente)
                {
                    File.AppendAllText(path,"o " + impliquant.bincode + "\n");
                    groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, Foreground= LinearBrush,FontSize = 28, Margin = new Thickness(36, 2, 36, 2), Text = impliquant.bincode });
                }

                border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 4, 36, 4), Width = nbVariables * 16, Child = null };
                groupesTable.Children.Add(border);
            }

            groupesTable.Children.RemoveAt(groupesTable.Children.Count - 1);


           

        }



    }
}
