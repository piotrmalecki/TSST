using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubnetworkController
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                new CC(args[0], args[1]);
               
            }
            else
            {
                new CC("a", "10102");
            }

       }
    }
}
