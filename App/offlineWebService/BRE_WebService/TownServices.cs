using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRE_WebService
{
    public class TownServices
    {
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public int TownID { get; set; }
        public double Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool HasPerimeter { get; set; }
        public string[] Perimeter { get; set; }
        public bool HasVirtualServices { get; set; }
        public string[] VirtualServices { get; set; }
    }
}