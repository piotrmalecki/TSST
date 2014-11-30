using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wezel_Sieciowy1
{
    public class Siec_Data
    {
        /**
         * 
         * zakladam 20 lamb na swiatlowod, 5 portow in, 5 out 
         * 1024 bajty ma hmmm, paczka? doprecyzowac. 
         **/

        private Siec Network = null;

        public Siec_Data(Siec Network)
        {
            this.Network = Network;
            //this.gniazdo = Network.gniazdo;
        }

        public Siec_Data()
        { }


        public String Receive_Data()
        {
            byte[] buffer = new byte[1024];
            String temp = null;

            Network.strumienCloud.Read(buffer, 0, buffer.Length);               
            
            temp = (String)(ByteArrayToObject(buffer));
            Console.WriteLine("Otrzymałem: " + temp.Substring(0,10));

            return (temp);
        }

        public void Send_Data(String pakiet)
        {
            byte[] buffer = new byte[1024];

            //Console.WriteLine("Wysylam do chmury: " + pakiet[0].Substring(0,10));
            buffer = ObjectToByteArray(pakiet);
            Network.strumienCloud.Write(buffer, 0, buffer.Length);
            Network.strumienCloud.Flush();
        }

        public Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            Object obj = (Object)binForm.Deserialize(memStream);
            
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



    }

    

}
