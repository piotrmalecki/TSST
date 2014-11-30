using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Wezel_Sieciowy1
{
    class AgentControl
    {
        private Siec Network = null;
        private Sygnal_Text communicationT = null;
        private Komutator kom = null;
        //private List<ModelLRM> LRMList;
        private List<ModelControl> ControlList;
        private List<int> listaZajetosciLambd;
        private ModelResource zasoby = null;

        //private Thread listenLRM = null;
        private Thread listenControl = null;

        private String[] command = new String[10];
        private Boolean allow = true;

        /**
         *  LRM id_połączenia(01) zakres_lambd_f1f2(0101)
         *  Gienerał id_połączenia(01) snppA(11) snppB(12)
         * **/

        public AgentControl(Siec Network, Komutator kom)
        {
            this.Network = Network;
            this.kom = kom;
            ControlList = new List<ModelControl>(); //lista rozkazow polaczen od CC
            //LRMList = new List<ModelLRM>();         //lista rozkazow polaczen od LRM, jesli ktores sie pokryje, polaczenie jest zestawiane
            listaZajetosciLambd = new List<int>();

            ControlList.Add(new ModelControl());    //generuje zerową porządkową pozycję(na potrzeby obslugi bledow)
            init();
        }

        private void handler(String message, Boolean serv)
        {
            if ((serv) && (message != ""))
                Console.WriteLine("LRM: " + message);
            else if ((!serv) && (message != ""))
                Console.WriteLine("Gienerał: " + message);

            String[] tab = message.Split(' ');

            switch (tab[0])
            {
                case ProtokolControl.CONN:
                    CreateControl(tab);
                    break;
                case ProtokolControl.LOGIN_R:
                    command[0] = tab[0];
                    break;
                case ProtokolControl.DISCONN:
                    Disconnect(tab);
                    break;
                default:
                    if (serv)
                        Console.WriteLine("błędne polecenie od LRM.");
                    else
                        Console.WriteLine("błędne polecenie od Gienerała");
                    break;
            }
        }



        private Boolean CreateControl(String[] tab)
        {
            String id = "";
            String linkA = "";
            String linkB = "";
            String f1I = "";
            String f2I = "";
            String f1O = "";
            String f2O = "";
            ModelControl control = null;
            String snnpA="", snnpB = "";

            linkA = tab[1];
            linkB = tab[2];
            f1I = tab[3];
            f2I = tab[4];
            f1O = tab[5];
            f2O = tab[6];
            id = tab[7];

            snnpA = zasoby.GetSnnpId(linkA);
            snnpB = zasoby.GetSnnpId(linkB);
            control = new ModelControl(id, snnpA, snnpB, f1I, f2I, f1O, f2O);

            if (CheckParameters(control))
            {
                communicationT.Send_Text(ProtokolControl.CONN_R+" "+id, Network.outputControl);    
                
                Connect(control);
                ControlList.Add(control);
                
                return true;
            }
            else
            {
                communicationT.Send_Text(ProtokolControl.CONN_F+" "+id, Network.outputControl);
                return false;
            }
        }

        private Boolean CheckParameters(ModelControl control)    // sprawdza czy mamy juz 2 odpowiadajace sobie jednostki,
        {                                                        //od Gienerała i od LRM, jesli tak, zestawiamy polaczenie
            int f1, f2;                                          //generalnie zwraca je.

            Console.WriteLine("CheckParameters: szukam dla snpp: {0}", control.snnpB);
            foreach (String s in Komutator.connTable)
            {
                if ((s.Substring(0, 2).Equals(control.snnpB)) || s.Substring(2, 2).Equals(control.snnpB))
                {
                    f1 = Convert.ToInt32(s.Substring(4, 2));
                    f2 = Convert.ToInt32(s.Substring(6, 2));

                    for (int i = f1; i < f2; i++)       //wpisz zajete lambdy
                        listaZajetosciLambd.Add(i);
                }

            }
            listaZajetosciLambd.Sort(); // sortuje, żeby sens mialo dalsze poszukiwanie

            Boolean git = true;
            foreach (int s in listaZajetosciLambd)
            {
                if((s <= Convert.ToInt32(control.f1O) && s >= Convert.ToInt32(control.f1I) || (s <= Convert.ToInt32(control.f2O) && s >= Convert.ToInt32(control.f2I))))
                    git = false;    //jezeli znajdzie konflikt to false.
            }

            listaZajetosciLambd.Clear(); //czyszczenie listy, gdyz sprawdzanie odbywa sie kazdorazowo
            return git;
        }

        private void Connect(ModelControl control)
        {
            int snnpA = Convert.ToInt32(control.snnpA);
            int snnpB = Convert.ToInt32(control.snnpB);
            int f1I = Convert.ToInt32(control.f1I);
            int f1O = Convert.ToInt32(control.f1O);

            kom.SetConnect(snnpA, snnpB, f1I, f1O, ++Agent.connections);
            // jako id polaczenia podawane jest connections zabrane ze zwyklego agenta, ale zwiekszone o 1 zeby liczba polaczen sie zgadzala

            if (control.konwersja == true)
                kom.SetConversion(control.f2I, control.f2O, Agent.connections);
        }

        private void Disconnect(String[] tab)
        {
            int id = Convert.ToInt32(tab[1]);
            int snnpA,f1;
            ModelControl s = Locate(id);

            if (!s.id.Equals("0"))                            // czyli ze znalazl, to dzialaj
            {
                snnpA = Convert.ToInt32(Locate(id).snnpA);
                f1 = Convert.ToInt32(Locate(id).f1I);
                kom.Disconnect(snnpA, f1);
                communicationT.Send_Text(ProtokolControl.DISCONN_R + " " + id, Network.outputControl);  
            }
            else
                communicationT.Send_Text(ProtokolControl.DISCONN_F + " " + id, Network.outputControl);  
        }

        private ModelControl Locate(int id)
        {
            foreach (ModelControl s in ControlList)
            {
                if (id == Convert.ToInt32(id))
                    return s;
            }
            return ControlList[0];
        }

        /// <summary>
        /// //to bedzie dziedziczone i zniknie!!!!!! znaczy od tego w dol!
        /// </summary>
        /// <returns></returns>
        /// 

        private void init()
        {
            //Boolean connLRM = false;
            Boolean connControl = false;

            communicationT = new Sygnal_Text();
            zasoby = new ModelResource(Network.idW);

            for (int i = 0; i < command.Length; i++)
                command[i] = null;


            //if (Network.connectService(true))
            //{
            //    Console.WriteLine("Nawiązano połączenie z Chmurą Kablową(LRM).");
            //    connLRM = true;
            //}
            //else
            //    Console.WriteLine("\nChmura Kablowa(LRM) niedostępna.");

            if (Network.connectService(false))
            {
                Console.WriteLine("Nawiązano połączenie z Systemem Sterowania Gienerał.");
                connControl = true;
            }
            else
                Console.WriteLine("\nGienek na rybach.");

            //if (connLRM)
            //{
            //    listenLRM = new Thread(new ParameterizedThreadStart(Listen_Text));
            //    listenLRM.Start(true);
            //}

            if (connControl)
            {
                listenControl = new Thread(new ParameterizedThreadStart(Listen_Text)); //przypisanie watka
                listenControl.Start(false); //rozpoczecie nasluchiwania CC
            }

            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie

            if (connControl)
            { //jeśli jest polaczenie z Gienerałem zaloguj się do niego
                if (Loguj())    //logowanie
                {
                    Console.WriteLine("gitara. Logowanie do Gienerała zakończone sukcesem.");

                    //Console.WriteLine("Wysyłam konfigurację łącz do RC.");
                    //WyslijLacza();
                }
            }

            else //zamyka połączenia, nasłuch wyłącza program.
            {
                try
                {
                    //listenLRM.Abort();
                    listenControl.Abort();
                }
                catch (ThreadStateException e) { }
                catch (NullReferenceException e) { }

                //Network.DisconnectService(true);
                Network.DisconnectService(false);

                Console.WriteLine("\n\nWystąpiły błedy, naciśnij dowolny klawisz aby zakończyć.");
                Console.ReadKey();
                Environment.Exit(0);


            }

            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie

            Thread.Sleep(1500);
        }

        private Boolean Loguj() //petla ponownie oczekuje na confirmation, nie wysyla 
        {                                       // jeszcze raz logina
            String message = "";
            Boolean x, y;
            x = false;
            y = false;
            int i = 0;

            message = Protokol.LOGIN + " " + Convert.ToString(Network.idW);
            communicationT.Send_Text(message, Network.outputControl);

            while (!x)
            {
                Thread.Sleep(1000);

                if (command[0] == Protokol.CONF)
                {
                    Console.WriteLine("Logowanie, sukces.");
                    x = true;   //przerywa petle
                    command[0] = null;
                    y = true;
                    return (y); //konczy obsluge
                }
                else if ((x == false) && (i < 5))
                {
                    Console.WriteLine("Błąd logowania, ponawiam próbę: " + Convert.ToString(i + 1));
                    i++;
                }
                else
                {
                    Console.WriteLine("Błąd logowania. Kończę nasłuchiwać.");
                    y = false;
                    return (y);
                }

            }
            return (y);
        }

        private void Listen_Text(object service)
        {
            Boolean serv = (Boolean)service;

            while (allow == true)
            {
                String message;

                //Console.WriteLine("nasluch");
                if (serv)
                {
                    message = communicationT.Receive_Text(Network.inputCloud); //odczytuje w trybie ciaglym ze strumieni wejsciowych
                    if (message != null)
                        handler(message, true);
                    message = null; //od razu czyszczenie
                }
                else
                {
                    message = communicationT.Receive_Text(Network.inputControl);
                    if (message != null)
                        handler(message, false);

                    message = null; //od razu czyszczenie
                }
            }

        }

    }
}

