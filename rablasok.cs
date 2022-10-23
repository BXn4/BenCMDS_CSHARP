using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenCMDS
{
    class rablasok
    {
        public string kitolrabol { get; set; }
        public string jelszo { get; set; }
        public int probalkozasok { get; set; }
        public rablasok(string sor)
        {
            string[] tomb = sor.Split(';');
            kitolrabol = tomb[0];
            jelszo = tomb[1];
            probalkozasok = int.Parse(tomb[2]);
        }
    }
}
