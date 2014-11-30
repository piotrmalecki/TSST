using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;

namespace TSST
{

    public class ClientService : Service
    {
      
        private ComboBox k1_f1;
        private ComboBox k1_f2;
        private ComboBox k2_f1;
        private ComboBox k2_f2;
        private ComboBox k3_f1;
        private ComboBox k3_f2;
        private ComboBox k1_mod;
        private ComboBox k2_mod;
        private ComboBox k3_mod;
        private Button wyslij_k;
        private Button wyczysc_k;
        private String idToConnect;
        private String idToDisconnect;
        private String capacity;


        public ClientService(Socket appSocket, ManagementApp managementApp, TextBox console, 
            ComboBox k1_f1, ComboBox k1_f2, ComboBox k2_f1, ComboBox k2_f2, ComboBox k3_f1, ComboBox k3_f2, 
            ComboBox k1_mod, ComboBox k2_mod, ComboBox k3_mod, Button wyslij_k, Button wyczysc_k)
        {
            this.managementApp = managementApp;
            this.appSocket = appSocket;
            this.console = console;
            this.k1_f1 = k1_f1;
            this.k1_f2 = k1_f2;
            this.k2_f1 = k2_f1;
            this.k2_f2 = k2_f2;
            this.k3_f1 = k3_f1;
            this.k3_f2 = k3_f2;
            this.k1_mod = k1_mod;
            this.k2_mod = k2_mod;
            this.k3_mod = k3_mod;
            this.wyslij_k = wyslij_k;
            this.wyczysc_k = wyczysc_k;
        }

      
        public void Run()
        {
            while (true)
            {
                String command = receive();
                string[] tab = command.Split(' ');
                command = tab[0];


                if (command.Equals(Protocol.LOGIN))
                {
                    id = tab[1];
                  
                    ChangeText("Klient: " + command +" " +id);
                    send(Protocol.CONF);
                    ChangeText("Wysłano: confirmation do klienta " + id);
                    wyslij_k.Invoke((MethodInvoker)delegate { wyslij_k.Enabled = true; });
                    wyczysc_k.Invoke((MethodInvoker)delegate { wyczysc_k.Enabled = true; });
                    if (id == "Franek")
                    {
                        k1_f1.Invoke((MethodInvoker)delegate { k1_f1.Enabled = true; });
                        k1_f2.Invoke((MethodInvoker)delegate { k1_f2.Enabled = true; });
                        k1_mod.Invoke((MethodInvoker)delegate { k1_mod.Enabled = true; });
                    }
                    else if (id == "Henio")
                    {
                        k2_f1.Invoke((MethodInvoker)delegate { k2_f1.Enabled = true; });
                        k2_f2.Invoke((MethodInvoker)delegate { k2_f2.Enabled = true; });
                        k2_mod.Invoke((MethodInvoker)delegate { k2_mod.Enabled = true; });
                    }
                    else if (id == "Zenek")
                    {
                        k3_f1.Invoke((MethodInvoker)delegate { k3_f1.Enabled = true; });
                        k3_f2.Invoke((MethodInvoker)delegate { k3_f2.Enabled = true; });
                        k3_mod.Invoke((MethodInvoker)delegate { k3_mod.Enabled = true; });
                    }
                }


                else if (command.Equals(Protocol.CONNECTION))
                {
                    idToConnect = tab[1];
                    capacity = tab[2];
                    ChangeText("Klient " + id + " : ŻĄDANIE POŁĄCZENIA z klientem " + idToConnect + ", przepustowość: " + capacity);
 
                }

                else if (command.Equals(Protocol.END))
                {
                    idToDisconnect = tab[1];
                    ChangeText("Klient " +  id + ": " + command + " " + idToDisconnect);
                }

                else if (command.Equals(Protocol.NULLCOMMAND))
                {
                    break;
                }

            }
            managementApp.removeClientService(this);
        }



        public void SendParameters(string s1, string s2, string s3)
        {

            send(Protocol.PARAMETERS + " " + s1 + " " + s2 + " " + s3);
            ChangeText("Wysłano: parameters do klienta " + id);
        }

    }
}


