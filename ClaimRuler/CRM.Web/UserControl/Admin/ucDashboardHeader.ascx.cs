
namespace CRM.Web.UserControl.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CRM.Data;
    using CRM.Data.Account;
    using System.Web.Security;
    using CRM.Core;
    using LinqKit;
    using CRM.Data.Entities;

    public partial class ucDashboardHeader : System.Web.UI.UserControl
    {
        int currentUserRoleId = 0;
        int currentUserId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] userRoleName = (((FormsIdentity)HttpContext.Current.User.Identity).Ticket).UserData.Split('|');
            currentUserRoleId = Convert.ToInt32(userRoleName[1]);
            currentUserId = Convert.ToInt32(Session["UserId"]);
            if (currentUserRoleId == 2)
            {
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] != null && Session["UserId"].ToString().Length > 0 && Convert.ToInt32(Session["UserId"]) > 0)
            {
                try
                {
                    string ErrorMessage = string.Empty;
                    createLoginLog();
                }
                catch (Exception ex)
                {

                }
            }
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        private void createLoginLog()
        {
            SecLoginLog loginlog = new SecLoginLog();

            loginlog.UserId = (int)Session["UserId"];

            SecLoginLogManager.Update(loginlog);
        }
    }
}