namespace CRM.Web.UserControl.Admin {
	#region Namespace
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Core;
	using CRM.Data.Account;
	using CRM.Data;
	using System.Transactions;
	using LinqKit;
	using System.IO;
	using System.Data;
	using System.Text;
	using System.Web.UI.HtmlControls;
	#endregion

	public partial class ucLeadComments : System.Web.UI.UserControl {
		string ErrorMessage = string.Empty;
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (Session["LeadIds"] != null) {

					hfLeadsId.Value = Session["LeadIds"].ToString();
					//FillDocument(Convert.ToInt32(Session["LeadIds"]));
					//FillDocumentList(Convert.ToInt32(Session["LeadIds"]));
					FillComments(Convert.ToInt32(Session["LeadIds"]));
					FillLeadInfo(Convert.ToInt32(Session["LeadIds"]));
					Session["LeadId"] = Session["LeadIds"].ToString();

					if (Session["View"] != null) {
						string view = Session["View"].ToString();
						if (view != null && view == "1") {
							dvEdit.Visible = false;
							//lblheading.Text = "Documents";
							//btnPhotos.Text = "Photos";


							//chkInsurancePolicy.Enabled = false;
							//chkSignedRetainer.Enabled = false;
							//chkDamageReport.Enabled = false;
							//chkDamagePhoto.Enabled = false;
							//chkCertifiedInsurancePolicy.Enabled = false;
							//chkOwnerContract.Enabled = false;
							//chkContentList.Enabled = false;
							//chkDamageEstimate.Enabled = false;


							//btnSave.Visible = false;
							//gvDocuments.Columns[2].Visible = false;
							hfView.Value = "1";
							btnNewComment.Visible = false;

						}
					}
				}
				Lead objLeadSubmitt = LeadsManager.GetByLeadId(Convert.ToInt32(hfLeadsId.Value));
				if (objLeadSubmitt != null && objLeadSubmitt.IsSubmitted == true)
					btnGenerateReport.Visible = true;
				else
					btnGenerateReport.Visible = false;

