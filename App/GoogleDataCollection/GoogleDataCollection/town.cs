using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleDataCollection.Town
{
    public class town
    {
        public address_componets[] address_components { get; set; }
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string place_id { get; set; }
        public string[] types { get; set; }


    }

    public class geometry
    {
        public bounds bounds { get; set; }
        public latLng location { get; set; }
        public string location_type { get; set; }
        public bounds viewport { get; set; }

    }

    public class bounds
    {
        public latLng northeast { get; set; }
        public latLng southwest { get; set; }

    }

    public class latLng
    {
        public string G { get; set; }
        public string K { get; set; }
    }

    

    public class address_componets
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }



}