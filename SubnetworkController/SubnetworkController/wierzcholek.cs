using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubnetworkController
{
   class Wierzcholek
    {
        public List<Sasiad> sasiedzi { get; set; }
        public String id { get; set; }
        public int d {get; set;}
        public String pop { get; set; }


        public Wierzcholek(String id, List<Sasiad> sasiedzi)
        {
            this.id = id;
            this.sasiedzi = sasiedzi;
            d = 1000;
            pop = "0";
           // Console.WriteLine("Wierzholek: id=" + id);
          
        }

    }
}
