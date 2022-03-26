using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimède
{
    class Minterme: IEquatable<Minterme>
    {
        //Nombre décimal représentant le minterme
        public int nombre { get; set;}
        //Représentation binaire du minterme
        public string bincode { get; set;}
        //Nombre de 1 dans la représentation binaire
        public int nbuns { get; set;}

        public Minterme(int nombre)
        {
            this.nombre = nombre;
            this.bincode = ConvertionAuBinaire(nombre);
            this.nbuns = NombreDeUns(this.bincode);
        }

        //Compte le nombre de 1 dans un code binaire
        private int NombreDeUns(string codebinaire)
        {
            return codebinaire.Count(ch => (ch == '1'));
        }

        //Convertit un nombre en code binaire
        private string ConvertionAuBinaire(int nombre)
        {
            return Convert.ToString(nombre, 2);
        }
        
        public bool Equals(Minterme other)
        {
            if(ReferenceEquals(other, null)) return false;
            if(ReferenceEquals(this, other)) return true;
            return int.Equals(nombre, other.nombre);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Minterme)obj);
        }

        /*public override int GetHashCode()
        {
            return (nombre != null ? nombre.GetHashCode() : 0);
        }*/

    }
}
