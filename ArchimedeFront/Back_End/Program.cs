/*using Archimède;
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
            int impliquantIndex = -1;
            int mintermeIndex = 0;
            int impliquantLevel = 1;
            int count;


            //Tant que la liste des mintermes n'est pas vide
            while (stringListMinterm.Count > 0)
            {

                mintermeIndex = 0;
                while (mintermeIndex < stringListMinterm.Count)
                {
                    count = 0;
                    impliquantIndex = -1;
                    for (int j = 0; j < impliquantsPremiers.Count; j++)
                    {
                        //Si l'impliquant j peut représenter le minterme i
                        if (impliquantsPremiers[j].represente(stringListMinterm[mintermeIndex]))
                        {
                            count += 1;
                            impliquantIndex = j;
                        }
                    }
                    Si le minterme i est représenté par impliquantLevel impliquant(s) initialement si le minterme i
                    est représenté uniquement par un impliquant donc il ajouté à la liste des impliquants essentiels
                    et supprimer le minterme de la liste des mintermes car il a ainsi été traité
                    if (count == impliquantLevel)
                    {

                        impliquantsEssentiels.Add(impliquantsPremiers[impliquantIndex]);

                        stringListMinterm.RemoveAt(mintermeIndex);

                    }
                    else //Sinon avancer dans la liste des mintermes
                    {
                        mintermeIndex += 1;
                    }
                }


                //Vérifier si en choisissant les impliquants essentiels quels mintermes on a traité
                mintermeIndex = 0;
                while (mintermeIndex < stringListMinterm.Count)
                {
                    count = 0;
                    for (int j = 0; j < impliquantsEssentiels.Count; j++)
                    {
                        //Si l'impliquant essentiel j peut représenter le minterme i
                        if (impliquantsEssentiels[j].represente(stringListMinterm[mintermeIndex]))
                        {
                            count += 1;
                        }
                    }
                    Si le minterme est représenté par au moins un impliquant essentiel donc ce minterme est supprimé de la liste des mintermes
                     car il a ainsi été traité
                    if (count > 0)
                    {
                        stringListMinterm.RemoveAt(mintermeIndex);
                    }
                    else //Sinon avancer dans la liste des mintermes
                    {
                        mintermeIndex += 1;
                    }
                }
                impliquantLevel += 1;



            }

            //Supprimer les doublons s'ils existent
            impliquantsEssentiels = impliquantsEssentiels.Distinct().ToList();
            #endregion

            #region RESULTAT

            StringBuilder alphabet;
            Console.WriteLine("Resultat de simplification : ");
            String resultat = "";
            string alpha;
            for (int i = 0; i < impliquantsEssentiels.Count; i++)
            {
                for (int j = 0; j < impliquantsEssentiels[i].bincode.Length; j++)
                {
                    if (impliquantsEssentiels[i].bincode[j] != '-')
                    {

                        if (literale)
                        {
                            alpha = alphabets[j];
                        }
                        else
                        {

                            alphabet = new StringBuilder(" ");

                            if (j >= 26)
                            {
                                alphabet = new StringBuilder("  ");


                                alphabet[0] = (char)((65 + j / 26 - 1));
                                alphabet[1] = (char)(65 + (j % 26));

                            }
                            else alphabet[0] = (char)(65 + j);

                            alpha = alphabet.ToString();

                        }

                        //Nommer les variables dans l'ordre alphabétique
                        if (impliquantsEssentiels[i].bincode[j] == '1')
                        {
                            resultat += alpha + ".";
                        }
                        else
                        {
                            resultat += "!" + alpha + ".";
                        }
                    }
                }
                //Enlever le " . " additionnel à la fin
                if (resultat.Length >= 1)
                {
                    resultat = resultat.Substring(0, resultat.Length - 1);
                }
                resultat += "+";
            }

            //Enlever le " + " additionnel à la fin
            if (resultat.Length >= 1)
            {
                resultat = resultat.Substring(0, resultat.Length - 1);
            }

            Console.WriteLine(resultat);

            Synthese.ExprBoolNode tree = Synthese.N_ary_DNF_ExpressionTree(resultat);
            Synthese.Circuit_Visualisation(tree);
            #endregion

        }
    }
}
*/