using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections.Generic;

/**
 * nagłówek wiadomości:
 * 2 znaki port wezla   +                           0,1
 * 2 znaki nr wezla     +                           2,3
 * 2 znaki f1           + numer pierwszej lambdy    4,5
 * 2 znaki f2           + numer ostatniej lambdy    6,7
 * 2 znaki nr klienta                               8,9
 * 
 * wiadomosc + kilka zer wynikająch z modulacji
 * 
 * **/

/**
 * connTable:
 * 2 znaki - portIn         0,1
 * 2 znaki - portOut        2,3
 * 2 znaki - f1             4,5
 * 2 znaki - f2             6,7
 * 
 * opcjonalnie:
 * 2 znaki - nowe f1        8,9
 * 2 znaki - nowe f2        10,11
 * 
 * **/


namespace Wezel_Sieciowy1
{
    public class Komutator
    {
        private int idW;                            //  id węzła
        private static Boolean connected;                  //  cos jak start/stop

        public static String[] connTable = null;          //  tablica kierowania, pamietac ze indeks to id (polaczenia-1)
        private String[] conversionTable = null;    //  tablica konwersji, indeks j/w
 
        public Thread krosowanieT;                  //  wątek odpowiedzialny za forwarding danych

        private Siec_Init Network;                  //  standardowy obiekt komunikacyjny


        public Komutator(Siec_Init Network)
        {
            this.Network = Network;
            this.idW = Network.idW;

            connTable = new String[5];                  //inicjalizacja tablicy kierowania
            for (int i = 0; i < connTable.Length; i++)
                connTable[i] = "000000000000";

            conversionTable = new String[5];
            for (int i = 0; i < conversionTable.Length; i++)
                conversionTable[i] = "000000000000";

            connected = false;

            krosowanieT = new Thread(Krosuj); //wątek odpowiedzialny za forwarding danych
            krosowanieT.Start();


        }


        private void Krosuj()
        {
            Siec_Data data = new Siec_Data(Network);
            String pakiet = null;
            int portOut = 0;
            int deltaLambda = 0;
            String[] header = new String[5];
            

            while (true)                                    // oczekiwanie na możliwość rozpoczęcia(na true od agenta)
            {
                Thread.Sleep(500);
                if (connected)
                    break;
            }

            while (connected)
            {
                for (int i = 0; i < header.Length; i++)             // zerowanie tablicy headera
                    header[i] = "00";

                Console.WriteLine("\nOdczyt danych.");              // wlasciwy odczyt danych
                pakiet = data.Receive_Data()[0];
                header = ExtractHeader(pakiet);                     // odczyt naglowka

                foreach (String s in connTable) //przeszukiwanie tablicy kierowania
                {
                    Console.WriteLine("wejscie: " + pakiet.Substring(0, 10) + " a connTable: " + s); //umozliwia wzrokowe porownanie naglowka tego co przyszlo z connTable
                    if ((s.Substring(0, 2).Equals(header[0]) && (s.Substring(4, 2).Equals(header[2]))))
                    //jesli zgodne portIn i f1, czyli polaczenie odnalezione, ustaw portOut(ostatnia linijka)
                    {
                        Console.WriteLine("połączenie odnalezione");
                        deltaLambda = 0;

                        if (!s.Substring(8, 2).Equals("00"))             //jesli pole nowe f1 niepuste wykonaj konwersje lambd
                            deltaLambda = Convert.ToInt32(s.Substring(8, 2)) - Convert.ToInt32(s.Substring(4, 2)); // pozyskaj przesuniecie czestotliwosci 

                        portOut = Convert.ToInt32(s.Substring(3, 1));  // jesli odnajdziesz portIn z naglowka w tabeli polaczen, uzyj go
                        //Console.WriteLine("ustawilem portout na: " + portOut);

                    }

                }


                if (portOut != 0)               //  zabezpieczenie przed wysylaniem do chmury ślepych pakietów, z portOut=0
                {
                    String [] pakietTab = new String[1];
                    pakietTab[0] = pakiet;
                    data.Send_Data(ReplaceHeader(pakietTab, header, portOut, deltaLambda));
                }
                portOut = 0;
            }

        }



