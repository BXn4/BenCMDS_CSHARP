using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenCMDS
{
    class blackjack
    {   
        public string id { get; set; }
        public int tet { get; set; }
        public string kartya1 { get; set; }
        public int kartya1ertek { get; set; }
        public string kartya2 { get; set; }
        public int kartya2ertek { get; set; }
        public string kartya3 { get; set; }
        public int kartya3ertek { get; set; }
        public string kartya4 { get; set; }
        public int kartya4ertek { get; set; }
        public string kartya5 { get; set; }
        public int kartya5ertek { get; set; }
        public string kartya6 { get; set; }
        public int kartya6ertek { get; set; }
        public string kartya7 { get; set; }
        public int kartya7ertek { get; set; }
        public int sajatertek { get; set; }
        public string dealer1 { get; set; }
        public int dealer1ertek { get; set; }
        public string dealer2 { get; set; }
        public int dealer2ertek { get; set; }
        public int dealerertek { get; set; }
        public int huzasokszama { get; set; }
        public blackjack(string sor)
        {
            string[] tomb = sor.Split(';');
            id = tomb[0];
            tet = int.Parse(tomb[1]);
            kartya1 = tomb[2];
            kartya1ertek = int.Parse(tomb[3]);
            kartya2 = tomb[4];
            kartya2ertek = int.Parse(tomb[5]);
            kartya3 = tomb[6];
            kartya3ertek = int.Parse(tomb[7]);
            kartya4 = tomb[8];
            kartya4ertek = int.Parse(tomb[9]);
            kartya5 = tomb[10];
            kartya5ertek = int.Parse(tomb[11]);
            kartya6 = tomb[12];
            kartya6ertek = int.Parse(tomb[13]);
            kartya7 = tomb[14];
            kartya7ertek = int.Parse(tomb[15]);
            sajatertek = int.Parse(tomb[16]);
            dealer1 = tomb[17];
            dealer1ertek = int.Parse(tomb[18]);
            dealer2 = tomb[19];
            dealer2ertek = int.Parse(tomb[20]);
            dealerertek = int.Parse(tomb[21]);
            huzasokszama = int.Parse(tomb[22]);

        }
    }
}
