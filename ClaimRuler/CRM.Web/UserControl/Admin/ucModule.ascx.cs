
namespace CRM.Web.UserControl.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CRM.Data.Account;
    using CRM.Data;
    using System.Transactions;
    using CRM.Core;
    public partial class ucModule : System.Web.UI.UserControl
    {
        List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                bindDDL();
                DoBind();
            }
            if (hfNewPermission.Value == "0")
                lbnNew.Enabled = false;
        }

        protected void ValidateURL(object source, ServerValidateEventArgs args)
        {
            if (ddlParentID.SelectedValue != "0")
            {
                args.IsValid = (txtURL.Text.Trim() != "");
            }
        }

        private void bindDDL()
        {
            CollectionManager.FillCollection(ddlParentID, "ModuleId", "ModuleName", SecModuleManager.GetAll());
        }

        private void DoBind()
        {
            gvData.DataSource = SecModuleManager.GetAll();
            gvData.DataBind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            CRM.Data.Entities.SecModule module = SecModuleManager.GetByModuleId(Convert.ToInt32(hdId.Value));
            module.ModuleName = txtModuleName.Text;
            module.ModuleDesc = txtModuleDescription.Text;
            module.Url = txtURL.Text;
            int? _parentId = null;
            module.ParentId = Convert.ToInt32(ddlParentID.SelectedValue) == 0 ? _parentId : Convert.ToInt32(ddlParentID.SelectedValue);
            module.Status = ddlStatus.SelectedValue == "1" ? true : false;
            if (txtURL.Text.Trim() != string.Empty)
                module.ModuleType = 1;
            else
                module.ModuleType = 0;
            if (Convert.ToInt32(ddlParentID.SelectedValue) > 0 && txtURL.Text.Trim() == string.Empty)
            {
                lblError.Text = "Please Enter Url";
                return;
            }
            SecModuleManager.Save(module);
            btnCancel_Click(null, null);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEdit.Enabled = false;
            pnlList.Enabled = true;
            pnlEdit.Visible = false;
            lbnNew.Visible = true;
            ResetForm();
        }
        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            if (e.CommandName.Equals("DoEdit"))
            {
                pnlEdit.Enabled = true;
                pnlList.Enabled = false;
                pnlEdit.Visible = true;
                lbnNew.Visible = false;
                CRM.Data.Entities.SecModule module = SecModuleManager.GetByModuleId(Convert.ToInt32(e.CommandArgument));
                hdId.Value = module.ModuleId.ToString();
                txtModuleName.Text = module.ModuleName;
                txtModuleDescription.Text = module.ModuleDesc;
                txtURL.Text = module.Url;
                ddlParentID.SelectedValue = module.ParentId == null ? "0" : module.ParentId.ToString();
                ddlStatus.SelectedValue = module.Status == true ? "1" : "0";
                ddlStatus.Focus();
            }
            else if (e.CommandName.Equals("DoDelete"))
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        pnlList.Enabled = true;
                        pnlEdit.Enabled = false;

                        CRM.Data.Entities.SecModule module = SecModuleManager.GetByModuleId(Convert.ToInt32(e.CommandArgument));
                        module.Status = false;
                        SecModuleManager.Save(module);
                        DoBind();
                        scope.Complete();
                        lblSave.Text = "Record Deleted Sucessfully.";
                        lblSave.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Record Not Deleted !!!";
                    lblError.Visible = true;
                }
            }

        }
        protected void lbnNew_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            pnlEdit.Enabled = true;
            pnlList.Enabled = false;
            pnlEdit.Visible = true;
            hdId.Value = "0";
            ddlStatus.Focus();
            lbnNew.Visible = false;
        }
        private void ResetForm()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            txtModuleName.Text = string.Empty;
            txtModuleDescription.Text = string.Empty;
            ddlParentID.SelectedIndex = 0;
            hdId.Value = "0";
            txtURL.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            DoBind();

        }

        protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            gvData.PageIndex = e.NewPageIndex;
            DoBind();
        }

    }
}
