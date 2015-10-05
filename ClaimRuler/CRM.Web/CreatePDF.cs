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
using System.Transactions;
using LinqKit;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CRM.Data.Entities;


namespace CRM.Web {


	public class CreatePDF {

		public static string CreateAndGetPDF(int claimID, string PDFFolderPath, out string ErrorMessage) {
			string filename = string.Empty;
			ErrorMessage = string.Empty;
			int res = 0;
			int PDFVersion = 0;
			try {
				res = GeneratePDF(claimID, PDFFolderPath, out PDFVersion);

				return 1 + "-V" + PDFVersion.ToString() + ".pdf";
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}

			return filename;
		}

		protected static int GeneratePDF(int claimID, string PDFFolderPath, out int PDFVersion) {
			PDFVersion = 0;
			Document doc = new Document(iTextSharp.text.PageSize.LETTER, 0, 0, 36, 36);

			PDFPageEventHelper pageEventHandler = null;
			string footerText = null;

			Claim claim = null;
			Leads lead = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			List<LeadsImage> objleadsimage = null;
			List<ClaimImage> claimImages = null;

			try {
				string ErrorMessage = string.Empty;

				string Occasion = string.Empty;
				string EmployeeName = string.Empty;
				//string imagesFolder = "LeadsImage/";

				string NewProposalDir = PDFFolderPath;
				if (!Directory.Exists(NewProposalDir)) {
					Directory.CreateDirectory(NewProposalDir);
				}


				// Get claim
				claim = ClaimsManager.Get(claimID);

				lead = claim.LeadPolicy.Leads;
				policy = claim.LeadPolicy;

				string applicationpath = PDFFolderPath.Substring(0, PDFFolderPath.LastIndexOf("PDF"));

				BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
				BaseColor CellBackgroundColor = BaseColor.BLACK;
				BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
				BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
				//float CellHeight = 17;
				BaseColor BorderColor = BaseColor.BLACK;


				PdfWriter w = PdfWriter.GetInstance(doc, new FileStream(PDFFolderPath + "" + claimID + ".pdf", FileMode.Create));

				//footerText = string.Format("{0} {1} {2}", lead.ClaimantFirstName ?? "", lead.ClaimantMiddleName ?? "", lead.ClaimantLastName ?? "");

				pageEventHandler = new PDFPageEventHelper(footerText);

				w.PageEvent = pageEventHandler;

				Rectangle rect = new Rectangle(iTextSharp.text.PageSize.LETTER.Width, iTextSharp.text.PageSize.LETTER.Height);
				rect.BackgroundColor = BaseColor.WHITE;
				doc.SetPageSize(rect);
				doc.Open();


				#region Fonts

				float fontsize = 10f;
				Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
				Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
				Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
				Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
				Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
				Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
				Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
				Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
				Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
				Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
				Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
				Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
				Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
				Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
				Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
				Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
				Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
				Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
				Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
				Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
				Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
				Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
				Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
				Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
				Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
				#endregion

				PdfPCell cellp = new PdfPCell(new Phrase("."));


				// PRINT CLIENT HEADER
				PdfPTable clientHeader = new PdfPTable(new float[] { 90 });

				printReporHeader(clientHeader, applicationpath);
				
				doc.Add(clientHeader);

		
				// PRINT POLICY IMAGES
				objleadsimage = LeadsUploadManager.getLeadsImageByLeadID(lead.LeadId, (int)policy.PolicyType);

				if (objleadsimage != null && objleadsimage.Count > 0)
					printImages(applicationpath, claim, objleadsimage, doc);

				// PRINT CLAIM IMAGES
				claimImages = CRM.Repository.ClaimImageManager.GetAll(claimID);

				if (claimImages != null && claimImages.Count > 0)
					printImages(applicationpath, claim, claimImages, doc);

				doc.NewPage();

				return 1;
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}
			finally {
				doc.Close();
			}

			return 0;
		}

