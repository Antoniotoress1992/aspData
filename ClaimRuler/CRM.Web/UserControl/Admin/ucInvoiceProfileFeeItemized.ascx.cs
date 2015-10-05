using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucInvoiceProfileFeeItemized : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindItems(int profileID) {
			ViewState["profileID"] = profileID.ToString();

			gvItems.DataSource = CarrierInvoiceProfileFeeItemizedManager.GetAll(profileID);

			gvItems.DataBind();
		}

		private void bindInvoiceServices() {
			int clientID = Core.SessionHelper.getClientId();
			List<InvoiceServiceType> services = null;

			services = InvoiceServiceManager.GetAll(clientID).ToList();


			Core.CollectionManager.FillCollection(ddlServices, "ServiceTypeID", "ServiceDescription", services);
		}

		private void bindInvoiceExpenses() {
			List<ExpenseType> expenses = null;
			int clientID = Core.SessionHelper.getClientId();

			using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
				expenses = repository.GetAll(clientID).ToList();
			}

			Core.CollectionManager.FillCollection(ddlExpenses, "ExpenseTypeID", "ExpenseName", expenses);
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			pnlAddService.Visible = false;
			pnlAddExpense.Visible = false;

			pnlGrid.Visible = true;

			refreshGrid();
		}

		private void clearItemFields() {
			txtItemDescription.Text = string.Empty;
			txtServicePercentage.Value = 0;
			txtServiceRate.Value = 0;

			txtExpenseDescription.Text = string.Empty;
			txtExpensePercentage.Value = 0;
			txtExpenseRate.Value = 0;
            txtSerPayroll.Text = "";
            txtSerPayrollFee.Text = "";
            txtExpPayroll.Text = "";
            txtExpPayrollFee.Text = "";
			ddlConditionalOperator.SelectedIndex = 0;
			txtOperand.Value = 0;
		}

		protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e) {
			CarrierInvoiceProfileFeeItemized item = null;
			int id = 0;
			int profileID = Convert.ToInt32(ViewState["profileID"]);

			if (e.CommandName == "DoEdit") {
				id = Convert.ToInt32(e.CommandArgument);

				ViewState["ID"] = id.ToString();

				item = CarrierInvoiceProfileFeeItemizedManager.Get(id);

				if (item != null) {
					if (item.InvoiceServiceType != null) {
						pnlAddService.Visible = true;
						pnlGrid.Visible = false;

						bindInvoiceServices();

						ddlServices.SelectedValue = (item.ServiceTypeID ?? 0).ToString();
						txtServicePercentage.Value = item.ItemPercentage;
						txtServiceRate.Value = item.ItemRate;
						txtItemDescription.Text = item.ItemDescription;
                        //NEW OC 10/9/14
                        txtSerPayroll.Value = item.AdjusterPayroll;
                        txtSerPayrollFee.Value = item.AdjusterPayrollFlatFee;
					}
					else if (item.ExpenseType != null) {
						pnlAddExpense.Visible = true;
						pnlGrid.Visible = false;

						bindInvoiceExpenses();

						ddlExpenses.SelectedValue = (item.ExpenseTypeID ?? 0).ToString();
						txtExpenseDescription.Text = item.ItemDescription;
						txtExpensePercentage.Value = item.ItemPercentage;
						txtExpenseRate.Value = item.ItemRate;
                        //NEW OC 10/9/14
                        txtExpPayroll.Value = item.AdjusterPayroll;
                        txtExpPayrollFee.Value = item.AdjusterPayrollFlatFee;
						if (item.LogicalOperator != null) {
							ddlConditionalOperator.SelectedValue = item.LogicalOperator.ToString();
							tr_percentage.Visible = false;
						}
						else {
							tr_percentage.Visible = true;
						}
						
						txtOperand.Value = item.LogicalOperatorOperand;
					}

				}
			}
			else if (e.CommandName == "DoDelete") {
				id = Convert.ToInt32(e.CommandArgument);
				CarrierInvoiceProfileFeeItemizedManager.Delete(id);

				refreshGrid();

			}
		}



		protected void lbtnAddService_Click(object sender, EventArgs e) {
			ViewState["ID"] = "0";
			pnlAddService.Visible = true;
			pnlGrid.Visible = false;

			clearItemFields();

			bindInvoiceServices();
		}

		protected void lbtnAddExpense_Click(object sender, EventArgs e) {
			ViewState["ID"] = "0";
			pnlAddExpense.Visible = true;
			pnlGrid.Visible = false;

			clearItemFields();

			bindInvoiceExpenses();

			tr_percentage.Visible = true;
		}

		protected void btnAddService_save_Click(object sender, EventArgs e) {
			CarrierInvoiceProfileFeeItemized item = null;

			Page.Validate("service");
			if (!Page.IsValid)
				return;

			int profileID = Convert.ToInt32(ViewState["profileID"]);

			int id = Convert.ToInt32(ViewState["ID"]);

			if (id == 0) {
				item = new CarrierInvoiceProfileFeeItemized();

				item.CarrierInvoiceProfileID = profileID;

				item.IsActive = true;
			}
			else {
				item = CarrierInvoiceProfileFeeItemizedManager.Get(id);
			}

			if (item != null) {
				item.ServiceTypeID = Convert.ToInt32(ddlServices.SelectedValue);

				item.ItemDescription = txtItemDescription.Text;

				item.ItemPercentage = txtServicePercentage.ValueDecimal;

				item.ItemRate = txtServiceRate.ValueDecimal;
                //new fields OC 10/9/14
                item.AdjusterPayroll = txtSerPayrollFee.Text == null ? 0 : txtSerPayroll.ValueDecimal;
                item.AdjusterPayrollFlatFee = txtSerPayrollFee.Text == null ? 0 : txtSerPayrollFee.ValueDecimal;
                //
				try {
					CarrierInvoiceProfileFeeItemizedManager.Save(item);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

				pnlAddService.Visible = false;
				pnlGrid.Visible = true;

				bindItems(profileID);
			}
		}


		protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e) {
			CarrierInvoiceProfileFeeItemized item = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				if (e.Row.DataItem != null) {
					item = e.Row.DataItem as CarrierInvoiceProfileFeeItemized;
					Label lblServiceDescription = e.Row.FindControl("lblServiceDescription") as Label;

					if (item.InvoiceServiceType != null) {
						lblServiceDescription.Text = item.InvoiceServiceType.ServiceDescription;
					}
					else if (item.ExpenseType != null) {
						lblServiceDescription.Text = item.ExpenseType.ExpenseDescription;
					}
				}
			}
		}

		private void refreshGrid() {
			int profileID = Convert.ToInt32(ViewState["profileID"]);

			bindItems(profileID);
		}

		protected void btnAddExpense_save_Click(object sender, EventArgs e) {
			CarrierInvoiceProfileFeeItemized item = null;
            ExpenseType expense = null; //NEW to account for rate pointing to correct place
			Page.Validate("expense");
			if (!Page.IsValid)
				return;

			int profileID = Convert.ToInt32(ViewState["profileID"]);

			int id = Convert.ToInt32(ViewState["ID"]);

			if (id == 0) {
				item = new CarrierInvoiceProfileFeeItemized();

				item.CarrierInvoiceProfileID = profileID;

				item.IsActive = true;
			}
			else {
				item = CarrierInvoiceProfileFeeItemizedManager.Get(id);
			}

			if (item != null) {
				item.ExpenseTypeID = Convert.ToInt32(ddlExpenses.SelectedValue);

				item.ItemDescription = txtExpenseDescription.Text;

				item.ItemPercentage = txtExpensePercentage.ValueDecimal;

				item.ItemRate = txtExpenseRate.ValueDecimal;

                item.AdjusterPayroll = txtExpPayroll.Text == null ? 0 : txtExpPayroll.ValueDecimal;

                item.AdjusterPayrollFlatFee = txtExpPayrollFee.Text == null ? 0 : txtExpPayrollFee.ValueDecimal;

				if (ddlConditionalOperator.SelectedIndex > 0) {
					item.LogicalOperator = Convert.ToInt32(ddlConditionalOperator.SelectedValue);
					item.LogicalOperatorOperand = txtOperand.ValueDecimal;
				}
				else {
					item.LogicalOperator = null;
					item.LogicalOperatorOperand = 0;
				}
                
				try {
					CarrierInvoiceProfileFeeItemizedManager.Save(item);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
                //new OC to save the expense rate into the expense type table 10/8/14
                try
                {
                    using (ExpenseTypeManager repository = new ExpenseTypeManager())
                    {
                        int expenseID = Convert.ToInt32(ddlExpenses.SelectedValue);
                        if (expenseID == 0)
                        {
                            expense = new ExpenseType();
                            expense.ClientID = Core.SessionHelper.getClientId();
                            expense.IsActive = true;
                        }
                        else
                        {
                            expense = repository.Get(expenseID);
                        }

                        if (expense != null)
                        {
                            expense.ExpenseRate = txtExpenseRate.ValueDecimal; //txtRateAmount.ValueDecimal;
                            expense = repository.Save(expense);

                        }
                    }

                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);
                    
                }

				pnlAddExpense.Visible = false;
				pnlGrid.Visible = true;

				bindItems(profileID);
			}
		}

		protected void ddlConditionalOperator_SelectedIndexChanged(object sender, EventArgs e) {
			bool isVisible = ddlConditionalOperator.SelectedIndex > 0;
			
			txtOperand.Visible = isVisible;
			
			// hide percentage when condition exists
			tr_percentage.Visible = !isVisible;

		}


	}
}