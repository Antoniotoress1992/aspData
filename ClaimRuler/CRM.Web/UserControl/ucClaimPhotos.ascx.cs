using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using CRM.Repository;

using System.Drawing;
using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucClaimPhotos : System.Web.UI.UserControl {

		public int claimID {
			get { return Session["ClaimID"] == null ? 0 : Convert.ToInt32(Session["ClaimID"]); }
		}

		PagedDataSource adsource = new PagedDataSource();
		int pos = 0;

		protected void Page_Load(object sender, EventArgs e) {
			
		}

		public void bindData(int claimID) {			
			Claim claim = ClaimsManager.Get(claimID);
			
			if (claim != null) {

				ViewState["vs1"] = Session["pageIndex1"] == null ? "0" : Session["pageIndex1"].ToString();

				pos = Convert.ToInt32(this.ViewState["vs1"]);

				Session["pageIndex1"] = null;

				ViewState["policyTypeID"] = claim.LeadPolicy.PolicyType.ToString();

				FillImage(claimID);
			}
		}

		// after upload is complete, this is called to force user to enter image description
		protected void btnGridBind_Click(object sender, EventArgs e) {			

			if (this.claimID > 0) {
				FillImage(claimID);
			}
		}

		private void FillImage(int claimID) {
			List<ClaimImage> images = null;

			images = ClaimImageManager.GetAll(claimID);

			if (images != null && images.Count > 0) {
				adsource.DataSource = images;
				ViewState["ls1"] = adsource.PageCount - 1;
				adsource.CurrentPageIndex = pos;
				Session["pageIndex1"] = pos;
				lblPageCount.Text = "Page " + (pos + 1) + " of " + adsource.PageCount;


				btnfirst.Enabled = !adsource.IsFirstPage;
				btnprevious.Enabled = !adsource.IsFirstPage;
				btnlast.Enabled = !adsource.IsLastPage;
				btnnext.Enabled = !adsource.IsLastPage;
				t1.Visible = true;
				dtlist.DataSource = adsource;
				dtlist.DataBind();
			}
			else {
				dtlist.DataSource = images;
				dtlist.DataBind();
				t1.Visible = false;
			}
		}
		protected void dtlist_ItemCommand(object sender, DataListCommandEventArgs e) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			ClaimImage claimImage = null;
			int claimImageID = 0;

			if (e.CommandName.Equals("DoDelete")) {

				claimImageID = Convert.ToInt32(e.CommandArgument);

				try {
					claimImage = ClaimImageManager.Get(claimImageID);
					claimImage.IsActive = false;
					ClaimImageManager.Save(claimImage);


					lblMessage.Text = "Photo deleted successfully.";
					lblMessage.CssClass = "ok";

					FillImage(this.claimID);
				}
				catch (Exception ex) {
					lblMessage.Text = "Photo not deleted.";
					lblMessage.CssClass = "error";
				}

			} else if (e.CommandName.Equals("DoRotate")) {
				try {
					claimImageID = Convert.ToInt32(e.CommandArgument);
					claimImage = ClaimImageManager.Get(claimImageID);

					//System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Item.FindControl("Image1");
					// get the full path of image url
					string path = string.Format("{0}/ClaimImage/{1}/{2}/{3}", appPath, claimImage.ClaimID, claimImage.ClaimImageID, claimImage.ImageName);
				
					// creating image from the image url
					System.Drawing.Image i = System.Drawing.Image.FromFile(path);

					// rotate Image 90' Degree
					i.RotateFlip(RotateFlipType.Rotate90FlipXY);

					// save it to its actual path
					i.Save(path);
					// release Image File
					i.Dispose();

					//img.Attributes.Add("ImageUrl", path);					
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void dtlist_ItemDataBound(object sender, DataListItemEventArgs e) {
			string js = null;
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();

			CheckBox cbxPrint = e.Item.FindControl("cbxPrint") as CheckBox;
			ClaimImage claimImage = e.Item.DataItem as ClaimImage;
			
			ImageButton btnRotate = e.Item.FindControl("btnRotate") as ImageButton;
			ImageButton ibtnDelete = e.Item.FindControl("ibtnDelete") as ImageButton;

			ibtnDelete.Visible = PermissionHelper.checkDeletePermission("UsersLeads.aspx");
			btnRotate.Visible = PermissionHelper.checkDeletePermission("UsersLeads.aspx");

			if (cbxPrint != null && claimImage != null) {
				js = string.Format("javascript:togglePrintOption(this, {0});", claimImage.ClaimImageID);
				cbxPrint.Attributes["onclick"] = js;
			}

			System.Web.UI.WebControls.Image photo = e.Item.FindControl("Image1") as System.Web.UI.WebControls.Image;

			if (!string.IsNullOrEmpty(claimImage.ImageName))
				photo.ImageUrl = string.Format("{0}/ClaimImage/{1}/{2}/{3}", appUrl, claimImage.ClaimID, claimImage.ClaimImageID, claimImage.ImageName);
			else
				photo.ImageUrl = string.Format("{0}/Images/no.jpg", appUrl);

		}

		protected void btnfirst_Click(object sender, ImageClickEventArgs e) {
			pos = 0;
			this.ViewState["vs1"] = pos;
			this.Session["pageIndex1"] = pos;
			
			FillImage(this.claimID);
		}

		protected void btnprevious_Click(object sender, ImageClickEventArgs e) {
			pos = Convert.ToInt32(this.ViewState["vs1"]);
			pos -= 1;
			this.ViewState["vs1"] = pos;
			this.Session["pageIndex1"] = pos;
			FillImage(this.claimID);
		}

		protected void btnnext_Click(object sender, ImageClickEventArgs e) {
			//pos = (int)this.ViewState["vs"];
			pos = Convert.ToInt32(this.ViewState["vs"]);
			pos += 1;
			this.ViewState["vs1"] = pos;
			this.Session["pageIndex1"] = pos;
			FillImage(this.claimID);
		}

		protected void btnlast_Click(object sender, ImageClickEventArgs e) {
			pos = Convert.ToInt32(ViewState["ls1"]); //adsource.PageCount - 1;
			this.ViewState["vs1"] = pos;
			this.Session["pageIndex1"] = pos;
			FillImage(this.claimID);
		}
	}
}