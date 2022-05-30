using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using dnf;
class Synthese
{

    // Represents a node of an N-ary tree
    public class ExprBoolNode
    {
        public dnf.Type type;
        public string info = "";
        public List<ExprBoolNode> children = new List<ExprBoolNode>();
        public string? id;
        public static int nbAND = 0;
        public static int nbOR = 0;
        public static int nbNOT = 0;
        public static int nbNAND = 0;
        public static int nbNOR = 0;
        public static int nbXOR = 0;
        public static int nbXNOR = 0;

        public ExprBoolNode(string value)
        {
            this.info = value;
            this.type = dnf.Type.VALEUR;
            //this.id = ExprBool.generateID();
            this.id = info;
        }

        public ExprBoolNode(dnf.Type type, List<ExprBoolNode> children)
        {
            switch (type)
            {
                case dnf.Type.ET:
                    this.info = "AND";
                    this.id = this.info + String.Format("{0}", nbAND);
                    nbAND++;
                    break;
                case dnf.Type.OU:
                    this.info = "OR";
                    this.id = this.info + String.Format("{0}", nbOR);
                    nbOR++;
                    break;
                case dnf.Type.NON:
                    this.info = "NOT";
                    this.id = this.info + String.Format("{0}", nbNOT);
                    nbNOT++;
                    break;
                case dnf.Type.NAND:
                    this.info = "NAND";
                    this.id = this.info + String.Format("{0}", nbNAND);
                    nbNAND++;
                    break;
                case dnf.Type.NOR:
                    this.info = "NOR";
                    this.id = this.info + String.Format("{0}", nbNOR);
                    nbNOR++;
                    break;
                case dnf.Type.XOR:
                    this.info = "XOR";
                    this.id = this.info + String.Format("{0}", nbXOR);
                    nbXOR++;
                    break;
                case dnf.Type.XNOR:
                    this.info = "XNOR";
                    this.id = this.info + String.Format("{0}", nbXNOR);
                    nbXNOR++;
                    break;
                default:
                    break;
            }
            this.type = type;
            this.children = children;
            //this.id = ExprBool.generateID();

        }

    };

    //Convertit l'arbre binaire de type ExprBool en arbre binaire de type ExprBoolNode
    private static ExprBoolNode Binary_To_ExprBoolNode(ExprBool tree)
    {
        if (tree.type == dnf.Type.VALEUR)
        {
            return new ExprBoolNode(tree.info);
        }
        else
        {
            List<ExprBoolNode> children = new List<ExprBoolNode>();
            if (tree.type == dnf.Type.NON)
            {
                children.Add(Binary_To_ExprBoolNode(tree.fd));
            }
            else
            {
                children.Add(Binary_To_ExprBoolNode(tree.fd));
                children.Add(Binary_To_ExprBoolNode(tree.fg));
            }
            return new ExprBoolNode(tree.type, children);
        }
    }

    //Fonction pour ajouter des fils à l'arbre syntaxique m-aire
    //Une valeur inférieure à 2 pour le nombre d'entrée considérera que le nombre d'entrées est illimité
    private static void DevelopChildren(List<ExprBoolNode> children, dnf.Type op, ExprBoolNode simple_node, int nb_entrees)
    {
        if (nb_entrees >= 2)
        {
            if ((simple_node.type != op) && (children.Count + 1 <= nb_entrees))
            {
                children.Add(simple_node);
            }
            else
            {
                if (children.Count + simple_node.children.Count <= nb_entrees)
                    children.AddRange(simple_node.children);
                else
                {
                    if (children.Count + 1 <= nb_entrees)
                        children.Add(simple_node);
                }
            }
        }
        else
        {
            if (simple_node.type != op)
                children.Add(simple_node);
            else
                children.AddRange(simple_node.children);
            /*nb_entrees = getAutoNbEntrees(children.Count + 1);
              if ((simple_node.type != op) && (children.Count + 1 <= nb_entrees))
              {
                  children.Add(simple_node);
              }
              else
              {
                  nb_entrees = getAutoNbEntrees(children.Count + simple_node.children.Count);
                  if (children.Count + simple_node.children.Count <= nb_entrees)
                      children.AddRange(simple_node.children);
                  else
                  {
                      nb_entrees = getAutoNbEntrees(children.Count + 1);
                      if (children.Count + 1 <= nb_entrees)
                          children.Add(simple_node);
                  }
              }*/
        }
    }
    
