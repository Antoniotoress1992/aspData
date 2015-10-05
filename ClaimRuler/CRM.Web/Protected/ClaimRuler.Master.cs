using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Web.Utilities;

using Infragistics.Web.UI.EditorControls;

namespace CRM.Web.Protected {
	public partial class ClaimRuler : System.Web.UI.MasterPage {

		// tooltip: http://gdakram.github.io/JQuery-Tooltip-Plugin/
		int currentUserRoleId = 0;
		int currentUserId = 0;
		List<SecRoleModuleManager.secRoleModuleGet> rolePermissions = null;
		SecRoleModuleManager.secRoleModuleGet rolePermission = null;

		private void buildMenu() {
			int clientID = Core.SessionHelper.getClientId();

			Infragistics.Web.UI.NavigationControls.ExplorerBarGroup menuGroup = null;
			Infragistics.Web.UI.NavigationControls.ExplorerBarItem menuItem = null;
			Infragistics.Web.UI.NavigationControls.ExplorerBarGroup subMenuGroup = null;
			List<SecRoleModuleManager.secRoleModuleGet> resRoleModule = null;

			lblUserWelcome.Text = "";
			if (Session["h1"] != null && Session["h2"] != null && Session["h3"] != null && Session["h4"] != null && Session["h5"] != null) {
				h1 = Session["h1"].ToString();
				h2 = Session["h2"].ToString();
				h3 = Session["h3"].ToString();
				h4 = Session["h4"].ToString();
				h5 = Session["h5"].ToString();
			}
			string[] userRoleName = (((FormsIdentity)HttpContext.Current.User.Identity).Ticket).UserData.Split('|');

			currentUserRoleId = Convert.ToInt32(userRoleName[1]);

			currentUserId = Convert.ToInt32(Session["UserId"]);

			//if (currentUserId <= 0 || userRoleName[0].ToString().Trim().Replace(" ", "").ToUpper() == "FORALL")
			//LogoutUser();

			//lblUserWelcome.Text = "Welcome (" + userRoleName[0].ToString() + ")";
			//lblUserWelcome.Text = string.Format("Welcome ({0})", Session["UserName"] ?? "");

			// fill socialbox
			lblUserWelcome.Text = string.Format("{0}", Session["UserName"] ?? "");

			// first/last name
			lblUserName.Text = userRoleName[2];

			if (userRoleName.Length > 3)
				lblUserEmail.Text = userRoleName[3] ?? "n/a";


			resRoleModule = new List<SecRoleModuleManager.secRoleModuleGet>();

			if (currentUserRoleId == (int)UserRole.Administrator || currentUserRoleId == (int)UserRole.Client) {
				resRoleModule = SecRoleModuleManager.getRoleModuleMenu(currentUserRoleId);
			}
			else {
				resRoleModule = SecRoleModuleManager.getRoleModuleMenu(clientID, currentUserRoleId);
			}

			List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleParent =
					resRoleModule.Where(x => (x.ParentId == 0 || x.ParentId == null)).OrderBy(x => x.SortOrder).ToList();
			List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleChild = new List<SecRoleModuleManager.secRoleModuleGet>();

			List<Infragistics.Web.UI.NavigationControls.ExplorerBarGroup> igCurrentMenuGroups = navBar.Groups.Cast<Infragistics.Web.UI.NavigationControls.ExplorerBarGroup>().ToList();

			List<Infragistics.Web.UI.NavigationControls.ExplorerBarGroup> menuGroups = new List<Infragistics.Web.UI.NavigationControls.ExplorerBarGroup>();

			// remove any existing groups in declarative text
			navBar.Groups.Clear();

			foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleParent in resRoleModuleParent) {

				if (objRoleModuleParent.Url.Trim() == "") {
					if (objRoleModuleParent.ModuleId > 0) {
						List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionParent = resRoleModule.Where(x => (x.ModuleId == objRoleModuleParent.ModuleId) && (x.ViewPermssion == true)).OrderBy(x => x.SortOrder).ToList();
						List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
						checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
						if (checkforchild.Count > 0 || checkviewPermissionParent.Count > 0) {
							//strMenu.Append("<li><a href='#'>" + objRoleModuleParent.ModuleName + "</a>");
							menuGroup = new Infragistics.Web.UI.NavigationControls.ExplorerBarGroup(objRoleModuleParent.ModuleName);
							menuGroup.Value = objRoleModuleParent.ModuleId.ToString();
							menuGroups.Add(menuGroup);
							//navBar.Groups.Add(menuGroup);
						}
					}
				}
				else {
					List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionParent = resRoleModule.Where(x => (x.ModuleId == objRoleModuleParent.ModuleId) && (x.ViewPermssion == true)).ToList();
					List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
                    checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).OrderBy(x => x.SortOrder).ToList();
					if (checkforchild.Count > 0 || checkviewPermissionParent.Count > 0) {
						//strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleParent.Url + "'>" + objRoleModuleParent.ModuleName + "</a>");
						menuGroup = new Infragistics.Web.UI.NavigationControls.ExplorerBarGroup(objRoleModuleParent.ModuleName);
						menuGroup.Value = objRoleModuleParent.ModuleId.ToString();
						// enable this to add URL navigation to group 
						//menuGroup.NavigateUrl = "~/Protected/Admin/" + objRoleModuleParent.Url;

						menuGroups.Add(menuGroup);
					}
				}



