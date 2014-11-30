using System;
using System.IO;
using System.Text;
using System.Net;

namespace Wezel_Sieciowy1
{
    public class Sygnal_Text
    {
        //private Siec_Init Network = null;

        public Sygnal_Text()
        {

        }

        public String Receive_Text(StreamReader inputT)
        {
            String message = "";

            if (inputT != null)
            {
                message = inputT.ReadLine();
                
                return (message);
                
            }
            else
                return ("odbiór failed");
        }

        public void Send_Text(String message, StreamWriter outputT)
        {
            if ((outputT != null) || (message != null))
            {
                Console.WriteLine("Wysyłam polecenie: "+message);
                outputT.WriteLine(message);
                outputT.Flush();
            }
            //Console.ReadKey();
        }


    }
}
