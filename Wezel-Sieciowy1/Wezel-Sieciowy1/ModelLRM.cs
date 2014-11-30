using System;

namespace Wezel_Sieciowy1
{
    class ModelLRM
    {
        public String id { get; private set; }
        public String f1 { get; private set; }
        public String f2 { get; private set; }

        public ModelLRM(String id, String f1, String f2)
        {
            int idI = Convert.ToInt32(id);
            int f1I = Convert.ToInt32(f1);
            int f2I = Convert.ToInt32(f2);

            if (idI >= 0 && idI <= 9)
                this.id = id;
            else
                this.id = "0" + id;
            if (f1I >= 0 && f1I <= 9)
                this.f1 = f1;
            else
                this.id = "0" + f1;
            if (f1I >= 0 && f1I <= 9)
                this.f2 = f2;
            else
                this.f2 = "0" + f2;
        }

        enum cyfry
        {
            gowno,
            sraczka
        

        }

    }
}
