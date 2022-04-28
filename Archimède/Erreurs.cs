using System;
using System.Text;
using System.Collections.Generic;



namespace DetectionErreurs
{
    class Erreurs
    {
       
        
        
            //Nombre d'erreur 
            

            //fonction qui retourne "vrai" si le caractere en cours est un operateur binaire {+,.}
            static bool estOperateurBinaire(char caractere)
            {
                bool bin;
                if (caractere == '+' || caractere == '.')
                {
                    bin = true;
                }
                else 
                    bin = false;
                return bin;
            }

            
            
            public static void DetectErreurs (string f){
                int nbrErreur = 0;
               
                //initialiser le nombre de parentheses ouverantes et fermantes
                int open = 0, close = 0; 

                //la fonction vide est toujours correcte
                //if (f.Length == 0) resultat = true;  

                //Detection d'erreur au debut de la fonction
                if (estOperateurBinaire(f[0]) || f[0] == ')')
                {
                    Console.WriteLine("Erreur au debut de la fonction");
                    nbrErreur++;
                }

                //Detection d'erreur à la fin de la fonction
                if (estOperateurBinaire(f[f.Length - 1]) || f[f.Length - 1] == '(' || f[f.Length - 1] == '!')
                {
                    Console.WriteLine("Erreur à la fin de la fonction");
                    nbrErreur++;
                }

                //sinon on parcour la fonction caractere par caractere
                for (int i = 0; i < f.Length; i++)
                 {
                        //{.,+,!} suivi de {.,+,)}
                        if ((i < f.Length - 1) && (estOperateurBinaire(f[i]) || f[i] == '!'))
                        {
                            if (estOperateurBinaire(f[i+1]) || f[i + 1] == ')')
                            {
                                Console.WriteLine("Deux operateurs d'affilé au niveau de aaaa " + (i + 1));
                                nbrErreur++;
                            }
                        }
                        //{!} pas precedé par {.,+,!,(}
                        else if (f[i] == '!')
                        {
                            if ((i>0) && (f[i - 1] != '.') && (f[i - 1] != '+') && (f[i - 1] != '!') && (f[i - 1] != '('))
                            {
                                Console.WriteLine("position de l'erreur : " + (i + 1));
                                nbrErreur++;
                            }
                        }
                        
                        else if (f[i] == '(')
                        {
                            //{(} pas precedé par {.,+,!,(}
                            if (i > 0 && f[i - 1] != '.' && f[i - 1] != '+' && f[i - 1] != '!' && f[i - 1] != '(')
                            {
                                Console.WriteLine("position de l'erreur : " + (i + 1));
                                //resultat = false;
                            }
                            //{ ( } suivi de { +,. }
                            if (f[i + 1] == '+' || f[i + 1] == '.')
                            {
                                Console.WriteLine("position de l'erreur : " + (i + 1));
                                nbrErreur++;
                                //resultat = false;
                            }
                            open++; // incremnter le nbr de p o
                        }
                        else if (f[i] == ')')
                        {
                            //{ ) } pas suivi de {+,.,)}
                            if ((i < f.Length - 1) && ((f[i + 1] != '+') && (f[i + 1] != '.') && (f[i + 1] != ')')))
                            {
                                Console.WriteLine("position de l'erreur : " + (i + 1));
                                nbrErreur++;
                                //resultat = false;
                            }
                            close++; // incrementer nbr de p f
                            if (close > open) //pf>po automatiquement erreur
                            {
                                Console.WriteLine("position de l'erreur : " + (i + 1));
                                nbrErreur++;
                               // resultat = false;
                            }
                        }
                    
                }
                if (close != open) nbrErreur++;


                if (nbrErreur == 0)
                    System.Console.WriteLine("La fonction est syntaxiquement juste !");
                else
                    Console.WriteLine("Il y a " + nbrErreur + " erreur(s)");
            }

        

    }
}