////////////////////////////////////// cmentarz dobrych funkcji

//private String SelectSNP(String snppB, String band, int id)      //sprawdzanie jakie lamby sa dostepne na laczu wyjsciowym
//{                                                                //i czy starczy ich 'obok siebie' na spelnie warunkow polaczenia
//    int bandI = Convert.ToInt32(band);                           //generalnie zwraca je.
//    int ileLambd = bandI / 20;
//    int snppBI = Convert.ToInt32(snppB);
//    int f1, f2 = 0;

//    if (snppBI > 9)
//        snppB = snppB.Substring(1, 1);   //druga cyfre tylko ma wziac
//    else if (snppBI < 10)
//        snppB = "0" + snppB;

//    Console.WriteLine("SelectSNP: szukam dla snpp: {0}", snppB);
//    foreach (String s in Komutator.connTable)
//    {
//        if ((s.Substring(0, 2).Equals(snppB)) || s.Substring(2, 2).Equals(snppB))
//        {
//            f1 = Convert.ToInt32(s.Substring(4, 2));
//            f2 = Convert.ToInt32(s.Substring(6, 2));

//            for (int i = f1; i < f2; i++)       //wpisz zajete lambdy
//                listaZajetosciLambd.Add(i);
//        }

//    }

//    listaZajetosciLambd.Sort(); //posortowac!

