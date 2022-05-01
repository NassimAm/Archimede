using System;
using System.Collections.Generic;
using System.Text;
using dnf;
class Synthese
{

    // Represents a node of an N-ary tree
    public class ExprBoolNode
    {
        public dnf.Type type;
        public string info = "";
        public List<ExprBoolNode> children = new List<ExprBoolNode>(100);
        public string? id;

        public ExprBoolNode(string value)
        {
            this.info = value;
            this.type = dnf.Type.VALEUR;
            this.id = ExprBool.generateID();
        }

        public ExprBoolNode(dnf.Type type, List<ExprBoolNode> children)
        {
            switch (type)
            {
                case dnf.Type.ET:
                    this.info = "AND";
                    break;
                case dnf.Type.OU:
                    this.info = "OR";
                    break;
                case dnf.Type.NON:
                    this.info = "NOT";
                    break;
                default:
                    break;
            }
            this.children = children;
            this.id = ExprBool.generateID();

        }

    };
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

    public static void Tree_Visualisation(ExprBoolNode root)
    {
        string? path = Directory.GetCurrentDirectory() + "\\tree2.txt";
        // creer le fichier text 'tree2.txt'
        File.WriteAllText(path, "");
        // nombre des noeuds nils
        int nbnils = 0;

        // construction du fichier 'tree.txt' (en langage DOT)
        File.AppendAllText(path, "strict graph arbre {\n");
        File.AppendAllText(path, "\tordering = out;\n");
        File.AppendAllText(path, "\tsplines = false;\n");
        // File.AppendAllText(path, "\trankdir = \"RL\";\n");
        if (root.info.Contains('!'))
        {
            // negated root
            string NOT_id = ExprBool.generateID();
            File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", "NON", NOT_id)); // generate a 'NOT' nod
            File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", root.info.Remove(0, 1), root.id));
            File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", NOT_id, root.id));
        }
        else
            File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", root.info, root.id));
        foreach (ExprBoolNode term in root.children)
        {
            if (term.type == dnf.Type.VALEUR)
            {
                if (term.info.Contains('!'))
                {
                    // negated term
                    string NOT_id = ExprBool.generateID();
                    File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", "NON", NOT_id)); // generate a 'NOT' node
                    File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbnils));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", root.id, NOT_id));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", root.id, nbnils++));
                    File.AppendAllText(path, " [style=invis];\n");


                    File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", term.info.Remove(0, 1), term.id));

                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", NOT_id, term.id));

                }
                else
                {
                    File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", term.info, term.id));
                    File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbnils));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", root.id, term.id));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", root.id, nbnils++));
                    File.AppendAllText(path, " [style=invis];\n");
                }
            }
            else
            {
                File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", term.info, term.id));
                File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbnils));
                File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", root.id, term.id));
                File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", root.id, nbnils++));
                File.AppendAllText(path, " [style=invis];\n");
            }

        }
        foreach (ExprBoolNode term in root.children)
        {
            foreach (ExprBoolNode litteral in term.children)
            {
                if (litteral.info.Contains('!'))
                {
                    // negated term
                    string NOT_id = ExprBool.generateID();
                    File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", "NON", NOT_id)); // generate a 'NOT' node
                    File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbnils));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", term.id, NOT_id));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", term.id, nbnils++));
                    File.AppendAllText(path, " [style=invis];\n");


                    File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", litteral.info.Remove(0, 1), litteral.id));

                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", NOT_id, litteral.id));

                }
                else
                {
                    File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", litteral.info, litteral.id));
                    File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbnils));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", term.id, litteral.id));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", term.id, nbnils++));
                    File.AppendAllText(path, " [style=invis];\n");
                }

            }
        }
        File.AppendAllText(path, "}\n");
        // End construction

        // conversion du fichier text en fichier png
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        // hide the terminal
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C dot -Tpng tree2.txt -o tree2.png";
        process.StartInfo = startInfo;
        process.Start();
        process.Close();
        // ouverture du fichier 'tree.png'
        startInfo.Arguments = "/C .\\tree2.png";
        process.StartInfo = startInfo;
        process.Start();
        // End visualisation
    }

    public static void Circuit_Visualisation(ExprBoolNode root)
    {
        string? path = Directory.GetCurrentDirectory() + "\\synthese.txt";
        // creer le fichier text 'tree2.txt'
        File.WriteAllText(path, "");
        // nombre des noeuds nils
        int nbnils = 0;
        string listentrees = "";
        // construction du fichier 'tree.txt' (en langage DOT)
        File.AppendAllText(path, "graph arbre{\n");
        File.AppendAllText(path, "\tsplines = ortho;\n");
        File.AppendAllText(path, "\trankdir=\"LR\";\n");
        File.AppendAllText(path, "\tranksep=1;\n");
        File.AppendAllText(path, "\tnode[width=0.5, height=0.5, shape=box, fontsize=16];\n");
        File.AppendAllText(path, "\tedge[arrowhead=none,penwidth=2];\n");
        if(root.info != "OR")
            File.AppendAllText(path, String.Format("\t\"{1}\" [label=\"{0}\"] \n", root.info, root.id));
        else
            File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/OR.png\",fixedsize=true,shape=plaintext] \n", root.id));

        listentrees += "{";
        foreach (ExprBoolNode term in root.children)
        {
            if(term.info != "AND")
                File.AppendAllText(path, String.Format("\t\"{1}\" [label=\"{0}\"] \n", term.info, term.id));
            else
                File.AppendAllText(path, String.Format("\t\"{0}\" [label=\"\",image=\"rsc/images/gates/AND.png\",fixedsize=true,shape=plaintext] \n", term.id));
            listentrees += "\""+term.id+"\",";
        }
        if(listentrees != "{")
        {
            listentrees = listentrees.Substring(0, listentrees.Length - 1);
            listentrees += "}";
            File.AppendAllText(path, String.Format("\t\"NIL{0}\" [label=\"\",shape = box,width=.001,height = {1}] \n", nbnils,root.children.Count / 4));
            File.AppendAllText(path, String.Format("\t\"NIL{0}\" -- \"{1}\"\n",nbnils,root.id));
            File.AppendAllText(path, String.Format("\t{0} -- \"NIL{1}\"\n", listentrees,nbnils++));
        }

        foreach (ExprBoolNode term in root.children)
        {
            listentrees = "{";
            foreach (ExprBoolNode litteral in term.children)
            {
                File.AppendAllText(path, String.Format("\t\"{1}\" [label=\"{0}\"] \n", litteral.info, litteral.id));
                listentrees += "\"" + litteral.id + "\",";
            }
            if (listentrees != "{")
            {
                listentrees = listentrees.Substring(0, listentrees.Length - 1);
                listentrees += "}";
                File.AppendAllText(path, String.Format("\t\"NIL{0}\" [label=\"\",shape = box,width=.001,height = {1}] \n", nbnils, term.children.Count / 4));
                File.AppendAllText(path, String.Format("\t\"NIL{0}\" -- \"{1}\"\n", nbnils, term.id));
                File.AppendAllText(path, String.Format("\t{0} -- \"NIL{1}\"\n", listentrees, nbnils++));
            }
        }
        File.AppendAllText(path, "}\n");
        // End construction

        // conversion du fichier text en fichier png
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        // hide the terminal
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C dot -Tpng synthese.txt -o synthese.png";
        process.StartInfo = startInfo;
        process.Start();
        process.Close();
        // ouverture du fichier 'tree.png'
        startInfo.Arguments = "/C .\\synthese.png";
        process.StartInfo = startInfo;
        process.Start();
        // End visualisation
    }
}


