using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RootController
{
    class Directory
    {
        public String name {get; set;}
        public String address { get; set; }

        public Directory(String name, String address)
        {
            this.name=name;
            this.address = address;
        }
    }
}
