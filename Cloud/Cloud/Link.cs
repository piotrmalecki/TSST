using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud
{
    class Link 
    {
        public char typeStartNode;
        public char typeEndNode;
        public int startNode;
        public int endNode;
        public int startPort;
        public int endPort;
        public Link(int start, int startport, char typestart, int end, int endport, char typeend)
        {
            startNode = start;
            endNode = end;
            typeStartNode=typestart;
            typeEndNode = typeend;
            startPort = startport;
            endPort = endport;
        }

        public Link()
        {
            startNode = 0;
            endNode = 0;
            typeStartNode = ' ';
            typeEndNode = ' ';
            startPort = 0;
            endPort = 0;
        }
    }
}
