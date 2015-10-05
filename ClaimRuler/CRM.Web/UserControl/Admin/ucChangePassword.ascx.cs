
namespace CRM.Web.UserControl.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CRM.Core;
    using CRM.Data.Account;
    using CRM.Data;
    using System.Transactions;
    using LinqKit;
    using System.Data;
    public partial class ucChangePassword : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //login
		   this.Page.Form.DefaultButton = btnSave.UniqueID;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;
          
            int userId = Convert.ToInt32(Session["UserId"]);
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConPassword.Text.Trim();

            
            

                CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userId);
                if (user != null && user.UserName != null && user.Password != null)
                {
                    string password = SecurityManager.Decrypt(user.Password);
                    if (password == oldPassword)
                    {
                        if (newPassword != confirmPassword)
                        {
                            lblMessage.Text = string.Empty;
                            lblMessage.Text = "Confirm Password Must Match!";
                            lblMessage.Visible = true;
                        }
                        else
                        {

                            user.UserId = userId;
                            user.UserName = user.UserName;
                            user.Password = SecurityManager.Encrypt(newPassword);
                            SecUserManager.Save(user);
                            resetControl();
                            lblSave.Text = "Password Updated Successfully!";
                            lblSave.Visible = true;
                        }
                    }
                    else
                    {
                        lblMessage.Text = string.Empty;
                        lblMessage.Text = "Old Password Is Wrong.";
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Text = "UserId Not Valid.";
                    lblMessage.Visible = true;
                }
            

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            resetControl();
        }
        private void resetControl()
        {
            lblError.Text = string.Empty;
            lblSave.Text = string.Empty;
            lblMessage.Text = string.Empty;
            lblError.Visible = false;
            lblSave.Visible = false;
            lblMessage.Visible = false;
            txtConPassword.Text = string.Empty;
            txtOldPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
        }
    }
}