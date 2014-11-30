using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Wezel_Sieciowy1
{
    class SiecControl
    {
        private TcpClient clientLRM = null;     // tak naprawde jest to polaczenie do chmury kablowej, na oddzielnym porcie do komunikacji z LRM sasiada.
        private TcpClient clientControl = null;

        public StreamReader inputLRM = null;   //2 strumienie tekstowe, do komunikacji z LRM-ami, przez Cloud
        public StreamWriter outputLRM = null;
        public StreamReader inputControl = null;   //2 strumienie tekstowe, do komunikacji z Gienerałem = NCC, CC, RC, bezpośrednio.
        public StreamWriter outputControl = null;

        private NetworkStream strumienLRM = null;
        private NetworkStream strumienControl = null;

        private int portLRM = 0;
        private int portControl = 0;
        public int idW = 0;
        private String ipCloud = null;
        private String ipControl = null;

        public SiecControl(String ipLRM, String ipControl, int portLRM, int portControl, int idW)
        {
            this.ipCloud = ipLRM;
            this.ipControl = ipControl;
            this.portLRM = portLRM;
            this.portControl = portControl;
            this.idW = idW;
        }

        public Boolean connectService(Boolean service) //System Zarządzania, CloudCable
        {
            try //uruchamianie polaczenie do SZ lub CC
            {
                if (service)
                {
                    clientLRM = new TcpClient();
                    clientLRM.Connect(ipCloud, portLRM);
                }
                else
                {
                    clientControl = new TcpClient();
                    clientControl.Connect(ipControl, portControl);
                }

                Console.WriteLine("Trwa łączenie...");

            }
            catch (SocketException e) { Console.WriteLine("\n\nBłąd połączenia.\n\n" + e.StackTrace); return false; }

            try
            {
                if (service) //uruchamianie strumieni
                {
                    strumienLRM = clientLRM.GetStream();
                    inputLRM = new StreamReader(strumienLRM);
                    outputLRM = new StreamWriter(strumienLRM);

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
                    inputLRM.Close();
                    outputLRM.Close();
                    strumienLRM.Close();
                    clientLRM.Close();
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
