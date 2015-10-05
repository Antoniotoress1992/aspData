using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierComment : System.Web.UI.UserControl {
		private int carrierID {
			get {
				return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {

		}

		private void bindComments() {
			gvComments.DataSource = CarrierCommentManager.GetAll(carrierID);

			gvComments.DataBind();
		}

		public void bindData() {
			bindComments();
		}

		protected void btnNewComment_Click(object sender, EventArgs e) {
			showEditPanel();

			clearFields();
		}

		private void clearFields() {
			ViewState["CarrierCommentID"] = "0";

			txtComment.Text = "";
		}

		protected void gvComments_RowCommand(object sender, GridViewCommandEventArgs e) {
			int commentID = Convert.ToInt32(e.CommandArgument);

			CarrierComment comment = null;

			if (e.CommandName == "DoEdit") {
				ViewState["CarrierCommentID"] = e.CommandArgument.ToString();

				comment = CarrierCommentManager.Get(commentID);

				if (comment != null) {
					showEditPanel();

					txtComment.Text = comment.CommentText;
				}
				
			}
			else if (e.CommandName == "DoDelete") {
				CarrierCommentManager.Delete(commentID);

				showGridPanel();

				bindComments();
			}

		}

		protected void gvComments_RowDataBound(object sender, GridViewRowEventArgs e) {

		}

		protected void gvComments_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			this.gvComments.PageIndex = e.NewPageIndex;
		}

		private void showEditPanel() {
			pnlEdit.Visible = true;
			pnlGrid.Visible = false;			
		}

		private void showGridPanel() {
			pnlEdit.Visible = false;
			pnlGrid.Visible = true;
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			int commentID = 0;

			Page.Validate("Comment");
			if (!Page.IsValid)
				return;

			CarrierComment comment = null;

			// carrier being edited
			commentID = Convert.ToInt32(ViewState["CarrierCommentID"]);

			if (commentID == 0) {
				comment = new CarrierComment();

				comment.CarrierID = carrierID;

				comment.CommentDate = DateTime.Now;

				comment.Status = true;
			}								

			comment.CommentText = txtComment.Text.Trim();
			
			comment.UserId = Core.SessionHelper.getUserId();

			

			try {
				CarrierCommentManager.Save(comment);

				bindComments();

				showGridPanel();

				lblMessage.Text = "Comment saved successfully.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Comment not saved successfully.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}

		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showGridPanel();
		}
	}
}