using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.UserControl.Admin {
	public partial class ucFeeDesignation : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public string SelectedValue {
			get { return ddlFeeDesignation.SelectedValue; }
			set { ddlFeeDesignation.SelectedValue = value; }
		}

		public int SelectedIndex {
			get { return ddlFeeDesignation.SelectedIndex; }
			set { ddlFeeDesignation.SelectedIndex = value; }
		}
	}
}