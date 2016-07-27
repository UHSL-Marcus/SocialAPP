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
            string mode;
            if ((string)Session[Paths.USERDETAILS] != null)
            {
                profileCreateBtn.Visible = false;
                pageTitleLbl.Text = "Update Account";
                mode = "ModeEnum.UPDATE";
            }
            else
            {
                profileUpdateBtn.Visible = false;
                pageTitleLbl.Text = "Create Account";
                mode = "ModeEnum.CREATE";
            }


            if (!IsPostBack)
            {
                buildCategories();
                setProfileDetails();

            }
            ScriptManager.RegisterStartupScript(profileUpdatePanel, profileUpdatePanel.GetType(), "wireup" + UniqueID, "wireupFormControls(" + mode + ");wireupProfileValidation();", true);
        }

        private void buildCategories()
        {
            HTTPRequest req = new HTTPRequest();
            string response = req.HttpSOAPRequest("", "GetListOfCategories");

            XMLParse xml = new XMLParse(response, SOAPRequest.soapNamespace);

            List<string> catSections = xml.getWholeSection("Category");


            string categoryInputs = "";
            Dictionary<string, string> cat_info = new Dictionary<string, string>();

            string usrDetails = (string)Session[Paths.USERDETAILS];
            XMLParse userCatInfo = null;
            if (usrDetails != null)
            {
                XMLParse usrXml = new XMLParse(usrDetails, SOAPRequest.soapNamespace);
                userCatInfo = new XMLParse(usrXml.getElementText("Categories", null, true), SOAPRequest.soapNamespace);
            }

            foreach(string catSection in catSections)
            {
                XMLParse catXml = new XMLParse(catSection, SOAPRequest.soapNamespace);
                string id = catXml.getElementText("CategoryID");
                string name = catXml.getElementText("CategoryName");
                int value = 1;
                if (userCatInfo !=null)
                    int.TryParse(userCatInfo.getElementFromSibling("CategoryInfo", "CategoryID", id, "CategoryValue"), out value);

                string valueS = value < 10 ? "0" + value : value.ToString();
                

                string htmlLit = @"
                                <div class=""flex-container"">
                                    <div class=""misc-text single-line flex-1"">{0}</div>
                                    <input runat=""server"" class=""input_range flex-1"" min=""1"" max=""10"" step=""1"" type=""range"" id=""category{1}"" data-id=""{1}"" value=""{2}"" />
                                    <div class=""misc-text range_number"">{3}</div>
                                </div>";
                categoryInputs += string.Format(htmlLit, name, id, value, valueS);

                cat_info.Add(id, name);
            }

            lifestyle_content.InnerHtml = categoryInputs;
            Session[Paths.CAT_INFO] = cat_info;
        }

        private void setProfileDetails()
        {
            if ((string)Session[Paths.USERDETAILS] != null)
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

            }
            else
            {
                DateMenu.setDateDropdown(profileSelMonth, profileSelYear);
            }
        }


        private void validationError()
        {
            ScriptManager.RegisterStartupScript(profileUpdatePanel, profileUpdatePanel.GetType(), "validate" + UniqueID, "validateAll(\".secton - content\");", true);
        }

        protected void updateProfile_Click(object sender, EventArgs e)
        {
            if (doCreateOrUpdate(false))
                profileActionMessage.Text = "Update Successful";
        }

        protected void profileCreateBtn_Click(object sender, EventArgs e)
        {
            if (doCreateOrUpdate(true))
            {
                profileActionMessage.Text = "Account Created Succesfully";
                Response.Redirect(Paths.PAGE_HOME);
            }

            
        }

        private bool doCreateOrUpdate(bool create)
        {
            profileActionMessage.Text = "Validation Error";

            // validate all input -- not doing anything comprehesive yet, no SQL injection defense. -- "vanity"/user experience validation is done in javascript
            // any validation done here is just to avoid errors and malicious entries, so no need to check if the email, phone number etc are real here too. If a user has turned off javascript
            // they are most likeley malicious; so validating the format of their email address etc is not a concern. 

            string userID = "-1";
            if (!create)
                userID = (new XMLParse((string)Session["userDetails"], SOAPRequest.soapNamespace)).getElementText("UserID");

            string SOAPbdy = "<UserDetails>";
            SOAPbdy += "<UserID>" + userID + "</UserID>";
            if (valididateInfo(profilePersonalFName.Value))
                SOAPbdy += "<Firstname>" + profilePersonalFName.Value + "</Firstname>";
            else { validationError(); return false; }

            if (valididateInfo(profilePersonalLName.Value))
                SOAPbdy += "<Surname>" + profilePersonalLName.Value + "</Surname>";
            else { validationError(); return false; }

            string gender = "Male";
            if (gender_female.Checked)
                gender = "Female";
            SOAPbdy += "<Gender>" + gender + "</Gender>";

            if (valididateInfo(profileSelHiddenDay.Value) && valididateInfo(profileSelMonth.SelectedValue) && valididateInfo(profileSelYear.SelectedValue))
            {
                SOAPbdy += "<DateOfBirth>" + profileSelYear.SelectedValue + "-";
                if (profileSelMonth.SelectedIndex < 10) SOAPbdy += "0";
                SOAPbdy += profileSelMonth.SelectedIndex + "-";
                if (short.Parse(profileSelHiddenDay.Value) < 10) SOAPbdy += "0";
                SOAPbdy += profileSelHiddenDay.Value + "</DateOfBirth>";
            }
            else { validationError(); return false; }

            if (valididateInfo(profileEmail.Value))
                SOAPbdy += "<Email>" + profileEmail.Value + "</Email>";
            else { validationError(); return false; }

            if (valididateInfo(profileUsername.Value))
                SOAPbdy += "<Username>" + profileUsername.Value + "</Username>";
            else { validationError(); return false; }

            if (profileChangePassword.Value != "")
            {
                if (profileChangePassword.Value.Equals(profileConfPassword.Value))
                    SOAPbdy += "<Password>" + profileChangePassword.Value + "</Password>";
                else { validationError(); return false; }
            }
            else if (create)
            {
                validationError(); return false;
            }
            else if (!create) // no attempt to change password, resend original. <-- not best idea to have password stored in session, best if webservice accepts "blank" as "no change" at least just for password.
                SOAPbdy += "<Password>" + (new XMLParse((string)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace)).getElementText("Password") + "</Password>";

            if (valididateInfo(profileHouseNumber.Value))
                SOAPbdy += "<HouseNumberName>" + profileHouseNumber.Value + "</HouseNumberName>";
            else { validationError(); return false; }

            if (valididateInfo(profileStreet.Value))
                SOAPbdy += "<Address>" + profileStreet.Value + "</Address>";
            else { validationError(); return false; }

            if (valididateInfo(profileTown.Value))
                SOAPbdy += "<Town>" + profileTown.Value + "</Town>";
            else { validationError(); return false; }

            if (valididateInfo(profilePostcode.Value))
                SOAPbdy += "<Postcode>" + profilePostcode.Value + "</Postcode>";
            else { validationError(); return false; }

            SOAPbdy += "<Categories>";

            string[] catVals = lifestyle_info.Value.Split(',');
            Dictionary<string, string> cat_info = (Dictionary<string, string>)Session[Paths.CAT_INFO];
            foreach (string catValStr in catVals)
            {
                if (catValStr.Length > 0)
                {
                    string id = int.Parse(catValStr.Substring(0, 2)).ToString();
                    string catValue = int.Parse(catValStr.Substring(3, 2)).ToString();
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

            string action = "UpdateUser";
            if (create)
                action = "SetNewUser";

            HTTPRequest req = new HTTPRequest();
            string response = req.HttpSOAPRequest(SOAPbdy, action);

            string result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText(action + "Result");

            bool success;
            bool.TryParse(result, out success);

            if (success)
                Session[Paths.USERDETAILS] = @"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><GetUserResponse xmlns=""http://tempuri.org/""><GetUserResult>" + SOAPbdy + "</GetUserResult></GetUserResponse></soap:Body></soap:Envelope>";
            else profileActionMessage.Text = "Input Error";

            buildCategories();
            setProfileDetails();

            return success;
        }


        // will be expanded to include real validation when needed. 
        private bool valididateInfo(string val)
        {
            if (val.Equals("")) return false;
            return true;
        }

        
    }
}