using Newtonsoft.Json;
using SocialApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Xml.Serialization;

namespace SocialApp.Pages
{
    public partial class Maps : System.Web.UI.Page
    {
        private SiteMaster thisMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            thisMaster = Master as SiteMaster;
            thisMaster.setChild(this);

            thisMaster.headersHidden();

            if (!IsPostBack)
                fillTownList();

            //mapMainUpdatePanel.Update();

        }

        private void fillTownList()
        {
            if (Session[Paths.TOWNLIST] == null)
                Session[Paths.TOWNLIST] = SOAPRequest.getTownList();    // grab the lost of available towns

            if (Session[Paths.TOWNLIST] == null) mapsTownList.Items.Add("No Towns"); // if something has gone wrong, still null
            else
            {
                ArrayList towns = (new XMLParse((String)Session[Paths.TOWNLIST], SOAPRequest.soapNamespace)).getAllElementsText("Town");    // pull out all the town names

                // add em to the options of the dropdown component, including home
                mapsTownList.Items.Add("Home");
                foreach (String town in towns)
                {
                    mapsTownList.Items.Add(town);
                }

                loadTownInfo(mapsTownList.SelectedValue); // load the info for the inially selected value
            }

        }

        private void loadTownInfo(String town)
        {
            //TODO: error log/alert messages

            XMLParse townList = new XMLParse((String)Session[Paths.TOWNLIST], SOAPRequest.soapNamespace); // set up the town list for parsing
            String lat = "";
            String lng = "";
            String home = "false";  // flag for if the selected town is their home


            if (town.Equals("Home"))
            {
               
                XMLParse geoLoc = new XMLParse((string) Session[Paths.USERGEOLOC]);

                lat = geoLoc.getElementText("lat", "location");                                                 // pull out lat and lng
                lng = geoLoc.getElementText("lng", "location");
                home = "true";
                //town = user.getElementText("Town");                                                             // set the town
                town = geoLoc.getElementFromSibling("address_component", "type", "postal_town", "long_name");



            }
            else
            {                                                                                                   // the selected option is not home
                lat = townList.getElementFromSibling("Towns", "Town", town, "Latitude");                        // pull lat and lng
                lng = townList.getElementFromSibling("Towns", "Town", town, "Longitude");
            }

            // TODO: move map rather than reload each time - didn't seem to work
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "map" + UniqueID, "setMapView(" + lat + "," + lng + "," + home + ");", true); // javascript function to move the map view
            ScriptManager.RegisterStartupScript(this, GetType(), "map" + UniqueID, "loadMap(" + lat + "," + lng + "," + home + ");", true);    

            // pulling service info from the database

            String id = townList.getElementFromSibling("Towns", "Town", town, "TownID");                        // extract the town ID
            HTTPRequest req = new HTTPRequest();
            String response = req.HttpSOAPRequest("<TownID>" + id + "</TownID>", "GetAllServicesForTownByID");  // request all the service info for that town
            String result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("TownID");       // check reply has data for the correct town

            int tID = 0;
            int returnTID = 0;
            Int32.TryParse(id, out tID);
            Int32.TryParse(result, out returnTID);  // safe parsing


