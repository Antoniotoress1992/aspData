using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Repository;
using CRM.Data.Entities;


namespace CRM.Web.UserControl.Admin {
	public partial class ucExpenseList : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;

		int clientID = 0;
		int userID = 0;
		int roleID = 0;

		protected void Page_Load(object sender, EventArgs e) {

		}
		protected void Page_Init(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			clientID = Core.SessionHelper.getClientId();
			userID = Core.SessionHelper.getUserId();
			roleID = Core.SessionHelper.getUserRoleId();
		}

		public void bindData() {
			clientID = Core.SessionHelper.getClientId();
			List<ExpenseType> expenseTypes = null;

			using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
				expenseTypes = repository.GetAll(clientID).ToList();
			}

			gvExpenseType.DataSource = expenseTypes;
			gvExpenseType.DataBind();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			showEditPanel();

			// clear fields
			txtExpenseDescription.Text = string.Empty;
			txtExpenseName.Text = string.Empty;
			txtRateAmount.Value = 0;

			ViewState["ExpenseTypeID"] = "0";
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			ExpenseType expense = null;
			int expenseID = 0;

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			expenseID = Convert.ToInt32(ViewState["ExpenseTypeID"]);

			try {
				using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
					if (expenseID == 0) {
						expense = new ExpenseType();
						expense.ClientID = Core.SessionHelper.getClientId();
						expense.IsActive = true;
					}
					else {
						expense = repository.Get(expenseID);
					}
					
					if (expense != null) {
						expense.ExpenseName = txtExpenseName.Text.Trim();
						expense.ExpenseDescription = txtExpenseDescription.Text.Trim();
						expense.ExpenseRate = txtRateAmount.ValueDecimal;

						expense = repository.Save(expense);

						showGridPanel();

						// refresh grid
						bindData();
					}
				}

			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
				showErrorMessage();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showGridPanel();

			bindData();
		}

		protected void gvExpenseType_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvExpenseType.PageIndex = e.NewPageIndex;

			bindData();
		}

		protected void gvExpenseType_Sorting(object sender, GridViewSortEventArgs e) {
			IQueryable<ExpenseType> expenseTypes = null;

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
				expenseTypes = repository.GetAll(clientID);

				gvExpenseType.DataSource = expenseTypes.orderByExtension(e.SortExpression, descending);

				gvExpenseType.DataBind();
			}
		}

		protected void gvExpenseType_RowCommand(object sender, GridViewCommandEventArgs e) {
			int expenseID = 0;
			ExpenseType expense = null;

			if (e.CommandName == "DoEdit") {
				expenseID = Convert.ToInt32(e.CommandArgument);

				using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
					expense = repository.Get(expenseID);
				}
				if (expense != null) {
					showEditPanel();

					txtExpenseDescription.Text = expense.ExpenseDescription;

					txtExpenseName.Text = expense.ExpenseName;

					txtRateAmount.Value = expense.ExpenseRate;

					ViewState["ExpenseTypeID"] = expenseID.ToString();
				}
			}
			else if (e.CommandName == "DoDelete") {
				try {
					expenseID = Convert.ToInt32(e.CommandArgument);

					using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
						expense = repository.Get(expenseID);

						if (expense != null) {
							expense.IsActive = false;

							expense = repository.Save(expense);

							// refresh grid
							bindData();
						}
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					showErrorMessage();
				}

			}
		}

		private void showEditPanel() {
			pnlGrid.Visible = false;
			pnlEditExpense.Visible = true;
		}

		private void showErrorMessage() {
			lblMessage.Text = "Unable to save changes.";
			lblMessage.CssClass = "error";
		}

		private void showGridPanel() {
			pnlGrid.Visible = true;
			pnlEditExpense.Visible = false;
		}
	}
}