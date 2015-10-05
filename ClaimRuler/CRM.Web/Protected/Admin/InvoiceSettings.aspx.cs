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

namespace CRM.Web.Protected.Admin {
	public partial class InvoiceSettings : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			if (!Page.IsPostBack)
				bindData();
		}

		private void bindData() {
			Client client = null;

			int clientID = SessionHelper.getClientId();

			client = ClientManager.Get(clientID);
			if (client != null) {
				txtInvoicePaymentTerms.Value = client.InvoicePaymentTerms;
				txtLossPercentageFee.Value = client.InvoiceContingencyFee;
				ddlAutomaticInvoiceMethod.SelectedValue = (client.InvoiceSettingID ?? 0).ToString();
			}
		}
		protected void btnSave_Click(object sender, EventArgs e) {
			Client client = null;

			int clientID = SessionHelper.getClientId();


			client = ClientManager.Get(clientID);
			if (client != null) {
				if (ddlAutomaticInvoiceMethod.SelectedIndex > 0)
					client.InvoiceSettingID = Convert.ToInt32(ddlAutomaticInvoiceMethod.SelectedValue);
				else
					client.InvoiceSettingID = null;

				client.InvoiceContingencyFee = txtLossPercentageFee.ValueDecimal;

				client.InvoicePaymentTerms = txtInvoicePaymentTerms.ValueInt;

				try {
					ClientManager.Save(client);
					lblMessage.Text = "Invoice settings saved successfully.";
					lblMessage.CssClass = "ok";

				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);

					lblMessage.Text = "Unable to save invoice settings.";
					lblMessage.CssClass = "error";
				}
			}
		}
	}
}