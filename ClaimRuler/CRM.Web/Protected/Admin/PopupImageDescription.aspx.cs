
namespace CRM.Web.Protected.Admin {
	#region Namespace
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
	using System.IO;
	using System.Data;
	using iTextSharp.text;
	using iTextSharp.text.pdf;
	using System.Text;
	using System.Collections;
	using System.Net;
	using System.Drawing;
    using CRM.Data.Entities;

	#endregion

	public partial class PopupImageDescription : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (Session["LeadIds"] != null) {

					hfLeadsId.Value = Session["LeadIds"].ToString();

					if (this.Request.QueryString["LeadImageId"] != null) {
						hfLeadImageId.Value = this.Request.QueryString["LeadImageId"];
						if (hfLeadImageId.Value != "")
							FillImage(Convert.ToInt32(Session["LeadIds"]), Convert.ToInt32(this.Request.QueryString["LeadImageId"]));
					}
				}
				if (Session["View"] != null) {
					string view = Session["View"].ToString();
					if (view != null && view == "1") {
						txtDescription.ReadOnly = true;
						txtLocation.ReadOnly = true;
						btnCancel.Visible = false;
						btnUpload.Visible = false;
						hfView.Value = "1";

					}
				}

			}
		}

		private void FillImage(int LeadId, int LeadImageId) {
			LeadsImage img = LeadsUploadManager.getLeadsImageByLeadID(LeadId).Where(x => x.LeadImageId == LeadImageId).SingleOrDefault();
			if (img != null) {
				txtLocation.Text = img.Location;
				txtDescription.Text = img.Description;
				string imageName = img.ImageName == String.Empty ? "../../Images/no.jpg" : "../../LeadsImage/" + Convert.ToString(img.LeadId) + "/" + Convert.ToString(img.LeadImageId) + "/" + Convert.ToString(img.ImageName);
				myimage.ImageUrl = imageName;
			}
			else {

			}
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			//string filepath = myimage.ImageUrl;
			//using (WebClient client = new WebClient())
			//{
			//    client.DownloadFile(filepath, Server.MapPath("~/Image/apple.jpg"));
			//}


			var list = LeadsUploadManager.GetLeadsImageById(Convert.ToInt32(hfLeadImageId.Value));
			list.Location = txtLocation.Text.Trim();
			list.Description = txtDescription.Text.Trim();
			LeadsUploadManager.SaveImage(list);





			//const string javaScript = "<script language=javascript>window.top.close(); window.opener.location.reload(true);</script>";
			const string javaScript = "<script language=javascript>window.top.close(); window.opener.RebindImages();</script>";
			if (!ClientScript.IsStartupScriptRegistered("CloseMyWindow")) {
				ClientScript.RegisterStartupScript(GetType(), "CloseMyWindow", javaScript);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			const string javaScript = "<script language=javascript>window.top.close(); window.opener.location.reload(true);</script>";
			if (!ClientScript.IsStartupScriptRegistered("CloseMyWindow")) {
				ClientScript.RegisterStartupScript(GetType(), "CloseMyWindow", javaScript);
			}
		}

		protected void btnRotate_Click(object sender, EventArgs e) {
			// get the full path of image url
			string path = Server.MapPath(myimage.ImageUrl);

			// creating image from the image url
			System.Drawing.Image i = System.Drawing.Image.FromFile(path);

			// rotate Image 90' Degree
			i.RotateFlip(RotateFlipType.Rotate90FlipXY);

			// save it to its actual path
			i.Save(path);

			// release Image File
			i.Dispose();

			// Set Image Control Attribute property to new image(but its old path)
			myimage.Attributes.Add("ImageUrl", path);
		}
	}
}