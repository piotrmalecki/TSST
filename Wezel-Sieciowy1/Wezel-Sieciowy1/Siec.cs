using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Wezel_Sieciowy1
{
    public class Siec
    {
        public TcpClient clientCloud = null; //klienty do logowania do SZ i CableCloud
        public TcpClient clientControl = null;

        public StreamReader inputCloud = null; //2 strumienie tekstowe, do komunikacji z CableCloud
        public StreamWriter outputCloud = null;
        public StreamReader inputControl = null; //2 strumienie tekstowe, do komunikacji z SZ
        public StreamWriter outputControl = null;

        public NetworkStream strumienCloud = null;
        public NetworkStream strumienControl = null;

        private IPAddress ipA = null;
        public static Boolean ifConn = false;
        private int portCloud = 0;
        private int portControl = 0;
        public int idW = 0;

        public Siec(String ip, int portCloud, int portControl, int idW)
        {
            this.ipA = IPAddress.Parse(ip);
            this.portCloud = portCloud;
            this.portControl = portControl;
            this.idW = idW;
        }

        public Boolean connectService(Boolean service) //System Zarządzania, CloudCable
        {
            try //uruchamianie polaczenie do SZ lub CC
            {
                if (service)
                {
                    clientCloud = new TcpClient();
                    clientCloud.Connect(ipA, portCloud);
                }
                else
                {
                    clientControl = new TcpClient();
                    clientControl.Connect(ipA, portControl);
                }

                Console.WriteLine("Trwa łączenie...");
            }
            catch (SocketException e) { Console.WriteLine("\n\nBłąd połączenia.\n\n" + e.StackTrace); return false; }


            try
            {
                if (service) //uruchamianie strumieni
                {
                    strumienCloud = clientCloud.GetStream();
                    inputCloud = new StreamReader(strumienCloud);
                    outputCloud = new StreamWriter(strumienCloud);

                }
                else
                {
                    strumienControl = clientControl.GetStream();
                    inputControl = new StreamReader(strumienControl);
                    outputControl = new StreamWriter(strumienControl);
                }

                return (true);

            }
            catch (IOException e) { Console.WriteLine("\nBłąd strumienia we/wy.\n\n" + e.StackTrace); return false; }
        }

        public void DisconnectService(Boolean service)
        {
            if (service)
            {
                try
                {
                    inputCloud.Close();
                    outputCloud.Close();
                    strumienCloud.Close();
                    clientCloud.Close();
                }
                catch (SocketException e) { }
                catch (EncoderFallbackException e) { }
                catch (NullReferenceException e) { }
            }
            else
            {
                try
                {
                    inputControl.Close();
                    outputControl.Close();
                    strumienControl.Close();
                    clientControl.Close();
                }
                catch (SocketException e) { }
                catch (EncoderFallbackException e) { }
                catch (NullReferenceException e) { }
            }
        }



    }
}
