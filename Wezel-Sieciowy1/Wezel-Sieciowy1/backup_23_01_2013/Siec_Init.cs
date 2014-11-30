using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Wezel_Sieciowy1
{
    public class Siec_Init
    {
        public TcpClient clientCloud    = null; //klienty do logowania do SZ i CableCloud
        private TcpClient clientSZ      = null;

        public StreamReader inputCloud  = null; //2 strumienie tekstowe, do komunikacji z CableCloud
        public StreamWriter outputCloud = null;
        public StreamReader inputSZ     = null; //2 strumienie tekstowe, do komunikacji z SZ
        public StreamWriter outputSZ    = null;

        public NetworkStream strumienCloud  = null;
        public NetworkStream strumienSZ     = null;

        
        //public Socket gniazdo = null;

        private IPAddress ipA = null;
        public static Boolean ifConn = false;
        private int portSZ      = 0;
        private int portCC      = 0;
        public int idW          = 0;

        public Siec_Init(String ip, int portSZ, int portCC, int idW)
        {
            this.ipA = IPAddress.Parse(ip);
            this.portSZ = portSZ;
            this.portCC = portCC;
            this.idW = idW;
        }

        public Boolean connectService(Boolean service) //System Zarządzania, CloudCable
        {
            try //uruchamianie polaczenie do SZ lub CC
            {
                if (service)
                {
                    clientSZ = new TcpClient();
                    clientSZ.Connect(ipA, portSZ);
                }
                else
                {
                    clientCloud = new TcpClient();
                    clientCloud.Connect(ipA, portCC);
                }

                Console.WriteLine("Trwa łączenie...");
                
            }
            catch (SocketException e) { Console.WriteLine("\n\nBłąd połączenia.\n\n" + e.StackTrace); return false; }


            try
            {
                if (service) //uruchamianie strumieni
                {
                    strumienSZ = clientSZ.GetStream();
                    inputSZ = new StreamReader(strumienSZ);
                    outputSZ = new StreamWriter(strumienSZ);

                }
                else
                {   
                    strumienCloud = clientCloud.GetStream();
                    inputCloud = new StreamReader(strumienCloud);
                    outputCloud = new StreamWriter(strumienCloud);
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
                    inputSZ.Close();
                    outputSZ.Close();
                    strumienSZ.Close();
                    clientSZ.Close();
                }
                catch (SocketException e) { }
                catch (EncoderFallbackException e) { }
                catch (NullReferenceException e) { }
            }
            else
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
        }

        //public Boolean cloudListen()
        //{
        //    Boolean answer = false;
        //    if (ifConn == false)
        //    {
        //        try
        //        {
        //            ipA = IPAddress.Parse("127.0.0.1");
        //            listenC = new TcpListener(ipA, (10000 + idW)); //inicjalizacja listenera polaczen chmury
        //            if (listenC != null)
        //            {
        //                listenC.Start();
        //                ifConn = true;
        //            }
        //            else
        //                Console.WriteLine("pusty listenC");
        //            answer = true;
        //        }
        //        catch (SocketException e) { Console.WriteLine("Uruchomienie nasłuchu chmury na porcie " + (10000 + idW) + " niemożliwe.\n" + e.StackTrace); answer = false; }
        //    }
        //    else
        //        answer = true;
        //    return answer;

        //}


        //public void Service()
        //{
        //    Console.WriteLine("Nasłuch połączeń przychodzących uruchomiony na porcie " + (10000 + idW) + ".");
        //    Console.WriteLine("Lokalnym end pointem jest " + listenC.LocalEndpoint);
        //    Console.WriteLine("Czekam na polaczenia...");
        //    if (connect)
        //    {
        //        Console.WriteLine(connect);
        //        Listen_Connection();
        //    }
        //}

        //private void Listen_Connection()
        //{
        //    while (connect == true)
        //    {
        //        gniazdo = listenC.AcceptSocket();
        //        Console.WriteLine("Połączenie nawiązane z: " + gniazdo.RemoteEndPoint);
                
        //        break;
        //    }
        //}


    }
}