            if (tID == returnTID)
            {

                List<String> allServe = (new XMLParse(response, SOAPRequest.soapNamespace).getWholeSection("AllServiceInfo")); // get list object of all the services 

                Dictionary<string, List<string>> allCategories = new Dictionary<string, List<string>>();        // Key: Category name, Value: List of subcategory names related to the category
                Dictionary<string, List<string>> allSubCategories = new Dictionary<string, List<string>>();     // Key: Subcategory name, Value: list of service ID's which have that subcategory
                Dictionary<string, string> allServices = new Dictionary<string, string>();                      // Key: Service ID, Value: service data (JSON object/string)


                foreach (String serviceXml in allServe)                                                                                 // for each service in the array of town services
                {
                    XMLParse serviceInfoParse = new XMLParse(serviceXml, SOAPRequest.soapNamespace);                                    // XML parse for entire service info
                    XMLParse serviceParse = new XMLParse(serviceInfoParse.getWholeSection("Service")[0], SOAPRequest.soapNamespace);    // XML parse for only the service data, not related town or category sections.

                    List<String> serviceCategories = serviceInfoParse.getWholeSection("Category");                                      // create array of category info
                    List<String> serviceSubCategories = serviceInfoParse.getWholeSection("SubCategory");                                // ^^^^ subcategories

                    String serviceID = serviceParse.getElementText("ServiceID");                                                        // grab the service ID

                    List<string> allCategoryNames = new List<string>();                                                                 // store all the names of the categories and subcategories 
                    List<string> allSubCategoryNames = new List<string>();                                                              // for adding to the XML later


                    foreach (String category in serviceCategories)                                                                      // loop through each of the categories returned with this service
                    {

                        XMLParse catXml = new XMLParse(category, SOAPRequest.soapNamespace);                                            // pull out the name and the list of subcategory ID's
                        String catName = catXml.getElementText("CategoryName");
                        String catID = catXml.getElementText("CategoryID");

                        if (!HelperMethods.List_Contains_Caseinsensitve(allCategoryNames, catName))
                            allCategoryNames.Add(catName);                                                                              // add this category name if needed

                        XMLParse allSubCatXML = new XMLParse((new HTTPRequest()).HttpSOAPRequest("<CategoryID>" + catID + "</CategoryID>", "GetListOfSubCategoriesByCategoryID"), SOAPRequest.soapNamespace);
                        List<string> subcatIDs = ((string[])allSubCatXML.getAllElementsText("SubCategoryID").ToArray(typeof(string))).ToList<string>();

                        if (!allCategories.ContainsKey(catName))
                            allCategories.Add(catName, new List<string>());                                                             // add new entry if needed

                        foreach (string subcategory in serviceSubCategories)                                                            // loop through all the subcategories returned with this service
                        {
                            XMLParse subcatXml = new XMLParse(subcategory, SOAPRequest.soapNamespace);                                  // pull name and ID
                            string subcatID = subcatXml.getElementText("SubCategoryID");
                            string subcatName = subcatXml.getElementText("SubCategoryName");

                            if (!HelperMethods.List_Contains_Caseinsensitve(allSubCategoryNames, subcatName))
                                allSubCategoryNames.Add(subcatName);                                                                    // add this subcategory name if needed

                            if (HelperMethods.List_Contains_Caseinsensitve(subcatIDs, subcatID))                                        // check if this subcategory is under the current category
                            {
                                if (!allCategories[catName].Contains(subcatName))                                                       // name subcategory name to the list if needed
                                    allCategories[catName].Add(subcatName);

                                if (!allSubCategories.ContainsKey(subcatName))                                                          // create new entry in subcategory dictionary if needed
                                    allSubCategories.Add(subcatName, new List<string>());

                                if (!allSubCategories[subcatName].Contains(serviceID))                                                  // add the service id to the list if needed
                                    allSubCategories[subcatName].Add(serviceID);
                            }
                        }
                    }


                    serviceParse.AddElement("Service", "CategoryNames");
                    foreach (string cat in allCategoryNames)
                        serviceParse.AddElement("CategoryNames", "String", cat);

                    serviceParse.AddElement("Service", "SubCategoryNames");
                    foreach (string subcat in allSubCategoryNames)
                        serviceParse.AddElement("SubCategoryNames", "String", subcat);

                    serviceParse.getElementText("CategoryNames");

                    if (!allServices.ContainsKey(serviceID))
                        allServices.Add(serviceID, serviceParse.convertToJSON());                                                       // add the entry to the services dictionary
                }

                string allServicesJSON = JsonConvert.SerializeObject(allServices);                                                      // convert all dictionaries to JSON Strings
                string allCategoriesJSON = JsonConvert.SerializeObject(allCategories);
                string allSubCategoriesJSON = JsonConvert.SerializeObject(allSubCategories);

                // Javascript function to load the overlay, takes the above generated info. 
                ScriptManager.RegisterStartupScript(mapInputUpdatePanel, mapInputUpdatePanel.GetType(), "Services" + UniqueID, "setOverlayInfo(" + allServicesJSON + "," + allCategoriesJSON + "," + allSubCategoriesJSON + ");", true);
            }
        }
        protected void mapsTownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadTownInfo(mapsTownList.SelectedValue);
        }


        public void getRouteReportInfo()
        {
            //JsonConvert.
        }




        public ArrayList tempResponse()
        {
            string folderPath = Server.MapPath("");
            string filename = "Hatfield Hertfordshire, UK.xml";
            String[] entries = null;

            using (var stream = File.OpenRead(Path.Combine(folderPath, filename)))
            {
                var serializer = new XmlSerializer(typeof(String[]));
                entries = (String[])serializer.Deserialize(stream);
                stream.Flush();
                stream.Close();
            }



            ArrayList tempobjects = new ArrayList();

            foreach (String ob in entries)
                tempobjects.Add(JsonConvert.DeserializeObject<Utils.tempObject>(ob));

            Dictionary<String, ArrayList> categories = new Dictionary<String, ArrayList>();
            ArrayList subcategories = new ArrayList();

            foreach (tempObject ob in tempobjects)
            {
                for (int i = 0; i < ob.categories.Length; i++)
                {
                    if (!categories.ContainsKey(ob.categories[i].categoryName)) categories.Add(ob.categories[i].categoryName, new ArrayList());

                    ArrayList subs;
                    if (categories.TryGetValue(ob.categories[i].categoryName, out subs))
                    {
                        foreach (String sub in ob.categories[i].subCategories)
                        {
                            if (!subcategories.Contains(sub)) subcategories.Add(sub);
                            if (!subs.Contains(subcategories.IndexOf(sub))) subs.Add(subcategories.IndexOf(sub));
                        }
                    }
                }
            }

            ArrayList categoryObs = new ArrayList();
            ArrayList subcategoryObs = new ArrayList();

            int count = 0;
            foreach (KeyValuePair<String, ArrayList> kv in categories)
            {
                category c = new category();
                c.name = kv.Key;
                c.id = count;
                c.subcategories = (int[])kv.Value.ToArray(typeof(int));
                categoryObs.Add(JsonConvert.SerializeObject(c));
                count++;
            }

            for (int sc = 0; sc < subcategories.Count; sc++)
            {
                subcategory sub = new subcategory();
                sub.id = sc;
                sub.name = subcategories[sc].ToString();
                subcategoryObs.Add(JsonConvert.SerializeObject(sub));
            }

            return new ArrayList() { entries, (String[])categoryObs.ToArray(typeof(String)), (String[])subcategoryObs.ToArray(typeof(String)) };
        }

        protected void mapsTownSelectionHidden_TextChanged(object sender, EventArgs e)
        {

        }

        protected void buttontemp_Click(object sender, EventArgs e)
        {
            loadTownInfo("Home");
        }
    }
}