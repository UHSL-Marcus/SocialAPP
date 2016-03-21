using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Utils;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        private string LastLoadedControl
        {
            get
            {
                return Session[Paths.CURRENTCTRL] as string;
            }
            set
            {
                Session[Paths.CURRENTCTRL] = value;
            }
        }

        private void LoadUserControl()
        {
            string controlPath = LastLoadedControl;

            ContentPlaceholder.Controls.Clear();

            if (!string.IsNullOrEmpty(controlPath))
            {
                Control uc = Page.LoadControl(controlPath);
                ContentPlaceholder.Controls.Add(uc);
            }
            else
                MenuButtons.Visible = true;

        }

        private void loginCheck(String path)
        {
            if (Session[Paths.USERDETAILS] == null) showLogin();
            else changeView(path);
        }

        private void signedInCheck()
        {
            if (Session[Paths.USERDETAILS] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signoutBtnFalse" + UniqueID, @"hideSignoutBtn(false);", true);
                XMLParse getName = new XMLParse((string)Session[Paths.USERDETAILS], SOAPRequest.soapNamespace);
                userNameHeaderLbl.Text = getName.getElementText("Firstname") + " " + getName.getElementText("Surname");
            }
            else { 
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signoutBtnTrue" + UniqueID, @"hideSignoutBtn(true);", true);
                userNameHeaderLbl.Text = "Not Signed In";
            }
        }

        private void changeView(String path)
        {
            LastLoadedControl = null;
            MenuButtons.Visible = false;
            Session[Paths.INITIAL_LOAD] = true;

            if (path.Equals("Logout"))
            {
                Session.Clear();
                Session.Abandon();
            }
            else if (!path.Equals("Main")) LastLoadedControl = Paths.USERCTRL_BASE + path + ".ascx";

            LoadUserControl();

            signedInCheck();
        }

        private void showLogin()
        {
            String t = "showLogin();";
            ScriptManager.RegisterStartupScript(ContentPlaceholder, ContentPlaceholder.GetType(), "ShowLogin" + UniqueID, t, true);
        }
 
        protected void Page_Load(object sender, EventArgs e)
        {
            String t = "wireUp_LoginEvents();";
            ScriptManager.RegisterStartupScript(ContentPlaceholder, ContentPlaceholder.GetType(), "loginEvents" + UniqueID, t, true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "pageColourDefault" + UniqueID, @"changeCurrentPage(""main"");", true);
            signedInCheck();
            LoadUserControl();
        }

        protected void temp_Click(object sender, EventArgs e)
        {
            String t = "test();";
            ScriptManager.RegisterStartupScript(ContentPlaceholder, ContentPlaceholder.GetType(), "temp" + UniqueID, t, true);
        }

        protected void profileBtn_Click(object sender, EventArgs e)
        {
            loginCheck("Profile");
        }

        protected void statsBtn_Click(object sender, EventArgs e)
        {
            loginCheck("Stats");
        }

        protected void mapsBtn_Click(object sender, EventArgs e)
        {
            loginCheck("Maps");
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {

            String SOAPbdy = "<Username>" + userIn.Value + "</Username><Password>" + passwordIn.Value + "</Password>";
            HTTPRequest req = new HTTPRequest();
            String response = req.HttpSOAPRequest(SOAPbdy, "GetUser");

            String uIDs = (new XMLParse(response, SOAPRequest.soapNamespace)).getElementText("UserID");
            int uID = 0;
            Int32.TryParse(uIDs, out uID);

            if (uID > 0 || true)
            {
                Session[Paths.USERDETAILS] = response;
                changeView("Profile");
            }
            else
            {
                info.Text = "Username/Passoword incorrect";
                showLogin();
            };
        }

        protected void toggleCreate_Click(object sender, EventArgs e)
        {
            changeView("Profile");
        }

        protected void homeBtn_Click(object sender, EventArgs e)
        {
            changeView("Main");
        }

        protected void signOutBtn_Click(object sender, EventArgs e)
        {
            changeView("Logout");
        }

        public void crashBackToHome(string e)
        {
            changeView("Main");
            if (e.Length > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "addressError" + UniqueID, @"alert('" + e + "');", true);
        }
    }
}