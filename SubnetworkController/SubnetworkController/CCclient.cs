using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;


namespace SubnetworkController
{
    class CCclient : Service
    {

        private String subnetwork;
       // private List<Wezel> wezly = new List<Wezel>();



        public CCclient(CC cc, String subnetwork, StreamWriter output, StreamReader input)
        {
            this.cc = cc;
            this.output = output;
            this.input = input;
            this.subnetwork = subnetwork;
        }

        public void Run()
        {
            send(Protocol.LOGIN + " " + subnetwork);
            while (true)
            {
                String command = receive();
                string[] tab = command.Split(' ');
                command = tab[0];
                //Console.WriteLine("RootController: " + command);

                if (command.Equals(Protocol.CONF))
                {
                    Console.WriteLine(DateTime.Now + " RootController: " + command);
                }

                else if (command.Equals(Protocol.CONN_REQ))
                {
                   
                    String from = tab[1];
                    String to = tab[2];
                    int[] lambdy = new int [2];
                    lambdy[0] = Convert.ToInt32(tab[3]);
                    lambdy [1] = Convert.ToInt32(tab[4]);
                    int callID = Convert.ToInt32(tab[5]);
                    int capacity = Convert.ToInt32(tab[6]);

                    Console.WriteLine(DateTime.Now + " CC: żądanie ConnectionRequest(" + from + ", " + to +") od root CC, callID: " + callID.ToString()
                        + ", przepustowość: " + capacity);
                    cc.Connect(from, to, lambdy, callID, capacity);
                    
                }

                else if (command.Equals(Protocol.NULLCOMMAND))
                {
                    Console.WriteLine(DateTime.Now + " RootController: " + command);
                    break;
                }
                }


              

            }
        

      public void sendConf(int callID, bool conf, int[]lambdy)
                {
                    if (conf)
                        send(Protocol.CONN_RSP + " " + callID + " " + lambdy[0] + " " + lambdy[1]);
                    else
                        send(Protocol.CONN_FAIL + " " + callID);
                }
    }
}


