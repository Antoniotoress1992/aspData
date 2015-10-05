using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class ClaimTimeExpense : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			//masterPage.checkPermission();

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		private void bindData() {
			int claimID = SessionHelper.getClaimID();

			claimExpenses.bindData(claimID);
			claimServices.bindData(claimID);
            var expCount = Session["ExpenseCount"];
            var serCount = Session["ServiceCount"];
            if (Convert.ToInt32(serCount) < 2 && Convert.ToInt32(expCount) < 2)
            {
                popup.Show();
            }
		}
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Protected/ClaimEdit.aspx");
        }

		#region Claim Expenses
		[System.Web.Services.WebMethod]
		public static string getServiceTypeDetail(int id) {
			InvoiceServiceType serviceType = null;
			string result = null;

			using (InvoiceServiceTypeManager repository = new InvoiceServiceTypeManager()) {
				serviceType = repository.Get(id);
			}

			if (serviceType != null) {
				result = string.Format("{0}|{1}", serviceType.DefaultQty, serviceType.ServiceDescription);
			}

			return result;
		}
		#endregion

		
		protected void btnNewService_Click(object sender, EventArgs e) 
        {
            var serCount = Session["ServiceCount"];
            if (Convert.ToInt32(serCount) < 2)
            {
                popUpService.Show();
            }
            else
            {
                claimServices.activateEditPanel();
            }
			
		}

		protected void lbntnNewExpense_Click(object sender, EventArgs e) 
        {
            var expCount = Session["ExpenseCount"];
            if (Convert.ToInt32(expCount) < 2)
            {
                popUpExpense.Show();
            }
            else
            {
                claimExpenses.activateEditPanel();
            }
			
		}
	}
}