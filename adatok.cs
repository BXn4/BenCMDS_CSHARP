using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenCMDS
{
    class adatok
    {
        public string nev { get; set; }
        public int penz { get; set; }
        public string allapot { get; set; }
        public adatok(string sor)
        {
            string[] tomb = sor.Split(';');
            nev = tomb[0];
            penz = int.Parse(tomb[1]);
            allapot = tomb[2];
        }
    }
}
