using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cloud
{
    class Point
    {
        public int id;
        public char type;
        public NetworkStream stream;
        CableCloud cablecloud;
        Socket sockett;
        public bool closed;

        public Point(Socket socket, CableCloud cloud)
        {
            stream = new NetworkStream(socket);
            sockett = socket;
            cablecloud = cloud;
            closed = false;
        }

        public Byte[] receive()
        {
            try
            {
                Byte[] data = new Byte[1024];
                Int32 bytes = stream.Read(data, 0, data.Length);
                return data;
            }
                        catch (IOException)
            {
            }
            cablecloud.RemovePoint(this);
            this.closed = true;
            return null;
        }

        public void close()
        {
            try
            {
                sockett.Close();
                stream.Close();
            }
            catch (IOException)
            {
                Console.WriteLine("Blad zamkniecia klienta/wezla");
            }
            finally
            {
                sockett = null;
                stream = null;
            }
        }

        public void receiveLogin()
        {
            String message = String.Empty;
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            

            try
            {
                message = reader.ReadLine();
            }

            catch (IOException)
            {
                Console.WriteLine("blad odczytu id od wezla/klienta");
            }

            Console.WriteLine(message);

            string[] tab = message.Split(' ');

            id = Convert.ToInt32(tab[1]);
            type = Convert.ToChar(tab[2]);
            if (tab[0] != "login")
                System.Console.Write("trzeba najpierw wyslac login");

            if ((type!= 'c') && (type!='n'))
                System.Console.Write("nieprawidlowy typ punktu");

            writer.WriteLine("confirmation");
            writer.Flush();

        }

        public void send(Byte[] data)
        {
            Int32 datasize = data.Length;
            stream.Write(data, 0, datasize);
        }


        private int[] whereToSend(int Node, int Port, char Type)
        {
            int[] wts= new int[3];
            Link lnk = cablecloud.links.Find(link => (link.startPort == Port && link.startNode == Node && link.typeStartNode == Type));
            try
            {
                wts[0] = lnk.endNode;
                wts[1] = lnk.endPort;
                wts[2] = Convert.ToInt16(lnk.typeEndNode);
            }
            catch (NullReferenceException)
            {
                wts[0] = 0;
                wts[1] = 0;
                wts[2] = 0;
            }
            return wts;
        }

        private Point findPoint(int Id, char Type)
        {
                return cablecloud.points.Find(point => ((point.id == Id) && (point.type == Type)));

            }

        public int[] header(String deserialized)
        {
            int[] tab = new int[3];
            int Id;
            int Port = Convert.ToInt32(deserialized.Substring(0,2));
            char type;

            if (Convert.ToInt32(deserialized.Substring(2, 2)) != 0)
            {
                Id = Convert.ToInt32(deserialized.Substring(2, 2));
                type = 'n';
            }
            else
            {
                Id = Convert.ToInt32(deserialized.Substring(8, 2));
                type = 'c';
            }
            tab[0] = Id;
            tab[1] = Port;
            tab[2] = Convert.ToInt32(type);
            return tab;
        }



        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

        private String ReplaceHeader(String message, int port)
        {
            StringBuilder mess = new StringBuilder(message);

            mess[1] = Convert.ToChar(Convert.ToString(port));
            message = mess.ToString();
            return message;


        }


        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }


        public void Run()
        {
            receiveLogin();
            while (!closed)
            {
                Byte[] message=receive();
                if (closed == false)
                {
                    String deserialized = (String)ByteArrayToObject(message);

                    int[] head = header(deserialized);

                    Console.WriteLine("\nodebrano wiadomosc: ");

                    Console.WriteLine(deserialized);

                    Console.WriteLine("od: " + Convert.ToChar(head[2]) + head[0] + " z portu " + head[1]);

                    int[] wts = whereToSend(head[0], head[1], Convert.ToChar(head[2]));
                    if (wts[0] != 0)
                    {
                        deserialized = ReplaceHeader(deserialized, wts[1]);

                        Console.WriteLine("\nWysylamy na port: " + wts[1]);
                        try
                        {
                            cablecloud.send(findPoint(wts[0], Convert.ToChar(wts[2])), ObjectToByteArray(deserialized));
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("nie można wysłać wiadomości!");
                        }
                    }
                    else
                        Console.WriteLine("\n nie mozna odnalezc wezla, do ktorego powinna zostac wyslana wiadomosc!");
                }
            }
        }

    }
}
