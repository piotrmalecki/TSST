using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wezel_Sieciowy1
{
    class ModelResource
    {
        public int idW { get; private set; }
        public String Network { get; private set; }
        public List<Snpp> snppList = new List<Snpp>();
        public const String BASE_PATH = "lacza.xml";

        public ModelResource(int idW)
        {
            this.idW = idW;
            LoadBase();
        }

        private void LoadBase()
        {
            Snpp lacze = null;

            int snppId = 0;
            int band = 0;
            int length = 0;
            String linkId = "";
            String snpp_end = "";

            XmlDocument dokument = new XmlDocument();
            dokument.Load(BASE_PATH);

            XmlNode root = dokument.SelectSingleNode("config");
            XmlNodeList lacza;
            XmlNodeList wezly = root.SelectNodes("node");

            foreach (XmlNode s in wezly)
            {
                if (s.Attributes.GetNamedItem("id").InnerText.Equals(idW.ToString()))   // jak odnajdziesz swoja pozycje
                {
                    Network = s.SelectSingleNode("network").InnerText;
                    lacza = s.SelectNodes("snpp");

                    foreach (XmlNode t in lacza)
                    {
                        snppId = Convert.ToInt32(t.Attributes.GetNamedItem("id").InnerText);
                        linkId = t.SelectSingleNode("link_id").InnerText;
                        band = Convert.ToInt32(t.SelectSingleNode("band").InnerText);
                        length = Convert.ToInt32(t.SelectSingleNode("length").InnerText);
                        snpp_end = t.SelectSingleNode("snpp_end").InnerText;

                        lacze = new Snpp(snppId, linkId, band, length, snpp_end);
                        snppList.Add(lacze);
                    }


                }
            }
        }

        //public String GetLinkId(String snppOut)
        //{
        //    foreach (Snpp s in snppList)
        //        if (snppOut.Equals(s.linkId))
        //            return s.linkId;

        //    return "0";
        //}


        public String GetSnnpId(String linkID)
        {
            Console.WriteLine("lokalizuje dla link: " + linkID);
            foreach (Snpp s in snppList)
            {
                Console.WriteLine("gowno: "+s.linkId.ToString());
                if (s.linkId.Equals(linkID))
                {
                    Console.WriteLine("znalazłem");
                    return s.snppId.ToString();
                }
            }

            return "0";
        }
    }

    class Snpp
    {
        public String linkId { get; private set; }
        public int band { get; private set; }
        public int length { get; private set; }
        public int snppId { get; private set; }
        public String snpp_end { get; private set; }

        public Snpp(int snppId, String linkId, int band, int length, String snpp_end)
        {
            this.linkId = linkId;
            this.band = band;
            this.length=length;
            this.snpp_end = snpp_end;
            this.snppId = snppId;
        }
    }


}
