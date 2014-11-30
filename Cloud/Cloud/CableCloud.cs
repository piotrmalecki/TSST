using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Cloud
{
    class CableCloud
    {
        public List<Link> links = new List<Link>();
        public List<Point> points = new List<Point>();
        Thread cablethread;
        TcpListener listener;
        Socket socket;

        public CableCloud(String Filename)
        {
            
            FileStream stream;
            stream = new FileStream(Filename, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            while (reader.EndOfStream == false)
            {
                string data;
                data = reader.ReadLine();
                string[] info = data.Split(' ');
                links.Add(new Link(Convert.ToInt32(info[0]), Convert.ToInt32(info[1]), Convert.ToChar(info[2]), Convert.ToInt32(info[3]), Convert.ToInt32(info[4]), Convert.ToChar(info[5])));
            }

            reader.Close();

            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 10100);
                listener.Start();
            }

            catch (ArgumentNullException)
            {
                System.Console.WriteLine("błąd uruchamiania aplikacji");
            }


            cablethread = new Thread(run);
            cablethread.Start();
        }


        public void send(Point point, Byte[] mess)
        {
            point.send(mess);
            Console.Write("do: " + point.type + point.id);
            Console.WriteLine(" ");
        }

        public void NewPoint(Socket s)
        {
            Point p = new Point(s, this);
            Thread pt = new Thread(p.Run);
            pt.Start();
            points.Add(p);
        }

        public void RemovePoint(Point p)
        {
            p.close();
            points.Remove(p);
        }

        public void run()
        {
            Console.WriteLine("oczekiwanie na polaczenie od klienta lub węzła");
                while (true)
                {
                    socket = listener.AcceptSocket();
                   
                    if (socket.Connected)
                    {
                        NewPoint(socket);
                    }

                 }

                
        }
    }
}
