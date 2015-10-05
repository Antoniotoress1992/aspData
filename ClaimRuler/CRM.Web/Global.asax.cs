
namespace CRM.Web {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.Security;
	using System.Web.SessionState;
	using CRM.Data;
	using System.Security.Principal;
    using CRM.Data.Entities;
    using System.IO.Compression;

	public class Global : System.Web.HttpApplication {

		protected void Application_Start(object sender, EventArgs e) {
			//Application["LoginTrial"] = new List<LoginTrials>();

			// for Unobtrusive Validation Mode 
			ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
					 new ScriptResourceDefinition {
						 Path = "~/js/jquery-1.8.3.js",
						 DebugPath = "~/js/jquery-1.8.3.js",
						
						 //Path = "~/js/jquery-1.7.2.min.js",
						 //DebugPath = "~/js/jquery-1.7.2.min.js",
						 //CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.min.js",
						 //CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.1.js"
					 });
		}

		protected void Session_Start(object sender, EventArgs e) {

		}

		protected void Application_BeginRequest(object sender, EventArgs e) {
           

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e) {
			if (HttpContext.Current.User != null) {
				if (HttpContext.Current.User.Identity.IsAuthenticated) {
					if (HttpContext.Current.User.Identity is FormsIdentity) {
						var id = (FormsIdentity)HttpContext.Current.User.Identity;
						var ticket = id.Ticket;

						// Get the stored user-data, in this case, our roles
						string userData = ticket.UserData;
						string[] roles = userData.Split(',');
						HttpContext.Current.User = new GenericPrincipal(id, roles);
					}
				}
			}
		}

        //protected void Application_Error(object sender, EventArgs e) {
        //    Exception exceptionError = Server.GetLastError().GetBaseException();

        //    Core.EmailHelper.emailError(exceptionError);

        //    Server.ClearError();

        //    Response.Redirect("~/ErrorPage.aspx"); 
        //}

		protected void Session_End(object sender, EventArgs e) {

		}

		protected void Application_End(object sender, EventArgs e) {
			//Clean Db Context
			DbContextHelper.CleanUp();
		}
	}
}