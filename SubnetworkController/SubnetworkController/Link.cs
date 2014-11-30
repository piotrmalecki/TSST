using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubnetworkController
{
 
        class Link
        {
            public String node { get; set; }
            public int linkId { get; set; }
            public int band { get; set; }
            public int length { get; set; }
            public String nodeEnd { get; set; }
            public String snpp { get; set; }
            public String snppEnd { get; set; }
            public bool[] lambdy { get; set; } //true jesli lambda wolna, false jesli zajeta


            public Link(String node, String lin, String ban, String len, String nodeEnd, String snpp, String snppEnd)
            {
                this.node = node;
                linkId = Convert.ToInt32(lin);
                band = Convert.ToInt32(ban);
                length = Convert.ToInt32(len);
                this.nodeEnd = nodeEnd;
                this.snpp = snpp;
                this.snppEnd = snppEnd;


                lambdy = new bool[band + 1];

                for (int i = 1; i < lambdy.Length; i++)
                {
                    lambdy[i] = true;
                }

            }
        }
    }


