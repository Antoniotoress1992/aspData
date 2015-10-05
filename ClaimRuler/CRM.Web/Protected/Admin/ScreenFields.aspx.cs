using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class ScreenFields : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			int formID = 0;

			Master.checkPermission();

			if (!Page.IsPostBack) {
				if (Request.Params["id"] != null) {

					formID = Convert.ToInt32(Request.Params["id"].ToString());
					pnlSelectScreen.Visible = false;
					bindFields(formID);

				}
				else {
					bindData();
				}
			}
		}

		private void bindData() {
			List<DataForm> dataForms = null;

			using (DataFormManager repository = new DataFormManager()) {
				dataForms = repository.GetScreenNames();
			}

			CollectionManager.FillCollection(ddlScreens, "FormID", "FormName", dataForms);
		}

		protected void ddlScreens_SelectedIndexChanged(object sender, EventArgs e) {
			int formID = 0;

			formID = Convert.ToInt32(ddlScreens.SelectedValue);

			bindFields(formID);
		}

		private void bindFields(int formID) {
			int clientID = SessionHelper.getClientId();
			List<FormFieldView> formFields = null;

			if (formID > 0) {
				using (DataFormManager repository = new DataFormManager()) {
					formFields = repository.GetFormFields(formID, clientID);

					if (formFields == null || formFields.Count == 0) {
						// client has not define a field template. Show all fields
						formFields = repository.GetFormFields(formID);
					}
				}				
			}

			gvFields.DataSource = formFields;
			gvFields.DataBind();
		}


		public void btnSaveScreenFields_Click(object sender, EventArgs e) {
			int templateID = 0;
			int formID = 0;
			int fieldID = 0;
			DataFormFieldTemplate fieldTemplate = null;

			int clientID = SessionHelper.getClientId();
			
			using (DataFormManager repository = new DataFormManager()) {

				using (TransactionScope scope = new TransactionScope()) {

					try {
						foreach (GridViewRow row in gvFields.Rows) {
							CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;

							HiddenField hf_templateID = row.FindControl("hf_templateID") as HiddenField;
							HiddenField hf_formID = row.FindControl("hf_formID") as HiddenField;
							HiddenField hf_fieldID = row.FindControl("hf_fieldID") as HiddenField;

							templateID = Convert.ToInt32(hf_templateID.Value);

							formID = Convert.ToInt32(hf_formID.Value);	//(int)gvFields.DataKeys[row.RowIndex].Values[1];

							fieldID = Convert.ToInt32(hf_fieldID.Value);	//(int)gvFields.DataKeys[row.RowIndex].Values[2];

							if (templateID > 0)
								fieldTemplate = repository.GetTemplate(templateID);
							else
								fieldTemplate = new DataFormFieldTemplate();

							fieldTemplate.ClientID = clientID;
							fieldTemplate.TemplateID = templateID;
							fieldTemplate.FormID = formID;
							fieldTemplate.FieldID = fieldID;
							fieldTemplate.IsSelected = cbxSelected.Checked;

							repository.SaveTemplateField(fieldTemplate);
						}
						scope.Complete();

						lblMessage.Text = "Fields Template saved successfully.";
						lblMessage.CssClass = "ok";
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
						lblMessage.Text = "Unable to save fields template.";
						lblMessage.CssClass = "error";
					}	// try/catch

				}	// using (TransactionScope
			}
		}
	}
};