namespace CRM.Web.UserControl.Admin {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Data.Account;
	using CRM.Data;
	using LinqKit;
	using CRM.Core;
	using System.Globalization;
	using System.Transactions;
    using CRM.Data.Entities;
	public partial class ucProducer : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			this.Page.Form.DefaultButton = btnSave.UniqueID;

			if (!IsPostBack) {					
				DoBind();
			}

			if (Session["LeadIds"] == null)
				btnReturnToClaim.Visible = false;
		}

		
		private void DoBind() {
			int clientID = 0;
			List<Activity> act = null;

			int roleID = Core.SessionHelper.getUserRoleId();

			clientID = Core.SessionHelper.getClientId();

			//producers = ProducerManager.GetAll(clientID);
            
            act = ActivityManager.GetAll();

			gvProducer.DataSource = act.ToList();
			gvProducer.DataBind();

		}



		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;

			if (e.CommandName.Equals("DoEdit")) {
				pnlEdit.Visible = true;

				//int producerId = Convert.ToInt32(e.CommandArgument);
                int activityId = Convert.ToInt32(e.CommandArgument);
                Session["ActivityID"] = Convert.ToInt32(e.CommandArgument);
                //hdId.Value = producerId.ToString();

				//ProducerMaster producer = ProducerManager.GetProducerId(producerId);

                Activity myActivity = ActivityManager.GetActivityById(activityId);
                if (myActivity != null)
                    txtActivity.Text = myActivity.Activity1;
                    txtActivity.Focus();
			}
			else if (e.CommandName.Equals("DoDelete")) {

				try {
					var producer = ProducerManager.GetProducerId(Convert.ToInt32(e.CommandArgument));
                    var activities = ActivityManager.GetActivityById(Convert.ToInt32(e.CommandArgument));
                    int activityId = Convert.ToInt32(e.CommandArgument);
					//producer.Status = 0;
					//ProducerManager.Save(producer);

                    ActivityManager.Delete(activityId);

					clearFields();
					
					lblSave.Text = "Record Deleted Successfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Deleted.";
					lblError.Visible = true;
				}
			}
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			IQueryable<ProducerMaster> producers = null;
			int clientID = Core.SessionHelper.getClientId();


			producers = ProducerManager.GetAll(clientID);

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gvProducer.DataSource = producers.Cast<ProducerMaster>().AsQueryable().orderByExtension(e.SortExpression, descending);

			gvProducer.DataBind();

		}


		protected void btnSave_Click(object sender, EventArgs e) {

			int clientID = Core.SessionHelper.getClientId();

			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
            Activity myActivity = null;

            int id = 0;
            id = Convert.ToInt32(Session["ActivityID"]);

            //bool exists = ProducerManager.IsExist(txtProducer.Text.Trim(), clientID);

            //if (exists) {
            //    lblMessage.Text = "Primary Producer Name already exists.";
            //    lblMessage.Visible = true;
            //    txtProducer.Focus();
            //    return;
            //}

			try {
				using (TransactionScope scope = new TransactionScope()) {
                    
                        if (id == 0)
                        {
                            myActivity = new Activity();
                        }
                        else
                        {
                            myActivity = ActivityManager.GetActivityById(id);
                        }
                        myActivity.Activity1 = txtActivity.Text;

                        ActivityManager.Save(myActivity);
                       
                    
                    //ProducerMaster producer = ProducerManager.GetProducerId(Convert.ToInt32(hdId.Value));
                    //producer.ProducerName = txtProducer.Text;
                    //producer.Status = 1;

					// tortega 2013-08-09
					//producer.ClientId = clientID;

					//ProducerManager.Save(producer);
                     clearFields();
					lblSave.Text = id == 0 ? "Record Saved Successfully." : "Record Updated Successfully.";
                    lblSave.Visible = true;
					scope.Complete();
				}

				pnlEdit.Visible = false;

				

			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Record Not Saved !!!";

				Core.EmailHelper.emailError(ex);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			pnlEdit.Visible = false;

			clearFields();
		}

		private void clearFields() {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtProducer.Text = string.Empty;
			hdId.Value = "0";
			DoBind();
		}

		protected void gvProducer_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			GridView gridView = sender as GridView;
			IQueryable<ProducerMaster> producers = null;
			int clientID = Core.SessionHelper.getClientId();

			producers = ProducerManager.GetAll(clientID);

			bool descending = false;

			string lastSorExpression = ViewState["lastSorExpression"] as string;

			if (lastSorExpression == null) {
				lastSorExpression = "ProducerName";
				descending = false;
			}
			else
				descending = !(bool)ViewState[lastSorExpression];

			ViewState[lastSorExpression] = descending;

			gridView.PageIndex = e.NewPageIndex;

			gridView.DataSource = producers.orderByExtension(lastSorExpression, descending);

			gridView.DataBind();

		}

		protected void btnNew_Click(object sender, EventArgs e) {
			pnlEdit.Visible = true;
            lblSave.Visible = false;
            Session["ActivityID"] = 0;
			txtActivity.Text = string.Empty;
		}

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				Response.Redirect("~/protected/newlead.aspx?q=" + CRM.Core.SecurityManager.EncryptQueryString(Session["LeadIds"].ToString()));
			}
		}
	}
}