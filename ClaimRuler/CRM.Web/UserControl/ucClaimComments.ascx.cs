using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;
using System.Configuration;

namespace CRM.Web.UserControl {
	public partial class ucClaimComments : System.Web.UI.UserControl {
		protected CRM.Web.Protected.ClaimRuler masterPage = null;

		public int claimID {
			get {
				return Session["ClaimID"] != null ? Convert.ToInt32(Session["ClaimID"]) : 0;
			}
			set {
				Session["ClaimID"] = value;
			}
		}
		public int policyID {
			get {
				return Session["policyID"] != null ? Convert.ToInt32(Session["policyID"]) : 0;
			}
		}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            dataBind(claimID);
        }

		protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                dataBind(claimID);
                bindActivity();
            }
		}
		protected void Page_Init(object sender, EventArgs e) {
			masterPage = (CRM.Web.Protected.ClaimRuler)this.Page.Master.Master;

			//lbtnNewComment.Visible = Core.PermissionHelper.checkAddPermission("UsersLeads.aspx");
		}
        private void bindActivity()
        {
            List<Activity> myActivity = ActivityManager.GetAll();

            CollectionManager.FillCollection(ddlActivity, "ActivityID", "Activity1", myActivity);

            //Core.CollectionManager.FillCollection(ddlActivity, "ServiceTypeID", "ServiceDescription", invoiceServiceTypes);
        }

		private void bindHistoricalComments() {
			int leadID = Core.SessionHelper.getLeadId();			
			
			Data.Entities.LeadPolicy policy = LeadPolicyManager.GetByID(this.policyID);

			if (policy != null && policy.PolicyType != null) {
				List<LeadComment> historicalComments = LeadCommentManager.getLeadCommentByLeadID(leadID, (int)policy.PolicyType);

				gvHistoricalComments.DataSource = historicalComments;
				gvHistoricalComments.DataBind();
			}
		}

		public void dataBind(int claimID) {
			gvComments.DataSource = ClaimCommentManager.GetAll(claimID);
			gvComments.DataBind();

			bindHistoricalComments();
		}

		protected void gvComments_RowCommand(object sender, GridViewCommandEventArgs e) {
			int commentID = Convert.ToInt32(e.CommandArgument);
			ClaimComment comment = null;

			if (e.CommandName == "DoEdit") {


				comment = ClaimCommentManager.Get(commentID);
				if (comment != null) {
					showCommentEditPanel();

					ViewState["CommentID"] = commentID.ToString();
                    ddlActivity.SelectedItem.Text = comment.ActivityType;
                    if (comment.StartDate!=null)
                    {
                    txtStartDate.Text = comment.StartDate.ToString();
                    }
                    if (comment.EndDate != null)
                    {
                        txtEndDate.Text = comment.EndDate.ToString();
                    }

					txtComment.Text = comment.CommentText;
				}
			}
            else if (e.CommandName == "DoDelete")
            {   
                    ClaimCommentManager.Delete(commentID);
                    dataBind(this.claimID);
            }
		}

		protected void gvComments_RowDataBound(object sender, GridViewRowEventArgs e) {

		}

		protected void btnCommentSave_Click(object sender, EventArgs e) {
			int commentID = Convert.ToInt32(ViewState["CommentID"]);

			ClaimComment comment = null;
			lblMessage.Text = string.Empty;

			if (commentID == 0) {
				comment = new ClaimComment();
				comment.ClaimID = this.claimID;
				comment.IsActive = true;
				comment.UserId = Core.SessionHelper.getUserId();
				comment.CommentDate = DateTime.Now;
			}
			else
				comment = ClaimCommentManager.Get(commentID);
            string serviceUrl = ConfigurationManager.AppSettings["importServiceUrl"];
            serviceUrl = serviceUrl + "/" + "ExportNotes?commentId=" + commentID;
            Common.SendRequest(serviceUrl);
			if (comment != null && comment.ClaimID > 0) {
				comment.CommentText = txtComment.Text.Trim();
                comment.ActivityType = ddlActivity.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                comment.StartDate =Convert.ToDateTime(txtStartDate.Text);
                }
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                {
                    comment.EndDate = Convert.ToDateTime(txtEndDate.Text);
                }

				try {
					ClaimCommentManager.Save(comment);

					showCommentGridPanel();

					dataBind(this.claimID);
				}
				catch (Exception ex) {
					lblMessage.Text = "Comment not saved!";
					lblMessage.CssClass = "error";
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void btnCommentCancel_Click(object sender, EventArgs e) {
			showCommentGridPanel();
           
		}

		protected void lbtnNewComment_Click(object sender, EventArgs e) {
			txtComment.Text = string.Empty;

			ViewState["CommentID"] = "0";

			showCommentEditPanel();
		}

		private void showCommentEditPanel() {
			pnlEdit.Visible = true;
			pnlGridPanel.Visible = true;
		}

		private void showCommentGridPanel() {
			pnlEdit.Visible = false;
			pnlGridPanel.Visible = true;
		}

		protected void gvHistoricalComments_PageIndexChanging(object sender, GridViewPageEventArgs e) {

			gvHistoricalComments.PageIndex = e.NewPageIndex;

			bindHistoricalComments();

		}

		protected void gvComments_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvComments.PageIndex = e.NewPageIndex;

			dataBind(this.claimID);
		}
	}
}