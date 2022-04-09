using Archimède;
using dnf;
using System.Text;


bool literale = true;
bool completionVariables = false;
bool utiliserPetrick = false;

//Variables globales  pour les 2 litterale et numerique :
int maxNbUns;
List<Impliquant> impliquantsEnAttente = new List<Impliquant>();
List<Impliquant> impliquants = new List<Impliquant>();
List<Minterme> mintermes;
Mintermes groupeMintermes;
List<string> stringListMinterm = new List<string>(); // liste des codes binaires du chaque minterms 
StringBuilder alphabet;
List<string> alphabets = new List<string>();
int nbVariables;

string choix;
Console.WriteLine("Voulez vous introduire une formule de forme litterale ou numerique ? ");
Console.WriteLine("1- Litterale\t2- Numerique");
choix = Console.ReadLine();
switch(choix)
{
    case "1":
        {
            literale = true;
            break;
        }
    default:
        {
            literale = false;
            break;
        }
}

Console.WriteLine("Voulez vous utiliser la methode de Petrick ? ");
Console.WriteLine("1- Oui\t2- Non");
choix = Console.ReadLine();
switch (choix)
{
    case "1":
        {
            utiliserPetrick = true;
            break;
        }
    default:
        {
            utiliserPetrick = false;
            break;
        }
}

