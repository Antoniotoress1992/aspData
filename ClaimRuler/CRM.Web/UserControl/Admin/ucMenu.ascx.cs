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
using System.Text;
using CRM.Data.Entities;


namespace CRM.Web.UserControl.Admin
{
    public partial class ucMenu : System.Web.UI.UserControl
    {
        int currentUserRoleId = 0;
        int currentUserId = 0;

        public string h1, h2, h3, h4, h5;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserWelcome.Text = "";
            if (Session["h1"] != null && Session["h2"] != null && Session["h3"] != null && Session["h4"] != null && Session["h5"] != null)
            {
                h1 = Session["h1"].ToString();
                h2 = Session["h2"].ToString();
                h3 = Session["h3"].ToString();
                h4 = Session["h4"].ToString();
                h5 = Session["h5"].ToString();
            }
            string[] userRoleName = (((FormsIdentity)HttpContext.Current.User.Identity).Ticket).UserData.Split('|');
            currentUserRoleId = Convert.ToInt32(userRoleName[1]);
            currentUserId = Convert.ToInt32(Session["UserId"]);
            if (currentUserId <= 0 || userRoleName[0].ToString().Trim().Replace(" ", "").ToUpper() == "FORALL")
                LogoutUser();

            lblUserWelcome.Text = "Welcome (" + userRoleName[0].ToString() + ")";

            List<SecRoleModuleManager.secRoleModuleGet> resRoleModule = new List<SecRoleModuleManager.secRoleModuleGet>();
            resRoleModule = SecRoleModuleManager.getRoleModuleMenu(currentUserRoleId);
            List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleParent = resRoleModule.Where(x => (x.ParentId == 0 || x.ParentId == null)).ToList();
            List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleChild = new List<SecRoleModuleManager.secRoleModuleGet>();

            StringBuilder strMenu = new StringBuilder();

            strMenu.Append("<div id='smoothmenu1' class='ddsmoothmenu'>");
            strMenu.Append("<ul style='padding-left:40px;'>");
            if (resRoleModuleParent.Count > 0)
            {
                if (resRoleModuleParent[0].RoleID == 1)
                    strMenu.Append("<li><a href='../../Protected/Admin/AllUsersLeads.aspx'>Home</a></li>");
                else
                    strMenu.Append("<li><a href='../../Protected/Admin/UsersLeads.aspx'>Home</a></li>");
            }
            foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleParent in resRoleModuleParent)
            {



                if (objRoleModuleParent.Url.Trim() == "")
                {
                    if (objRoleModuleParent.ModuleId > 0)
                    {
                        List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionParent = resRoleModule.Where(x => (x.ModuleId == objRoleModuleParent.ModuleId) && (x.ViewPermssion == true)).ToList();
                        List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
                        checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
                        if (checkforchild.Count > 0 || checkviewPermissionParent.Count > 0)
                        {
                            strMenu.Append("<li><a href='#'>" + objRoleModuleParent.ModuleName + "</a>");
                        }
                    }
                }
                else
                {
                    List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionParent = resRoleModule.Where(x => (x.ModuleId == objRoleModuleParent.ModuleId) && (x.ViewPermssion == true)).ToList();
                    List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
                    checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
                    if (checkforchild.Count > 0 || checkviewPermissionParent.Count > 0)
                    {
                        strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleParent.Url + "'>" + objRoleModuleParent.ModuleName + "</a>");
                    }
                }

                resRoleModuleChild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId))).ToList();
                if (resRoleModuleChild.Count > 0)
                {
                    strMenu.Append("<ul>");
                    foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleChild in resRoleModuleChild)
                    {
                        List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleChildChildren = new List<SecRoleModuleManager.secRoleModuleGet>();
                        resRoleModuleChildChildren = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();

                        if (objRoleModuleChild.Url.Trim() == "")
                        {
                            List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
                            List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
                            checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
                            if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0)
                            {
                                strMenu.Append("<li><a href='#'>" + objRoleModuleChild.ModuleName + "</a>");
                            }

                            strMenu.Append("<ul>");
                            foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleChildChildren in resRoleModuleChildChildren)
                            {
                                if (objRoleModuleChildChildren.Url.Trim() == "")
                                {
                                    strMenu.Append("<li><a href='#'>" + objRoleModuleChildChildren.ModuleName + "</a>");
                                }
                                else
                                {
                                    strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleChildChildren.Url + "'>" + objRoleModuleChildChildren.ModuleName + "</a></li>");
                                }

                            }
                            strMenu.Append("</ul>");
                            strMenu.Append("</li>");

                        }
                        else if (objRoleModuleChild.Url.Trim() == "AllUserLeadsReport.aspx")
                        {
                            List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
                            List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
                            checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
                            if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0)
                            {
                                strMenu.Append("<li><a href='../../Protected/Reports/" + objRoleModuleChild.Url + "'>" + objRoleModuleChild.ModuleName + "</a></li>");
                            }
                        }
                        else
                        {
                            List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
                            List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
                            checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
                            if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0)
                            {
                                strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleChild.Url + "'>" + objRoleModuleChild.ModuleName + "</a></li>");
                            }
                        }
                    }

                    strMenu.Append("</ul>");
                }
                strMenu.Append("</li>");
            }
            strMenu.Append("</ul>");
            strMenu.Append("<br style='clear: left' />");
            strMenu.Append("</div>");
            dvMenu.InnerHtml = strMenu.ToString();
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "menukey", "cssdropdown.startchrome('chromemenu2');", true);
            setUserName();
        }

        private void setUserName()
        {
           CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(Convert.ToInt32(Session["UserId"]));
            if (user.UserName != null)
            {
                lblUserWelcome.Text = "Welcome (" + user.UserName + ")";
            }
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            LogoutUser();
        }

        private void LogoutUser()
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