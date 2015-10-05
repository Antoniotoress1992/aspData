using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;

namespace CRM.Web.UserControl.Admin {
	public partial class ucPolicyType : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack)
				bindData();	

		}

		public string requiredValidation {
			get { return ViewState["requiredValidation"].ToString(); }
			set { 
				ViewState["requiredValidation"] = value;
				//rfvPolicyType.Visible = value.ToLower().Equals("true");
			}
		}

		protected void bindData() {
			CollectionManager.FillCollection(ddlPolicyType, "LeadPolicyTypeID", "Description", LeadPolicyTypeManager.GetAll());
		}
		public string SelectedValue {
            get { return ddlPolicyType.SelectedValue; }
            set { ddlPolicyType.SelectedValue = value; }
		}

		public int SelectedIndex {
			get { return ddlPolicyType.SelectedIndex; }
			set { ddlPolicyType.SelectedIndex = value; }
		}
	}
}