using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierDocument : System.Web.UI.UserControl {
		private int carrierID {
			get {
				return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0;
			}
		}
		protected void Page_Load(object sender, EventArgs e) {
		}

		public void bindData(int carrierID) {
			hf_carrierID.Value = carrierID.ToString();

			gvDocuments.DataSource = CarrierDocumentManager.GetAll(carrierID);
			gvDocuments.DataBind();
		}
		protected void gvDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e) {

		}

		protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e) {
			CarrierDocument document = null;
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			int documentID = Convert.ToInt32(e.CommandArgument);
			
			if (e.CommandName == "DoDelete") {
				document = CarrierDocumentManager.Get(documentID);

				if (document != null) {					
					string documentPath = string.Format("{0}/CarrierDocuments/{1}/{2}", appPath, document.CarrierID, document.DocumentName);
					
					if (File.Exists(documentPath))
						File.Delete(documentPath);

					CarrierDocumentManager.Delete(documentID);

					bindData(carrierID);
				}
			}
		}

		protected void btnNewDocument_Click(object sender, EventArgs e) {

		}
		
		protected void btnRefresh_Click(object sender, EventArgs e) {
			bindData(carrierID);
		}

		protected void gvDocuments_RowDataBound(object sender, GridViewRowEventArgs e) {
			CarrierDocument document = null;
			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();


			if (e.Row.RowType == DataControlRowType.DataRow) {
				HyperLink hlnkDocument = e.Row.FindControl("hlnkDocument") as HyperLink;
				document = e.Row.DataItem as CarrierDocument;

				if (document != null) {
					string documentPath = string.Format("{0}/CarrierDocuments/{1}/{2}", appPath, document.CarrierID, document.DocumentName);

					if (File.Exists(documentPath))
						hlnkDocument.NavigateUrl = string.Format("{0}/CarrierDocuments/{1}/{2}", appUrl, document.CarrierID, document.DocumentName);
				}
			}
		}


	}
}