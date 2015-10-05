using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

namespace CRM.Core {
	static public class PermissionHelper {
		static private bool AddPermssion = false;
		static private bool DeletePermission = false;
		static private bool EditPermission = false;
		static private bool ViewPermssion = false;

		static public bool checkAction(int actionID) {
			bool isAllowed = false;
			int roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client) {
				isAllowed = true;
			}
			else {
				List<int> roleActions = SessionHelper.getRoleActions();

				if (roleActions != null && roleActions.Count > 0) {
					isAllowed = roleActions.Any(x => x == actionID);
				}
			}

			return isAllowed;
		}

		static public void checkPermission(string pagename) {
			string accessDeniedURL = "~/AccessDenied.aspx";

			if (checkViewPermission(pagename) == false)
				HttpContext.Current.Response.Redirect(accessDeniedURL);
			
		}

		static public bool checkAddPermission(string pageName) {
			int roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client) {
				AddPermssion = true;
			}
			else {
				getPermissions(pageName);
			}

			return AddPermssion;
		}

		static public bool checkDeletePermission(string pageName) {
			int roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client) {
				DeletePermission = true;
			}
			else {
				getPermissions(pageName);
				
			}

			return DeletePermission;
		}

		static public bool checkEditPermission(string pageName) {
			int roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client) {
				EditPermission = true;
			}
			else {
				getPermissions(pageName);
			}

			return EditPermission;
		}

		static public bool checkViewPermission(string pageName) {
			
			int roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client) {
				ViewPermssion = true;
			}
			else {
				getPermissions(pageName);
			}

			return ViewPermssion;
		}

		static private void getPermissions(string url) {
			List<SecRoleModuleManager.secRoleModuleGet> rolePermissions = null;
			SecRoleModuleManager.secRoleModuleGet rolePermission = null;

			//string str = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
			string str = url.ToLower();
			string pagename = str.Substring(str.LastIndexOf("/") + 1);

			if (HttpContext.Current.Session["rolePermission"] != null) {
				rolePermissions = (List<SecRoleModuleManager.secRoleModuleGet>)HttpContext.Current.Session["rolePermission"];

				rolePermission = rolePermissions.Where(x => x.Url.ToLower() == pagename).FirstOrDefault();

				if (rolePermission != null) {
					AddPermssion = rolePermission.AddPermssion ?? false;
					EditPermission = rolePermission.EditPermission ?? false;
					DeletePermission = rolePermission.DeletePermission ?? false;
					ViewPermssion = rolePermission.ViewPermssion ?? false;
				}
			}
		}

		static public bool isAdmin() {
			return (HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserName"].ToString().ToLower().Equals("admin"));
		}
		
	
	}
}
