using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

namespace CRM.Web.Content {
	public partial class PrintLetterOfRep : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			generateRepLetter();
		}

		protected void generateRepLetter() {
			int LeadId = Convert.ToInt32(Session["LeadIds"]);

			if (LeadId > 0) {
				var _leads = LeadsManager.GetByLeadId(LeadId);

				string InsurancecompanyName = _leads.InsuranceCompanyName == null ? "" : _leads.InsuranceCompanyName.ToString();
				string InsuranceContactName = _leads.InsuranceContactName == null ? "" : _leads.InsuranceContactName.ToString();
				string InsuranceAddress = _leads.InsuranceAddress == null ? "" : _leads.InsuranceAddress.ToString();
				string iCity = _leads.CityMaster_1 == null ? "" : _leads.CityMaster_1.CityName;

				string iState = "";
				if (_leads.StateMaster1 != null) {
					iState = _leads.StateMaster1.StateCode == null ? "" : _leads.StateMaster1.StateCode.ToString();
				}
				string zip = _leads.Zip == null ? "" : _leads.Zip.ToString();
				string izip = _leads.InsuranceZipCode == null ? "" : _leads.InsuranceZipCode;
				string insfulladdress = iState + " " + zip;

				string _firstName = _leads.ClaimantFirstName == null ? "" : _leads.ClaimantFirstName.ToString();
				string _lastName = _leads.ClaimantLastName == null ? "" : _leads.ClaimantLastName.ToString();
				string Claimant_Name = _firstName + ' ' + _lastName;
				string BusinessName = _leads.BusinessName == null ? "" : _leads.BusinessName.ToString();
				string ClaimantAddress = _leads.LossAddress == null ? "" : _leads.LossAddress.ToString();

				string City = _leads.CityMaster == null ? "" : _leads.CityMaster.CityName;
				string State = "";
				if (_leads.StateMaster != null) {
					State = _leads.StateMaster.StateCode == null ? "" : _leads.StateMaster.StateCode;
				}

				string fulladdress = State + " " + izip;

				string policy = _leads.InsurancePolicyNumber == null ? "" : _leads.InsurancePolicyNumber.ToString();



				StringBuilder sb = new StringBuilder();

				string url = string.Empty;
				url = "http://demo.claimruler.com/images/claim_ruler_logo.jpg";
				//url = "http://csg.sandyclaims.org/images/lg.jpg";
				//url = Request.PhysicalApplicationPath + "Images/lg.jpg";



				//string url = string.Empty;
				//string domain1;
				//Uri urls = HttpContext.Current.Request.Url;
				//domain1 = urls.AbsoluteUri.Replace(urls.PathAndQuery, string.Empty);
				//url = domain1 + "/Images/";

				sb.Append("<html>");
				sb.Append("<head>");
				//sb.Append("<style type='text/css'>");
				//sb.Append("body {margin:5%;width:90%;}");
				//sb.Append("</style>");
				sb.Append("</head>");
				sb.Append("<body  >");
				sb.Append("<form>");
				sb.Append("<div align='center'>");
				//sb.Append("<p style='width: 20px; height: 20px;'><img src='" + url + "lg.jpg' style='height: 20px; width: 20px' /></p>");
				sb.Append("<p style='width: 20px; height: 20px;'><img src='" + url + "' style='height: 20px; width: 20px' /></p>");
				sb.Append("<p style='font-family: Times New Roman; font-size: 14px;'>");
				sb.Append("401 East Las Olas Blvd #130-356<br />");
				sb.Append(" Fort Lauderdale, FL 33301");
				sb.Append("</p>");
				sb.Append("</div>");
				sb.Append("<div style='height: 25px'>");
				sb.Append(" &nbsp;</div>");
				sb.Append("<div style='text-align: center;'>");
				sb.Append("<p style='font-family: Times New Roman; font-size: 16px; font-weight: bold;'>");
				sb.Append("Notice of Representation and Lien on Claim</p>");
				sb.Append("</div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("" + DateTime.Now.Date.ToString("MMMM dd, yyyy") + "<br />");
				sb.Append("<br />");
				sb.Append("" + InsurancecompanyName + "<br />");
				sb.Append("" + InsuranceAddress + "<br />");
				sb.Append("" + City + ", " + fulladdress + "<br />");
				//sb.Append(""+insfulladdress+"<br />");
				//sb.Append("" + fulladdress + "<br />");

				// 2013-03-11 tortega
				sb.Append("<p>Insurance Company Contact:<br/>");
				sb.Append(_leads.InsuranceContactName + "<br/>");
				sb.Append(_leads.InsuranceContactPhone + "<br/>");
				sb.Append(_leads.InsuranceContactEmail + "<br/>");
				sb.Append("</p>");

				sb.Append("</div>");
				sb.Append("<div style='height: 30px;'>");
				sb.Append("&nbsp;</div>");
				sb.Append("<div style='font-family: Times New Roman; margin-left: 35px; font-size: 16px;'>");
				sb.Append("<label style='font-weight: bold;'>");
				sb.Append("Re:&nbsp;&nbsp;</label>" + Claimant_Name + "<br/>");
				sb.Append(ClaimantAddress + "<br />");

				if (!string.IsNullOrEmpty(_leads.LossAddress2))
					sb.Append(_leads.LossAddress2 + "<br/>");

				sb.Append(iCity + ", " + insfulladdress + "<br />");
				//sb.Append("" + fulladdress + "");
				//sb.Append("" + insfulladdress + "");

				sb.Append("</div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("<br />");
				sb.Append("<label style='font-weight: bold;'>");
				sb.Append("Insurance Co:&nbsp;&nbsp;</label>" + InsurancecompanyName);
				sb.Append("<br />");
				sb.Append("<label style='font-weight: bold;'>Policy #:&nbsp;&nbsp;</label>" + policy + "");
				sb.Append("<br />");
				sb.Append("<label style='font-weight: bold; margin-left:187px;'>Claim #:&nbsp;&nbsp;</label>" + _leads.ClaimsNumber);
				sb.Append("</div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("<p>");
				sb.Append("Please be advised that we represent the above named client with regard to damages and claim<br />");
				sb.Append("arising from Superstorm Sandy.  Attached is a copy of our Contract For Services.");
				sb.Append("</p>");

				// 2013-03-11 tortega
				sb.Append("<p>");
				sb.Append("Please be advised that Claims Strategies Group has a lien to the extent of its fees on all<br/>");
				sb.Append("proceeds and agreement with the client that it be named as a party on all payments.<br/>");
				sb.Append("The correct title of the company to be named is: <b>Claim Ruler Example Public Adjsuter</b>");
				sb.Append("</p>");

				sb.Append("<p>");
				sb.Append("At this time we have not completed our review of or estimate of the client’s claim.<br />");
				sb.Append("We understand that your adjuster has attended the property.  Please provide the following<br />");
				sb.Append("information at the earliest time:");
				sb.Append("</p>");
				sb.Append("<p style='margin-left:20px;'>");
				sb.Append("1&nbsp;&nbsp;&nbsp;A complete copy of the policy of insurance.<br />");
				sb.Append("2&nbsp;&nbsp;&nbsp;A complete copy of the application for insurance.<br />");
				sb.Append("3&nbsp;&nbsp;&nbsp;A complete copy of any estimates completed and photographs taken.<br />");
				sb.Append("4&nbsp;&nbsp;&nbsp;The name and contact information for your adjuster who will be appointed for any<br />");
				sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;re-inspection necessary or the handling adjuster so we may correspond further.");

				sb.Append("</p>");
				sb.Append("<p>");
				sb.Append("If you have any questions, please contact us:<br />");
				sb.Append("<br />");
				sb.Append("Thank you,");

				sb.Append("</p>");
				sb.Append("</div>");
				sb.Append("<div style='height:25px;'></div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("Jane Doe<br />");
				sb.Append("Claims Processor<br />");
				sb.Append("Office: +1 (732) 555-1212<br />");
				sb.Append("Fax +1 (866) 555-1212<br />");
				sb.Append("E-mail:<a href='#' >processor@publicadjusterexample.com</a><br />");
				sb.Append("Web: <a href='#' > http://www.examplepublicadjuster.com</a><br />");
				sb.Append("</div>");
				sb.Append("</form>");
				sb.Append("</body>");
				sb.Append("</html>");




				Response.AppendHeader("Content-Type", "application/msword");
				Response.AppendHeader("Content-disposition", "attachment; filename=Rep Letter .doc");
				Response.Write(sb);
			}

		}

	}
}