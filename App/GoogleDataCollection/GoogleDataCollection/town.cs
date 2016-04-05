using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleDataCollection.Town
{
    public class town
    {
        public address_componets[] address_components;
        public string formatted_address;
        public geometry geometry;
        public string place_id;
        public string[] types;


    }

    public class geometry
    {
        public bounds bounds;
        public latLng location;
        public string location_type;
        public bounds viewport;

    }

    public class bounds
    {
        public latLng northeast;
        public latLng southwest;

        [JsonConstructor]
        public bounds(string north, string south, string east, string west)
        {
            northeast = new latLng(north, east);
            southwest = new latLng(south, west);
        }

    }

    public class latLng
    {
        //public string G;
        //public string K;
        public string lat;
        public string lng;

        public latLng() { }
        public latLng(string lat, string lng)
        {
            this.lat = lat;
            this.lng = lng;
        }
    }

    

    public class address_componets
    {
        public string long_name;
        public string short_name;
        public string[] types;
    }



}