		private static string getClaimantAddress(Leads lead) {
			string fulladdress = null;
							
			fulladdress = string.Format("{0}, {1}, {2} {3}", lead.LossAddress ?? " ", lead.CityName ?? " ", lead.StateName ?? " ", lead.Zip ?? " ");

			return fulladdress;
		}


		private static void printPolicyHeader(PdfPTable pdftable, string headerPrompt, string headerValue) {
			PdfPCell cellp = null;

			try {
				#region Fonts
				BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
				BaseColor CellBackgroundColor = BaseColor.BLACK;
				BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
				BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
				float CellHeight = 17;
				BaseColor BorderColor = BaseColor.BLACK;

				float fontsize = 10f;
				Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
				Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
				Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
				Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
				Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
				Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
				Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
				Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
				Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
				Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
				Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
				Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
				Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
				Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
				Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
				Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
				Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
				Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
				Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
				Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
				Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
				Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
				Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
				Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
				Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
				#endregion

				cellp = new PdfPCell(new Phrase(headerPrompt ?? " "));
				cellp.Border = 0;
				cellp.PaddingTop = 5;
				//cellp.PaddingLeft = 32;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				pdftable.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(headerValue ?? " "));
				cellp.Border = 0;
				cellp.PaddingTop = 5;
				cellp.Colspan = 2;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black9Font;
				pdftable.AddCell(cellp);
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}
		}

		private static void printImages(string applicationpath, Claim claim, List<LeadsImage> objleadsimage, Document doc) {
			string imageDate = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			Leads lead = null;


			#region Fonts
			BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
			BaseColor CellBackgroundColor = BaseColor.BLACK;
			BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
			BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
			float CellHeight = 17;
			BaseColor BorderColor = BaseColor.BLACK;

			float fontsize = 10f;
			Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
			Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
			Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
			Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
			Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
			Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
			Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
			Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
			Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
			Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
			Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
			Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
			Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
			Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
			Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
			Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
			Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
			Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
			Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
			Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
			Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
			Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
			Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
			#endregion


			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			PdfPTable ptable3 = null;
			PdfPCell cellp = null;
			string imgPath = null;
			iTextSharp.text.Image img = null;

			lead = claim.LeadPolicy.Leads;
			policy = claim.LeadPolicy;

			// picture counter
			int p = 1;


			for (int l = 0; l < objleadsimage.Count; l++) {

				if (p == 1) {
					// first page
					PdfPTable policyHeaderTable = new PdfPTable(new float[] { 30, 70 });
					policyHeaderTable.WidthPercentage = 90;

					// claimant name
					printPolicyHeader(policyHeaderTable, "Policy Holder Name: ", lead.InsuredName ?? " ");

					printPolicyHeader(policyHeaderTable, "Policy Holder Address: ", getClaimantAddress(lead));

					// policy header
					printPolicyHeader(policyHeaderTable, "Insurance Company: ", policy.InsuranceCompanyName ?? "N/A");

					printPolicyHeader(policyHeaderTable, "Policy Number: ", policy.PolicyNumber ?? "N/A");

					printPolicyHeader(policyHeaderTable, "Claim Number: ", claim.AdjusterClaimNumber ?? "N/A");

					if (claim.LossDate != null)
						printPolicyHeader(policyHeaderTable, "Loss Date: ", string.Format("{0:MM/dd/yyyy}", claim.LossDate));


					doc.Add(policyHeaderTable);

					ptable3 = new PdfPTable(new float[] { 60, 30 });
					ptable3.WidthPercentage = 90;
				}
				else if (p % 2 == 0) {
					doc.Add(ptable3);

					doc.NewPage();

					ptable3 = new PdfPTable(new float[] { 60, 30 });

					ptable3.WidthPercentage = 90;
				}


				cellp = new PdfPCell(new Phrase("."));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.Border = 0;
				cellp.Colspan = 2;
				cellp.PaddingTop = 5;
				((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
				ptable3.AddCell(cellp);


				PdfPTable ptableLocation = new PdfPTable(new float[] { 15, 75 });
				ptableLocation.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Location :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(objleadsimage[l].Location ?? " "));	// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(ptableLocation);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable3.AddCell(cellp);


				PdfPTable ptabledes = new PdfPTable(new float[] { 15, 75 });
				ptabledes.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Description :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(objleadsimage[l].Description ?? " "));		// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(ptabledes);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable3.AddCell(cellp);

				if (objleadsimage[l].ImageName != null && objleadsimage[l].ImageName.Trim().Length > 0) {
					#region image 1

					imgPath = appPath + "/LeadsImage/" + objleadsimage[l].LeadId + "/" + objleadsimage[l].LeadImageId + "/" + objleadsimage[l].ImageName;

					if (File.Exists(imgPath)) {
						img = iTextSharp.text.Image.GetInstance(imgPath);
						//img.ScaleToFit(5000, 200);
						img.ScaleToFit(350f, 250f);
						cellp = new PdfPCell(img, false);
						cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
						cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
						cellp.PaddingTop = 2;
						cellp.PaddingBottom = 2;
						cellp.PaddingRight = 2;
						cellp.Border = 0;
						ptable3.AddCell(cellp);
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
						ptable3.AddCell(cellp);
					}



					PdfPTable ptableInner = new PdfPTable(new float[] { 90 });
					ptableInner.WidthPercentage = 90;

					cellp = new PdfPCell(new Phrase("Picture"));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					((Chunk)(cellp.Phrase[0])).Font = black9BoldFont;
					ptableInner.AddCell(cellp);

					cellp = new PdfPCell(new Phrase((p++).ToString()));
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

					//cellp = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy")));
					imageDate = string.Format("{0:dd/MM/yyyy}", objleadsimage[l].InsertDate);

					cellp = new PdfPCell(new Phrase(imageDate));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);


					cellp = new PdfPCell(ptableInner);
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.Border = 0;
					cellp.PaddingLeft = 5;
					ptable3.AddCell(cellp);
					#endregion image 1
				}

				


			}

