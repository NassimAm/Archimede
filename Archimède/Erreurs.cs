using System;
using System.Text;
using System.Collections.Generic;



namespace DetectionErreurs
{
    class Erreurs {

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

        public static List<string> DetectionErreurs(string f, bool forme)
        {

            Console.Write("donnez une fonction : ");
            
            f = f.Replace(" ", "");

            List<string> erreursList = new List<string>();
            if (forme)
            {
                //initialiser le nombre de parentheses ouverantes et fermantes
                int open = 0, close = 0;

                //la fonction vide est toujours fausse
                if (string.IsNullOrEmpty(f))
                {
                    //erreursList.Add("0");
                    erreursList.Add("Fonction vide.");
                }

                else if (f.Length == 1)
                {
                    if (!estUneVariable(f[0]))//
                    {

                        if (estOperateurBinaire(f[0]))
                        {
                            //Console.WriteLine("1 : une fonction ne peut pas commencer par un operateur");
                            erreursList.Add("Position 1 : Une fonction ne peut pas commencer par un operateur binaire (" + f[0] + ").");
                            //erreursList.Add("0-1");
                        }
                        else if (f[0] == '!')
                        {
                            erreursList.Add("Position 1 : La fonction introduite est incompléte.");
                            //A la position x : Il manque une variable pour le NON
                            //Il manque une variable après le NON de la position x
                        }
                        else
                        {
                            erreursList.Add("Position 1 : " + f[0] + " n'est pas inclus dans le langage");

                            //erreursList.Add("4-1");
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
                            if (f[i + 1] == ')')
                            {
                                erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Parenthéses vides.");
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
                    }

                    if ((f[f.Length - 1]) == ')')
                    {
                        close++;
                        if (close > open)
                        {
                            erreursList.Add("Position " + f.Length + " : Il manque une parenthese ouvrante.");
                            //erreursList.Add("2-" + f.Length);
                        }
                    }

                    if (open > close) //((()
                    {
                        erreursList.Add("Position " + f.Length + " : Il manque " + (open - close) + " parenthese(s) fermente(s).");
                    }
                }
            }
            //forme num
            //forme num
            else
            {
                if (string.IsNullOrEmpty(f))
                {
                    erreursList.Add("La fonction est vide.");
                }

                else
                {
                    for (int i = 0; i < f.Length - 1; i++)
                    {

                        if (f[i] == ',')
                        {
                            if (i == 0)
                            {
                                erreursList.Add("Position " + (i + 1) + " : Une fonction ne peut pas commencer par une virgule.");
                            }
                            if (f[i + 1] == ',')
                            {
                                erreursList.Add("Position " + (i + 1) + "-" + (i + 2) + " : Il manque une valeur numérique entre les deux virgules.");
                            }
                        }
                        else if (!char.IsDigit(f[i]))
                        {
                            erreursList.Add("Position " + (i + 1) + " : " + f[i] + " n'est pas inclus dans le langage. Insérez une valeur numérique.");
                        }
                    }
                    if (f[f.Length - 1] == ',')
                    {
                        erreursList.Add("Position " + (f.Length) + " : Une fonction ne peut pas se terminer par une virgule.");
                    }

                    else if (!char.IsDigit(f[f.Length - 1]))
                    {
                        erreursList.Add("Position " + (f.Length) + " : " + f[f.Length - 1] + " n'est pas inclus dans le langage. Insérez une valeur numérique.");
                    }
                }
            }
            return erreursList;
        }
    }
}


