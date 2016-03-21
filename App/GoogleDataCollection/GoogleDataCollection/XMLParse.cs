using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace GoogleDataCollection.Utils
{
    public class XMLParse
    {
        private String XMLtoRead;
        public XMLParse(String xml)
        {
            XMLtoRead = xml;
        }
        private XmlReader settings()
        {
            // Set the reader settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;

            // Create a resolver with default credentials.
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

            // Set the reader settings object to use the resolver.
            settings.XmlResolver = resolver;

            //set the stringreader
            StringReader sr = new StringReader(XMLtoRead);
            return XmlReader.Create(sr, settings);

        }
        // Element: element to search for. Will return the value of the first element with the given name
        // parent: if the element is under a specific parent element (used if there is an element of the same name, in higher levels)
        public String getElementText(String element, String parent = null)
        {
            
            XmlReader reader = settings();
            Boolean nextVal = false;
            Boolean section = false;
            try
            {
                while (reader.Read())
                {

                    if (reader.NodeType == XmlNodeType.Text && nextVal) return reader.Value;
                    else nextVal = false;

                    if (parent != null)
                    {

                        if (reader.Name.Equals(parent)) section = section ? false : true;
                    }
                    else section = true;

                    if (reader.Name.Equals(element) && section) nextVal = true;

                }
            }
            catch (System.Xml.XmlException) {}
            return "";
        }

        // same as getElementText(), only it returns an array of the values of all found elements with the given name
        public ArrayList getAllElementsText(String element)
        {
            ArrayList retList = new ArrayList();

            XmlReader reader = settings();
            Boolean nextVal = false;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text && nextVal)
                {
                    retList.Add(reader.Value);
                    nextVal = false;
                }
                else nextVal = false;

                if (reader.Name.Equals(element) && reader.NodeType == XmlNodeType.Element) nextVal = true;
                
            }

            return retList;
        }

        // toplevelElement: the name of the element acting as the parent for the section desired. 
        /*  <toplevelElement>
                <element></element>
                <element></element>
                <element></element>
            </toplevelElement>
         */
        public ArrayList getWholeSection(String topLevelElement)
        {
            ArrayList result = new ArrayList();
           
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLtoRead);

            var ns = new XmlNamespaceManager(xml.NameTable);
            ns.AddNamespace("ns", "http://tempuri.org/");
            XmlNodeList nodes = xml.DocumentElement.SelectNodes("//ns:" + topLevelElement, ns);

            foreach (XmlNode node in nodes)
                result.Add(node.InnerXml);

            return result;
        }


        // finds the value of an element which is a sibling of a known element/value (refrenceElement/refrenceValue) under a named parent (parentElement)
        // used when the parent element is a repeated pattern, the refrence value should be unique to a specific instance of the parent 
        // example:
        /*  <parentElement>
                <element></element>
                <refrenceElement>wrong refrenceValue</refrenceElement>
                <element></element>
                <targetElement><targetElement>
            </parentElement>
            <parentElement>
                <element></element>
                <refrenceElement>correct refrenceValue</refrenceElement>
                <element></element>
                <targetElement><targetElement>
            </parentElement>
         */
        public String getElementFromSibling(String parentElement, String refrenceElement, String refrenceValue, String targetElement, Boolean includeParent = false)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLtoRead);

            var ns = new XmlNamespaceManager(xml.NameTable);
            ns.AddNamespace("ns", "http://tempuri.org/");
            XmlNodeList nodes = xml.DocumentElement.SelectNodes("//ns:" + parentElement, ns);

            foreach(XmlNode node in nodes)
            {
                XmlNode n = node.SelectSingleNode("ns:"+refrenceElement, ns);
                if (n != null)
                {
                    String v = n.InnerText;
                    if (string.Equals(v, refrenceValue, StringComparison.OrdinalIgnoreCase))
                    {
                        if (includeParent)
                            return "<parent>" + node.SelectSingleNode("ns:" + targetElement, ns).InnerXml + "</parent>";
                        else
                            return node.SelectSingleNode("ns:" + targetElement, ns).InnerXml;
                    }
                }
            }
            return null;
        }
    }
}