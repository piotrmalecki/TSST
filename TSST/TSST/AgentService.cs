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
using System.Diagnostics;

namespace TSST
{

    public class AgentService : Service
    {
        private ComboBox w1_in;
        private ComboBox w1_out;
        private ComboBox w2_in;
        private ComboBox w2_out;
        private ComboBox w3_in;
        private ComboBox w3_out;
        private ComboBox w4_in;
        private ComboBox w4_out;
        private ComboBox w5_in;
        private ComboBox w5_out;
        private ComboBox w6_in;
        private ComboBox w6_out;
        private ComboBox w1_f1;
        private ComboBox w1_f2;
        private ComboBox w2_f1;
        private ComboBox w2_f2;
        private ComboBox w3_f1;
        private ComboBox w3_f2;
        private ComboBox w4_f1;
        private ComboBox w4_f2;
        private ComboBox w5_f1;
        private ComboBox w5_f2;
        private ComboBox w6_f1;
        private ComboBox w6_f2;
        private ComboBox w1_f1conv;
        private ComboBox w1_f2conv;
        private ComboBox w2_f1conv;
        private ComboBox w2_f2conv;
        private ComboBox w3_f1conv;
        private ComboBox w3_f2conv;
        private ComboBox w4_f1conv;
        private ComboBox w4_f2conv;
        private ComboBox w5_f1conv;
        private ComboBox w5_f2conv;
        private ComboBox w6_f1conv;
        private ComboBox w6_f2conv;
        private Button wyslij_w;
        private Button rozlacz;
        private Button wyczysc_w;

