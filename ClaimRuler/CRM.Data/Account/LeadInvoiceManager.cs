using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CRM.Data.Entities;
using LinqKit;
using System.Linq.Expressions;

namespace CRM.Data.Account {
	static public class LeadInvoiceManager {
		#region Invoice methods
		static public IQueryable<LeadInvoice> GetInvoices(int leadID) {

			IQueryable<LeadInvoice> invoices = from x in DbContextHelper.DbContext
								   .LeadInvoice
								   .Include("LeadInvoiceDetail")
								   .Include("LeadInvoiceDetail.InvoiceServiceType")
										where x.LeadId == leadID //&& (x.isVoid ?? false) == false
										orderby x.InvoiceDate
										select x;

			return invoices;
		}

		static public LeadInvoice Get(int invoiceID) {

			LeadInvoice invoice = (from x in DbContextHelper.DbContext
								   .LeadInvoice
								   .Include("LeadInvoiceDetail")
								   .Include("CarrierInvoiceProfile")
							   where x.InvoiceID == invoiceID
							   select x).FirstOrDefault<LeadInvoice>();

			return invoice;
		}

		static public List<InvoiceView> GetInvoiceForReport(int invoiceID) {
			InvoiceView invoice = null;
			List<InvoiceView> invoices = new List<InvoiceView>();
			ZipCodeMaster zipCodeMaster = null;

			LeadInvoice leadInvoice = (from x in DbContextHelper.DbContext
								   .LeadInvoice
								   .Include("LeadInvoiceDetail")
								   .Include("LeadInvoiceDetail.InvoiceServiceType")
								   .Include("LeadInvoiceDetail.InvoiceServiceType.InvoiceServiceUnit")
								   .Include("Leads")
								   .Include("Leads.Client")
								   .Include("Leads.Client.StateMaster")
								   .Include("Leads.Client.CityMaster")
								   .Include("Leads.LeadPolicy")
								   .Include("Leads.LeadPolicy.LeadPolicyType")
								  where x.InvoiceID == invoiceID
								  select x
						    ).FirstOrDefault<LeadInvoice>();

           

			if (leadInvoice != null) {
				invoice = new InvoiceView();
				invoice.invoiceID = leadInvoice.InvoiceID;
				invoice.invoiceDate = (DateTime)leadInvoice.InvoiceDate;
				invoice.dueDate = leadInvoice.DueDate == null ? DateTime.Now : (DateTime)leadInvoice.DueDate;
				invoice.invoiceNumber = (int)(leadInvoice.InvoiceNumber ?? 0);

				invoice.billTo = leadInvoice.BillToName;
				invoice.billToAddress1 = leadInvoice.BillToAddress1;
				invoice.billToAddress2 = leadInvoice.BillToAddress2;
				invoice.billToAddress3 = leadInvoice.BillToAddress3;
				invoice.totalAmount = leadInvoice.TotalAmount ?? 0;

				invoice.adjusterInvoiceNumber = leadInvoice.AdjusterInvoiceNumber;

				// get policy type associated with invoice
				leadInvoice.Leads.LeadPolicy.FirstOrDefault(x => x.PolicyType == leadInvoice.PolicyTypeID);

                //if (leadInvoice.Leads.LeadPolicy != null && leadInvoice.Leads.LeadPolicy.LeadPolicyType != null)
                //    invoice.policyType = leadInvoice.Leads.LeadPolicy.Description;

                //invoice.policyNumber = leadInvoice.Leads.LeadPolicy;
                //invoice.claimNumber = leadInvoice.Leads.LeadPolicy.ClaimNumber;
                //invoice.insurerFileNo = leadInvoice.Leads.LeadPolicy.InsurerFileNo;


                if (leadInvoice.Leads.LeadPolicy != null && leadInvoice.Leads.LeadPolicy.FirstOrDefault().LeadPolicyType != null)
                    invoice.policyType = leadInvoice.Leads.LeadPolicy.FirstOrDefault().LeadPolicyType.Description;

                invoice.policyNumber = leadInvoice.Leads.LeadPolicy.FirstOrDefault().PolicyNumber;
                invoice.claimNumber = leadInvoice.Leads.LeadPolicy.FirstOrDefault().ClaimNumber;
                invoice.insurerFileNo = leadInvoice.Leads.LeadPolicy.FirstOrDefault().InsurerFileNo;

				


				invoice.claimantName = leadInvoice.Leads.ClaimantFirstName + " " + leadInvoice.Leads.ClaimantLastName;
				invoice.claimantAddress1 = leadInvoice.Leads.LossAddress;
				invoice.claimantAddress2 = leadInvoice.Leads.LossAddress2;
				invoice.claimantAddress3 = string.Format("{0}, {1} {2}",
						leadInvoice.Leads.CityName ?? "",
						leadInvoice.Leads.StateName ?? "",
						leadInvoice.Leads.Zip ?? "");

				invoice.taxRate = leadInvoice.TaxRate ?? 0;


				if (leadInvoice.Leads.DateSubmitted != null)
					invoice.claimDate = (DateTime)leadInvoice.Leads.DateSubmitted;

				invoice.legacyInvoiceLines = leadInvoice.LeadInvoiceDetail.ToList();

				// add sales tax
				if (invoice.taxRate > 0) {
					LeadInvoiceDetail invoiceTaxLine = new LeadInvoiceDetail();
					invoiceTaxLine.InvoiceID = invoice.invoiceID;
					invoiceTaxLine.LineDescription = "Sales Tax";
					invoiceTaxLine.isBillable = true;
					invoiceTaxLine.LineAmount = invoice.totalAmount * (invoice.taxRate / 100);
					invoice.legacyInvoiceLines.Add(invoiceTaxLine);
				}

				if (leadInvoice.Leads.Client != null) {
					invoice.adjusterInvoiceNumber = leadInvoice.AdjusterInvoiceNumber;

					invoice.clientName = leadInvoice.Leads.Client.BusinessName;
					invoice.clientAddress1 = leadInvoice.Leads.Client.StreetAddress1;
					invoice.clientAddress2 = leadInvoice.Leads.Client.StreetAddress2;

					string stateName = leadInvoice.Leads.Client.StateMaster != null ? leadInvoice.Leads.Client.StateMaster.StateCode : "";
					string cityName = leadInvoice.Leads.Client.CityMaster != null ? leadInvoice.Leads.Client.CityMaster.CityName : "";
					string zipCode = leadInvoice.Leads.Client.ZipCode ?? "";

					// this is badly designed.
					if (!string.IsNullOrEmpty(zipCode)) {
						zipCodeMaster = ZipCode.Get(zipCode);
						if (zipCodeMaster != null)
							zipCode = zipCodeMaster.ZipCode;
					}

					invoice.clientAddress3 = cityName + ", " + stateName + " " + zipCode;

					invoice.clientPhone = leadInvoice.Leads.Client.PrimaryPhoneNo;
					invoice.clientFax = leadInvoice.Leads.Client.SecondaryPhoneNo;
					invoice.clientEmail = leadInvoice.Leads.Client.PrimaryEmailId;

					invoice.logoPath = string.Format("{0}/ClientLogo/{1}.jpg", ConfigurationManager.AppSettings["appURL"].ToString(), leadInvoice.Leads.ClientID);
				}
				else {
					invoice.logoPath = ConfigurationManager.AppSettings["appURL"].ToString() + "/images/claim_ruler_logo.jpg";
					invoice.clientName = "Claim Ruler Demo";
					invoice.clientAddress1 = "400 East Las Olas Blvd";
					invoice.clientAddress2 = "Suite 404";
					invoice.clientAddress3 = "Ft. Lauderdale, FL 33301";
					invoice.clientPhone = "999-999-9999";
					invoice.clientFax = "999-999-9999";
					invoice.clientEmail = "email@claimruler.com";
					invoice.federalIDNo = "99999999";
				}

				invoices.Add(invoice);
			}

			return invoices;
		}

