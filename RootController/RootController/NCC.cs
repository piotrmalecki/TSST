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

namespace RootController
{
    public class NCC
    {

        private TcpListener listener;
        private Socket socket;
        private ClientService clientService;
        private List<ClientService> clients = new List<ClientService>();
        private List<Directory> direct = new List<Directory>();
        RC rc = new RC();
        private static int callID = 0;



        public NCC()
        {
             
             FileStream stream = new FileStream("Directory", FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            while (reader.EndOfStream == false)
            {
                string data;
                data = reader.ReadLine();
                string[] info = data.Split(' ');
                direct.Add(new Directory(info[0], info[1]));
            }

            reader.Close();


            IPAddress localaddr = IPAddress.Parse("127.0.0.1");
            try
            {
                listener = new TcpListener(localaddr, 40001);
                listener.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine("Błąd uruchamiania NCC");
            }

            Console.WriteLine("NCC uruchomiony");
            Thread thread = new Thread(Run);
            thread.Start();
        }

        void Run()
        {
            while (true)
            {
                try
                {
                    socket = listener.AcceptSocket();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Nie można połączyć z klientem");
                }
                
                if (socket.Connected)
                {

                    Console.WriteLine("Klient połączony");
                    clientService = new ClientService(socket, this, callID++);
                    addClientService(clientService);
                }
            }
        }



        private void addClientService(ClientService clientService)
        {
            try
            {
                clientService.init();
                clients.Add(clientService);

            }
            catch (IOException)
            {
                Console.WriteLine("Błąd strumienia wejścia/wyjścia");
            }
            Thread service = new Thread(new ThreadStart(clientService.Run));
            service.Start();
        }


        public void removeClientService(ClientService clientService)
        {
            Console.WriteLine("Zakonczono serwis klienta " + clientService.getId().ToString());
            clientService.close();
            clients.Remove(clientService);

        }

        void Connect(String idFrom, String idTo, int callID)
        {
            rc.ZnajdzDroge(idFrom, idTo);

        }




    }
}