        public AgentService(Socket appSocket, ManagementApp managementApp, TextBox console, 
            ComboBox w1_in, ComboBox w1_out, ComboBox w2_in, ComboBox w2_out, ComboBox w3_in, ComboBox w3_out, 
            ComboBox w4_in, ComboBox w4_out, ComboBox w5_in, ComboBox w5_out, ComboBox w6_in, ComboBox w6_out, 
            ComboBox w1_f1, ComboBox w1_f2, ComboBox w2_f1, ComboBox w2_f2, ComboBox w3_f1, ComboBox w3_f2, 
            ComboBox w4_f1, ComboBox w4_f2, ComboBox w5_f1, ComboBox w5_f2, ComboBox w6_f1, ComboBox w6_f2,
            ComboBox w1_f1conv, ComboBox w1_f2conv, ComboBox w2_f1conv, ComboBox w2_f2conv, ComboBox w3_f1conv, ComboBox w3_f2conv,
            ComboBox w4_f1conv, ComboBox w4_f2conv, ComboBox w5_f1conv, ComboBox w5_f2conv, ComboBox w6_f1conv, ComboBox w6_f2conv, 
            Button wyslij_w, Button rozlacz, Button wyczysc_w)
        {
            this.managementApp = managementApp;
            this.appSocket = appSocket;
            this.console = console;
            this.w1_in = w1_in;
            this.w1_out = w1_out;
            this.w2_in = w2_in;
            this.w2_out = w2_out;
            this.w3_in = w3_in;
            this.w3_out = w3_out;
            this.w4_in = w4_in;
            this.w4_out = w4_out;
            this.w5_in = w5_in;
            this.w5_out = w5_out;
            this.w6_in = w6_in;
            this.w6_out = w6_out;
            this.w1_f1 = w1_f1;
            this.w1_f2 = w1_f2;
            this.w2_f1 = w2_f1;
            this.w2_f2 = w2_f2;
            this.w3_f1 = w3_f1;
            this.w3_f2 = w3_f2;
            this.w4_f1 = w4_f1;
            this.w4_f2 = w4_f2;
            this.w5_f1 = w5_f1;
            this.w5_f2 = w5_f2;
            this.w6_f1 = w6_f1;
            this.w6_f2 = w6_f2;
            this.w1_f1conv = w1_f1conv;
            this.w1_f2conv = w1_f2conv;
            this.w2_f1conv = w2_f1conv;
            this.w2_f2conv = w2_f2conv;
            this.w3_f1conv = w3_f1conv;
            this.w3_f2conv = w3_f2conv;
            this.w4_f1conv = w4_f1conv;
            this.w4_f2conv = w4_f2conv;
            this.w5_f1conv = w5_f1conv;
            this.w5_f2conv = w5_f2conv;
            this.w6_f1conv = w6_f1conv;
            this.w6_f2conv = w6_f2conv;
            this.wyslij_w = wyslij_w;
            this.rozlacz = rozlacz;
            this.wyczysc_w = wyczysc_w;
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
                    
                        ChangeText("Agent: " + command + " " + id);
                        wyslij_w.Invoke((MethodInvoker)delegate { wyslij_w.Enabled = true; });
                        rozlacz.Invoke((MethodInvoker)delegate { rozlacz.Enabled = true; });
                        wyczysc_w.Invoke((MethodInvoker)delegate { wyczysc_w.Enabled = true; });
                        if (id == "1")
                        {
                            w1_in.Invoke((MethodInvoker)delegate { w1_in.Enabled = true; });
                            w1_out.Invoke((MethodInvoker)delegate { w1_out.Enabled = true; });
                            w1_f1.Invoke((MethodInvoker)delegate { w1_f1.Enabled = true; });
                            w1_f2.Invoke((MethodInvoker)delegate { w1_f2.Enabled = true; });
                            w1_f1conv.Invoke((MethodInvoker)delegate { w1_f1conv.Enabled = true; });
                            w1_f2conv.Invoke((MethodInvoker)delegate { w1_f2conv.Enabled = true; });
                        }
                        else if (id == "2")
                        {
                            w2_in.Invoke((MethodInvoker)delegate { w2_in.Enabled = true; });
                            w2_out.Invoke((MethodInvoker)delegate { w2_out.Enabled = true; });
                            w2_f1.Invoke((MethodInvoker)delegate { w2_f1.Enabled = true; });
                            w2_f2.Invoke((MethodInvoker)delegate { w2_f2.Enabled = true; });
                            w2_f1conv.Invoke((MethodInvoker)delegate { w2_f1conv.Enabled = true; });
                            w2_f2conv.Invoke((MethodInvoker)delegate { w2_f2conv.Enabled = true; });
                        }
                        else if (id == "3")
                        {
                            w3_in.Invoke((MethodInvoker)delegate { w3_in.Enabled = true; });
                            w3_out.Invoke((MethodInvoker)delegate { w3_out.Enabled = true; });
                            w3_f1.Invoke((MethodInvoker)delegate { w3_f1.Enabled = true; });
                            w3_f2.Invoke((MethodInvoker)delegate { w3_f2.Enabled = true; });
                            w3_f1conv.Invoke((MethodInvoker)delegate { w3_f1conv.Enabled = true; });
                            w3_f2conv.Invoke((MethodInvoker)delegate { w3_f2conv.Enabled = true; });
                        }
                        else if (id == "4")
                        {
                            w4_in.Invoke((MethodInvoker)delegate { w4_in.Enabled = true; });
                            w4_out.Invoke((MethodInvoker)delegate { w4_out.Enabled = true; });
                            w4_f1.Invoke((MethodInvoker)delegate { w4_f1.Enabled = true; });
                            w4_f2.Invoke((MethodInvoker)delegate { w4_f2.Enabled = true; });
                            w4_f1conv.Invoke((MethodInvoker)delegate { w4_f1conv.Enabled = true; });
                            w4_f2conv.Invoke((MethodInvoker)delegate { w4_f2conv.Enabled = true; });
                        }

                        else if (id == "5")
                        {
                            w5_in.Invoke((MethodInvoker)delegate { w5_in.Enabled = true; });
                            w5_out.Invoke((MethodInvoker)delegate { w5_out.Enabled = true; });
                            w5_f1.Invoke((MethodInvoker)delegate { w5_f1.Enabled = true; });
                            w5_f2.Invoke((MethodInvoker)delegate { w5_f2.Enabled = true; });
                            w5_f1conv.Invoke((MethodInvoker)delegate { w5_f1conv.Enabled = true; });
                            w5_f2conv.Invoke((MethodInvoker)delegate { w5_f2conv.Enabled = true; });
                        }
                        else if (id == "6")
                        {
                            w6_in.Invoke((MethodInvoker)delegate { w6_in.Enabled = true; });
                            w6_out.Invoke((MethodInvoker)delegate { w6_out.Enabled = true; });
                            w6_f1.Invoke((MethodInvoker)delegate { w6_f1.Enabled = true; });
                            w6_f2.Invoke((MethodInvoker)delegate { w6_f2.Enabled = true; });
                            w6_f1conv.Invoke((MethodInvoker)delegate { w6_f1conv.Enabled = true; });
                            w6_f2conv.Invoke((MethodInvoker)delegate { w6_f2conv.Enabled = true; });
                        }

                        send(Protocol.CONF);
                        ChangeText("Wysłano: confirmation do agenta " + id);
                    }

                
                else if (command.Equals(Protocol.NULLCOMMAND))
                {
                    ChangeText("Błąd odczytu danych od agenta " + id);
                  
                    break;
                }



                else if (command.Equals(Protocol.SET_RSP))
                {
                  
                    ChangeText("Agent " + id + " : " + command);
                }

                else if (command.Equals(Protocol.DISCONNECT_RSP))
                {

                    ChangeText("Agent " + id + " : " + command);
                }


                else if (command.Equals(Protocol.ERROR))
                {

                    ChangeText("Agent " + id + " : " + command);
                }

            }
            managementApp.removeAgentService(this);
        }

        public void SendSettings(string s1, string s2, string s3, string s4)
        {

            send(Protocol.SET + " " + s1 + " " + s2 + " " + s3 + " " + s4);
            ChangeText("Wysłano: set do agenta " + id);
        }

        public void SendDisconnect(string s1, string s2, string s3, string s4)
        {

            send(Protocol.DISCONNECT + " " + s1 + " " + s2 + " " + s3 + " " + s4);
            ChangeText("Wysłano: disconnect do agenta " + id);
        }

        public void SendConvert(string s1, string s2)
        {

            send(Protocol.CONVERT + " " + s1 + " " + s2);
            ChangeText("Wysłano: convert do agenta " + id);
        }




        }
    
}


