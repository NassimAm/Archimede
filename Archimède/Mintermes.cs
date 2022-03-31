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
            List<Minterme> result = new List<Minterme>();
            while(mintermes.Count>0)
            {
                long min = mintermes[0].nombre;
                int min_index = 0;
                for(int i=0;i<mintermes.Count;i++)
                {
                    if(mintermes[i].nombre<min)
                    {
                        min = mintermes[i].nombre;
                        min_index = i;
                    }
                }
                result.Add(mintermes[min_index]);
                mintermes.RemoveAt(min_index);
            }
            return result;
        }

        //Ordonne la liste d'impliquants en fonction du nombre décimal et retourne la nouvelle liste ordonnée
        public List<Impliquant> OrdonnerListe(List<Impliquant> impliquants)
        {
            List<Impliquant> result = new List<Impliquant>();
            while (impliquants.Count > 0)
            {
                long min = impliquants[0].mintermes[0].nombre;
                int min_index = 0;
                for (int i = 0; i < impliquants.Count; i++)
                {
                    if (impliquants[i].mintermes[0].nombre < min)
                    {
                        min = impliquants[i].mintermes[0].nombre;
                        min_index = i;
                    }
                }
                result.Add(impliquants[min_index]);
                impliquants.RemoveAt(min_index);
            }
            return result;
        }

    }
}
