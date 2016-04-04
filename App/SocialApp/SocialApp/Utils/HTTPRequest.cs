using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SocialApp.Utils
{
    public class HTTPRequest
    {
        public String HttpSOAPRequest(String body, string action)
        {
            try {
                string SOAPReq = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                SOAPReq += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
                SOAPReq += "<soap:Body>";
                SOAPReq += "<" + action + "  xmlns=\"http://tempuri.org/\">";
                SOAPReq += body;
                SOAPReq += "</" + action + ">";
                SOAPReq += "</soap:Body>";
                SOAPReq += "</soap:Envelope>";

                //Builds the connection to the WebService.
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://sash.herts.ac.uk/cs/BRESocialAppService.asmx");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost:3492/BRESocialAppService.asmx");
                req.Method = "POST";
                req.ContentType = "text/xml; charset=utf-8";
                req.ContentLength = Encoding.UTF8.GetByteCount(SOAPReq);
                req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + action + "\"");


                //Pass the SoapRequest String to the WebService
                StreamWriter stmw = new StreamWriter(req.GetRequestStream());
                stmw.Write(SOAPReq);
                stmw.Flush();

                WebResponse resp = req.GetResponse();
                StreamReader r = new StreamReader(resp.GetResponseStream());

                return r.ReadToEnd();
            } catch (Exception e)
            {
                Console.WriteLine("HTTP Exception: " + e.ToString() + " (" + e.Message + ")");
            }
            return null;

            

        }
        public static string getGoogleGeoLocation(string userXML)
        {
            XMLParse user = new XMLParse(userXML, SOAPRequest.soapNamespace);
            string url = "https://maps.googleapis.com/maps/api/geocode/xml?address=";                       // build the google geocode webrequest
            url += user.getElementText("HouseNumberName");
            url += "+" + user.getElementText("Address");
            url += "+" + user.getElementText("Town");
            url += "+" + user.getElementText("Postcode");
            url += "+UK";

            HttpWebRequest AddressReq = (HttpWebRequest)WebRequest.Create(url);                             // send it
            WebResponse resp = AddressReq.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());
            return r.ReadToEnd();
        }
    }
}