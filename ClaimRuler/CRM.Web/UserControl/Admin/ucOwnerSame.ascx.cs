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

namespace CRM.Web.UserControl.Admin
{
    public partial class ucOwnerSame : System.Web.UI.UserControl
    {
        List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ** Set the permission of loged in user to hidden fields according to page name  ** //

                string str = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                string pagename = str.Substring(str.LastIndexOf("/") + 1);
                if (Session["rolePermission"] != null)
                {
                    resRolePermission = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];
                    resRolePermission = resRolePermission.Where(x => (x.Url.ToLower() == pagename)).ToList();
                    if (resRolePermission.Count > 0)
                    {
                        hfDeletePermission.Value = resRolePermission[0].DeletePermission == null ? "0" : resRolePermission[0].DeletePermission == true ? "1" : "0";
                        hfNewPermission.Value = resRolePermission[0].AddPermssion == null ? "0" : resRolePermission[0].AddPermssion == true ? "1" : "0";
                        hfEditPermission.Value = resRolePermission[0].EditPermission == null ? "0" : resRolePermission[0].EditPermission == true ? "1" : "0";
                        hfViewPermission.Value = resRolePermission[0].ViewPermssion == null ? "0" : resRolePermission[0].ViewPermssion == true ? "1" : "0";
                    }
                }
                else
                {
                    Response.Redirect("~/Protected/Admin/ServiceNotAvailable.aspx");
                }
                DoBind();
            }
        }
        private void DoBind()
        {
            List<OwnerSameMaster> lst = OwnerSameManager.GetAll();
            if (lst.Count > 0)
            {
                lvData.DataSource = lst;
                lvData.DataBind();
                dvData.Visible = true;
                PagerRow.Visible = true;
            }
            else
            {
                dvData.Visible = false;
                PagerRow.Visible = false;
            }
        }

        protected void lvData_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            lblError.Text = string.Empty;
            lblSave.Text = string.Empty;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            lblError.Visible = false;
            lblSave.Visible = false;
            if (e.CommandName.Equals("DoEdit"))
            {
                int id = Convert.ToInt32(e.CommandArgument);
                hdId.Value = id.ToString();
                Label lblName = (Label)e.Item.FindControl("lblName");
                txtName.Text = lblName.Text;

            }
            else if (e.CommandName.Equals("DoDelete"))
            {
                // In Case of delete 
                try
                {
                    var list = CheckPrimaryValue.CheckPrimaryValueExists(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(e.CommandArgument), 0);
                    if (list.ToList().Count > 0)
                    {
                        lblMessage.Text = "Owner Same is used so you can't delete !!!";
                        lblMessage.Visible = true;
                        return;
                    }
                    var ownerSame = OwnerSameManager.GetById(Convert.ToInt32(e.CommandArgument));
                    ownerSame.Status = false;
                    OwnerSameManager.Save(ownerSame);
                    btnCancel_Click(null, null);
                    lblSave.Text = "Record Deleted Sucessfully.";
                    lblSave.Visible = true;
                }
                catch (Exception ex)
                {
                    lblError.Text = "Record Not Deleted.";
                    lblError.Visible = true;
                }
            }
        }


        protected void lvData_PreRender(object sender, EventArgs e)
        {
            DoBind();
            //PagerRow.PageSize = 15;
        }
        string saveMsg = string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    bool exists = OwnerSameManager.IsExist(txtName.Text.Trim(), Convert.ToInt32(hdId.Value));
                    if (exists)
                    {
                        lblMessage.Text = "Owner Same Name already exists.";
                        lblMessage.Visible = true;
                        txtName.Focus();
                        return;
                    }
                    OwnerSameMaster ownerSame = OwnerSameManager.GetById(Convert.ToInt32(hdId.Value));
                    ownerSame.OwnerSame = txtName.Text;
                    ownerSame.Status = true;
                    OwnerSameManager.Save(ownerSame);
                    saveMsg = hdId.Value == "0" ? "Record Saved Sucessfully." : "Record Updated Sucessfully.";
                    btnCancel_Click(null, null);
                    lblSave.Text = saveMsg;
                    lblSave.Visible = true;
                    scope.Complete();
                }
                //btnCancel_Click(null, null);
                //lblSave.Visible = true;
                //lblSave.Text = "Record Saved Sucessfully.";
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Record Not Saved !!!";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            txtName.Text = string.Empty;
            hdId.Value = "0";

            DoBind();
        }
    }
}