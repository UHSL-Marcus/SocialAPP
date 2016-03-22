using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialApp.Utils
{
    public class tempObject
    {
        public String place_id;
        public String name;
        public String formatted_address;
        public String formatted_phone_number;
        public double rating;
        public String website;
        public latLng location;
        public obcategory[] categories;
    }

    public class latLng
    {
        public double lat;
        public double lng;

        public latLng(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }
    }

    public class obcategory
    {
        public String categoryName;
        public String[] subCategories;

        public obcategory(String name, String[] sub)
        {
            categoryName = name;
            subCategories = sub;
        }
    }

    public class category
    {
        public int id;
        public String name;
        public int[] subcategories;

    }
    public class subcategory
    {
        public int id;
        public String name;
    }
}