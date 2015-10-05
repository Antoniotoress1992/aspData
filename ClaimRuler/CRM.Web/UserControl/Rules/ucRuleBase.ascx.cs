using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Repository;

namespace CRM.Web.UserControl.Rules {
	public partial class ucRuleBase : System.Web.UI.UserControl {
		public event EventHandler cancelButtonClick;

		protected virtual void Page_Load(object sender, EventArgs e) {

		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			if (cancelButtonClick != null) {
				cancelButtonClick(this, e);
			}
		}


		protected void setValue(DropDownList ddl, string value) {
			ListItem item = null;

			item = ddl.Items.FindByValue(value);
			if (item != null)
				ddl.SelectedIndex = ddl.Items.IndexOf(item);
			else
				ddl.SelectedIndex = -1;
		}

		public XDocument buildRootNode() {
			XDocument ruleXml = new XDocument(new XDeclaration("1.0", "UTF-8", "yes")); //create xml doc
			XElement ruleNode = new XElement("rule");
			
			ruleXml.Add(ruleNode);

			return ruleXml;
		}

	}
}