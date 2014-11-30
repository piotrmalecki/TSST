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
    class Service
    {
        protected CC cc;
        protected Socket socket;
        protected NetworkStream stream;
        protected StreamWriter output;
        protected StreamReader input;
        public String id;


        protected Service()
        {
        }


        public void init()
        {

            stream = new NetworkStream(socket);
            output = new StreamWriter(stream);
            input = new StreamReader(stream);
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
                socket.Close();
            }
            catch (IOException)
            {
                Console.WriteLine(DateTime.Now + " Błąd zamknięcia serwisu węzła " + id);
            }
            finally
            {
                output = null;
                input = null;
                socket = null;
            }
        }

        public int getId()
        {
            return Convert.ToInt32(id);
        }


      
    }
}
