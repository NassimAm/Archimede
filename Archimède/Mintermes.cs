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
        public List<Minterme>[] groupesMintermes ;
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
        public List<Impliquant> InitImpliquants(List<Minterme> mintermes)
        {
            List<Impliquant> result = new List<Impliquant>();
            for(int i = 0;i<mintermes.Count;i++)
            {
                result.Add(new Impliquant( mintermes[i].bincode ));
            }
            return result;
        }


        public void GrouperListes(List<Impliquant> impliquants , int cptGroupe)
        {
         

            List<Impliquant>[] mintermeGroupes = new List<Impliquant>[nbGroupes + 1];
            mintermeGroupes[0] = new List<Impliquant>();

            for (int i = 0; i <= nbGroupes; i++)
            {
                
                mintermeGroupes[i] = new List<Impliquant>();

                for (int j = 0; j < impliquants.Count; j++)
                {
                    if ((impliquants[j].bincode.Count(ch => (ch == '1')||(ch == '-'))   - cptGroupe ) == i)
                    {
                        if(impliquants[j].nbDontCare == cptGroupe ) impliquants[j].status = true;
                        else impliquants[j].status = false;
                        mintermeGroupes[i].Add(impliquants[j]);
                    }
                }
            }

            this.groupesImpliquants = mintermeGroupes;
        }


    }
}
