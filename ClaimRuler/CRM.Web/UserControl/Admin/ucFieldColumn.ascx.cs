using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucFieldColumn : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack) {
				loadData();
			}
		}

		protected void btnSave_click(object sender, EventArgs e) {
			int clientID = SessionHelper.getClientId();
			
			try {
				FieldColumnManager.DeleteAll(clientID);

				foreach (GridViewRow row in gvFieldColumn.Rows) {
					if (row.RowType == DataControlRowType.DataRow) {
						CheckBox cbx = row.FindControl("cbx") as CheckBox;
						if (cbx != null) {
							ClientFieldColumn column = new ClientFieldColumn();
							column.ClientID = clientID;
							column.FieldColumnID = (int)gvFieldColumn.DataKeys[row.RowIndex].Value;
							column.isVisible = cbx.Checked;

							FieldColumnManager.Save(column);
						}
					}
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}

		}

		protected void loadData() {
			int clientID = SessionHelper.getClientId();

			List<vw_FieldColumn> columns = FieldColumnManager.GetAll(clientID);
			
			if (columns == null || columns.Count == 0) {
				columns = FieldColumnManager.GetInitialFieldList(clientID);
			}

			gvFieldColumn.DataSource = columns;
			gvFieldColumn.DataBind();
		}
	}
}