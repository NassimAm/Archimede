using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            List<Impliquant> groupe;
            for(int i = 0; i< Data.groupeMintermes.groupesImpliquants.Length; i++)
            {
                groupe = Data.groupeMintermes.groupesImpliquants[i];
                if(groupe.Count > 0) {
                    groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style ,FontSize=22 , Margin=new Thickness(36,6,36,6) , Text="Poid "+i , FontWeight=FontWeights.Bold , HorizontalAlignment=HorizontalAlignment.Left , TextAlignment=TextAlignment.Left});
                    foreach(Impliquant impliquant in groupe)
                    {
                        groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style ,Padding=new Thickness(4,0,4,0) ,FontSize = 28 , Margin = new Thickness(36,2,36,2) , Text = impliquant.bincode});
                    }
                
                    border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 8, 36, 8) , Width = nbVariables*16 , Child = null  };
                    groupesTable.Children.Add(border);
                }
            }


     

            if (Data.literal && Data.impliquantsEnAttente.Count > 0)
            {
                // linear gradiant background for impliquants en attente

                LinearGradientBrush LinearBrush = new LinearGradientBrush();
                LinearBrush.StartPoint = new Point(0, 0);
                LinearBrush.EndPoint = new Point(0, 1);
                LinearBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#00CBBD"), 0.1));
                LinearBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#00E17C"), 1));
                //-------
                groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 22, Margin = new Thickness(36, 6, 36, 6), Text = "Impliquant en attente", FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Left, TextAlignment = TextAlignment.Left });

                foreach (Impliquant impliquant in Data.impliquantsEnAttente)
                {
                    groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, Foreground= LinearBrush,FontSize = 28, Margin = new Thickness(36, 2, 36, 2), Text = impliquant.bincode });
                }

                border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 4, 36, 4), Width = nbVariables * 16, Child = null };
                groupesTable.Children.Add(border);
            }
          


            groupesTable.Children.RemoveAt(groupesTable.Children.Count - 1);


           

        }



    }
}