			doc.Add(ptable3);
			doc.NewPage();
		}

		private static void printImages(string applicationpath, Claim claim, List<ClaimImage> objleadsimage, Document doc) {
			string imageDate = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			Leads lead = null;


			#region Fonts
			BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
			BaseColor CellBackgroundColor = BaseColor.BLACK;
			BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
			BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
			float CellHeight = 17;
			BaseColor BorderColor = BaseColor.BLACK;

			float fontsize = 10f;
			Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
			Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
			Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
			Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
			Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
			Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
			Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
			Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
			Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
			Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
			Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
			Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
			Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
			Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
			Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
			Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
			Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
			Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
			Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
			Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
			Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
			Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
			Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
			#endregion


			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			PdfPTable ptable3 = null;
			PdfPCell cellp = null;
			string imgPath = null;
			iTextSharp.text.Image img = null;

			lead = claim.LeadPolicy.Leads;
			policy = claim.LeadPolicy;

			// picture counter
			int p = 1;


			for (int l = 0; l < objleadsimage.Count; l++) {

				if (p == 1) {
					// first page
					PdfPTable policyHeaderTable = new PdfPTable(new float[] { 30, 70 });
					policyHeaderTable.WidthPercentage = 90;

					// claimant name
					printPolicyHeader(policyHeaderTable, "Policy Holder Name: ", lead.InsuredName);

					printPolicyHeader(policyHeaderTable, "Policy Holder Address: ", getClaimantAddress(lead));

					// policy header
					if (!string.IsNullOrEmpty(policy.InsuranceCompanyName))
						printPolicyHeader(policyHeaderTable, "Insurance Company: ", policy.InsuranceCompanyName);

					if (!string.IsNullOrEmpty(policy.PolicyNumber))
						printPolicyHeader(policyHeaderTable, "Policy Number: ", policy.PolicyNumber);

					if (!string.IsNullOrEmpty(claim.AdjusterClaimNumber))
						printPolicyHeader(policyHeaderTable, "Claim Number: ", claim.AdjusterClaimNumber ?? " ");

					if (claim.LossDate != null)
						printPolicyHeader(policyHeaderTable, "Loss Date: ", string.Format("{0:MM/dd/yyyy}", claim.LossDate));


					doc.Add(policyHeaderTable);

					ptable3 = new PdfPTable(new float[] { 60, 30 });
					ptable3.WidthPercentage = 90;
				}
				else if (p % 2 == 0) {
					doc.Add(ptable3);

					doc.NewPage();

					ptable3 = new PdfPTable(new float[] { 60, 30 });

					ptable3.WidthPercentage = 90;
				}


				cellp = new PdfPCell(new Phrase("."));
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.Border = 0;
				cellp.Colspan = 2;
				cellp.PaddingTop = 5;
				((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
				ptable3.AddCell(cellp);


				PdfPTable ptableLocation = new PdfPTable(new float[] { 15, 75 });
				ptableLocation.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Location :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(objleadsimage[l].Location ?? " "));	// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptableLocation.AddCell(cellp);

				cellp = new PdfPCell(ptableLocation);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable3.AddCell(cellp);


				PdfPTable ptabledes = new PdfPTable(new float[] { 15, 75 });
				ptabledes.WidthPercentage = 90;

				cellp = new PdfPCell(new Phrase("Description :"));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(objleadsimage[l].Description ?? " "));		// 2013-05-07 tortega
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				ptabledes.AddCell(cellp);

				cellp = new PdfPCell(ptabledes);
				cellp.PaddingTop = 5;
				cellp.Border = 0;
				cellp.Colspan = 2;
				ptable3.AddCell(cellp);

				if (objleadsimage[l].ImageName != null && objleadsimage[l].ImageName.Trim().Length > 0) {
					#region image 1

					imgPath = appPath + "/ClaimImage/" + objleadsimage[l].ClaimID + "/" + objleadsimage[l].ClaimImageID + "/" + objleadsimage[l].ImageName;

					if (File.Exists(imgPath)) {
						img = iTextSharp.text.Image.GetInstance(imgPath);
						//img.ScaleToFit(5000, 200);
						img.ScaleToFit(350f, 250f);
						cellp = new PdfPCell(img, false);
						cellp.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
						cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
						cellp.PaddingTop = 2;
						cellp.PaddingBottom = 2;
						cellp.PaddingRight = 2;
						cellp.Border = 0;
						ptable3.AddCell(cellp);
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
						ptable3.AddCell(cellp);
					}



					PdfPTable ptableInner = new PdfPTable(new float[] { 90 });
					ptableInner.WidthPercentage = 90;

					cellp = new PdfPCell(new Phrase("Picture"));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					((Chunk)(cellp.Phrase[0])).Font = black9BoldFont;
					ptableInner.AddCell(cellp);

					cellp = new PdfPCell(new Phrase((p++).ToString()));
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

					imageDate = string.Format("{0:dd/MM/yyyy}", objleadsimage[l].ImageDate);

					cellp = new PdfPCell(new Phrase(imageDate));
					cellp.Border = 0;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					ptableInner.AddCell(cellp);


					cellp = new PdfPCell(ptableInner);
					cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
					cellp.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
					cellp.Border = 0;
					cellp.PaddingLeft = 5;
					ptable3.AddCell(cellp);
					#endregion image 1
				}




			}

			doc.Add(ptable3);
			doc.NewPage();
		}

