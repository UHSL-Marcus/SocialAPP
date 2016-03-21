﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WebApplication1.Utils;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApplication1.Views
{
    public partial class Profile : System.Web.UI.UserControl
    {
        
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Paths.INITIAL_LOAD] != null)
            {
                Session[Paths.INITIAL_LOAD] = null;
                loadView();
            }

            String t = "Create();";
            if (Session[Paths.USERDETAILS] != null) t = "Profile();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "createProfile" + UniqueID, t, true);
            ScriptManager.RegisterStartupScript(CreatePage, CreatePage.GetType(), "pageColourProfile" + CreatePage.UniqueID, @"changeCurrentPage(""profile"");", true);
        }

        private void loadView()
        {
            ProfilePage.Visible = false;
            CreatePage.Visible = false;
            if (Session[Paths.USERDETAILS] == null)
            {
                CreatePage.Visible = true;
                setCreateDetails();
            }
            else
            {
                ProfilePage.Visible = true;
                setProfileDetails();
            }
        }
     
        private void setProfileDetails()
        {
            XMLParse xml = new XMLParse((String)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace);
            // Personal Details
            profilePersonalFName.Value = xml.getElementText("Firstname");
            profilePersonalLName.Value = xml.getElementText("Surname");
            profileSelGender.SelectedValue = xml.getElementText("Gender");
            DateTime dob = DateTime.Parse(xml.getElementText("DateOfBirth"));
            setDateDropdown(profileSelMonth, profileSelYear, dob.Month, dob.Year);
            profileSelHiddenDay.Value = dob.Day.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setDay" + UniqueID, "setProfileDay();", true);
            
            
            // Contact Details
            profileHouseNumber.Value = xml.getElementText("HouseNumberName");
            profileStreet.Value = xml.getElementText("Address");
            profileTown.Value = xml.getElementText("Town");
            profilePostcode.Value = xml.getElementText("Postcode");
            profileTel.Value = "";

            //Login Details
            profileEmail.Value = xml.getElementText("Email");
            profileUsername.Value = xml.getElementText("Username");

            //lifestyle details
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
            profileCatLabel1.InnerText = "Enviroment";
        }

        protected void UpdatePanel_Unload(object sender, EventArgs e)
        {
            MethodInfo methodInfo = typeof(ScriptManager).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(i => i.Name.Equals("System.Web.UI.IScriptManagerInternal.RegisterUpdatePanel")).First();
            methodInfo.Invoke(ScriptManager.GetCurrent(Page),
                new object[] { sender as UpdatePanel });
        }

        private void setCreateDetails()
        {
            setDateDropdown(createSelMonth, createSelYear);
        }

        private void setDateDropdown(DropDownList sMonth, DropDownList sYear, int month = 0, int year = 0)
        {
            //Fill Years
            sYear.Items.Clear();
            sYear.Items.Add("");
            for (int i = DateTime.Now.Year - 100; i <= DateTime.Now.Year; i++)
            {
                sYear.Items.Add(i.ToString()); //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(8)
            }
            String y = "";
            if (year != 0) y = year.ToString();
            sYear.Items.FindByValue(y).Selected = true;


            //Fill Months
            sMonth.Items.Clear();
            sMonth.Items.Add("");
            for (int i = 1; i <= 12; i++)
            {
                sMonth.Items.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
            }
            String m = "";
            if (month != 0) m = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
            sMonth.Items.FindByValue(m).Selected = true;
        }

        private void fillDays(DropDownList sDay, DropDownList sMonth, DropDownList sYear, int day = 0)
        {
            if (!sYear.SelectedValue.Equals("") && !sMonth.SelectedValue.Equals(""))
            {
                // save currently selected day
                if (day < 1 && !sDay.SelectedValue.Equals(""))
                    day = Convert.ToInt16(sDay.SelectedValue);


                sDay.Items.Clear();
                //getting numbner of days in selected month & year
                int noofdays = DateTime.DaysInMonth(Convert.ToInt32(sYear.SelectedValue), Convert.ToInt32(sMonth.SelectedIndex));
                //Fill days
                for (int i = 1; i <= noofdays; i++)
                {
                    sDay.Items.Add(i.ToString());
                }

                // make sure selected day is not out of range
                if (day > noofdays)
                    day = noofdays;
                else if (day < 1)
                    day = 1;

                sDay.Items.FindByValue(day.ToString()).Selected = true;

            }
        }

        private void validationError() {
            String p = "CreatePage";
            if (Session[Paths.USERDETAILS] != null) p = "ProfilePage";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "validate" + UniqueID, "validateAll('" + p + "');", true);
        }

        
        protected void submitCreate_Click(object sender, EventArgs e)
        {
            errorMessage.InnerText = "Validation Error";
            
            // validate all input -- not doing anything comprehesive yet, no SQL injection defense. -- "vanity"/user experience validation is done in javascript
            // any validation done here is just to avoid errors and malicious entires, so no need to check if the email, phone number etc are real here too. If a user has turned off javascript
            // they are most likeley malicious; so validating the format of their email address etc is not a concern. 
            String SOAPbdy = "<UserDetails>";
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
                if (Int16.Parse(createSelHiddenDay.Value) < 10) SOAPbdy += "0";
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
            String response = req.HttpSOAPRequest(SOAPbdy, "SetNewUser");

            String result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("SetNewUserResult");
            
            Boolean success;
            Boolean.TryParse(result, out success);

            if (success)
            {
                errorMessage.InnerText = "";
                //info.Text = "User created sucessfully";
            }
            else errorMessage.InnerText = "Input Error";    
        }

        protected void updateProfile_Click(object sender, EventArgs e)
        {
            updateProfileMessage.InnerText = "Validation Error";

            // validate all input -- not doing anything comprehesive yet, no SQL injection defense. -- "vanity"/user experience validation is done in javascript
            // any validation done here is just to avoid errors and malicious entires, so no need to check if the email, phone number etc are real here too. If a user has turned off javascript
            // they are most likeley malicious; so validating the format of their email address etc is not a concern. 

            String SOAPbdy = "<UserDetails>";
            SOAPbdy += "<UserID>" + (new XMLParse((String)Session["userDetails"], SOAPRequest.soapNamespace)).getElementText("UserID") + "</UserID>";
            if (valididateInfo(profilePersonalFName.Value))
                SOAPbdy += "<Firstname>" + profilePersonalFName.Value + "</Firstname>";
            else { validationError(); return; }

            if (valididateInfo(profilePersonalLName.Value))
                SOAPbdy += "<Surname>" + profilePersonalLName.Value + "</Surname>";
            else { validationError(); return; }

            if (valididateInfo(profileSelGender.SelectedValue))
                SOAPbdy += "<Gender>" + profileSelGender.SelectedValue + "</Gender>";
            else { validationError(); return; }

            if (valididateInfo(profileSelHiddenDay.Value) && valididateInfo(profileSelMonth.SelectedValue) && valididateInfo(profileSelYear.SelectedValue))
            {
                SOAPbdy += "<DateOfBirth>" + profileSelYear.SelectedValue + "-";
                if (profileSelMonth.SelectedIndex < 10) SOAPbdy += "0";
                SOAPbdy += profileSelMonth.SelectedIndex + "-";
                if (Int16.Parse(profileSelHiddenDay.Value) < 10) SOAPbdy += "0";
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
                SOAPbdy += "<Password>" + (new XMLParse((String)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace)).getElementText("Password") +"</Password>"; 

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

            if (valididateInfo(profileCat1.Value))
                SOAPbdy += "<Category_1>" + profileCat1.Value + "</Category_1>";
            else { validationError(); return; }

            if (valididateInfo(profileCat2.Value))
                SOAPbdy += "<Category_2>" + profileCat1.Value + "</Category_2>";
            else { validationError(); return; }

            if (valididateInfo(profileCat3.Value))
                SOAPbdy += "<Category_3>" + profileCat1.Value + "</Category_3>";
            else { validationError(); return; }

            if (valididateInfo(profileCat4.Value))
                SOAPbdy += "<Category_4>" + profileCat1.Value + "</Category_4>";
            else { validationError(); return; }

            if (valididateInfo(profileCat5.Value))
                SOAPbdy += "<Category_5>" + profileCat1.Value + "</Category_5>";
            else { validationError(); return; }

            if (valididateInfo(profileCat6.Value))
                SOAPbdy += "<Category_6>" + profileCat1.Value + "</Category_6>";
            else { validationError(); return; }

            if (valididateInfo(profileCat7.Value))
                SOAPbdy += "<Category_7>" + profileCat1.Value + "</Category_7>";
            else { validationError(); return; }

            if (valididateInfo(profileCat8.Value))
                SOAPbdy += "<Category_8>" + profileCat1.Value + "</Category_8>";
            else { validationError(); return; }

            if (valididateInfo(profileCat9.Value))
                SOAPbdy += "<Category_9>" + profileCat1.Value + "</Category_9>";
            else { validationError(); return; }

            if (valididateInfo(profileCat10.Value))
                SOAPbdy += "<Category_10>" + profileCat1.Value + "</Category_10>";
            else { validationError(); return; }

            if (valididateInfo(profileCat11.Value))
                SOAPbdy += "<Category_11>" + profileCat1.Value + "</Category_11>";
            else { validationError(); return; }

            if (valididateInfo(profileCat12.Value))
                SOAPbdy += "<Category_12>" + profileCat1.Value + "</Category_12>";
            else { validationError(); return; }

            if (valididateInfo(profileCat13.Value))
                SOAPbdy += "<Category_13>" + profileCat1.Value + "</Category_13>";
            else { validationError(); return; }

            if (valididateInfo(profileCat14.Value))
                SOAPbdy += "<Category_14>" + profileCat1.Value + "</Category_14>";
            else { validationError(); return; }

            if (valididateInfo(profileCat15.Value))
                SOAPbdy += "<Category_15>" + profileCat1.Value + "</Category_15>";
            else { validationError(); return; }

            if (valididateInfo(profileCat16.Value))
                SOAPbdy += "<Category_16>" + profileCat1.Value + "</Category_16>";
            else { validationError(); return; }

            if (valididateInfo(profileCat17.Value))
                SOAPbdy += "<Category_17>" + profileCat1.Value + "</Category_17>";
            else { validationError(); return; }
            SOAPbdy += "</UserDetails>";

            HTTPRequest req = new HTTPRequest();
            String response = req.HttpSOAPRequest(SOAPbdy, "UpdateUser");

            String result = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("UpdateUserResult");

            Boolean success;
            Boolean.TryParse(result, out success);

            if (success)
            {
                updateProfileMessage.InnerText = "";
                profileResult.InnerText = "Profile updated sucessfully";
                Session[Paths.USERDETAILS] = @"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><GetUserResponse xmlns=""http://tempuri.org/""><GetUserResult>" + SOAPbdy + "</GetUserResult></GetUserResponse></soap:Body></soap:Envelope>";
            }
            else updateProfileMessage.InnerText = "Input Error";

            //ScriptManager.RegisterStartupScript(CreatePage, CreatePage.GetType(), "pageColourProfile" + CreatePage.UniqueID, @"changeCurrentPage(""profile"");", true);

        }

        // will be expanded to include real validation when needed. 
        private Boolean valididateInfo(String val)
        {
            if (val.Equals("")) return false;
            return true;
        }

        
    }
}