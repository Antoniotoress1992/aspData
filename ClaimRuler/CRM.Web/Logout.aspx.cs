using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null && Session["UserId"].ToString().Length > 0 && Convert.ToInt32(Session["UserId"]) > 0)
            {
                try
                {
                    string ErrorMessage = string.Empty;
                    createLoginLog();
                }
                catch { }
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