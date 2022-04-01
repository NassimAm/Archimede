using Archimède;
using System.Text;




//Introduction des Valeurs ========================================================================================================================

Console.Write("Entrez le nombre de variables : ");
int nbVariables = int.Parse(Console.ReadLine());
Console.WriteLine("Entrez la liste de mintermes (séparés par des virgules) : ");
string mintermesString = Console.ReadLine();
string[] listMintermesString = mintermesString.Split(',');

//Creation de la liste des mintermes
List<Minterme> mintermes = new List<Minterme>();

long parsedInt; 

for(int i = 0;i<listMintermesString.Length;i++)
{
    try {
        parsedInt = long.Parse(listMintermesString[i]);
        mintermes.Add(new Minterme(parsedInt));
    }
    catch(OverflowException ) {
  
        mintermes.Add(new Minterme(listMintermesString[i])); 
    }
    
    
}   



//Vérifier le nombre maximal de 1 (Calculer le nombre de groupes à créer)
//En meme temps vérifier le nombre minimal de variables qui correspond à la liste de mintermes

int maxNbUns = Minterme.maxNbUns;
int maxMintermeLong = Minterme.maxNbVariables ;


//Dans le cas où le nombre de variables introduit ne correspond pas à la liste de mintermes introduites
if(maxMintermeLong>nbVariables)
{
    Console.WriteLine("Avertissement : La liste de mintermes introduite depasse le nombre maximal de variables introduit");
    Console.WriteLine("On travaillera donc suivant la liste de mintermes donc avec "+maxMintermeLong.ToString()+" variables");
    nbVariables = maxMintermeLong;
}


//Corriger les codes binaires (en ajoutant des zéros au début pour qu'ils aient tous la mê^me longueur)
for(int i = 0;i<mintermes.Count;i++)
{
    mintermes[i].bincode = mintermes[i].bincode.PadLeft(nbVariables, '0');
}

//Créer les groupes ===================================================================================================================
//Les groupes sont formés d'impliquants initiaux (les mintermes introduits)
Mintermes groupeMintermes = new Mintermes(maxNbUns);
List<Impliquant> impliquants = groupeMintermes.InitImpliquants(mintermes);
List<Impliquant> impliquantsPremiers = new List<Impliquant>();
groupeMintermes.GrouperListes(impliquants);

/*//Petit affichage de groupage initial
for(int i = 0;i<groupeMintermes.groupesImpliquants.Length;i++)
{
    for(int j=0;j< groupeMintermes.groupesImpliquants[i].Count;j++)
    {
        Console.WriteLine(groupeMintermes.groupesImpliquants[i][j].bincode);
    }
    Console.WriteLine("----------------------------------");
}*/

