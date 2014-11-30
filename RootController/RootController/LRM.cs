using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RootController
{
    class LRM
    {
        List<Link> links = new List<Link>();
        bool [] lambdy;

        public LRM()
        {

            links = LoadXML();
            Console.WriteLine (DateTime.Now + " LRM: Wczytuję topologię");
            /*for (int i = 0; i < links.Count; i++)
            {
                Console.WriteLine("node:" + links[i].node + " linkId:" + links[i].linkId + " band:" + links[i].band + " length:" + links[i].length
                    + " nodeEnd:" + links[i].nodeEnd);


            }*/
        }

        public int[] ZnajdzLambdy(String snpp, String snppEnd, int przepust)
        {
            Console.WriteLine(DateTime.Now + " LRM: Żądanie LinkConnectionRequest od CC (" +snpp+ ", " + snppEnd + ")");
           int ileLambd = przepust;
           int lacze = 0;
             
                                                    
               for (int i = 0; i< links.Count; i++)
               {
                   if (links[i].snpp.Equals(snpp) && links[i].snppEnd.Equals(snppEnd))
                   {
                       lambdy = new bool[links[i].band];   
                       lambdy = links[i].lambdy;
                       lacze = links[i].linkId;
                   }
               }
            int[] ret = new int[2];
            ret[0] = 0;
            ret[1] = 0;

            for (int i = 1; i < lambdy.Length; i++)
            {
               // Console.WriteLine("i: " + i);
                int j = 0;
                while (j < ileLambd)
                {
                    //Console.WriteLine("j: " + j);
                    if (lambdy[i + j] == true)
                    {
                        if (j == ileLambd-1)
                        {
                            int f1 = i;
                            int f2 = f1 + ileLambd - 1;
                            Console.WriteLine(DateTime.Now + "  LRM: Łącze " + lacze + ": wybrany przedział częstotliwości: " + f1 + "-" + f2);
                            
                            ret[0] = f1;
                            ret[1] = f2;
                            for (int k = f1; k <= f2; k++)
                            {

                                lambdy[k] = false;
                            }
                            return ret;

                        }
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Console.WriteLine(DateTime.Now + " LRM:  Brak wystarczającej ilości wolnych częstotliwości w łączu " + lacze);
            return ret;
           
        }

        

        public List<Link> getLacza()
        {
            return links;
        }


        List<Link> LoadXML()
        {
            XmlDocument dokument = new XmlDocument();
            dokument.Load("RootLinks.xml");

            XmlNode root = dokument.SelectSingleNode("config");
            XmlNodeList wezly = root.SelectNodes("node");

            foreach (XmlNode s in wezly)
            {
                String node = s.Attributes.GetNamedItem("id").InnerText;
                String linkId = s.SelectSingleNode("link_id").InnerText;
                String band = s.SelectSingleNode("band").InnerText;
                String length = s.SelectSingleNode("length").InnerText;
                String node_end = s.SelectSingleNode("node_end").InnerText;
                String snpp = s.SelectSingleNode("snpp").InnerText;
                String snpp_end = s.SelectSingleNode("snpp_end").InnerText;

                Link lacze = new Link(node, linkId, band, length, node_end, snpp, snpp_end);
                links.Add(lacze);


            }

            return links;
        }

    }
       
}
