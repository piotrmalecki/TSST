using System;

namespace Wezel_Sieciowy1
{
    class ModelControl
    {
        public String id { get; private set; }
        public String snnpA { get; private set; }
        public String snnpB { get; private set; }
        public String band { get; private set; }


        public ModelControl(String id, String snnpA, String snnpB, String band)
        {
            int idI = Convert.ToInt32(id);
            int snnpAI = Convert.ToInt32(snnpA);
            int snnpBI = Convert.ToInt32(snnpB);
            int bandI = Convert.ToInt32(band);

            if (idI >= 0 && idI <= 9)
                this.id = id;
            else
                this.id = "0" + id;
            if (snnpAI >= 0 && snnpAI <= 9)
                this.snnpA = snnpA;
            else
                this.id = "0" + snnpA;
            if (snnpBI >= 0 && snnpBI <= 9)
                this.snnpB = snnpB;
            else
                this.snnpB = "0" + snnpB;
            if (bandI >= 0 && bandI <= 9)
                this.band = band;
            else
                this.band = "0" + band;
        }
    }
}
