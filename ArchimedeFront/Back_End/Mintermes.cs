using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
     public class Mintermes
    {

        // un tableau de liste<Minterme> chaque liste[i] est un groupe de mintermes avec (mitemerme.nbUns = i)
        public List<Minterme>[] groupesMintermes;
        // un tableau de liste<Impliquant> chaque liste[i] est un groupe de mintermes avec (mitemerme.nbUns = i)
        public List<Impliquant>[] groupesImpliquants;
        // nombre de groupes créés
        public int nbGroupes;

        public Mintermes(int nbGroupes)
        {
            this.groupesMintermes = new List<Minterme>[nbGroupes];
            this.nbGroupes = nbGroupes;
        }

        public Mintermes()
        { }

        //Créer les impliquants initiaux
        /// <summary>
        /// transform une List<Minterme> en List<Impliquant>
        /// </summary>

        public List<Impliquant> InitImpliquants(List<Minterme> mintermes)
        {
            List<Impliquant> result = new List<Impliquant>();
            for (int i = 0; i < mintermes.Count; i++)
            {
                result.Add(new Impliquant(mintermes[i].bincode));
            }
            return result;
        }

        /// <summary>
        /// grouper la liste des impliquants selon le nombre de 1
        /// </summary>
        public void GrouperListes(List<Impliquant> impliquants)
        {


            List<Impliquant>[] mintermeGroupes = new List<Impliquant>[nbGroupes + 1];
            mintermeGroupes[0] = new List<Impliquant>();

            for (int i = 0; i <= nbGroupes; i++)
            {

                mintermeGroupes[i] = new List<Impliquant>();


                for (int j = 0; j < impliquants.Count; j++)
                {
                    if ((impliquants[j].bincode.Count(ch => (ch == '1'))) == i)
                    {

                        impliquants[j].status = true;
                        mintermeGroupes[i].Add(impliquants[j]);
                    }
                }
            }

            this.groupesImpliquants = mintermeGroupes;
        }


        /// <summary>
        /// genere la liste des impliquants (non ordonnes) du prochain groupe
        /// </summary>
        /// <param name="cptGroupes">Numero du groupe</param>
        /// <returns>liste des impliquants non tries</returns>
        public List<Impliquant> generateNextGroupeImpliquants(int cptGroupes, List<Impliquant> impliquantsEnAttente)
        {
            int differentAt;

            List<Impliquant> result = new List<Impliquant>();

            for (int i = 0; i < groupesImpliquants.Length - 1; i++)
            {
                for (int j = 0; j < groupesImpliquants[i].Count; j++)
                {
                    //premiere boucle pour trouver les adjacents avec "groupesImpliquants[i + 1]"
                    for (int k = 0; k < groupesImpliquants[i + 1].Count; k++)
                    {

                        differentAt = groupesImpliquants[i][j].verifierAdjacence(groupesImpliquants[i + 1][k], false);
                        //Si les deux impliquants sont adjacents
                        if (differentAt >= 0)
                        {
                            //Les deux impliquants sont traités (status = false)
                            groupesImpliquants[i][j].status = false;
                            groupesImpliquants[i + 1][k].status = false;
                            //Actualiser le code binaire et le simplifier
                            StringBuilder sb = new StringBuilder(groupesImpliquants[i][j].bincode);
                            sb[differentAt] = '-';
                            //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                            result.Add(new Impliquant(sb.ToString()));
                        }
                    }


                    //deuxieme  boucle pour trouver les adjacents avec les impliquants en attente
                    for (int k = 0; k < impliquantsEnAttente.Count; k++)
                    {
                        differentAt = groupesImpliquants[i][j].verifierAdjacence(impliquantsEnAttente[k], true);

                        //Si les deux impliquants sont adjacents
                        if (differentAt >= 0 && groupesImpliquants[i][j].bincode[differentAt] != '-')
                        {
                            //Les deux impliquants sont traités (status = false)
                            groupesImpliquants[i][j].status = false;
                            //Actualiser le code binaire et le simplifier
                            StringBuilder sb = new StringBuilder(groupesImpliquants[i][j].bincode);
                            sb[differentAt] = '-';
                            //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                            result.Add(new Impliquant(sb.ToString()));
                        }

                    }
                }
            }
            int finalGroupeIndex = groupesImpliquants.Length - 1;
            for (int j = 0; j < groupesImpliquants[finalGroupeIndex].Count; j++)
            {
                for (int k = 0; k < impliquantsEnAttente.Count; k++)
                {

                    differentAt = groupesImpliquants[finalGroupeIndex][j].verifierAdjacence(impliquantsEnAttente[k], true);

                    //Si les deux impliquants sont adjacents
                    if (differentAt >= 0 && groupesImpliquants[finalGroupeIndex][j].bincode[differentAt] != '-')
                    {
                        //Les deux impliquants sont traités (status = false)
                        groupesImpliquants[finalGroupeIndex][j].status = false;


                        //Actualiser le code binaire et le simplifier
                        StringBuilder sb = new StringBuilder(groupesImpliquants[finalGroupeIndex][j].bincode);
                        sb[differentAt] = '-';

                        //Ajouter le nouveau impliquant créé à la liste des impliquants qu'il faut traiter encore
                        result.Add(new Impliquant(sb.ToString()));
                    }

                }

            }


            return result;
        }

        /// <summary>
        /// genere le prochain groupe apres la verfication d'adjacencce 
        /// </summary>
        /// <param name="literale">vrai , si on manipule une expression literale</param>
        /// <param name="cptGroupes">numero du groupe</param>
        /// <returns>Vrai si on peut encore grouper</returns>
        public bool generateNextGroupe(bool literale, int cptGroupes, List<Impliquant> impliquantsEnAttente, List<Impliquant> impliquantsPremiers)
        {

            List<Impliquant> impliquants = this.generateNextGroupeImpliquants(cptGroupes, impliquantsEnAttente);


            //Filtrer les impliquants et trouver les impliquants premiers qui ne peuvent plus etre simplifiés
            for (int i = 0; i < this.groupesImpliquants.Length; i++)
            {
                impliquantsPremiers.AddRange(this.groupesImpliquants[i].FindAll(impliquant => impliquant.status));
            }


            if (literale)
            {
                impliquants.AddRange(impliquantsEnAttente.Where(m => m.nbDontCare == cptGroupes).ToList()); // filtrer les impliquannts qui contient cptGroupe - 
                impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == cptGroupes));// supprimer ces derniers 


                //dans le cas ou il y'a pas d'adjacents mais la liste des impliquants en attente n'est pas vide 
                while (impliquants.Count == 0 && impliquantsEnAttente.Count > 0)
                {
                    cptGroupes++;
                    impliquants.AddRange(impliquantsEnAttente.Where(m => m.nbDontCare == cptGroupes).ToList());
                    impliquantsEnAttente.RemoveAll(m => (m.nbDontCare == cptGroupes));
                }

            }


            //Si la liste des impliquants qu'il faut encore traiter n'est pas vide donc regrouper les impliquants
            if (impliquants.Count > 0)
            {
                impliquants = impliquants.Distinct().ToList();
                this.GrouperListes(impliquants);
                return false;
            }
            else //Sinon Arrêter la boucle
            {
                return true;
            }

        }


    }
}
