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

namespace CRM.Web.UserControl {
	public partial class ucClaimCasualtyLimits : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(int claimID) {
			List<vw_ClaimLimit> limits = ClaimLimitManager.GetAll(claimID, LimitType.LIMIT_TYPE_CASUALTY);

			if (limits != null && limits.Count == 0) {
				//limits = primeCasualtyLimits();
			}

			//gvLimits.DataSource = limits;
			//gvLimits.DataBind();
            lblNoRecordFound.Visible = false;
            if (limits.Count > 0)
            {

                if (limits[0].IsStatic != false)
                {

                    gvLimits.DataSource = limits;
                    gvLimits.DataBind();

                    gvLimits2.DataSource = null;
                    gvLimits2.DataBind();
                }
                else
                {

                    gvLimits.DataSource = null;
                    gvLimits.DataBind();
                    gvLimits2.DataSource = limits;
                    gvLimits2.DataBind();                    
                }
            }
            else
            {
                lblNoRecordFound.Visible = true;
            }
            
		}

		private List<vw_ClaimLimit> primeCasualtyLimits() {
			List<Limit> limits = null;
			List<vw_ClaimLimit> casualtyLimits = null;

			limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_CASUALTY);

			casualtyLimits = (from x in limits
						   select new vw_ClaimLimit {
							   LimitID = x.LimitID,
							   LimitLetter = x.LimitLetter,
							   LimitType = x.LimitType,
							   LimitDescription = x.LimitDescription
						   }).ToList<vw_ClaimLimit>();

			return casualtyLimits;
		}
		public void saveLimits(int claimID) {
			int claimLimitID = 0;
			int limitID = 0;
			ClaimLimit limit = null;

			foreach (GridViewRow row in gvLimits.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					try {
						WebNumericEditor txtLossAmount = row.FindControl("txtLossAmount") as WebNumericEditor;


						claimLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[0];
						limitID = (int)gvLimits.DataKeys[row.RowIndex].Values[1];

						if (claimLimitID == 0) {
							// new record
							limit = new ClaimLimit();
							limit.ClaimID = claimID;
							limit.LimitID = limitID;
						}
						else
							limit = ClaimLimitManager.Get(claimLimitID);
						
						limit.LossAmountACV = txtLossAmount.Value == null ? 0 : Convert.ToDecimal(txtLossAmount.Value);


						ClaimLimitManager.Save(limit);
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}
			}
		}
	}
}