//Générer les impliquants premiers
int count = 0;
int differentAt = -1;
bool stop = false;
while(!stop)
{
    impliquants.Clear();
    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length - 1; i++)
    {
        for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
        {
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
                }
                //Si les deux impliquants sont adjacents
                if (count == 1)
                {
                    //Les deux impliquants sont traités (status = false)
                    groupeMintermes.groupesImpliquants[i][j].status = false;
                    groupeMintermes.groupesImpliquants[i + 1][k].status = false;
                    //Fusionner leurs mintermes correspondants
                    groupeMintermes.groupesImpliquants[i][j].mintermes.AddRange(groupeMintermes.groupesImpliquants[i + 1][k].mintermes);
                    StringBuilder sb = new StringBuilder(groupeMintermes.groupesImpliquants[i][j].bincode);
                    //Actualiser le code binaire et le simplifier
                    sb[differentAt] = '-';
                    groupeMintermes.groupesImpliquants[i][j].bincode = sb.ToString();
                    //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                    impliquants.Add(groupeMintermes.groupesImpliquants[i][j]);
                }
            }
        }
    }

    //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
    for (int i = 0; i < groupeMintermes.groupesImpliquants.Length; i++)
    {
        for (int j = 0; j < groupeMintermes.groupesImpliquants[i].Count; j++)
        {
            //Les impliquants qui n'ont pas d'adjacents donc non traités au dessus (status = true) seront ajoutés aux impliquants premiers directement
            if (groupeMintermes.groupesImpliquants[i][j].status == true)
            {
                impliquantsPremiers.Add(groupeMintermes.groupesImpliquants[i][j]);
            }
        }
        //impliquantsPremiers.AddRange(groupeMintermes.groupesImpliquants[i].FindAll( impliquant => impliquant.status )) ;

    }

    //Si la liste des impliquants qu'il faut encore traiter n'est pas vide donc regrouper les impliquants
    if(impliquants.Count>0)
    {
        groupeMintermes.GrouperListes(impliquants);

        /*//Petit affichage du groupage
        Console.WriteLine("=================================");
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

/*//Petit affichage des impliquants premiers
for (int i = 0; i < impliquantsPremiers.Count; i++)
{
    Console.WriteLine(impliquantsPremiers[i].bincode);
    Console.WriteLine("Mintermes : ");
    for(int j = 0; j < impliquantsPremiers[i].mintermes.Count;j++)
    {
        Console.WriteLine(impliquantsPremiers[i].mintermes[j].bincode);
    }
}*/

//Recherche des impliquants premiers essentiels ===================================================================================

/*//Petit affichage de la tables des impliquants premiers
for (int i = 0; i < mintermes.Count; i++)
{
    Console.Write(mintermes[i].bincode + " ");
}
Console.WriteLine();
for (int i = 0; i < impliquantsPremiers.Count; i++)
{
    for (int j = 0; j < mintermes.Count; j++)
    {
        if (impliquantsPremiers[i].mintermes.Contains(mintermes[j]))
        {
            Console.Write("Vrai ");
        }
        else
        {
            Console.Write("Faux ");
        }
    }
    Console.WriteLine(impliquantsPremiers[i].bincode);
}
Console.WriteLine();*/

List<Impliquant> impliquantsEssentiels = new List<Impliquant>();
int impliquantIndex = -1;
int mintermeIndex = 0;
int impliquantLevel = 1;

//Tant que la liste des mintermes n'est pas vide
while(mintermes.Count>0)
{
    mintermeIndex = 0;
    while (mintermeIndex < mintermes.Count)
    {
        count = 0;
        impliquantIndex = -1;
        for (int j = 0; j < impliquantsPremiers.Count; j++)
        {
            //Si l'impliquant j peut représenter le minterme i
            if (impliquantsPremiers[j].mintermes.Contains(mintermes[mintermeIndex]))
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
            mintermes.RemoveAt(mintermeIndex);
        }
        else //Sinon avancer dans la liste des mintermes
        {
            mintermeIndex += 1;
        }
    }


    //Vérifier si en choisissant les impliquants essentiels quels mintermes on a traité
    mintermeIndex = 0;
    while (mintermeIndex < mintermes.Count)
    {
        count = 0;
        for (int j = 0; j < impliquantsEssentiels.Count; j++)
        {
            //Si l'impliquant essentiel j peut représenter le minterme i
            if (impliquantsEssentiels[j].mintermes.Contains(mintermes[mintermeIndex]))
            {
                count += 1;
            }
        }
        /*Si le minterme est représenté par au moins un impliquant essentiel donc ce minterme est supprimé de la liste des mintermes
         car il a ainsi été traité*/
        if (count > 0)
        {
            mintermes.RemoveAt(mintermeIndex);
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
Console.WriteLine();
Console.WriteLine("Resultat de simplification : ");
String resultat = "";
for(int i=0;i<impliquantsEssentiels.Count;i++)
{
    for(int j=0;j<impliquantsEssentiels[i].bincode.Length;j++)
    {
        if(impliquantsEssentiels[i].bincode[j] != '-')
        {

            StringBuilder alphabet = new StringBuilder(" ");
            //
            if (j >= 26) {
                alphabet = new StringBuilder("  ");
                
                
                alphabet[0] = (char)((65 + j / 26 - 1));
                alphabet[1] = (char) (   65 + ( j  % 26 )  )   ;
                
            }
            else  alphabet[0] = (char)(65 + j);




            //




            //Nommer les variables dans l'ordre alphabétique
            if (impliquantsEssentiels[i].bincode[j] == '1')
            {
                resultat += "(" + alphabet + ")";
            }
            else
            {
                resultat += "!" + "( " + alphabet + " )";
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


