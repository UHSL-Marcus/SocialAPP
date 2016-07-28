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

        private void showLogin(string path)
        {
            followOnPath.Value = path;
            string t = "showLogin();";
            ScriptManager.RegisterStartupScript(homeUpdatePanel, homeUpdatePanel.GetType(), "ShowLogin" + UniqueID, t, true);
            t = "wireUp_LoginEvents();";
            ScriptManager.RegisterStartupScript(homeUpdatePanel, homeUpdatePanel.GetType(), "WireuUpLogin" + UniqueID, t, true);
        }

        private bool loginCheck(string path)
        {
            if (Session[Paths.USERDETAILS] == null) { showLogin(path); return false; }
            return true;
        }

        public void clickProfile()
        {
            profileBtn_Click(this, EventArgs.Empty);
        }

        protected void profileBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck(Paths.PAGE_PROFILE))
            {
                Response.Redirect(Paths.PAGE_PROFILE);
            }
        }

        protected void statsBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck(Paths.PAGE_STATS))
            {
                Response.Redirect(Paths.PAGE_STATS);
            }
        }

        protected void mapsBtn_Click(object sender, EventArgs e)
        {
            if (loginCheck(Paths.PAGE_MAPS))
            {
                Response.Redirect(Paths.PAGE_MAPS);
            }
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            string SOAPbdy = "<Username>" + userIn.Value + "</Username><Password>" + passwordIn.Value + "</Password>";
            HTTPRequest req = new HTTPRequest();
            string response = req.HttpSOAPRequest(SOAPbdy, "GetUser");

            string uIDs = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("UserID");
            int uID = 0;
            int.TryParse(uIDs, out uID);

            if (uID > 0)
            {
                Session[Paths.USERDETAILS] = response;
                Session[Paths.USERGEOLOC] = HTTPRequest.getGoogleGeoLocation(response);
                thisMaster.signedInCheck();
                Response.Redirect(followOnPath.Value);
            }
            else
            {
                info.Text = "Username/Passoword incorrect";
                showLogin(followOnPath.Value);
            };
        }

        protected void toggleCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect(Paths.PAGE_PROFILE);
        }
    }
}