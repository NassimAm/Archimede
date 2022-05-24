using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dnf;

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Interaction logic for ResultTransformation.xaml
    /// </summary>
    public partial class ResultTransformation : Page
    {
        public ResultTransformation()
        {
            InitializeComponent();
            ExprBool? tree = null;
            StringBuilder sb = new StringBuilder();
            string result = "";

            switch (Data.codeTransformation)
            {

                case '0':
                    tree = ExprBool.ParseExpression(Data.expression);
                    tree = ExprBool.dnf(tree);
                    ExprBool.inorder(tree, sb);
                    result = sb.ToString();
                    result = ExprBool.SimplifyDNFExpression(result);

                    break;
                case '1':
                    tree = ExprBool.ParseExpression(Data.expression);
                    tree = ExprBool.cnf(tree);
                    ExprBool.inorder(tree, sb);
                    result = sb.ToString();
                    result = ExprBool.SimplifyCNFExpression(result);

                    if (result == "0")
                    {
                        expressionTransforme.Text = "Faux";
                        Data.expression = "0";
                        return;
                    }
                    if (result == "1")
                    {
                        expressionTransforme.Text = "Vrai";
                        Data.expression = "1";
                        return;
                    }


                    sb = new StringBuilder();
                    sb.Append("(");
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (result[i] != '.')
                        {
                            sb.Append(result[i]);
                        }
                        else
                        {
                            sb.Append(").(");
                        }
                    }
                    sb.Append(")");
                    result = sb.ToString();
                    break;
                case '2':
                    tree = ExprBool.ParseExpression(Data.expression);
                    tree = ExprBool.onlyNand(tree);
                    ExprBool.inorder(tree, sb);
                    result = sb.ToString();
                    sb = new StringBuilder();
                    ExprBool.inorderParanthese(tree, sb);
                    result = sb.ToString();
                    break;
                case '3':
                    tree = ExprBool.ParseExpression(Data.expression);
                    tree = ExprBool.onlyNor(tree);

                    sb = new StringBuilder();
                    ExprBool.inorderParanthese(tree, sb);

                    result = sb.ToString();
                    break;
                default:
                    return;
            }

            expressionTransforme.Text = result;
            Data.expression = result;
        }

        private void syntheseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("pack://application:,,,/Pages/SynthesePage.xaml", UriKind.Absolute));
        }
    }
}