using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRE_WebService
{
    public class TownCategoryCount
    {
        public int TownID { get; set; }
        public int CategoryID { get; set; }
        public int Count { get; set; }
        public int Normal { get; set; }
        public int CountVirt { get; set; }
        public int NormalVirt { get; set; }
    }
}