				// get all menu options for menu group
				resRoleModuleChild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId))).OrderBy(x=>x.SortOrder).ToList();
               
				if (resRoleModuleChild.Count > 0) {
					//strMenu.Append("<ul>");
					foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleChild in resRoleModuleChild) {
						List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleChildChildren = new List<SecRoleModuleManager.secRoleModuleGet>();
						resRoleModuleChildChildren = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();

						menuItem = new Infragistics.Web.UI.NavigationControls.ExplorerBarItem();
						menuGroup.Items.Add(menuItem);
						menuItem.Text = objRoleModuleChild.ModuleName;
						menuItem.Value = objRoleModuleChild.ModuleId.ToString();

						menuItem.NavigateUrl = string.Format("~/Protected/Admin/{0}?p={1}&c={2}", objRoleModuleChild.Url, menuGroup.Value, menuItem.Value);

						// 2014-04-16
						//if (menuItem.NavigateUrl.Contains("Reports"))
						//	menuItem.Target = "_blank";

						//subMenuGroup = new Infragistics.Web.UI.NavigationControls.ExplorerBarGroup(objRoleModuleChild.ModuleName);
						//menuGroup.Items.Add(subMenuGroup);	

						if (objRoleModuleChild.Url.Trim() == "") {
							List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
							List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
							checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
							if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0) {
								//strMenu.Append("<li><a href='#'>" + objRoleModuleChild.ModuleName + "</a>");


							}

							//strMenu.Append("<ul>");
							foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleChildChildren in resRoleModuleChildChildren) {
								if (objRoleModuleChildChildren.Url.Trim() == "") {
									//strMenu.Append("<li><a href='#'>" + objRoleModuleChildChildren.ModuleName + "</a>");
									//subMenuGroup.Items.Add(new Infragistics.Web.UI.NavigationControls.ExplorerBarItem(objRoleModuleChildChildren.ModuleName));
								}
								else {
									//strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleChildChildren.Url + "'>" + objRoleModuleChildChildren.ModuleName + "</a></li>");
									//subMenuGroup.Items.Add(new Infragistics.Web.UI.NavigationControls.ExplorerBarItem(objRoleModuleChildChildren.ModuleName));
								}

							}


						}
						else if (objRoleModuleChild.Url.Trim() == "AllUserLeadsReport.aspx") {
							List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
							List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
							checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
							if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0) {
								//strMenu.Append("<li><a href='../../Protected/Reports/" + objRoleModuleChild.Url + "'>" + objRoleModuleChild.ModuleName + "</a></li>");
							}
						}
						else {
							List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
							List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
							checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
							if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0) {
								//strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleChild.Url + "'>" + objRoleModuleChild.ModuleName + "</a></li>");
							}
						}
					}

					//strMenu.Append("</ul>");
				}
				//strMenu.Append("</li>");

			}

			foreach (Infragistics.Web.UI.NavigationControls.ExplorerBarGroup group in menuGroups)
				navBar.Groups.Add(group);

			foreach (Infragistics.Web.UI.NavigationControls.ExplorerBarGroup group in igCurrentMenuGroups)
				navBar.Groups.Add(group);
		}

		protected void ibtnSearch_Click(object sender, ImageClickEventArgs e) {
			string url = null;

			if (!String.IsNullOrEmpty(txtSearchText.Text)) {
				url = string.Format("~/protected/Admin/UsersLeads.aspx?s={0}", txtSearchText.Text);

				Response.Redirect(url);
			}
		}

		public void checkPermission() {
			string accessDeniedURL = "~/AccessDenied.aspx";
			string pagename = null;

			if (Session["rolePermission"] != null) {
				// get requested page
				pagename = getRequestPageName();


				if (this.hasViewPermission == false) {
					// user has no permission to visit this page
					Response.Redirect(accessDeniedURL);
				}
			}
			else {
				Response.Redirect(accessDeniedURL);
			}
		}

		public void checkPermission(string url) {
			string accessDeniedURL = "~/AccessDenied.aspx";

			if (Session["rolePermission"] != null) {
				// user has no permission to visit this page
				if (!Core.PermissionHelper.checkViewPermission(url))
					Response.Redirect(accessDeniedURL);
			}
			else {
				Response.Redirect(accessDeniedURL);
			}
		}


		/// <summary>
		/// Disable DropDownList, TextBox, WebTextEditor
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="State"></param>
		public void disableControls(Control parent, bool State) {
			foreach (Control c in parent.Controls) {
				if (c is DropDownList) {
					((DropDownList)(c)).Enabled = State;
				}
				if (c is TextBox) {
					((TextBox)(c)).Enabled = State;
				}
				if (c is WebTextEditor) {
					((WebTextEditor)(c)).Enabled = State;
				}
				//if (c is Button) {
				//	((Button)(c)).Enabled = State;
				//}
				//if (c is ImageButton) {
				//	((ImageButton)(c)).Enabled = State;
				//}
				//if (c is HyperLink) {
				//	((HyperLink)(c)).Enabled = State;
				//}
				disableControls(c, State);
			}
		}

		private string getRequestPageName() {
			string str = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
			string pagename = str.Substring(str.LastIndexOf("/") + 1);

			return pagename;
		}

		public bool hasEditPermission {
			get {

				bool hasPermission = true;

				if (checkUserRoleRequiresPermission()) {
					rolePermissions = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];

					string pageName = getRequestPageName();

					hasPermission = rolePermissions.Any(x => x.Url.Substring(x.Url.LastIndexOf("/") + 1).ToLower() == pageName && x.EditPermission == true);
				}

				return hasPermission;
			}
		}
		public bool hasAddPermission {
			get {
				bool hasPermission = true;

				if (checkUserRoleRequiresPermission()) {
					rolePermissions = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];

					string pageName = getRequestPageName();

					hasPermission = rolePermissions.Any(x => x.Url.Substring(x.Url.LastIndexOf("/") + 1).ToLower() == pageName && x.AddPermssion == true);
				}

				return hasPermission;
			}
		}

		public bool hasViewPermission {
			get {
				bool hasPermission = true;

				if (checkUserRoleRequiresPermission()) {
					rolePermissions = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];

					string pageName = getRequestPageName();

					hasPermission = rolePermissions.Any(x => x.Url.Substring(x.Url.LastIndexOf("/") + 1).ToLower() == pageName && x.ViewPermssion == true);
				}

				return hasPermission;
			}
		}

		public bool hasDeletePermission {
			get {
				bool hasPermission = true;

				if (checkUserRoleRequiresPermission()) {
					rolePermissions = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];

					string pageName = getRequestPageName();

					hasPermission = rolePermissions.Any(x => x.Url.Substring(x.Url.LastIndexOf("/") + 1).ToLower() == pageName && x.DeletePermission == true);
				}

				return hasPermission;
			}
		}

		public string h1, h2, h3, h4, h5;

		public void btnReturnToClaim_Click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/ClaimEdit.aspx");
		}


        //public void btnReturnToClient_Click(object sender, EventArgs e)
        //{
        //    int leadID = Core.SessionHelper.getLeadId();
        //    if (leadID > 0)
        //    {
        //        Response.Redirect("~/Protected/NewLead.aspx?q=" + Core.SecurityManager.EncryptQueryString(leadID.ToString()));
        //    }
        //}

        public void btnReturnToPolicy_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Protected/LeadPolicyEdit.aspx");
        }



		public void btnReturnToClient_Click(object sender, EventArgs e) {
			int leadID = Core.SessionHelper.getLeadId();
			if (leadID > 0) {
				Response.Redirect("~/Protected/NewLead.aspx?q=" + Core.SecurityManager.EncryptQueryString(leadID.ToString()));
			}
		}

		private bool checkUserRoleRequiresPermission() {
			bool hasPermission = true;

			currentUserRoleId = SessionHelper.getUserRoleId();

			if (currentUserRoleId == (int)UserRole.Client || currentUserRoleId == (int)UserRole.Administrator)
				hasPermission = false;

			return hasPermission;
		}

		protected void Page_Load(object sender, EventArgs e) {
		
			if (!Page.IsPostBack) {
				buildMenu();

				reselectMenuOption();
			}

			hf_userID.Value = SessionHelper.getUserId().ToString();

			imgUser.ImageUrl = Core.Common.getUserPhotoURL(SessionHelper.getUserId());
			user_thumbnail.ImageUrl = Core.Common.getUserPhotoURL(SessionHelper.getUserId()); 			
		}

		// this method gets executed before page load
		protected void Page_Init(object sender, EventArgs e) {
			if (Session["UserId"] == null) {
				logout();


				//Response.Redirect("~/Login.aspx");
			}

			//if (SessionHelper.getUserRoleId() != (int)UserRole.Administrator)
			//	checkPermission();
		}


		public int LeadID {
			get { return Core.SessionHelper.getLeadId(); }
		}

		protected void logOut_Click(object sender, EventArgs e) {
			logout();

		}

		private void logout() {
			Session.Abandon();

			FormsAuthentication.SignOut();

			Response.Redirect("~/Login.aspx");
		}

		private void reselectMenuOption() {
			string parentMenuValue = null;
			string childMenuValue = null;
			Infragistics.Web.UI.NavigationControls.ExplorerBarGroup menuGroup = null;
			Infragistics.Web.UI.NavigationControls.ExplorerBarItem menuItem = null;

			if (Request.Params["p"] != null && Request.Params["c"] != null) {
				parentMenuValue = Request.Params["p"].ToString();
				childMenuValue = Request.Params["c"].ToString();

				menuGroup = (Infragistics.Web.UI.NavigationControls.ExplorerBarGroup)navBar.Groups.Where(x => x.Value == parentMenuValue).FirstOrDefault();
				menuGroup.Selected = true;
				menuGroup.Expanded = true;

				menuItem = (Infragistics.Web.UI.NavigationControls.ExplorerBarItem)menuGroup.Items.Where(x => x.Value == childMenuValue).FirstOrDefault();
				if (menuItem != null)
					menuItem.Selected = true;
			}
		}

		protected void lbtnLogout_Click(object sender, EventArgs e) {
			logout();
		}

		
	}
}