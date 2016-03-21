using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleDataCollection
{
    public class service
    {
        public String place_id;
        public String name;
        public String formatted_address;
        public String formatted_phone_number;
        public double rating;
        public String website;
        public latLng location;
        public category[] categories;
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

    public class category
    {
        public String categoryName;
        public String[] subCategories;

        public category(String name, String[] sub)
        {
            categoryName = name;
            subCategories = sub;
        }
    }
}