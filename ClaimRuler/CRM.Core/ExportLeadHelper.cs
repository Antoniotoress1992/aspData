using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;

using iTextSharp.text;
using iTextSharp.text.pdf;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Core {
	public class ExportLeadHelper {
		//static string[] policyDescriptions = { "", "Homeowners", "Commerical", "Flood", "Earthquake" };
		static Document doc = null;
		static BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
		BaseColor CellBackgroundColor = BaseColor.BLACK;
		BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
		BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
		BaseColor BorderColor = BaseColor.BLACK;
		//static string tempPath = null;

		static string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		#region Fonts

		static float fontsize = 10f;
		static Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
		static Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
		static Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
		static Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
		static Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
		static Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
		static Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
		static Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
		static Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
		static Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
		static Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
		static Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
		static Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
		static Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
		static Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
		static Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
		static Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
		static Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
		static Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
		static Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
		static Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
		static Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
		static Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
		static Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
		static Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
		#endregion

		static public string exportLead(ExportParameter exportParamaters, int claimID) {
			string finalReportPath = null;
			//tempPath = path;
			Claim claim = null;
			Leads lead = null;
			LeadPolicy policy = null;

			claim = ClaimsManager.Get(claimID);

			lead = claim.LeadPolicy.Leads;

			policy = claim.LeadPolicy;
		
			// fully qualified path

			//string reportPath = HttpContext.Current.Server.MapPath(string.Format("~/Temp/{0}.pdf", Guid.NewGuid().ToString()));

			string reportPath = HttpContext.Current.Server.MapPath(string.Format("~/Temp/ClaimReport_{0}_{1}_{2:yyyyMMddhhmmss}.pdf", lead.insuredName.Replace(" ", "_"), claim.InsurerClaimNumber, DateTime.Now));
			
			doc = new Document(iTextSharp.text.PageSize.LETTER, 25, 25, 25, 25);

			PdfWriter w = PdfWriter.GetInstance(doc, new FileStream(reportPath, FileMode.Create));

			Rectangle rect = new Rectangle(iTextSharp.text.PageSize.LETTER.Width, iTextSharp.text.PageSize.LETTER.Height);
			rect.BackgroundColor = BaseColor.WHITE;
			doc.SetPageSize(rect);
			doc.Open();

			setClientLogoHeader(lead.Client);

			if (exportParamaters.isAll) {
				exportDemographics(lead, claim);

				exportCoverage(policy);

				exportClaimLog(policy);

				exportPhotos(policy);
			}
			else {

				exportDemographics(lead, claim);

				if (exportParamaters.isCoverage)
					exportCoverage(policy);

				if (exportParamaters.isClaimLogo) {
					// legacy comments
					exportClaimLog(policy);
					exportClaimLog(claimID, claim.AdjusterClaimNumber);
				}

				if (exportParamaters.isPhotos) {
					// legacy
					exportPhotos(policy);
					exportPhotos(claimID, claim.AdjusterClaimNumber);
				}


			}



			doc.Close();

			// include any pdf documents uploaded
			if (exportParamaters.isAll || exportParamaters.isDocuments) {
				string reportMergePath = HttpContext.Current.Server.MapPath(string.Format("~/Temp/ClaimReport_{0}_{1}_{2:yyyyMMddhhmmss}.pdf", lead.insuredName.Replace(" ", "_"), claim.AdjusterClaimNumber, DateTime.Now.AddMinutes(1)));

				finalReportPath = mergeDocuments(reportPath, reportMergePath, lead.LeadId, claimID);
			}
			else
				finalReportPath = reportPath;

			return finalReportPath;
		}

		#region demographics

		static protected void exportDemographics(Leads lead, Claim claim) {
			Stream imageStream = null;
			string cityName = null;
			string stateName = null;
			string zipCode = null;
			string[] damageType = null;
			string imagePath = null;

			if (lead == null)
				return;

			PdfPTable tblHeader = new PdfPTable(new float[] { 30, 60 });
			tblHeader.WidthPercentage = 90;

			PdfPCell headerCell = new PdfPCell(new Phrase("Policyholder Information"));
			headerCell.Colspan = 2;
			headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			tblHeader.AddCell(headerCell);
			doc.Add(tblHeader);

			PdfPTable ptable = new PdfPTable(new float[] { 30, 60 });

			addTableCell(ptable, "Insured Name");
			addTableCell(ptable, lead.InsuredName);

			addTableCell(ptable, "First Name");
			addTableCell(ptable, lead.ClaimantFirstName);

			addTableCell(ptable, "Last Name");
			addTableCell(ptable, lead.ClaimantLastName);

			addTableCell(ptable, "Business Name");
			addTableCell(ptable, lead.BusinessName);

			addTableCell(ptable, "Phone");
			addTableCell(ptable, lead.PhoneNumber);

			addTableCell(ptable, " ");
			addTableCell(ptable, lead.SecondaryPhone);

			addTableCell(ptable, "Email");
			addTableCell(ptable, lead.EmailAddress);

			addTableCell(ptable, " ");
			addTableCell(ptable, lead.SecondaryEmail);


			addTableCell(ptable, "Loss Address");
			addTableCell(ptable, lead.LossAddress);

			addTableCell(ptable, " ");
			addTableCell(ptable, lead.LossAddress2);

			cityName = lead.CityName ?? " ";
			stateName = lead.StateName ?? " ";
			zipCode = lead.Zip ?? " ";

			addTableCell(ptable, " ");
			addTableCell(ptable, cityName + ", " + stateName + " " + zipCode);

			addTableCell(ptable, "Type of Loss");

			if (!string.IsNullOrEmpty(claim.CauseOfLoss)) {
				string[] causeOfLossDescriptions = TypeofDamageManager.GetDescriptions(claim.CauseOfLoss);

				string typeOfLoss = string.Join(",", causeOfLossDescriptions);
				addTableCell(ptable, typeOfLoss);
			}
			
			//addTableCell(ptable, "Lead Source");
			//addTableCell(ptable, lead.LeadSourceMaster == null ? " " : lead.LeadSourceMaster.LeadSourceName);


			doc.Add(ptable);

			//if (lead.Latitude != null && lead.Longitude != null) {
			//	imageStream = bingMapHelper.getMapImage(lead.Latitude, lead.Longitude);

			//	// add areal map to pdf
			//	if (imageStream != null) {
			//		Image img = iTextSharp.text.Image.GetInstance(imageStream);

			//		PdfPTable maptable = new PdfPTable(new float[] { 90 });
			//		maptable.WidthPercentage = 90;

			//		PdfPCell filler = new PdfPCell(new Phrase(" "));
			//		filler.Border = 0;
			//		maptable.AddCell(filler);

			//		PdfPCell imageHeader = new PdfPCell(new Phrase("Loss Map"));
			//		imageHeader.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			//		imageHeader.VerticalAlignment = PdfPCell.ALIGN_CENTER;
			//		//imageHeader.PaddingBottom = 10f;
			//		//imageHeader.PaddingTop = 10f;
			//		maptable.AddCell(imageHeader);

			//		img.ScaleToFit(500f, 400f);
			//		PdfPCell cellp = new PdfPCell(img, false);


			//		cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
			//		cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;


			//		cellp.Padding = 2f;
			//		//cellp.PaddingBottom = 2;
			//		//cellp.PaddingRight = 2;
			//		cellp.BorderWidthLeft = 1f;
			//		cellp.BorderWidthRight = 1f;
			//		cellp.BorderWidthTop = 1f;
			//		cellp.BorderWidthBottom = 1f;
			//		maptable.AddCell(cellp);

			//		doc.Add(maptable);
			//	}
			//}
		}

		#endregion

		#region claim log
		static protected void exportClaimLog(LeadPolicy policy) {
			int leadID = (int)policy.LeadId;
			int policyType = (int)policy.PolicyType;

			string policyDescription = policy.LeadPolicyType != null ? policy.LeadPolicyType.Description : "";

			List<LeadComment> policyComments = LeadCommentManager.getLeadCommentByLeadID(leadID, policyType);

			if (policyComments == null || policyComments.Count == 0)
				return;

			// create table 2 cells
			PdfPTable ptable = new PdfPTable(new float[] { 20, 80 });
			ptable.WidthPercentage = 90;


			addPolicyComments(ptable, policyComments, policyDescription);


			doc.Add(ptable);

		}
		
		static protected void exportClaimLog(int claimID, string claimNumber) {

			List<ClaimCommented> claimComments = ClaimCommentManager.GetAll(claimID);

			if (claimComments == null || claimComments.Count == 0)
				return;

			// create table 2 cells
			PdfPTable ptable = new PdfPTable(new float[] { 20, 80 });
			ptable.WidthPercentage = 90;


			addClaimComments(ptable, claimComments, claimNumber);


			doc.Add(ptable);

		}
		static protected void addPolicyComments(PdfPTable ptable, List<LeadComment> comments, string title) {
			if (comments == null || comments.Count == 0)
				return;

			// create header
			doc.NewPage();

			PdfPCell headerCell = new PdfPCell(new Phrase(title + " Claim Log"));
			headerCell.Colspan = 2;
			headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			ptable.AddCell(headerCell);

			foreach (LeadComment comment in comments) {
				// date
				PdfPCell cellp = new PdfPCell(new Phrase("."));
				cellp = new PdfPCell(new Phrase(comment.InsertDate.ToString()));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				cellp.Border = 0;
				cellp.PaddingLeft = 10;
				((Chunk)(cellp.Phrase[0])).Font = black9Font;
				ptable.AddCell(cellp);

				// comment
				PdfPCell cellComment = new PdfPCell(new Phrase("."));

				// removed html tags
				string commentText = Regex.Replace(comment.CommentText ?? "", "<.*?>", string.Empty);
				if (!string.IsNullOrEmpty(commentText)) {
					cellComment = new PdfPCell(new Phrase(commentText));
					cellComment.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
					cellComment.VerticalAlignment = PdfPCell.ALIGN_TOP;
					cellComment.Border = 0;
					cellComment.PaddingLeft = 10;
					((Chunk)(cellComment.Phrase[0])).Font = black9Font;
					ptable.AddCell(cellComment);
				}
			}
		}
		static protected void addClaimComments(PdfPTable ptable, List<ClaimCommented> comments, string title) {
			if (comments == null || comments.Count == 0)
				return;

			// create header
			doc.NewPage();

			PdfPCell headerCell = new PdfPCell(new Phrase(title + " Claim Log"));
			headerCell.Colspan = 2;
			headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			ptable.AddCell(headerCell);

			foreach (ClaimCommented comment in comments) {
				// date
				PdfPCell cellp = new PdfPCell(new Phrase("."));
				cellp = new PdfPCell(new Phrase(comment.CommentDate.ToString()));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				cellp.Border = 0;
				cellp.PaddingLeft = 10;
				((Chunk)(cellp.Phrase[0])).Font = black9Font;
				ptable.AddCell(cellp);

				// comment
				PdfPCell cellComment = new PdfPCell(new Phrase("."));

				// removed html tags
				string commentText = Regex.Replace(comment.CommentText ?? "", "<.*?>", string.Empty);

				cellComment = new PdfPCell(new Phrase(commentText));
				cellComment.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellComment.VerticalAlignment = PdfPCell.ALIGN_TOP;
				cellComment.Border = 0;
				cellComment.PaddingLeft = 10;
				((Chunk)(cellComment.Phrase[0])).Font = black9Font;
				ptable.AddCell(cellComment);
			}
		}
		#endregion

		#region photos

		static protected void exportPhotos(LeadPolicy policy) {
			int leadID = (int)policy.LeadId;
			int policyTypeID = (int)policy.PolicyType;
			string policyDescription = policy.LeadPolicyType != null ? policy.LeadPolicyType.Description : "";

			List<LeadsImage> images = LeadsUploadManager.getLeadsImageByLeadID(leadID, policyTypeID);


			if (images == null || images.Count == 0)
				return;


			addPhotos(images, policyDescription);
		}
		static protected void exportPhotos(int claimID, string claimNumber) {

			List<ClaimImage> images = Repository.ClaimImageManager.GetAll(claimID);


			if (images == null || images.Count == 0)
				return;


			addPhotos(images, claimNumber);
		}

		static protected void addPhotos(List<LeadsImage> images, string title) {
			if (images == null || images.Count == 0)
				return;

			string imgPath = null;

			PdfPTable ptable = null;

			// create header		

			doc.NewPage();

			PdfPTable tblHeader = new PdfPTable(new float[] { 60, 30 });
			tblHeader.WidthPercentage = 90;

			PdfPCell headerCell = new PdfPCell(new Phrase(title + " Photos"));
			headerCell.Colspan = 2;
			headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			tblHeader.AddCell(headerCell);
			doc.Add(tblHeader);

			// picture counter
			int p = 1;

			PdfPCell cellp = new PdfPCell(new Phrase("."));
			iTextSharp.text.Image img = null;

			for (int l = 0; l < images.Count; l++) {

				if (p == 1) {
					// first page
					ptable = new PdfPTable(new float[] { 60, 30 });
					ptable.WidthPercentage = 90;
				}

				cellp = new PdfPCell(new Phrase("."));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.Border = 0;
				cellp.Colspan = 2;
				cellp.PaddingTop = 10;
				((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
				ptable.AddCell(cellp);


				PdfPTable ptableLocation = new PdfPTable(new float[] { 15, 75 });
				ptableLocation.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Location :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(images[l].Location ?? ""));	// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(ptableLocation);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable.AddCell(cellp);


				PdfPTable ptabledes = new PdfPTable(new float[] { 15, 75 });
				ptabledes.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Description :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(images[l].Description ?? ""));		// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(ptabledes);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable.AddCell(cellp);

				if (images[l].ImageName != null) {

					//imgPath = HttpContext.Current.Server.MapPath("~/LeadsImage/" + images[l].LeadId + "/" + images[l].LeadImageId + "/" + images[l].ImageName);
					// 2014-01-08
					imgPath = appPath + "/LeadsImage/" + images[l].LeadId + "/" + images[l].LeadImageId + "/" + images[l].ImageName;

					if (File.Exists(imgPath)) {
						img = iTextSharp.text.Image.GetInstance(imgPath);
						//img.ScaleToFit(5000, 200);
						img.ScaleToFit(300f, 200f);
						cellp = new PdfPCell(img, false);
						cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
						cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
						cellp.PaddingTop = 2;
						cellp.PaddingBottom = 2;
						cellp.PaddingRight = 2;
						cellp.Border = 0;
						ptable.AddCell(cellp);
					}
					else {
						cellp = new PdfPCell(new Phrase("."));
						cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
						cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
						cellp.PaddingTop = 2;
						cellp.PaddingBottom = 2;
						cellp.PaddingRight = 2;
						cellp.Border = 0;
						((Chunk)(cellp.Phrase[0])).Font = white9Font;
						ptable.AddCell(cellp);
					}



					PdfPTable ptableInner = new PdfPTable(new float[] { 90 });
					ptableInner.WidthPercentage = 90;

					cellp = new PdfPCell(new Phrase("Picture"));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					((Chunk)(cellp.Phrase[0])).Font = black9BoldFont;
					ptableInner.AddCell(cellp);

					// increase picture number
					cellp = new PdfPCell(new Phrase((p).ToString()));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);

					cellp = new PdfPCell(new Phrase("Date"));
					cellp.Border = 0;
					((Chunk)(cellp.Phrase[0])).Font = black9BoldFont;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);

					cellp = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy")));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);


					cellp = new PdfPCell(ptableInner);
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.Border = 0;
					cellp.PaddingLeft = 5;
					ptable.AddCell(cellp);
				}

				if (p % 2 == 0) {
					doc.Add(ptable);

					doc.NewPage();

					ptable = new PdfPTable(new float[] { 60, 30 });

					ptable.WidthPercentage = 90;
				}

				++p;

			} // for

			//if (p % 2 > 0) {
			doc.Add(ptable);


		}
		static protected void addPhotos(List<ClaimImage> images, string title) {
			if (images == null || images.Count == 0)
				return;

			string imgPath = null;

			PdfPTable ptable = null;

			// create header		

			doc.NewPage();

			PdfPTable tblHeader = new PdfPTable(new float[] { 60, 30 });
			tblHeader.WidthPercentage = 90;

			PdfPCell headerCell = new PdfPCell(new Phrase(title + " Photos"));
			headerCell.Colspan = 2;
			headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			tblHeader.AddCell(headerCell);
			doc.Add(tblHeader);

			// picture counter
			int p = 1;

			PdfPCell cellp = new PdfPCell(new Phrase("."));
			iTextSharp.text.Image img = null;

			for (int l = 0; l < images.Count; l++) {

				if (p == 1) {
					// first page
					ptable = new PdfPTable(new float[] { 60, 30 });
					ptable.WidthPercentage = 90;
				}

				cellp = new PdfPCell(new Phrase("."));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.Border = 0;
				cellp.Colspan = 2;
				cellp.PaddingTop = 10;
				((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
				ptable.AddCell(cellp);


				PdfPTable ptableLocation = new PdfPTable(new float[] { 15, 75 });
				ptableLocation.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Location :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(images[l].Location ?? ""));	// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(ptableLocation);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable.AddCell(cellp);


				PdfPTable ptabledes = new PdfPTable(new float[] { 15, 75 });
				ptabledes.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Description :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(images[l].Description ?? ""));		// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(ptabledes);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable.AddCell(cellp);

				if (images[l].ImageName != null) {

					//imgPath = HttpContext.Current.Server.MapPath("~/LeadsImage/" + images[l].LeadId + "/" + images[l].LeadImageId + "/" + images[l].ImageName);
					// 2014-01-08
					imgPath = appPath + "/ClaimImage/" + images[l].ClaimID + "/" + images[l].ClaimImageID + "/" + images[l].ImageName;

					if (File.Exists(imgPath)) {
						img = iTextSharp.text.Image.GetInstance(imgPath);
						//img.ScaleToFit(5000, 200);
						img.ScaleToFit(300f, 200f);
						cellp = new PdfPCell(img, false);
						cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
						cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
						cellp.PaddingTop = 2;
						cellp.PaddingBottom = 2;
						cellp.PaddingRight = 2;
						cellp.Border = 0;
						ptable.AddCell(cellp);
					}
					else {
						cellp = new PdfPCell(new Phrase("."));
						cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
						cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
						cellp.PaddingTop = 2;
						cellp.PaddingBottom = 2;
						cellp.PaddingRight = 2;
						cellp.Border = 0;
						((Chunk)(cellp.Phrase[0])).Font = white9Font;
						ptable.AddCell(cellp);
					}



					PdfPTable ptableInner = new PdfPTable(new float[] { 90 });
					ptableInner.WidthPercentage = 90;

					cellp = new PdfPCell(new Phrase("Picture"));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					((Chunk)(cellp.Phrase[0])).Font = black9BoldFont;
					ptableInner.AddCell(cellp);

					// increase picture number
					cellp = new PdfPCell(new Phrase((p).ToString()));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);

					cellp = new PdfPCell(new Phrase("Date"));
					cellp.Border = 0;
					((Chunk)(cellp.Phrase[0])).Font = black9BoldFont;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);

					cellp = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy")));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);


					cellp = new PdfPCell(ptableInner);
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.Border = 0;
					cellp.PaddingLeft = 5;
					ptable.AddCell(cellp);
				}

				if (p % 2 == 0) {
					doc.Add(ptable);

					doc.NewPage();

					ptable = new PdfPTable(new float[] { 60, 30 });

					ptable.WidthPercentage = 90;
				}

				++p;

			} // for

			//if (p % 2 > 0) {
			doc.Add(ptable);


		}

		#endregion

		#region logo and client address

		static protected void setClientLogoHeader(Client client) {
			string logopath = null;

			if (client != null) {
				logopath = string.Format("{0}/ClientLogo/{1}.jpg", appPath, client.ClientId);

				//logopath = HttpContext.Current.Server.MapPath(lpath);
			}
			else {
				client = new Client();
				//logopath = HttpContext.Current.Server.MapPath("~/Images/claim_ruler_logo.jpg");
				logopath = appPath + "/Images/claim_ruler_logo.jpg";
			}

			iTextSharp.text.Image img = null;

			PdfPCell cellp = new PdfPCell(new Phrase("."));

			PdfPTable ptableAddress = new PdfPTable(new float[] { 90 });
			ptableAddress.WidthPercentage = 90;
			ptableAddress.HorizontalAlignment = 0;

			if (File.Exists(logopath)) {
				img = iTextSharp.text.Image.GetInstance(logopath);
				img.ScaleToFit(120, 100);
				cellp = new PdfPCell(img, false);
				cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.PaddingLeft = 10;
				cellp.PaddingTop = 20;
				cellp.Border = 0;
				ptableAddress.AddCell(cellp);
			}

			// business name
			if (!string.IsNullOrEmpty(client.BusinessName)) {
				cellp = new PdfPCell(new Phrase(client.BusinessName ?? " "));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.Border = 0;
				cellp.PaddingLeft = 10;
				((Chunk)(cellp.Phrase[0])).Font = black9Font;
				ptableAddress.AddCell(cellp);

			}

			// client address 1/address 1
			string address1 = string.Format("{0} {1}", client.StreetAddress1 ?? " ", client.StreetAddress2 ?? " ");

			cellp = new PdfPCell(new Phrase(address1));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			// client city/state/zip
			string city = client.CityMaster == null ? " " : client.CityMaster.CityName;
			string state = client.StateMaster == null ? " " : client.StateMaster.StateCode;

			string cityStateZip = string.Format("{0}, {1} {2}", city, state, client.ZipCode ?? " ");

			cellp = new PdfPCell(new Phrase(cityStateZip));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			// client phone
			cellp = new PdfPCell(new Phrase(client.PrimaryPhoneNo ?? " "));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			cellp = new PdfPCell(new Phrase(client.SecondaryEmailId ?? " "));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);


			//cellp = new PdfPCell(new Phrase("."));
			//cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			//cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			//cellp.Border = 0;
			//cellp.PaddingLeft = 10;
			//((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
			//ptableAddress.AddCell(cellp);

			//cellp = new PdfPCell(new Phrase("."));
			//cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			//cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			//cellp.Border = 0;
			//cellp.PaddingLeft = 10;
			//((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
			//ptableAddress.AddCell(cellp);

			cellp = new PdfPCell(new Phrase("Claim Export Report"));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			cellp = new PdfPCell(new Phrase("."));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
			ptableAddress.AddCell(cellp);

			//cellp = new PdfPCell(new Phrase("--------------------------------------------------------------------------------------------------"));
			//cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			//cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			//cellp.Border = 0;
			//cellp.PaddingLeft = 10;
			//((Chunk)(cellp.Phrase[0])).Font = black9Font;
			//ptableAddress.AddCell(cellp);

			//cellp = new PdfPCell(new Phrase("."));
			//cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			//cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			//cellp.Border = 0;
			//cellp.PaddingLeft = 10;
			//((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
			//ptableAddress.AddCell(cellp);

			doc.Add(ptableAddress);
		}

		#endregion

		#region coverage

		static protected void exportCoverage(LeadPolicy policy) {
			string coverageHeder = string.Empty;
			int policyTypeID = (int)policy.PolicyType;

			LeadPolicyType policyType = null;

			// create header
			policyType = LeadPolicyTypeManager.Get(policyTypeID);

			if (policyType != null)
				coverageHeder = policyType.Description + " - Coverages";

			doc.NewPage();

			PdfPTable tblHeader = new PdfPTable(new float[] { 30, 60 });
			tblHeader.WidthPercentage = 90;

			PdfPCell headerCell = new PdfPCell(new Phrase(coverageHeder));
			headerCell.Colspan = 2;
			headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			tblHeader.AddCell(headerCell);
			doc.Add(tblHeader);

			PdfPTable ptable = new PdfPTable(new float[] { 30, 60 });
			string typeDescripion = "";
			string cityName = null;
			string stateName = null;
			string zipCode = null;
			string inspectionCompleted = null;

			//foreach (LeadPolicy policy in policies) {

			if (!string.IsNullOrEmpty(policy.InsuranceCompanyName)) {
				addTableCell(ptable, "Insurance Type");
				if (policy.PolicyType != null)
					typeDescripion = policy.LeadPolicyType.Description;

				addTableCell(ptable, typeDescripion);

				addTableCell(ptable, "Insurance Company");
				addTableCell(ptable, policy.InsuranceCompanyName);

				addTableCell(ptable, "Address");
				addTableCell(ptable, policy.InsuranceAddress ?? " ");

				addTableCell(ptable, " ");
				cityName = policy.CityMaster == null ? " " : policy.CityMaster.CityName;
				stateName = policy.StateMaster == null ? " " : policy.StateMaster.StateCode;
				zipCode = policy.InsuranceZipCode ?? " ";

				addTableCell(ptable, cityName + " " + stateName + " " + zipCode);

				addTableCell(ptable, "Policy Number");
				addTableCell(ptable, policy.PolicyNumber ?? " ");

				//addTableCell(ptable, "Claim Number");
				//addTableCell(ptable, policy.ClaimNumber ?? " ");

				//addTableCell(ptable, "Status");
				//addTableCell(ptable, policy.StateMaster == null ? " " : policy.StateMaster.StateName);

				//addTableCell(ptable, "SubStatus");
				//addTableCell(ptable, policy.SubStatusMaster == null ? " " : policy.SubStatusMaster.SubStatusName);

				//addTableCell(ptable, "Site Survey Date");
				//addTableCell(ptable, policy.SiteSurveyDate == null ? " " : Convert.ToDateTime(policy.SiteSurveyDate).ToShortDateString());

				//addTableCell(ptable, "Site Inspection Complete");
				//if (policy.SiteInspectionCompleted == null)
				//	inspectionCompleted = "No";
				//else
				//	inspectionCompleted = policy.SiteInspectionCompleted == 1 ? "Yes" : "No";

				addTableCell(ptable, inspectionCompleted);

				// blank row
				addTableCell(ptable, " ");
				addTableCell(ptable, " ");
			}

			//}
			doc.Add(ptable);
		}

		static private void addTableCell(PdfPTable ptable, string text) {
			// put text inside cell
			PdfPCell cellp = new PdfPCell(new Phrase(text ?? " ", black9Font));

			cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
			cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			//((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptable.AddCell(cellp);
		}
		#endregion

		#region documents

		static protected string mergeDocuments(string sourcepath, string destinationPath, int leadID, int claimID) {
			List<LeadsDocument> documents = null;
			List<ClaimDocument> claimDocuments = null;
			List<string> pdfs = new List<string>();

			string mergedReportPath = null;

			documents = LeadsUploadManager.getLeadsDocumentForExportByLeadID(leadID);
			claimDocuments = ClaimDocumentManager.GetAll(claimID);

			// add original document to list
			pdfs.Insert(0, sourcepath);

			// lead documents
			if (documents != null && documents.Count > 0) {

				List<string> leadPDFs = (from x in documents
								 where x.DocumentName.Contains(".pdf")
								 select string.Format("{0}/LeadsDocument/{1}/{2}/{3}", appPath, x.LeadId, x.LeadDocumentId, x.DocumentName)
							  ).ToList();

				foreach(string pdf in leadPDFs) {
					pdfs.Add(pdf);
				}
			}

			// claim documents
			if (claimDocuments != null && claimDocuments.Count > 0) {

				List<string> claimPDFs = (from x in claimDocuments
									where x.DocumentName.Contains(".pdf")
									select string.Format("{0}/ClaimDocuments/{1}/{2}/{3}", appPath, x.ClaimID, x.ClaimDocumentID, x.DocumentName)
							  ).ToList();

				foreach (string pdf in claimPDFs) {
					pdfs.Add(pdf);
				}
			}

			// mergedReportPath = Path.GetDirectoryName(sourcepath) + "\\" + Guid.NewGuid().ToString() + ".pdf";
			// mergePDFFiles(mergedReportPath, pdfs.ToArray());

			mergePDFFiles(destinationPath, pdfs.ToArray());

			return destinationPath;
		}

	
		static protected void mergePDFFiles(string destinationFile, string[] sourceFiles) {
			// source: http://stackoverflow.com/questions/6196124/merge-pdf-files

			int f = 0;
			String outFile = destinationFile;
			Document document = null;
			PdfCopy writer = null;
			string filePath = null;

			while (f < sourceFiles.Length) {
				// build pdf path
				filePath = sourceFiles[f];

				// Create a reader for a certain document
				if (File.Exists(filePath)) {
					PdfReader reader = new PdfReader(filePath);

					// Retrieve the total number of pages
					int n = reader.NumberOfPages;

					if (f == 0) {
						// Step 1: Creation of a document-object
						document = new Document(reader.GetPageSizeWithRotation(1));
						// Step 2: Create a writer that listens to the document
						writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
						// Step 3: Open the document
						document.Open();
					}

					// Step 4: Add content
					PdfImportedPage page;
					for (int i = 0; i < n; ) {
						++i;
						page = writer.GetImportedPage(reader, i);
						writer.AddPage(page);
					}
					PRAcroForm form = reader.AcroForm;
					if (form != null)
						writer.CopyAcroForm(reader);
				}

				++f;
			}
			// Step 5: Close the document
			document.Close();

			// dispose
			document.Dispose();
			document = null;
		}
		#endregion
	}
}
