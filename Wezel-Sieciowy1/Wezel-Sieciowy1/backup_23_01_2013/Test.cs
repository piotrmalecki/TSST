using System;
using System.Text;

namespace Wezel_Sieciowy1
{
    class Test
    {
        private String czlonek;
        private byte[] bufor = new byte[1024];
        private Siec_Init Network = null;
        //private Siec_Data data = null;


        public Test(Siec_Init Network)
        {
            czlonek = null;

            //czlonek.Insert(0, "010203");
            //czlonek[1].Insert(0, "kikakikakiaka");
            //czlonek[2].Insert(0, "balcer ma slaba glowe");
            //czlonek[3].Insert(0, "piter ma zla deserializacje");
            //czlonek[4].Insert(0, "karolina jest najladniejsza na swiecie");

            Siec_Data data = new Siec_Data(Network);

            this.Network = Network;

            //data.Send_Data(czlonek);

        }

        public void odchod()
        {
            Console.WriteLine(czlonek[0]);
            Console.WriteLine(czlonek[1]);

            

            //bufor = data.ObjectToByteArray(czlonek);
            
            Object[] wynik = new String[10];
            //wynik = (String[])data.ByteArrayToObject(bufor);

            Console.WriteLine(wynik[1]);


        }



        //public void streaming()
        //{
        //    data.Send_Data(czlonek);
        //}

    }
}
