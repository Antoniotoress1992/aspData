using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class ClientList : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();
			
			// make sure this is Admin
			if (!Core.PermissionHelper.isAdmin()) {
				Response.Redirect("~/AccessDenied.aspx");
			}

			if (!Page.IsPostBack) {
				loadData();
			}
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			Session["EditClientId"] = null;

			Server.Transfer("~/Protected/Admin/ClientEdit.aspx");
		}

		protected void deleteClient(int clientId) {
			Client client = ClientManager.Get(clientId);

			if (client != null) {
				client.Active = 0;

				ClientManager.Save(client);
			}
		}

		protected void gvClients_RowCommand(object sender, GridViewCommandEventArgs e) {
			int clientID = 0;
			Client client = null;

			if (e.CommandName == "DoEdit") {
				Session["EditClientId"] = e.CommandArgument.ToString();

				Server.Transfer("~/Protected/Admin/ClientEdit.aspx");
			}

			if (e.CommandName == "DoNew") {
				Session["EditClientId"] = null;

				Server.Transfer("~/Protected/Admin/ClientEdit.aspx");
			}

			if (e.CommandName == "DoDelete") {
				Session["EditClientId"] = null;
				clientID = Convert.ToInt32(e.CommandArgument.ToString());

				deleteClient(clientID);

				// reload clients
				loadData();
			}

			if (e.CommandName == "DoImpersonate") {
				Session["EditClientId"] = null;
				clientID = Convert.ToInt32(e.CommandArgument.ToString());

				client = ClientManager.Get(clientID);
				
				if (client != null) {
					impersonateClient(client);
				}

				// reload clients
				loadData();
			}
			
		}

		protected void impersonateClient(Client client) {
			List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
			
			string userData = null;
			CRM.Data.Entities.SecUser user = null;

			user = client.SecUser;

			Session["UserId"] = client.SecUser.UserId;
			Session["UserName"] = client.SecUser.UserName;
			Session["RoleId"] = client.SecUser.SecRole.RoleId.ToString();

			// 201307-29
			Session["ClientId"] = client.SecUser.ClientID;
			Session["ClientShowTask"] = (client.isShowTasks ?? true);

			userData = string.Format("{0}|{1}|{2} {3}|{4}", user.SecRole.RoleName, user.SecRole.RoleId, user.FirstName, user.LastName, user.Email);

			var ticket = new FormsAuthenticationTicket
			    (
				   1,
				   client.SecUser.UserId.ToString(),
				   DateTime.Now,
				   DateTime.Now.AddMinutes(120),
				   true,
				   userData,	//(client.SecUser.SecRole.RoleName + "|" + client.SecUser.SecRole.RoleId.ToString()),
				   FormsAuthentication.FormsCookiePath
			    );

			string encryptedTicket = FormsAuthentication.Encrypt(ticket);
			Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
			Session["rolePermission"] = resRolePermission = SecRoleModuleManager.getRolePermission(client.SecUser.SecRole.RoleId).ToList();




			var url = FormsAuthentication.DefaultUrl;
			Response.Redirect(url);
		}

		protected void gvClients_onSorting(object sender, GridViewSortEventArgs e) {
			IQueryable<Client> clients = ClientManager.GetClients();
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gvClients.DataSource = clients.orderByExtension(e.SortExpression, descending);

			gvClients.DataBind();

		}

		protected void gvClients_PageIndexChanging(object sender, GridViewPageEventArgs e) {
		}

		protected void loadData() {
			gvClients.DataSource = ClientManager.GetAll();

			gvClients.DataBind();
		}
	}
}