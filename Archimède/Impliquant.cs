using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Impliquant : IEquatable<Impliquant>
    {

        //Code binaire de l'impliquant
        public string bincode { get; set; } 

        public int nbDontCare { get; set; } 


        //Staus de l'impliquant (il est à faux si l'impliquant doit encore être traité sinon c'est un impliquant essentiel)
        public bool status ; 




        public Impliquant(string bincode) {
            
           
            this.bincode = bincode;
            nbDontCare = this.bincode.Count(ch => (ch == '-'));

            status =  nbDontCare <= 0 ;   
        }

        

        public  bool represente(Minterme minterme) {

            for (int i = 0; i < minterme.bincode.Length; i++ ) {
               
                if (this.bincode[i] != '-') {
                    if (this.bincode[i] != minterme.bincode[i]) return false;
                }
            
            }

            return true;
        
        
        }

       
        public bool represente(Impliquant impliquant)
        {
            for (int i = 0; i < impliquant.bincode.Length; i++)
            {

                if (this.bincode[i] != '-')
                {
                    if (this.bincode[i] != impliquant.bincode[i]) return false;
                }

            }
            return true;
        }


        public bool represente(String bincode)
        {
            for (int i = 0; i < bincode.Length; i++)
            {

                if (this.bincode[i] != '-')
                {
                    if (this.bincode[i] != bincode[i]) return false;
                }

            }
            return true;
        }


        public bool Equals(Impliquant other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            return bincode.SequenceEqual(other.bincode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Impliquant)obj);
        }
        

        public override int GetHashCode()
        {
            return (bincode != null ? bincode.GetHashCode() : 0);
        }
    }
}
