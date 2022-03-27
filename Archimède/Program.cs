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

//Générer les impliquants finaux
bool stop = false;
int count = 0;
int differentAtIndex = -1;
bool prime = true;
List<Impliquant> impliquants = new List<Impliquant>();
List<Impliquant> finalImpliquants = new List<Impliquant>();

Console.WriteLine("Impliquants ===================");
while (stop == false)
{
    for (int i = 0; i < groupeMintermes.groupesMintermes.Length - 1; i++)
    {
        for(int j = 0;j<groupeMintermes.groupesMintermes[i].Count;j++)
        {
            prime = true;
            for(int k=0;k<groupeMintermes.groupesMintermes[i+1].Count;k++)
            {
                count = 0;
                differentAtIndex = -1;
                for (int l=0;l<groupeMintermes.groupesMintermes[i][j].bincode.Length;l++)
                {
                    if (groupeMintermes.groupesMintermes[i][j].bincode[l] != groupeMintermes.groupesMintermes[i + 1][k].bincode[l])
                    {
                        count += 1;
                        differentAtIndex = l;
                    }
                }
                if (count > 0)
                    prime = false;
                if (count == 1)
                {
                    if (differentAtIndex != -1)
                    {
                        Impliquant impliquant = new Impliquant(new List<Minterme>() { groupeMintermes.groupesMintermes[i][j], groupeMintermes.groupesMintermes[i + 1][k] });
                        StringBuilder sb = new StringBuilder(impliquant.bincode);
                        sb[differentAtIndex] = '-';
                        impliquant.bincode = sb.ToString();
                        impliquant.status = false;
                        impliquants.Add(impliquant);
                        Console.WriteLine(impliquant.bincode);
                    }
                }
            }
            if(prime)
            {
                Impliquant impliquant = new Impliquant(new List<Minterme>() { groupeMintermes.groupesMintermes[i][j] });
                impliquant.status = true;
                finalImpliquants.Add(impliquant);
                Console.WriteLine(impliquant.bincode);
            }
        }
    }
    Console.WriteLine("==================");
    if(impliquants.Count != 0)
    {
        groupeMintermes.GrouperListes(impliquants);
        impliquants.Clear();
    }
    else
    {
        stop = true;
    }
}

/*for(int i = 0;i<impliquants.Count;i++)
{
    Console.WriteLine(impliquants[i].bincode);
}*/