		public static IQueryable<LeadInvoice> GetInvoicesForApproval(int clientID) {
			IQueryable<LeadInvoice> invoices = null;

			invoices = from x in DbContextHelper.DbContext.LeadInvoice
					 .Include("Carrier")
					 .Include("Lead")
					 .Include("LeadPolicy")
					 .Include("CarrierInvoiceType")
					 
					 where x.ClientID == clientID && (x.isVoid == false || x.isVoid == null)
					 select x;

			return invoices;
		}

		static public LeadInvoice GetByLeadID(int leadID, int policyTypeID) {

			LeadInvoice invoice = (from x in DbContextHelper.DbContext
								   .LeadInvoice
								   .Include("LeadInvoiceDetail")
							   where x.LeadId == leadID &&
							   x.PolicyTypeID == policyTypeID
							   select x).FirstOrDefault<LeadInvoice>();

			return invoice;
		}

		static public int GetNextInvoiceNumber(int clientID) {
			int? nextInvoiceNumber = 0;

			nextInvoiceNumber = (from x in DbContextHelper.DbContext.LeadInvoice
							 where x.ClientID == clientID
							 select x.InvoiceNumber
								 ).Max();

			// increment next number
			return (int)(nextInvoiceNumber == null ? 1 : ++nextInvoiceNumber);
		}

