using SocialApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialApp.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        private SiteMaster thisMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            thisMaster = Master as SiteMaster;
            thisMaster.setChild(this);

            buildCategories();
            setProfileDetails();
            ScriptManager.RegisterStartupScript(profileUpdatePanel, profileUpdatePanel.GetType(), "wireup" + UniqueID, "wireupHeadings();wireupFormControls();wireupProfileValidation();", true);
        }

        private void buildCategories()
        {
            HTTPRequest req = new HTTPRequest();
            string response = req.HttpSOAPRequest("", "GetListOfCategories");

            XMLParse xml = new XMLParse(response, SOAPRequest.soapNamespace);

            List<string> catSections = xml.getWholeSection("Category");


            string categoryInputs = "";
            Dictionary<string, string> cat_info = new Dictionary<string, string>();

            foreach(string catSection in catSections)
            {
                XMLParse catXml = new XMLParse(catSection, SOAPRequest.soapNamespace);
                string id = catXml.getElementText("CategoryID");
                string name = catXml.getElementText("CategoryName");

                string htmlLit = @"
                                <div class=""flex-container"">
                                    <div class=""misc-text single-line flex-1"">{0}</div>
                                    <input runat=""server"" class=""input_range flex-1"" min=""1"" max=""10"" step=""1"" type=""range"" id=""category{1}"" data-id=""{1}"" value=""1"" />
                                    <div class=""misc-text range_number"">01</div>
                                </div>";
                categoryInputs += string.Format(htmlLit, name, id);

                cat_info.Add(id, name);
            }

            lifestyle_content.InnerHtml = categoryInputs;
            Session[Paths.CAT_INFO] = cat_info;
        }

        private void setProfileDetails()
        {
            XMLParse xml = new XMLParse((string)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace);

            

            // Personal Details
            profilePersonalFName.Value = xml.getElementText("Firstname");
            profilePersonalLName.Value = xml.getElementText("Surname");

            string gender = xml.getElementText("Gender");
            if (gender.Equals("Male", StringComparison.InvariantCultureIgnoreCase))
                gender_male.Checked = true;

            if (gender.Equals("Female", StringComparison.InvariantCultureIgnoreCase))
                gender_female.Checked = true;
            

            DateTime dob = DateTime.Parse(xml.getElementText("DateOfBirth"));
            DateMenu.setDateDropdown(profileSelMonth, profileSelYear, dob.Month, dob.Year);
            profileSelHiddenDay.Value = dob.Day.ToString();
            ScriptManager.RegisterStartupScript(profileUpdatePanel, profileUpdatePanel.GetType(), "setDay" + UniqueID, "setProfileDay();", true);

            //Login Details
            profileEmail.Value = xml.getElementText("Email");
            profileUsername.Value = xml.getElementText("Username");

            // Contact Details
            profileHouseNumber.Value = xml.getElementText("HouseNumberName");
            profileStreet.Value = xml.getElementText("Address");
            profileTown.Value = xml.getElementText("Town");
            profilePostcode.Value = xml.getElementText("Postcode");
            profileTel.Value = "";

            //Login Details
            profileEmail.Value = xml.getElementText("Email");
            profileUsername.Value = xml.getElementText("Username");

            /*/lifestyle details
            profileCat1.Value = xml.getElementText("Category_1");
            profileCat2.Value = xml.getElementText("Category_2");
            profileCat3.Value = xml.getElementText("Category_3");
            profileCat4.Value = xml.getElementText("Category_4");
            profileCat5.Value = xml.getElementText("Category_5");
            profileCat6.Value = xml.getElementText("Category_6");
            profileCat7.Value = xml.getElementText("Category_7");
            profileCat8.Value = xml.getElementText("Category_8");
            profileCat9.Value = xml.getElementText("Category_9");
            profileCat10.Value = xml.getElementText("Category_10");
            profileCat11.Value = xml.getElementText("Category_11");
            profileCat12.Value = xml.getElementText("Category_12");
            profileCat13.Value = xml.getElementText("Category_13");
            profileCat14.Value = xml.getElementText("Category_14");
            profileCat15.Value = xml.getElementText("Category_15");
            profileCat16.Value = xml.getElementText("Category_16");
            profileCat17.Value = xml.getElementText("Category_17");
            profileCatLabel1.InnerText = "Enviroment";*/

        }


        private void validationError()
        {
            ScriptManager.RegisterStartupScript(profileUpdatePanel, profileUpdatePanel.GetType(), "validate" + UniqueID, "validateAll(\".secton - content\");", true);
        }

        protected void updateProfile_Click(object sender, EventArgs e)
        {
            updateProfileMessage.Text = "Validation Error";

            // validate all input -- not doing anything comprehesive yet, no SQL injection defense. -- "vanity"/user experience validation is done in javascript
            // any validation done here is just to avoid errors and malicious entries, so no need to check if the email, phone number etc are real here too. If a user has turned off javascript
            // they are most likeley malicious; so validating the format of their email address etc is not a concern. 

            string SOAPbdy = "<UserDetails>";
            SOAPbdy += "<UserID>" + (new XMLParse((string)Session["userDetails"], SOAPRequest.soapNamespace)).getElementText("UserID") + "</UserID>";
            if (valididateInfo(profilePersonalFName.Value))
                SOAPbdy += "<Firstname>" + profilePersonalFName.Value + "</Firstname>";
            else { validationError(); return; }

            if (valididateInfo(profilePersonalLName.Value))
                SOAPbdy += "<Surname>" + profilePersonalLName.Value + "</Surname>";
            else { validationError(); return; }

            if (valididateInfo(profileSelHiddenDay.Value) && valididateInfo(profileSelMonth.SelectedValue) && valididateInfo(profileSelYear.SelectedValue))
            {
                SOAPbdy += "<DateOfBirth>" + profileSelYear.SelectedValue + "-";
                if (profileSelMonth.SelectedIndex < 10) SOAPbdy += "0";
                SOAPbdy += profileSelMonth.SelectedIndex + "-";
                if (short.Parse(profileSelHiddenDay.Value) < 10) SOAPbdy += "0";
                SOAPbdy += profileSelHiddenDay.Value + "</DateOfBirth>";
            }
            else { validationError(); return; }

            if (valididateInfo(profileEmail.Value))
                SOAPbdy += "<Email>" + profileEmail.Value + "</Email>";
            else { validationError(); return; }

            if (valididateInfo(profileUsername.Value))
                SOAPbdy += "<Username>" + profileUsername.Value + "</Username>";
            else { validationError(); return; }

            if (profileChangePassword.Value != "")
            {
                if (profileChangePassword.Value.Equals(profileConfPassword.Value))
                    SOAPbdy += "<Password>" + profileChangePassword.Value + "</Password>";
                else { validationError(); return; }
            }
            else // no attempt to change password, resend original. <-- not best idea to have password stored in session, best if webservice accepts "blank" as "no change" at least just for password.
                SOAPbdy += "<Password>" + (new XMLParse((string)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace)).getElementText("Password") + "</Password>";

            if (valididateInfo(profileHouseNumber.Value))
                SOAPbdy += "<HouseNumberName>" + profileHouseNumber.Value + "</HouseNumberName>";
            else { validationError(); return; }

            if (valididateInfo(profileStreet.Value))
                SOAPbdy += "<Address>" + profileStreet.Value + "</Address>";
            else { validationError(); return; }

            if (valididateInfo(profileTown.Value))
                SOAPbdy += "<Town>" + profileTown.Value + "</Town>";
            else { validationError(); return; }

            if (valididateInfo(profilePostcode.Value))
                SOAPbdy += "<Postcode>" + profilePostcode.Value + "</Postcode>";
            else { validationError(); return; }

            SOAPbdy += "<Categories>";

            string[] catVals = lifestyle_info.Value.Split(',');
            Dictionary<string, string> cat_info = (Dictionary<string, string>)Session[Paths.CAT_INFO];
            foreach (string catValStr in catVals)
            {
                if(catValStr.Length > 0)
                {
                    string id = catValStr.Substring(0, 2);
                    string catValue = catValStr.Substring(3, 2);
                    string name;
                    if (cat_info.TryGetValue(id, out name))
                    {
                        string html = @"
                                        <CategoryInfo>
                                            <CategoryID>{0}</CategoryID>
                                            <CategoryName>{1}</CategoryName>
                                            <CategoryValue>{2}</CategoryValue>
                                        </CategoryInfo>";
                        SOAPbdy += string.Format(html, id, name, catValue);
                                
                    }
                }
            }
            SOAPbdy += "</Categories>";
            SOAPbdy += "</UserDetails>";

            HTTPRequest req = new HTTPRequest();
            string response = req.HttpSOAPRequest(SOAPbdy, "UpdateUser");

            string result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("UpdateUserResult");

            bool success;
            bool.TryParse(result, out success);

            if (success)
            {
                updateProfileMessage.Text = "Profile updated sucessfully";
                Session[Paths.USERDETAILS] = @"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><GetUserResponse xmlns=""http://tempuri.org/""><GetUserResult>" + SOAPbdy + "</GetUserResult></GetUserResponse></soap:Body></soap:Envelope>";
            }
            else updateProfileMessage.Text = "Input Error";

            //ScriptManager.RegisterStartupScript(CreatePage, CreatePage.GetType(), "pageColourProfile" + CreatePage.UniqueID, @"changeCurrentPage(""profile"");", true);
        }


        // will be expanded to include real validation when needed. 
        private bool valididateInfo(string val)
        {
            if (val.Equals("")) return false;
            return true;
        }
    }
}