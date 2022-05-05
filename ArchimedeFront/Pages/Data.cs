using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimède;
using dnf;

namespace ArchimedeFront.Pages
{
    public class Data
    {
        public static bool literal = true;
        public static string? expression; //l'expression a simplifier ;
        public static string? expressionTransforme; //l'expression apres transformation en dnf 
        public static List<string> variables = new List<string>(); // liste des variables ;
        public static int nbVariables;
        public static List<string>? listMintermesString;
        public static List<string> stringListMinterm = new List<string>(); // liste des codes binaires du chaque minterms 
        public static List<Impliquant> impliquantsEnAttente = new List<Impliquant>();
        public static List<Impliquant> impliquants = new List<Impliquant>();
        public static List<Minterme> mintermes = new List<Minterme>();
        public static Mintermes? groupeMintermes;
        public static int cptGroupes = 0;
        public static List<Impliquant> impliquantsPremiers = new List<Impliquant>();
        public static List<Impliquant> impliquantsEssentiels = new List<Impliquant>();

        public static void resete()
        {
            literal = true;
            expression = ""; //l'expression a simplifier ;
            expressionTransforme = ""; //l'expression apres transformation en dnf 
            variables = new List<string>(); // liste des variables ;
            nbVariables = 0;
            listMintermesString = null;
            stringListMinterm = new List<string>(); // liste des codes binaires du chaque minterms 
            impliquantsEnAttente = new List<Impliquant>();
            impliquants = new List<Impliquant>();
            mintermes = new List<Minterme>();
            groupeMintermes = null;
            cptGroupes = 0;
            impliquantsPremiers = new List<Impliquant>();
            impliquantsEssentiels = new List<Impliquant>();
        }
    }
}