		public static int Save(LeadInvoice invoice) {
			if (invoice.InvoiceID == 0)
				DbContextHelper.DbContext.Add(invoice);

			DbContextHelper.DbContext.SaveChanges();

			return invoice.InvoiceID;
		}

		public static LeadInvoice Save_bak(LeadInvoice invoice) {
			LeadInvoice updateInvoice = null;
			LeadInvoiceDetail updateinvoiceLine = null;

			if (invoice.InvoiceID == 0) {
				DbContextHelper.DbContext.Add(invoice);

				//DbContextHelper.DbContext.SaveChanges();
			}
			else {
				updateInvoice = Get(invoice.InvoiceID);

				updateInvoice.InvoiceDate = invoice.InvoiceDate;

				updateInvoice.PolicyTypeID = invoice.PolicyTypeID;

				updateInvoice.BillToName = invoice.BillToName;
				updateInvoice.BillToAddress1 = invoice.BillToAddress1;
				updateInvoice.BillToAddress2 = invoice.BillToAddress2;
				updateInvoice.BillToAddress3 = invoice.BillToAddress3;

				updateInvoice.AdjusterInvoiceNumber = invoice.AdjusterInvoiceNumber;

				//DbContextHelper.DbContext.SaveChanges();

				foreach (LeadInvoiceDetail invoiceLine in invoice.LeadInvoiceDetail) {
					if (invoiceLine.InvoiceLineID == 0) {
						updateinvoiceLine = new LeadInvoiceDetail();

						updateinvoiceLine.InvoiceID = invoice.InvoiceID;

						DbContextHelper.DbContext.Add(updateinvoiceLine);
					}
					else
						updateinvoiceLine = GetInvoiceDetailLine(invoiceLine.InvoiceLineID);

					updateinvoiceLine.LineItemNo = invoiceLine.LineItemNo;

					updateinvoiceLine.LineDescription = invoiceLine.LineDescription;

					updateinvoiceLine.LineAmount = invoiceLine.LineAmount;



				}
			}

			DbContextHelper.DbContext.SaveChanges();

			return invoice;
		}

		#endregion

		#region Invoice Detail Lines
		static public LeadInvoiceDetail GetInvoiceDetailLine(int invoiceLineID) {

			LeadInvoiceDetail invoiceLine = (from x in DbContextHelper.DbContext.LeadInvoiceDetail
									   where x.InvoiceLineID == invoiceLineID
									   select x).FirstOrDefault<LeadInvoiceDetail>();

			return invoiceLine;
		}

		static public void DeleteInvoiceDetailLine(int invoiceLineID, int lineItemNo) {

			LeadInvoiceDetail invoiceLine = (from x in DbContextHelper.DbContext.LeadInvoiceDetail
									   where x.InvoiceLineID == invoiceLineID &&
									   x.LineItemNo == lineItemNo
									   select x).FirstOrDefault<LeadInvoiceDetail>();

			if (invoiceLine != null) {
				DbContextHelper.DbContext.DeleteObject(invoiceLine);

				DbContextHelper.DbContext.SaveChanges();
			}

		}

		#endregion


	}
}