		private static void printClaimantInfo(PdfPTable ptableAdditional, int LeadId) {
			PdfPCell cellp = new PdfPCell(new Phrase("."));

			#region Fonts
			BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
			BaseColor CellBackgroundColor = BaseColor.BLACK;
			BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
			BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
			float CellHeight = 17;
			BaseColor BorderColor = BaseColor.BLACK;

			float fontsize = 10f;
			Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
			Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
			Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
			Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
			Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
			Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
			Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
			Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
			Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
			Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
			Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
			Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
			Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
			Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
			Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
			Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
			Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
			Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
			Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
			Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
			Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
			Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
			Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
			#endregion

			ptableAdditional.WidthPercentage = 90;

			var _leads = LeadsManager.GetByLeadId(LeadId);

			string _firstName = _leads.ClaimantFirstName == null ? " " : _leads.ClaimantFirstName.ToString();
			string _lastName = _leads.ClaimantLastName == null ? " " : _leads.ClaimantLastName.ToString();
			
			string Claimant_Name = _leads.InsuredName;

			string BusinessName = _leads.BusinessName == null ? " " : _leads.BusinessName.ToString();
			string ClaimantAddress = _leads.LossAddress == null ? " " : _leads.LossAddress.ToString();
			string City = _leads.CityMaster == null ? " " : _leads.CityMaster.CityName;
		
			string State = string.Empty;
			if (_leads.StateMaster != null) {
				State = _leads.StateMaster.StateCode == null ? " " : _leads.StateMaster.StateCode.ToString();
			}
			string zip = _leads.Zip == null ? " " : _leads.Zip.ToString();
			string fulladdress = ClaimantAddress + ',' + City + ',' + State + ',' + zip;

			cellp = new PdfPCell(new Phrase("Claimant Name: "));
			cellp.Border = 0;
			cellp.PaddingLeft = 32;
			cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
			cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
			((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
			ptableAdditional.AddCell(cellp);

			cellp = new PdfPCell(new Phrase(Claimant_Name));
			cellp.Border = 0;
			cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
			cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAdditional.AddCell(cellp);

			if (BusinessName != string.Empty) {
				cellp = new PdfPCell(new Phrase("Business Name: "));
				cellp.Border = 0;
				cellp.PaddingLeft = 32;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
				ptableAdditional.AddCell(cellp);

				cellp = new PdfPCell(new Phrase(BusinessName));
				cellp.Border = 0;
				cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
				cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
				((Chunk)(cellp.Phrase[0])).Font = black9Font;
				ptableAdditional.AddCell(cellp);
			}

			cellp = new PdfPCell(new Phrase("Claimant Address: "));
			cellp.Border = 0;
			cellp.PaddingLeft = 32;
			cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
			cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
			((Chunk)(cellp.Phrase[0])).Font = black12FontBold;
			ptableAdditional.AddCell(cellp);

			cellp = new PdfPCell(new Phrase(fulladdress));
			cellp.Border = 0;
			cellp.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
			cellp.VerticalAlignment = PdfPCell.ALIGN_TOP;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAdditional.AddCell(cellp);

			cellp = new PdfPCell(ptableAdditional);
			cellp.Border = 0;
			ptableAdditional.AddCell(cellp);

		}

		private static void printReporHeader(PdfPTable ptableAddress, string applicationpath) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			Client client = null;
			int clientID = Core.SessionHelper.getClientId();

			ptableAddress.WidthPercentage = 90;
			ptableAddress.HorizontalAlignment = 0;
			string imgPath = string.Empty;
			iTextSharp.text.Image img = null;

			PdfPCell cellp = new PdfPCell(new Phrase("."));

			#region Fonts

			BaseColor DocumentBackgroundColor = new BaseColor(System.Drawing.Color.FromArgb(14410214));
			BaseColor CellBackgroundColor = BaseColor.BLACK;
			BaseColor MessageBackgound = new BaseColor(System.Drawing.Color.FromArgb(231232234));
			BaseColor EmployeeBackground = new BaseColor(System.Drawing.Color.FromArgb(022022946));
			float CellHeight = 17;
			BaseColor BorderColor = BaseColor.BLACK;

			float fontsize = 10f;
			Font linkFont = FontFactory.GetFont("swiss721 BT", fontsize, Font.NORMAL, BaseColor.BLUE);
			Font black1Font = FontFactory.GetFont("swis721 cn BT", 1f, Font.NORMAL, BaseColor.BLACK);
			Font black9Font = FontFactory.GetFont("swis721 cn BT", 9f, Font.NORMAL, BaseColor.BLACK);
			Font black8Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
			Font black11BoldFont = FontFactory.GetFont("swis721 cn BT", 11f, Font.BOLD, BaseColor.BLACK);
			Font black16BoldFont = FontFactory.GetFont("swis721 cn BT", 16.25f, Font.BOLD, BaseColor.BLACK);
			Font black10BoldFont = FontFactory.GetFont("swis721 cn BT", 10f, Font.BOLD, BaseColor.BLACK);
			Font white15BoldFont = FontFactory.GetFont("swis721 cn BT", 15f, Font.BOLD, BaseColor.WHITE);
			Font white12BoldFont = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.WHITE);
			Font white5Font = FontFactory.GetFont("swis721 cn BT", 5f, Font.NORMAL, BaseColor.WHITE);
			Font black12Font = FontFactory.GetFont("swis721 cn BT", 12f, Font.NORMAL, BaseColor.BLACK);
			Font white9Font = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, BaseColor.WHITE);
			Font black30Font = FontFactory.GetFont("swis721 cn BT", 30f, Font.NORMAL, BaseColor.BLACK);
			Font black25Font = FontFactory.GetFont("swis721 cn BT", 25f, Font.NORMAL, BaseColor.BLACK);
			Font transparentNormalFont = FontFactory.GetFont("swiss721 cn BT", 9.5f, Font.NORMAL, DocumentBackgroundColor);
			Font transparent5Font = FontFactory.GetFont("swiss721 cn BT", 3f, Font.NORMAL, DocumentBackgroundColor);
			Font black913Font = FontFactory.GetFont("swis721 cn BT", 8f, Font.NORMAL, BaseColor.BLACK);
			Font white913Font = FontFactory.GetFont("swiss721 cn BT", 7.5f, Font.NORMAL, BaseColor.WHITE);
			Font black12FontBold = FontFactory.GetFont("swis721 cn BT", 12f, Font.BOLD, BaseColor.BLACK);
			Font black8FontBold = FontFactory.GetFont("swis721 cn BT", 8f, Font.BOLD, BaseColor.BLACK);
			Font black6Font = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.BLACK);
			Font red = FontFactory.GetFont("swis721 cn BT", 6f, Font.NORMAL, BaseColor.RED);
			Font white28BoldFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.BOLD, BaseColor.WHITE);
			Font white28NormalFont = FontFactory.GetFont("swis721 cn BT", 22f, Font.NORMAL, BaseColor.WHITE);
			#endregion


			if (HttpContext.Current.Session["ClientId"] == null) {
				imgPath = appPath + "/Images/claim_ruler_logo.jpg";
			}
			else {
				imgPath = string.Format("{0}/Images/ClientLogo/{1}.jpg", appPath, HttpContext.Current.Session["ClientId"]);
			}

			if (File.Exists(imgPath)) {

				img = iTextSharp.text.Image.GetInstance(imgPath);
				img.ScaleToFit(180, 130);
				cellp = new PdfPCell(img, false);
				cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
				cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
				cellp.PaddingLeft = 10;
				cellp.PaddingTop = 20;
				cellp.Border = 0;
				ptableAddress.AddCell(cellp);
			}

			client = ClientManager.Get(clientID);

			string companyName = client.BusinessName ?? " ";

			cellp = new PdfPCell(new Phrase(companyName));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			cellp = new PdfPCell(new Phrase(string.Format("{0} {1}", client.StreetAddress1 ?? " ", client.StreetAddress2 ?? " ")));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			string cityStateZip = client.CityMaster.CityName + ", " + client.StateMaster.StateName + " " + client.ZipCode;

			cellp = new PdfPCell(new Phrase(cityStateZip));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			cellp = new PdfPCell(new Phrase(client.PrimaryPhoneNo ?? " "));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = black9Font;
			ptableAddress.AddCell(cellp);

			cellp = new PdfPCell(new Phrase(client.PrimaryEmailId ?? " "));
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

			cellp = new PdfPCell(new Phrase("."));
			cellp.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
			cellp.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
			cellp.Border = 0;
			cellp.PaddingLeft = 10;
			((Chunk)(cellp.Phrase[0])).Font = transparentNormalFont;
			ptableAddress.AddCell(cellp);

		}
	}
}