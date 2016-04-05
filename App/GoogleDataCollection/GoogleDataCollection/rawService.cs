using System;


namespace GoogleDataCollection
{
    public class rawService
    {
        public String place_id;
        public String name;
        public String formatted_address;
        public String formatted_phone_number;
        public double rating;
        public String website;
        public String[] types;
        public geometry geometry;
        public aspects[] aspects;
        public reviews[] reviews;

    }

    public class location
    {
        //public double G;
        //public double K;
        public double lat;
        public double lng;
    }

    public class geometry
    {
        public location location;
    }

    public class aspects
    {
        public double rating;
        public String type;
    }

    public class reviews
    {
        public aspects[] aspects;
    }
}