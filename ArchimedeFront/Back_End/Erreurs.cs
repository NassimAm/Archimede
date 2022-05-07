using System;
using System.Text;
using System.Collections.Generic;



namespace DetectionErreurs
{
    class Erreurs
    {
        //fonction qui retourne "vrai" si le caractere en cours est un operateur binaire
        public static bool estOperateurBinaire(char caractere)
        {
            if (caractere == '+' || caractere == '.' || caractere == '↑' || caractere == '↓' || caractere == '⊕' || caractere == '⊙')
                return true;
            return false;
        }

        public static bool estUneVariable(char caractere)
        {
            if (char.IsLetterOrDigit(caractere) || caractere == '_')
                return true;
            return false;
        }

        public static List<string> detectionErreurs(string f)
        {
            f = f.Replace(" ", "");

            List<string> erreursList = new List<string>();

            //initialiser le nombre de parentheses ouverantes et fermantes
            int open = 0, close = 0;

            //la fonction vide est toujours fausse
            if (f.Length == 0)
            {
                //erreursList.Add("0");
                erreursList.Add("Fonction vide.");
            }



            else if (f.Length == 1)
            {
                if (!estUneVariable(f[0]))
                {

                    if (estOperateurBinaire(f[0]))
                    {
                        erreursList.Add("Position 1 : Une fonction ne peut pas commencer par un operateur binaire (" + f[0] + ").");
                        //erreursList.Add("0-1");
                    }
                    else if (f[0] == '!')
                    {
                        erreursList.Add("Position 1 : La fonction introduite est incompléte.");
                    }
                    else
                    {
                        erreursList.Add("Position 1 : " + f[0] + " n'est pas inclus dans le langage");
                    }
                }
            }

            else
            {
                for (int i = 0; i < f.Length - 1; i++)
                {
                    if (estOperateurBinaire(f[i]))
                    {
                        if (i == 0)
                        {
                            erreursList.Add("Position 1 : Une fonction ne peut pas commencer par un operateur binaire (" + f[0] + ").");
                            //erreursList.Add("0-1");
                        }


                        if (estOperateurBinaire(f[i + 1]) || f[i + 1] == ')')
                        {
                            erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Il manque une variable entre (" + f[i] + ") et (" + f[i + 1] + ").");
                            //erreursList.Add("1-" + (i + 1) + "/" + (i + 2));
                        }

                    }
                    else if (f[i] == '!')
                    {
                        if (estOperateurBinaire(f[i + 1]) || f[i + 1] == ')')
                        {
                            erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Il manque une variable entre (" + f[i] + ") et (" + f[i + 1] + ").");
                            //erreursList.Add("1-" + (i + 1) + "/" + (i + 2));
                        }
                    }
                    else if (f[i] == '(')
                    {
                        open++;
                        if (estOperateurBinaire(f[i + 1]))
                        {
                            erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Il manque une variable entre (" + f[i] + ") et (" + f[i + 1] + ").");
                            //erreursList.Add("1-" + (i + 1) + "/" + (i + 2));
                        }
                    }
                    else if (f[i] == ')')
                    {
                        close++;
                        if (close > open)
                        {
                            erreursList.Add("Position " + (i + 1) + " : Il manque une parenthese ouvrante.");
                            //erreursList.Add("2-" + (i + 1));
                        }
                        if (!estOperateurBinaire(f[i + 1]) && f[i + 1] != ')')
                        {
                            erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Il manque un opérateur binaire.");
                            //erreursList.Add("3-" + (i + 1) + "/" + (i + 2));
                        }
                    }
                    else if (estUneVariable(f[i]))
                    {
                        if (f[i + 1] == '!' || f[i + 1] == '(')
                        {
                            erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Il manque un opérateur binaire.");
                            //erreursList.Add("1-" + (i + 1) + "/" + (i + 2));
                        }
                    }
                    else
                    {
                        erreursList.Add("Position " + (i + 1) + " : " + f[i] + " n'est pas inclus dans le langage.");
                        //erreursList.Add("4-" + (i + 1));
                    }
                }
                if (estOperateurBinaire(f[(f.Length) - 1]) || f[(f.Length) - 1] == '!')
                {
                    erreursList.Add("Position " + f.Length + " : La fonction ne peut pas se terminer par un operateur.");
                    //erreursList.Add("5-" + f.Length);
                }
                else if (f[f.Length - 1] == '(')
                {
                    erreursList.Add("Position " + f.Length + " : La fonction ne peut pas se terminer par une parenthese ouvrante."); open++;
                    //erreursList.Add("6-" + f.Length);
                }else if ((f[f.Length - 1]) == ')')
                {
                    close++;
                    if (close > open)
                    {
                        erreursList.Add("Position " + f.Length + " : Il manque une parenthese ouvrante.");
                        //erreursList.Add("2-" + f.Length);
                    }
                }
                else if (! estUneVariable(f[f.Length - 1]) && f[f.Length -1] != '_')
                {
                    erreursList.Add("Position " + f.Length + " : " + f[f.Length-1] + " n'est pas inclus dans le langage.");
                    //erreursList.Add("0-1");
                }

                if (open > close) //((()
                {
                    erreursList.Add("Position " + f.Length + " : Il manque " + (open - close) + " parenthese(s) fermente(s).");
                }
            }

            return erreursList;
        }
    }
}
