using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Impliquant
    {
        //Liste de mintermes que l'impliquant représente
        public List<Minterme> mintermes { get; set; }
        //Code binaire de l'impliquant
        public string bincode { get; set; }
        //Staus de l'impliquant (il est à faux si l'impliquant doit encore être traité sinon c'est un impliquant essentiel)
        public bool status;

        public Impliquant(List<Minterme> mintermes)
        {
            this.mintermes = mintermes;
            this.bincode = mintermes[0].bincode;
            this.status = true;
        }
    }
}
