using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace Wezel_Sieciowy1
{
    class AgentControl
    {
        private SiecControl Network = null;
        private Sygnal_Text communicationT = null;
        private List<ModelLRM> LRMList;
        private List<ModelControl> ControlList;
        private ModelResource zasoby = null;

        private Thread listenLRM = null;
        private Thread listenControl = null;

        private String[] command = new String[10];
        private Boolean allow = true;

        /**
         *  LRM id_połączenia(01) zakres_lambd_f1f2(0101)
         *  Gienerał id_połączenia(01) snppA(11) snppB(12)
         * **/



        public AgentControl(SiecControl Network)
        {
            this.Network = Network;
            init();
        }


        private void init()
        {
            Boolean connLRM = false;
            Boolean connControl = false;

            communicationT = new Sygnal_Text();
            zasoby = new ModelResource(Network.idW);

            for (int i = 0; i < command.Length; i++)
                command[i] = null;


            if (Network.connectService(true))
            {
                Console.WriteLine("Nawiązano połączenie z Chmurą Kablową(LRM).");
                connLRM = true;
            }
            else
                Console.WriteLine("\nChmura Kablowa(LRM) niedostępna.");

            //if (Network.connectService(false))
            //{
            //    Console.WriteLine("Nawiązano połączenie z Systemem Sterowania Gienerał.");
            //    connControl = true;
            //}
            //else
            //    Console.WriteLine("\nGienek na rybach.");

            if (connLRM)
            {
                listenLRM = new Thread(new ParameterizedThreadStart(Listen_Text));
                listenLRM.Start(true);
            }

            if (connControl)
            {
                listenControl = new Thread(new ParameterizedThreadStart(Listen_Text)); //przypisanie watka
                listenControl.Start(false); //rozpoczecie nasluchiwania CC
            }

            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie

            //if (connControl)
            //{ //jeśli jest polaczenie z Gienerałem zaloguj się do niego
            //    if (Loguj())    //logowanie
            //    {
            //        Console.WriteLine("gitara. Logowanie do Gienerała zakończone sukcesem.");
            //        Console.WriteLine("Wysyłam konfigurację łącz do RC.");

            //        WyslijLacza();
            //    }
            //}

            //else //zamyka połączenia, nasłuch wyłącza program.
            //{
            //    try
            //    {
            //        listenLRM.Abort();
            //        listenControl.Abort();
            //    }
            //    catch (ThreadStateException e) { }
            //    catch (NullReferenceException e) { }

            //    Network.DisconnectService(true);
            //    Network.DisconnectService(false);

            //    Console.WriteLine("\n\nWystąpiły błedy, naciśnij dowolny klawisz aby zakończyć.");
            //    Console.ReadKey();
            //    Environment.Exit(0);


            //}

            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie

            Thread.Sleep(1500);
        }

        private void WyslijLacza()
        {
            String[] zasobyString = new String[zasoby.snppList.Count];
            for (int i = 0; i < zasoby.snppList.Count; i++)
            {
                zasobyString[i] = "id:"+zasoby.snppList[i].linkId+" band:"+zasoby.snppList[i].band.ToString()+" length:"+zasoby.snppList[i].length.ToString()+"snpp_end:"+zasoby.snppList[i].snpp_end;
            }

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
                    message = communicationT.Receive_Text(Network.inputLRM); //odczytuje w trybie ciaglym ze strumieni wejsciowych
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

        private void handler(String message, Boolean serv)
        {
            StringBuilder word = new StringBuilder();       //do splitowania ladnego na literki i cyferki
            StringBuilder digits = new StringBuilder();
            String digitsT = null;

            if ((serv) && (message != ""))
                Console.WriteLine("LRM: " + message);
            else if ((!serv) && (message != ""))
                Console.WriteLine("Gienerał: " + message);


            foreach (String s in Regex.Split(message, "[0-9 ' ' ^ $]")) //jesli wykryje litery, wywalone tez spacje
            {
                word.Append(s);
            }

            foreach (String s in Regex.Split(message, "[a-z_ ' ' ^ $]")) //jesli cyfry i znaki _ , wywalone tez spacje
            {
                digits.Append(s);
            }


            message = word.ToString();
            digitsT = digits.ToString();

            //Console.WriteLine("litery: "+message);
            //Console.WriteLine("cyfry: "+digitsT);

            switch (message)
            {
                case ProtokolControl.LRM:
                    CreateLRM(digitsT);
                    CheckParameters();
                    break;
                case ProtokolControl.CONN:
                    if (CreateControl(digitsT))
                    {
                        CheckParameters();
                        communicationT.Send_Text(ProtokolControl.CONN_R, Network.outputControl);
                    }
                    else
                        communicationT.Send_Text(ProtokolControl.CONN_F, Network.outputControl);
                    
                    break;
                case ProtokolControl.LOGIN_R:
                    command[0] = message;
                    break;
                default:
                    if (serv)
                        Console.WriteLine("błędne polecenie od LRM.");
                    else
                        Console.WriteLine("błędne polecenie od Gienerała");
                    break;
            }


        }



        private void CreateLRM(String digitsT)
        {
            String id = "";
            String f1 = "";
            String f2 = "";

            id = digitsT.Substring(0, 2);
            f1 = digitsT.Substring(2, 2);
            f2 = digitsT.Substring(4, 2);

            ModelLRM lrm = new ModelLRM(id, f1, f2);

            LRMList.Add(lrm);
        }

        private Boolean CreateControl(String digitsT)
        {
            String id = "";
            String snppA = "";
            String snppB = "";
            String band = "";
            ModelControl control = null;

            id = digitsT.Substring(0, 2);
            snppA = digitsT.Substring(2, 2);
            snppB = digitsT.Substring(4, 2);
            band = digitsT.Substring(6, 2);

            if(!SelectSNP(snppB, band).Equals("00")) 
            {
                control = new ModelControl(id, snppA, snppB, band);
                ControlList.Add(control);
                return true;
            }
            return false;
            
        }

        private String SelectSNP(String snppB, String band)    //sprawdzanie jakie lamby sa dostepne na laczu wyjsciowym
        {                                                       //i czy starczy ich 'obok siebie' na spelnie warunkow polaczenia
            int bandI = Convert.ToInt32(band);                  //generalnie zwraca je.
            int ileLambd = bandI / 200;
            int snppBI = Convert.ToInt32(snppB);
            int f1, f2 = 0;
            List<int> listaZajetosciLambd = new List<int>();
            int count = 0;

            if (snppBI > 9)
                snppB = snppB.Substring(1, 1);   //druga cyfre tylko ma wziac
            else if (snppBI < 10)
                snppB = "0" + snppB;

            foreach (String s in Komutator.connTable)
            {
                if ((s.Substring(0, 2).Equals(snppB)) || s.Substring(2, 2).Equals(snppB))
                {
                    f1 = Convert.ToInt32(s.Substring(4, 2));
                    f2 = Convert.ToInt32(s.Substring(6, 2));

                    for (int i = f1; i < f2; i++)       //wpisz zajete lambdy
                        listaZajetosciLambd.Add(i);
                }

            }

            listaZajetosciLambd.Sort(); //posortowac!

            f1 = f2 = 0; //ponowne uzycie zmiennych
            Boolean p, k;
            p = k = false;
            for (int i = 1; i < zasoby.snppList.Count; i++)
            {
                foreach (int s in listaZajetosciLambd)
                {
                    if ((i != s) && (p != true))              //jesli zmiennej nie ma na liscie, i nie masz jeszcze poczatku, zapisz, szukaj dalej konca
                    {
                        f1 = i;
                        p = true;                             //mam poczatek!
                    }

                    if (p == true && count < ileLambd)        // jesli przedzial nadal za krotki, kontynuuj
                    {
                        f2 = i;
                        count++;
                    }

                    if (p == true && count == ileLambd)
                    {
                        f2 = i;
                        k = true;
                        break;
                    }

                    if (i == s)                          //jesli natrafiles po drodze na zajete, zeruj
                    {
                        f1 = 0;
                        f2 = 0;
                        p = false;
                        k = false;
                    }
                }

                if (p == true && k == true)
                {
                    StringBuilder snp = new StringBuilder();

                    if (f1 < 10)
                        snp.Append("0" + f1);
                    else if (f1 > 9)
                        snp.Append(f1);

                    if (f2 < 10)
                        snp.Append("0" + f2);
                    else if (f2 > 9)
                        snp.Append(f2);

                    return snp.ToString();
                }

                else
                    return "00";
            }

            return "00";
        }

        private void CheckParameters()  // sprawdza czy mamy juz 2 odpowiadajace sobie jednostki,
        {                               //od Gienerała i od LRM, jesli tak, zestawiamy polaczenie
            foreach (ModelControl s in ControlList)
            {
                foreach (ModelLRM t in LRMList)
                {
                    if (s.id.Equals(t.id))
                    {
                        int snnpA, snnpB, f1, f2;               // ta bezsensowna konwersja ale trudno
                        snnpA = Convert.ToInt32(s.snnpA);
                        snnpB = Convert.ToInt32(s.snnpB);
                        f1 = Convert.ToInt32(t.f1);
                        f2 = Convert.ToInt32(t.f2);

                        Komutator.SetConnect(snnpA, snnpB, f1, f2, ++Agent.connections);    // jako id polaczenia podawane jest connections zabrane ze zwyklego agenta, ale zwiekszone o 1 zeby liczba polaczen sie zgadzala
                    }
                }
            }
        }

        private void Send_LRM(String id, String idW, String f1, String f2)
        {
            StringBuilder message = new StringBuilder();

            message.Append(idW);
            message.Append(id);
            message.Append(f1);
            message.Append(f2);

            communicationT.Send_Text(message.ToString(), Network.outputLRM);
        }
    }
}
