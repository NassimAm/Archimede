using Archimède;
using System.Text;


bool literale = true;




//Variables globales  pour les 2 litterale et numerique :
int maxNbUns;
List<Impliquant> impliquantsEnAttente = new List<Impliquant>();
List<Impliquant> impliquants = new List<Impliquant>();
List<Minterme> mintermes;
Mintermes groupeMintermes;
List<string> stringListMinterm = new List<string>(); // liste des codes binaires du chaque minterms 
StringBuilder alphabet;
List<string> alphabets = new List<string>();


if ( literale )
{
    Console.WriteLine("Entrez une expresseion en forme disjonctif:");
    string expression = Console.ReadLine();
    
    
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


    int nbVariables = alphabets.Count; // le nombre de variables = nombre d'alphabets

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
       impliquants.Add(new Impliquant(mintermBinCode));
    }

    groupeMintermes = new Mintermes(maxNbUns);


}
else {


    Console.Write("Entrez le nombre de variables : ");
    int nbVariables = int.Parse(Console.ReadLine());

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



}











//Créer les groupes ===================================================================================================================
//Les groupes sont formés d'impliquants initiaux (les mintermes introduits)


List<Impliquant> impliquantsPremiers = new List<Impliquant>();




groupeMintermes.GrouperListes(impliquants , 0);


//Petit affichage de groupage initial
Console.WriteLine("\t1-Groupage initial :");
for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
{
    for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
    {
        Console.WriteLine(groupeMintermes.groupesImpliquants[i][j].bincode);
    }
    Console.WriteLine("----------------------------------");
}


//Générer les impliquants premiers
int count = 0;
int differentAt = -1;
bool stop = false;





int cptGroupes = 0; // compteur de nombre de groupages


//Impliquant sauvImpliquant; 

while (!stop)
{
    cptGroupes++;

    impliquants.Clear();


    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length - 1; i++)
    {
        for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
        {

            if (groupeMintermes.groupesImpliquants[i][j].nbDontCare >= cptGroupes)
            {
                // on va  traiter ce minterm dans les prochaine groupes
                 continue; 

            }

            for (int k = 0; k < groupeMintermes.groupesImpliquants[i + 1].Count; k++)
            {                              
                count = 0;
                differentAt = -1;
                for (int l = 0; l < groupeMintermes.groupesImpliquants[i][j].bincode.Length; l++)
                {
                    //les tires dans ce que sont soit des 0 soit des 1  soit des x dans on peut dire qu'il n'y a pas de difference (continue pour passer a la prochaine iteration )
                    if ((groupeMintermes.groupesImpliquants[i + 1][k].nbDontCare >= cptGroupes ) && (groupeMintermes.groupesImpliquants[i + 1][k].bincode[l] == '-')) continue; 


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
                    impliquants.Add(new Impliquant(sb.ToString())) ;
                }
            }
        }
    }


    //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
    {
        impliquantsPremiers.AddRange(groupeMintermes.groupesImpliquants[i].FindAll( impliquant => impliquant.status )) ;
    }


    
    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
    {
        impliquants.AddRange(groupeMintermes.groupesImpliquants[i].FindAll(impliquant =>  (    impliquant.bincode.Count(ch => ch == '-') >= cptGroupes)  ));
    }



    //Si la liste des impliquants qu'il faut encore traiter n'est pas vide donc regrouper les impliquants
    if (impliquants.Count>0 )
    {

        impliquants = impliquants.Distinct().ToList() ;
          
        groupeMintermes.GrouperListes(impliquants , cptGroupes);

        //Petit affichage du groupage
/*        Console.WriteLine("\t-Groupage :" + cptGroupes);
        for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
        {
            for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
            {
                Console.WriteLine(groupeMintermes.groupesImpliquants[i][j].bincode);
            }
            Console.WriteLine("----------------------------------");
        }*/
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
}
*/



List<Impliquant> impliquantsEssentiels = new List<Impliquant>();
int impliquantIndex = -1;
int mintermeIndex = 0;
int impliquantLevel = 1;


//Tant que la liste des mintermes n'est pas vide
while (stringListMinterm.Count>0)
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
            if (impliquantsEssentiels[j].represente(stringListMinterm[mintermeIndex]) )
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


