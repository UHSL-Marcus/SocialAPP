using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRE_WebService
{
    public class AllServiceInfo
    {
        public Towns Town { get; set; }
        public TownServices Service { get; set; }
        public Category[] Categories { get; set; }
        public SubCategory[] Subcategories { get; set; }
    }
}