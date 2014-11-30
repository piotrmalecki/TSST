using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubnetworkController
{
    class Sasiad
    {
        public String id { get; set; }
        public int waga { get; set; }
        public int lacze { get; set; }
        public String snpp { get; set; }
        public String snppEnd { get; set; }


        public Sasiad(String id, int lacze, int band, String snpp, String snppEnd)
        {
            this.id = id;
            this.lacze = lacze;
            this.snpp = snpp;
            this.snppEnd = snppEnd;

            float d = 100 * (1 / (float)band);
            waga = (int)Math.Round(d);


        }


    }
}
