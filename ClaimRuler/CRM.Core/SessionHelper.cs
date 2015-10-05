using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Core {
	static public class SessionHelper {
		/// <summary>
		/// Returns current claim id being modified
		/// </summary>
		/// <returns></returns>
		static public int getClaimID() {

			return HttpContext.Current.Session["ClaimID"] != null ? Convert.ToInt32(HttpContext.Current.Session["ClaimID"]) : 0;

		}
		static public int getPolicyID() {		
				return HttpContext.Current.Session["policyID"] != null ? Convert.ToInt32(HttpContext.Current.Session["policyID"]) : 0;						
		}
		static public void setPolicyID(int? id) {
			HttpContext.Current.Session["policyID"] = id;
		}

		/// <summary>
		/// Returns Role ID from session for current logged user
		/// </summary>
		/// <returns></returns>
		static public int getUserRoleId() {
			int id = 0;
			if (HttpContext.Current.Session["RoleId"] != null)
				id = Convert.ToInt32(HttpContext.Current.Session["RoleId"].ToString());

			return id;
		}

		static public int getClientId() {
			int id = 0;
			if (HttpContext.Current.Session["ClientId"] != null)
				id = Convert.ToInt32(HttpContext.Current.Session["ClientId"].ToString());

			return id;
		}

		static public bool getClientShowTasks() {
			bool show = true;

			if (HttpContext.Current.Session["ClientShowTask"] != null)
				show = (bool)HttpContext.Current.Session["ClientShowTask"];

			return show;
		}

		static public int getLeadId() {
			int id = 0;
			if (HttpContext.Current.Session["LeadIds"] != null)
				id = Convert.ToInt32(HttpContext.Current.Session["LeadIds"].ToString());

			return id;
		}
		static public void setLeadId(int? id) {
			HttpContext.Current.Session["LeadIds"] = id;			
		}

		static public List<int> getRoleActions() {

			return HttpContext.Current.Session["roleActions"] != null ? (List<int>)HttpContext.Current.Session["roleActions"] : null;

		}

		static public int getUserId() {
			int id = 0;
			if (HttpContext.Current.Session["UserId"] != null)
				id = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());

			return id;
		}

		static public string getUserName() {

			return HttpContext.Current.Session["UserName"] == null ? "" : HttpContext.Current.Session["UserName"].ToString();

		}
		/// <summary>
		/// Returns claimant name from session
		/// </summary>
		/// <returns></returns>
		static public string getClaimantName() {
			string firstName = null;
			string lastName = null;

			firstName = HttpContext.Current.Session["ClaimantFirstName"] == null ? "" : HttpContext.Current.Session["ClaimantFirstName"].ToString();
			lastName = HttpContext.Current.Session["ClaimantLastName"] == null ? "" : HttpContext.Current.Session["ClaimantLastName"].ToString();

			return firstName + " " + lastName;
		}
	}
}