using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using LinqKit;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Collections;
using System.Drawing;
using Infragistics.Web.UI.EditorControls;

using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {


	public partial class UploadImages : System.Web.UI.UserControl {

		PagedDataSource adsource = new PagedDataSource();

		int pos;
		string ErrorMessage = string.Empty;

		//int leadID = 0;

		public int leadID {
			get { return Session["LeadIds"] == null ? 0 : Convert.ToInt32(Session["LeadIds"]); }
		}
		public int policyID {
			get { return Session["policyID"] == null ? 0 : Convert.ToInt32(Session["policyID"]); }
		}
		protected void Page_Load(object sender, EventArgs e) {


		}

		public void bindData(int claimID) {
			adsource.PageSize = 10;
			adsource.AllowPaging = true;

			pos = Convert.ToInt32(ViewState["vs"]);

			Data.Entities.LeadPolicy policy = LeadPolicyManager.Get(this.policyID);

			if (policy != null) {
				ViewState["policyTypeID"] = policy.PolicyType.ToString();

				ViewState["vs"] = ViewState["pageIndex"] == null ? "0" : ViewState["pageIndex"].ToString();

				pos = Convert.ToInt32(ViewState["vs"]);

				Session["pageIndex"] = null;

				FillImage(this.leadID);

			}
		}

		



		private void FillImage(int LeadId) {
			int policyTypeID = Convert.ToInt32(ViewState["policyTypeID"]);

			if (policyTypeID > 0) {
				List<LeadsImage> img = LeadsUploadManager.getLeadsImageByLeadID(LeadId, policyTypeID);
				if (img != null && img.Count > 0) {

					adsource.DataSource = img;
					ViewState["ls"] = adsource.PageCount - 1;
					adsource.CurrentPageIndex = pos;
					ViewState["pageIndex"] = pos;
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
					// divImage.Visible = false;
					dtlist.DataSource = img;
					dtlist.DataBind();
					t1.Visible = false;
				}
			}
			else {
				dtlist.DataSource = null;
				dtlist.DataBind();
				t1.Visible = false;
			}
		}




		protected void dtlist_ItemCommand(object sender, DataListCommandEventArgs e) {			
			if (e.CommandName.Equals("DoDelete")) {

				int LeadImageId = Convert.ToInt32(e.CommandArgument);
				try {
					var list = LeadsUploadManager.GetLeadsImageById(LeadImageId);
					list.Status = 0;
					LeadsUploadManager.SaveImage(list);


					lblSave.Text = "Image deleted Successfully.";
					lblSave.Visible = true;
					FillImage(this.leadID);
				}
				catch (Exception ex) {
					lblError.Text = "Image not deleted.";
					lblError.Visible = true;
				}

			}
			if (e.CommandName.Equals("DoRotate")) {
				try {

					System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Item.FindControl("Image1");
					// get the full path of image url
					string path = Server.MapPath(img.ImageUrl);

					// creating image from the image url
					System.Drawing.Image i = System.Drawing.Image.FromFile(path);

					// rotate Image 90' Degree
					i.RotateFlip(RotateFlipType.Rotate90FlipXY);

					// save it to its actual path
					i.Save(path);
					// release Image File
					i.Dispose();

					img.Attributes.Add("ImageUrl", path);
				}
				catch (Exception ex) {
				}
			}
		}

		protected void dtlist_ItemDataBound(object sender, DataListItemEventArgs e) {
			string js = null;
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();

			CheckBox cbxPrint = e.Item.FindControl("cbxPrint") as CheckBox;
			LeadsImage leadImage = e.Item.DataItem as LeadsImage;

			if (cbxPrint != null && leadImage != null) {
				js = string.Format("javascript:togglePrintOption(this, {0});", leadImage.LeadImageId);
				cbxPrint.Attributes["onclick"] = js;
			}

			System.Web.UI.WebControls.Image photo = e.Item.FindControl("Image1") as System.Web.UI.WebControls.Image;

			if (!string.IsNullOrEmpty(leadImage.ImageName))
				photo.ImageUrl = string.Format("{0}/LeadsImage/{1}/{2}/{3}", appUrl, leadImage.LeadId, leadImage.LeadImageId, leadImage.ImageName);
			else
				photo.ImageUrl = string.Format("{0}/Images/no.jpg", appUrl);

		}

		protected void btnfirst_Click(object sender, ImageClickEventArgs e) {
			pos = 0;
			this.ViewState["vs"] = pos;
			this.ViewState["pageIndex"] = pos;
			FillImage(this.leadID);
		}

		protected void btnprevious_Click(object sender, ImageClickEventArgs e) {
			pos = Convert.ToInt32(ViewState["vs"]);
			pos -= 1;
			this.ViewState["vs"] = pos;
			this.ViewState["pageIndex"] = pos;
			FillImage(this.leadID);
		}

		protected void btnnext_Click(object sender, ImageClickEventArgs e) {
			pos = Convert.ToInt32(this.ViewState["vs"]);
			pos += 1;
			this.ViewState["vs"] = pos;
			this.ViewState["pageIndex"] = pos;
			FillImage(this.leadID);
		}

		protected void btnlast_Click(object sender, ImageClickEventArgs e) {
			pos = Convert.ToInt32(ViewState["ls"]); //adsource.PageCount - 1;
			this.ViewState["vs"] = pos;
			this.ViewState["pageIndex"] = pos;
			FillImage(this.leadID);
		}

	}


}