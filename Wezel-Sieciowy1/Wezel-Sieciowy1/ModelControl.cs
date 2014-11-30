using System;

namespace Wezel_Sieciowy1
{
    class ModelControl
    {
        public String id { get; private set; }
        public String snnpA { get; private set; }
        public String snnpB { get; private set; }
        public String f1I { get; private set; }
        public String f2I { get; private set; }
        public String f1O { get; private set; }
        public String f2O { get; private set; }
        public Boolean konwersja { get; private set; }

        public ModelControl(String id, String snnpA, String snnpB, String f1I, String f2I, String f1O, String f2O)
        {
            int idI = Convert.ToInt32(id);
            int snnpAI = Convert.ToInt32(snnpA);
            int snnpBI = Convert.ToInt32(snnpB);
            int f1II = Convert.ToInt32(f1I);
            int f2II = Convert.ToInt32(f2I);
            int f1OI = Convert.ToInt32(f1O);
            int f2OI = Convert.ToInt32(f2O);

            konwersja = f1I!=f2I ? true : false;    // jezeli f1 rozne od f2 to true - konwersja

            if (idI >= 0 && idI <= 9)
                this.id = "0" + id;
            else
                this.id = id;
            if (snnpAI >= 0 && snnpAI <= 9)
                this.snnpA = "0" + snnpA;
            else
                this.id = snnpA;
            if (snnpBI >= 0 && snnpBI <= 9)
                this.snnpB = "0" + snnpB;
            else
                this.snnpB = snnpB;
            if (f1II >= 0 && f1II <= 9)
                this.f1I = "0" + f1I;
            else
                this.f1I = f1I;

            if (f2II >= 0 && f2II <= 9)
                this.f2I = "0" + f2I;
            else
                this.f2I = f2I;

            if (f1OI >= 0 && f1OI <= 9)
                this.f1O = "0" + f1O;
            else
                this.f1O = f1O;

            if (f2OI >= 0 && f2OI <= 9)
                this.f2O = "0" + f2O;
            else
                this.f2O = f2O;
        }

        public ModelControl()   //przygotowuje zerową pozycję. id=zero, jej zwrócenie oznacza błąd
        {
            id = "0";
        }
    }
}
