using SocialApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocialApp.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        private SiteMaster thisMaster;

        protected void Page_Load(object sender, EventArgs e)
        {
            thisMaster = this.Master as SiteMaster;
            thisMaster.setChild(this);
            loadView();
        }

        private void loadView()
        {
            ProfilePage.Visible = false;
            CreatePage.Visible = false;
            if (Session[Paths.USERDETAILS] != null)
            {
                CreatePage.Visible = true;
                //setCreateDetails();
            }
            else
            {
                ProfilePage.Visible = true;
                //setProfileDetails();
            }
        }

        protected void submitCreate_Click(object sender, EventArgs e)
        {

        }

        protected void updateProfile_Click(object sender, EventArgs e)
        {

        }
    }
}