//    f1 = 1;
//    f2 = f1 + ileLambd - 1; //przyjmujemy wartosci poczatkowe lambd i sprawdzamy czy sa git.

//    foreach (int s in listaZajetosciLambd)
//    {
//        while((s<=f2) && (s>=f1))
//        {
//            f1++;
//            f2++;
//        }
//    }

//    Console.WriteLine("wybrałem lambdy: {0}, {1}", f1, f2);
//    if (f2 > zasoby.snppList[id].band)
//    { //sprawdza czy lambda taka moze byc, przy tym algorytmie starczy
//        Console.WriteLine("zwracam 00, bo band={0}", zasoby.snppList[id].band);
//        return "00";
//    }
//    else
//    {
//        String f1S = f1.ToString(), f2S = f2.ToString();

//        if (f1 < 10)
//            f1S = f1S.Insert(0, "0");
//        if (f2 < 10)
//            f2S = f2S.Insert(0, "0");

//        return (f1S + " " + f2S);
//    }

//}

//private void CheckParameters()  // sprawdza czy mamy juz 2 odpowiadajace sobie jednostki,
//{                               //od Gienerała i od LRM, jesli tak, zestawiamy polaczenie
//    foreach (ModelControl s in ControlList)
//    {
//        foreach (ModelLRM t in LRMList)
//        {
//            if (s.id.Equals(t.id))
//            {
//                int snnpA, snnpB, f1, f2;               // ta bezsensowna konwersja ale trudno
//                snnpA = Convert.ToInt32(s.snnpA);
//                snnpB = Convert.ToInt32(s.snnpB);
//                f1 = Convert.ToInt32(t.f1);
//                f2 = Convert.ToInt32(t.f2);

//                Komutator.SetConnect(snnpA, snnpB, f1, f2, ++Agent.connections);    // jako id polaczenia podawane jest connections zabrane ze zwyklego agenta, ale zwiekszone o 1 zeby liczba polaczen sie zgadzala
//            }
//        }
//    }
//}

//private void Send_LRM(String id, String portOut, String idW, String f1, String f2)
//{
//    StringBuilder message = new StringBuilder(ProtokolControl.LRM+" ");

//    message.Append(idW+" ");
//    message.Append(portOut + " ");
//    message.Append(id+" ");
//    message.Append(f1+" ");
//    message.Append(f2);

//    communicationT.Send_Text(message.ToString(), Network.outputCloud);
//}

//private void WyslijLacza()
//{
//    Console.WriteLine("wysylanie laczy");
//    String[] zasobyString = new String[zasoby.snppList.Count];

//    Console.WriteLine("ile: " + zasoby.snppList.Count);
//    for (int i = 0; i < zasoby.snppList.Count; i++)
//    {
//        zasobyString[i] = ProtokolControl.TOPOLOGY + " " + zasoby.snppList[i].linkId + " " + zasoby.snppList[i].band.ToString() + " " + zasoby.snppList[i].length.ToString() + " " + zasoby.snppList[i].snpp_end;
//        //kolejnosc: topology linkID, band, length, snpp_end
//        Console.WriteLine(zasobyString[i]);

//        if (Network.outputControl != null)
//            communicationT.Send_Text(zasobyString[i], Network.outputControl);
//        else
//            Console.WriteLine("Brak połączenia z Gienkiem, nie wysłałem łącza.");
//    }
//}

//private void CreateLRM(String[] tab)
//{
//    String id = "";
//    String f1 = "";
//    String f2 = "";

//    id = tab[1];
//    f1 = tab[2];
//    f2 = tab[3];

//    ModelLRM lrm = new ModelLRM(id, f1, f2);

//    LRMList.Add(lrm);
//}

//private int Locate(String snnp)
//{
//    Console.WriteLine("szukam dla snpp: " + snnp);
//    for(int i=0; i<zasoby.snppList.Count; i++)
//    {
//        if(zasoby.snppList[i].snppId.ToString().Equals(snnp))
//            return i;
//    }

//    return 0;
//}