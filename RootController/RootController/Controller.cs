using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;

namespace RootController
{
    class Controller
    {

        private TcpListener listener;
        private Socket socket;
        private TcpListener listenerCC;
        private Socket socketCC;
        private ClientService clientService;
        private List<ClientService> clients = new List<ClientService>();
        private CCService ccService;
        private List<CCService> cclist = new List<CCService>();
        public static int  callID=0;
        private RC rc;
        private LRM lrm;
        private List<Directory> direct = new List<Directory>();
         int[] lam;
        String brakLambdSnpp;
        String brakLambdSnppEnd;
        String odKogo;
        String doKogo;



        public Controller()
        {
           
            FileStream  stream = new FileStream("Directory.txt", FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            while (reader.EndOfStream == false)
            {
                string data;
                data = reader.ReadLine();
                string[] info = data.Split(' ');
                direct.Add(new Directory(info[0], info[1]));
            }


            IPAddress localaddr = IPAddress.Parse("127.0.0.1");
            try
            {
                listener = new TcpListener(localaddr, 40001);
                listener.Start();
                listenerCC = new TcpListener(localaddr, 40002);
                listenerCC.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine(DateTime.Now + " Błąd uruchamiania RootControllera");
            }

            Console.WriteLine(DateTime.Now + " RootController uruchomiony");
            Thread thread = new Thread(Run);
            thread.Start();
            Thread CCthread = new Thread(RunCC);
            CCthread.Start();
            
            rc = new RC();
            lrm = new LRM();

            rc.stworzGraf(lrm.getLacza());

                 
    }

        
        void Run()
        {
            while (true)
            {
                try
                {
                    socket = listener.AcceptSocket();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine(DateTime.Now + " Nie można połączyć z klientem");
                }
                
                if (socket.Connected)
                {

                    Console.WriteLine(DateTime.Now + " Klient połączony");
                   clientService = new ClientService(socket, this, callID);
                    addClientService(clientService);
                }
            }
        }

        void RunCC()
        {
            while (true)
            {
                try
                {
                    socketCC = listenerCC.AcceptSocket();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine(DateTime.Now + " Nie można połączyć z Subnetwork Controllerem");
                }

                if (socketCC.Connected)
                {

                    //Console.WriteLine(DateTime.Now + " Subnetwork Controller połączony");
                    ccService = new CCService(socketCC, this);
                    addCCService(ccService);
                    //ilePodsieci++;
                }
            }
        }




        private void addClientService(ClientService clientService)
        {
            try
            {
                clientService.init();
                clients.Add(clientService);

            }
            catch (IOException)
            {
                Console.WriteLine(DateTime.Now + " Błąd strumienia wejścia/wyjścia");
            }
            Thread service = new Thread(clientService.Run);
            service.Start();
        }


        public void removeClientService(ClientService clientService)
        {
            Console.WriteLine(DateTime.Now + " Zakonczono serwis klienta " + clientService.getId().ToString());
            clientService.close();
            clients.Remove(clientService);

        }


        private void addCCService(CCService ccService)
        {
            try
            {
                ccService.init();
                cclist.Add(ccService);

            }
            catch (IOException)
            {
                Console.WriteLine(DateTime.Now + " Błąd strumienia wejścia/wyjścia");
            }
            Thread service = new Thread(ccService.Run);
            service.Start();
        }


        public void removeCCService(CCService ccService)
        {
            Console.WriteLine(DateTime.Now + " Zakonczono serwis Subnetwork Controllera " + ccService.getId().ToString());
            ccService.close();
            cclist.Remove(ccService);

        }

        public void Connect(String idFrom, String idTo, int capacity, int callID)
        {
            odKogo = idFrom;
            doKogo = idTo;

            Console.WriteLine("\n" + DateTime.Now + " NCC: Przeszukuję katalog...");
            Console.WriteLine(DateTime.Now + " Połączenie od klienta: " + znajdzWKatalogu(idFrom) + ", do klienta: " + znajdzWKatalogu(idTo));

            string[] s = znajdzWKatalogu(idFrom).Split('.');
            string[] z = znajdzWKatalogu(idTo).Split('.');
            bool znaleziono;

            List<String> snppSeq = new List<String>();
            if (!s[1].Equals(z[1]))
            {
                snppSeq = rc.ZnajdzDroge(s[1], z[1]);
                znaleziono = ZnajdzLam(snppSeq, capacity);
            }
            else
            {
                snppSeq.Add(z[2]);
                snppSeq.Add(s[2]);
                znaleziono = true;
                lam = new int[2];
                lam[0] = 0;
                lam[1] = 0;
            }
               
                if (!znaleziono)
                {
                    Console.WriteLine("Nie ma linkow");
                    Console.WriteLine("Snpp " + brakLambdSnpp);
                    Console.WriteLine("SnppEnd " + brakLambdSnppEnd);

                }


                else
                {
                    bool zestawione = ZestawPol(snppSeq, callID, capacity, s[1], s[2], z[1], z[2]);
                    if (!zestawione)
                    {
                        Console.WriteLine(DateTime.Now + " CC: Wysyłam ConectionFailed do NCC");

                        //ccClient.sendConf(callID, false);
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now + " CC: Wysyłam ConectionConfirmed do NCC");
                        for (int i = 0; i < clients.Count; i++)
                        {
                            
                            if (clients[i].id.Equals(doKogo))
                            {
                                bool accept = clients[i].SendCallInd(odKogo, doKogo);

                                for (int j = 0; j < clients.Count; j++)
                                {
                                    if (clients[j].id.Equals(odKogo))
                                    {
                                        for (int k = 0; k < cclist.Count; k++)
                                        {

                                            if (cclist[k].lambdyK[0] != 0 && cclist[k].lambdyK[0] != 0)
                                                clients[j].SendCallAccept(odKogo, doKogo, accept, cclist[k].lambdyK[0], cclist[k].lambdyK[1]);
                                           // else
                                                //clients[j].SendCallAccept(odKogo, doKogo, accept, 0, 0);
                                        }



                                    }
                                }
                            }
                        }
                    }
                }
            }

        
       bool ZnajdzLam(List<String> snppSeq, int capacity)
       {
            lam = new int[2];
            lam = lrm.ZnajdzLambdy(snppSeq[0], snppSeq[1], capacity);
            if (lam[0] == 0 || lam[1] == 0)
            {
                brakLambdSnpp = snppSeq[1];
                brakLambdSnppEnd = snppSeq[0];
                //Console.WriteLine("Nie ma lambd");
                return false;
            }

            return true;
       }
           

       bool ZestawPol(List<String> snppSeq, int callID, int capacity, String s1, String s2, String z1, String z2)
       {
          // Console.WriteLine("LAMBDY : " + lam[0] + " " + lam[1]);
            for (int i = 0; i < cclist.Count; i++)
            {
                if (cclist[i].id.Equals(s1))
                {
                    bool b = cclist[i].sendConnReq(s2, snppSeq[0], lam, callID, capacity);
                    if (!b)
                        return false;
                    
                }

                else if (cclist[i].id.Equals(z1))
                {
                  bool b = cclist[i].sendConnReq(snppSeq[1], z2, lam, callID, capacity);
                  if (!b)
                        return false;
                    
                }


            }

            return true;
        }
       






       String znajdzWKatalogu(String id)
        {
            
            for (int i = 0; i < direct.Count; i++)
            {
                if (direct[i].name.Equals(id))
                {
                    return direct[i].address;
                }
            }

            return null;
        }




    }
}

