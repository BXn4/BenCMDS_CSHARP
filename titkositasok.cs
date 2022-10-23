using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenCMDS
{
    class titkositasok
    {
        public string hash { get; set; }
        public titkositasok(string sor)
        {
            string[] tomb = sor.Split(';');
            hash = tomb[0];
        }
    }
}
