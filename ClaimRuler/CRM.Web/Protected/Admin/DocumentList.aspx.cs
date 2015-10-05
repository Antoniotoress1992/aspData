using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

namespace CRM.Web.Protected.Admin {
	public partial class DocumentList : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void btnSave_click(object sender, EventArgs e) {
			DocumentListMaster document = null;
			int DocumentListId = 0;
			int PolicyTypeId = 0;
			int clientID = Core.SessionHelper.getClientId();

			Page.Validate("NewDocument");
			
			if (!Page.IsValid)
				return;

			if (Session["DocumentListId"] == null) {
				// new document master
				document = new DocumentListMaster();

				PolicyTypeId = Convert.ToInt32(ddlPolicyType.SelectedValue);

				document.clientID = clientID;

				document.DocumentName = txtDocument.Text.Trim();
				document.PolicyTypeId = PolicyTypeId;

				document.IsActive = true;
				DocumentLisMasterManager.Save(document);
			}
			else {
				DocumentListId = Convert.ToInt32(Session["DocumentListId"]);

				document = DocumentLisMasterManager.GetDocumentMaster(DocumentListId);
				if (document != null) {
					document.DocumentName = txtDocument.Text.Trim();

					DocumentLisMasterManager.Save(document);
				}
			}

			txtDocument.Text = "";

			Session.Remove("DocumentListId");

			bindDocumentList();
		}

		protected void ddlPolicyType_SelectedIndex(object sender, EventArgs e) {
			bindDocumentList();			
		}

		protected void bindDocumentList() {
			int clientID = 0;
			int policyTypeID = Convert.ToInt32(ddlPolicyType.SelectedValue);

			clientID = Core.SessionHelper.getClientId();

			if (clientID > 0 && policyTypeID > 0) {
				gvDocumentList.DataSource = DocumentLisMasterManager.GetDocumentListMaster(clientID, policyTypeID);

				gvDocumentList.DataBind();
			}
			else {
				gvDocumentList.DataSource = null;

				gvDocumentList.DataBind();
			}
		}

		protected void gvDocumentList_RowCommand(object sender, GridViewCommandEventArgs e) {
			int DocumentListId = 0;

			if (e.CommandName == "DoEdit") {
				Session["DocumentListId"] = e.CommandArgument.ToString();
				DocumentListId = Convert.ToInt32(e.CommandArgument.ToString());

				DocumentListMaster document = DocumentLisMasterManager.GetDocumentMaster(DocumentListId);

				txtDocument.Text = document.DocumentName;

			}

			if (e.CommandName == "DoDelete") {
				Session["DocumentListId"] = e.CommandArgument.ToString();
				DocumentListId = Convert.ToInt32(e.CommandArgument.ToString());

				DocumentListMaster document = DocumentLisMasterManager.GetDocumentMaster(DocumentListId);

				if (document != null) {
					document.IsActive = false;

					DocumentLisMasterManager.Save(document);
				}
			}

			bindDocumentList();
		}
	}
}