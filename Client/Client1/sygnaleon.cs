using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client1
{
    class sygnaleon
    {
        int f1;
        int f2;
        int modulacja;
        public sygnaleon(String message)
        {
            string[] s = message.Split(' ');
            f1 = Convert.ToInt16(s[1]);
            f2 = Convert.ToInt16(s[2]);
            modulacja = Convert.ToInt16(s[3]);
        }
        public sygnaleon()
        {
        } 
        // Tablica do reprezantacji technologi Eon
        //public string[,] arrayEon;
        // czas przesyłany przez Aplikację zarządzania
        public static int t;
        // częstotliwość przesyłana przez aplikację zarządzania.
        public static int f;
        // zmienna pomocnicza
        public string temp;
        static TextReader q;
        public char Nr_klienta;
        public String Jakie_f1;
        public String Jakie_f2;

        //public static void PostacEon()
        //{
        //    q.ReadTextFromFile();
        //    q.Podstaw(t, f);

        //}
        /// <summary>
        /// Założenie że f1 jest mniejsze od f2
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public String CreateMessage(String message)
        {
         String result = String.Empty;
         String IleZer=String.Empty;
           for(int i =1; i<=modulacja; i++){
               IleZer+="0";
            }

           if (f1 >= 10 && f2 >= 10)
           {
               result = "02" + "00" + Convert.ToString(f1) + Convert.ToString(f2) + "0" + Form1.ID_client2 + message + IleZer;
           }
           if (f1 < 10 && f2 >= 10)
           {
               result = "02" + "00" + "0" + Convert.ToString(f1) + Convert.ToString(f2) + "0" + Form1.ID_client2 + message + IleZer;
           }
           else if (f1 < 10 && f2 < 10)
           {
               result = "02" + "00" + "0" + Convert.ToString(f1) + "0" + Convert.ToString(f2) + "0" + Form1.ID_client2 + message + IleZer;
           }
           return result;

        }
        //public String[] CreateMessage(String message)
        //{
        //    String[] result;
        //    if (f1 > f2)
        //        f2++;
        //    else
        //        f1++;
        //    int deltaf = ((f1 > f2) ? f1 - f2 : f2 - f1) + 1;
        //    result = new string[deltaf + 1];
        //    int inonecell = Convert.ToInt32(message.Length / deltaf);
        //    int biggersizecells = message.Length % deltaf;
        //    int charno = 0;
        //    result[0] = "00" + "02" + "0" + Form1.ID_client2;
        //    for (int i = 1; i <= deltaf ; i++)
        //    {
               
        //        if (i <= biggersizecells)
        //        {
        //            result[i] = Convert.ToString(i + f1 - 1) + message.Substring(charno, inonecell + 1);
        //            charno += inonecell + 1;
        //        }
        //        else
        //        {
        //            result[i] = convert.tostring(i + f1 - 1) + message.substring(charno, inonecell);
        //            charno += inonecell;
        //        }
        //    }

        //    return result;
        //}

        public String DeCreateMessage(String result)
        {
            String message = String.Empty;
            StringBuilder str = new StringBuilder(result);
            StringBuilder mess = new StringBuilder(message);
            Nr_klienta = result[9];
            Jakie_f1=Convert.ToString(result[4])+Convert.ToString(result[5]);
            Jakie_f2 = Convert.ToString(result[6]) + Convert.ToString(result[7]);
            for (int i = 0; i < str.Length; i++)
            {
                if (!(str[i].Equals('1')) && !(str[i].Equals('2')) && !(str[i].Equals('3')) && !(str[i].Equals('4')) && !(str[i].Equals('5')) && !(str[i].Equals('6')) && !(str[i].Equals('7')) && !(str[i].Equals('8')) && !(str[i].Equals('9')) && !(str[i].Equals('0')))
                {
                    mess.Insert(mess.Length, str[i]);
                }
            }
            message = mess.ToString();
            return message;
        }


        //public String DeCreateMessage(String[] result)
        //{
        //    String message = String.Empty;
        //    String tempt = String.Empty;
            
        //    for(int i = 0; i<result.Length; i++) 
        //    {
        //        tempt += result[i];
        //    }
        //    Nr_klienta=tempt[5];
        //    StringBuilder str = new StringBuilder(tempt);
        //    StringBuilder mess = new StringBuilder(message);

        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (!(str[i].Equals('1')) && !(str[i].Equals('2')) && !(str[i].Equals('3')) && !(str[i].Equals('4')) && !(str[i].Equals('5')) && !(str[i].Equals('6')) && !(str[i].Equals('7')) && !(str[i].Equals('8')) && !(str[i].Equals('9')) && !(str[i].Equals('0')))
        //        {
        //            mess.Insert(mess.Length, str[i]);
        //        }
        //    }

        //    message = mess.ToString();
        //    return message;


        //}



    }
}
