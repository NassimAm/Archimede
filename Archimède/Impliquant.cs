using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Impliquant
    {
        public List<Minterme> mintermes { get; set; }
        public string bincode { get; set; }
        public bool status;

        public Impliquant(List<Minterme> mintermes)
        {
            this.mintermes = mintermes;
            this.bincode = mintermes[0].bincode;
            this.status = true;
        }
    }
}
