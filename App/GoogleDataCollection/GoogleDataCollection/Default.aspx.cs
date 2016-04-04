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
            String ErrorString = "";
            String TempError = "";
            String soapBody = "";
            //string[] uploadedServices = placeData.Value.Split(new Char[] { ';' });
            String rawServiceText = ServiceData.Value;
            string folderPath = Server.MapPath("");
            ArrayList services = new ArrayList();

            try
            {
                TempError = "";
                rawService rawServiceOb = JsonConvert.DeserializeObject<rawService>(rawServiceText);
                town townOb = JsonConvert.DeserializeObject<town>(TownData.Value);

                TempError = "String to Object Completed\n";

                service serviceToStore = new service();
                serviceToStore.place_id = rawServiceOb.place_id;
                serviceToStore.name = rawServiceOb.name;
                serviceToStore.formatted_address = rawServiceOb.formatted_address;
                serviceToStore.formatted_phone_number = rawServiceOb.formatted_phone_number;
                serviceToStore.website = rawServiceOb.website;
                serviceToStore.location = new latLng(rawServiceOb.geometry.location.G, rawServiceOb.geometry.location.K);

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
                        aspectRating = ((double)totalAspectRating / (double)aspectRatingCount) * 10;
                }

                TempError += "initalRating: " + initalRating + ", aspectRating: " + aspectRating + ", aspectRatingCount: " + aspectRatingCount + "\n";

                

                int divisor = 2;
                if (initalRating < 1 || aspectRating < 1)
                    divisor = 1;

                if (!Double.TryParse("" + ((initalRating + aspectRating) / divisor), out serviceToStore.rating)) // average of them both to create the rating
                    serviceToStore.rating = (double)0;

                if (serviceToStore.rating == 0 && aspectRatingCount == 0)       // we can assume that if the rating is zero, and there are no aspect or review ratings, that the service has not been rated, 
                    serviceToStore.rating = -1;                                 // rather than been rated 0

                

                TempError += "calculated Rating: " + serviceToStore.rating + "\n";

                TempError += "Rating set\n";

                string typesFilename = "initial_categorisation.txt";
                string typesPath = Path.Combine(folderPath, typesFilename);

                string[] categoriesAndTypes = Regex.Replace(File.ReadAllText(typesPath, System.Text.Encoding.ASCII), @"\t|\n|\r", "").Split(new Char[] { ';' });

                ArrayList splitTypes = new ArrayList();

                for (int i = 0; i < categoriesAndTypes.Length; i++)
                {
                    splitTypes.Add(categoriesAndTypes[i].Split(new Char[] { ',' }));
                }

                Dictionary<String, ArrayList> tempcategory = new Dictionary<String, ArrayList>();
                foreach (String[] categoryArray in splitTypes) // array of types and their category (index 0 is the category name)
                {
                    foreach (String type in categoryArray) // each type string
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
                foreach (KeyValuePair<String, ArrayList> pair in tempcategory)
                {
                    tempAllcats.Add(new category(pair.Key, (String[])pair.Value.ToArray(typeof(String))));
                }
                serviceToStore.categories = (category[])tempAllcats.ToArray(typeof(category));

                TempError += "Categories Set\n";

                String action = "SetNewService";
                String serviceID = "-1";

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

                String response = HttpSOAPRequest(soapBody.Replace("'", "''"), action);

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
                foreach (KeyValuePair<String, ArrayList> cat in tempcategory)
                {
                    foreach (String subcat in cat.Value)
                    {
                        if (!arrayListContainsCaseInsensitve(allCapturedTypes, subcat))
                            allCapturedTypes.Add(subcat);

                        if (arrayListContainsCaseInsensitve(allsubcatnames, subcat))
                        {
                            String tempsubcatID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", subcat, "SubCategoryID");

                            // only add if this subcat is not already paired with this service and if it has not already been added to the new ID's
                            if (!arrayListContainsCaseInsensitve(currentSubCatNames, subcat) && !arrayListContainsCaseInsensitve(subcatIDs, tempsubcatID)) 
                                subcatIDs.Add(tempsubcatID);
                            
                        }
                    }
                }

                TempError += "pruning subcategory/service pairs\n"; 

                //TODO: test pruning - later
                foreach (String currentName in currentSubCatNames)
                {
                    // check if the name is in the current capured list or not
                    if (!arrayListContainsCaseInsensitve(allCapturedTypes, currentName))
                    {
                        String tempsubcatID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", currentName, "SubCategoryID");
                        // delete it, this subcategory is no longer linked to this service
                        String SOAPbdy = @" <pair>
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

                ErrorString = "\n-----New Entry-----\n" + TempError;

            }
            catch (Exception ex) {
                ErrorString = "\n------New Error-------\nRaw Data: " + rawServiceText + "\nSOAP:\n" + soapBody + "\nLog:\n";
                ErrorString += TempError + "\nException:\n";
                ErrorString += ex.ToString();
            }


            //errorText.Value += ErrorString;

            StreamWriter objWriter = new StreamWriter("c:\\report.txt", true);

            // write a line of text to the file
            objWriter.WriteLine(ErrorString);

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
            string lat = town.geometry.location.G;
            string lng = town.geometry.location.K;
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
                String soapBody = "<Town>" + new XElement("TownID", "-1") + 
                    new XElement("Town", townName) + new XElement("County", county) +
                    new XElement("Latitude", town.geometry.location.G) + 
                    new XElement("Longitude", town.geometry.location.K) +
                    "</Town>";

                HttpSOAPRequest(soapBody, "SetNewTown");

                return getTownID(town);

            }
        }
        private void updateCategories(Dictionary<String, ArrayList> catAndSubCat)
        {
            foreach (KeyValuePair<String, ArrayList> cat in catAndSubCat)
            {
                // grab all the current categories
                XMLParse allcategories = new XMLParse(HttpSOAPRequest("", "GetListOfCategories"));
                ArrayList allcatnames = allcategories.getAllElementsText("CategoryName");
                
                // update the subcategories and then grab all the subcategories
                updateSubCategories(cat.Value);
                XMLParse allsubcats = new XMLParse(HttpSOAPRequest("", "GetListOfSubCategories"));
                ArrayList allsubcatnames = allsubcats.getAllElementsText("SubCategoryName");

                String catID = "-1";
                ArrayList subcatIDs = new ArrayList();

                // if this category is already present
                if (arrayListContainsCaseInsensitve(allcatnames, cat.Key))
                {
                    catID = allcategories.getElementFromSibling("Category", "CategoryName", cat.Key, "CategoryID"); // Category ID
                    XMLParse currentSubCats = new XMLParse(HttpSOAPRequest("<CategoryID>" + catID + "</CategoryID>", "GetListOfSubCategoriesByCategoryID")); // get all subcategories this cat already has
                    ArrayList allCurrentSubCatNames = currentSubCats.getAllElementsText("SubCategoryName"); // get their names

                    // loop each type (subcategory) passed
                    foreach (String type in cat.Value)
                    {
                        // make sure this subcategory exists in the database
                        if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                        {
                            // if this type is not currently a subcategory for this category
                            if (!arrayListContainsCaseInsensitve(allCurrentSubCatNames, type))
                            {
                                String tempID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", type, "SubCategoryID");
                                String SOAPbdy = @" <pair>
                                                        <CategoryID>" + catID + @"</CategoryID>
                                                        <SubCategoryID>" + tempID + @"</SubCategoryID>
                                                    </pair>";
                                HttpSOAPRequest(SOAPbdy, "SetNewCategorySubCategoryPair"); // create a new pair refrence
                            }
                        }
                    }
                }
                else // create new category
                {
                    String SOAPbdy = @" <Category>
                                            <CategoryID>" + catID + @"</CategoryID>
                                            <CategoryName>" + cat.Key + @"</CategoryName>
                                        </Category>";
                    HttpSOAPRequest(SOAPbdy, "SetNewCategory");

                    foreach (String type in cat.Value)
                    {
                        // make sure this subcategory exists in the database
                        if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                        {
                            String tempID = allsubcats.getElementFromSibling("SubCategory", "SubCategoryName", type, "SubCategoryID");
                            SOAPbdy = @"<pair>
                                            <CategoryID>" + catID + @"</CategoryID>
                                            <SubCategoryID>" + tempID + @"</SubCategoryID>
                                        </pair>";
                            HttpSOAPRequest(SOAPbdy, "SetNewCategorySubCategoryPair"); // create the pair
                        }
                    }


                }





                    /*subcatIDs = new XMLParse(allcategories.getElementFromSibling("Category", "CategoryName", cat.Key, "SubCategories", true)).getWholeSection("string");
                    

                    foreach (String type in cat.Value)
                    {
                        if (arrayListContainsCaseInsensitve(allsubcatnames, type))
                        {
                            String tempID = allsubcats.getElementFromSibling("SubCategories", "SubCategoryName", type, "SubCategoryID");
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
                    String soapBody = @"<Category>
                                            <CategoryID>" + catID + @"</CategoryID>
                                            <CategoryName>" + cat.Key + @"</CategoryName>
                                            <SubCategories>";

                    foreach (String subcatID in subcatIDs)
                        soapBody += "<string>" + subcatID + "</string>";

                    foreach(String type in cat.Value)
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
            foreach (String type in types)
            {
                if (!arrayListContainsCaseInsensitve(names, type))
                {
                    // Create a new subcategory if the name is not in the list
                    String soapBody = @"<Subcategory>
                                            <SubCategoryID>-1</SubCategoryID>
                                            <SubCategoryName>" + type + @"</SubCategoryName>
                                        </Subcategory>";
                    HttpSOAPRequest(soapBody, "SetNewSubCategory");
                }
            }

        }

        private Boolean arrayListContainsCaseInsensitve(ArrayList al, String ob)
        {
            foreach (String entry in al)
                if(string.Equals(entry, ob, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

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