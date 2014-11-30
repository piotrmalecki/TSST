using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Agent
{

    public class Agent
    {
        private TcpClient tcpClient;
   
        private StreamWriter output;
        private StreamReader input;
        private int id;



        public Agent()
        {
            Console.WriteLine("Podaj ID:");
            id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Agent " +id);


            try
            {
                
                tcpClient = new TcpClient("127.0.0.1", 8888);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Nie można połączyć z aplikacją zarządzania");
                Close();
               
            }
                     
               
            Console.WriteLine("Połączony z aplikacją zarządzania");

            try
            {
                NetworkStream clientSockStream = tcpClient.GetStream();
                input = new StreamReader(clientSockStream);
                output = new StreamWriter(clientSockStream);
            }
            catch (IOException e)
            {
                Console.WriteLine("Błąd strumienia wejścia/wyjścia");
                Close();
            }
            Thread clientThread = new Thread(new ThreadStart(this.Run));
            clientThread.Start();

            send(Protocol.LOGIN + " " + id);
        }


        void Run()
        {
            while (true)
            {
                String command = receive();
                Console.WriteLine("Serwer: " + command);
                string[] tab = command.Split(' ');
                command = tab[0];

                if (command.Equals(Protocol.SET))
                {
                    send(Protocol.SET_RSP);
                    Console.WriteLine("Wyslano: SET_RSP");

                }
                else if (command.Equals(Protocol.NULLCOMMAND))
                {
                    break;
                }
            }
        }

     

        private void Close()
        {
            Console.WriteLine("wpisz c aby zamknąć program");
            {
                while (true)
                {
                    if (Console.ReadLine() == "c")
                    {
                        Environment.Exit(1);
                    }
                    else
                    {
                        Console.WriteLine("wpisz c aby zamknąć program");

                    }
                }
            }
        }

        private String receive()
        {
            try
            {
                return input.ReadLine();
            }
            catch (IOException e)
            {
                Console.WriteLine("Błąd odczytu danych od agenta" + id);
            }
            return Protocol.NULLCOMMAND; return input.ReadLine();
        }

        void send(String command)
        {
            if (output != null)
            {
                output.WriteLine(command);
                output.Flush();
            }
        }


    }



    public class Protocol
    {

        public const String LOGIN = "login";

        public const String SET = "set";

        public const String SET_RSP = "set_rsp";

        public const String CLOSE = "close";

        public const String NULLCOMMAND = "nullcommand";

        public const String ALIVE = "alive";
    }


    class Program
    {
        static void Main(string[] args)
        {
            new Agent();
        }
    }
}
