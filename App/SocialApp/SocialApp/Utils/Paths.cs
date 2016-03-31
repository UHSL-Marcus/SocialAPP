using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialApp.Utils
{
    public class Paths
    {
        /************************SESSION CONSTS*************************/
        public const String USERDETAILS = "userDetails";

        /************************ PAGE CONSTS***************************/
        public const String PAGEADDR_BASE = "~/Pages/";
        public const String PAGE_HOME = PAGEADDR_BASE + "Home.aspx";
        public const String PAGE_PROFILE = PAGEADDR_BASE + "Profile.aspx";
        public const String PAGE_CREATE = PAGEADDR_BASE + "Create.aspx";
        public const String PAGE_MAPS = PAGEADDR_BASE + "Maps.aspx";
        public const String PAGE_STATS = PAGEADDR_BASE + "Stats.aspx";

        /*************************WEB SERVICE CONSTS*******************/
        public const String TOWNLIST = "town_list";
        public const String TOWNSERVE = "town_services";
    }
}