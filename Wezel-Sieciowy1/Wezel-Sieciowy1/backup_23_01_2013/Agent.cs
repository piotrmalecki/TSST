using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Timers;


namespace Wezel_Sieciowy1
{
    class Agent
    {
        public static int connections;
        private String[] command = new String[10];
        Komutator kom = null;

        private Siec_Init Network;
        private Sygnal_Text communicationT = null;

        private Thread listenCC = null;
        private Thread listenSZ = null;
        
        
        /**  command[10]:
         * logowanie nasluchuje odpowiedzi na 0 - conf
         * polaczenie nr 1 na 1. np. 23 - 2 in 3 out
         *            nr 2 na 2 itd max 5 polaczen
         *            nr 3
         *            nr 4
         *            nr 5
         *            
         * konwersja portów nr 6.
         * disconnect na 9   
         * 
         * bledy na 10
         **/

        public Agent(Siec_Init Network)
        {
            this.Network = Network;
            connections = 0;

            Boolean connCC = false;
            Boolean connSZ = false;

            communicationT = new Sygnal_Text();
            kom = new Komutator(Network);

            //Thread closerT = new Thread(Maintenance.closer);
            //closerT.Start();

            for (int i = 0; i < command.Length; i++)
                command[i] = null;


            if (Network.connectService(true))
            {
                Console.WriteLine("Nawiązano połączenie z Systemem Zarządzania.");
                connSZ = true;
            }
            else
                Console.WriteLine("\nSystem Zarządzania niedostępny.");

            if (Network.connectService(false))
            {
                Console.WriteLine("Nawiązano połączenie z Chmurą Kablową.");
                connCC = true;
            }
            else
                Console.WriteLine("\nChmura Kablowa niedostępna.");

            if (connSZ)
            {
                listenSZ = new Thread(new ParameterizedThreadStart(Listen_Text));
                listenSZ.Start(true); 
            }

            if (connCC)
            {
                listenCC = new Thread(new ParameterizedThreadStart(Listen_Text)); //przypisanie watka
                listenCC.Start(false); //rozpoczecie nasluchiwania CC
            }

            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie

            if (connSZ && connCC) //jeśli sa polaczenia rozpocznij logowanie do systemów
            {
                if (Loguj(true))
                {
                    Console.WriteLine("gitara. Logowanie do Systemu Zarządzania zakończone sukcesem."); //jesli logowanie zakonczone sukcesem, kontynuuj, jesli nie, zakoncz nasluch.
                    if (Loguj(false))
                        Console.WriteLine("gitara. Logowanie do Chmury Kablowej zakończone sukcesem.");
                    else
                        Console.WriteLine("\nLogowanie do Chmury Kablowej zakończone porażką. bęben.");
                }
                else
                    Console.WriteLine("\nLogowanie do Systemu Zarządzania zakończone porażką. bęben. Węzeł na złom.");
            }
            else //zamyka połączenia, nasłuch wyłącza program.
            {
                try
                {
                    listenSZ.Abort();
                    listenCC.Abort();
                }
                catch (ThreadStateException e) { }
                catch (NullReferenceException e) { }
                
                Network.DisconnectService(true);
                Network.DisconnectService(false);
                
                Console.WriteLine("\n\nWystąpiły błedy, naciśnij dowolny klawisz aby zakończyć.");
                Console.ReadKey();
                Environment.Exit(0);


            }
            
            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie

            try
            {
                listenCC.Abort(); //wylaczenie nasluchu tekstowego na CC

                Thread.Sleep(1500);
                //Network.inputCC.Close();
                //Network.outputCC.Close();
                Network.inputCloud = null;
                Network.outputCloud = null;
                Console.WriteLine("Wyłączono nasluch tekstowy CC.");
            }
            catch (NullReferenceException e) { Console.WriteLine("Nie wyłączono nasłuchu."); }
            catch (EncoderFallbackException e) { Console.WriteLine("Nie wyłączono nasłuchu."); }

            Console.Write("\n------------------------------\n\n"); //robi odstęp dla czystości w oknie
            //if (Console.ReadLine().Equals("x"))
            //{
            //    Test testuj = new Test(Network);
            //}
            //Loguj(true);
            
          
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! KONIEC KONSTRUKTORA !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        private Boolean Loguj(Boolean service) //petla ponownie oczekuje na confirmation, nie wysyla 
        {                                       // jeszcze raz logina
            String message = "";
            Boolean x,y;
            x = false;
            y = false;
            int i = 0;


            if (service)
            {
                message = Protokol.LOGIN + " " + Convert.ToString(Network.idW);
                communicationT.Send_Text(message, Network.outputSZ);
            }
            else if (service == false) 
            {
                message = (Protokol.LOGIN + " " + Convert.ToString(Network.idW) + " n");
                communicationT.Send_Text(message, Network.outputCloud);
            }

            //System.Timers.Timer timer_Log = new System.Timers.Timer();
            //timer_Log.Elapsed = 
            //timer_Log.Interval = 2000;
            //timer_Log.Enabled = true;
            
            while (!x)
            {
                Thread.Sleep(1000);

                if (command[0] == Protokol.CONF)
                {
                    Console.WriteLine("Logowanie, sukces.");
                    x = true;
                    command[0] = null;
                    y = true;
                    return (y);
                }
                else if ((x == false) && (i < 5))
                {
                    Console.WriteLine("Błąd logowania, ponawiam próbę: " + Convert.ToString(i+1));
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
            StringBuilder word;
            StringBuilder digits;
            Boolean serv = (Boolean)service;
            Boolean x = true;
            

            while (x==true)
            {
                word = null;
                digits = null;
                String message;

                word = new StringBuilder(); //potrzebne do rozpoznawania polecen
                digits = new StringBuilder();

                //Console.WriteLine("nasluch");
                if (serv)
                    message = communicationT.Receive_Text(Network.inputSZ); //odczytuje w trybie ciaglym ze strumieni wejsciowych
                else
                {
                    message = communicationT.Receive_Text(Network.inputCloud);
                    x = false;
                }
                
                String digitsT = null;

                if ((serv) && (message != ""))
                    Console.WriteLine("SZ: " + message);
                else if ((!serv) && (message != ""))
                    Console.WriteLine("CC: " + message);

                
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
                    case Protokol.CONF:
                        command[0] = message;
                        break;
                    case Protokol.ERR:
                        command[10] = message;
                        break;
                    case Protokol.SET:
                        command[connections] = digitsT;
                        if (checkConf())
                            SetConf();
                        break;
                    case Protokol.DISCON:
                        command[9] = digitsT;
                        Disconnect();
                        break;
                    case Protokol.CONV:
                        command[6] = digitsT;
                        Conversion();
                        break;
                    default:
                        if (serv)
                            Console.WriteLine("błędne polecenie od SZ.");
                        else
                            Console.WriteLine("błędne polecenie od CC");
                        break;
                }
                
                
            }

        }

        private void Disconnect()
        {
            int portIn, portOut, f1;

            portIn = Convert.ToInt32(command[9].Substring(0, 1));
            portOut = Convert.ToInt32(command[9].Substring(1, 1));
            f1 = Convert.ToInt32(command[9].Substring(2, 1));


            if (connections > 0)
            {
                kom.Disconnect(portIn, portOut, f1);
                communicationT.Send_Text(Protokol.DISCON_R, Network.outputSZ);
                connections--;
            }
            else
                communicationT.Send_Text(Protokol.ERR, Network.outputSZ);

            Console.WriteLine("ilosc polaczen: " + connections);
        }

        private Boolean checkConf()         //sprawdza w prosty sposob czy jest mozliwosc zestawienia polaczenia
        {
            if (connections < 5)            //5 bo pierwotnie bylo 5 portow
            {
                return true;
            }
            else
            {
                communicationT.Send_Text(Protokol.ERR, Network.outputSZ);
                return false;
            }

        }

        private void SetConf()  //powiadamia Komutator ze trzeba krosowac
        {
            int portIn, portOut, f1, f2;

            communicationT.Send_Text(Protokol.SET_R, Network.outputSZ);
            portIn = Convert.ToInt32(command[connections].Substring(0, 1));
            portOut = Convert.ToInt32(command[connections].Substring(1, 1));
            f1 = Convert.ToInt32(command[connections].Substring(2,1));
            f2 = Convert.ToInt32(command[connections].Substring(3, 1));

            if (Network.strumienCloud != null)
            {
                connections++; //zwiekszanie licznika polaczen
                Komutator.SetConnect(portIn, portOut, f1, f2, connections); // wyslanie danych nowego polaczenia, connections = id polaczenia
            }

            
        }

        private void Conversion()
        {
            String f1_new, f2_new;
            f1_new = f2_new = null;

            f1_new = command[6].Substring(0, 1);    //poprawic! zeby dzialal tez z liczbami wiekszymi niz 9, zamiana na dwucyfrowy itd
            f2_new = command[6].Substring(1, 1);

            if (f1_new.Length < 2)              //dopelnianie zeby z 1 zrobic 01 itd
                f1_new = f1_new.Insert(0, "0");
            if (f2_new.Length < 2)
                f2_new = f2_new.Insert(0, "0");


            kom.SetConversion(f1_new, f2_new, connections);

        }


       



    }
}
