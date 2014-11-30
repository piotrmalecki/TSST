using System;
using System.IO;
using System.Collections;

namespace Client1
{
    class TextReader
    {

        // funkcja do przesylania zmiennych z tablicy do klasy SygnalEon
        //public void Podstaw(int a, int b)
        //{
        //    a = Convert.ToInt32(temp1[0]);
        //    b = Convert.ToInt32(temp1[1]);
        //}
        public static ArrayList ReadTextFromFile(string File)
        {


            StreamReader objReader = new StreamReader(File);
            string sLine = "";
            ArrayList arrText = new ArrayList();

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }
            objReader.Close();

            foreach (string sOutput in arrText)
                Console.WriteLine(sOutput);
            Console.ReadLine();
            return arrText;
        }


    }
}
