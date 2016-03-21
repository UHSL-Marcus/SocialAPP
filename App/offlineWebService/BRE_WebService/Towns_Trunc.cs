using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRE_WebService
{
    public class Towns_Trunc
    {
        public int TownID { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}