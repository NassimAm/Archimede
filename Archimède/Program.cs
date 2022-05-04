using Archimède;
using dnf;
using System.Text;

namespace Archimède
{
    class Program
    {
        static void Main(string[] args)
        {
            #region CHOIX

            bool literale;
            string choix;
            Console.WriteLine(">Voulez vous introduire une formule de forme litterale ou numerique ? ");
            Console.WriteLine("1- Litterale\t2- Numerique");
            choix = Console.ReadLine();
            literale = (choix[0] == '1');

            #endregion

            #region DECLARATIONS
            List<string> stringListMinterm = new List<string>(); // liste des codes binaires du chaque minterms 
            List<string> alphabets = new List<string>();
            List<Impliquant> impliquantsEnAttente = new List<Impliquant>();
            List<Impliquant> impliquants = new List<Impliquant>();
            Mintermes groupeMintermes;
            List<Minterme> mintermes;
            #endregion


            if (literale)
            {
                #region INUPT_LITTERALE
                Console.WriteLine("Entrez une expression en forme disjonctif:");
                string expression = Console.ReadLine().Replace(" ", "");


                alphabets = ExprBool.getVariables(expression);
                int nbVariables = alphabets.Count;
                stringListMinterm = ExprBool.getMinterms(expression, alphabets);

                int maxNbUns = stringListMinterm.MaxBy(x => x.Count(ch => (ch == '1' || ch == '-'))).Count(ch => (ch == '1' || ch == '-'));


                foreach (string mintermBinCode in stringListMinterm)
                {
                    Impliquant impliquant = new Impliquant(mintermBinCode);
                    if (impliquant.nbDontCare > 0) impliquantsEnAttente.Add(impliquant); // ces impliquants vont etre traités dans les prochaine groupe 
                    else impliquants.Add(impliquant); // impliquants  en forme canonique 
                }

                groupeMintermes = new Mintermes(maxNbUns);

                #endregion
            }
            else
            {
                #region INPUT_NUMERIQUE
                Console.Write("Entrez le nombre de variables : ");
                int nbVariables = int.Parse(Console.ReadLine());

                Console.WriteLine("Entrez la liste de mintermes (séparés par des virgules) : ");
                string mintermesString = Console.ReadLine();

                string[] listMintermesString = mintermesString.Split(',');


                //Vérifier le nombre maximal de 1 (Calculer le nombre de groupes à créer)
                //En meme temps vérifier le nombre minimal de variables qui correspond à la liste de mintermes

                mintermes = new List<Minterme>();
                long parsedInt;

                for (int i = 0; i < listMintermesString.Length; i++)
                {

                    try
                    {
                        parsedInt = long.Parse(listMintermesString[i]);
                        mintermes.Add(new Minterme(parsedInt));
                    }
                    catch (OverflowException)
                    {
                        mintermes.Add(new Minterme(listMintermesString[i]));
                    }

                }


                int maxNbUns = Minterme.maxNbUns;
                int maxMintermeLong = Minterme.maxNbVariables;


                //Dans le cas où le nombre de variables introduit ne correspond pas à la liste de mintermes introduites
                if (maxMintermeLong > nbVariables)
                {
                    Console.WriteLine("Avertissement : La liste de mintermes introduite depasse le nombre maximal de variables introduit");
                    Console.WriteLine("On travaillera donc suivant la liste de mintermes donc avec " + maxMintermeLong.ToString() + " variables");
                    nbVariables = maxMintermeLong;
                }




                //Corriger les codes binaires (en ajoutant des zéros au début pour qu'ils aient tous la mê^me longueur)
                for (int i = 0; i < mintermes.Count; i++)
                {
                    mintermes[i].bincode = mintermes[i].bincode.PadLeft(nbVariables, '0');
                    stringListMinterm.Add(mintermes[i].bincode);
                }


                groupeMintermes = new Mintermes(maxNbUns);
                impliquants = groupeMintermes.InitImpliquants(mintermes);
                stringListMinterm = stringListMinterm.Distinct().ToList();

                #endregion
            }

            #region GROUPAGE
            List<Impliquant> impliquantsPremiers = new List<Impliquant>();
            groupeMintermes.GrouperListes(impliquants);


            List<Impliquant> impliquantsEssentiels = new List<Impliquant>();

            int cptGroupes = 0; // compteur de nombre de groupages
            bool stop = false;
            while (!stop)
            {

                cptGroupes++;
                stop = groupeMintermes.generateNextGroupe(literale, cptGroupes, impliquantsEnAttente, impliquantsPremiers);
            }
            impliquantsPremiers = impliquantsPremiers.Distinct().ToList();
            #endregion


            #region IMPLIQUANTS_ESSENTIELS
            Mintermes.getListImpliquantsEssentiaux(ref impliquantsEssentiels, stringListMinterm, impliquantsPremiers);
            #endregion

            #region RESULTAT
            Console.WriteLine("Resultat de simplification : ");
            string resultat = Mintermes.getResultatExpression(literale, impliquantsEssentiels, alphabets);
            Console.WriteLine(resultat);
            #endregion

            #region SYNTHESE
            Console.Write("Expression (Test Synthese): ");
            string exp =  Console.ReadLine();
            Synthese.ExprBoolNode tree = Synthese.To_N_ary(exp);
            Synthese.Affich_Arbre(tree);
            Synthese.Tree_Visualisation(tree);
            Synthese.Circuit_Visualisation(tree);
            #endregion
        }
    }
}
