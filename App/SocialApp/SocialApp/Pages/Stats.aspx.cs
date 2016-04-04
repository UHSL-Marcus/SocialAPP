using Newtonsoft.Json;
using SocialApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace SocialApp.Pages
{
    public partial class Stats : System.Web.UI.Page
    {
        private SiteMaster thisMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            thisMaster = this.Master as SiteMaster;
            thisMaster.setChild(this);

            if (!IsPostBack)
                fillTownList();
        }

        private void fillTownList()
        {
            if (Session[Paths.TOWNLIST] == null)
                Session[Paths.TOWNLIST] = SOAPRequest.getTownList();    // grab the lost of available towns

            if (Session[Paths.TOWNLIST] == null) statsTownList.Items.Add("No Towns"); // if something has gone wrong, still null
            else
            {
                ArrayList towns = (new XMLParse((String)Session[Paths.TOWNLIST], SOAPRequest.soapNamespace)).getAllElementsText("Town");    // pull out all the town names

                // add em to the options of the dropdown component, including home
                statsTownList.Items.Add("Home");
                foreach (String town in towns)
                {
                    statsTownList.Items.Add(town);
                }

                displayTownInfo(statsTownList.SelectedValue); // load the info for the inially selected value
            }

        }

        private void displayTownInfo(String town)
        {
            //TODO: Try Catch
            if (town.Equals("Home"))
            {
                XMLParse geoLoc = new XMLParse((string)Session[Paths.USERGEOLOC]);
                town = geoLoc.getElementFromSibling("address_component", "type", "postal_town", "long_name");
            }


            String id = (new XMLParse((String)Session[Paths.TOWNLIST], SOAPRequest.soapNamespace)).getElementFromSibling("Towns", "Town", town, "TownID");

            HTTPRequest req = new HTTPRequest();
            String response = req.HttpSOAPRequest("<TownID>" + id + "</TownID>", "GetTownFromTownID");

            String result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("TownID"); // check reply has data for the correct town

            int tID = 0;
            int returnTID = 0;
            Int32.TryParse(id, out tID);
            Int32.TryParse(result, out returnTID);

            if (tID == returnTID)
            {
                List<string> allCategoryCounts = new XMLParse(response, SOAPRequest.soapNamespace).getWholeSection("TownCategoryCount");

                Dictionary<string, int> categoryRating = new Dictionary<string, int>();     // rating is based on service count, not google ratings
                Dictionary<string, int> categoryVirtRating = new Dictionary<string, int>();

                Dictionary<String, String> serviceModalHTML = new Dictionary<String, String>();
                Dictionary<String, int> serviceCount = new Dictionary<String, int>();

                foreach (string categoryCount in allCategoryCounts)
                {
                    XMLParse categoryCountXML = new XMLParse(categoryCount, SOAPRequest.soapNamespace);
                    String categoryID = categoryCountXML.getElementText("CategoryID");
                    int rating = 0;
                    int ratingVirt = 0;

                    Int32.TryParse(categoryCountXML.getElementText("Normal"), out rating);
                    Int32.TryParse(categoryCountXML.getElementText("NormalVirt"), out ratingVirt);
                    String categoryName = new XMLParse(req.HttpSOAPRequest("<CategoryID>" + categoryID + "</CategoryID>", "GetCategoryByID"), SOAPRequest.soapNamespace).getElementText("CategoryName");

                    categoryRating.Add(categoryName, rating);
                    categoryVirtRating.Add(categoryName, ratingVirt);

                    List<string> servicesXML = new XMLParse(req.HttpSOAPRequest("<CategoryID>" + categoryID + "</CategoryID>", "GetAllServicesByCategoryID"), SOAPRequest.soapNamespace).getWholeSection("AllServiceInfo");

                    foreach (string serviceXML in servicesXML)
                    {
                        XMLParse servParse = new XMLParse(serviceXML, SOAPRequest.soapNamespace);
                        ArrayList categoryNames = servParse.getAllElementsText("CategoryName");
                        ArrayList subCategoryNames = servParse.getAllElementsText("SubCategoryName");

                        String serveName = servParse.getElementText("Name");

                        int uniqueCount = 0;
                        serviceCount.TryGetValue(serveName, out uniqueCount);   // if there is more than one service with the same name, this will make the name unique. 
                        serviceCount[serveName] = uniqueCount + 1;

                        if (!serviceModalHTML.ContainsKey(categoryName))
                            serviceModalHTML.Add(categoryName, "<div id=\"stat" + Regex.Replace(categoryName, @"\W", "") + "Modal\" class=\"hidden\">");

                        serviceModalHTML[categoryName] += "<div>"
                        + "<button class=\"btn btn-default\" type=\"button\" data-toggle=\"collapse\" data-target=\"#" + Regex.Replace(serveName, @"\W", "") + uniqueCount
                        + "CollapseInfo\">"
                        + serveName + "</button>"
                        + "<div class=\"collapse\" id=\"" + Regex.Replace(serveName, @"\W", "") + uniqueCount + "CollapseInfo\">"
                        + "Town<br/>" + servParse.getElementText("Town", "Town") + ", " + servParse.getElementText("County")
                        + "<br/>Categories<br/>";
                        foreach (string category in categoryNames)
                            serviceModalHTML[categoryName] += category + "</br>";

                        serviceModalHTML[categoryName] += "<br/>SubCategories<br/>";
                        foreach (string subcategory in subCategoryNames)
                            serviceModalHTML[categoryName] += subcategory + "</br>";

                        serviceModalHTML[categoryName] += "<br/>Rating<br/>" + rating;

                        Boolean hasVirtualServices = false;
                        Boolean.TryParse(servParse.getElementText("HasVirtualServices"), out hasVirtualServices);
                        if (hasVirtualServices)
                        {
                            serviceModalHTML[categoryName] += "<br/>Virtual Services";
                            ArrayList virtualServices = (new XMLParse(servParse.getElementText("VirtualServices"))).getAllElementsText("string");
                            foreach (string virtualService in virtualServices)
                                serviceModalHTML[categoryName] += virtualService + "<br/>";
                        }
                        serviceModalHTML[categoryName] += "</div>"
                            + "</div>";
                    }

                }

                foreach (String html in serviceModalHTML.Values)
                {
                    statExpandModalBody.InnerHtml = html + "</div>" + statExpandModalBody.InnerHtml;
                }

                Dictionary<string, int> userProfileRatings = new Dictionary<string, int>();

                XMLParse userInfo = new XMLParse((String)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace);

                userProfileRatings.Add("Education", Int32.Parse(userInfo.getElementText("Category_1")));
                userProfileRatings.Add("Transport", Int32.Parse(userInfo.getElementText("Category_2")));
                userProfileRatings.Add("Entertainment", Int32.Parse(userInfo.getElementText("Category_3")));
                userProfileRatings.Add("Disposal", Int32.Parse(userInfo.getElementText("Category_4")));
                userProfileRatings.Add("Local Economy", Int32.Parse(userInfo.getElementText("Category_5")));
                userProfileRatings.Add("Comms & Soc Conn", Int32.Parse(userInfo.getElementText("Category_6")));
                userProfileRatings.Add("Well being", Int32.Parse(userInfo.getElementText("Category_7")));
                userProfileRatings.Add("Psychological", Int32.Parse(userInfo.getElementText("Category_8")));
                userProfileRatings.Add("Recog", Int32.Parse(userInfo.getElementText("Category_9")));
                userProfileRatings.Add("Health", Int32.Parse(userInfo.getElementText("Category_10")));
                userProfileRatings.Add("Saftey Sec", Int32.Parse(userInfo.getElementText("Category_11")));
                userProfileRatings.Add("Consum", Int32.Parse(userInfo.getElementText("Category_12")));
                userProfileRatings.Add("Enviro Perf", Int32.Parse(userInfo.getElementText("Category_13")));
                userProfileRatings.Add("Biodi", Int32.Parse(userInfo.getElementText("Category_14")));
                userProfileRatings.Add("Govern", Int32.Parse(userInfo.getElementText("Category_15")));
                userProfileRatings.Add("Shelter", Int32.Parse(userInfo.getElementText("Category_16")));
                userProfileRatings.Add("Emotional Well", Int32.Parse(userInfo.getElementText("Category_17")));

                String userProfileRatingsJSON = JsonConvert.SerializeObject(userProfileRatings);                                                      // convert all dictionaries to JSON Strings
                String townRatingsJSON = JsonConvert.SerializeObject(categoryRating);
                String townVirtRatingsJSON = JsonConvert.SerializeObject(categoryVirtRating);

                ScriptManager.RegisterStartupScript(this, GetType(), "Services" + UniqueID, "LoadStats(" + userProfileRatingsJSON + "," + townRatingsJSON + "," + townVirtRatingsJSON + ");", true);



                /*List<String> allServe = (new XMLParse(response, SOAPRequest.soapNamespace).getWholeSection("AllServiceInfo"));

                Dictionary<String, String> serviceModalHTML = new Dictionary<String, String>();
                Dictionary<String, int> serviceCount = new Dictionary<String, int>(); 

                Dictionary<string, Dictionary<string, double>> categoryRating = new Dictionary<string, Dictionary<string, double>>();     // stores rating for each category
                                                                                                                                        // Cat name => totalRating, entryCount, virtualService
                foreach (String serviceXml in allServe)
                {
                    XMLParse servParse = new XMLParse(serviceXml, SOAPRequest.soapNamespace);
                    ArrayList categoryNames = servParse.getAllElementsText("CategoryName");
                    ArrayList subCategoryNames = servParse.getAllElementsText("SubCategoryName");

                    String serveName = servParse.getElementText("Name");

                    int uniqueCount = 0;
                    serviceCount.TryGetValue(serveName, out uniqueCount);   // if there is more than one service with the same name, this will make the name unique. 
                    serviceCount[serveName] = uniqueCount + 1;
 
                    foreach (string categoryName in categoryNames)
                    {
                        if (!categoryRating.ContainsKey(categoryName))
                            categoryRating.Add(categoryName, new Dictionary<string,double>{{"totalRating", 0},{"entryCount", 0}, {"hasVirtualService", 0}});

                        double rating;
                        if (double.TryParse(servParse.getElementText("Rating"), out rating)) 
                        {
                            if (rating > -1) 
                            {
                                categoryRating[categoryName]["totalRating"] += rating;
                                categoryRating[categoryName]["entryCount"]++;
                            }
                        }

                        Boolean hasVirtualServices = false;
                        Boolean.TryParse(servParse.getElementText("HasVirtualServices"), out hasVirtualServices);
                        if (hasVirtualServices)
                            categoryRating[categoryName]["hasVirtualService"] = 1;

                        if (!serviceModalHTML.ContainsKey(categoryName))
                            serviceModalHTML.Add(categoryName, "<div id=\"stat" + Regex.Replace(categoryName, @"\W", "") + "Modal\" class=\"hidden\">");

                        

                        serviceModalHTML[categoryName] += "<div>"
                        + "<button class=\"btn btn-default\" type=\"button\" data-toggle=\"collapse\" data-target=\"#" + Regex.Replace(serveName, @"\W", "") + uniqueCount
                        + "CollapseInfo\">"
                        + serveName + "</button>"
                        + "<div class=\"collapse\" id=\"" + Regex.Replace(serveName, @"\W", "") + uniqueCount + "CollapseInfo\">"
                        + "Town<br/>" + servParse.getElementText("Town") + ", " + servParse.getElementText("County")
                        + "<br/>Categories<br/>";
                        foreach (string category in categoryNames)
                            serviceModalHTML[categoryName] += category + "</br>";

                        serviceModalHTML[categoryName] += "<br/>SubCategories<br/>";
                        foreach (string subcategory in subCategoryNames)
                            serviceModalHTML[categoryName] += subcategory + "</br>";

                        serviceModalHTML[categoryName] += "<br/>Rating<br/>" + rating;

                        if (hasVirtualServices)
                        {
                            serviceModalHTML[categoryName] += "<br/>Virtual Services";
                            ArrayList virtualServices = (new XMLParse(servParse.getElementText("VirtualServices"))).getAllElementsText("string");
                            foreach (string virtualService in virtualServices)
                                serviceModalHTML[categoryName] += virtualService + "<br/>";
                        }
                        serviceModalHTML[categoryName] += "</div>"
                            + "</div>";

                        

                        
                    }
                }

                
                foreach (String html in serviceModalHTML.Values)
                {
                    statExpandModalBody.InnerHtml = html + "</div>" + statExpandModalBody.InnerHtml;
                }

                Dictionary<string, double> userProfileRatings = new Dictionary<string, double>();
                Dictionary<string, double> townRatings = new Dictionary<string, double>();
                Dictionary<string, double> townVirtRatings = new Dictionary<string, double>();

                foreach (KeyValuePair<string, Dictionary<string, double>> cat in categoryRating)
                {
                    if (!townRatings.ContainsKey(cat.Key))
                        townRatings.Add(cat.Key, cat.Value["totalRating"] / cat.Value["entryCount"]);

                    if (cat.Value["hasVirtualService"] == 1)
                    {
                        if (!townVirtRatings.ContainsKey(cat.Key))
                            townVirtRatings.Add(cat.Key, cat.Value["totalRating"] / cat.Value["entryCount"]);
                    }
                }

                XMLParse userInfo = new XMLParse((String)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace);

                userProfileRatings.Add("Education", int.parse(userInfo.getElementText("Category_1")));
                userProfileRatings.Add("Transport", int.parse(userInfo.getElementText("Category_2")));
                userProfileRatings.Add("Entertainment", int.parse(userInfo.getElementText("Category_3")));
                userProfileRatings.Add("Disposal", int.parse(userInfo.getElementText("Category_4")));
                userProfileRatings.Add("Local Economy", int.parse(userInfo.getElementText("Category_5")));
                userProfileRatings.Add("Comms & Soc Conn", int.parse(userInfo.getElementText("Category_6")));
                userProfileRatings.Add("Well being", int.parse(userInfo.getElementText("Category_7")));
                userProfileRatings.Add("Psychological", int.parse(userInfo.getElementText("Category_8")));
                userProfileRatings.Add("Recog", int.parse(userInfo.getElementText("Category_9")));
                userProfileRatings.Add("Health", int.parse(userInfo.getElementText("Category_10")));
                userProfileRatings.Add("Saftey Sec", int.parse(userInfo.getElementText("Category_11")));
                userProfileRatings.Add("Consum", int.parse(userInfo.getElementText("Category_12")));
                userProfileRatings.Add("Enviro Perf", int.parse(userInfo.getElementText("Category_13")));
                userProfileRatings.Add("Biodi", int.parse(userInfo.getElementText("Category_14")));
                userProfileRatings.Add("Govern", int.parse(userInfo.getElementText("Category_15")));
                userProfileRatings.Add("Shelter", int.parse(userInfo.getElementText("Category_16")));
                userProfileRatings.Add("Emotional Well", int.parse(userInfo.getElementText("Category_17")));

                String userProfileRatingsJSON = JsonConvert.SerializeObject(userProfileRatings);                                                      // convert all dictionaries to JSON Strings
                String townRatingsJSON = JsonConvert.SerializeObject(townRatings);
                String townVirtRatingsJSON = JsonConvert.SerializeObject(townVirtRatings);

                ScriptManager.RegisterStartupScript(statsTownList, statsTownList.GetType(), "Services" + UniqueID, "LoadStats(" + userProfileRatingsJSON + "," + townRatingsJSON + "," + townVirtRatingsJSON + ");", true);*/




            }
        }

        private String catAverage(ArrayList input)
        {
            return ((int)input[0] / (int)input[1]).ToString();
        }

        protected void statsTownList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}