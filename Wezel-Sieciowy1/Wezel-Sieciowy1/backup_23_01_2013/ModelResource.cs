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

            String linkId="";
            int band=0;
            int length=0;
            String snpp_end="";

            XmlDocument dokument = new XmlDocument();
            dokument.Load(BASE_PATH);

            XmlNode root = dokument.SelectSingleNode("/config");
            XmlNodeList lacza;
            XmlNodeList wezly = root.SelectNodes("node");

            foreach (XmlNode s in wezly)
            {
                if (s.Attributes.GetNamedItem("id").InnerText.Equals(idW))   // jak odnajdziesz swoja pozycje
                {
                    Network = s.SelectSingleNode("network").InnerText;
                    lacza = s.SelectNodes("snpp");

                    foreach (XmlNode t in lacza)
                    {
                        linkId = t.Attributes.GetNamedItem("id").InnerText;
                        band = Convert.ToInt32(t.SelectSingleNode("band").Value);
                        length = Convert.ToInt32(t.SelectSingleNode("length").Value);
                        snpp_end = t.SelectSingleNode("snpp_end").Value;
                    }

                    lacze = new Snpp(linkId, band, length, snpp_end);
                }
            }
        }
    }

    class Snpp
    {
        public String linkId { get; private set; }
        public int band { get; private set; }
        public int length { get; private set; }
        public String snpp_end { get; private set; }

        public Snpp(String linkId, int band, int length, String snpp_end)
        {
            this.linkId = linkId;
            this.band = band;
            this.length=length;
            this.snpp_end = snpp_end;
        }
    }


}
