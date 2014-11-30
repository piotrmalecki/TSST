using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    class Klient
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("laczenie..");

                tcpclnt.Connect("127.0.0.1", 10000);

                Console.WriteLine("connected");
                Console.Write("pisz co przeslac: ");

                String wejscie = Console.ReadLine();
                Stream strumien = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(wejscie);
                Console.WriteLine("wysylam...");

                strumien.Write(ba, 0, ba.Length);

                byte[] odbior = new byte[100]; //odbior potwierdzenia;
                int k = strumien.Read(odbior,0,100);

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(odbior[i]));

                tcpclnt.Close();

                Console.WriteLine();

            }
            catch (Exception e) { Console.WriteLine("chuj"); }


        }
    }
}
