using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Repository {
	static public class InvoiceManager {

		static public decimal calculateInvoiceTotal(int invoiceID) {
			decimal totalAmount = 0;
			
			totalAmount = (from x in DbContextHelper.DbContext.InvoiceDetail
						    where x.InvoiceID == invoiceID
						    select (x.LineAmount ?? 0)
						    ).Sum();

			return totalAmount;
		}

		static public IQueryable<Invoice> GetAll(int claimID) {

			IQueryable<Invoice> invoices = from x in DbContextHelper.DbContext
								   .Invoice
								   .Include("InvoiceDetail")
								   .Include("InvoiceDetail.InvoiceServiceType")
									 where x.ClaimID == claimID && (x.IsVoid == false)
									 orderby x.InvoiceDate
									 select x;

			return invoices;
		}

		static public Invoice Get(int invoiceID) {

			Invoice invoice = (from x in DbContextHelper.DbContext
								   .Invoice
								   .Include("InvoiceDetail")
								   .Include("CarrierInvoiceProfile")
						    where x.InvoiceID == invoiceID
						    select x).FirstOrDefault<Invoice>();

			return invoice;
		}
		static public Invoice GetByID(int invoiceID) {

			Invoice invoice = (from x in DbContextHelper.DbContext
								   .Invoice
						    where x.InvoiceID == invoiceID
						    select x).FirstOrDefault<Invoice>();

			return invoice;
		}
		static public List<InvoiceView> GetInvoiceForReport(int invoiceID) {
			InvoiceView invoice = null;
			List<InvoiceView> invoices = new List<InvoiceView>();
			Invoice claimInvoice = null;
			Client client = null;
			LeadPolicy policy = null;
			Leads lead = null;
			ZipCodeMaster zipCodeMaster = null;
            //ClaimService mActivity = null;
           
            
			claimInvoice = (from x in DbContextHelper.DbContext
								   .Invoice
								   .Include("InvoiceDetail")
								   .Include("InvoiceDetail.InvoiceServiceType")
								   .Include("InvoiceDetail.InvoiceServiceType.InvoiceServiceUnit")
								   .Include("Claim")
								   .Include("Claim.LeadPolicy.Leads.Client")
								   .Include("Claim.LeadPolicy.Leads.Client.StateMaster")
								   .Include("Claim.LeadPolicy.Leads.Client.CityMaster")
								   .Include("Claim.LeadPolicy")
								   .Include("Claim.LeadPolicy.LeadPolicyType")
                                   .Include("Claim.ClaimService")
						 where x.InvoiceID == invoiceID
                         
						 select x
						    ).FirstOrDefault<Invoice>();
            
			if (claimInvoice != null) 
            {
				invoice = new InvoiceView();
				invoice.invoiceID = claimInvoice.InvoiceID;
				invoice.invoiceDate = (DateTime)claimInvoice.InvoiceDate;
				invoice.dueDate = claimInvoice.DueDate == null ? DateTime.Now : (DateTime)claimInvoice.DueDate;
				invoice.invoiceNumber = (int)(claimInvoice.InvoiceNumber ?? 0);

				invoice.billTo = claimInvoice.BillToName;
				invoice.billToAddress1 = claimInvoice.BillToAddress1;
				invoice.billToAddress2 = claimInvoice.BillToAddress2;
				invoice.billToAddress3 = claimInvoice.BillToAddress3;
				invoice.totalAmount = claimInvoice.TotalAmount ?? 0;

                //invoice.activity = claimInvoice.Claim.ClaimService NEW OC 9/30/14
				
                    //invoice.adjusterInvoiceNumber = claimInvoice.AdjusterInvoiceNumber;

				// get policy type associated with invoice
				//claimInvoice.Lead.LeadPolicies.FirstOrDefault(x => x.PolicyType == claimInvoice.PolicyTypeID);
               
                //mActivity = claimInvoice.Claim.ClaimService.FirstOrDefault();
                //invoice.activity = mActivity.Activity;
				
                policy = claimInvoice.Claim.LeadPolicy;
				if (policy != null) 
                {
					invoice.policyType = policy.LeadPolicyType == null ? "" : policy.LeadPolicyType.Description;
					invoice.policyNumber = policy.PolicyNumber;

					lead = policy.Leads;
                    
				}

				invoice.claimNumber = claimInvoice.Claim.AdjusterClaimNumber;
				invoice.insurerFileNo = claimInvoice.Claim.InsurerClaimNumber;
				invoice.adjusterInvoiceNumber = claimInvoice.InvoiceNumber.ToString();

				if (lead != null) 
                {
                    invoice.claimantName = lead.InsuredName;//policyHolderName; OC  9/30/14
					invoice.claimantAddress1 = lead.LossAddress;
					invoice.claimantAddress2 = lead.LossAddress2;
					invoice.claimantAddress3 = string.Format("{0}, {1} {2}",
							lead.CityName ?? "",
							lead.StateName ?? "",
							lead.Zip ?? "");

					client = lead.Client;
				}

				invoice.taxRate = claimInvoice.TaxRate;

				if (claimInvoice.Claim.LossDate != null)
					invoice.claimDate = (DateTime)claimInvoice.Claim.LossDate;

				invoice.invoiceLines = claimInvoice.InvoiceDetail.ToList();

				// add sales tax
				if (invoice.taxRate > 0) 
                {
					InvoiceDetail invoiceTaxLine = new InvoiceDetail();
					invoiceTaxLine.InvoiceID = invoice.invoiceID;
					invoiceTaxLine.LineDescription = "Sales Tax";
					//invoiceTaxLine.isBillable = true;
					invoiceTaxLine.LineAmount = invoice.totalAmount * (invoice.taxRate / 100);
                    invoiceTaxLine.Activity = invoice.activity;
					invoice.invoiceLines.Add(invoiceTaxLine);
                    
				}

				if (client != null) 
                {
					client = lead.Client;

					invoice.clientName = client.BusinessName;
					invoice.clientAddress1 = client.StreetAddress1;
					invoice.clientAddress2 = client.StreetAddress2;

					string stateName = client.StateMaster != null ? client.StateMaster.StateCode : "";
					string cityName = client.CityMaster != null ? client.CityMaster.CityName : "";
					string zipCode = client.ZipCode ?? "";

					// this is badly designed.
					if (!string.IsNullOrEmpty(zipCode)) 
                    {
						zipCodeMaster = ZipCode.Get(zipCode);
						if (zipCodeMaster != null)
							zipCode = zipCodeMaster.ZipCode;
					}

					invoice.clientAddress3 = cityName + ", " + stateName + " " + zipCode;

					invoice.clientPhone = client.PrimaryPhoneNo;
					invoice.clientFax = client.SecondaryPhoneNo;
					invoice.clientEmail = client.PrimaryEmailId;
					invoice.federalIDNo = client.FederalIDNo;

					invoice.logoPath = string.Format("{0}/ClientLogo/{1}.jpg", ConfigurationManager.AppSettings["appURL"].ToString(), client.ClientId);
				}
				else 
                {
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

		

		static public IQueryable<Invoice> GetInvoicesForApproval(int clientID) {
			IQueryable<Invoice> invoices = null;

			invoices = from x in DbContextHelper.DbContext.Invoice
					 .Include("Claim")
					 .Include("Claim.LeadPolicy")
					  .Include("Claim.LeadPolicy.Carrier")
					  .Include("Claim.LeadPolicy.Leads")
					 .Include("CarrierInvoiceType")
					 where x.Claim.LeadPolicy.Leads.ClientID == clientID && (x.IsVoid == false)
					 select x;

			return invoices;
		}
		static public IQueryable<vw_InvoiceApprovalQueue> GetInvoiceApprovalQueue(int clientID) {
			IQueryable<vw_InvoiceApprovalQueue> invoices = null;

            invoices = from x in DbContextHelper.DbContext.vw_InvoiceApprovalQueue
                       .Include("InvoiceType")
                       from t in DbContextHelper.DbContext.InvoiceType// ADDED NEW 11/13/14 To inlude the invoice service type in the approval queue
                       where x.ClientID == clientID
                       select  x ;

			return invoices;
		}

		//static public Invoice GetByLeadID(int leadID, int policyTypeID) {

		//	LeadInvoice invoice = (from x in DbContextHelper.DbContext
		//						   .LeadInvoices
		//						   .Include("LeadInvoiceDetails")
		//					   where x.LeadId == leadID &&
		//					   x.PolicyTypeID == policyTypeID
		//					   select x).FirstOrDefault<LeadInvoice>();

		//	return invoice;
		//}

		static public int GetNextInvoiceNumber(int clientID) {
			int? nextInvoiceNumber = 0;

			nextInvoiceNumber = (from x in DbContextHelper.DbContext.Invoice
							 where x.Claim.LeadPolicy.Leads.ClientID == clientID
							 select x.InvoiceNumber
								 ).Max();

			// increment next number
			return (int)(nextInvoiceNumber == null ? 1 : ++nextInvoiceNumber);
		}

		static public int Save(Invoice invoice) {
			if (invoice.InvoiceID == 0)
				DbContextHelper.DbContext.Add(invoice);

			DbContextHelper.DbContext.SaveChanges();

			return invoice.InvoiceID;
		}

		static public IQueryable<Invoice> Search(Expression<Func<Invoice, bool>> predicate) {

			IQueryable<Invoice> invoices = null;

			invoices = DbContextHelper.DbContext.Invoice
						.AsExpandable()
						.Where(predicate);

			return invoices;

		}

        static public IQueryable<Invoice> SearchLeadInvoiceLedger(Expression<Func<Invoice, bool>> predicate)
        {

            IQueryable<Invoice> invoices = null;

            invoices = DbContextHelper.DbContext.Invoice.Include("Claim")
                        .AsExpandable()
                        .Where(predicate);

            return invoices;

        }

	}
}
