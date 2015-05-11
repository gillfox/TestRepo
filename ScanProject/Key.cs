using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ScanProject
{
    class Key
    {
        public string HashCode { get; set; }
        public string Name { get; set; }
        

        public override bool Equals(object obj)
        {
            Key other = (Key)obj;
            return other.HashCode == this.HashCode;
        }

        public override int GetHashCode()
        {
            string str = String.Format("{0}",this.HashCode);
            return str.GetHashCode();
        }
        public override string ToString()
        {
            return String.Format("{0}",this.HashCode);
        }
    }
}
