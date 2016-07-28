using GoogleDataCollection.Town;
using GoogleDataCollection.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml.Linq;

namespace GoogleDataCollection
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "map" + UniqueID, "loadMap();", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "map" + UniqueID, "loadOverlayInit();", true);

            //doJsonRead();
        }

        protected void uploadResults_Click(object sender, EventArgs e)
        {
            string Errorstring = "";
            string TempError = "";
            string soapBody = "";
            //string[] uploadedServices = placeData.Value.Split(new Char[] { ';' });
            string rawServiceText = ServiceData.Value;
            string folderPath = Server.MapPath("");
            ArrayList services = new ArrayList();

            try
            {
                TempError = "";
                rawService rawServiceOb = JsonConvert.DeserializeObject<rawService>(rawServiceText);
                town townOb = JsonConvert.DeserializeObject<town>(TownData.Value);

                TempError = "string to Object Completed\n";

                service serviceToStore = new service();
                serviceToStore.place_id = rawServiceOb.place_id;
                serviceToStore.name = rawServiceOb.name;
                serviceToStore.formatted_address = rawServiceOb.formatted_address;
                serviceToStore.formatted_phone_number = rawServiceOb.formatted_phone_number;
                serviceToStore.website = rawServiceOb.website;
                //serviceToStore.location = new latLng(rawServiceOb.geometry.location.G, rawServiceOb.geometry.location.K);
                serviceToStore.location = new latLng(rawServiceOb.geometry.location.lat, rawServiceOb.geometry.location.lng);
                //serviceToStore.location = new latLng(rawServiceOb.geometry.access_points[0].location.lat, rawServiceOb.geometry.access_points[0].location.lat);

                TempError += "Primitive data set\n";

                double initalRating = rawServiceOb.rating * 2; // 0 - 5 --> 0 - 10
                double totalAspectRating = 0;
                int aspectRatingCount = 0;
                double aspectRating = 0;
                bool hasAspects = false;

                if (rawServiceOb.aspects != null)
                {
                    hasAspects = true;
                    foreach (aspects aspect in rawServiceOb.aspects)
                    {
                        //if (aspect.type.Equals("overall"))
                        //{
                            totalAspectRating += aspect.rating;
                            aspectRatingCount++;
                       // }
                    }
                }

                if (rawServiceOb.reviews != null)
                {
                    foreach (reviews review in rawServiceOb.reviews)
                    {
                        foreach (aspects aspect in review.aspects)
                        {
                            hasAspects = true;
                            //if (aspect.type.Equals("overall")) // only bother with overall, we don't need to much fidelity
                            //{
                                totalAspectRating += aspect.rating;
                                aspectRatingCount++;
                           // }
                        }

                    }

                    aspectRatingCount = aspectRatingCount * 3; // max for each aspect is 3.
                    if (totalAspectRating > 0 && aspectRatingCount > 0)
                        aspectRating = totalAspectRating / aspectRatingCount * 10;
                }

                TempError += "initalRating: " + initalRating + ", aspectRating: " + aspectRating + ", aspectRatingCount: " + aspectRatingCount + "\n";

                

                int divisor = 2;
                if (initalRating < 1 || aspectRating < 1)
                    divisor = 1;

                if (!double.TryParse("" + ((initalRating + aspectRating) / divisor), out serviceToStore.rating)) // average of them both to create the rating
                    serviceToStore.rating = (double)0;

                if (serviceToStore.rating == 0 && aspectRatingCount == 0)       // we can assume that if the rating is zero, and there are no aspect or review ratings, that the service has not been rated, 
                    serviceToStore.rating = -1;                                 // rather than been rated 0

                

                TempError += "calculated Rating: " + serviceToStore.rating + "\n";

                TempError += "Rating set\n";

                string typesFilename = "initial_categorisation.txt";
                string typesPath = Path.Combine(folderPath, typesFilename);

                string[] categoriesAndTypes = Regex.Replace(File.ReadAllText(typesPath, Encoding.ASCII), @"\t|\n|\r", "").Split(new char[] { ';' });

                ArrayList splitTypes = new ArrayList();

                for (int i = 0; i < categoriesAndTypes.Length; i++)
                {
                    splitTypes.Add(categoriesAndTypes[i].Split(new Char[] { ',' }));
                }

                Dictionary<string, ArrayList> tempcategory = new Dictionary<string, ArrayList>();
                foreach (string[] categoryArray in splitTypes) // array of types and their category (index 0 is the category name)
                {
                    foreach (string type in categoryArray) // each type string
                    {
                        if (rawServiceOb.types.Contains(type)) // this service has this type
                        {
                            if (tempcategory.ContainsKey(categoryArray[0])) // already an entry 
                            {
                                if (!tempcategory[categoryArray[0]].Contains(type))
                                    tempcategory[categoryArray[0]].Add(type);
                            }
                            else
                            {
                                tempcategory.Add(categoryArray[0], new ArrayList() { type });
                            }

                        }
                    }

                }

                ArrayList tempAllcats = new ArrayList();
                foreach (KeyValuePair<string, ArrayList> pair in tempcategory)
                {
                    tempAllcats.Add(new category(pair.Key, (string[])pair.Value.ToArray(typeof(string))));
                }
                serviceToStore.categories = (category[])tempAllcats.ToArray(typeof(category));

                TempError += "Categories Set\n";

                string action = "SetNewService";
                string serviceID = "-1";

                TempError += "Getting Town...\n";
                int townID = getTownID(townOb);

                if (Session["TownID"] == null)
                    Session["TownID"] = townID;

                TempError += "services for town HTTP request\n";
                //TODO: webservice, "GetServiceByTownIDIfExists"
                XMLParse Allservices = new XMLParse(HttpSOAPRequest("<TownID>" + townID + "</TownID>", "GetAllServicesForTownByID"));
                ArrayList allservicenames = Allservices.getAllElementsText("Name");

                TempError += "Checking if update or new service\n";
                if (arrayListContainsCaseInsensitve(allservicenames, serviceToStore.name))
                {
                    action = "UpdateTownServices";
                    serviceID = Allservices.getElementFromSibling("Service", "Name", serviceToStore.name, "ServiceID");
                }

                soapBody = "<Service>" +
                                new XElement("ServiceID", serviceID) +
                                new XElement("Name", serviceToStore.name) +
                                new XElement("TownID", townID);
         
                soapBody += "" + 
                    new XElement("Rating", serviceToStore.rating) +
                    new XElement("Latitude", serviceToStore.location.lat) +
                    new XElement("Longitude", serviceToStore.location.lng) +
                    new XElement("HasPerimeter", 0);

                TempError += "Category and subcategory added to soapBody\n";

                TempError += "Adding virtual Services\n";
                if (serviceToStore.website != null)
                {
                    soapBody += new XElement("HasVirtualServices", 1) +
                                "<VirtualServices>" +
                                new XElement("string", serviceToStore.website);
                }
                else soapBody += new XElement("HasVirtualServices", 0) +
                                "<VirtualServices>";


                soapBody += "</VirtualServices></Service>";

                TempError += "Sending service set soap request\n";

                string response = HttpSOAPRequest(soapBody.Replace("'", "''"), action);

                TempError += "Response: " + response + "\n";

                if (serviceID.Equals("-1")) 
                    serviceID = (new XMLParse(response)).getElementText("SetNewServiceResult");

                TempError += "Updating categoies and subcategories\n";

                updateCategories(tempcategory);

                XMLParse allsubcats = new XMLParse(HttpSOAPRequest("", "GetListOfSubCategories"));
                ArrayList allsubcatnames = allsubcats.getAllElementsText("SubCategoryName");
                XMLParse currentSubCats = new XMLParse(HttpSOAPRequest("<ServiceID>" + serviceID + "</ServiceID>", "GetListOfSubCategoriesByServiceID")); // get all subcategories this service already has
                ArrayList currentSubCatNames = currentSubCats.getAllElementsText("SubCategoryName"); // get their names
                ArrayList subcatIDs = new ArrayList();
                ArrayList allCapturedTypes = new ArrayList(); // storing all the captured types from google, for pruning


                TempError += "Adding new subcategory IDs\n";
                foreach (KeyValuePair<string, ArrayList> cat in tempcategory)
                {
                    foreach (string subcat in cat.Value)
                    {
                        if (!arrayListContainsCaseInsensitve(allCapturedTypes, subcat))
                            allCapturedTypes.Add(subcat);

                        if (arrayListContainsCaseInsensitve(allsubcatnames, subcat))
                        {
                            string tempsubcatID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", subcat, "SubCategoryID");

                            // only add if this subcat is not already paired with this service and if it has not already been added to the new ID's
                            if (!arrayListContainsCaseInsensitve(currentSubCatNames, subcat) && !arrayListContainsCaseInsensitve(subcatIDs, tempsubcatID)) 
                                subcatIDs.Add(tempsubcatID);
                            
                        }
                    }
                }

                TempError += "pruning subcategory/service pairs\n"; 

                //TODO: test pruning - later
                foreach (string currentName in currentSubCatNames)
                {
                    // check if the name is in the current capured list or not
                    if (!arrayListContainsCaseInsensitve(allCapturedTypes, currentName))
                    {
                        string tempsubcatID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", currentName, "SubCategoryID");
                        // delete it, this subcategory is no longer linked to this service
                        string SOAPbdy = @" <pair>
                                                <ServiceID>" + serviceID + @"</ServiceID>
                                                <SubCategoryID>" + tempsubcatID + @"</SubCategoryID>
                                            </pair>";
                        HttpSOAPRequest(SOAPbdy, "DeleteServiceSubCategoryPair"); // delete the pair refrence

                    }

                }


                TempError += "creating subcategory/service pairs\n"; 

                foreach (string id in subcatIDs) 
                {
                    string SOAPbdy = @" <pair>
                                            <ServiceID>" + serviceID + @"</ServiceID>
                                            <SubCategoryID>" + id + @"</SubCategoryID>
                                        </pair>";
                    HttpSOAPRequest(SOAPbdy, "SetNewSubCategoryServicePair"); // create a new pair refrence
                }

                TempError += "Done\n";

                Errorstring = "\n-----New Entry-----\n" + TempError;

            }
            catch (Exception ex) {
                Errorstring = "\n------New Error-------\nRaw Data: " + rawServiceText + "\nSOAP:\n" + soapBody + "\nLog:\n";
                Errorstring += TempError + "\nException:\n";
                Errorstring += ex.ToString();
            }


            //errorText.Value += Errorstring;

            StreamWriter objWriter = new StreamWriter("c:\\report.txt", true);

            // write a line of text to the file
            objWriter.WriteLine(Errorstring);

            // close the stream
            objWriter.Close();
        }

        protected void UpdateCountAndNormal_Click(object sender, EventArgs e)
        {
            
            HttpSOAPRequest("<TownID>" + Session["TownID"] + "</TownID>", "UpdateTownServiceCount");
            HttpSOAPRequest("", "UpdateTownNormalisations");

            Session["TownID"] = null;

            ScriptManager.RegisterStartupScript(UpdateCountAndNormal, UpdateCountAndNormal.GetType(), "updateCountNormal" + UniqueID, "uploadFinalReturn();", true);
        }

        private int getTownID(town town)
        {
            XMLParse towns = new XMLParse(HttpSOAPRequest("", "GetListOfTowns"));
            ArrayList townNames = towns.getAllElementsText("Town");
            int townID = -1;

            string townName = "";
            string county = "";
            //string lat = town.geometry.location.G;
            //string lng = town.geometry.location.K;
            string lat = town.geometry.location.lat;
            string lng = town.geometry.location.lng;
            foreach (address_componets com in town.address_components)
            {
                if (arrayListContainsCaseInsensitve(new ArrayList(com.types), "locality"))
                    townName = com.long_name;

                if (arrayListContainsCaseInsensitve(new ArrayList(com.types), "administrative_area_level_2"))
                    county = com.long_name;
                
            }

            if (arrayListContainsCaseInsensitve(townNames, townName))
                int.TryParse(towns.getElementFromSibling("Towns", "Town", townName, "TownID"), out townID);

            if (townID > -1)
                return townID;
            else
            {
                string soapBody = "<Town>" + new XElement("TownID", "-1") + 
                    new XElement("Town", townName) + new XElement("County", county) +
                    //new XElement("Latitude", town.geometry.location.G) + 
                    //new XElement("Longitude", town.geometry.location.K) +
                    new XElement("Latitude", town.geometry.location.lat) +
                    new XElement("Longitude", town.geometry.location.lng) +
                    "</Town>";

                HttpSOAPRequest(soapBody, "SetNewTown");

                return getTownID(town);

            }
        }
        private void updateCategories(Dictionary<string, ArrayList> catAndSubCat)
        { 
            foreach (KeyValuePair<string, ArrayList> cat in catAndSubCat)
            {
                // grab all the current categories
                XMLParse allcategories = new XMLParse(HttpSOAPRequest("", "GetListOfCategories"));
                ArrayList allcatnames = allcategories.getAllElementsText("CategoryName");
                
                // update the subcategories and then grab all the subcategories
                updateSubCategories(cat.Value);
                XMLParse allsubcats = new XMLParse(HttpSOAPRequest("", "GetListOfSubCategories"));
                ArrayList allsubcatnames = allsubcats.getAllElementsText("SubCategoryName");

                string catID = "-1";
                ArrayList subcatIDs = new ArrayList();

                // if this category is already present
                if (arrayListContainsCaseInsensitve(allcatnames, cat.Key))
                {
                    catID = allcategories.getElementFromSibling("Category", "CategoryName", cat.Key, "CategoryID"); // Category ID
                    if (catID != "-1") // if the ID wasn't found
                    {
                        XMLParse currentSubCats = new XMLParse(HttpSOAPRequest("<CategoryID>" + catID + "</CategoryID>", "GetListOfSubCategoriesByCategoryID")); // get all subcategories this cat already has
                        ArrayList allCurrentSubCatNames = currentSubCats.getAllElementsText("SubCategoryName"); // get their names

                        // loop each type (subcategory) passed
                        foreach (string type in cat.Value)
                        {
                            // make sure this subcategory exists in the database
                            if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                            {
                                // if this type is not currently a subcategory for this category
                                if (!arrayListContainsCaseInsensitve(allCurrentSubCatNames, type))
                                {
                                    string tempID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", type, "SubCategoryID");
                                    string SOAPbdy = @" <pair>
                                                        <CategoryID>" + catID + @"</CategoryID>
                                                        <SubCategoryID>" + tempID + @"</SubCategoryID>
                                                    </pair>";
                                    HttpSOAPRequest(SOAPbdy, "SetNewCategorySubCategoryPair"); // create a new pair refrence
                                }
                            }
                        }
                    }
                }
                else // create new category
                {
                    string SOAPbdy = @" <Category>
                                            <CategoryID>" + catID + @"</CategoryID>
                                            <CategoryName>" + cat.Key + @"</CategoryName>
                                        </Category>";
                    HttpSOAPRequest(SOAPbdy, "SetNewCategory");

                    foreach (string type in cat.Value)
                    {
                        // make sure this subcategory exists in the database
                        if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                        {
                            string tempID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", type, "SubCategoryID");
                            SOAPbdy = @"<pair>
                                            <CategoryID>" + catID + @"</CategoryID>
                                            <SubCategoryID>" + tempID + @"</SubCategoryID>
                                        </pair>";
                            HttpSOAPRequest(SOAPbdy, "SetNewCategorySubCategoryPair"); // create the pair
                        }
                    }


                }





                    /*subcatIDs = new XMLParse(allcategories.getElementFromSibling("Category", "CategoryName", cat.Key, "SubCategories", true)).getWholeSection("string");
                    

                    foreach (string type in cat.Value)
                    {
                        if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                        {
                            string tempID = allsubcats.getElementFromSibling("SubCategories", "SubCategoryName", type, "SubCategoryID");
                            if (!arrayListContainsCaseInsensitve(subcatIDs, tempID))
                            {
                                action = "UpdateCategory";
                                break;
                            }
                        }

                    }


                }
                
                if (action.Length > 1)
                {
                    string soapBody = @"<Category>
                                            <CategoryID>" + catID + @"</CategoryID>
                                            <CategoryName>" + cat.Key + @"</CategoryName>
                                            <SubCategories>";

                    foreach (string subcatID in subcatIDs)
                        soapBody += "<string>" + subcatID + "</string>";

                    foreach(string type in cat.Value)
                    {
                        if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                        {
                            int tempID;
                            if (int.TryParse(allsubcats.getElementFromSibling("SubCategories", "SubCategoryName", type, "SubCategoryID"), out tempID))
                            {
                                if (!arrayListContainsCaseInsensitve(subcatIDs, ""+tempID))
                                    soapBody += "<string>" + tempID + "</string>";
                            }
                        }
                    }

                    soapBody += @"  </SubCategories>
                                </Category>";

                    HttpSOAPRequest(soapBody, action);
                         
                }*/
            }
        }

        // Creates any new subcategories in the database if needed, based on a passed arraylist of names
        private void updateSubCategories (ArrayList types)
        {
            // grab all subcategories, then grab all the names
            XMLParse allsubcats = new XMLParse(HttpSOAPRequest("", "GetListOfSubCategories"));
            ArrayList names = allsubcats.getAllElementsText("SubCategoryName");
            
            // for each of the passed types check if it exists in the list of names
            foreach (string type in types)
            {
                if (!arrayListContainsCaseInsensitve(names, type))
                {
                    // Create a new subcategory if the name is not in the list
                    string soapBody = @"<Subcategory>
                                            <SubCategoryID>-1</SubCategoryID>
                                            <SubCategoryName>" + type + @"</SubCategoryName>
                                        </Subcategory>";
                    HttpSOAPRequest(soapBody, "SetNewSubCategory");
                }
            }

        }

        private bool arrayListContainsCaseInsensitve(ArrayList al, string ob)
        {
            foreach (string entry in al)
                if(string.Equals(entry, ob, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        public string HttpSOAPRequest(string body, string action)
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


            //Pass the SoapRequest string to the WebService
            StreamWriter stmw = new StreamWriter(req.GetRequestStream());
            stmw.Write(SOAPReq);
            stmw.Flush();

            WebResponse resp = req.GetResponse();
            StreamReader r = new StreamReader(resp.GetResponseStream());

            return r.ReadToEnd();



        }

        
    }
}