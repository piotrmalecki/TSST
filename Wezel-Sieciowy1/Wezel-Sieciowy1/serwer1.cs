using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication2
{
    public class Serwer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("kurwa kutas");
            try
            {
                IPAddress ipA = IPAddress.Parse("127.0.0.1");

                TcpListener nasluch = new TcpListener(ipA, 10000); //inicjalizacja listenera
                nasluch.Start();

                Console.WriteLine("Serwer uruchomiony na porcie 10000");
                Console.WriteLine("lokalnym end pointem jest " + nasluch.LocalEndpoint);
                Console.WriteLine("czekam na polaczenie...");

                Socket gniazdo = nasluch.AcceptSocket();
                Console.WriteLine("polaczenie nawiazane z: " + gniazdo.RemoteEndPoint);

                byte[] b = new byte[100];
                int k = gniazdo.Receive(b);

                Console.WriteLine("odebralem..");
                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(b[i]));

                ASCIIEncoding asen = new ASCIIEncoding();
                gniazdo.Send(asen.GetBytes("string odebrany madafaka."));
                Console.WriteLine("\n wyslalem potwierdzenie dziwko");

                gniazdo.Close();
                nasluch.Stop();



            }
            catch (Exception e) { Console.WriteLine("error: " + e.StackTrace); }



        }


    }
}
