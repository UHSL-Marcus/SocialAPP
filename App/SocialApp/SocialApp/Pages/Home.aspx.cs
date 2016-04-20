using SocialApp.Utils;
using System;
using System.Web.UI;

namespace SocialApp.Pages
{
    public partial class Home : Page
    {
        private SiteMaster thisMaster;
        protected void Page_Load(object sender, EventArgs e)
        {
            thisMaster = Master as SiteMaster;
            thisMaster.setChild(this);
        }

        private void showLogin()
        {
            string t = "showLogin();";
            ScriptManager.RegisterStartupScript(homeUpdatePanel, homeUpdatePanel.GetType(), "ShowLogin" + UniqueID, t, true);
            t = "wireUp_LoginEvents();";
            ScriptManager.RegisterStartupScript(homeUpdatePanel, homeUpdatePanel.GetType(), "WireuUpLogin" + UniqueID, t, true);
        }

        private bool loginCheck()
        {
            if (Session[Paths.USERDETAILS] == null) { showLogin(); return false; }
            return true;
        }

        public void clickProfile()
        {
            profileBtn_Click(this, EventArgs.Empty);
        }

        protected void profileBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck())
            {
                Response.Redirect(Paths.PAGE_PROFILE);
            }
        }

        protected void statsBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck())
            {
                Response.Redirect(Paths.PAGE_STATS);
            }
        }

        protected void mapsBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck())
            {
                Response.Redirect(Paths.PAGE_MAPS);
            }
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            String SOAPbdy = "<Username>" + userIn.Value + "</Username><Password>" + passwordIn.Value + "</Password>";
            HTTPRequest req = new HTTPRequest();
            String response = req.HttpSOAPRequest(SOAPbdy, "GetUser");

            String uIDs = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("UserID");
            int uID = 0;
            Int32.TryParse(uIDs, out uID);

            if (uID > 0)
            {
                Session[Paths.USERDETAILS] = response;
                Session[Paths.USERGEOLOC] = HTTPRequest.getGoogleGeoLocation(response);
                thisMaster.signedInCheck();
            }
            else
            {
                info.Text = "Username/Passoword incorrect";
                showLogin();
            };
        }

        protected void toggleCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(Paths.PAGE_CREATE);
        }
    }
}