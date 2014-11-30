using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubnetworkController
{
    class RC
    {

        private List<Wierzcholek> wierzcholki = new List<Wierzcholek>();
        private List<Wierzcholek> kolejka = new List<Wierzcholek>();
        String dodSnpp;
       
        int KlientNaPoczatku =0;
        public RC() { }

        public List<String> ZnajdzDroge(String idFrom, String idTo)
        {
            
            Console.WriteLine(DateTime.Now + " RC: Żądanie RouteTableQuery od CC, " + idFrom + ", " + idTo);
            String start="";
            String end="";
            if (idFrom[0].Equals('k') && idTo[0].Equals('k'))
            {
                start = idFrom;
                end = idTo;
            }
            else if (idFrom[0].Equals('k'))
            {
                start = idFrom;
                end = "w" + idTo[0];
                KlientNaPoczatku = 1;
                dodSnpp = idTo;
            }
            else if (idTo[0].Equals('k'))
            {
                start = "w" + idFrom[0];
                end = idTo;
                KlientNaPoczatku = 2;
                dodSnpp = idFrom;
            }
            Console.WriteLine(DateTime.Now + " RC: Wyliczam scieżkę między " + start + " i " + end);
            return wyliczSciezke(start, end);
        }


        public void stworzGraf(List<Link> links)
        {
            Console.WriteLine(DateTime.Now + " RC: Otrzymano LocalTopology od LRM");
            // wierzcholki.Add(w);
          List<Sasiad> sasiedzi = new List<Sasiad>();
          Wierzcholek w = new Wierzcholek(links[0].node, sasiedzi);
          wierzcholki.Add(w);
          
            for (int i = 1; i<links.Count; i++)
                 {
                    if (!wierzcholki[wierzcholki.Count-1].id.Equals(links[i].node))
                         {
                             
                             List<Sasiad> sas = new List<Sasiad>();
                             Wierzcholek wierz = new Wierzcholek(links[i].node, sas);
                             wierzcholki.Add(wierz);
                         }
                     
            }

            for (int i = 0; i < wierzcholki.Count; i++)
            {
                for (int k = 0; k < links.Count; k++)
                {
                    if (wierzcholki[i].id.Equals(links[k].node))
                        if (!links[k].nodeEnd.Equals("e"))
                            wierzcholki[i].sasiedzi.Add(new Sasiad(links[k].nodeEnd, links[k].linkId, links[k].band, links[k].snpp, links[k].snppEnd));
                }
           }


                 /*for (int i = 0; i < wierzcholki.Count; i++)
                 {
                     Console.WriteLine("\n\nwierzcholek " + wierzcholki[i].id);
                     for (int j=0; j<wierzcholki[i].sasiedzi.Count; j++)
                     {
                         Console.WriteLine("Sasiad: " + wierzcholki[i].sasiedzi[j].id + ", łącze: " + wierzcholki[i].sasiedzi[j].lacze
                                 + ", waga: " + wierzcholki[i].sasiedzi[j].waga);
                     }
                            

                 }*/
                 
        }




        List<String> wyliczSciezke(String start, String end)
        {
            for (int i = 0; i < wierzcholki.Count; i++)
            {
                kolejka.Add(wierzcholki[i]);
            }

            int k = find(start);

            Wierzcholek current = wierzcholki[k];
            current.d = 0;

            while (kolejka.Count > 0)
            {
                //Console.WriteLine("wierzcholek " + current.id);
                for (int i = 0; i < current.sasiedzi.Count; i++)
                {

                    int w = find(current.sasiedzi[i].id);
                    Wierzcholek next = wierzcholki[w];
                   // Console.WriteLine("sasiad " + next.id);
                    //Console.WriteLine("nowa waga: " + (current.d + current.sasiedzi[i].waga).ToString());
                    //Console.WriteLine("stara waga: " + next.d);

                    if (current.sasiedzi[i].waga + current.d < next.d)
                    {
                        next.pop = current.id;
                        next.d = current.sasiedzi[i].waga + current.d;
                       // Console.WriteLine("d" + next.d);
                       // Console.WriteLine("pop" + next.pop);

                    }
                }

                kolejka.Remove(current);
                if (kolejka.Count > 0)
                {
                    current = findNext();
                }


            }

            List<String> sciezkaW = new List<String>();
            int lacze = 0;
            List<String> snppSeq = new List<String>();
            String snpp = "null";
            String snppEnd = "null";

            if (KlientNaPoczatku==1)
                snppSeq.Add(dodSnpp);
           
                int a = find(end);
                Wierzcholek cur = wierzcholki[a];
                sciezkaW.Add(cur.id);
               // Console.WriteLine("CUR id  " + cur.id);

                while (!(cur.pop.Equals("0")))
                {
                    for (int i = 0; i < wierzcholki.Count; i++)
                    {
                        if (wierzcholki[i].id.Equals(cur.id))
                        {
                            int waga = 1000;
                            for (int j = 0; j < wierzcholki[i].sasiedzi.Count; j++)
                            {
                                if (wierzcholki[i].sasiedzi[j].id.Equals(cur.pop))
                                {
                                    if (wierzcholki[i].sasiedzi[j].waga < waga)
                                    {
                                        lacze = wierzcholki[i].sasiedzi[j].lacze;
                                        waga = wierzcholki[i].sasiedzi[j].waga;
                                        snpp = wierzcholki[i].sasiedzi[j].snppEnd;
                                        snppEnd = wierzcholki[i].sasiedzi[j].snpp;
                                    }

                                }

                            }
                        }
                    }


                    snppSeq.Add(snppEnd);
                    //snppSeq.Add(lacze.ToString());
                    snppSeq.Add(snpp);
                    
                   
                    a = find(cur.pop);
                    cur = wierzcholki[a];
                    sciezkaW.Add(cur.id);
                    //Console.WriteLine("CUR id  " + cur.id);

                }

                if (KlientNaPoczatku==2)
                    snppSeq.Add(dodSnpp);


                Console.Write("" + DateTime.Now + " RC: Ścieżka wyznaczona.\n     Węzły:");
                for (int i = sciezkaW.Count - 1; i >= 0; i--)
                {
                    Console.Write(" " + sciezkaW[i]);
                }

                Console.WriteLine(snppSeq.Count);
                Console.Write("     Sekwencja snpp: ");
                for (int i = snppSeq.Count - 1; i >= 0; i--)
                {
                    Console.Write(snppSeq[i] + " ");
                }
                
             
             


                return snppSeq;

            
        }


       int find(String dd)
        {
            for (int i=0; i<wierzcholki.Count; i++)
                if (wierzcholki[i].id.Equals(dd))
                {
                   return i;
                }

            return 0;
        }

        Wierzcholek findNext()
        {
            Wierzcholek next = kolejka[0];
            for (int i = 1; i < kolejka.Count; i++)
            {
                if (kolejka[i].d < next.d)
                {
                    next = kolejka[i];
                }

            }

            return next;

        }

        

    }
        

    }

