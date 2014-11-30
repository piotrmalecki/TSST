using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace Wezel_Sieciowy1
{
    public class Wezel
    {
        public static void Main(string[] args)
        {
            String ip;
            int portSZ = 0;
            int portCloud = 0;
            int portControl = 0;
            int portLRM = 0;

            int idW = 0;

            Console.Title = ("Węzeł Sieciowy");

            if (args.Length == 6)
            {
                idW = Convert.ToInt32(args[0]);
                ip = args[1];
                portSZ = Convert.ToInt32(args[2]);
                portCloud = Convert.ToInt32(args[3]);
                portControl = Convert.ToInt32(args[4]);
                portLRM = Convert.ToInt32(args[5]);
            }
            else 
            {
                Console.Write("Podaj ip Systemu Zarządzania lub wybierz zestaw: ");

                ip = Console.ReadLine();

                if (ip[0].Equals('g'))
                {
                    idW = Convert.ToInt32(ip.Substring(1, 1));
                    ip = "127.0.0.1";
                    portSZ = 10000;
                    portCloud = 10100;
                    portLRM = 10101;
                    portControl = 10102;
                }
                else
                {
                    Console.Write("Podaj port Systemu Zarządzania: ");
                    portSZ = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Podaj port Chmury Kablowej: ");
                    portCloud = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Podaj port Systemu Sterowania: ");
                    portControl = Convert.ToInt32(Console.ReadLine());
                }

            }

            
            Console.Title = ("Węzeł Sieciowy, Id: "+(idW.ToString()));  //ustawienie tekstu belki tytulowej

            Siec_Init Network = new Siec_Init(ip, portSZ, portCloud, idW);
            SiecControl NetworkC = new SiecControl(ip, ip, portLRM, portControl, idW);
                                                
            new Agent(Network);
            new AgentControl(NetworkC);


        }
    }
}
