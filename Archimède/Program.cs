using Archimède;

//Introduction des Valeurs ========================================================================================================================

Console.Write("Entrez le nombre de variables : ");
int nbVariables = int.Parse(Console.ReadLine());
Console.WriteLine("Entrez la liste de mintermes (séparés par des virgules) : ");
string mintermesString = Console.ReadLine();
string[] listMintermesString = mintermesString.Split(',');

//Creation de la liste des mintermes
List<Minterme> mintermes = new List<Minterme>();
for(int i = 0;i<listMintermesString.Length;i++)
{
    mintermes.Add(new Minterme(int.Parse(listMintermesString[i])));
}

//Vérifier le nombre maximal de 1 (Calculer le nombre de groupes à créer)
//En meme temps vérifier le nombre minimal de variables qui correspond à la liste de mintermes
List<int> listNbUns = new List<int>();
List<int> listMintermeLong = new List<int>();
for(int i = 0;i<mintermes.Count;i++)
{
    listNbUns.Add(mintermes[i].nbuns);
    listMintermeLong.Add(mintermes[i].bincode.Length);
}
int maxNbUns = listNbUns.Max();
int maxMintermeLong = listMintermeLong.Max();

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
    while(mintermes[i].bincode.Length < nbVariables)
    {
        mintermes[i].bincode = "0" + mintermes[i].bincode;
    }
}

//Créer les groupes ====================================================================================

Mintermes groupeMintermes = new Mintermes(maxNbUns);
groupeMintermes.GrouperListes(mintermes);

for(int i = 0;i<groupeMintermes.groupesMintermes.Length;i++)
{
    for(int j=0;j< groupeMintermes.groupesMintermes[i].Count;j++)
    {
        Console.WriteLine(groupeMintermes.groupesMintermes[i][j].bincode);
    }
    Console.WriteLine("----------------------------------");
}

/*//Générer les impliquants finaux
bool stop = false;
while(!stop)
{
    for(int i=0;i<groupeMintermes.groupesMintermes.Length-1;i++)
    {

    }
}*/