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
    using CRM.Data.Entities;
	#endregion
	public partial class ucApplicationConfiguration : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				fillRecord();
			}
		}
		List<ApplicationConfiguration> objAppConfig = null;
		private void fillRecord() {
			var predicate = PredicateBuilder.True<CRM.Data.Entities.ApplicationConfiguration>();
			predicate = predicate.And(config => config.Status == 1);
			objAppConfig = ApplicationConfigurationManager.GetPredicate(predicate);
			if (objAppConfig != null && objAppConfig.Count > 0) {
				txtEmailSite1.Text = objAppConfig[0].EmailNotificationSite1 == null ? string.Empty : objAppConfig[0].EmailNotificationSite1.ToString();
				txtEmailSite2.Text = objAppConfig[0].EmailNotificationSite2 == null ? string.Empty : objAppConfig[0].EmailNotificationSite2.ToString();
				txtEmailSite3.Text = objAppConfig[0].EmailNotificationSite3 == null ? string.Empty : objAppConfig[0].EmailNotificationSite3.ToString();
				txtPrimeryProducer.Text = objAppConfig[0].PrimaryProducerCommissionPercent == null ? "0" : objAppConfig[0].PrimaryProducerCommissionPercent.ToString();
				txtSecondaryProducer.Text = objAppConfig[0].SecondaryProducerCommissionPercent == null ? "0" : objAppConfig[0].SecondaryProducerCommissionPercent.ToString();
			}
			else {
				txtEmailSite1.Text = string.Empty;
				txtEmailSite2.Text = string.Empty;
				txtEmailSite3.Text = string.Empty;
				txtPrimeryProducer.Text = "0";
				txtSecondaryProducer.Text = "0";
			}
		}
		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			try {
				using (TransactionScope scope = new TransactionScope()) {
					ApplicationConfigurationManager.UpdateAppConfigStatus();
					ApplicationConfiguration objAppConfig = new ApplicationConfiguration();
					objAppConfig.PrimaryProducerCommissionPercent = Convert.ToDecimal(txtPrimeryProducer.Text);
					objAppConfig.SecondaryProducerCommissionPercent = Convert.ToDecimal(txtSecondaryProducer.Text);

					objAppConfig.EmailNotificationSite1 = txtEmailSite1.Text.Trim();
					objAppConfig.EmailNotificationSite2 = txtEmailSite2.Text.Trim();
					objAppConfig.EmailNotificationSite3 = txtEmailSite3.Text.Trim();
					objAppConfig.Status = 1;
					ApplicationConfigurationManager.Save(objAppConfig);
					fillRecord();
					lblSave.Text = "Record Saved.";
					lblSave.Visible = true;
					scope.Complete();
				}
			}
			catch (Exception ex) {
				lblError.Text = "Record Not Saved.";
				lblError.Visible = true;
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			fillRecord();
		}
	}
}