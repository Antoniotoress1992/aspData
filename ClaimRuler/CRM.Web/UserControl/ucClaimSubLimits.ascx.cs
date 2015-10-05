using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Web.Utilities;

using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucClaimSubLimits : System.Web.UI.UserControl {
		public int policyID {
			get {
				return Session["policyID"] != null ? Convert.ToInt32(Session["policyID"]) : 0;
			}
		}

		List<PolicySubLimit> policySublimits = null;

		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(int claimID) {
			List<ClaimSubLimit> claimSublimits = null;
			ClaimSubLimit subLimitBlank = null;
			string[] sublimitAmounts = null;

			policySublimits = PolicySublimitManager.GetAll(policyID, LimitType.LIMIT_TYPE_PROPERTY);

			if (policySublimits != null && policySublimits.Count > 0) {
				sublimitAmounts = policySublimits.Select(x => x.Sublimit.ToString()).ToArray();

				hf_SublimitAmounts.Value = string.Join("|", sublimitAmounts);
			}

			claimSublimits = ClaimSubLimitManager.GetAll(claimID);

			if (claimSublimits != null) {
				subLimitBlank = new ClaimSubLimit();
				claimSublimits.Add(subLimitBlank);
			}

			gvLimits.DataSource = claimSublimits;
			gvLimits.DataBind();
		}
		

		protected void gvLimits_RowCommand(object sender, GridViewCommandEventArgs e) {

		}

		protected void gvLimits_RowDataBound(object sender, GridViewRowEventArgs e) {
			DropDownList ddlSubLimit = null;
			ClaimSubLimit claimSubLimit = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				ddlSubLimit = e.Row.FindControl("ddlSubLimit") as DropDownList;				
				CollectionManager.FillCollection(ddlSubLimit, "PolicySublimitID", "SublimitDescription", policySublimits);

				if (e.Row.DataItem != null) {
					claimSubLimit = e.Row.DataItem as ClaimSubLimit;

					// set selected value
					if (claimSubLimit.PolicySubLimit != null) {
						ddlSubLimit.SelectedValue = claimSubLimit.PolicySublimitID.ToString();
					}
				}
			}
		}

		public void saveLimits(int claimID) {
			int claimSubLimitID = 0;
			ClaimSubLimit sublimit = null;
			

			foreach (GridViewRow row in gvLimits.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					WebTextEditor txtCoverageLetter = row.FindControl("txtCoverageLetter") as WebTextEditor;
					DropDownList ddlSubLimit = row.FindControl("ddlSubLimit") as DropDownList;

					// skip blank
					if (string.IsNullOrEmpty(txtCoverageLetter.Text) && ddlSubLimit.SelectedIndex == 0)
						continue;


					WebNumericEditor txtSubLimitLossAmount = row.FindControl("txtSubLimitLossAmount") as WebNumericEditor;
					WebNumericEditor txtACVAmount = row.FindControl("txtACVAmount") as WebNumericEditor;
					WebNumericEditor txtRCVAmount = row.FindControl("txtRCVAmount") as WebNumericEditor;
					WebNumericEditor txtOverage = row.FindControl("txtOverage") as WebNumericEditor;

					claimSubLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[0];

					if (claimSubLimitID == 0) {
						// new record
						sublimit = new ClaimSubLimit();
						sublimit.ClaimID = claimID;
					}
					else
						sublimit = ClaimSubLimitManager.Get(claimSubLimitID);

					if (ddlSubLimit.SelectedIndex > 0)
						sublimit.PolicySublimitID = Convert.ToInt32(ddlSubLimit.SelectedValue);
					else
						sublimit.PolicySublimitID = null;

					sublimit.LimitLetter = txtCoverageLetter.Text;
					sublimit.LossAmount = txtSubLimitLossAmount.Value == null ? 0 : Convert.ToDecimal(txtSubLimitLossAmount.Value);
					sublimit.ACVAmount = txtACVAmount.Value == null ? 0 : Convert.ToDecimal(txtACVAmount.Value);
					sublimit.RCVAmount = txtRCVAmount.Value == null ? 0 : Convert.ToDecimal(txtRCVAmount.Value);
					sublimit.OverageAmount = txtOverage.Value == null ? 0 : Convert.ToDecimal(txtOverage.Value);

					try {
						ClaimSubLimitManager.Save(sublimit);
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}
			}
		}		
	}
}