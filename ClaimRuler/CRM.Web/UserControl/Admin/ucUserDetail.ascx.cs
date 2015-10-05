

namespace CRM.Web.UserControl.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CRM.Data.Account;
    using CRM.Data;
    using LinqKit;
    using System.Globalization;

    public partial class ucUserDetail : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillForm();
            }
        }
        // ** Display  The User Data ** //
        private void FillForm()
        {
            string userId = Session["UIDV"].ToString();// Request.QueryString["userid"];

            if (String.IsNullOrWhiteSpace(userId))
                return;

            var user = SecUserManager.GetByUserId(Convert.ToInt32(userId));

            lblUserName.Text = user.UserName;
            lblFitstName.Text = user.FirstName;
            lblLastName.Text = user.LastName;
            lblRoleName.Text = user.SecRole.RoleName;
            if (user.Status == true)
            {
                lblStatus.Text = "Active";
            }
            else
            {
                lblStatus.Text = "In-Active";
            }
           
           if (user.Blocked == true)
            {
                lblBlocked.Text = "Unlocked";
            }
            else
            {
                lblBlocked.Text = "Locked";
            }

        }
    }
}