        private String[] ExtractHeader(String pakiet)
        {
            String[] header = new String[5];

            header[0] = pakiet.Substring(0, 2);    // port wezla, wejsciowy (pozniej zamieniany na Out)
            header[1] = pakiet.Substring(2, 2);    // nr wezla - idW
            header[2] = pakiet.Substring(4, 2);    // f1 numer pierwszej lambdy
            header[3] = pakiet.Substring(6, 2);    // f2 numer ostatniej lambdy
            header[4] = pakiet.Substring(8, 2);    // id klienta - nieuzywane

            //Console.WriteLine("port: "+header[0]);
            //Console.WriteLine("idW: " + header[0]);
            //Console.WriteLine("f1: "+header[2]);
            //Console.WriteLine("f2: "+header[3]);
            //Console.WriteLine("idKlienta: "+header[4]);

            return header;
        }

        private String[] ReplaceHeader(String[] pakiet, String[] header, int portOut, int deltaLambda)
        {
            //StringBuilder header = new StringBuilder(pakiet);
            //header[1] = Convert.ToChar(idW.ToString());         //podmienia id wezla na bieżące
            //header[3] = Convert.ToChar(portOut.ToString());     //podmienia port wejsciowy na wyjsciowy

            //pakiet=header.ToString();
            //return pakiet;

            header[0] = portOut.ToString();
            header[1] = idW.ToString();
            header[2] = (Convert.ToInt32(header[2]) + deltaLambda).ToString();
            header[3] = (Convert.ToInt32(header[3]) + deltaLambda).ToString();

            if (portOut < 10)
                header[0] = header[0].Insert(0, "0");
            if (idW < 10)
                header[1] = header[1].Insert(0, "0");
            if (Convert.ToInt32(header[2]) < 10)
                header[2] = header[2].Insert(0, "0");
            if (Convert.ToInt32(header[3]) < 10)
                header[3] = header[3].Insert(0, "0");


            StringBuilder newHeader = new StringBuilder(header[0]); //stringbuilder a nie string bo szybciej lepiej i wygodniej.
            newHeader.Append(header[1]).Append(header[2]).Append(header[3]).Append(header[4]);

            Console.WriteLine(newHeader.ToString());

            //pakiet.Remove(0, 10);
            //pakiet.Insert(0, newHeader.ToString());

            pakiet[0] = pakiet[0].Remove(0, 10).Insert(0, newHeader.ToString());
            return pakiet;
        }


        public static void SetConnect(int portIn, int portOut, int f1, int f2, int idP)        //dopisuje regule do tablicy kierowania connTable
        {
            //pierwsze jej wywolanie uruchamia komutator(connected=true)
            String portInS="",portOutS="",f1S="",f2S="";

            if (portIn > 9)
                portInS = "0" + portIn.ToString()[1];
            if (portOut > 9)
                portOutS = "0" + portOut.ToString()[1];
            if (f1 > 9)
                f1S = "0" + f1.ToString()[1];
            if (f2 > 9)
                f2S = "0" + f2.ToString()[1];

            if (portIn < 10)                                //dopelnianie zerem jesli mniejsze
                portInS = "0" + portIn.ToString();
            if (portOut < 10)
                portOutS = "0" + portOut.ToString();
            if (f1 < 10)
                f1S = "0" + f1.ToString();
            if (f2 < 10)
                f2S = "0" + f2.ToString();

            connTable[(idP - 1)] = (portInS + portOutS + f1S + f2S + "0000");
            connected = true;
            //Console.WriteLine(connected);
        }


        public void Disconnect(int portIn, int portOut, int f1) //po poleceniu disconnect od agenta(od SZ) zeruje dany wiersz connTable
        {
            int idP = 0;

            String f1S, portInS;

            portInS = portIn.ToString();
            f1S = f1.ToString();

            if (portIn < 10)
                portInS = portInS.Insert(0, "0");
            if (f1 < 10)
                f1S = f1S.Insert(0, "0");

            for (int i = 0; i < connTable.Length; i++)
                //if (connTable[i].Substring(0, 1).Equals(portIn.ToString()))
                if ((connTable[i].Substring(0, 2).Equals(portInS) && (connTable[i].Substring(4, 2).Equals(f1S))))
                    idP = i;
            // napisane w ten sposob, a nie bezposrednio connTable[i]="00", aby zachowac jednolity sposob obslugi

            connTable[idP] = "000000000000";
        }

        public void SetConversion(String f1_new, String f2_new, int idP)
        {
            connTable[idP - 1] = connTable[idP - 1].Insert(8, f1_new);   //wsadzaj te f1 i f2, zera sie raczej przesuna w prawo, ale to bez znaczenia.
            connTable[idP - 1] = connTable[idP - 1].Insert(10, f2_new);

        }
    }
}
