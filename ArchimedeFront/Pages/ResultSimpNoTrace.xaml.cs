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
    /// Interaction logic for ResultSimpNoTrace.xaml
    /// </summary>
    public partial class ResultSimpNoTrace : Page
    {
        public ResultSimpNoTrace()
        {
            InitializeComponent();
            if (Data.literal)
            {



                int maxNbUns = Data.stringListMinterm.MaxBy(x => x.Count(ch => (ch == '1' || ch == '-'))).Count(ch => (ch == '1' || ch == '-'));

                foreach (string mintermBinCode in Data.stringListMinterm)
                {
                    Impliquant impliquant = new Impliquant(mintermBinCode);
                    if (impliquant.nbDontCare > 0) Data.impliquantsEnAttente.Add(impliquant); // ces impliquants vont etre traités dans les prochaine groupe 
                    else (Data.impliquants).Add(impliquant); // impliquants  en forme canonique 
                }

                Data.groupeMintermes = new Mintermes(maxNbUns);
                



            }
            else
            {

                //Corriger les codes binaires (en ajoutant des zéros au début pour qu'ils aient tous la mê^me longueur)
                for (int i = 0; i < Data.mintermes.Count; i++)
                {
                    Data.mintermes[i].bincode = Data.mintermes[i].bincode.PadLeft(Data.nbVariables, '0');
                    Data.stringListMinterm.Add(Data.mintermes[i].bincode);

                }
                Data.groupeMintermes = new Mintermes(Minterme.maxNbUns);
                Data.impliquants = Data.groupeMintermes.InitImpliquants(Data.mintermes);
                Data.stringListMinterm = Data.stringListMinterm.Distinct().ToList();
                //mintermes = Data.listMintermesString;

                //expression.Text = string.Join(" ,", Data.listMintermesString);
            }

           
            Data.impliquantsEssentiels = Mintermes.simplifyMintermes(Data.impliquants, Data.impliquantsEnAttente, Data.groupeMintermes, Data.stringListMinterm, Data.literal);
            string resultat;
            if (Data.codeTransformation == '1')
            {
                resultat = Mintermes.getResultatExpressionCNF(Data.literal, Data.impliquantsEssentiels, Data.variables);
                if (resultat.Length == 0)
                {
                    FonctionSimplifie.Text = "Faux";
                    Data.expression = "0";
                }
                else {
                     FonctionSimplifie.Text = resultat;
                     Data.expression = resultat;
                }
            }
            else
            {
                resultat = Mintermes.getResultatExpressionDNF(Data.literal, Data.impliquantsEssentiels, Data.variables);
                if (resultat.Length == 0) FonctionSimplifie.Text = "Vrai";
                else FonctionSimplifie.Text = resultat;
            }


           


        }

        private void syntheseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/SynthesePage.xaml", UriKind.Absolute));
        }
    }
}
