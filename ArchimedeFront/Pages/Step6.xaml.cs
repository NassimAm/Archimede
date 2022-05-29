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
using Archimède;

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Logique d'interaction pour Step6.xaml
    /// </summary>
    public partial class Step6 : Page
    {
        public Step6()
        {
            InitializeComponent();
            string resultat;
            if (Data.resultatFaux )
            {
                FonctionSimplifieContainer.Text = "FAUX";
                resultat = "0";
            }
            else
            {
                if(Data.codeTransformation == '1')
                {
                     resultat = Mintermes.getResultatExpressionCNF(Data.literal, Data.impliquantsEssentiels, Data.variables);
                }
                else
                {
                     resultat = Mintermes.getResultatExpressionDNF(Data.literal, Data.impliquantsEssentiels, Data.variables);
                }

                if (resultat.Length == 0)
                {
                    FonctionSimplifieContainer.Text = "VRAI";
                    resultat = "1";
                }
                else {
                    FonctionSimplifieContainer.Text = resultat;
                }
                   
            }
            Data.expression = resultat;

        }
    }
}
