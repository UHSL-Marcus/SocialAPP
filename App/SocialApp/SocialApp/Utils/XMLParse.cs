using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SocialApp.Utils
{
    public class XMLParse
    {
        private String XMLtoRead;
        private String nmspc;
        public XMLParse(String xml, String ns = null)
        {
            XMLtoRead = xml;
            nmspc = ns;
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
        // wrapper: return the data wrapped in a parent element, to keep the reply as valid XML
        public String getElementText(String element, String parent = null, bool wrapper = false)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLtoRead);

            var ns = new XmlNamespaceManager(xml.NameTable);
            if (nmspc != null)
            {
                ns.AddNamespace("ns", nmspc);
                element = "ns:" + element;
                if (parent != null)
                    parent = "ns:" + parent;
            }
            

            var baseElement = xml.DocumentElement;
            XmlNode node = null;

            if (parent != null)
            {
                var tempnode = baseElement.SelectSingleNode("//" + parent, ns);
                node = tempnode.SelectSingleNode(".//" + element, ns);
            }
            else
                node = baseElement.SelectSingleNode("//" + element, ns);


            if (node != null)
            {
                if (wrapper) 
                    return node.OuterXml;

                 return node.InnerXml;
            }
            else return null;
            
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
        public List<String> getWholeSection(String topLevelElement, bool includeParent = true)
        {
            List<String> result = new List<String>();
           
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLtoRead);

            var ns = new XmlNamespaceManager(xml.NameTable);
            if (nmspc != null)
            {
                ns.AddNamespace("ns", nmspc);
                topLevelElement = "ns:" + topLevelElement;
            }

            XmlNodeList nodes = xml.DocumentElement.SelectNodes("//" + topLevelElement, ns);

            foreach (XmlNode node in nodes)
            {
                if (includeParent)
                    result.Add(node.OuterXml);
                else
                    result.Add(node.InnerXml);
            }
                

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
            if (nmspc != null)
            {
                ns.AddNamespace("ns", nmspc);
                parentElement = "ns:" + parentElement;
                refrenceElement = "ns:" + refrenceElement;
                targetElement = "ns:" + targetElement;
            }

            XmlNodeList nodes = xml.DocumentElement.SelectNodes("//" + parentElement, ns);

            foreach (XmlNode node in nodes)
            {
                XmlNode n = node.SelectSingleNode(refrenceElement, ns);
                if (n != null)
                {
                    String v = n.InnerText;
                    if (string.Equals(v, refrenceValue, StringComparison.OrdinalIgnoreCase))
                    {
                        if (includeParent)
                            return node.SelectSingleNode(targetElement, ns).OuterXml;
                        else
                            return node.SelectSingleNode(targetElement, ns).InnerXml;
                    }
                }
            }
            return null;
        }

        // Adds a new element of name (elementName) with value (elementValue) to the xml document under the (directParent) element
        public void AddElement(string directParent, string elementName, string elementValue = "")
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLtoRead);

            var ns = new XmlNamespaceManager(xml.NameTable);
            if (nmspc != null)
            {
                ns.AddNamespace("ns", nmspc);
                directParent = "ns:" + directParent;
            }

            XmlNode parent = xml.DocumentElement.SelectSingleNode("//" + directParent, ns);

            XmlNode newNode = xml.CreateNode(XmlNodeType.Element, elementName, nmspc);
            if (elementValue.Length > 0) newNode.InnerText = elementValue;

            parent.AppendChild(newNode);

            XMLtoRead = xml.OuterXml.ToString();


        }

        // converts the XML to Json format
        public String convertToJSON()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XMLtoRead);

            JObject temp = createJObject(xml.ChildNodes[0].ChildNodes); // ignore the root node

            return temp.ToString();
        }

        private JObject createJObject(XmlNodeList nodes)
        {
            JObject temp = new JObject();   // create object
            JToken tempJToken;              // for use with detecting repeats

            foreach (XmlNode node in nodes) // loop through each node in the list
            {   
                if (node.NodeType == XmlNodeType.Element)       // element nodes are what we are interested in at this level
                    foreach (XmlNode cnode in node.ChildNodes)  // now loop through each of the child nodes of this element
                    {
                        if (cnode.NodeType == XmlNodeType.Text)                     // this child is a text node, which means this element is a text element and does not have child elements
                        {
                            if (temp.TryGetValue(node.LocalName, out tempJToken))   // detect if a node with this name already exists
                            {
                                if (tempJToken.Type == JTokenType.String)           // if the node is only a string, convert to an array to store both values        
                                {
                                    String currentData = tempJToken.ToString();
                                    temp.Property(node.LocalName).Remove();
                                    temp.Add(node.LocalName, new JArray(new String[] { currentData, cnode.InnerText }));
                                }
                                if (tempJToken.Type == JTokenType.Array)           // if the node is already an array, add a new entry
                                {
                                    List<String> currentData = new List<String>();

                                    foreach (JToken child in tempJToken.Children())
                                        currentData.Add(child.ToString());

                                    currentData.Add(cnode.InnerText);

                                    temp.Property(node.LocalName).Remove();
                                    temp.Add(node.LocalName, new JArray(currentData.ToArray()));

                                }
                            }else
                                temp.Add(node.LocalName, cnode.InnerText);  // node does not exist, add to object, using the element name and the text from the text node
                        }
                        
                        if (cnode.NodeType == XmlNodeType.Element && !temp.TryGetValue(node.LocalName, out tempJToken)) // if a child node of the element is also an element,                                                                                           
                            temp.Add(node.LocalName, createJObject(node.ChildNodes));                                   // this element has children, so recurse the method
                                                                                                                        // and add the resulting object to the current object.
                                                                                                                        // there is also a check to avoid repeat objects (causes an exception)
                            
                    }    
            }

            return temp;
        }
    }

    public class serviceObject
    {
        public string ServiceID { get; set; }
        public string Name { get; set; }
        public string TownID { get; set; }
        public string Rating { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string HasPerimeter { get; set; }
        public string HasVirtualServices { get; set; }
        public string[] VirtualServices { get; set; }
        public string[] CategoryIDs { get; set; }
        public string[] SubCategoryIDs { get; set; }
      
    }
}