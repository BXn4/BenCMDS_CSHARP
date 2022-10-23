using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenCMDS
{
    class covid
    {
        public class data
        {
            public int deaths { get; set; }
            public int confirmed { get; set; }
        }
        public class root
        {
            public data data { get; set; }
        }
    }
}
