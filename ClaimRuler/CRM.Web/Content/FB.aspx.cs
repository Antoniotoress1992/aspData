using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using Facebook;

namespace CRM.Web.Content {
	public partial class FB : System.Web.UI.Page {
	
		// source
		// http://blue-and-orange.net/articles/facebook/integrating-facebook-login-button-in-aspnet-mvc-4-application/
		//http://computerbeacon.net/library/facebookgraphtoolkit/getstarted

		protected void Page_Load(object sender, EventArgs e) {
			string accessToken = null;

			
			if (HttpContext.Current.Session["accessToken"] != null) {
				//token.Text = HttpContext.Current.Session["accessToken"].ToString();

				accessToken = HttpContext.Current.Session["accessToken"].ToString();

				// get user info
				//FacebookClient fbClient = new FacebookClient(accessToken);
				//dynamic fbresult = fbClient.Get("me?fields=id,email,first_name,last_name,gender,locale,link,username,timezone,location,picture");

				// get friend list
				//dynamic resultFriends = fbClient.Get("me/friends");
				
			}
		}

		[System.Web.Services.WebMethod]
		public static void facebooklogin(string accessToken) {
			HttpContext.Current.Session["accessToken"] = accessToken;			
		}
	}
}