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
    class CCService : Service
    {
        //private String idToConnect;
        //private String idToDisconnect;
        //private String capacity;
       // private Wierzcholek wierzcholek;
        //private List<Sasiad> sasiedzi;
        string[] tab;
        public int[] lambdyK { get; set; }
      

        public CCService(Socket socket, Controller controller)
        {
            this.controller = controller;
            this.socket = socket;
            lambdyK = new int[2];
            lambdyK[0] = 0;
            lambdyK[1] = 0;
        }

        public bool sendConnReq(String from, String to, int[] lambdy, int callID, int capacity)
        {
            
            Console.WriteLine(DateTime.Now + " CC: Wysyłam ConnectionRequest("+from+", " + to + ") do CC " + id + ", callID: " + callID);
            send(Protocol.CONN_REQ + " " + from + " " + to + " " + lambdy[0].ToString() + " " + lambdy[1].ToString() + " " + callID.ToString() + " "
                + capacity.ToString());


            Thread.Sleep(5000);
            if (tab[0] == Protocol.CONN_RSP)
            {
                callID = Convert.ToInt32(tab[1]);
                lambdyK = new int[2];
               
                    lambdyK[0] = Convert.ToInt32(tab[2]);
                    lambdyK[1] = Convert.ToInt32(tab[3]);
               
                Console.WriteLine(DateTime.Now + "CC: ConnectionConfirmed od CC w podsieci " + id + ", callID: " + callID);
                return true;
            }
            else if (tab[0] == Protocol.CONN_FAIL)
            {
                callID = Convert.ToInt32(tab[1]);
                Console.WriteLine(DateTime.Now + "CC: ConnectionFailed od CC w podsieci " + id + ", callID: " + callID);
                return false;
            }

            return false;
                   

        }
        public void Run()
        {
            while (true)
            {
       
                    String command = receive();
                  
                        tab = command.Split(' ');
                        command = tab[0];
                    if (command.Equals(Protocol.LOGIN))
                    {
                        id = tab[1];

                        Console.WriteLine(DateTime.Now + " SubnetworkController: " + command + " " + id);
                        send(Protocol.CONF);
                        //Console.WriteLine("Wysłano: confirmation do CC " + id);

                      
                    }


                  
                    else if (command.Equals(Protocol.NULLCOMMAND))
                    {
                        break;
                    }
   
            }
             controller.removeCCService(this);
        }
    }
}