if (literale)
{
    if(!utiliserPetrick)
    {
        Console.WriteLine("Voulez vous activer la completion de variables ?");
        Console.WriteLine("1- Oui\t2- Non");
        choix = Console.ReadLine();
        switch (choix)
        {
            case "1":
                {
                    completionVariables = true;
                    break;
                }
            default:
                {
                    completionVariables = false;
                    break;
                }
        }
    }
    else
    {
        completionVariables = true;
    }

    Console.WriteLine("Entrez une expression en forme disjonctif:");
    string expression = Console.ReadLine().Replace(" ","");
    
    int indexCh = 0; // index pour parcourir l'expression 

        //premier parcour de l'expression pour determiner les variables utilitses avec les poids fort et faible 
        while (indexCh < expression.Length)
        {

            alphabet = new StringBuilder(); // alphabet represente la variable (puisque on peut aller a plus de 26 variables l'alphabet sont des string aaa, aabadf ... )

            //une boucle pour separer  les operateurs du l'alphabet 
            while ((indexCh < expression.Length) && (expression[indexCh] != '+') && (expression[indexCh] != '.') && (expression[indexCh] != '!'))
            {

                alphabet.Append(expression[indexCh]);
                indexCh++;
            }

            //si cette alphabet n'est pas traites deja on l'insere dans la liste d'alphabets
            if ((alphabet.Length > 0) && (!alphabets.Contains(alphabet.ToString())))
            {
                alphabets.Add(alphabet.ToString());
            }

            indexCh++;
        }


    nbVariables = alphabets.Count; // le nombre de variables = nombre d'alphabets

    //petit affichage des variables 
    Console.WriteLine("Nombre de variables = " + nbVariables);
    Console.Write("Variables : [ ");
        foreach (string x in alphabets)
        {
            Console.Write(x + ",");
        }
    Console.WriteLine(" ]");




    indexCh = 0;
    
    StringBuilder bincode;//code binaire d'un minterme 
    StringBuilder term;//minterme

    //deuxieme parcour pour trouver les mintermes avec leurs code binaire 
    while (indexCh < expression.Length)
    {
        bincode = new StringBuilder();
        term = new StringBuilder();

        //sparer le minterme dans la variable term 
        while ((indexCh < expression.Length) && (expression[indexCh] != '+'))
        {
            term.Append(expression[indexCh]);
            indexCh++;
        }

        string[] termAlphabets = term.ToString().Split('.'); //tableau qui va contenir les varibales present dans un minterm 

        for (int i = 0; i < alphabets.Count; i++)
        {


            if (termAlphabets.Contains(alphabets[i]) && termAlphabets.Contains("!" + alphabets[i]))
            {
                // a.!a = vide 
                bincode.Clear();
                break;

            }
            else if (termAlphabets.Contains(alphabets[i]))
            {
                // a

                bincode.Append('1');
            }
            else if (termAlphabets.Contains("!" + alphabets[i]))
            {
                // !a 
                bincode.Append('0');
            }
            else
            {
                // a n'est pas present donc soit 0 soit 1 => -
                bincode.Append('-');
            }
        }

        if (bincode.Length > 0) stringListMinterm.Add(bincode.ToString());
        indexCh++;
    }

    stringListMinterm = stringListMinterm.Distinct().ToList();

    List<string> newStringListMinterm = new List<string>();
    List<string> newTempStringListMinterm = new List<string>();
    int buildIndex = 0;
    if(completionVariables)
    {
        for(int i=0;i<stringListMinterm.Count;i++)
        {
            if(stringListMinterm[i].Count(ch => (ch == '-')) == 0)
            {
                newStringListMinterm.Add(stringListMinterm[i]);
            }
            else
            {
                newTempStringListMinterm.Add(stringListMinterm[i]);
                while(newTempStringListMinterm[0].Count(ch => (ch=='-')) > 0)
                {
                    buildIndex = newTempStringListMinterm[0].IndexOf('-');
                    StringBuilder sb = new StringBuilder(newTempStringListMinterm[0]);
                    sb[buildIndex] = '0';
                    newTempStringListMinterm.Add(sb.ToString());
                    sb[buildIndex] = '1';
                    newTempStringListMinterm.Add(sb.ToString());

                    newTempStringListMinterm.RemoveAt(0);
                }
                newStringListMinterm.AddRange(newTempStringListMinterm);
                newTempStringListMinterm.Clear();
            }
        }
        stringListMinterm.Clear();
        stringListMinterm = newStringListMinterm;
        stringListMinterm = stringListMinterm.Distinct().ToList();
    }

    //affichage du code binaire des mintermes 
    Console.WriteLine("Minterms : ");
    foreach (string x in stringListMinterm)
    {
        Console.Write(x + "|");
    }
    Console.WriteLine();



    //calcul de max nombre des uns dans les mintermes
    //en fait prend en consideration les (-) comme des 1 
     maxNbUns = stringListMinterm.MaxBy(x => x.Count(ch => (ch == '1' || ch == '-' )   )).Count(ch =>  (ch == '1' || ch == '-'));

 
    foreach (string mintermBinCode in stringListMinterm)
    {
        Impliquant impliquant = new Impliquant(mintermBinCode); 
        if(impliquant.nbDontCare > 0) impliquantsEnAttente.Add(impliquant); // ces impliquants vont etre traités dans les prochaine groupe 
        else impliquants.Add(impliquant); // impliquants  en forme canonique 
    }

    groupeMintermes = new Mintermes(maxNbUns);


}
else {


    Console.Write("Entrez le nombre de variables : ");
    nbVariables = int.Parse(Console.ReadLine());

    Console.WriteLine("Entrez la liste de mintermes (séparés par des virgules) : ");
    string mintermesString = Console.ReadLine();

    string[] listMintermesString = mintermesString.Split(',');



    //Vérifier le nombre maximal de 1 (Calculer le nombre de groupes à créer)
    //En meme temps vérifier le nombre minimal de variables qui correspond à la liste de mintermes


    mintermes = new List<Minterme>();
    
    
    long parsedInt;

    for (int i = 0;i<listMintermesString.Length;i++)
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




    maxNbUns = Minterme.maxNbUns;
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


}










//Créer les groupes ===================================================================================================================
//Les groupes sont formés d'impliquants initiaux (les mintermes introduits)


List<Impliquant> impliquantsPremiers = new List<Impliquant>();
groupeMintermes.GrouperListes(impliquants );


//Petit affichage de groupage initial
/*Console.WriteLine("\t1-Groupage initial :");
for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
{
    for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
    {
        Console.WriteLine(groupeMintermes.groupesImpliquants[i][j].bincode);
    }
    Console.WriteLine("----------------------------------");
}
*/

//Générer les impliquants premiers
int count = 0;
int differentAt = -1;
bool stop = false;

