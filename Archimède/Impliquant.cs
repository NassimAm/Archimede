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
        public bool status;




        public Impliquant(string bincode)
        {


            this.bincode = bincode;
            nbDontCare = this.bincode.Count(ch => (ch == '-'));
            status = true;
        }


        /// <summary>
        /// verifie s'il  reprsente minterme 
        /// ex : 1--- represente 1000
        /// </summary>
        public bool represente(Minterme minterme)
        {

            for (int i = 0; i < minterme.bincode.Length; i++)
            {

                if (this.bincode[i] != '-')
                {
                    if (this.bincode[i] != minterme.bincode[i]) return false;
                }

            }

            return true;


        }

        /// <summary>
        /// verifie s'il  reprsente impliquant 
        /// ex : 1--- represente 1000
        /// </summary>
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

        /// <summary>
        /// verifie s'il  reprsente bincode 
        /// ex : 1--- represente 1000
        /// </summary>
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

        /// <summary>
        /// verifier si il y'a une adjacence avec impliquant
        /// </summary>
        /// <param name="ignoreDontCare">true si on compare avec un impliquant en attente</param>
        /// <returns>la position de l'adjacence , sinon -1</returns>
        public int verifierAdjacence(Impliquant impliquant, bool ignoreDontCare)
        {
            int count = 0;
            int differentAt = -1;

            if (!ignoreDontCare)
            {
                for (int l = 0; l < this.bincode.Length; l++)
                {
                    if (this.bincode[l] != impliquant.bincode[l])
                    {
                        count += 1;
                        differentAt = l;
                    }

                    if (count > 1) return -1;

                }
            }
            else
            {
                for (int l = 0; l < this.bincode.Length; l++)
                {
                    //les tires dans ce que sont soit des 0 soit des 1  soit des x donc on peut dire qu'il n'y a pas de difference (continue pour passer a la prochaine iteration )
                    if ((impliquant.bincode[l] == '-')) continue;


                    if (this.bincode[l] != impliquant.bincode[l])
                    {
                        count += 1;
                        differentAt = l;
                    }

                    if (count > 1) return -1;

                }
            }

            if (count == 1) return differentAt;
            return -1;


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
