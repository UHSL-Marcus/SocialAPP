using SocialApp.Utils;
using System;
using System.Web.UI;

namespace SocialApp
{
    public partial class SiteMaster : MasterPage
    {
        private Page currentChild; 
        protected void Page_Load(object sender, EventArgs e)
        {
            signedInCheck();
        }

        private bool loginCheck()
        {
            return Session[Paths.USERDETAILS] != null;
        }


        public void signedInCheck()
        {
            if (loginCheck()) {
                ScriptManager.RegisterStartupScript(topRightHeaderUpdatePanel, topRightHeaderUpdatePanel.GetType(), "signoutBtnFalse" + UniqueID, @"hideSignoutBtn(false);", true);
                XMLParse getName = new XMLParse((string)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace);
                userNameHeaderLbl.Text = getName.getElementText("Firstname") + " " + getName.getElementText("Surname");
            } else {
                ScriptManager.RegisterStartupScript(topRightHeaderUpdatePanel, topRightHeaderUpdatePanel.GetType(), "signoutBtnTrue" + UniqueID, @"hideSignoutBtn(true);", true);
                userNameHeaderLbl.Text = "Not Signed In checked";

                if (currentChild is Pages.Home) {
                    Console.WriteLine("Home Page");
                }
            }
            //topRightHeaderUpdatePanel.Update();
        }

        public void setChild(Page page)
        {
            currentChild = page;
        }

        protected void userNameHeaderLbl_headerProfileBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck()) {
                if (!(currentChild is Pages.Profile))
                    Response.Redirect(Paths.PAGE_PROFILE); // profile
            } else {
                if (currentChild is Pages.Home)
                    (currentChild as Pages.Home).clickProfile();
                else Response.Redirect(Paths.PAGE_HOME); //default
            }
        }

        protected void signOutBtn_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            if (!(currentChild is Pages.Home))
                Response.Redirect(Paths.PAGE_HOME); // default
            else signedInCheck();
        }

        protected void homeBtn_Click(object sender, ImageClickEventArgs e)
        {
            if (!(currentChild is Pages.Home))
                Response.Redirect(Paths.PAGE_HOME); // default
        }
    }
}