using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialApp.Utils
{
    public class SOAPRequest
    {
        public static String soapNamespace = "http://tempuri.org/";

        public static String getTownList()
        {
            HTTPRequest req = new HTTPRequest();
            String response = req.HttpSOAPRequest("", "GetListOfTowns");

            String result = (new XMLParse(response, soapNamespace)).getElementText("TownID"); // make sure at least one exists

            int tID = 0;
            Int32.TryParse(result, out tID);

            if (tID > 0)
            {
                return response;
            }
            return null;
        }
    }
}
