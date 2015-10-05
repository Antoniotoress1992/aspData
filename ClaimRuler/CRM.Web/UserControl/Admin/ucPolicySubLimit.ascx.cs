using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucPolicySubLimit : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
		}

		public void bindData(int policyID, int limitType) {
			List<PolicySubLimit> limits = null;
			PolicySubLimit limit = null;

			limits = PolicySublimitManager.GetAll(policyID, limitType);

			if (limits != null && limits.Count == 0) {
				switch (limitType) {
					case 1:	// property limits
						limits = primePropertyLimits();
						break;

					default:
						break;
				}
			}
			else {
				// add a blank entry to allow new entry
				limit = new PolicySubLimit();

				limit.LimitType = LimitType.LIMIT_TYPE_PROPERTY;

				limits.Add(limit);
			}

			gvLimits.DataSource = limits;
			gvLimits.DataBind();
		}
		private List<PolicySubLimit> primePropertyLimits() {
			List<PolicySubLimit> limits = new List<PolicySubLimit>();

			PolicySubLimit limit = new PolicySubLimit();

			limit.LimitType = LimitType.LIMIT_TYPE_PROPERTY;

			limits.Add(limit);

			return limits;
		}

		public void saveLimits(int policyID) {
			int policySubLimitID = 0;
			int limitType = 0;
			PolicySubLimit limit = null;

			foreach (GridViewRow row in gvLimits.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					WebTextEditor txtCoverage = row.FindControl("txtCoverage") as WebTextEditor;
					WebTextEditor txtSublimit = row.FindControl("txtSublimit") as WebTextEditor;

					if (string.IsNullOrEmpty(txtCoverage.Text.Trim()) && string.IsNullOrEmpty(txtSublimit.Text.Trim()))
						continue;

					policySubLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[0];
					limitType = (int)gvLimits.DataKeys[row.RowIndex].Values[1];

					if (policySubLimitID == 0)
						limit = new PolicySubLimit();
					else
						limit = PolicySublimitManager.Get(policySubLimitID);

					limit.PolicyID = policyID;
					limit.PolicySublimitID = policySubLimitID;
					limit.LimitType = limitType;

					limit.SublimitDescription = txtCoverage.Text;
					limit.Sublimit = txtSublimit.Value == null ? 0 : Convert.ToDecimal(txtSublimit.Value);

					try {
						PolicySublimitManager.Save(limit);
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}

				}
			}
            bindData(policyID,1);
		}

		protected void gvLimits_RowCommand(object sender, GridViewCommandEventArgs e) {
			int policySublimitID = 0;
			int policyID = 0;
			string[] values = e.CommandArgument.ToString().Split(new char[] { ',' });

			policySublimitID = Convert.ToInt32(values[0]);

			policyID = Convert.ToInt32(values[1]);

			try {
				PolicySublimitManager.Delete(policySublimitID);
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}
			finally {
				bindData(policyID, LimitType.LIMIT_TYPE_PROPERTY);
			}
		}
	}
}