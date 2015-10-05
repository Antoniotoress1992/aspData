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
    using LinqKit;
    using CRM.Data.Entities;
    public partial class ucCountry : System.Web.UI.UserControl
    {
        #region OLD Code of Country
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //        DoBind(); 
        //}
        //private void DoBind()
        //{
        //    gvData.DataSource = CountryMasterManager.GetAll();
        //    gvData.DataBind();
        //}
        //protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName.Equals("DoEdit"))
        //    {
        //        pnlEdit.Enabled = true;
        //        pnlList.Enabled = false;

        //        CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(e.CommandArgument));
        //        hdId.Value = country.CountryID.ToString();
        //        txtCountryName.Text = country.CountryName;
        //        if (country.Status == true)
        //            txtStatus.Text = "1";
        //        else
        //            txtStatus.Text = "0";

        //        //SecModuleManager.Save(module);
        //    }
        //    else if (e.CommandName.Equals("DoDelete"))
        //    {
        //        pnlList.Enabled = true;
        //        pnlEdit.Enabled = false;

        //        CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(e.CommandArgument));
        //        country.Status = false;//Status.Deleted.ToString();
        //        CountryMasterManager.Save(country);
        //        DoBind();
        //    }

        //}
        //protected void lbnNew_Click(object sender, EventArgs e)
        //{
        //    pnlEdit.Enabled = true;
        //    pnlList.Enabled = false;
        //    hdId.Value = "0";
        //}
        //private void ResetForm()
        //{
        //    txtCountryName.Text = string.Empty;
        //    hdId.Value = "0";
        //    txtStatus.Text = string.Empty;
        //    DoBind();

        //}
        //protected void btnSave_Click(object sender, ImageClickEventArgs e)
        //{
        //    CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(hdId.Value));
        //    country.CountryName = txtCountryName.Text.Trim();
        //    bool _status = false;
        //    if (txtStatus.Text.Trim() == "1")
        //        _status = true;
        //    else
        //        _status = false;
        //    country.Status = _status;
        //    CountryMasterManager.Save(country);
        //    btnCancel_Click(null, null);
        //}
        //protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        //{
        //    pnlEdit.Enabled = false;
        //    pnlList.Enabled = true;

        //    ResetForm();
        //}
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(hdId.Value));
        //    country.CountryName = txtCountryName.Text.Trim();
        //    bool _status = false;
        //    if (txtStatus.Text.Trim() == "1")
        //        _status = true;
        //    else
        //        _status = false;
        //    country.Status = _status;
        //    CountryMasterManager.Save(country);
        //    btnCancel_Click(null, null);
        //}
        #endregion
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
                DoBind();
            }
            if (hfNewPermission.Value == "0")
                btnSave.Enabled = false;
        }
        private void DoBind()
        {
            var predicate = PredicateBuilder.True<CRM.Data.Entities.CountryMaster>();
            if (!String.IsNullOrWhiteSpace(hfKeywordSearch.Value))
            {
                var keyword = hfKeywordSearch.Value;
                predicate = predicate.And(country => country.CountryName.Contains(keyword)
                    //|| commission.Status.Contains(keyword)
                                        );
            }
            lvCountry.DataSource = CountryMasterManager.GetPredicate(predicate);
            lvCountry.DataBind();
        }
        protected void lvCountry_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            if (e.CommandName.Equals("DoEdit"))
            {
                divEntry.Visible = true;
                pnlList.Enabled = false;
                btnSave.Enabled = true;
                CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(e.CommandArgument));
                hdId.Value = country.CountryID.ToString();
                txtCountryName.Text = country.CountryName;
            }
            else if (e.CommandName.Equals("DoDelete"))
            {
                try
                {
                    pnlList.Enabled = true;
                    //pnlEdit.Enabled = false;

                    CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(e.CommandArgument));
                    //country.Status = false;
                    //CountryMasterManager.Save(country);
                    CountryMasterManager.Delete(country);
                    DoBind();
                    lblSave.Text = "Record Deleted Sucessfully.";
                    lblSave.Visible = true;
                }
                catch (Exception ex)
                {
                    lblError.Text = "Record Not Deleted.";
                    lblError.Visible=true;
                }
            }
        }
        protected void lbnNew_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;

            //pnlEdit.Enabled = true;
            //pnlEdit.Visible = true;
            divEntry.Visible = true;
            pnlList.Enabled = false;
            hdId.Value = "0";
        }
        private void ResetForm()
        {
            txtCountryName.Text = string.Empty;
            hdId.Value = "0";
            ddlStatus.SelectedIndex = 0;
            DoBind();

        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            try
            {
                //CountryMaster country = CountryMasterManager.GetByCountryId(Convert.ToInt32(hdId.Value));
                //country.CountryName = txtCountryName.Text.Trim();
                //country.Status = ddlStatus.SelectedValue == "1" ? true : false;
                //CountryMasterManager.Save(country);
                //btnCancel_Click(null, null);
                //lblSave.Text = "Record Saved Sucessfully.";
                //lblSave.Visible = true;
                bool isnew = false;
                CountryMaster country = new CountryMaster();
                if (hdId.Value == "0")
                {
                    isnew = true;
                }
                else
                {
                    country = CountryMasterManager.GetByCountryId(Convert.ToInt32(hdId.Value));
                }
                bool Exists = CountryMasterManager.IsCountryExists(txtCountryName.Text.Trim());
                if (isnew)
                {
                    if (Exists)
                    {
                        lblMessage.Text = "Country Name Exists !!!";
                        lblMessage.Visible = true;
                        txtCountryName.Focus();
                        return;
                    }
                }
                else
                {
                    if (country.CountryName != txtCountryName.Text.Trim())
                    {
                        if (Exists)
                        {
                            lblMessage.Text = "Country Name Exists !!!";
                            lblMessage.Visible = true;
                            txtCountryName.Focus();
                            return;
                        }
                    }
                }
                country.CountryName = txtCountryName.Text.Trim();
                CountryMasterManager.Save(country);
                btnCancel_Click(null, null);
                lblSave.Text = "Record Saved Sucessfully.";
                lblSave.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Record Not Saved.";
                lblError.Visible = true;
            }
        }
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;

            //pnlEdit.Enabled = false;
            //pnlEdit.Visible = false;
            divEntry.Visible = false;
            pnlList.Enabled = true;

            ResetForm();
        }

        protected void lvCountry_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;

            PagerRow.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DoBind();
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            PagerRow.SetPageProperties(0, PagerRow.MaximumRows, true);
            if (!String.IsNullOrWhiteSpace(txtKeywords.Text))
            {
                hfKeywordSearch.Value = txtKeywords.Text.Trim();
            }
            DoBind();
        }

        protected void btnReset_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = string.Empty;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
            lblError.Visible = false;
            lblMessage.Visible = false;
            lblMessage.Text = string.Empty;
            txtKeywords.Text = "";
            hfKeywordSearch.Value = "";
            DoBind();

        }
    }
}