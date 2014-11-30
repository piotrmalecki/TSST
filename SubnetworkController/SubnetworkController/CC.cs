using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SubnetworkController
{

    class CC
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private StreamWriter output;
        private StreamReader input;
        private String subnetwork;
        private TcpListener listener;
        private Socket socket;
        private AgentService agentService;
        private List<AgentService> agents = new List<AgentService>();
        private CCclient ccClient;
        private LRM lrm;
        private RC rc;
        public static int stan = 0;
        List<int[]> lam;
        String brakLambdSnpp;
        String brakLambdSnppEnd;
        int[] lambdyDoWyslania = new int[2];
     


        public CC(String subnetwork, String port)
        {
            
            this.subnetwork = subnetwork;

            IPAddress localaddr = IPAddress.Parse("127.0.0.1");
            try
            {
                listener = new TcpListener(localaddr, Convert.ToInt32(port));
                listener.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine(DateTime.Now + " Błąd uruchamiania SubnetworkControllera");
            }

            Console.WriteLine(DateTime.Now + " SubnetworkController uruchomiony");
            Thread thread = new Thread(RunServ);
            thread.Start();




            try
            {
                tcpClient = new TcpClient("127.0.0.1", 40002);

            }


            catch (SocketException)
            {
                Console.WriteLine(DateTime.Now + " Nie udało się nawiązać połączenia z RootConrrollerem");
            }
            try
            {
                stream = tcpClient.GetStream();
                input = new StreamReader(stream);
                output = new StreamWriter(stream);
            }
            catch (IOException)
            {
                Console.WriteLine(DateTime.Now + " Błąd strumienia wejścia/wyjścia");

            }
            ccClient = new CCclient(this, subnetwork, output, input);
            Thread th = new Thread(ccClient.Run);
            th.Start();

            lrm = new LRM(subnetwork);
            rc = new RC();
            rc.stworzGraf(lrm.getLacza());

        }



        void RunServ()
        {
            while (true)
            {
                while (true)
                {
                    try
                    {
                        socket = listener.AcceptSocket();
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine(DateTime.Now + " Nie można połączyć z węzłem");
                    }

                    if (socket.Connected)
                    {

                        Console.WriteLine(DateTime.Now + " Wezęł połączony");
                        agentService = new AgentService(socket, this, ccClient);
                        addAgentService(agentService);
                        //ileWezlow++;
                    }
                }
            }
        }


        private void addAgentService(AgentService agentService)
        {
            try
            {
                agentService.init();
                agents.Add(agentService);

            }
            catch (IOException)
            {
                Console.WriteLine(DateTime.Now + " Błąd strumienia wejścia/wyjścia");
            }
            Thread service = new Thread(agentService.Run);
            service.Start();
        }


        public void removeAgentService(AgentService agentService)
        {
            Console.WriteLine(DateTime.Now + " Zakonczono serwis agenta " + agentService.getId().ToString());
            agentService.close();
            agents.Remove(agentService);
            //ileWezlow--;

        }

        public void Connect(String idFrom, String idTo, int[] lambdyOdCC, int callID, int capacity)
        {
            List<String> snppSeq = new List<String>();
           // Console.WriteLine("LAMBDY : " + lambdyOdCC[0] + " " + lambdyOdCC[1]);
            snppSeq = rc.ZnajdzDroge(idFrom, idTo);
            
            bool znaleziono = ZnajdzLam(idFrom, idTo, snppSeq, capacity, lambdyOdCC, callID);
            if (!znaleziono)
            {
                Console.WriteLine("Nie ma linkow");
                Console.WriteLine("Snpp " + brakLambdSnpp);
                Console.WriteLine("SnppEnd " + brakLambdSnppEnd);

            }

            else
            {
                Console.WriteLine(lam.Count);
               bool zestawione = ZestawPol(snppSeq, idFrom, idTo, callID);
               //bool zestawione = false;
                 if (!zestawione)
                {

                    Console.WriteLine(DateTime.Now + " CC: Wysyłam ConectionFailed do root CC");
                    ccClient.sendConf(callID, false, lambdyDoWyslania);
                }
                else
                {
                    Console.WriteLine(DateTime.Now + " CC: Wysyłam ConectionConfirmed do root CC");
                    ccClient.sendConf(callID, true, lambdyDoWyslania);
                }
            }


        }

        public bool ZestawPol(List<String> snppSeq, String idFrom, String idTo, int callID)
        {
            if (snppSeq[snppSeq.Count - 1][0].Equals('k') && snppSeq[0][0].Equals('k'))
           {
               if (snppSeq[snppSeq.Count - 1][0].Equals('k'))
               {
                   for (int i = snppSeq.Count - 2; i > 0; i = i - 2)
                   {

                       for (int j = 0; j < agents.Count; j++)
                       {

                           if (agents[j].id[0].Equals(snppSeq[i][0]))
                           {
                               bool b = agents[j].sendConnReq(snppSeq[i], snppSeq[i - 1], lam[lam.Count - i], lam[lam.Count - i+1], callID);
                               if (!b)
                                   return false;
                           }
                       }
                   }
               }
            
            }
            else if (snppSeq[snppSeq.Count - 1][0].Equals('k'))
            {
               for (int i = snppSeq.Count - 2; i > 0; i = i - 2)
                {
                    for (int j = 0; j < agents.Count; j++)
                    {
                        
                        if (agents[j].id[0].Equals(snppSeq[i][0]))
                        {

                            if (i == 0)
                            {
                                bool b = agents[j].sendConnReq(snppSeq[i], idTo, lam[lam.Count - i - 1], lam[lam.Count - i], callID);
                                if (!b)
                                    return false;
                            }

                            else
                            {
                              
                                bool b = agents[j].sendConnReq(snppSeq[i], snppSeq[i - 1], lam[lam.Count - i - 1], lam[lam.Count - i], callID);
                                if (!b)
                                    return false;
                            }
                        }

                   }
                }
            }

            else if (snppSeq[0][0].Equals('k'))
            {
              
                for (int i = snppSeq.Count - 2; i > 0; i = i - 2)
                {
                    for (int j = 0; j < agents.Count; j++)
                    {

                       
                        if (agents[j].id[0].Equals(snppSeq[i][0]))
                        {

                            if (i == snppSeq.Count - 2)
                            {
                                bool b = agents[j].sendConnReq(idFrom, snppSeq[i], lam[lam.Count - i - 1], lam[lam.Count - i], callID);
                                if (!b)
                                    return false;
                            }

                            else
                            {
                                bool b = agents[j].sendConnReq(snppSeq[i], snppSeq[i - 1], lam[lam.Count - i - 1], lam[lam.Count - i], callID);
                                if (!b)
                                    return false;
                            }
                        }

                    }
                }
            }
            return true;
        }

    public bool ZnajdzLam(String idFrom, String idTo, List<String> snppSeq, int capacity, int[] lambdyOdCC, int callID)
        {
            lambdyDoWyslania[0] = 0;
            lambdyDoWyslania[1] = 0;
             lam = new List<int[]>();
             int[] lambdyWyj = new int[2];
             int [] lambdyK = new int[2];
            int[] lambdyWej = new int[2];
                   
   
               if (snppSeq[snppSeq.Count - 1][0].Equals('k'))
                {
                   lambdyK = lrm.ZnajdzLambdy(snppSeq[snppSeq.Count - 1], snppSeq[snppSeq.Count - 2], capacity);
                   if (lambdyK[0] == 0 || lambdyK[1] == 0)
                   {
                       brakLambdSnpp = snppSeq[snppSeq.Count - 1];
                       brakLambdSnppEnd =  snppSeq[snppSeq.Count - 2];
                       //Console.WriteLine("Nie ma lambd");
                       return false;
                   }

                   lambdyDoWyslania = lambdyK;
                  
                   //Console.WriteLine("Klient na poczatku");
                   lambdyWej = lambdyK;
                  
                   for (int i = snppSeq.Count - 3; i >= 0; i = i - 2)
                   {
                       if (i == 0)
                           lambdyWyj = lambdyOdCC;
                       else
                       lambdyWyj = lrm.ZnajdzLambdy(snppSeq[i], snppSeq[i-1], capacity);
                       if (lambdyWyj[0] == 0 || lambdyWyj[1] == 0)
                       {
                            brakLambdSnpp = snppSeq[i+1];
                            brakLambdSnppEnd = snppSeq[i];
                           //Console.WriteLine("Nie ma lambd");
                           return false;
                       }

                      
                       lam.Add(lambdyWej);
                       lam.Add(lambdyWyj);
                      

                       lambdyWej = lambdyWyj;
                   }                 

                }
                else if (snppSeq[0][0].Equals('k'))
                {
                   lambdyK = lrm.ZnajdzLambdy(snppSeq[1], snppSeq[0], capacity);
                   if (lambdyK[0] == 0 || lambdyK[1] == 0)
                   {
                       brakLambdSnpp = snppSeq[1];
                       brakLambdSnppEnd =  snppSeq[0];
                       //Console.WriteLine("Nie ma lambd");
                       return false;
                   }
                  
                   //Console.WriteLine("Klient na koncu");
                   lambdyWej = lambdyOdCC;
                  
                   for (int i = snppSeq.Count - 2; i > 0; i = i - 2)
                   {
                       if (i == 1)
                           lambdyWyj = lambdyK;
                       else
                       lambdyWyj = lrm.ZnajdzLambdy(snppSeq[i], snppSeq[i-1], capacity);
                       if (lambdyWyj[0] == 0 || lambdyWyj[1] == 0)
                       {
                            brakLambdSnpp = snppSeq[i+1];
                            brakLambdSnppEnd = snppSeq[i];
                           //Console.WriteLine("Nie ma lambd");
                           return false;
                       }

                       lam.Add(lambdyWej);
                       lam.Add(lambdyWyj);

                       lambdyWej = lambdyWyj;
                   }                 

                }
                

                return true;
}



            }
}

           