    private static int getAutoNbEntrees(int nb_children)
    {
        if(nb_children<4)
        {
            return 2;
        }
        else
        {
            if((nb_children>=4)&&(nb_children<8))
            {
                return 4;
            }
            else
            {
                if ((nb_children >= 8) && (nb_children < 16))
                {
                    return 8;
                }
                else
                {
                    return 16;
                }
            }
        }
    }

    //Fonction récursive pour la création de l'arbre m-aire à partir de l'abre binaire
    private static ExprBoolNode Binary_To_N_ary(ExprBoolNode tree, int nb_and, int nb_or, int nb_nand, int nb_nor,int nb_xor, int nb_xnor, Dictionary<string,int> gates)
    {
        if (tree.type == dnf.Type.VALEUR)
        {
            return tree;
        }
        else
        {
            dnf.Type op = tree.type;
            List<ExprBoolNode> children = new List<ExprBoolNode>();
            ExprBoolNode simple_node;
            foreach (ExprBoolNode node in tree.children)
            {
                simple_node = Binary_To_N_ary(node, nb_and, nb_or, nb_nand, nb_nor,nb_xor,nb_xnor,gates);
                switch (tree.type)
                {
                    case dnf.Type.ET:
                        {
                            DevelopChildren(children, op, simple_node, nb_and);
                            break;
                        }
                    case dnf.Type.OU:
                        {
                            DevelopChildren(children, op, simple_node, nb_or);
                            break;
                        }
                    case dnf.Type.NON:
                        {
                            DevelopChildren(children, op, simple_node, 1);
                            break;
                        }
                    case dnf.Type.NAND:
                        {
                            DevelopChildren(children, op, simple_node, nb_nand);
                            break;
                        }
                    case dnf.Type.NOR:
                        {
                            DevelopChildren(children, op, simple_node, nb_nor);
                            break;
                        }
                    case dnf.Type.XOR:
                        {
                            DevelopChildren(children, op, simple_node, nb_xor);
                            break;
                        }
                    case dnf.Type.XNOR:
                        {
                            DevelopChildren(children, op, simple_node, nb_xnor);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return new ExprBoolNode(tree.type, children);
        }
    }

    //Convertit une expression en arbre syntaxique m-aire
    public static ExprBoolNode To_N_ary(string expression, int nb_and, int nb_or, int nb_nand, int nb_nor,int nb_xor,int nb_xnor, Dictionary<string, int> gates)
    {
        expression = expression.Replace(" ", "");
        string postfix = "";
        List<string> listVars = new List<string>();
        ExprBool.To_RNP(expression, ref postfix, listVars);
        ExprBool? root = ExprBool.expressionTreeWithAllOperators(postfix, listVars);

        ExprBoolNode binary_root = Binary_To_ExprBoolNode(root);
        if(gates == null)
            gates = new Dictionary<string, int>();
        gates["ET"] = 0;
        gates["OU"] = 0;
        gates["NON"] = 0;
        gates["NAND"] = 0;
        gates["NOR"] = 0;
        gates["XOR"] = 0;
        gates["XNOR"] = 0;
        ExprBoolNode n_ary_root = Binary_To_N_ary(binary_root, nb_and, nb_or,nb_nand,nb_nor,nb_xor,nb_xnor,gates);
        CalculerNbPortes(n_ary_root, gates);
        return n_ary_root;
    }

    //Convertit une expression en arbre syntaxique m-aire d'un arbre binaire
    public static ExprBoolNode To_N_ary_From_Binary(ExprBool root, Dictionary<string, int> gates)
    {
        //ExprBool.Draw_Tree(root);
        ExprBoolNode binary_root = Binary_To_ExprBoolNode(root);
        
        if (gates == null)
            gates = new Dictionary<string, int>();
        gates["ET"] = 0;
        gates["OU"] = 0;
        gates["NON"] = 0;
        gates["NAND"] = 0;
        gates["NOR"] = 0;
        gates["XOR"] = 0;
        gates["XNOR"] = 0;
        CalculerNbPortes(binary_root, gates);
        return binary_root;
    }

    //Affiche un arbre binaire de type ExprBool
    public static void Affich_Binary(ExprBool tree)
    {
        if (tree != null)
        {
            Console.WriteLine(tree.info);
            Affich_Binary(tree.fd);
            Affich_Binary(tree.fg);
        }
    }

    //Affiche un arbre de type ExprBoolNode
    public static void Affich_Arbre(ExprBoolNode tree)
    {
        if (tree != null)
        {
            Console.WriteLine(tree.info);
            foreach (ExprBoolNode node in tree.children)
            {
                Affich_Arbre(node);
            }
        }
    }

    //Caluler le nombre de portes d'un circuit
    public static void CalculerNbPortes(ExprBoolNode tree, Dictionary<string, int> gates)
    {
        if (tree != null)
        {
            switch (tree.type)
            {
                case dnf.Type.ET:
                    {
                        gates["ET"] += 1;
                        break;
                    }
                case dnf.Type.OU:
                    {
                        gates["OU"] += 1;
                        break;
                    }
                case dnf.Type.NON:
                    {
                        gates["NON"] += 1;
                        break;
                    }
                case dnf.Type.NAND:
                    {
                        gates["NAND"] += 1;
                        break;
                    }
                case dnf.Type.NOR:
                    {
                        gates["NOR"] += 1;
                        break;
                    }
                case dnf.Type.XOR:
                    {
                        gates["XOR"] += 1;
                        break;
                    }
                case dnf.Type.XNOR:
                    {
                        gates["XNOR"] += 1;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            CalculerNbPortesRecursive(tree, gates);
        }
    }
    //Caluler le nombre de portes d'un circuit (Récursive)
    public static void CalculerNbPortesRecursive(ExprBoolNode tree,Dictionary<string,int> gates)
    {
        if (tree != null)
        {
            foreach (ExprBoolNode node in tree.children)
            {
                CalculerNbPortesRecursive(node,gates);
                switch(node.type)
                {
                    case dnf.Type.ET:
                        {
                            gates["ET"] += 1;
                            break;
                        }
                    case dnf.Type.OU:
                        {
                            gates["OU"] += 1;
                            break;
                        }
                    case dnf.Type.NON:
                        {
                            gates["NON"] += 1;
                            break;
                        }
                    case dnf.Type.NAND:
                        {
                            gates["NAND"] += 1;
                            break;
                        }
                    case dnf.Type.NOR:
                        {
                            gates["NOR"] += 1;
                            break;
                        }
                    case dnf.Type.XOR:
                        {
                            gates["XOR"] += 1;
                            break;
                        }
                    case dnf.Type.XNOR:
                        {
                            gates["XNOR"] += 1;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }

    public static ExprBoolNode N_ary_DNF_ExpressionTree(string expression)
    {
        List<string> minterms = (expression.Split('+')).ToList();
        List<ExprBoolNode> children = new List<ExprBoolNode>(minterms.Count);
        ExprBoolNode root;
        if (minterms.Count == 1)
        {
            string[] litterals = minterms[0].Split('.');
            List<ExprBoolNode> litteralsNodes = new List<ExprBoolNode>(litterals.Length);
            if (litterals.Length == 1)
            {
                root = new ExprBoolNode(litterals[0]);
            }
            else
            {
                foreach (var litteral in litterals)
                {
                    litteralsNodes.Add(new ExprBoolNode(litteral));
                }

                root = new ExprBoolNode(dnf.Type.ET, litteralsNodes);
            }

        }
        else
        {
            foreach (string minterm in minterms)
            {
                children.Add(new ExprBoolNode(minterm));
            }

            root = new ExprBoolNode(dnf.Type.OU, children);
        }


        for (var i = 0; i < root.children.Count; i++)
        {
            string[] litterals = root.children[i].info.Split('.');
            List<ExprBoolNode> litteralsNodes = new List<ExprBoolNode>(litterals.Length);
            if (litterals.Length == 1)
            {
                root.children[i] = new ExprBoolNode(litterals[0]);
            }
            else
            {
                foreach (string litteral in litterals)
                {
                    litteralsNodes.Add(new ExprBoolNode(litteral));
                }
                root.children[i] = new ExprBoolNode(dnf.Type.ET, litteralsNodes);
            }

        }
        return root;

        // tree construction finished
    }
    public static ExprBoolNode N_ary_CNF_ExpressionTree(string expression)
    {
        List<string> maxterms = (expression.Split('.')).ToList();
        List<ExprBoolNode> children = new List<ExprBoolNode>(maxterms.Count);
        ExprBoolNode root;
        if (maxterms.Count == 1)
        {
            string[] litterals = maxterms[0].Split('+');
            List<ExprBoolNode> litterlasNodes = new List<ExprBoolNode>(litterals.Length);
            if (litterals.Length == 1)
            {
                root = new ExprBoolNode(litterals[0]);
            }
            else
            {
                foreach (var litteral in litterals)
                {
                    litterlasNodes.Add(new ExprBoolNode(litteral));
                }
                root = new ExprBoolNode(dnf.Type.OU, litterlasNodes);
            }

        }
        else
        {
            foreach (string maxterm in maxterms)
            {
                children.Add(new ExprBoolNode(maxterm));
            }

            root = new ExprBoolNode(dnf.Type.ET, children);
        }


        for (var i = 0; i < root.children.Count; i++)
        {
            string[] litterals = root.children[i].info.Split('+');
            List<ExprBoolNode> litterlasNodes = new List<ExprBoolNode>(litterals.Length);
            if (litterals.Length == 1)
            {
                root.children[i] = new ExprBoolNode(litterals[0]);
            }
            else
            {
                foreach (var litteral in litterals)
                {
                    litterlasNodes.Add(new ExprBoolNode(litteral));
                }
                root.children[i] = new ExprBoolNode(dnf.Type.OU, litterlasNodes);
            }

        }
        return root;

        // tree construction finished
    }

    //Fonction pour visualiser l'arbre sur GraphViz (DOT)
    public static void Tree_Visualisation(ExprBoolNode tree)
    {
        string? path = Directory.GetCurrentDirectory() + "\\tree.txt";
        // creer le fichier text 'tree2.txt'
        File.WriteAllText(path, "");
        // nombre des noeuds nils
        int nbnils = 0;

        // construction du fichier 'tree.txt' (en langage DOT)
        File.AppendAllText(path, "strict graph arbre {\n");
        File.AppendAllText(path, "\tordering = out;\n");
        File.AppendAllText(path, "\tsplines = false;\n");

        //Début construction
        Tree_Visualisation_Recursive(tree, path);

        File.AppendAllText(path, "}\n");
        // End construction

        // conversion du fichier text en fichier png
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        // hide the terminal
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C dot -Tpng tree.txt -o tree.png";
        process.StartInfo = startInfo;
        process.Start();
        process.Close();
        // ouverture du fichier 'tree.png'
        startInfo.Arguments = "/C .\\tree.png";
        process.StartInfo = startInfo;
        process.Start();
        // End visualisation
    }

    //Fonction récursive pour la construction la visualisation de l'arbre
    private static void Tree_Visualisation_Recursive(ExprBoolNode tree, string? path)
    {
        File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", tree.info, tree.id));
        string listentrees = "{";
        foreach (ExprBoolNode child in tree.children)
        {
            Tree_Visualisation_Recursive(child, path);
            listentrees += "\"" + child.id + "\",";
        }
        if (listentrees != "{")
        {
            listentrees = listentrees.Substring(0, listentrees.Length - 1);
            listentrees += "}";
            File.AppendAllText(path, String.Format("\t\"{0}\" -- {1}\n", tree.id, listentrees));
        }
    }

    //Construit le circuit grâce à GraphViz (DOT)
    public static void Circuit_Visualisation(ExprBoolNode root,List<string> variables)
    {
        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Synthese");
        string? path = Directory.GetCurrentDirectory() + "\\Synthese\\synthese.txt";
        // creer le fichier text 'synthese.txt'
        File.WriteAllText(path, "");
        // nombre des noeuds nils
        int nbnils = 0;
        string listentrees = "";
        // construction du fichier 'synthese.txt' (en langage DOT)
        File.AppendAllText(path, "graph arbre{\n");
        File.AppendAllText(path, "\tsplines = ortho;\n");
        File.AppendAllText(path, "\trankdir=\"LR\";\n");
        File.AppendAllText(path, "\tranksep=2;\n");
        File.AppendAllText(path, "\tnode[width=0.5, height=0.5, shape=box, fontsize=16];\n");
        File.AppendAllText(path, "\tedge[arrowhead=none,penwidth=2];\n");

        Circuit_Visualisation_Recursive(root, path, ref nbnils);

        string rankSettings = "\t{rank = same; ";
        for (int i=0;i<variables.Count;i++)
        {
            rankSettings += variables[i] + ";";
        }
        rankSettings += "}\n";
        File.AppendAllText(path, rankSettings);
        File.AppendAllText(path, "}\n");
        // End construction

        // conversion du fichier text en fichier png
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        // hide the terminal 
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.CreateNoWindow = true;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C dot -Tpng Synthese\\synthese.txt -o Synthese\\synthese.png";
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
        process.Close();
        // End visualisation
    }

    //Fonction récursive pour la construction du circuit
    private static void Circuit_Visualisation_Recursive(ExprBoolNode root, string? path, ref int nbnils)
    {
        switch (root.info)
        {
            case "OR":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/OR.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            case "AND":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/AND.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            case "NOT":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/NOT.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            case "NAND":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/NAND.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            case "NOR":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/NOR.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            case "XOR":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/XOR.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            case "XNOR":
                {
                    File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/XNOR.png\",fixedsize=true,shape=plaintext] \n", root.id));
                    break;
                }
            default:
                {
                    File.AppendAllText(path, String.Format("\t\"{1}\" [label=\"{0}\",width=0.75,height=0.75] \n", root.info.Replace("!", ""), root.id));
                    break;
                }
        }
        string listentrees = "{";
        for (int i = 0; i < root.children.Count; i++)
        {
            Circuit_Visualisation_Recursive(root.children[i], path, ref nbnils);
            listentrees += "\"" + root.children[i].id + "\",";
        }
        if (listentrees != "{")
        {
            listentrees = listentrees.Substring(0, listentrees.Length - 1);
            listentrees += "}";
            File.AppendAllText(path, String.Format("\t\"NIL{0}\" [label=\"\",shape = box,width=.001,height = {1}] \n", nbnils, root.children.Count / 4));
            File.AppendAllText(path, String.Format("\t\"NIL{0}\" -- \"{1}\" [label=\"{2} bit(s)\",len=1]\n", nbnils, root.id,root.children.Count));
            File.AppendAllText(path, String.Format("\t{0} -- \"NIL{1}\"\n", listentrees, nbnils++));
        }
    }
}