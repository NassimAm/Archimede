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
        // nombre de groupes créés
        private int nbGroupes;

        public Mintermes(int nbGroupes)
        {
            this.groupesMintermes = new List<Minterme>[nbGroupes];
            this.nbGroupes = nbGroupes;
        }

        public Mintermes()
        { }

        //Grouper les mintermes binaire en groupe d'après le nombre de 1
        public void GrouperListes(List<Minterme> mintermes)
        {
            mintermes = OrdonnerListe(mintermes);
            int maxnbGroupes = this.nbGroupes;
            List<Minterme>[] mintermeGroupes = new List<Minterme>[maxnbGroupes + 1];
            mintermeGroupes[0] = new List<Minterme>();
            for(int i = 0;i<mintermes.Count;i++)
            {
                if(mintermes[i].nbuns == 0)
                {
                    mintermeGroupes[0].Add(mintermes[i]);
                }
            }
            for(int i = 1;i<=maxnbGroupes;i++)
            {
                mintermeGroupes[i] = new List<Minterme>();
                for(int j = 0;j<mintermes.Count;j++)
                {
                    if(mintermes[j].nbuns == i)
                    {
                        mintermeGroupes[i].Add(mintermes[j]);
                    }
                }
            }

            this.groupesMintermes = mintermeGroupes;
        }

        public void GrouperListes(List<Impliquant> impliquants)
        {
            List<Minterme> mintermes = new List<Minterme>();
            for(int i=0;i<impliquants.Count;i++)
            {
                Minterme minterme = impliquants[i].mintermes[0];
                minterme.bincode = impliquants[i].bincode;
                mintermes.Add(minterme);
            }
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
        }

        //Ordonne la liste de mintermes en fonction du nombre décimal et retourne la nouvelle liste ordonnée
        public List<Minterme> OrdonnerListe(List<Minterme> mintermes)
        {
            List<Minterme> result = new List<Minterme>();
            while(mintermes.Count>0)
            {
                int min = mintermes[0].nombre;
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

    }
}
