using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Data.Account;
using CRM.Data;
using LinqKit;
using CRM.Core;
using System.Globalization;

using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {

	public partial class ucEditUser : System.Web.UI.UserControl {
		int roleID = SessionHelper.getUserRoleId();
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!IsPostBack) {
				bindRole();
				DoBind();
			}
		}

		private Expression<Func<CRM.Data.Entities.SecUser, bool>> buildPredicate() {
			Expression<Func<CRM.Data.Entities.SecUser, bool>> predicate = null;
			int clientID = Core.SessionHelper.getClientId();

			predicate = PredicateBuilder.True<CRM.Data.Entities.SecUser>();
			
			predicate = predicate.And(x => (x.RoleId != 2));
			predicate = predicate.And(x => (x.UserId != 0));

			if (roleID > 0) {
				if (!String.IsNullOrWhiteSpace(hfKeywordSearch.Value)) {
					var keyword = hfKeywordSearch.Value;
					predicate = predicate.And(user => user.UserName.Contains(keyword) || user.FirstName.Contains(keyword) || user.LastName.Contains(keyword));
				}
				if (Convert.ToInt32(hfRole.Value) > 0) {
					var keyword = Convert.ToInt32(hfRole.Value);
					predicate = predicate.And(user => user.RoleId == keyword);
				}
				if (Convert.ToInt32(hfStatus.Value) > 0) {
					bool keyword;
					if (hfStatus.Value == "2") {
						keyword = false;
					}
					else {
						keyword = true;
					}
					predicate = predicate.And(user => user.Status == keyword);
				}

				switch (roleID) {
					case (int)UserRole.Administrator:
						break;

				
					default:
						predicate = predicate.And(x => x.ClientID == clientID);

						break;
				}
			}

			return predicate;
		}

		private void DoBind() {
			Expression<Func<CRM.Data.Entities.SecUser, bool>> predicate = buildPredicate();

			IQueryable<CRM.Data.Entities.SecUser> secUsers = SecUserManager.GetUsers(predicate);

			gvUsers.DataSource = secUsers.ToList();//.orderByExtension("LastName", false);
			
			gvUsers.DataBind();

			// clear 
			Session["UID"] = null;
		}

		private void bindRole() {
			List<SecRole> roles = null;

			roles = SecRoleManager.GetAll(roleID);

			CollectionManager.FillCollection(ddlRole, "RoleId", "RoleName", roles);

		}

		protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			if (e.CommandName.Equals("DoEdit")) {
				Session["UID"] = e.CommandArgument;
				var url = "~/protected/admin/UserEdit.aspx";
				Response.Redirect(url);
			}
			else if (e.CommandName.Equals("DoDelete")) {

				try {
					var user = SecUserManager.GetByUserId(Convert.ToInt32(e.CommandArgument));
					user.Status = false;
					SecUserManager.Save(user);
					DoBind();
					lblSave.Text = "Record Deleted Sucessfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Deleted.";
					lblError.Visible = true;
				}
			}
			else if (e.CommandName.Equals("DoView")) {

				Session["UIDV"] = e.CommandArgument;
				var url = "~/protected/admin/UserDetail.aspx";
				Response.Redirect(url);

				Response.Redirect(url);
			}
			else if (e.CommandName.Equals("DoUnlock")) {

				try {
					var user = SecUserManager.GetByUserId(Convert.ToInt32(e.CommandArgument));
					user.Blocked = false;
					SecUserManager.Save(user);
					DoBind();
					lblSave.Text = "User Unlock Successfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "User Not Unlocked.";
					lblError.Visible = true;
				}
			}
			else if (e.CommandName.Equals("Dolock")) {

				try {
					var user = SecUserManager.GetByUserId(Convert.ToInt32(e.CommandArgument));
					user.Blocked = true;
					SecUserManager.Save(user);
					DoBind();
					lblSave.Text = "User lock Successfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "User Not locked.";
					lblError.Visible = true;
				}
			}
			else if (e.CommandName.Equals("DoImpersonate")) {
				impersonateUser(Convert.ToInt32(e.CommandArgument));
			}
		}

		protected void btnSearch_Click(object sender, EventArgs e) {
			// Search The Data
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			if (!String.IsNullOrWhiteSpace(txtSearch.Text) || ddlRole.SelectedIndex > 0 || ddlStatus.SelectedIndex > 0) {
				hfKeywordSearch.Value = txtSearch.Text.Trim();
				hfRole.Value = ddlRole.SelectedValue;
				hfStatus.Value = ddlStatus.SelectedValue;
			}
			DoBind();

		}

		protected void btnReset_Click(object sender, EventArgs e) {
			// Reset the form controls
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			txtSearch.Text = string.Empty;
			ddlStatus.SelectedIndex = 0;
			ddlRole.SelectedIndex = 0;

			hfKeywordSearch.Value = "";
			hfRole.Value = "0";
			hfStatus.Value = "0";
			DoBind();
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {			
			SortDirection sortDirection = SortDirection.Ascending;
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;


			sortDirection = descending ? SortDirection.Descending : SortDirection.Ascending;

			ViewState[e.SortExpression] = descending;

			Expression<Func<CRM.Data.Entities.SecUser, bool>> predicate = buildPredicate();

			IQueryable<CRM.Data.Entities.SecUser> secUsers = SecUserManager.GetUsers(predicate);

            if (sortDirection == SortDirection.Descending)
            {
                switch (e.SortExpression)
                {
                    case "Username":
                        gvUsers.DataSource = secUsers.OrderByDescending(x=>x.UserName).ToList();
                        break;
                    case "LastName":
                         gvUsers.DataSource = secUsers.OrderByDescending(x=>x.LastName).ToList();
                        break;
                    case "FirstName":
                         gvUsers.DataSource = secUsers.OrderByDescending(x=>x.FirstName).ToList();
                         break;
                    ////default:
                    ////     gvUsers.DataSource = secUsers.OrderByDescending(x => x.SecRole.RoleName).ToList();
                    ////    break;
                }
                //.orderByNested(e.SortExpression, sortDirection);           
			gvUsers.DataBind();
            }
            else
            {
            gvUsers.DataSource = secUsers.orderBy(e.SortExpression).ToList();//.orderByNested(e.SortExpression, sortDirection);
           
			gvUsers.DataBind();
            }

			ViewState["lastSortExpression"] = e.SortExpression;
			ViewState["lastSortDirection"] = descending;
		}

		//protected void lvData_PreRender(object sender, EventArgs e) {

		//	//DoBind();

		//	string sortExpression = ViewState["lastSortExpression"] == null ? "LastName" : ViewState["lastSortExpression"].ToString();
		//	bool descending = ViewState["lastSortDirection"] == null ? false : (bool)ViewState["lastSortDirection"];

		//	Expression<Func<SecUser, bool>> predicate = buildPredicate();

		//	IQueryable<SecUser> secUsers = SecUserManager.GetUsers(predicate);

		//	gvUsers.DataSource = secUsers.orderByExtension(sortExpression, descending);
		//	gvUsers.DataBind();

		//}

		//protected void gvUsers_ItemDataBound(object sender, ListViewItemEventArgs e) {
		//	if (e.Item.ItemType == ListViewItemType.DataItem) {
		//		ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDelete");
		//		Label lbltotal = (Label)e.Item.FindControl("lbltotal");
		//		if (imgDelete != null) {
		//			if (imgDelete.Enabled == false)
		//				imgDelete.Style.Add(HtmlTextWriterStyle.Cursor, "default");
		//		}

		//		if (SessionHelper.getUserName() == "admin") {
		//			ImageButton btnImpersonate = (ImageButton)e.Item.FindControl("btnImpersonate");
		//			btnImpersonate.Visible = true;
		//		}
		//	}
		//}

		protected void impersonateUser(int userID) {
			List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
			string userData = null;

			CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);
			if (user != null) {
				Session["UserId"] = user.UserId;
				Session["UserName"] = user.UserName;
				Session["RoleId"] = user.SecRole.RoleId.ToString();

				// 201307-29
				Session["ClientId"] = user.ClientID;
				
				userData = string.Format("{0}|{1}|{2} {3}|{4}", user.SecRole.RoleName, user.SecRole.RoleId, user.FirstName, user.LastName, user.Email);

				var ticket = new FormsAuthenticationTicket
				    (
					   1,
					   user.UserId.ToString(),
					   DateTime.Now,
					   DateTime.Now.AddMinutes(120),
					   true,
					   userData,	//(user.SecRole.RoleName + "|" + user.SecRole.RoleId.ToString()),
					   FormsAuthentication.FormsCookiePath
				    );

				string encryptedTicket = FormsAuthentication.Encrypt(ticket);
				Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
				Session["rolePermission"] = resRolePermission = SecRoleModuleManager.getRolePermission(user.SecRole.RoleId).ToList();

				var url = FormsAuthentication.DefaultUrl;
				Response.Redirect(url);
			}
		}

		protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			SortDirection sortDirection = SortDirection.Ascending;

			

			string lastSortExpression = ViewState["lastSortExpression"] == null ? "LastName" : ViewState["lastSortExpression"].ToString();
			
			bool descending = ViewState["lastSortDirection"] == null ? false : (bool)ViewState["lastSortDirection"];

			
			sortDirection = descending ? SortDirection.Descending : SortDirection.Ascending;

			
			Expression<Func<CRM.Data.Entities.SecUser, bool>> predicate = buildPredicate();

			IQueryable<CRM.Data.Entities.SecUser> secUsers = SecUserManager.GetUsers(predicate);
			
			gvUsers.PageIndex = e.NewPageIndex;

            gvUsers.DataSource = secUsers.ToList();//orderByNested(lastSortExpression, sortDirection);
			gvUsers.DataBind();

			
			
		}

		protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e) {
			int userID = 0;
			Image userPhoto = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				userID = (int)gvUsers.DataKeys[e.Row.RowIndex].Value;

				userPhoto = e.Row.FindControl("userPhoto") as Image;

				
				userPhoto.ImageUrl = Core.Common.getUserPhotoURL(userID);
				
			}
		}
	}
}
