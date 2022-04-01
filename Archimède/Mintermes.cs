using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Mintermes
    {
        // un tableau de liste<Minterme> chaque liste[i] est un groupe de mintermes avec (mitemerme.nbUns = i)
        public List<Minterme>[] groupesMintermes;
        // un tableau de liste<Impliquant> chaque liste[i] est un groupe de mintermes avec (mitemerme.nbUns = i)
        public List<Impliquant>[] groupesImpliquants;
        // nombre de groupes créés
        private int nbGroupes;

        public Mintermes(int nbGroupes)
        {
            this.groupesMintermes = new List<Minterme>[nbGroupes];
            this.nbGroupes = nbGroupes;
        }

        public Mintermes()
        { }

        //Créer les impliquants initiaux
        public List<Impliquant> InitImpliquants(List<Minterme> mintermes)
        {
            List<Impliquant> result = new List<Impliquant>();
            for(int i = 0;i<mintermes.Count;i++)
            {
                result.Add(new Impliquant(new List<Minterme>() { mintermes[i] }));
            }
            return result;
        }

        /*//Grouper les mintermes binaire en groupe d'après le nombre de 1
        public void GrouperListes(List<Minterme> mintermes)
        {
            mintermes = OrdonnerListe(mintermes);
            int maxnbGroupes = this.nbGroupes;
            List<Minterme>[] mintermeGroupes = new List<Minterme>[maxnbGroupes + 1];
            mintermeGroupes[0] = new List<Minterme>();
            for (int i = 0; i < mintermes.Count; i++)
            {
                if (mintermes[i].nbuns == 0)
                {
                    mintermeGroupes[0].Add(mintermes[i]);
                }
            }
            for (int i = 1; i <= maxnbGroupes; i++)
            {
                mintermeGroupes[i] = new List<Minterme>();
                for (int j = 0; j < mintermes.Count; j++)
                {
                    if (mintermes[j].nbuns == i)
                    {
                        mintermeGroupes[i].Add(mintermes[j]);
                    }
                }
            }

            this.groupesMintermes = mintermeGroupes;
        }*/

        //Grouper les mintermes binaire en groupe d'après le nombre de 1
        public void GrouperListes(List<Minterme> mintermes)
        {
            mintermes = OrdonnerListe(mintermes);
            List<Impliquant> impliquants = InitImpliquants(mintermes);
            int maxnbGroupes = this.nbGroupes;
            List<Impliquant>[] mintermeGroupes = new List<Impliquant>[maxnbGroupes + 1];
            mintermeGroupes[0] = new List<Impliquant>();
            for (int i = 0; i < impliquants.Count; i++)
            {
                if (impliquants[i].mintermes[0].nbuns == 0)
                {
                    mintermeGroupes[0].Add(impliquants[i]);
                }
            }
            for (int i = 1; i <= maxnbGroupes; i++)
            {
                mintermeGroupes[i] = new List<Impliquant>();
                for (int j = 0; j < impliquants.Count; j++)
                {
                    if (impliquants[j].mintermes[0].nbuns == i)
                    {
                        mintermeGroupes[i].Add(impliquants[j]);
                    }
                }
            }

            this.groupesImpliquants = mintermeGroupes;
        }

        public void GrouperListes(List<Impliquant> impliquants)
        {
            impliquants = OrdonnerListe(impliquants);
            int maxnbGroupes = this.nbGroupes;
            List<Impliquant>[] mintermeGroupes = new List<Impliquant>[maxnbGroupes + 1];
            mintermeGroupes[0] = new List<Impliquant>();
            for (int i = 0; i < impliquants.Count; i++)
            {
                if (impliquants[i].mintermes[0].bincode.Count(ch => (ch == '1')) == 0)
                {
                    impliquants[i].status = true;
                    mintermeGroupes[0].Add(impliquants[i]);
                }
            }
            for (int i = 1; i <= maxnbGroupes; i++)
            {
                mintermeGroupes[i] = new List<Impliquant>();
                for (int j = 0; j < impliquants.Count; j++)
                {
                    if (impliquants[j].mintermes[0].bincode.Count(ch => (ch == '1')) == i)
                    {
                        impliquants[j].status = true;
                        mintermeGroupes[i].Add(impliquants[j]);
                    }
                }
            }

            this.groupesImpliquants = mintermeGroupes;
        }

        //Ordonne la liste de mintermes en fonction du nombre décimal et retourne la nouvelle liste ordonnée
        public List<Minterme> OrdonnerListe(List<Minterme> mintermes)
        {

           
            mintermes.Sort( 
                 delegate(Minterme m1 , Minterme m2) {
                    if(m1.nombre < 0 ||  m2.nombre < 0  )
                        return string.Compare(m1.nombreChaine, m2.nombreChaine);
                    else
                        return (m1.nombre).CompareTo(m2.nombre);
                 }
            );

            return mintermes;
           
           
        }

   

        //Ordonne la liste d'impliquants en fonction du nombre décimal et retourne la nouvelle liste ordonnée
        public List<Impliquant> OrdonnerListe(List<Impliquant> impliquants)
        {
            impliquants.Sort(
                delegate (Impliquant m1, Impliquant m2) {
                    if (m1.mintermes[0].nombre < 0 || m2.mintermes[0].nombre < 0)
                        return string.Compare(m1.mintermes[0].nombreChaine, m2.mintermes[0].nombreChaine);
                    else
                        return (m1.mintermes[0].nombre).CompareTo(m2.mintermes[0].nombre);
                }
           );

            return impliquants;
        }

    }
}
