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
  
        class ClientService : Service
        {
            private String idToConnect;
            private String idToDisconnect;
            private String capacity;
            private int callID;
            String command;
            


            public ClientService(Socket socket, Controller controller, int callID)
            {
                this.controller = controller;
                this.socket = socket;
                this.callID = callID;
             }

            public void SendConf(int callID, int[]lambdy, bool conf)
            {
                if (conf)
                    send(Protocol.CONN_RSP + " " + lambdy[0] + " " + lambdy[1] + " " + callID);
                else
                    send(Protocol.CONN_FAIL + " " + callID);
            }

            public bool SendCallInd(String odKogo, String doKogo)
            {
                 Console.WriteLine(DateTime.Now + " NCC: Wysyłam CallIndication " + doKogo);
                send(Protocol.CALL_IND + " " + odKogo);
                Thread.Sleep(2000);
                if (command.Equals(Protocol.CALL_ACCEPT))
                {
                    Console.WriteLine(DateTime.Now + " NCC: Otrzymano CallAccept od " + doKogo);
                    return true;
                }
                else
                {
                    Console.WriteLine(DateTime.Now + " NCC: " + doKogo + " nie zaakceptował połączenia ");
                    return false;
                }

            }

            public void SendCallAccept(String odKogo, String doKogo, bool accept, int lam0, int lam1)
            {
                if (accept)
                {
                    Console.WriteLine(DateTime.Now + " NCC: Wysyłam CallAccept do " + odKogo);
                    send(Protocol.CALL_ACCEPT + " " + lam0 + " " + lam1);
                }

                else
                {
                    Console.WriteLine(DateTime.Now + " NCC: Wysyłam CallFailed do " + odKogo);
                    send(Protocol.CALL_FAIL + " " + doKogo);

                }


            }

            public void Run()
            {
                while (true)
                {
                    command = receive();
                    string[] tab = command.Split(' ');
                    command = tab[0];


                    if (command.Equals(Protocol.LOGIN))
                    {
                        id = tab[1];

                        Console.WriteLine(DateTime.Now + " Klient: " + command + " " + id);
                        send(Protocol.CONF);
                        //Console.WriteLine("Wysłano: confirmation do " + id);
                    }
                     


                    else if (command.Equals(Protocol.CONNECTION))
                    {
                        callID++;
                        idToConnect = tab[1];
                        capacity = tab[2];
                        Console.WriteLine(DateTime.Now + " NCC: CallRequest od klienta " + id + ": ŻĄDANIE POŁĄCZENIA z " + idToConnect + ", przepustowość: " + capacity +", CallID " + callID);
                        controller.Connect(id, idToConnect, Convert.ToInt32(capacity), callID);

                    }

                    else if (command.Equals(Protocol.END))
                    {
                        idToDisconnect = tab[1];
                        Console.WriteLine(id + ": " + command + " " + idToDisconnect);
                    }

                    else if (command.Equals(Protocol.NULLCOMMAND))
                    {
                        break;
                    }

                }
                controller.removeClientService(this);
            }
            }



        }
    

