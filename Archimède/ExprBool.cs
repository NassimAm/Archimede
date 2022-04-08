using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnf
{


    //type des noeuds de l'arbre v
    enum Type
    {

        VALEUR,
        ET,
        OU,
        NON

    };



    class ExprBool
    {

        public Type type;

        public string info = ""; //dans le cas d'une valeur 

        public ExprBool? fg; //fils gauche

        public ExprBool? fd; //fils droit

        public string id;




        /// <summary>constructeur pour les noeuds 'VALEUR' </summary>
        public ExprBool(string value)
        {

            this.info = value;
            type = Type.VALEUR;
            fg = null;
            fd = null;
            this.id = generateID();
        }

        /// <summary>constructeur pour les autres types </summary>
        public ExprBool(Type type, ExprBool? fg, ExprBool? fd)
        {

            this.type = type;
            this.fg = fg;
            this.fd = fd;

            switch (type)
            {
                case Type.NON:
                    this.info = "!";
                    break;
                case Type.ET:
                    this.info = ".";
                    break;
                case Type.OU:
                    this.info = "+";
                    break;

            }
            this.id = generateID();

        }





        /// <summary>
        /// cette fonction foncionne si l'arbre est en forme DNF 
        ///elle retourne une liste des mintermes 
        ///ex : si l'arbre represente [a.b.c + !a.b + k.l]=> elle retourne une liste des minterms  { a.b.c , !a,b  , k.l }
        /// </summary>
        public void getMintermes(List<ExprBool> mintermes)
        {

            switch (type)
            {
                case Type.OU:
                    fg.getMintermes(mintermes);
                    fd.getMintermes(mintermes);
                    break;

                case Type.ET:
                case Type.NON:
                case Type.VALEUR:

                    mintermes.Add(this);
                    break;

                default:
                    return;
            }

        }

        /// <summary>clones (this) and returns a shallow copy of the actual node</summary>
        public ExprBool? clone()

        {
            if (this == null) return null;

            ExprBool clonedTree = (ExprBool)this.MemberwiseClone();
            clonedTree.id = generateID();
            if (clonedTree.fg != null) clonedTree.fg = clonedTree.fg.clone();
            if (clonedTree.fd != null) clonedTree.fd = clonedTree.fd.clone();

            return clonedTree;
        }



        /// <summary>
        /// gets the Disjunctive Normal Form from an expression's syntactic tree of root `tete`,
        /// </summary>
        /// <remarks>Note : the root of the equivalent tree doesn't have to be `tete`</remarks>
        /// <returns>the root of the equivalent syntactic tree in DNF</returns>

        public static ExprBool? dnf(ExprBool tete)
        {

            if (tete == null) return null;

            //hauteur ==  1   
            if (tete.type == Type.VALEUR) return tete;//a

            //hauteur == 2
            switch (tete.type)
            {

                case Type.NON:
                    if (tete.fd.type == Type.VALEUR) return tete;  //!a 
                    break;

                case Type.OU:
                case Type.ET:
                    if (tete.fg.type == Type.VALEUR && tete.fd.type == Type.VALEUR) return tete; //a+b , a.b
                    break;

                default:
                    return null;
            }

            //hauteur >= 3

            tete.fg = dnf(tete.fg);

            tete.fd = dnf(tete.fd);


            //!( negation d'une expression en forme disjonctif )
            if (tete.type == Type.NON)
            {
                if (tete.fd.type == Type.NON)
                {
                    return dnf(tete.fd.fd);
                }// double negation 

                tete = negation(tete.fd);  //negation d'une forme disjonctif avec les lois de morgan

                return dnf(tete);
            }


            //  si un noeud est une valeur , on va le mettre a gauche 
            //  Conventional order: (fg.type , fd.type)  (OU, NON), (ET, NON), (ET, OU).

            if (tete.fg.type != Type.VALEUR && tete.fd.type == Type.VALEUR) (tete.fg, tete.fd) = (tete.fd, tete.fg);


            if (tete.fg.type == Type.NON && (tete.fd.type == Type.OU || tete.fd.type == Type.ET)) (tete.fg, tete.fd) = (tete.fd, tete.fg);
            else if (tete.fg.type == Type.OU && tete.fd.type == Type.ET) (tete.fg, tete.fd) = (tete.fd, tete.fg);



            // un OU entre deux formules en forme disjonctive reste une forme disjonctive
            if (tete.type == Type.OU) return tete;

            // un ET entre deux formules en forme disjonctive ne donne pas une forme disjonctive
            if (tete.type == Type.ET)
            {

                if (tete.fg.type == Type.VALEUR && tete.fd.type == Type.NON) return tete;  // a . !b
                if (tete.fg.type == Type.VALEUR && tete.fd.type == Type.ET) return tete; //a . (b . c)

                if (tete.fg.type == Type.VALEUR && tete.fd.type == Type.OU)
                {

                    // x  . ( a  +  b)  =  x.a + x.b
                    ExprBool

                        x = tete.fg,
                        a = tete.fd.fg,
                        b = tete.fd.fd,


                    xa = new ExprBool(Type.ET, x.clone(), a.clone()), // x.a
                    xb = new ExprBool(Type.ET, x.clone(), b.clone()); // x.b

                    xa = dnf(xa);
                    xb = dnf(xb);

                    ExprBool xa_xb = new ExprBool(Type.OU, xa, xb);  // x.a + x.b


                    return xa_xb;

                }

                // une conjonction des conjonctions des litteraux est une forme disjonctive
                if (tete.fg.type == Type.ET && tete.fd.type == Type.ET) return tete;  // a.c . b.d


                if (tete.fg.type == Type.VALEUR && tete.fd.type == Type.VALEUR) return tete; // a.b


                //  tete.fd.type ne peut pas etre "ET" ou "OU",
                //  donc il peut etre une "VALEUR" ou un "NON"
                if (tete.fg.type == Type.NON) return tete;


                //  tete.fg.type peut etre "OU" ou bien "ET" (à cause de l'ordre conventionel)
                if (tete.fd.type == Type.NON)
                {

                    if (tete.fg.type == Type.ET) return tete;  // (a.b).!c
                    else
                    {

                        //  (a|b) & !c => (a&!c) | (b&!c):
                        ExprBool


                         a = tete.fg.fg,

                         b = tete.fg.fd,


                         c = tete.fd, // c represente !c

                         ac = new ExprBool(Type.ET, a.clone(), c.clone()), //a.!c
                         bc = new ExprBool(Type.ET, b.clone(), c.clone()); //b.!c

                        ac = dnf(ac);
                        bc = dnf(bc);

                        ExprBool ac_bc = new ExprBool(Type.OU, ac, bc); // a.!c + b.!c

                        return (ac_bc);

                    }

                }


                //  tete.fd.type ne peut pas etre 'ET' ,
                // donc il peut etre 'OU' ou 'NON' , Mais le cas 'NON' a été traité auparavent
                if (tete.fg.type == Type.OU)
                {
                    // on est sur que tete.fd.type == 'OU'
                    ExprBool
                        a = tete.fg.fg,
                        b = tete.fg.fd,
                        c = tete.fd.fg,
                        d = tete.fd.fd;


                    //(a|b) & (c|d) => a&b | a&c | b&c | b&d


                    ExprBool
                        etNoeud1 = new ExprBool(Type.ET, a.clone(), c.clone()),  // a.b

                        etNoeud2 = new ExprBool(Type.ET, a.clone(), d.clone()),  // a.c

                        etNoeud3 = new ExprBool(Type.ET, b.clone(), c.clone()),  // b.c

                        etNoeud4 = new ExprBool(Type.ET, b.clone(), d.clone());  // b.d


                    etNoeud1 = dnf(etNoeud1);
                    etNoeud2 = dnf(etNoeud2);
                    etNoeud3 = dnf(etNoeud3);
                    etNoeud4 = dnf(etNoeud4);

                    ExprBool

                       gOuNoeud = new ExprBool(Type.OU, etNoeud1, etNoeud2), // le noeud fils gauche de la nouvelle tete = a.c+a.d

                       dOuNoeud = new ExprBool(Type.OU, etNoeud3, etNoeud4); // le noeud fils droit de la nouvelle tete  = b.c+b.d

                    ExprBool newTete = new ExprBool(Type.OU, gOuNoeud, dOuNoeud);  // la nouvelle tete : (a.c+a.d)+(b.c+b.d)

                    return newTete;


                }

                // une conjonction d'une conjonction et une disjonction n'est pas une forme disjonctive (il faut distribuer)
                if (tete.fg.type == Type.ET && tete.fd.type == Type.OU)
                {

                    ExprBool?
                        x = tete.fg,
                        c = tete.fd.fg,
                        d = tete.fd.fd,

                        xc = new ExprBool(Type.ET, x.clone(), c.clone()),
                        xd = new ExprBool(Type.ET, x.clone(), d.clone());

                    xc = dnf(xc);
                    xd = dnf(xd);

                    ExprBool xc_xd = new ExprBool(Type.OU, xc, xd);

                    return xc_xd;

                }


            }   //tete.type == Type.ET (Line 194)


            return null;

        }




        /// <summary>returns the negation of a DNF expression</summary>
        public static ExprBool? negation(ExprBool tete)
        {
            List<ExprBool> minterms = new List<ExprBool>();
            tete.getMintermes(minterms);

            StringBuilder mintermString;

            string[] literales;
            ExprBool newLiterale;
            ExprBool? conjonction = null;
            ExprBool? disjonction = null;

            foreach (ExprBool minterm in minterms)
            {

                mintermString = new StringBuilder();
                ExprBool.inorder(minterm, mintermString);

                literales = mintermString.ToString().Split(".");


                disjonction = null;
                foreach (string literal in literales)
                {

                    if (literal[0] == '!')
                    {
                        newLiterale = new ExprBool(literal[1..]);
                    }
                    else
                    {
                        newLiterale = new ExprBool(Type.NON, null, new ExprBool(literal));
                    }

                    if (disjonction == null)
                    {
                        disjonction = newLiterale;
                    }
                    else
                    {
                        disjonction = new ExprBool(Type.OU, disjonction, newLiterale);
                    }

                }

                if (conjonction == null)
                {
                    conjonction = disjonction;
                }
                else
                {
                    conjonction = new ExprBool(Type.ET, conjonction, disjonction);
                }


            }

            return conjonction;

        }


        /// <summary>defines the priority of each logical operator</summary>
        public static int Priority(char chr)
        {
            switch (chr)
            {
                case '!':
                    return 7;

                case '§':   // XNOR
                    return 6;

                case '>':   // NAND
                    return 6;

                case '<':   // NOR
                    return 6;

                case '^':   // XOR  
                    return 5;

                case '&':
                    return 4;

                case '.':
                    return 4;

                case '|':
                    return 3;

                case '+':
                    return 3;

                case '-':   // IMPLICATION
                    return 2;

                case '=':   // EQUIVALENCE
                    return 1;

                default:
                    return 0;

            }
        }

        /// <summary>returns the reverse polish notation of an expression written in infix notation</summary>
        public static string To_RNP(string infix)
        {
            string postfix = "";
            char x;
            bool stop;
            Stack<char> st = new Stack<char>();
            foreach (char chr in infix)
            {
                if (chr == '(')
                {
                    st.Push(chr);
                }
                else if (ExprBool.isOperator(chr))
                {
                    stop = false;
                    while (!stop && st.Count > 0)
                    {
                        x = st.Pop();
                        if (Priority(x) < Priority(chr) || x == '(')
                        {
                            st.Push(x);
                            stop = true;
                        }
                        else
                        {
                            postfix += x;
                        }
                    }
                    st.Push(chr);
                }
                else if (chr == ')')
                {
                    stop = false;
                    while (!stop && st.Count > 0)
                    {
                        x = st.Pop();
                        if (x == '(')
                            stop = true;
                        else
                            postfix += x;
                    }
                }
                else
                { // chr is an operand
                    postfix += chr;
                }
            }
            while (st.Count > 0)
            {
                postfix += st.Pop();
            }
            return postfix;
        }



        /// <summary>checks if a character (ch) is an operator</summary>

        public static bool isOperator(char ch)
        {
            if (ch == '!' || ch == '&' || ch == '.' || ch == '|' || ch == '+' || ch == '>' || ch == '<' || ch == '^' || ch == '§' || ch == '-' || ch == '=')
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// parse a boolean expression (postfix) to a binary expression tree,
        /// and return a root for this tree.
        /// </summary>
        public static ExprBool expressionTree(String postfix)
        {

            Stack<ExprBool> st = new Stack<ExprBool>();
            ExprBool t1, t2, temp;
            Dictionary<char, Type> operators = new Dictionary<char, Type>();
            operators.Add('!', Type.NON);
            operators.Add('&', Type.ET);
            operators.Add('|', Type.OU);
            operators.Add('.', Type.ET);



            for (int i = 0; i < postfix.Length; i++)  // we loop through the expression
            {
                if (!isOperator(postfix[i]))
                {    // value found
                    temp = new ExprBool($"{postfix[i]}");

                    st.Push(temp);
                }
                else
                {  // operator found

                    if (postfix[i] == '!')
                    { //  unary operator 
                        temp = new ExprBool(Type.NON, null, null);  // creates a node temp with the operator as value

                        t1 = st.Pop();

                        temp.fd = t1;
                        st.Push(temp);
                    }
                    else
                    { //  binary operator
                        // pop two values of the stack
                        t1 = st.Pop();
                        t2 = st.Pop();

                        if (postfix[i] == '^') // XOR
                        {
                            ExprBool
                                notB = new ExprBool(Type.NON, null, t1),
                                notA = new ExprBool(Type.NON, null, t2),
                                A_notB = new ExprBool(Type.ET, t2, notB),
                                B_notA = new ExprBool(Type.ET, t1, notA),
                                AxorB = new ExprBool(Type.OU, A_notB, B_notA);
                            st.Push(AxorB);
                        }
                        else if (postfix[i] == '§') // XNOR
                        {
                            ExprBool
                                notB = new ExprBool(Type.NON, null, t1),
                                notA = new ExprBool(Type.NON, null, t2),
                                A_notB = new ExprBool(Type.OU, t2, notB),
                                B_notA = new ExprBool(Type.OU, t1, notA),
                                AxnorB = new ExprBool(Type.ET, A_notB, B_notA);
                            st.Push(AxnorB);
                        }
                        else if (postfix[i] == '>')
                        { // NAND
                            ExprBool
                                notA = new ExprBool(Type.NON, null, t2),
                                notB = new ExprBool(Type.NON, null, t1),
                                notA_notB = new ExprBool(Type.ET, notA, notB);
                            st.Push(notA_notB);
                        }
                        else if (postfix[i] == '<')
                        { // NOR
                            ExprBool
                                notA = new ExprBool(Type.NON, null, t2),
                                notB = new ExprBool(Type.NON, null, t1),
                                notA_notB = new ExprBool(Type.OU, notA, notB);
                            st.Push(notA_notB);
                        }
                        else if (postfix[i] == '-')
                        {   // IMPLICATION : (a → b) = !a+b
                            ExprBool
                                notA = new ExprBool(Type.NON, null, t2),
                                notA_B = new ExprBool(Type.OU, notA, t1);
                            st.Push(notA_B);
                        }
                        else if (postfix[i] == '=')
                        {   // EQUIVALENCE : (a ←→ b) = (!a+b).(!b+a)
                            ExprBool
                                notA = new ExprBool(Type.NON, null, t2),
                                notB = new ExprBool(Type.NON, null, t1),
                                notA_B = new ExprBool(Type.OU, notA, t1),
                                notB_A = new ExprBool(Type.OU, notB, t2),
                                A_B = new ExprBool(Type.ET, notA_B, notB_A);
                            st.Push(A_B);
                        }
                        else
                        {  // operator is 'AND' or 'OR' 
                            temp = new ExprBool(operators[postfix[i]], t2, t1);
                            st.Push(temp);
                        }

                    }

                }

            }
            temp = st.Pop(); // we pop the remaining node in the stack, which will be the tree's root
            return temp;
        }

        /// <summary>
        /// inorder traversal for a binary tree
        /// </summary>
        public static void inorder(ExprBool? root)
        {
            if (root == null)
                return;


            inorder(root.fg);

            switch (root.type)
            {
                case Type.NON: Console.Write("!"); break;
                case Type.ET: Console.Write("."); break;
                case Type.OU: Console.Write("+"); break;
                case Type.VALEUR: Console.Write(root.info); break;


            }
            inorder(root.fd);



        }

        public static void inorder(ExprBool? root, StringBuilder expression)
        {
            if (root == null)
                return;



            inorder(root.fg, expression);

            switch (root.type)
            {
                case Type.NON: expression.Append("!"); break;
                case Type.ET: expression.Append("."); break;
                case Type.OU: expression.Append("+"); break;
                case Type.VALEUR: expression.Append(root.info); break;

            }

            inorder(root.fd, expression);



        }
        /// <summary>
        /// postorder traversal for a binary tree
        /// </summary>
        public static void postorder(ExprBool? root)
        {
            if (root == null)
                return;
            postorder(root.fg);
            postorder(root.fd);

            switch (root.type)
            {
                case Type.NON: Console.Write("!"); break;
                case Type.ET: Console.Write("."); break;
                case Type.OU: Console.Write("+"); break;
                case Type.VALEUR: Console.Write(root.info); break;


            }
        }




        //tree visualisation 
        public static void arbre_to_txt(ExprBool? root, ref int nbNils, string path)
        {

            if (root != null)
            {
                // dessiner un arc vers le fils gauche
                if (root.fg != null)
                {
                    File.AppendAllText(path, String.Format("  \"{1}\" [label=\"{0}\"]  \n", root.fg.info, root.fg.id));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", root.id, root.fg.id));
                }
                else
                {

                    File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbNils));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", root.id, nbNils++));
                    File.AppendAllText(path, " [style=invis];\n");
                }

                // Dessiner un fils NIL "virtuel" et "invisible" au milieu (pour une meilleure séparation des fils gauches et droits)

                File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbNils));
                File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", root.id, nbNils++));
                File.AppendAllText(path, " [style=invis];\n");

                // Dessiner un arc vers le fils droit
                if (root.fd != null)
                {
                    File.AppendAllText(path, String.Format("  \"{1}\" [label=\"{0}\"] \n", root.fd.info, root.fd.id));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"{1}\"; \n", root.id, root.fd.id));
                }
                else
                {

                    File.AppendAllText(path, String.Format("  \"NIL{0}\" [style=invis];\n", nbNils));
                    File.AppendAllText(path, String.Format("  \"{0}\" -- \"NIL{1}\" ", root.id, nbNils++));
                    File.AppendAllText(path, " [style=invis];\n");
                }

                // dessiner les sous-arbres gauche et droit
                arbre_to_txt(root.fg, ref nbNils, path);
                arbre_to_txt(root.fd, ref nbNils, path);
            }
        }

        public static void Draw_Tree(ExprBool root)
        {
            string? path = Directory.GetCurrentDirectory() + "\\tree.txt";
            File.WriteAllText(path, ""); // creer le fichier text 'tree.txt'
            int nbnils = 0; // nombre des noeuds nils
            // construction du fichier 'tree.txt' (en langage DOT)
            File.AppendAllText(path, "strict graph arbre {\n");
            File.AppendAllText(path, "\tordering = out;\n");
            File.AppendAllText(path, "\tsplines = false;\n");
            File.AppendAllText(path, String.Format(" \"{1}\" [label=\"{0}\"] \n", root.info, root.id));
            arbre_to_txt(root, ref nbnils, path);
            File.AppendAllText(path, "}\n");

            // conversion du fichier text en fichier png
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; // hide the terminal
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C dot -Tpng tree.txt -o tree.png";
            process.StartInfo = startInfo;
            process.Start();
            process.Close();
            // ouverture du fichier 'tree.png'
            startInfo.Arguments = "/C tree.png";
            process.StartInfo = startInfo;
            process.Start();

        }


        public static string generateID()  // for generating unique IDs
        {
            return Guid.NewGuid().ToString("N");
        }

        // end tree visualisation




    }

}
