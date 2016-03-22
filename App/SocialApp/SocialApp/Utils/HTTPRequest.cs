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

            

        }
    }
}