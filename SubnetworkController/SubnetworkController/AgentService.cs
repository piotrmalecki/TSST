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

namespace SubnetworkController
{
   class AgentService : Service
        {
         
            CCclient ccClient;
            string[] tab;
            public int callID { get; set; }
    

            public AgentService(Socket socket, CC cc, CCclient ccClient)
            {
                this.cc = cc;
                this.socket = socket;
                this.ccClient = ccClient;
             }

            public bool sendConnReq(String from, String to, int[] lambdyWej, int[] lambdyWyj, int call)
            {
              
                        Console.WriteLine(DateTime.Now + " CC: Wysyłam ConnectionRequest(" + from + ", " + to + ") do CC w w" + id + ", callID: " + call);
                        send(Protocol.CONN_REQ + " " + from + " " + to + " " + lambdyWej[0].ToString() + " " + lambdyWej[1].ToString() + " "
                            + lambdyWyj[0].ToString() + " " + lambdyWyj[1].ToString() + " " + call.ToString());


                        Thread.Sleep(1000);
                        if (tab[0] == Protocol.CONN_RSP)
                        {
                            callID = Convert.ToInt32(tab[1]);
                            Console.WriteLine(DateTime.Now + " CC: ConnectionConfirmed od wezla " + id + ", callID: " + callID);
                            return true;
                        }
                        else
                        {
                            callID = Convert.ToInt32(tab[1]);
                            Console.WriteLine(DateTime.Now + " CC: ConnectionFailed od wezla " + id + ", callID: " + callID);
                            
                            return false;
                        }
                   

                }



            


            public void Run()
            {
                while (true)
                {
                    String com = receive();
                   // Console.WriteLine(com);
                    tab = com.Split(' ');
                    String command = tab[0];

                    if (command.Equals(Protocol.LOGIN))
                    {
                        id = tab[1];

                        Console.WriteLine(DateTime.Now + " Węzeł: " + command + " " + id);
                        send(Protocol.CONF);
                        //Console.WriteLine("Wysłano: confirmation do " + id);

                    }

                    else if (command.Equals(Protocol.NULLCOMMAND))
                    {
                        break;
                    }

                }
               cc.removeAgentService(this);
            }
            }
}
