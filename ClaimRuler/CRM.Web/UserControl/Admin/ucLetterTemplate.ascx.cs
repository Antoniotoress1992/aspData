using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;

using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucLetterTemplate : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!Page.IsPostBack) {
				loadData();
			}

		}

		protected void btnCancel_click(object sender, EventArgs e) {
			pnlGrid.Visible = true;
			pnlUpload.Visible = false;

			loadData();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			pnlGrid.Visible = false;
			pnlUpload.Visible = true;

			txtDescription.Text = "";
			hfID.Value = "0";
			lblFilename.Text = "";

		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			int clientID = SessionHelper.getClientId();
			ClientLetterTemplate template = null;
			int templateID = 0;
			int hidden_templateID = 0;

			Page.Validate("newTemplate");
			if (!Page.IsValid)
				return;

			// no client available
			if (clientID == 0)
				return;

			// this is non zero when in edit mode
			hidden_templateID = Convert.ToInt32(hfID.Value);

			if (fileUpload.HasFile && hidden_templateID == 0) {
				// new template being uploaded
				template = new ClientLetterTemplate();
				template.ClientID = clientID;
				template.Description = txtDescription.Text.Trim();
				template.Path = fileUpload.FileName;

				// save template
				templateID = LetterTemplateManager.Save(template);

				if (templateID > 0) {
					// upload letter template
					uploadTemplate(clientID, templateID);					
				}
			}
			else if (!fileUpload.HasFile && hidden_templateID > 0) {
				// update template description only, no upload file specified				
				template = LetterTemplateManager.Get(hidden_templateID);
				if (template != null) {
					template.Description = txtDescription.Text.Trim();
					LetterTemplateManager.Save(template);
				}
			}
			else if (fileUpload.HasFile && hidden_templateID > 0) {
				// update template description and upload file 
				template = LetterTemplateManager.Get(hidden_templateID);
				if (template != null) {
					template.Description = txtDescription.Text.Trim();
					template.Path = fileUpload.FileName;
					templateID = LetterTemplateManager.Save(template);

					// upload letter template
					if (templateID > 0)
						uploadTemplate(clientID, templateID);

				}
			}

			pnlGrid.Visible = true;
			pnlUpload.Visible = false;

			hfID.Value = "0";

			// refresh grdiview
			loadData();
		}

		protected void uploadTemplate(int clientID, int templateID) {
			string clientTemplatePath = null;
			string fileExtension = null;
			string filePath = null;

			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			clientTemplatePath = appPath + "/ClientLetterTemplates/" + clientID.ToString();

			if (!Directory.Exists(clientTemplatePath)) {
				Directory.CreateDirectory(clientTemplatePath);
			}

			fileExtension = System.IO.Path.GetExtension(fileUpload.FileName);

			filePath = clientTemplatePath + "/" + templateID + fileExtension;

			fileUpload.SaveAs(filePath);
		}

		protected void gvLetterTemplate_RowCommand(object sender, GridViewCommandEventArgs e) {
			int clientID = SessionHelper.getClientId();
			string clientTemplatePath = null;
			string filePath = null;
			int templateID = 0;
			ClientLetterTemplate template = null;

			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			if (e.CommandName == "DoEdit") {
				pnlUpload.Visible = true;
				pnlGrid.Visible = false;

				templateID = Convert.ToInt32(e.CommandArgument);
				template = LetterTemplateManager.Get(templateID);

				if (template != null) {
					hfID.Value = e.CommandArgument.ToString();

					txtDescription.Text = template.Description;

					lblFilename.Text = template.Path;
				}
			}
			else if (e.CommandName == "DoDelete") {
				templateID = Convert.ToInt32(e.CommandArgument);

				try {
					template = LetterTemplateManager.Get(templateID);

					LetterTemplateManager.Delete(templateID);

					if (clientID > 0 && template != null) {
						clientTemplatePath = appPath + "/ClientLetterTemplates/" + clientID.ToString();

						filePath = clientTemplatePath + "/" + templateID + "." + Path.GetExtension(template.Path);

						if (File.Exists(filePath))
							File.Delete(filePath);
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}

			// refresh grdiview
			loadData();
		}

		protected void loadData() {
			List<ClientLetterTemplate> templates = null;

			int clientID = Core.SessionHelper.getClientId();
			//int roleID = SessionHelper.getUserRoleId();

			//if (roleID == (int)UserRole.Administrator) {
			//	templates = LetterTemplateManager.GetAll();
			//}
			//else if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && clientID > 0) {
			//	templates = LetterTemplateManager.GetAll(clientID);
			//}

			templates = LetterTemplateManager.GetAll(clientID);

			gvLetterTemplate.DataSource = templates;
			gvLetterTemplate.DataBind();
		}
	}
}