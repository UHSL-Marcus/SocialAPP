using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRE_WebService
{
    public class UserCategoryPair
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int CategoryValue { get; set; }

    }
}