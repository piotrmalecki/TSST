using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace Client1
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        bool a = true;
        bool b = false;
        String ConnectionMessage;
        // NetworkStream Management;
        NetworkStream Cloud;
        NetworkStream clientSockStreamCloud;
        sygnaleon eon;
        public static int ID_client;
        sygnaleon s;
        TextReader text;
        Int32 talker;
        ArrayList conf;
        private TcpListener appTcpListener_Cable;
        private Socket appSocket_Cable;
        public static int ID_client2;
        public string f1;
        public string f2;
        public string modulacja;
        private TcpClient tcpClient;
        private TcpClient tcpClientCloud;
        private StreamWriter outputCloud;
        private StreamReader inputCloud;
        private bool viaManagement = false;
        private String name;
        private StreamWriter output;
        private StreamReader input;
        private string DisConnectionMessage;
        public bool sendMsg = true;
        //listBox1.Text= Convert.ToString(ID_client2);
        //public Socket m_socListener;
        //id naszego aktualnego rozmowcy

        public Form1(string ID)
        {
            name = ID;
            InitializeComponent();
            switch (name)
            {
                case "Franek":
                ID_client2 = 1;
                break;
                case "Henio":
                ID_client2 = 2;
                break;
                case "Zenek":
                ID_client2 = 3;
                break;


            }
            //Int32[] conf=new Int32[1];
            //String path = @"D:\Dane.txt";
            //conf = TextReader.ReadTextFromFile(@"dane.txt");
         
            listBox1.Items.Add(name);

            //ID_client = 1; //conf[0];
            s = new sygnaleon();
            IPAddress ipadres = IPAddress.Parse("127.0.0.1");
            string host = "127.0.0.1";
            int port2 = 10100;
            IPAddress ipadres1 = IPAddress.Parse("127.0.0.1");
            string host1 = "127.0.0.1";
            int port;
            if (viaManagement)
            {
                port = 30001;
            }
            else
            {
                port = 40001;
            }
                try 
                {
                    tcpClient = new TcpClient(host1, port);
                }
              catch (SocketException e1)
                {
                     ChangeText("Nie udało się nawiązać połączenia z aplika");
                     MessageBox.Show(e1.ToString());
                }
               try
                {
              NetworkStream clientSockStream = tcpClient.GetStream();
              input = new StreamReader(clientSockStream);
              output = new StreamWriter(clientSockStream);
                }
            catch (IOException e2)
            {
                ChangeText("Błąd strumienia wejścia/wyjścia");

            }

               Thread clientThread = new Thread(new ThreadStart(this.Run));
               clientThread.Start();
        



            try
            {


                tcpClientCloud = new TcpClient(host, port2);

            }

            catch (SocketException e)
            {
                ChangeText("Nie udało się nawiązać połączenia z aplika");
                MessageBox.Show(e.ToString());

            }
            try
            {
                clientSockStreamCloud = tcpClientCloud.GetStream();
                inputCloud = new StreamReader(clientSockStreamCloud);
                outputCloud = new StreamWriter(clientSockStreamCloud);
            }
            catch (IOException e)
            {
                ChangeText("Błąd strumienia wejścia/wyjścia");

            }

            Thread appThread_cable = new Thread(new ThreadStart(this.Run_Cable));
            appThread_cable.Start();
 
            send(Protocol.LOGIN + " " + ID_client2+ " c", outputCloud);
        }
       
        void send(String command, StreamWriter output2)


        {
            if (output2 != null)
            {
                output2.WriteLine(command);
                output2.Flush();
            }
        }

        
        void Run_Cable()
        {
            while (true)
            {
                while(a)
                {
                String command = receiveCloud();
                if (command.Equals(Protocol.CONFIRMATION))
                {
                    ChangeText("Przyszło CONFIRMATION");
                    a=false;
                    b = true;
                }
                }
                while (b)
                {
                    Byte[] data2 = new Byte[1024];
                    String Received = String.Empty;

                    clientSockStreamCloud.Read(data2, 0, data2.Length);

                   
                    Received = (String)ByteArrayToObject(data2);
                    String DoWyswietlenia = s.DeCreateMessage(Received);
                    ChangeText2("Klient " + Convert.ToString(s.Nr_klienta)+ " ( f1= "+ s.Jakie_f1+ ", f2= "+ s.Jakie_f2+ " ): " +DoWyswietlenia+"\n");
                    
                }
        
                
            }

            
        }
        
        public void ChangeText(string t)
        {
            if (InfoBox.InvokeRequired)
            {
                InfoBox.Invoke((MethodInvoker)delegate { InfoBox.Items.Add(t); });
            }
            else
            {
                InfoBox.Items.Add(t);
            }
        }

        private void ChangeText2(string t)
        {
            if (InfoBox.InvokeRequired)
            {
                InfoBox.Invoke((MethodInvoker)delegate { ChatBox.Items.Add(t); });
            }
            else
            {
                ChatBox.Items.Add(t);
            }
        }
        void Run()
        {
            send(Protocol.LOGIN + " " + name, output);
            //ChangeText("WYSLALEM");
            while (true)
            {
                String command = receive();
               // ChangeText("Serwer: " + command);
                string[] tab = command.Split(' ');
                command = tab[0];

                if (command.Equals(Protocol.CONFIRMATION))
                {
                    ChangeText("Przyszło CONFIRMATION");
                  
                 }

                else if (command.Equals(Protocol.CALL_IND))
                {
                    String odKogo = tab[1];
                    ChangeText("Przyszło CALL_INDIC. " + odKogo);
                    send(Protocol.CALL_ACCEPT, output);
                    ChangeText("Wysylam CallAccept");


                }

                else if (command.Equals(Protocol.CALL_FAIL))
                {
                  
                    ChangeText("Przyszło CALL FAILED ");
    
                }

                else if (command.Equals(Protocol.CALL_ACCEPT))
                {
                    f1 = tab[1];
                    f2 = tab[2];
                    modulacja = "1";
                    ChangeText("Przyszło CALL ACCEPT");
                    ChangeText(f1 + " " + f2 + " " + modulacja);
                    sendMsg = true;


                }




                else if (command.Equals(Protocol.PARAMETERS))
                {
                    ChangeText("Przyszło PARAMETERS");
                    eon = new sygnaleon(Protocol.PARAMETERS  +" " + tab[1] + " " + tab[2]+ " "+tab[3]);

                    f1 = tab[1];
                    f2 = tab[2];
                    modulacja = tab[3];
                   
                }
                else if (command.Equals(Protocol.NULLCOMMAND))
                {
                    break;
                }
            }
        }

        
        private String receiveCloud()
        {
            try
            {
                return inputCloud.ReadLine();
            }
            catch (IOException e)
            {
                ChangeText("Błąd odczytu danych od agenta");
            }
            return Protocol.NULLCOMMAND; //return input.ReadLine();
        }
        
        private String receive()
        {
            try
            {
                return input.ReadLine();
            }
            catch (IOException e)
            {
                ChangeText("Błąd odczytu danych od zarządcy/NCC");
            }
            return Protocol.NULLCOMMAND; //return input.ReadLine();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (Username.InvokeRequired)
            {
                Username.Invoke((MethodInvoker)delegate { ConnectionMessage = Protocol.CONNECTION + " " + Username.Text + " " + Capacity.Text; });
            }
            else
            {
                ConnectionMessage = Protocol.CONNECTION + " " + Username.Text + " " + Capacity.Text;
            }

            send(ConnectionMessage, output);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ChatBox.Items.Clear();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {

            // send(Protocol.LOGIN, outputCloud);
            //ChatBox.Text += " To nie dziala";
            ChangeText2("Wysłano do " + Username.Text +": " + SendBox.Text + "\n");
            sendMessage(SendBox.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChatBox.Text = null;
        }

        private void sendMessage(String message)
        {
            String tab = eon.CreateMessage(message);
            Byte[] data2 = ObjectToByteArray(tab);
            //Int32 datasize = message.Length;
           // Cloud = new NetworkStream(appSocket_Cable);
           // NetworkStream clientSockStreamCloud = tcpClientCloud.GetStream();
            //foreach (Byte b in data2)
           // {
            //    ChangeText2(Convert.ToString(b));
           // }
                clientSockStreamCloud.Write(data2, 0, data2.Length);
            clientSockStreamCloud.Flush();
           

            // Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            //Stream.Write(data, 0, datasize);
            // ChangeText(message);
        }
        //private void sendMessage(String[,] message, NetworkStream Stream)
        // {
        //    string mess = Convert.ToString(ID_client);
        //   int width = message.GetLength(1);
        //   for (int i = 0; i < width; i++)
        //   {
        //       mess += " " + Convert.ToString(message[0, i]) + " " + Convert.ToString(message[1, i]);
        //    }
        //   Byte[] data = System.Text.Encoding.ASCII.GetBytes(mess);
        //   Stream.Write(data, 0, mess.Length);
        //   ChangeText(mess);
        //  }
        private void Disconnect_Click(object sender, EventArgs e)
        {
            DisConnectionMessage = Protocol.END + " " + Username.Text + " " + Capacity.Text;
            send(DisConnectionMessage, output);
         }

        private void InfoBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public  Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            //obj2= Convert.ToStringArray(obj);
            return obj;
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

        private void exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox.Checked)
            {
                viaManagement = true;
            }

            else
            {
                viaManagement = false;
            }
        }

       
    }
}

