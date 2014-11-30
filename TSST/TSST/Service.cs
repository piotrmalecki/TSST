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
    public class Service
    {
        protected ManagementApp managementApp;
        protected Socket appSocket;
        protected NetworkStream appStream;
        protected StreamWriter output;
        protected StreamReader input;
        protected String id;
        protected TextBox console;

        public Service()
        {
        }


        public void init()
        {

            appStream = new NetworkStream(appSocket);
            output = new StreamWriter(appStream);
            input = new StreamReader(appStream);
        }

        protected String receive()
        {
            try
            {
                return input.ReadLine();
            }
            catch (IOException)
            {
            }
            return Protocol.NULLCOMMAND;
        }

         protected void send(String command)
        {
            output.WriteLine(command);
            output.Flush();
        }


         public void close()
         {
             try
             {
                 output.Close();
                 input.Close();
                 appSocket.Close();
             }
             catch (IOException)
             {
                 ChangeText("Błąd zamknięcia serwisu agenta/klienta " + id);
             }
             finally
             {
                 output = null;
                 input = null;
                 appSocket = null;
             }
         }

         protected void ChangeText(string t)
        {
            if (console.InvokeRequired)
            {
                console.Invoke((MethodInvoker)delegate { console.Text += t + "\r\n"; });
            }
            else
            {
                console.Text += t + "\r\n";
            }
        }

        public int getId()
        {
            return Convert.ToInt32(id);
        }

        public String getName()
        {
            return id;
        }

    }
    
    }


