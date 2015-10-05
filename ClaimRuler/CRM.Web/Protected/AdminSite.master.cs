
namespace CRM.Web.Protected {
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
    using CRM.Data.Entities;

	public partial class AdminSite : System.Web.UI.MasterPage {
		List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {
				string str = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
				string pagename = str.Substring(str.LastIndexOf("/") + 1);

				if (pagename.ToLower().Contains("dashboard.aspx") ||
					pagename.ToLower().Contains("newlead.aspx") ||
					pagename.ToLower().Contains("leadsimagesupload.aspx") ||
					pagename.ToLower().Contains("allusersleads.aspx") ||
					pagename.ToLower().Contains("usersleads.aspx") ||
					pagename.ToLower().Contains("leadsupload.aspx") ||
					pagename.ToLower().Contains("leadcomments.aspx") ||
					pagename.ToLower().Contains("leadsimagesuploaddescription.aspx") ||
					pagename.ToLower().Contains("leademail.aspx") ||
					pagename.ToLower().Contains("tasks.aspx") ||
					pagename.ToLower().Contains("leadsimagesuploadmobile.aspx") ||
					pagename.ToLower().Contains("emailsettings.aspx") ||
					pagename.ToLower().Contains("leadlimitreached.aspx") ||
					pagename.ToLower().Contains("leadschedule.aspx") ||
					// letters
					pagename.ToLower().Contains("contractadjustingservices.aspx") ||
					pagename.ToLower().Contains("mergetemplateletter.aspx") 	||
					pagename.ToLower().Contains("leadimportemail.aspx") 					
					) {
				}
				else {
					if (Session["rolePermission"] != null) {
						resRolePermission = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];
						resRolePermission = resRolePermission.Where(x => (x.Url.ToLower() == pagename)).ToList();
						if (resRolePermission.Count > 0) {
							hfViewPermission.Value = resRolePermission[0].ViewPermssion == null ? "0" : resRolePermission[0].ViewPermssion == true ? "1" : "0";
						}
						if (hfViewPermission.Value == "0") {
							Response.Redirect("~/Protected/Admin/ServiceNotAvailable.aspx");
						}
					}
				}
			}
		}

		private void setUserName() {
			CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(Convert.ToInt32(Session["UserId"]));
			if (user.UserName != null) {
			}
		}
		protected void logOut_Click(object sender, ImageClickEventArgs e) {
			if (Session["UserId"] != null && Session["UserId"].ToString().Length > 0 && Convert.ToInt32(Session["UserId"]) > 0) {
				try {
					string ErrorMessage = string.Empty;
					createLoginLog();
				}
				catch (Exception ex) {

				}
			}
			FormsAuthentication.SignOut();
			Session.Abandon();
			Response.Redirect("~/Login.aspx");
		}

		private void createLoginLog() {
			SecLoginLog loginlog = new SecLoginLog();

			/*changed after DB changes*/
			//loginlog.userid = (int)Session["UserId"];
			loginlog.UserId = (int)Session["UserId"];

			SecLoginLogManager.Update(loginlog);
		}
		
	}
}