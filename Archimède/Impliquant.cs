using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Impliquant
    {
        public Minterme minterme { get; set; }
        public string bincode { get; set; }
        public bool status;

        public Impliquant(Minterme minterme)
        {
            this.minterme = minterme;
            this.bincode = minterme.bincode;
            this.status = true;
        }
    }
}
