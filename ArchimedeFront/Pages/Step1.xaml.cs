using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using dnf;
using Archimède;
using System.Diagnostics;

namespace ArchimedeFront.Pages
{
    /// <summary>
    /// Interaction logic for Step1.xaml
    /// </summary>
    public partial class Step1 : Page
    {
         int stepNumber = 1;
         int step4Number = 0;
        bool stop = false;

        public Step1()
        {
            InitializeComponent();

            List<string> mintermes;
            if (Data.literal)
            {
                Data.expressionTransforme = ExprBool.transformerDNF(Data.expression.Replace(" ", ""));
                expression.Text = Data.expressionTransforme;
                Data.variables = ExprBool.getVariables(Data.expressionTransforme);
                Data.nbVariables = Data.variables.Count;
                Data.stringListMinterm = ExprBool.getMinterms(Data.expressionTransforme, Data.variables);
                int maxNbUns = Data.stringListMinterm.MaxBy(x => x.Count(ch => (ch == '1' || ch == '-'))).Count(ch => (ch == '1' || ch == '-'));

                foreach (string mintermBinCode in Data.stringListMinterm)
                {
                    Impliquant impliquant = new Impliquant(mintermBinCode);
                    if (impliquant.nbDontCare > 0) Data.impliquantsEnAttente.Add(impliquant); // ces impliquants vont etre traités dans les prochaine groupe 
                    else (Data.impliquants).Add(impliquant); // impliquants  en forme canonique 
                }

                Data.groupeMintermes = new Mintermes(maxNbUns);
                 mintermes = Data.expressionTransforme.Split("+").ToList();
            }
            else
            {
                expression.Text = Data.expression;
                //Corriger les codes binaires (en ajoutant des zéros au début pour qu'ils aient tous la mê^me longueur)
                for (int i = 0; i < Data.mintermes.Count; i++)
                {
                    Data.mintermes[i].bincode = Data.mintermes[i].bincode.PadLeft(Data.nbVariables, '0');
                    Data.stringListMinterm.Add(Data.mintermes[i].bincode);

                }
                Data.groupeMintermes = new Mintermes(Minterme.maxNbUns);
                Data.impliquants = Data.groupeMintermes.InitImpliquants(Data.mintermes);
                Data.stringListMinterm = Data.stringListMinterm.Distinct().ToList();
                mintermes = Data.listMintermesString;
            }
           




            
            WrapPanel wrappanel;
            for (int i = 0;i < Data.stringListMinterm.Count;i++)
            {
                wrappanel = new WrapPanel();
                wrappanel.Orientation = Orientation.Vertical;
                wrappanel.VerticalAlignment = VerticalAlignment.Top;
                wrappanel.Margin = new Thickness(10, 0, 10, 10);
                wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(0, 0, 0, 10), Text = mintermes[i]});
                wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(0, 0, 0, 0), Text = Data.stringListMinterm[i]});
                mintermesList.Children.Add(wrappanel);

                if(i != mintermes.Count-1)
                {
                    wrappanel = new WrapPanel();
                    wrappanel.Orientation = Orientation.Vertical;
                    wrappanel.VerticalAlignment = VerticalAlignment.Top;
                    wrappanel.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(0, 0, 0, 0), Text = "+"});
                    mintermesList.Children.Add(wrappanel);
                }
            }
        }

        private void nextStepButton_click(object sender, RoutedEventArgs e)
        {
            switch (stepNumber)
            {
                case 1:
                    _NextStep2.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step2.xaml", UriKind.RelativeOrAbsolute));
                    step1.Margin = new Thickness(0, 0, 0,82);
                   
                    stepNumber++;
                    expandButtons.BringIntoView();
                    break;
                case 2:
                    _NextStep3.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step3.xaml", UriKind.RelativeOrAbsolute));
                    _NextStep2.Margin = new Thickness(0, 0, 0, 82);
                  

                    stepNumber++;
                    expandButtons.BringIntoView();
                    break;
                case 3:
                    _NextStep4.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step4.xaml", UriKind.RelativeOrAbsolute));
                    _NextStep3.Margin = new Thickness(0, 0, 0, 82);
                 
                  
                    expandBottomButton.Style =  FindResource("expandButtonHoriz")  as Style; 
                    int nbVariables = Data.nbVariables;

                    Data.cptGroupes++;
                    Data.impliquants = Data.groupeMintermes.generateNextGroupeImpliquants(Data.cptGroupes, Data.impliquantsEnAttente);
                   //affichage du groupe
                    Border border;
                    StackPanel groupesTable;

                        groupesTable = new StackPanel() { Margin = new Thickness(10, 30, 10, 30) };
                        foreach (List<Impliquant> groupe in Data.groupeMintermes.groupesImpliquants)
                        {
                            if(groupe.Count > 0)
                            {
                                foreach (Impliquant impliquant in groupe)
                                {
                                    if (impliquant.status)
                                     groupesTable.Children.Add(generatePrimeImplicant(impliquant.bincode));
                                    else groupesTable.Children.Add(generateCheckedImplicant(impliquant.bincode));

                            }

                            border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0), Width = nbVariables * 20, Child = null };
                                groupesTable.Children.Add(border);
                            }
                            

                        }

                        if (Data.literal && Data.impliquantsEnAttente.Count > 0)
                        {
                            foreach (Impliquant impliquant in Data.impliquantsEnAttente)
                            {
                                groupesTable.Children.Add(generateSelectedImplicant(impliquant.bincode));
                            }

                            border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 4, 36, 4), Width = nbVariables * 16, Child = null };
                            groupesTable.Children.Add(border);
                        }

                        groupesTable.Children.RemoveAt(groupesTable.Children.Count - 1);
                        groupesMatrix.Children.Add(groupesTable);
                        
                        stepNumber = -1;
                    // fin d'affichage 
                    expandButtons.BringIntoView();


                    //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
                    for (int i = 0; i < Data.groupeMintermes.groupesImpliquants.Length; i++)
                    {
                        Data.impliquantsPremiers.AddRange(Data.groupeMintermes.groupesImpliquants[i].FindAll(impliquant => impliquant.status));
                    }

                    if (Data.literal)
                    {
                        Data.impliquants.AddRange(Data.impliquantsEnAttente.Where(m => m.nbDontCare == Data.cptGroupes).ToList()); // filtrer les impliquannts qui contient cptGroupe - 
                        Data.impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == Data.cptGroupes));// supprimer ces derniers 


                        //dans le cas ou il y'a pas d'adjacents mais la liste des impliquants en attente n'est pas vide 
                        while (Data.impliquants.Count == 0 && Data.impliquantsEnAttente.Count > 0)
                        {
                            Data.cptGroupes++;
                            Data.impliquants.AddRange(Data.impliquantsEnAttente.Where(m => m.nbDontCare == Data.cptGroupes).ToList());
                            Data.impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == Data.cptGroupes));
                        }

                    }
                    if (Data.impliquants.Count > 0)
                    {
                        Data.impliquants = Data.impliquants.Distinct().ToList();
                        Data.groupeMintermes.GrouperListes(Data.impliquants);
                        stop = false;


                         

                        groupesTable = new StackPanel() { Margin = new Thickness(10, 30, 10, 30) };
                        foreach (List<Impliquant> groupe in Data.groupeMintermes.groupesImpliquants)
                        {
                            if(groupe.Count > 0)
                            {
                                foreach (Impliquant impliquant in groupe)
                                {

                                    groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(36, 2, 36, 2), Text = impliquant.bincode });
                                }

                                border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0), Width = Data.nbVariables * 20, Child = null };
                                groupesTable.Children.Add(border);
                            }   
                            

                        }

                        groupesTable.Children.RemoveAt(groupesTable.Children.Count - 1);
                        groupesMatrix.Children.Add(groupesTable);


                    }
                    else //Sinon Arrêter la boucle
                    {
                        stop = true;
                    }

                    break;
                case -1:
                    if (!stop)
                    {
                        Data.cptGroupes++;
                        Data.impliquants = Data.groupeMintermes.generateNextGroupeImpliquants(Data.cptGroupes, Data.impliquantsEnAttente);
                        //affichage du groupe
                       

                        groupesTable = new StackPanel() { Margin = new Thickness(10, 30, 10, 30) };
                        foreach (List<Impliquant> groupe in Data.groupeMintermes.groupesImpliquants)
                        {
                            if(groupe.Count > 0)
                            {
                                foreach (Impliquant impliquant in groupe)
                                {
                                    if (impliquant.status)
                                        groupesTable.Children.Add(generatePrimeImplicant(impliquant.bincode));
                                    else groupesTable.Children.Add(generateCheckedImplicant(impliquant.bincode));

                                }

                                border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0), Width = Data.nbVariables * 20, Child = null };
                                groupesTable.Children.Add(border);
                            }
                            

                        }
                        if (Data.literal && Data.impliquantsEnAttente.Count > 0)
                        {
                            foreach (Impliquant impliquant in Data.impliquantsEnAttente)
                            {
                                groupesTable.Children.Add(generateSelectedImplicant(impliquant.bincode));
                            }

                            border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 4, 36, 4), Width = Data.nbVariables * 16, Child = null };
                            groupesTable.Children.Add(border);
                        }

                        groupesTable.Children.RemoveAt(groupesTable.Children.Count - 1);
                        groupesMatrix.Children.RemoveAt(groupesMatrix.Children.Count - 1);
                        groupesMatrix.Children.Add(groupesTable);

                        // fin d'affichage 
                        


                        //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
                        for (int i = 0; i < Data.groupeMintermes.groupesImpliquants.Length; i++)
                        {
                            Data.impliquantsPremiers.AddRange(Data.groupeMintermes.groupesImpliquants[i].FindAll(impliquant => impliquant.status));
                        }

                        if (Data.literal)
                        {
                            Data.impliquants.AddRange(Data.impliquantsEnAttente.Where(m => m.nbDontCare == Data.cptGroupes).ToList()); // filtrer les impliquannts qui contient cptGroupe - 
                            Data.impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == Data.cptGroupes));// supprimer ces derniers 


                            //dans le cas ou il y'a pas d'adjacents mais la liste des impliquants en attente n'est pas vide 
                            while (Data.impliquants.Count == 0 && Data.impliquantsEnAttente.Count > 0)
                            {
                                Data.cptGroupes++;
                                Data.impliquants.AddRange(Data.impliquantsEnAttente.Where(m => m.nbDontCare == Data.cptGroupes).ToList());
                                Data.impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == Data.cptGroupes));
                            }

                        }
                        if (Data.impliquants.Count > 0)
                        {
                            Data.impliquants = Data.impliquants.Distinct().ToList();
                            Data.groupeMintermes.GrouperListes(Data.impliquants);
                            stop = false;


                            groupesTable = new StackPanel() { Margin = new Thickness(10, 30, 10, 30) };
                            foreach (List<Impliquant> groupe in Data.groupeMintermes.groupesImpliquants)
                            {
                                if(groupe.Count > 0)
                                {
                                    foreach (Impliquant impliquant in groupe)
                                    {

                                        groupesTable.Children.Add(new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Margin = new Thickness(36, 2, 36, 2), Text = impliquant.bincode });
                                    }

                                    border = new Border() { Style = FindResource("dashedBorder") as Style, BorderThickness = new Thickness(0, 0, 0, 2), Margin = new Thickness(36, 0, 36, 0), Width = Data.nbVariables * 20, Child = null };
                                    groupesTable.Children.Add(border);
                                }
                                

                            }

                            groupesTable.Children.RemoveAt(groupesTable.Children.Count - 1);
                            groupesMatrix.Children.Add(groupesTable);


                        }
                        else //Sinon Arrêter la boucle
                        {
                            stop = true;
                            expandBottomButton.Style = FindResource("expandButton") as Style;
                            stepNumber = 4;
                        }

                    }
                    else
                    {
                        expandBottomButton.Style = FindResource("expandButton") as Style;
                        stepNumber = 4;
                    }
                    
                        
                   
                    expandButtons.BringIntoView();
                    break;
                case 4:
                    _NextStep5.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step5.xaml", UriKind.RelativeOrAbsolute));
                    groupesTableContainer.Margin = new Thickness(0, 0, 0, 82);
               
                    stepNumber++;
                    expandButtons.BringIntoView();
                    break;
                case 5:
                    _NextStep6.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step6.xaml", UriKind.RelativeOrAbsolute));
                    _NextStep5.Margin = new Thickness(0, 0, 0, 82);
                    _NextStep6.BringIntoView();
                    expandButtons.Visibility = Visibility.Collapsed;
                    stepNumber++;
                    break;
                default:
                    break;


               
            }

           

            


        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {

            _NextStep6.NavigationService.Navigate(new Uri("pack://application:,,,/Pages/Step6.xaml", UriKind.RelativeOrAbsolute));
            expandButtons.Visibility = Visibility.Collapsed;
            
        }


        private StackPanel generateCheckedImplicant(string bincode)
        {
            StackPanel result = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 2, 10, 2) };
            Viewbox viewbox = new Viewbox() { Width = 24, Margin = new Thickness(0, 0, 14, 0), Child = new Path() { Data = (Geometry)FindResource("CHECKED_ICON"), Fill = (SolidColorBrush?)new BrushConverter().ConvertFrom("#C4C4C4") } };
            TextBlock text = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Text = bincode };
            result.Children.Add(viewbox);
            result.Children.Add(text);
            return result;
        }
        private StackPanel generatePrimeImplicant(string bincode)
        {
            StackPanel result = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 2, 10, 2) };
            Grid grid = new Grid() { Width = 24, Margin = new Thickness(0, 0, 14, 0) };
            grid.Children.Add(new Ellipse() { Style = FindResource("greenDot") as Style });
            TextBlock text = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Text = bincode };

            result.Children.Add(grid);
            result.Children.Add(text);

            return result;
        }

        private StackPanel generateSelectedImplicant(string bincode)
        {
            StackPanel result = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10, 2, 10, 2) };
            Viewbox viewbox = new Viewbox() { Width = 24, Margin = new Thickness(0, 0, 14, 0), Child = new Path() { Style = FindResource("greenIcon") as Style, Data = (Geometry)FindResource("RIGHT_ARROW_ICON") , Fill =Brushes.Red } };
            TextBlock text = new TextBlock() { Style = FindResource("paragraphe") as Style, FontSize = 28, Text = bincode , Foreground = Brushes.Red };
            result.Children.Add(viewbox);
            result.Children.Add(text);
            return result;
        }

    }
}
