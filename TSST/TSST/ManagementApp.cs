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
    public partial class ManagementApp : Form
    {
        private TcpListener appTcpListener;
        private Socket appSocket;
        private AgentService agentService;
        private List<AgentService> agents = new List<AgentService>();



        private TcpListener appTcpListener_Client;
        private Socket appSocket_Client;
        private ClientService clientService;
        private List<ClientService> clients = new List<ClientService>();



        public ManagementApp()
        {
            InitializeComponent();
            IPAddress localaddr = IPAddress.Parse("127.0.0.1");

            try
            {
                appTcpListener = new TcpListener(localaddr, 10008);
                appTcpListener.Start();
                appTcpListener_Client = new TcpListener(localaddr, 30001);
                appTcpListener_Client.Start();
            }
            catch (SocketException e)
            {
                ChangeText("Błąd uruchamiania aplikacji");
            }

            ChangeText("Aplikacja Zarządzania uruchomiona");

            Thread appThread = new Thread(Run);
            appThread.Start();

            Thread appThread_Client = new Thread(Run_Client);
            appThread_Client.Start();
        }

        void Run_Client()
        {
            while (true)
            {
                try
                {
                    appSocket_Client = appTcpListener_Client.AcceptSocket();
                }
                catch (InvalidOperationException)
                {
                    ChangeText("Nie można połączyć z klientem");
                }

      
                if (appSocket_Client.Connected)
                {

                    ChangeText("Klient połączony");
                    clientService = new ClientService(appSocket_Client, this, console, 
                        k1_f1, k1_f2, k2_f1, k2_f2, k3_f1, k3_f2, k1_mod, k2_mod, k3_mod, wyslij_k, wyczysc_k);
                    addClientService(clientService);
                }
            }
        }

        void Run()
        {
            while (true)
            {
                try
                {
                    appSocket = appTcpListener.AcceptSocket();
                }
                catch (InvalidOperationException)
                {
                    ChangeText("Nie można połączyć z agenetem");
                }

                if (appSocket.Connected)
                {

                    ChangeText("Agent połączony");
                    agentService = new AgentService(appSocket, this, console, 
                        w1_in, w1_out, w2_in, w2_out, w3_in, w3_out, w4_in, w4_out, w5_in, w5_out, w6_in, w6_out, 
                        w1_f1, w1_f2, w2_f1, w2_f2, w3_f1, w3_f2, w4_f1, w4_f2, w5_f1, w5_f2, w6_f1, w6_f2,
                        w1_f1conv, w1_f2conv, w2_f1conv, w2_f2conv, w3_f1conv, w3_f2conv,
                        w4_f1conv, w4_f2conv, w5_f1conv, w5_f2conv, w6_f1conv, w6_f2conv, 
                        wyslij_w, rozlacz, wyczysc_w);
                    addAgentService(agentService);
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
                ChangeText("Błąd strumienia wejścia/wyjścia");
            }
            Thread service = new Thread(new ThreadStart(agentService.Run));
            service.Start();
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
                ChangeText("Błąd strumienia wejścia/wyjścia");
            }
            Thread serviceC = new Thread(new ThreadStart(clientService.Run));
            serviceC.Start();
        }

        public void removeAgentService(AgentService agentService)
        {

            ChangeText("!!!!!!!!!!!! AWARIA WĘZŁA " + agentService.getId().ToString() + " !!!!!!!!!!!!");
            if (agentService.getId() == 1)
            {
                w1_in.Invoke((MethodInvoker)delegate { w1_in.Enabled = false; });
                w1_out.Invoke((MethodInvoker)delegate { w1_out.Enabled = false; });
                w1_f1.Invoke((MethodInvoker)delegate { w1_f1.Enabled = false; });
                w1_f2.Invoke((MethodInvoker)delegate { w1_f2.Enabled = false; });
                w1_f1conv.Invoke((MethodInvoker)delegate { w1_f1conv.Enabled = false; });
                w1_f2conv.Invoke((MethodInvoker)delegate { w1_f2conv.Enabled = false; });
            }
            else if (agentService.getId() == 2)
            {
                w2_in.Invoke((MethodInvoker)delegate { w2_in.Enabled = false; });
                w2_out.Invoke((MethodInvoker)delegate { w2_out.Enabled = false; });
                w2_f1.Invoke((MethodInvoker)delegate { w2_f1.Enabled = false; });
                w2_f2.Invoke((MethodInvoker)delegate { w2_f2.Enabled = false; });
                w2_f1conv.Invoke((MethodInvoker)delegate { w2_f1conv.Enabled = false; });
                w2_f2conv.Invoke((MethodInvoker)delegate { w2_f2conv.Enabled = false; });
            }
            else if (agentService.getId() == 3)
            {
                w3_in.Invoke((MethodInvoker)delegate { w3_in.Enabled = false; });
                w3_out.Invoke((MethodInvoker)delegate { w3_out.Enabled = false; });
                w3_f1.Invoke((MethodInvoker)delegate { w3_f1.Enabled = false; });
                w3_f2.Invoke((MethodInvoker)delegate { w3_f2.Enabled = false; });
                w3_f1conv.Invoke((MethodInvoker)delegate { w3_f1conv.Enabled = false; });
                w3_f2conv.Invoke((MethodInvoker)delegate { w3_f2conv.Enabled = false; });
            }
            else if (agentService.getId() == 4)
            {
                w4_in.Invoke((MethodInvoker)delegate { w4_in.Enabled = false; });
                w4_out.Invoke((MethodInvoker)delegate { w4_out.Enabled = false; });
                w4_f1.Invoke((MethodInvoker)delegate { w4_f1.Enabled = false; });
                w4_f2.Invoke((MethodInvoker)delegate { w4_f2.Enabled = false; });
                w4_f1conv.Invoke((MethodInvoker)delegate { w4_f1conv.Enabled = false; });
                w4_f2conv.Invoke((MethodInvoker)delegate { w4_f2conv.Enabled = false; });
            }
            else if (agentService.getId() == 5)
            {
                w5_in.Invoke((MethodInvoker)delegate { w5_in.Enabled = false; });
                w5_out.Invoke((MethodInvoker)delegate { w5_out.Enabled = false; });
                w5_f1.Invoke((MethodInvoker)delegate { w5_f1.Enabled = false; });
                w5_f2.Invoke((MethodInvoker)delegate { w5_f2.Enabled = false; });
                w5_f1conv.Invoke((MethodInvoker)delegate { w5_f1conv.Enabled = false; });
                w5_f2conv.Invoke((MethodInvoker)delegate { w5_f2conv.Enabled = false; });
            }
            else if (agentService.getId() == 6)
            {
                w6_in.Invoke((MethodInvoker)delegate { w6_in.Enabled = false; });
                w6_out.Invoke((MethodInvoker)delegate { w6_out.Enabled = false; });
                w6_f1.Invoke((MethodInvoker)delegate { w6_f1.Enabled = false; });
                w6_f2.Invoke((MethodInvoker)delegate { w6_f2.Enabled = false; });
                w6_f1conv.Invoke((MethodInvoker)delegate { w6_f1conv.Enabled = false; });
                w6_f2conv.Invoke((MethodInvoker)delegate { w6_f2conv.Enabled = false; });
            }
            
            agentService.close();
            agents.Remove(agentService);
        }

        public void removeClientService(ClientService clientService)
        {
            ChangeText("Zakonczono serwis klienta " + clientService.getName());
            if (clientService.getId() == 1)
            {
                k1_f1.Invoke((MethodInvoker)delegate { k1_f1.Enabled = false; });
                k1_f2.Invoke((MethodInvoker)delegate { w1_f2.Enabled = false; });
                k1_mod.Invoke((MethodInvoker)delegate { k1_mod.Enabled = false; });
            }
            else if (clientService.getId() == 2)
            {
                k2_f1.Invoke((MethodInvoker)delegate { k2_f1.Enabled = false; });
                k2_f2.Invoke((MethodInvoker)delegate { k2_f2.Enabled = false; });
                k1_mod.Invoke((MethodInvoker)delegate { k1_mod.Enabled = false; });
            }
            else if (clientService.getId() == 3)
            {
                k3_f1.Invoke((MethodInvoker)delegate { k3_f1.Enabled = false; });
                k3_f2.Invoke((MethodInvoker)delegate { k3_f2.Enabled = false; });
                k1_mod.Invoke((MethodInvoker)delegate { k1_mod.Enabled = false; });
            }
            clientService.close();
            clients.Remove(clientService);
           
        }

        private void ChangeText(string t)
        {
            if (w6_in.InvokeRequired)
            {
                console.Invoke((MethodInvoker)delegate {console.Text += t + "\r\n"; });
            }
            else
            {
                console.Text += t + "\r\n";
            }


        }

        private void wyslij_w_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                if (agents[i].getId() == 1)
                {
                    string w1_inValue = w1_in.Text;
                    string w1_outValue = w1_out.Text;
                    string w1_f1Value = w1_f1.Text;
                    string w1_f2Value = w1_f2.Text;
                    string w1_f1convValue = w1_f1conv.Text;
                    string w1_f2convValue = w1_f2conv.Text;

                    if (w1_inValue != "" & w1_outValue != "" & w1_f1Value != "" & w1_f2Value != "")
                        agents[i].SendSettings(w1_inValue, w1_outValue, w1_f1Value, w1_f2Value);
                    if (w1_f1convValue != "" & w1_f2convValue != "")
                        agents[i].SendConvert(w1_f1convValue, w1_f2convValue);
                }

                else if (agents[i].getId() == 2)
                {
                    string w2_inValue = w2_in.Text;
                    string w2_outValue = w2_out.Text;
                    string w2_f1Value = w2_f1.Text;
                    string w2_f2Value = w2_f2.Text;
                    string w2_f1convValue = w2_f1conv.Text;
                    string w2_f2convValue = w2_f2conv.Text;

                    if (w2_inValue != "" & w2_outValue != "" & w2_f1Value != "" & w2_f2Value != "")
                        agents[i].SendSettings(w2_inValue, w2_outValue, w2_f1Value, w2_f2Value);
                    if (w2_f1convValue != "" & w2_f2convValue != "")
                        agents[i].SendConvert(w2_f1convValue, w2_f2convValue);
                }

                else if (agents[i].getId() == 3)
                {

                    string w3_inValue = w3_in.Text;
                    string w3_outValue = w3_out.Text;
                    string w3_f1Value = w3_f1.Text;
                    string w3_f2Value = w3_f2.Text;
                    string w3_f1convValue = w3_f1conv.Text;
                    string w3_f2convValue = w3_f2conv.Text;

                    if (w3_inValue != "" & w3_outValue != "" & w3_f1Value != "" & w3_f2Value != "")
                        agents[i].SendSettings(w3_inValue, w3_outValue, w3_f1Value, w3_f2Value);
                    if (w3_f1convValue != "" & w3_f2convValue != "")
                        agents[i].SendConvert(w3_f1convValue, w3_f2convValue);
                }

                else if (agents[i].getId() == 4)
                {

                    string w4_inValue = w4_in.Text;
                    string w4_outValue = w4_out.Text;
                    string w4_f1Value = w4_f1.Text;
                    string w4_f2Value = w4_f2.Text;
                    string w4_f1convValue = w4_f1conv.Text;
                    string w4_f2convValue = w4_f2conv.Text;

                    if (w4_inValue != "" & w4_outValue != "" & w4_f1Value != "" & w4_f2Value != "")
                        agents[i].SendSettings(w4_inValue, w4_outValue, w4_f1Value, w4_f2Value);
                    if (w4_f1convValue != "" & w4_f2convValue != "")
                        agents[i].SendConvert(w4_f1convValue, w4_f2convValue);
                }

                else if (agents[i].getId() == 5)
                {

                    string w5_inValue = w5_in.Text;
                    string w5_outValue = w5_out.Text;
                    string w5_f1Value = w5_f1.Text;
                    string w5_f2Value = w5_f2.Text;
                    string w5_f1convValue = w5_f1conv.Text;
                    string w5_f2convValue = w5_f2conv.Text;

                    if (w5_inValue != "" & w5_outValue != "" & w5_f1Value != "" & w5_f2Value != "")
                        agents[i].SendSettings(w5_inValue, w5_outValue, w5_f1Value, w5_f2Value);
                    if (w5_f1convValue != "" & w5_f2convValue != "")
                        agents[i].SendConvert(w5_f1convValue, w5_f2convValue);
                }

                else if (agents[i].getId() == 6)
                {

                    string w6_inValue = w6_in.Text;
                    string w6_outValue = w6_out.Text;
                    string w6_f1Value = w6_f1.Text;
                    string w6_f2Value = w6_f2.Text;
                    string w6_f1convValue = w6_f1conv.Text;
                    string w6_f2convValue = w6_f2conv.Text;

                    if (w6_inValue != "" & w6_outValue != "" & w6_f1Value != "" & w6_f2Value != "")
                        agents[i].SendSettings(w6_inValue, w6_outValue, w6_f1Value, w6_f2Value);
                    if (w6_f1convValue != "" & w6_f2convValue != "")
                        agents[i].SendConvert(w6_f1convValue, w6_f2convValue);
                }

            }

        }

        private void wyslij_k_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].getId() == 1)
                {
                    string k1_f1Value = k1_f1.Text;
                    string k1_f2Value = k1_f2.Text;
                    string k1_modValue = k1_mod.Text;

                    if (k1_f1Value != "" & k1_f2Value != "" & k1_modValue != "")
                        clients[i].SendParameters(k1_f1Value, k1_f2Value, k1_modValue);
                }

                else if (clients[i].getId() == 2)
                {
                    string k2_f1Value = k2_f1.Text;
                    string k2_f2Value = k2_f2.Text;
                    string k2_modValue = k2_mod.Text;

                    if (k2_f1Value != "" & k2_f2Value != "" & k2_modValue != "")
                        clients[i].SendParameters(k2_f1Value, k2_f2Value, k2_modValue);
                }

                else if (clients[i].getId() == 3)
                {
                    string k3_f1Value = k3_f1.Text;
                    string k3_f2Value = k3_f2.Text;
                    string k3_modValue = k3_mod.Text;

                    if (k3_f1Value != "" & k3_f2Value != "" & k3_modValue != "")
                        clients[i].SendParameters(k3_f1Value, k3_f2Value, k3_modValue);
                }



            }

        }

        private void Rozlacz_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                if (agents[i].getId() == 1)
                {
                    string w1_inValue = w1_in.Text;
                    string w1_outValue = w1_out.Text;
                    string w1_f1Value = w1_f1.Text;
                    string w1_f2Value = w1_f2.Text;

                    if (w1_inValue != "" & w1_outValue != "" & w1_f1Value != "" & w1_f2Value != "")
                        agents[i].SendDisconnect(w1_inValue, w1_outValue, w1_f1Value, w1_f2Value);
                }

                else if (agents[i].getId() == 2)
                {
                    string w2_inValue = w2_in.Text;
                    string w2_outValue = w2_out.Text;
                    string w2_f1Value = w2_f1.Text;
                    string w2_f2Value = w2_f2.Text;

                    if (w2_inValue != "" & w2_outValue != "" & w2_f1Value != "" & w2_f2Value != "")
                        agents[i].SendDisconnect(w2_inValue, w2_outValue, w2_f1Value, w2_f2Value );
                }

                else if (agents[i].getId() == 3)
                {

                    string w3_inValue = w3_in.Text;
                    string w3_outValue = w3_out.Text;
                    string w3_f1Value = w3_f1.Text;
                    string w3_f2Value = w3_f2.Text;

                    if (w3_inValue != "" & w3_outValue != "" & w3_f1Value != "" & w3_f2Value != "")
                        agents[i].SendDisconnect(w3_inValue, w3_outValue, w3_f1Value, w3_f2Value);
                }

                else if (agents[i].getId() == 4)
                {

                    string w4_inValue = w4_in.Text;
                    string w4_outValue = w4_out.Text;
                    string w4_f1Value = w4_f1.Text;
                    string w4_f2Value = w4_f2.Text;

                    if (w4_inValue != "" & w4_outValue != "" & w4_f1Value != "" & w4_f2Value != "")
                        agents[i].SendDisconnect(w4_inValue, w4_outValue, w4_f1Value, w4_f2Value);
                }

                else if (agents[i].getId() == 5)
                {

                    string w5_inValue = w5_in.Text;
                    string w5_outValue = w5_out.Text;
                    string w5_f1Value = w5_f1.Text;
                    string w5_f2Value = w5_f2.Text;

                    if (w5_inValue != "" & w5_outValue != "" & w5_f1Value != "" & w5_f2Value != "")
                        agents[i].SendDisconnect(w5_inValue, w5_outValue, w5_f1Value, w5_f2Value);
                }

                else if (agents[i].getId() == 6)
                {

                    string w6_inValue = w6_in.Text;
                    string w6_outValue = w6_out.Text;
                    string w6_f1Value = w6_f1.Text;
                    string w6_f2Value = w6_f2.Text;

                    if (w6_inValue != "" & w6_outValue != "" & w6_f1Value != "" & w6_f2Value != "")
                        agents[i].SendDisconnect(w6_inValue, w6_outValue, w6_f1Value, w6_f2Value);
                }

            }

        }

        private void exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void wyczysc_w_Click(object sender, EventArgs e)
        {
            w1_in.Text="";
            w1_out.Text = "";
            w1_f1.Text = "";
            w1_f2.Text = "";
            w1_f2conv.Text = "";
            w1_f1conv.Text = "";
        
            w2_in.Text = "";
            w2_out.Text = "";
            w2_f1.Text = "";
            w2_f2.Text = "";
            w2_f1conv.Text = "";
            w2_f2conv.Text = "";
            
            
            w3_in.Text = "";
            w3_out.Text = "";
            w3_f1.Text = "";
            w3_f2.Text = "";
            w3_f1conv.Text = "";
            w3_f2conv.Text = "";
            
            w4_in.Text = "";
            w4_out.Text = "";
            w4_f1.Text = "";
            w4_f2.Text = "";
            w4_f1conv.Text = "";
            w4_f2conv.Text = "";

            w5_in.Text = "";
            w5_out.Text = "";
            w5_f1.Text = "";
            w5_f2.Text = "";
            w5_f1conv.Text = "";
            w5_f2conv.Text = "";

            w6_in.Text = "";
            w6_out.Text = "";
            w6_f1.Text = "";
            w6_f2.Text = "";
            w6_f1conv.Text = "";
            w6_f2conv.Text = "";
        }

        private void wyczysc_k_Click(object sender, EventArgs e)
        {
            k1_f1.Text="";
            k1_f2.Text = "";
            k1_mod.Text = "";
            k2_f1.Text = "";
            k2_f2.Text = "";
            k2_mod.Text = "";
            k3_f1.Text = "";
            k3_f2.Text = "";
            k3_mod.Text = "";

        }

       }

}