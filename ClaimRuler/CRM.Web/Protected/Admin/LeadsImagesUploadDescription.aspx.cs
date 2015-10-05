using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class LeadsImagesUploadDescription : System.Web.UI.Page {

		protected void Page_Load(object sender, EventArgs e) {			
			if (!Page.IsPostBack) {
				loadData();
			}
		}

		protected void btnSaveContinue_Click(object sender, EventArgs e) {
			foreach (DataListItem item in dlImagesLocationDescription.Items) {
				int leadImageID = (int)dlImagesLocationDescription.DataKeys[item.ItemIndex];
				LeadsImage leadImage = null;

				TextBox txtLocation = item.FindControl("txtLocation") as TextBox;

				TextBox txtDescription = item.FindControl("txtDescription") as TextBox;
				
				if (!string.IsNullOrEmpty(txtLocation.Text) && !string.IsNullOrEmpty(txtDescription.Text)) {
					leadImage = LeadsUploadManager.GetLeadsImageById(leadImageID);
					if (leadImage != null) {
						leadImage.Location = txtLocation.Text;

						leadImage.Description = txtDescription.Text;

						LeadsUploadManager.SaveImage(leadImage);
					}
				}				
			}

			// send user back to list of images
			Response.Redirect("~/Protected/Admin/LeadsImagesUpload.aspx");

		}

		

		private void loadData() {
			List<LeadsImage> images = null;
			int leadID = 0;

			leadID = Convert.ToInt32(Session["LeadIds"]);
			images = LeadsUploadManager.getLeadsImagesMissingLocationDescription(leadID);
			//image.LeadImageId
			dlImagesLocationDescription.DataSource = images;

			dlImagesLocationDescription.DataBind();
		}

		protected void dlImagesLocationDescription_ItemDataBound(object sender, DataListItemEventArgs e) {
			if (e.Item.ItemType == ListItemType.Item ||e.Item.ItemType == ListItemType.AlternatingItem) {
				Image photoImage = (Image)e.Item.FindControl("photoImage");

				LeadsImage leadImage = e.Item.DataItem as LeadsImage;

				string imageUrl = leadImage.ImageName == String.Empty ? "../../Images/no.jpg" : "../../LeadsImage/" + Convert.ToString(leadImage.LeadId) + "/" + Convert.ToString(leadImage.LeadImageId) + "/" + Convert.ToString(leadImage.ImageName);

				photoImage.ImageUrl = imageUrl;
			}
		}
	}
}