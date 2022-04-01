using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Minterme: IEquatable<Minterme>
    {
        //Nombre décimal représentant le minterme
        public long nombre { get  ; set;}

        //Nombre décimal comme chaine de caractere  représentant le minterme 
        public string nombreChaine { get; set;}
        //Représentation binaire du minterme
        public string bincode { get; set;}
        //Nombre de 1 dans la représentation binaire
        public int nbuns { get; set;}

        //le max de nombre des uns pour les mintermes
        public static int maxNbUns = 0   ;  

        //la longeur maximale des minterme cree 
        public static int maxNbVariables = 0;


        public Minterme(long nombre)
        {
            this.nombre = nombre;
            this.bincode = ConvertionAuBinaire(nombre);
            this.nbuns = NombreDeUns(this.bincode);


            
            if (this.nbuns > maxNbUns) maxNbUns = this.nbuns;
            if(this.bincode.Length > maxNbVariables) maxNbVariables = this.bincode.Length;


        }

        public Minterme(string nombreChaine)
        {
            this.nombre = -1; 
            this.nombreChaine = nombreChaine;
            this.bincode = ConvertionAuBinaire(nombreChaine);
            this.nbuns = NombreDeUns(this.bincode);



            if (this.nbuns > maxNbUns) maxNbUns = this.nbuns;
            if (this.bincode.Length > maxNbVariables) maxNbVariables = this.bincode.Length;


        }

        //Compte le nombre de 1 dans un code binaire
        private int NombreDeUns(string codebinaire)
        {
            return codebinaire.Count(ch => (ch == '1'));
        }

        //Convertit un nombre en code binaire
        public static string ConvertionAuBinaire(long nombre)
        {
            return Convert.ToString(nombre, 2);
        }

        public static string ConvertionAuBinaire(string nombre)
        {

            //n tableau quii represente le puissance de deux 63 -> 100
            string[] twoPowers = {
  "9223372036854775808"
, "18446744073709551616"
, "36893488147419103232"
, "73786976294838206464"
, "147573952589676412928"
, "295147905179352825856"
, "590295810358705651712"
, "1180591620717411303424"
, "2361183241434822606848"
, "4722366482869645213696"
, "9444732965739290427392"
, "18889465931478580854784"
, "37778931862957161709568"
, "75557863725914323419136"
, "151115727451828646838272"
, "302231454903657293676544"
, "604462909807314587353088"
, "1208925819614629174706176"
, "2417851639229258349412352"
, "4835703278458516698824704"
, "9671406556917033397649408"
, "19342813113834066795298816"
, "38685626227668133590597632"
, "77371252455336267181195264"
, "154742504910672534362390528"
, "309485009821345068724781056"
, "618970019642690137449562112"
 ,"1237940039285380274899124224"
, "2475880078570760549798248448"
, "4951760157141521099596496896"
, "9903520314283042199192993792"
, "19807040628566084398385987584"
, "39614081257132168796771975168"
 ,"79228162514264337593543950336"
, "158456325028528675187087900672"
, "316912650057057350374175801344"
, "633825300114114700748351602688"
, "1267650600228229401496703205376"
};

        
            int index = twoPowers.Length -1 ;
            string suffix = "";
            string resultat ;
            string sauv = nombre;
            bool ajouter = false; 


            while (index >= 0) {
                
                
                resultat = soustraire(sauv, twoPowers[index]);
                if (resultat[0] == '-')
                {
                    if(ajouter) suffix += "0";

                }
                else {
                    sauv = resultat;
                    suffix += "1";
                    ajouter = true;
                }
                index--;
            }

            return suffix + Convert.ToString( long.Parse(sauv), 2).PadLeft(63 , '0');

        }


        //soustraire num2 de num1 avec num2 et num1 des string (sans parser en entier )
        public static string soustraire(string num1 , string num2)
        {

            StringBuilder result = new StringBuilder("");

            if (num1.Length < num2.Length)  num1 = num1.PadLeft(num2.Length , '0'); 
            else num2 =  num2.PadLeft(num1.Length , '0');

            
            if (string.Compare(num1, num2) < 0) return "-"; 
           

            int digits = num1.Length ;

            int digit1;
            int digit2;

            int digitResult;
            int carry = 0;

            

            for (int i = digits - 1; i >= 0 ; i--)
            {
                digit1 = (int)Char.GetNumericValue(num1[i]);
                digit2 = (int)Char.GetNumericValue(num2[i]);
                

                digitResult = digit1 - digit2 + carry;

                if (digitResult < 0)
                {
                    carry = -1;
                    digitResult += 10;
                }
                else if (digitResult >= 10)
                {
                    carry = 1;
                    digitResult -= 10;
                }
                else
                {
                    carry = 0;
                }

                result.Insert(0 , digitResult);


            }   

            return result.ToString(); 
        }



        public bool Equals(Minterme other)
        {
            if(ReferenceEquals(other, null)) return false;
            if(ReferenceEquals(this, other)) return true;
            return int.Equals(nombre, other.nombre);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Minterme)obj);
        }

        /*public override int GetHashCode()
        {
            return (nombre != null ? nombre.GetHashCode() : 0);
        }*/

    }
}
