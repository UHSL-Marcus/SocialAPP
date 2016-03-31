using SocialApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialApp.Pages
{
    public partial class Create : System.Web.UI.Page
    {

        private SiteMaster thisMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            thisMaster = Master as SiteMaster;
            thisMaster.setChild(this);

            setCreateDetails();
            ScriptManager.RegisterStartupScript(createUpdatePanel, createUpdatePanel.GetType(), "wireup" + UniqueID, "wireupCreateValidation();", true);
        }

        private void setCreateDetails()
        {
            DateMenu.setDateDropdown(createSelMonth, createSelYear);
        }

        protected void submitCreate_Click(object sender, EventArgs e)
        {
            errorMessage.InnerText = "Validation Error";

            // validate all input -- not doing anything comprehesive yet, no SQL injection defense. -- "vanity"/user experience validation is done in javascript
            // any validation done here is just to avoid errors and malicious entries, so no need to check if the email, phone number etc are real here too. If a user has turned off javascript
            // they are most likeley malicious; so validating the format of their email address etc is not a concern. 
            string SOAPbdy = "<UserDetails>";
            if (valididateInfo(createPersonalFName.Value))
                SOAPbdy += "<Firstname>" + createPersonalFName.Value + "</Firstname>";
            else { validationError(); return; }

            if (valididateInfo(createPersonalLName.Value))
                SOAPbdy += "<Surname>" + createPersonalFName.Value + "</Surname>";
            else { validationError(); return; }

            if (valididateInfo(createSelGender.SelectedValue))
                SOAPbdy += "<Gender>" + createSelGender.SelectedValue + "</Gender>";
            else { validationError(); return; }

            if (valididateInfo(createSelHiddenDay.Value) && valididateInfo(createSelMonth.SelectedValue) && valididateInfo(createSelYear.SelectedValue))
            {
                SOAPbdy += "<DateOfBirth>" + createSelYear.SelectedValue + "-";
                if (createSelMonth.SelectedIndex < 10) SOAPbdy += "0";
                SOAPbdy += createSelMonth.SelectedIndex + "-";
                if (short.Parse(createSelHiddenDay.Value) < 10) SOAPbdy += "0";
                SOAPbdy += createSelHiddenDay.Value + "</DateOfBirth>";
            }
            else { validationError(); return; }

            if (valididateInfo(createEmail.Value))
                SOAPbdy += "<Email>" + createEmail.Value + "</Email>";
            else { validationError(); return; }

            if (valididateInfo(createUsername.Value))
                SOAPbdy += "<Username>" + createUsername.Value + "</Username>";
            else { validationError(); return; }

            if (valididateInfo(createPassword.Value) && createPassword.Value.Equals(createConfPassword.Value))
                SOAPbdy += "<Password>" + createPassword.Value + "</Password>";
            else { validationError(); return; }

            if (valididateInfo(createHouseNumber.Value))
                SOAPbdy += "<HouseNumberName>" + createHouseNumber.Value + "</HouseNumberName>";
            else { validationError(); return; }

            if (valididateInfo(createStreet.Value))
                SOAPbdy += "<Address>" + createStreet.Value + "</Address>";
            else { validationError(); return; }

            if (valididateInfo(createTown.Value))
                SOAPbdy += "<Town>" + createTown.Value + "</Town>";
            else { validationError(); return; }

            if (valididateInfo(createPostcode.Value))
                SOAPbdy += "<Postcode>" + createPostcode.Value + "</Postcode>";
            else { validationError(); return; }

            if (valididateInfo(createCat1.Value))
                SOAPbdy += "<Category_1>" + createCat1.Value + "</Category_1>";
            else { validationError(); return; }

            if (valididateInfo(createCat2.Value))
                SOAPbdy += "<Category_2>" + createCat1.Value + "</Category_2>";
            else { validationError(); return; }

            if (valididateInfo(createCat3.Value))
                SOAPbdy += "<Category_3>" + createCat1.Value + "</Category_3>";
            else { validationError(); return; }

            if (valididateInfo(createCat4.Value))
                SOAPbdy += "<Category_4>" + createCat1.Value + "</Category_4>";
            else { validationError(); return; }

            if (valididateInfo(createCat5.Value))
                SOAPbdy += "<Category_5>" + createCat1.Value + "</Category_5>";
            else { validationError(); return; }

            if (valididateInfo(createCat6.Value))
                SOAPbdy += "<Category_6>" + createCat1.Value + "</Category_6>";
            else { validationError(); return; }

            if (valididateInfo(createCat7.Value))
                SOAPbdy += "<Category_7>" + createCat1.Value + "</Category_7>";
            else { validationError(); return; }

            if (valididateInfo(createCat8.Value))
                SOAPbdy += "<Category_8>" + createCat1.Value + "</Category_8>";
            else { validationError(); return; }

            if (valididateInfo(createCat9.Value))
                SOAPbdy += "<Category_9>" + createCat1.Value + "</Category_9>";
            else { validationError(); return; }

            if (valididateInfo(createCat10.Value))
                SOAPbdy += "<Category_10>" + createCat1.Value + "</Category_10>";
            else { validationError(); return; }

            if (valididateInfo(createCat11.Value))
                SOAPbdy += "<Category_11>" + createCat1.Value + "</Category_11>";
            else { validationError(); return; }

            if (valididateInfo(createCat12.Value))
                SOAPbdy += "<Category_12>" + createCat1.Value + "</Category_12>";
            else { validationError(); return; }

            if (valididateInfo(createCat13.Value))
                SOAPbdy += "<Category_13>" + createCat1.Value + "</Category_13>";
            else { validationError(); return; }

            if (valididateInfo(createCat14.Value))
                SOAPbdy += "<Category_14>" + createCat1.Value + "</Category_14>";
            else { validationError(); return; }

            if (valididateInfo(createCat15.Value))
                SOAPbdy += "<Category_15>" + createCat1.Value + "</Category_15>";
            else { validationError(); return; }

            if (valididateInfo(createCat16.Value))
                SOAPbdy += "<Category_16>" + createCat1.Value + "</Category_16>";
            else { validationError(); return; }

            if (valididateInfo(createCat17.Value))
                SOAPbdy += "<Category_17>" + createCat1.Value + "</Category_17>";
            else { validationError(); return; }
            SOAPbdy += "</UserDetails>";

            HTTPRequest req = new HTTPRequest();
            string response = req.HttpSOAPRequest(SOAPbdy, "SetNewUser");

            string result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("SetNewUserResult");

            bool success;
            bool.TryParse(result, out success);

            if (success)
            {
                errorMessage.InnerText = "Sucess";
                //info.Text = "User created sucessfully";
            }
            else errorMessage.InnerText = "Input Error";
        }

        // will be expanded to include real validation when needed. 
        private bool valididateInfo(string val)
        {
            if (val.Equals("")) return false;
            return true;
        }

        private void validationError()
        {
            ScriptManager.RegisterStartupScript(createUpdatePanel, createUpdatePanel.GetType(), "validate" + UniqueID, "validateAll('CreatePage');", true);
        }
    }
}