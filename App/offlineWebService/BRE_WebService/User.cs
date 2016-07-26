using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BRE_WebService
{
    public class User
    {
        public int UserID {get; set;}
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string HouseNumberName { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public List<CategoryInfo> Categories { get; set; }
        
    }

    public class CategoryInfo
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int CategoryValue { get; set; }
    }
}