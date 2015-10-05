using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Repository;

namespace CRM.Web.Protected.Admin {
	public partial class EarningCodeList : System.Web.UI.Page {
		int clientID = 0;
		
		protected void Page_Load(object sender, EventArgs e) {
			clientID = SessionHelper.getClientId();
			
			// check user permission
			Master.checkPermission();

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		private void bindData() {
			List<EarningCode> earningCodes = null;

			using (EarningCodeManager repository = new EarningCodeManager()) {
				earningCodes = repository.GetAll(this.clientID);			
			}

			gvEarningCode.DataSource = earningCodes;
			gvEarningCode.DataBind();
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showGridPanel();

			bindData();
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			EarningCode earningCode = null;
			int id = 0;

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			id = Convert.ToInt32(ViewState["EarningCodeID"]);

			if (id == 0) {
				earningCode = new EarningCode();
				earningCode.ClientID = this.clientID;
				earningCode.IsActive = true;
			}
			else {
				using (EarningCodeManager repository = new EarningCodeManager()) {
					earningCode = repository.Get(id);
				}
			}

			if (earningCode != null) {
				earningCode.Code = this.txtEarningCode.Text.Trim();
				earningCode.CodeDescription = this.txtEarningDescription.Text.Trim();

				try {
					using (EarningCodeManager repository = new EarningCodeManager()) {
						earningCode = repository.Save(earningCode);
					}

					showGridPanel();

					// refresh grid
					bindData();

				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					showErrorMessage();
				}
			}
		}
		protected void gvEarningCode_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvEarningCode.PageIndex = e.NewPageIndex;

			bindData();
		}

		protected void gvEarningCode_Sorting(object sender, GridViewSortEventArgs e) {

		}

		protected void gvEarningCode_RowCommand(object sender, GridViewCommandEventArgs e) {
			int id = 0;
			EarningCode earningCode = null;

			if (e.CommandName == "DoEdit") {
				id = Convert.ToInt32(e.CommandArgument);

				using (EarningCodeManager repository = new EarningCodeManager()) {
					earningCode = repository.Get(id);
				}
				if (earningCode != null) {
					showEditPanel();

					this.txtEarningCode.Text = earningCode.Code; ;

					this.txtEarningDescription.Text = earningCode.CodeDescription;

					ViewState["EarningCodeID"] = e.CommandArgument.ToString();
				}
			}
			else if (e.CommandName == "DoDelete") {
				try {
					id = Convert.ToInt32(e.CommandArgument);

					using (EarningCodeManager repository = new EarningCodeManager()) {
						earningCode = repository.Get(id);

						if (earningCode != null) {
							earningCode.IsActive = false;

							earningCode = repository.Save(earningCode);

							// refresh grid
							bindData();
						}
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					showErrorMessage();
				}

			}
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			showEditPanel();

			// clear fields
			this.txtEarningCode.Text = string.Empty;
			this.txtEarningDescription.Text = string.Empty;

			ViewState["EarningCodeID"] = "0";
		}

		private void showEditPanel() {
			pnlGrid.Visible = false;
			pnlEdit.Visible = true;
		}

		private void showErrorMessage() {
			lblMessage.Text = "Unable to save changes.";
			lblMessage.CssClass = "error";
		}

		private void showGridPanel() {
			pnlGrid.Visible = true;
			pnlEdit.Visible = false;
		}
	}
}