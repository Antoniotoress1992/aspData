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
    public partial class ucWindPolicy : System.Web.UI.UserControl
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

            List<WindPolicyMaster> lst = WindPolicyManager.GetAll();
            if (lst.Count > 0)
            {
                dvData.Visible = true;
                PagerRow.Visible = true;
                lvData.DataSource = lst;
                lvData.DataBind();
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
                int ReportedtoinsId = Convert.ToInt32(e.CommandArgument);
                hdId.Value = ReportedtoinsId.ToString();
                Label lblWindPolicy = (Label)e.Item.FindControl("lblWindPolicy");
                txtWindPolicy.Text = lblWindPolicy.Text;

            }
            else if (e.CommandName.Equals("DoDelete"))
            {
                // In Case of delete 
                try
                {
                    var list = CheckPrimaryValue.CheckPrimaryValueExists(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(e.CommandArgument), 0, 0, 0, 0);
                    if (list.ToList().Count > 0)
                    {
                        lblMessage.Text = "Wind Policy is used so you can't delete !!!";
                        lblMessage.Visible = true;
                        return;
                    }
                    var dt = WindPolicyManager.GetbyWindPolicyId(Convert.ToInt32(e.CommandArgument));
                    dt.Status = false;
                    WindPolicyManager.Save(dt);
                    DoBind();
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

                bool exists = WindPolicyManager.IsExist(txtWindPolicy.Text.Trim(), Convert.ToInt32(hdId.Value));
                if (exists)
                {
                    lblMessage.Text = "Wind Policy allready exists.";
                    lblMessage.Visible = true;
                    txtWindPolicy.Focus();
                    return;
                }
                WindPolicyMaster obj = WindPolicyManager.GetbyWindPolicyId(Convert.ToInt32(hdId.Value));
                obj.WindPolicy = txtWindPolicy.Text;
                obj.Status = true;
                WindPolicyManager.Save(obj);
                saveMsg = hdId.Value == "0" ? "Record Saved Sucessfully." : "Record Updated Sucessfully.";
                btnCancel_Click(null, null);
                lblSave.Text = saveMsg;
                lblSave.Visible = true;
               
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Record Not Saved !!!";
            }
        }

        private void clearControls()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            txtWindPolicy.Text = string.Empty;
            hdId.Value = "0";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            txtWindPolicy.Text = string.Empty;
            hdId.Value = "0";
            DoBind();
        }
    }
}