				if (Session["LeadIds"] != null) {


					hfLeadsId.Value = Session["LeadIds"].ToString();

					//FillDocument(Convert.ToInt32(Session["LeadIds"]));
					FillComments(Convert.ToInt32(Session["LeadIds"]));


					if (Session["Submitted"] != null) {
						string submitted = Session["submitted"].ToString();
						if (submitted != null && submitted == "1")
							dvEdit.Visible = false;
					}
				}
			}
		}

		private void FillLeadInfo(int LeadId) {
			Lead objLead = LeadsManager.GetByLeadId(LeadId);

			if (objLead.LeadId > 0 && objLead.LeadId != null) {
				string LastName = objLead.ClaimantLastName != null ? objLead.ClaimantLastName : "";
				lblClaimantName.Text = objLead.ClaimantFirstName == null ? "" : objLead.ClaimantFirstName.ToString() + " " + LastName;
				lblBusinessName.Text = objLead.BusinessName == null ? "" : objLead.BusinessName.ToString();
				//
				string ClaimantAddress = objLead.LossAddress == null ? "" : objLead.LossAddress.ToString();
				string City = objLead.CityMaster == null ? "" : objLead.CityMaster.CityName;
				string State = string.Empty;
				if (objLead.StateMaster != null) {
					State = objLead.StateMaster.StateCode == null ? "" : objLead.StateMaster.StateCode.ToString();
				}
				string zip = objLead.Zip == null ? "" : objLead.Zip.ToString();
				string fulladdress = ClaimantAddress + ", " + City + ", " + State + ", " + zip;
				lblClaimantAdd.Text = fulladdress;
			}
		}

		private void FillComments(int LeadId) {

			//List<LeadsDocument> lstComment = LeadsUploadManager.getLeadsDocumentByLeadID(LeadId);
			List<LeadComment> lstComment = LeadCommentManager.getLeadCommentByLeadID(LeadId);
			if (lstComment != null && lstComment.Count > 0) {
				gvComments.DataSource = lstComment;
				gvComments.DataBind();
			}
			else {
				gvComments.DataSource = null;
				gvComments.DataBind();
			}


		}

		protected void btnNewComment_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;

			dvEdit.Visible = true;
			btnNewComment.Visible = false;
			btnCancelNew.Visible = false;

		}

		protected void btnSaveContinue_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			int commentID = 0;

			try {
				if (int.TryParse(hfCommentId.Value, out commentID) && commentID > 0) {
					LeadComment comment = LeadCommentManager.GetLeadCommentById(commentID);
					comment.CommentText = txtComment.Text.Trim();
					comment.UserId = Convert.ToInt32(Session["UserId"]);

					LeadComment objld = LeadCommentManager.Save(comment);
					int LeadID = Convert.ToInt32(hfLeadsId.Value);

					lblSave.Text = "Comment Saved Successfully";
					lblSave.Visible = true;
					txtComment.Text = string.Empty;
					FillComments(LeadID);
					dvEdit.Visible = false;
					btnNewComment.Visible = true;
					btnCancelNew.Visible = true;

				}
				else {


					LeadComment objLeadComment = new LeadComment();
					int LeadID = 0;
					if (hfLeadsId.Value != null && Convert.ToInt32(hfLeadsId.Value) > 0) {
						LeadID = Convert.ToInt32(hfLeadsId.Value);
					}
					else {
						lblError.Text = string.Empty;
						lblError.Text = "There is a problem to save.";
						lblError.Visible = true;
						return;
					}
					objLeadComment.LeadId = LeadID;
					objLeadComment.UserId = Convert.ToInt32(Session["UserId"]);
					objLeadComment.CommentText = txtComment.Text;
					objLeadComment.Status = 1;
					LeadComment objld = LeadCommentManager.Save(objLeadComment);
					if (objLeadComment.CommentId > 0) {
						lblSave.Text = string.Empty;
						lblSave.Text = "Comment Saved Successfully";
						lblSave.Visible = true;
						txtComment.Text = string.Empty;
						FillComments(LeadID);
						dvEdit.Visible = false;
						btnNewComment.Visible = true;
						btnCancelNew.Visible = true;
					}
				}
			}
			catch (Exception ex) {
				lblError.Text = string.Empty;
				lblError.Text = "There is a problem to save.";
				lblError.Visible = true;
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			txtComment.Text = string.Empty;
			dvEdit.Visible = false;
			dvEdit.Visible = false;
			btnNewComment.Visible = true;
			btnCancelNew.Visible = true;
		}

		protected void btnDocument_Click(object sender, EventArgs e) {
			if (hfView.Value == "1")
				Session["View"] = 1;
			var url = "~/protected/admin/LeadsUpload.aspx";
			Response.Redirect(url);
		}

		protected void gvComments_RowCommand(object sender, GridViewCommandEventArgs e) {
			int commentID = 0;
			int leadID = 0;

			if (e.CommandName == "DoDelete") {
				commentID = Convert.ToInt32(e.CommandArgument.ToString());

				try {
					LeadCommentManager.Delete(commentID);

					// refresh comments list
					int.TryParse(Session["LeadIds"].ToString(), out leadID);

					FillComments(leadID);
				}
				catch (Exception ex) {
				}
			}

			if (e.CommandName == "DoEdit") {
				dvEdit.Visible = true;

				commentID = Convert.ToInt32(e.CommandArgument.ToString());

				LeadComment comment = LeadCommentManager.GetLeadCommentById(commentID);

				txtComment.Text = comment.CommentText;

				hfCommentId.Value = commentID.ToString();

			}
		}

		protected void gvComments_RowBound(object sender, GridViewRowEventArgs e) {
			int roleID = 0;

			if (Session["RoleId"] != null && int.TryParse(Session["RoleId"].ToString(), out roleID)) {

				if (e.Row.RowType == DataControlRowType.DataRow) {
					ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;

					// make delete available for administrator
					if (btnDelete != null)
						btnDelete.Visible = roleID == (int)UserRole.Administrator;

				}
			}
		}

		protected void btnCancelNew_Click(object sender, EventArgs e) {
			//if (hfView.Value == "1")
			//    Session["View"] = 1;
			//var url = "~/protected/admin/NewLead.aspx";
			//Response.Redirect(url);
			string view = "";
			if (hfView.Value == "1")
				view = "1";
			Session["LeadIds"] = null;
			Session["View"] = null;
			this.Context.Items.Add("selectedleadid", hfLeadsId.Value);
			this.Context.Items.Add("view", view);
			Server.Transfer("~/protected/admin/newlead.aspx");
		}

		protected void gvComments_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvComments.PageIndex = e.NewPageIndex;
			FillComments(Convert.ToInt32(Session["LeadIds"]));
		}

		protected void btnGenerateReport_Click(object sender, EventArgs e) {
			if (hfLeadsId.Value != null && Convert.ToInt32(hfLeadsId.Value) > 0) {
				try {

					int LeadId = Convert.ToInt32(hfLeadsId.Value);
					string filename1 = CreatePDF.CreateAndGetPDF(LeadId, Request.PhysicalApplicationPath + "PDF\\", out ErrorMessage);


					LeadReportGenerateLog objLeadReportGenerateLog = new LeadReportGenerateLog();
					objLeadReportGenerateLog.LeadId = Convert.ToInt32(hfLeadsId.Value);
					objLeadReportGenerateLog.GenerateDate = DateTime.Now;
					objLeadReportGenerateLog.Generatedby = Convert.ToInt32(Session["UserId"]);
					LeadReportLogManager.Save(objLeadReportGenerateLog);


					OpenNewWindow(LeadId);
				}
				catch (Exception ex) {
					lblError.Text = "Report Not Generated.";
					lblError.Visible = true;
				}
			}
		}

		public void hideActionButtons() {
			this.divActionButtons.Visible = false;
		}

		public void hideCancelButton() {
			this.btnCancelNew.Visible = false;
		}

		public void hideHeaderSection() {
			this.divHeader.Visible = false;
		}


		public void OpenNewWindow(int LeadId) {
			string url = Request.PhysicalApplicationPath + "PDF/" + LeadId + ".pdf";
			if (File.Exists(url)) {

				string FileName = LeadId + ".pdf";
				Response.ContentType = "Application/pdf";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + " ");
				Response.TransmitFile(url);
				Response.End();
			}
		}

		protected void btnUploadPhoto_Click(object sender, EventArgs e) {
			if (hfView.Value == "1")
				Session["View"] = 1;
			Session["pageIndex"] = 0;
			var url = "~/protected/admin/LeadsImagesUpload.aspx";
			Response.Redirect(url);
		}
	}
}