int cptGroupes = 0; // compteur de nombre de groupages

List<Impliquant> impliquantsEssentiels = new List<Impliquant>();

if (!utiliserPetrick) //Quine McCluskey seul
{
    while (!stop)
    {

        cptGroupes++;

        impliquants.Clear();


        for (int i = 0; i < groupeMintermes.groupesImpliquants.Length - 1; i++)
        {


            for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
            {

                //premiere boucle pour trouver les adjacents avec "groupesImpliquants[i + 1]"
                for (int k = 0; k < groupeMintermes.groupesImpliquants[i + 1].Count; k++)
                {
                    count = 0;
                    differentAt = -1;
                    for (int l = 0; l < groupeMintermes.groupesImpliquants[i][j].bincode.Length; l++)
                    {


                        if (groupeMintermes.groupesImpliquants[i][j].bincode[l] != groupeMintermes.groupesImpliquants[i + 1][k].bincode[l])
                        {
                            count += 1;
                            differentAt = l;
                        }

                        if (count > 1) break;

                    }
                    //Si les deux impliquants sont adjacents
                    if (count == 1)
                    {
                        //Les deux impliquants sont traités (status = false)
                        groupeMintermes.groupesImpliquants[i][j].status = false;
                        groupeMintermes.groupesImpliquants[i + 1][k].status = false;


                        //Actualiser le code binaire et le simplifier
                        StringBuilder sb = new StringBuilder(groupeMintermes.groupesImpliquants[i][j].bincode);
                        sb[differentAt] = '-';

                        //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                        impliquants.Add(new Impliquant(sb.ToString()));
                    }
                }


                //deuxieme  boucle pour trouver les adjacents avec les impliquants en attente
                for (int k = 0; k < impliquantsEnAttente.Count; k++)
                {

                    count = 0;
                    differentAt = -1;

                    for (int l = 0; l < groupeMintermes.groupesImpliquants[i][j].bincode.Length; l++)
                    {
                        //les tires dans ce que sont soit des 0 soit des 1  soit des x donc on peut dire qu'il n'y a pas de difference (continue pour passer a la prochaine iteration )
                        if ((impliquantsEnAttente[k].nbDontCare >= cptGroupes) && (impliquantsEnAttente[k].bincode[l] == '-')) continue;


                        if (groupeMintermes.groupesImpliquants[i][j].bincode[l] != impliquantsEnAttente[k].bincode[l] && groupeMintermes.groupesImpliquants[i][j].bincode[l] != '-')
                        {
                            count += 1;
                            differentAt = l;
                        }

                        if (count > 1) break;

                    }

                    //Si les deux impliquants sont adjacents
                    if (count == 1)
                    {
                        //Les deux impliquants sont traités (status = false)
                        groupeMintermes.groupesImpliquants[i][j].status = false;


                        //Actualiser le code binaire et le simplifier
                        StringBuilder sb = new StringBuilder(groupeMintermes.groupesImpliquants[i][j].bincode);
                        sb[differentAt] = '-';

                        //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                        impliquants.Add(new Impliquant(sb.ToString()));
                    }

                }


            }
        }


        //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
        for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
        {
            impliquantsPremiers.AddRange(groupeMintermes.groupesImpliquants[i].FindAll(impliquant => impliquant.status));
        }


        if (literale)
        {
            impliquants.AddRange(impliquantsEnAttente.Where(m => m.nbDontCare == cptGroupes).ToList()); // filtrer les impliquannts qui contient cptGroupe - 
            impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == cptGroupes));// supprimer ces derniers 


            //dans le cas ou il y'a pas d'adjacents mais la liste des impliquants en attente n'est pas vide 
            while (impliquants.Count == 0 && impliquantsEnAttente.Count > 0)
            {
                cptGroupes++;
                impliquants.AddRange(impliquantsEnAttente.Where(m => m.nbDontCare == cptGroupes).ToList());
                impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == cptGroupes));
            }

        }








        //Si la liste des impliquants qu'il faut encore traiter n'est pas vide donc regrouper les impliquants
        if (impliquants.Count > 0)
        {

            impliquants = impliquants.Distinct().ToList();

            groupeMintermes.GrouperListes(impliquants);

            //Petit affichage du groupage
            /*        Console.WriteLine("\t-Groupage :" + cptGroupes);
                    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
                    {
                        for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
                        {
                            Console.WriteLine(groupeMintermes.groupesImpliquants[i][j].bincode);
                        }
                        Console.WriteLine("----------------------------------");
                    }
                    */
        }
        else //Sinon Arrêter la boucle
        {
            stop = true;
        }
    }


    impliquantsPremiers = impliquantsPremiers.Distinct().ToList(); //suprimer les impliquants qui se repete 



    //Petit affichage des impliquants premiers
    /*Console.WriteLine("\nLes impliquants premiers : \n");
    for (int i = 0; i < impliquantsPremiers.Count; i++)
    {
        Console.WriteLine(impliquantsPremiers[i].bincode);
    }*/

    int impliquantIndex = -1;
    int mintermeIndex = 0;
    int impliquantLevel = 1;


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
            /*Si le minterme i est représenté par impliquantLevel impliquant(s) initialement si le minterme i
            est représenté uniquement par un impliquant donc il ajouté à la liste des impliquants essentiels
            et supprimer le minterme de la liste des mintermes car il a ainsi été traité*/
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
            /*Si le minterme est représenté par au moins un impliquant essentiel donc ce minterme est supprimé de la liste des mintermes
             car il a ainsi été traité*/
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

    /*//Petit Affichage des impliquants essentiels
    for (int i = 0; i < impliquantsEssentiels.Count; i++)
    {
        Console.WriteLine(impliquantsEssentiels[i].bincode);
    }*/
}
else
{
    impliquants.Clear();


    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length - 1; i++)
    {


        for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
        {

            //premiere boucle pour trouver les adjacents avec "groupesImpliquants[i + 1]"
            for (int k = 0; k < groupeMintermes.groupesImpliquants[i + 1].Count; k++)
            {
                count = 0;
                differentAt = -1;
                for (int l = 0; l < groupeMintermes.groupesImpliquants[i][j].bincode.Length; l++)
                {


                    if (groupeMintermes.groupesImpliquants[i][j].bincode[l] != groupeMintermes.groupesImpliquants[i + 1][k].bincode[l])
                    {
                        count += 1;
                        differentAt = l;
                    }

                    if (count > 1) break;

                }
                //Si les deux impliquants sont adjacents
                if (count == 1)
                {
                    //Les deux impliquants sont traités (status = false)
                    groupeMintermes.groupesImpliquants[i][j].status = false;
                    groupeMintermes.groupesImpliquants[i + 1][k].status = false;


                    //Actualiser le code binaire et le simplifier
                    StringBuilder sb = new StringBuilder(groupeMintermes.groupesImpliquants[i][j].bincode);
                    sb[differentAt] = '-';

                    //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                    impliquants.Add(new Impliquant(sb.ToString()));
                }
            }

        }
    }


    //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
    {
        impliquantsPremiers.AddRange(groupeMintermes.groupesImpliquants[i].FindAll(impliquant => impliquant.status));
    }

    impliquantsPremiers.AddRange(impliquants);

    //Construire les produits de sommes pour les impliquants (P = (I1+I2).(I3+I4)....)
    string P = "";
    count = 0;
    for(int i=0;i<stringListMinterm.Count;i++)
    {
        P += "(";
        for(int j=0;j<impliquantsPremiers.Count;j++)
        {
            if(impliquantsPremiers[j].represente(stringListMinterm[i]))
            {
                P += j.ToString()+"|";
            }
        }
        P = P.Substring(0, P.Length - 1);
        P += ").";
    }
    P = P.Substring(0, P.Length - 1);
    Console.WriteLine("P = " + P);

    //Covertir P en forme disjonctive
    StringBuilder petrick_exp = new StringBuilder();
    string P_postfix = ExprBool.To_RNP(P);
    ExprBool? root = ExprBool.expressionTree(P_postfix);
    root = ExprBool.dnf(root);
    ExprBool.inorder(root,petrick_exp);
    Console.WriteLine("P = " + petrick_exp.ToString());

    //Appliquer la loi d'absorbsion x + xy = x et x.x = x et x+x=x
    List<List<string>> ImpliquantsEssentiauxList = new List<List<string>>();
    List<bool> ImpliquantEssentiauxStatus = new List<bool>();
    List<string> ImpliquantsTempList = petrick_exp.ToString().Split('+').ToList();
    for(int i=0;i<ImpliquantsTempList.Count;i++)
    {
        ImpliquantsEssentiauxList.Add(ImpliquantsTempList[i].Split('.').Distinct().ToList());
        ImpliquantEssentiauxStatus.Add(true);
    }
    ImpliquantsTempList.Clear();

    //Affichage================================================
    for (int i = 0; i < ImpliquantsEssentiauxList.Count; i++)
    {
        Console.Write("(");
        for (int j = 0; j < ImpliquantsEssentiauxList[i].Count; j++)
        {
            Console.Write(ImpliquantsEssentiauxList[i][j] + ".");
        }
        Console.Write(") + ");
    }
    Console.WriteLine();

    for (int i=0;i<ImpliquantsEssentiauxList.Count;i++)
    {
        for(int j=0;j<ImpliquantsEssentiauxList.Count;j++)
        {
            if ((i != j) && (ImpliquantEssentiauxStatus[i]) && (ImpliquantEssentiauxStatus[j]))
            {
                if(ImpliquantsEssentiauxList[j].Intersect(ImpliquantsEssentiauxList[i]).Count() == ImpliquantsEssentiauxList[i].Count())
                {
                    ImpliquantEssentiauxStatus[j] = false;
                }
            }
        }
    }

    //Affichage================================================
    for (int i = 0; i < ImpliquantsEssentiauxList.Count; i++)
    {
        if (ImpliquantEssentiauxStatus[i])
        {
            Console.Write("(");
            for (int j = 0; j < ImpliquantsEssentiauxList[i].Count; j++)
            {
                Console.Write(ImpliquantsEssentiauxList[i][j] + ".");
            }
            Console.Write(") + ");
        }

    }
    Console.WriteLine();

    //Extraire la plus petite combinaison de d'impliquants
    int minNb = stringListMinterm.Count+1;
    for(int i=0;i<ImpliquantsEssentiauxList.Count;i++)
    {
        if(ImpliquantEssentiauxStatus[i])
        {
            if(ImpliquantsEssentiauxList[i].Count<minNb)
            {
                ImpliquantsTempList = ImpliquantsEssentiauxList[i];
                minNb = ImpliquantsEssentiauxList[i].Count;
            }
        }
    }

    //Ajouter cette combinaison aux impliquants essentiaux
    for(int i=0;i<ImpliquantsTempList.Count;i++)
    {
        impliquantsEssentiels.Add(impliquantsPremiers[int.Parse(ImpliquantsTempList[i])]);
    }
    ImpliquantsEssentiauxList.Clear();
    ImpliquantsTempList.Clear();

    /*for (int i=0;i<impliquantsPremiers.Count;i++)
    {
        Console.WriteLine("@ " + impliquantsPremiers[i].bincode + " @");
    }*/
}

//Affichage de la formule simplifiée ====================================================================================================

Console.WriteLine("Resultat de simplification : ");
String resultat = "";
string alpha; 
for(int i=0;i<impliquantsEssentiels.Count;i++)
{
    for(int j=0;j<impliquantsEssentiels[i].bincode.Length;j++)
    {
        if(impliquantsEssentiels[i].bincode[j] != '-')
        {

            if (literale)
            {
                alpha = alphabets[j];               
            }
            else {
               
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
                resultat += "(" + alpha + ")";
            }
            else
            {
                resultat += "!" + "( " + alpha + " )";
            }
        }
    }
    resultat += " + ";
}

//Enlever le " + " additionnel à la fin
if(resultat.Length>=3)
{
    resultat = resultat.Substring(0,resultat.Length-3);
}

Console.WriteLine(resultat);


