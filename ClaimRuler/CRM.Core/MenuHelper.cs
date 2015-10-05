using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI.HtmlControls;

using CRM.Data.Account;

namespace CRM.Core {
	static public class MenuHelper {
		static public void buildMenu(HtmlGenericControl dvMenu, List<SecRoleModuleManager.secRoleModuleGet> menuItems) {
			int currentUserRoleId = 0;
			int currentUserId = 0;
			StringBuilder strMenu = new StringBuilder();

			List<SecRoleModuleManager.secRoleModuleGet> resRoleModule = new List<SecRoleModuleManager.secRoleModuleGet>();
			//List<SecRoleModuleManager.secRoleModuleGet> resRoleModule = menuItems;

			resRoleModule = SecRoleModuleManager.getRoleModuleMenu(currentUserRoleId);
			List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleParent = menuItems.Where(x => (x.ParentId == 0 || x.ParentId == null)).ToList();
			List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleChild = new List<SecRoleModuleManager.secRoleModuleGet>();


			strMenu.Append("<div id='smoothmenu2' class='ddsmoothmenu'>");
			strMenu.Append("<ul style='padding-left:40px;'>");

			foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleParent in resRoleModuleParent) {



				if (objRoleModuleParent.Url.Trim() == "") {
					if (objRoleModuleParent.ModuleId > 0) {
						List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionParent = resRoleModule.Where(x => (x.ModuleId == objRoleModuleParent.ModuleId) && (x.ViewPermssion == true)).ToList();
						List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
						checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
						if (checkforchild.Count > 0 || checkviewPermissionParent.Count > 0) {
							strMenu.Append("<li><a href='#'>" + objRoleModuleParent.ModuleName + "</a>");
						}
					}
				}
				else {
					List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionParent = resRoleModule.Where(x => (x.ModuleId == objRoleModuleParent.ModuleId) && (x.ViewPermssion == true)).ToList();
					List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
					checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
					if (checkforchild.Count > 0 || checkviewPermissionParent.Count > 0) {
						strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleParent.Url + "'>" + objRoleModuleParent.ModuleName + "</a>");
					}
				}

				resRoleModuleChild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleParent.ModuleId))).ToList();
				if (resRoleModuleChild.Count > 0) {
					strMenu.Append("<ul>");
					foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleChild in resRoleModuleChild) {
						List<SecRoleModuleManager.secRoleModuleGet> resRoleModuleChildChildren = new List<SecRoleModuleManager.secRoleModuleGet>();
						resRoleModuleChildChildren = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();

						if (objRoleModuleChild.Url.Trim() == "") {
							List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
							List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
							checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
							if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0) {
								strMenu.Append("<li><a href='#'>" + objRoleModuleChild.ModuleName + "</a>");
							}

							strMenu.Append("<ul>");
							foreach (SecRoleModuleManager.secRoleModuleGet objRoleModuleChildChildren in resRoleModuleChildChildren) {
								if (objRoleModuleChildChildren.Url.Trim() == "") {
									strMenu.Append("<li><a href='#'>" + objRoleModuleChildChildren.ModuleName + "</a>");
								}
								else {
									strMenu.Append("<li><a href='../../Protected/Admin/" + objRoleModuleChildChildren.Url + "'>" + objRoleModuleChildChildren.ModuleName + "</a></li>");
								}

							}
							strMenu.Append("</ul>");
							strMenu.Append("</li>");

						}
						else if (objRoleModuleChild.Url.Trim() == "AllUserLeadsReport.aspx") {
							List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
							List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
							checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
							if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0) {
								strMenu.Append("<li><a href='../../Protected/Reports/" + objRoleModuleChild.Url + "'>" + objRoleModuleChild.ModuleName + "</a></li>");
							}
						}
						else {
							List<SecRoleModuleManager.secRoleModuleGet> checkviewPermissionChild = resRoleModule.Where(x => (x.ModuleId == objRoleModuleChild.ModuleId) && (x.ViewPermssion == true)).ToList();
							List<SecRoleModuleManager.secRoleModuleGet> checkforchild = new List<SecRoleModuleManager.secRoleModuleGet>();
							checkforchild = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(objRoleModuleChild.ModuleId)) && (x.ViewPermssion == true || x.ViewPermssion == null)).ToList();
							if (checkforchild.Count > 0 || checkviewPermissionChild.Count > 0) {
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
			//this.Page.ClientScript.RegisterStartupScript(this.GetType(), "menukey", "cssdropdown.startchrome('chromemenu2');", true);
		}

	}
}
