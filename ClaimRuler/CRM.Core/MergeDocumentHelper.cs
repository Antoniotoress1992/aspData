using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Core {
	public class mergeField {
		public string fieldName { get; set; }
		public string fieldValue { get; set; }

		public mergeField(string fieldName, string fieldValue) {
			this.fieldName = fieldName ?? "";
			this.fieldValue = (fieldValue ?? "").Trim();
		}
	}


	static public class MergeDocumentHelper {
		static public List<mergeField> mergeFields = new List<mergeField>();
		static public string[] fieldNames = null;
		static public string[] fieldValues = null;

		//2014-04-25 deprecated
		static public void loadFieldValues(Leads lead) {
			//string homeownerPolicy = null;
			//string commercialPolicy = null;
			//string floodPolicy = null;
			//string earhtquakePolicy = null;

			//string homeownerClaimNumber = null;
			//string commercialClaimNumber = null;
			//string floodClaimNumber = null;
			//string earhtquakeClaimNumber = null;

			//string homeowner_insurance = null;
			//string homeonwer_insurance_address = null;
			//string homeonwer_insurance_address2 = null;
			//string homeowner_insurance_city = null;
			//string homeowner_insurance_state = null;
			//string homeonwer_insurance_zipcode = null;
			//string homeonwer_phone_number = null;

			//string commercial_insurance_address = null;
			//string commercial_insurance_city = null;
			//string commercial_insurance_state = null;
			//string commercial_insurance_zipcode = null;
			//string commercial_phone_number = null;

			//string flood_insurance_address = null;
			//string flood_insurance_city = null;
			//string flood_insurance_state = null;
			//string flood_insurance_zipcode = null;
			//string flood_phone_number = null;

			//string earthquake_insurance_address = null;
			//string earthquake_insurance_city = null;
			//string earthquake_insurance_state = null;
			//string earthquake_insurance_zipcode = null;
			//string earthquake_phone_number = null;

			//CityMaster cityMaster = null;
			//StateMaster stateMaster = null;

			//string lossDate = null;

			//string commercialInsurance = null;
			//string floodInsurance = null;
			//string earhtquakeInsurance = null;

			//// remove existing fields
			//mergeFields.Clear();

			//mergeFields.Add(new mergeField("TODAYS_DATE", DateTime.Now.ToShortDateString()));
			//mergeFields.Add(new mergeField("CLAIMANT_FIRST_NAME", lead.ClaimantFirstName));
			//mergeFields.Add(new mergeField("CLAIMANT_LAST_NAME", lead.ClaimantLastName));

			//mergeFields.Add(new mergeField("LOSS_ADDRESS", lead.LossAddress));
			//mergeFields.Add(new mergeField("LOSS_ADDRESS 2", lead.LossAddress2));
			//mergeFields.Add(new mergeField("LOSS_STATE", lead.StateName));
			//mergeFields.Add(new mergeField("LOSS_CITY", lead.CityName));
			//mergeFields.Add(new mergeField("LOSS_ZIPCODE", lead.Zip));

			//mergeFields.Add(new mergeField("CLAIMANT_MAILING_ADDRESS", lead.MailingAddress));
			//mergeFields.Add(new mergeField("CLAIMANT_MAILING_STATE", lead.MailingState));
			//mergeFields.Add(new mergeField("CLAIMANT_MAILING_CITY", lead.MailingCity));
			//mergeFields.Add(new mergeField("CLAIMANT_MAILING_ZIPCODE", lead.MailingZip));

			//mergeFields.Add(new mergeField("CLAIMANT_PHONE_NUMBER", lead.PhoneNumber));
			//mergeFields.Add(new mergeField("CLAIMANT_EMAIL", lead.EmailAddress));

			//LeadPolicy policy = null;

			//if (lead.LeadPolicies != null) {
			//	#region homeowners
			//	policy = lead.LeadPolicies.FirstOrDefault(x => x.PolicyType == (int)PolicyType.Homeowners);
			//	homeownerPolicy = policy.PolicyNumber;
			//	homeownerClaimNumber = policy.ClaimNumber;
			//	homeowner_insurance = policy.InsuranceCompanyName;
			//	homeonwer_insurance_address = policy.InsuranceAddress;

			//	cityMaster = policy.CityMaster;
			//	homeowner_insurance_city = cityMaster == null ? "" : cityMaster.CityName;

			//	stateMaster = policy.StateMaster;
			//	homeowner_insurance_state = stateMaster == null ? "" : stateMaster.StateName;

			//	string homeowner_zip = policy.InsuranceZipCode ?? "";
			//	homeonwer_insurance_zipcode = getZipCode(homeowner_zip);

			//	homeonwer_phone_number = policy.PhoneNumber ?? "";
			//	#endregion

			//	#region commercial
			//	policy = lead.LeadPolicies.FirstOrDefault(x => x.PolicyType == (int)PolicyType.Commercial);
			//	if (policy != null) {
			//		commercialClaimNumber = policy.ClaimNumber;

			//		commercialPolicy = policy.PolicyNumber;
			//		commercialInsurance = policy.InsuranceCompanyName;
			//		commercial_insurance_address = policy.InsuranceAddress;
			//		cityMaster = policy.CityMaster;
			//		commercial_insurance_city = cityMaster == null ? "" : cityMaster.CityName;

			//		stateMaster = policy.StateMaster;
			//		commercial_insurance_state = stateMaster == null ? "" : stateMaster.StateName;

			//		string commercial_zip = policy.InsuranceZipCode ?? "";
			//		commercial_insurance_zipcode = getZipCode(commercial_zip);

			//		commercial_phone_number = policy.PhoneNumber ?? "";
			//	}
			//	#endregion

			//	#region flood
			//	policy = lead.LeadPolicies.FirstOrDefault(x => x.PolicyType == (int)PolicyType.Flood);
			//	if (policy != null) {
			//		floodClaimNumber = policy == null ? "" : policy.ClaimNumber;
			//		floodPolicy = policy.PolicyNumber;
			//		floodInsurance = policy.InsuranceCompanyName;
			//		flood_insurance_address = policy.InsuranceAddress;
			//		cityMaster = policy.CityMaster;
			//		flood_insurance_city = cityMaster == null ? "" : cityMaster.CityName;

			//		stateMaster = policy.StateMaster;
			//		flood_insurance_state = stateMaster == null ? "" : stateMaster.StateName;

			//		string flood_zip = policy.InsuranceZipCode ?? "";
			//		flood_insurance_zipcode = getZipCode(flood_zip);

			//		flood_phone_number = policy.PhoneNumber;
			//	}
			//	#endregion

			//	#region earthquake
			//	policy = lead.LeadPolicies.FirstOrDefault(x => x.PolicyType == (int)PolicyType.Earthquake);
			//	if (policy != null) {
			//		earhtquakeClaimNumber = policy == null ? "" : policy.ClaimNumber;
			//		earhtquakePolicy = policy.PolicyNumber;
			//		earhtquakeInsurance = policy.InsuranceCompanyName;
			//		earthquake_insurance_address = policy.InsuranceAddress;
			//		cityMaster = policy.CityMaster;
			//		earthquake_insurance_city = cityMaster == null ? "" : cityMaster.CityName;

			//		stateMaster = policy.StateMaster;
			//		earthquake_insurance_state = stateMaster == null ? "" : stateMaster.StateName;

			//		string earthquake_zip = policy.InsuranceZipCode;
			//		earthquake_insurance_zipcode = getZipCode(earthquake_zip);

			//		earthquake_phone_number = policy.PhoneNumber;
			//	}
			//	#endregion

			//}

			//mergeFields.Add(new mergeField("HOMEOWNER_POLICY_NO", homeownerPolicy));
			//mergeFields.Add(new mergeField("COMMERCIAL_POLICY_NO", commercialPolicy));
			//mergeFields.Add(new mergeField("FLOOD_POLICY_NO", floodPolicy));
			//mergeFields.Add(new mergeField("EARTHQUAKE_POLICY_NO", earhtquakePolicy));

			//mergeFields.Add(new mergeField("HOMEOWNER_CLAIM_NUMBER", homeownerClaimNumber));
			//mergeFields.Add(new mergeField("COMMERCIAL_CLAIM_NUMBER", commercialClaimNumber));
			//mergeFields.Add(new mergeField("FLOOD_CLAIM_NUMBER", floodClaimNumber));
			//mergeFields.Add(new mergeField("EARTHQUAKE_CLAIM_NUMBER", earhtquakeClaimNumber));

			//mergeFields.Add(new mergeField("HOMEOWNER_INSURANCE_CO", homeowner_insurance));
			//mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_ADDRESS", homeonwer_insurance_address));
			//mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_ADDRESS2", homeonwer_insurance_address2));
			//mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_CITY", homeowner_insurance_city));
			//mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_STATE", homeowner_insurance_state));
			//mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_ZIPCODE", homeonwer_insurance_zipcode));
			//mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_PHONE", homeonwer_phone_number));


			//mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_CO", commercialInsurance));
			//mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_ADDRESS", commercial_insurance_address));
			//mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_CITY", commercial_insurance_city));
			//mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_STATE", commercial_insurance_state));
			//mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_ZIPCODE", commercial_insurance_zipcode));
			//mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_PHONE", commercial_phone_number));

			//mergeFields.Add(new mergeField("FLOOD_INSURANCE_CO", floodInsurance));
			//mergeFields.Add(new mergeField("FLOOD_INSURANCE_ADDRESS", flood_insurance_address));
			//mergeFields.Add(new mergeField("FLOOD_INSURANCE_CITY", flood_insurance_city));
			//mergeFields.Add(new mergeField("FLOOD_INSURANCE_STATE", flood_insurance_state));
			//mergeFields.Add(new mergeField("FLOOD_INSURANCE_ZIPCODE", flood_insurance_zipcode));
			//mergeFields.Add(new mergeField("FLOOD_INSURANCE_PHONE", flood_phone_number));

			//mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_CO", earhtquakeInsurance));
			//mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_ADDRESS", earthquake_insurance_address));
			//mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_CITY", earthquake_insurance_city));
			//mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_STATE", earthquake_insurance_state));
			//mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_ZIPCODE", earthquake_insurance_zipcode));
			//mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_PHONE", earthquake_phone_number));

			//if (lead.LossDate != null)
			//	lossDate = string.Format("{0:MM-dd-yyyy}", lead.LossDate);

			//mergeFields.Add(new mergeField("LOSS_DATE", lossDate));

			//string[] damageType = lead.TypeofDamageText.Split(',');
			//if (damageType != null && damageType.Length > 0) {
			//	string[] damageCause = damageType.Where(x => !string.IsNullOrEmpty(x)).ToArray();

			//	mergeFields.Add(new mergeField("DAMAGE_CAUSE", string.Join(",", damageCause)));
			//}

			//if (lead.ClientID != null) {
			//	mergeFields.Add(new mergeField("OFFICE_NAME", lead.Client.BusinessName));
			//	mergeFields.Add(new mergeField("FEDERAL_ID_NO", lead.Client.FederalIDNo));
			//	mergeFields.Add(new mergeField("OFFICE_ADDRESS", lead.Client.StreetAddress1 ?? "" + " " + lead.Client.StreetAddress2 ?? ""));
			//	mergeFields.Add(new mergeField("OFFICE_CITY", lead.Client.CityMaster == null ? "" : lead.Client.CityMaster.CityName));
			//	mergeFields.Add(new mergeField("OFFICE_STATE", lead.Client.StateMaster == null ? "" : lead.Client.StateMaster.StateName));
			//	if (lead.Client.ZipCode != null) {
			//		mergeFields.Add(new mergeField("OFFICE_ZIPCODE", getZipCode(lead.Client.ZipCode)));
			//	}
			//}

			//// build field name arrays
			//fieldNames = mergeFields.Select(x => x.fieldName).ToArray();

			//// build field value arrays
			//fieldValues = mergeFields.Select(x => x.fieldValue).ToArray();
		}

		static public void loadFieldValues(Claim claim) {
			Leads lead = claim.LeadPolicy.Leads;
			LeadPolicy policy = claim.LeadPolicy;
			Carrier carrier = policy.Carrier;
			Client portal = portal = lead.Client;
           
            PolicyLienholder lienHolder = policy.PolicyLienholder as PolicyLienholder;
            //PolicyLienholder lienHolder = policy.PolicyLienholder as PolicyLienholder;

            List<vw_ClaimLimit> limits = null;
            limits = ClaimLimitManager.GetAll(claim.ClaimID, LimitType.LIMIT_TYPE_PROPERTY);
            if (limits != null && limits.Count == 0)
            {
                limits = primePropertyLimits();
            }

            List<vw_ClaimLimit> limitsCasuality = ClaimLimitManager.GetAll(claim.ClaimID, LimitType.LIMIT_TYPE_CASUALTY);

            if (limitsCasuality != null && limitsCasuality.Count == 0)
            {
                limitsCasuality = primeCasualtyLimits();
            }


            //decimal p= limits[0].Depreciation??0;

            string policy_number = null;
			string claim_number = null;

			string insurance_company = null;
			string insurance_address = null;
			string insurance_address2 = null;
			string insurance_city = null;
			string insurance_state = null;
			string insurance_zipcode = null;
			string insurance_phone_number = null;

            decimal recovDepreA = 0;
            decimal recovDepreB = 0;
            decimal recovDepreC = 0;
            decimal recovDepreTotal = 0;
            decimal nonRecovDepreA = 0;
            decimal nonRecovDepreB = 0;
            decimal nonRecovDepreC = 0;
            decimal nonRecovDepreTotal = 0;
            decimal deductibleA = 0;
            decimal deductibleB = 0;
            decimal deductibleC = 0;
            decimal deductibleTotal =0;
            decimal limitA = 0;
            decimal limitB = 0;
            decimal limitC = 0;
            decimal limitD = 0;
            decimal limitE = 0;
            decimal limitF = 0;
            decimal lossAmountRCVA = 0;
            decimal lossAmountRCVB = 0;
            decimal lossAmountRCVC = 0;
            decimal lossAmountRCVD = 0;
            decimal totalLossAmountRCVAThruD = 0;

            decimal lossAmountACVA = 0;
            decimal lossAmountACVB = 0;
            decimal lossAmountACVC = 0;
            decimal lossAmountACVD = 0;
            decimal lossAmountACVE = 0;
            decimal lossAmountACVF = 0;
            decimal totalLossAmountACVAThruD = 0;
            decimal totalLossAmountACVEThruF = 0;


			CityMaster cityMaster = null;
			StateMaster stateMaster = null;

			string loss_date = null;
            string dateOpenReported = string.Empty;
            string dateContacted = string.Empty;
            string dateInspectionCompleted = string.Empty;








            if (limits.Count>=4)
            {
             recovDepreA = limits[0].Depreciation ?? 0;
             recovDepreB = limits[1].Depreciation ?? 0;
             recovDepreC = limits[2].Depreciation ?? 0;
             recovDepreTotal = recovDepreA + recovDepreB + recovDepreC;

             nonRecovDepreA = limits[0].NonRecoverableDepreciation ?? 0;
             nonRecovDepreB = limits[1].NonRecoverableDepreciation ?? 0;
             nonRecovDepreC = limits[2].NonRecoverableDepreciation ?? 0;
             nonRecovDepreTotal = nonRecovDepreA + nonRecovDepreB + nonRecovDepreC;

             deductibleA = limits[0].LimitDeductible ?? 0;
             deductibleB = limits[1].LimitDeductible ?? 0;
             deductibleC = limits[2].LimitDeductible ?? 0;
             deductibleTotal = deductibleA + deductibleB + deductibleC;

             limitA = limits[0].LimitAmount ?? 0;
             limitB = limits[1].LimitAmount ?? 0;
             limitC = limits[2].LimitAmount ?? 0;
             limitD = limits[3].LimitAmount ?? 0;

             lossAmountRCVA = limits[0].LossAmountRCV ?? 0;
             lossAmountRCVB = limits[1].LossAmountRCV ?? 0;
             lossAmountRCVC = limits[2].LossAmountRCV ?? 0;
             lossAmountRCVD = limits[3].LossAmountRCV ?? 0;
             totalLossAmountRCVAThruD = lossAmountRCVA + lossAmountRCVB + lossAmountRCVC + lossAmountRCVD;

             lossAmountACVA = limits[0].LossAmountACV ?? 0;
             lossAmountACVB = limits[1].LossAmountACV ?? 0;
             lossAmountACVC = limits[2].LossAmountACV ?? 0;
             lossAmountACVD = limits[3].LossAmountACV ?? 0;
             totalLossAmountACVAThruD = lossAmountACVA + lossAmountACVB + lossAmountACVC + lossAmountACVD;
            }

            if (limitsCasuality.Count>=2)
            {
                limitE = limitsCasuality[0].LimitAmount ?? 0;
                limitF = limitsCasuality[1].LimitAmount ?? 0;

                lossAmountACVE = limitsCasuality[0].LossAmountACV ?? 0;
                lossAmountACVF = limitsCasuality[1].LossAmountACV ?? 0;
                totalLossAmountACVEThruF = lossAmountACVE + lossAmountACVF;
            }



			
			// policy info
			policy_number = policy.PolicyNumber;

			if (carrier != null) { 
				insurance_company = carrier.CarrierName;
				insurance_address = carrier.AddressLine1;
				insurance_address2 = carrier.AddressLine2;
				cityMaster = carrier.CityMaster;
				insurance_city = cityMaster == null ? "" : cityMaster.CityName;

				stateMaster = carrier.StateMaster;
				insurance_state = stateMaster == null ? "" : stateMaster.StateName;
				insurance_zipcode = carrier.ZipCode;		
			}

			// claim info
			claim_number = claim.AdjusterClaimNumber;
			if (claim.LossDate != null)
				loss_date = string.Format("{0:MM-dd-yyyy}", claim.LossDate);

            if(claim.DateOpenedReported!=null)
                dateOpenReported = string.Format("{0:MM-dd-yyyy}", claim.DateOpenedReported);

            if (claim.DateContacted != null)
                dateContacted = string.Format("{0:MM-dd-yyyy}", claim.DateContacted);

            if (claim.DateInspectionCompleted != null)
                dateInspectionCompleted = string.Format("{0:MM-dd-yyyy}", claim.DateInspectionCompleted);
            
			// remove existing fields
			mergeFields.Clear();
            
			mergeFields.Add(new mergeField("TODAYS_DATE", DateTime.Now.ToShortDateString()));
			mergeFields.Add(new mergeField("CLAIMANT_FIRST_NAME", lead.ClaimantFirstName));
			mergeFields.Add(new mergeField("CLAIMANT_LAST_NAME", lead.ClaimantLastName));

           

            /////
			mergeFields.Add(new mergeField("LOSS_ADDRESS", lead.LossAddress));
			mergeFields.Add(new mergeField("LOSS_ADDRESS 2", lead.LossAddress2));
			mergeFields.Add(new mergeField("LOSS_STATE", lead.StateName));
			mergeFields.Add(new mergeField("LOSS_CITY", lead.CityName));
			mergeFields.Add(new mergeField("LOSS_ZIPCODE", lead.Zip));
            /////
			mergeFields.Add(new mergeField("CLAIMANT_MAILING_ADDRESS", lead.MailingAddress));
			mergeFields.Add(new mergeField("CLAIMANT_MAILING_STATE", lead.MailingState));
			mergeFields.Add(new mergeField("CLAIMANT_MAILING_CITY", lead.MailingCity));
			mergeFields.Add(new mergeField("CLAIMANT_MAILING_ZIPCODE", lead.MailingZip));

			mergeFields.Add(new mergeField("CLAIMANT_PHONE_NUMBER", lead.PhoneNumber));
			mergeFields.Add(new mergeField("CLAIMANT_EMAIL", lead.EmailAddress));

			mergeFields.Add(new mergeField("POLICY_NO", policy_number));
			mergeFields.Add(new mergeField("CLAIM_NUMBER", claim_number));
			mergeFields.Add(new mergeField("INSURANCE_CO", insurance_company));
			mergeFields.Add(new mergeField("INSURANCE_ADDRESS", insurance_address));
			mergeFields.Add(new mergeField("INSURANCE_ADDRESS2", insurance_address2));
			mergeFields.Add(new mergeField("INSURANCE_CITY", insurance_city));
			mergeFields.Add(new mergeField("INSURANCE_STATE", insurance_state));
			mergeFields.Add(new mergeField("INSURANCE_ZIPCODE", insurance_zipcode));
			mergeFields.Add(new mergeField("INSURANCE_PHONE", insurance_phone_number));

			mergeFields.Add(new mergeField("HOMEOWNER_POLICY_NO", policy_number));
			mergeFields.Add(new mergeField("COMMERCIAL_POLICY_NO", policy_number));
			mergeFields.Add(new mergeField("FLOOD_POLICY_NO", policy_number));
			mergeFields.Add(new mergeField("EARTHQUAKE_POLICY_NO", policy_number));

			mergeFields.Add(new mergeField("HOMEOWNER_CLAIM_NUMBER", claim_number));
			mergeFields.Add(new mergeField("COMMERCIAL_CLAIM_NUMBER", claim_number));
			mergeFields.Add(new mergeField("FLOOD_CLAIM_NUMBER", claim_number));
			mergeFields.Add(new mergeField("EARTHQUAKE_CLAIM_NUMBER", claim_number));

			mergeFields.Add(new mergeField("HOMEOWNER_INSURANCE_CO", insurance_company));
			mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_ADDRESS", insurance_address));
			mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_ADDRESS2", insurance_address2));
			mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_CITY", insurance_city));
			mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_STATE", insurance_state));
			mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_ZIPCODE", insurance_zipcode));
			mergeFields.Add(new mergeField("HOMEONWER_INSURANCE_PHONE", insurance_phone_number));


			mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_CO", insurance_company));
			mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_ADDRESS", insurance_address));
			mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_CITY", insurance_city));
			mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_STATE", insurance_state));
			mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_ZIPCODE", insurance_zipcode));
			mergeFields.Add(new mergeField("COMMERCIAL_INSURANCE_PHONE", insurance_phone_number));

			mergeFields.Add(new mergeField("FLOOD_INSURANCE_CO", insurance_company));
			mergeFields.Add(new mergeField("FLOOD_INSURANCE_ADDRESS", insurance_address));
			mergeFields.Add(new mergeField("FLOOD_INSURANCE_CITY", insurance_city));
			mergeFields.Add(new mergeField("FLOOD_INSURANCE_STATE", insurance_state));
			mergeFields.Add(new mergeField("FLOOD_INSURANCE_ZIPCODE", insurance_zipcode));
			mergeFields.Add(new mergeField("FLOOD_INSURANCE_PHONE", insurance_phone_number));

			mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_CO", insurance_company));
			mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_ADDRESS", insurance_address));
			mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_CITY", insurance_city));
			mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_STATE", insurance_state));
			mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_ZIPCODE", insurance_zipcode));
			mergeFields.Add(new mergeField("EARTHQUAKE_INSURANCE_PHONE", insurance_phone_number));

			

			mergeFields.Add(new mergeField("LOSS_DATE", loss_date));
			
			if (!string.IsNullOrEmpty(claim.CauseOfLoss)) {
				string[] causeOfLossDescriptions = TypeofDamageManager.GetDescriptions(claim.CauseOfLoss);
				
				mergeFields.Add(new mergeField("DAMAGE_CAUSE", string.Join(",", causeOfLossDescriptions)));				
			}

			if (portal != null) {
				mergeFields.Add(new mergeField("OFFICE_NAME", portal.BusinessName));
				mergeFields.Add(new mergeField("FEDERAL_ID_NO", portal.FederalIDNo));
				mergeFields.Add(new mergeField("OFFICE_ADDRESS", portal.StreetAddress1 ?? "" + " " + lead.Client.StreetAddress2 ?? ""));
				mergeFields.Add(new mergeField("OFFICE_CITY", portal.CityMaster == null ? "" : lead.Client.CityMaster.CityName));
				mergeFields.Add(new mergeField("OFFICE_STATE", portal.StateMaster == null ? "" : lead.Client.StateMaster.StateName));
				if (portal.ZipCode != null) {
					mergeFields.Add(new mergeField("OFFICE_ZIPCODE", getZipCode(portal.ZipCode)));
				}
			}

            ///// ADD BY CHETU TEAM
            mergeFields.Add(new mergeField("INSURED_NAME", lead.InsuredName));
            mergeFields.Add(new mergeField("INSURER_CLAIM_NUMBER", claim.InsurerClaimNumber));

           
            mergeFields.Add(new mergeField("ESTIMATOR_NAME", claim.AdjusterMaster.AdjusterName));
            mergeFields.Add(new mergeField("ESTIMATOR_CELL_PHONE", claim.AdjusterMaster.PhoneNumber));
            mergeFields.Add(new mergeField("ESTIMATOR_EMAIL", claim.AdjusterMaster.email));
            mergeFields.Add(new mergeField("TYPE_OF_LOSS", claim.TypeofLoss));
            string carrierExaminer = ClaimsManager.GetContactByContactID(claim.ManagerID ?? 0);
            mergeFields.Add(new mergeField("CARRIER_EXAMINER", carrierExaminer));
            mergeFields.Add(new mergeField("RECOVERABLEDEPRECIATION",Convert.ToString(claim.Depreciation)));
            mergeFields.Add(new mergeField("NONRECOVERABLEDEPRECIATION", Convert.ToString(claim.NonRecoverableDepreciation)));
            mergeFields.Add(new mergeField("MORTGAGEENAME", Convert.ToString(lienHolder.Mortgagee.MortageeName)));
            mergeFields.Add(new mergeField("LOANNUMBER", Convert.ToString(lienHolder.LoanNumber)));
            /////////////
            mergeFields.Add(new mergeField("RECOVERABLEDEPRECIATION_A", Convert.ToString(recovDepreA)));
            mergeFields.Add(new mergeField("RECOVERABLEDEPRECIATION_B", Convert.ToString(recovDepreB)));
            mergeFields.Add(new mergeField("RECOVERABLEDEPRECIATION_C", Convert.ToString(recovDepreC)));
            mergeFields.Add(new mergeField("RECOVERABLEDEPRECIATION_TOTAL", Convert.ToString(recovDepreTotal)));

            mergeFields.Add(new mergeField("NONRECOVERABLEDEPRECIATION_A", Convert.ToString(nonRecovDepreA)));
            mergeFields.Add(new mergeField("NONRECOVERABLEDEPRECIATION_B", Convert.ToString(nonRecovDepreB)));
            mergeFields.Add(new mergeField("NONRECOVERABLEDEPRECIATION_C", Convert.ToString(nonRecovDepreC)));
            mergeFields.Add(new mergeField("NONRECOVERABLEDEPRECIATION_Total", Convert.ToString(nonRecovDepreTotal)));

            mergeFields.Add(new mergeField("DEDUCTIBLE_A", Convert.ToString(deductibleA)));
            mergeFields.Add(new mergeField("DEDUCTIBLE_B", Convert.ToString(deductibleB)));
            mergeFields.Add(new mergeField("DEDUCTIBLE_C", Convert.ToString(deductibleC)));
            mergeFields.Add(new mergeField("DEDUCTIBLE_Total", Convert.ToString(deductibleTotal)));

            mergeFields.Add(new mergeField("LIMIT_A", Convert.ToString(limitA)));
            mergeFields.Add(new mergeField("LIMIT_B", Convert.ToString(limitB)));
            mergeFields.Add(new mergeField("LIMIT_C", Convert.ToString(limitC)));
            mergeFields.Add(new mergeField("LIMIT_D", Convert.ToString(limitD)));

            mergeFields.Add(new mergeField("LOSSAMOUNT_RCV_A", Convert.ToString(lossAmountRCVA)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_RCV_B", Convert.ToString(lossAmountRCVB)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_RCV_C", Convert.ToString(lossAmountRCVC)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_RCV_D", Convert.ToString(lossAmountRCVD)));
            mergeFields.Add(new mergeField("TOTAL_LOSS_AMOUNT_A_THRU_DRCV", Convert.ToString(totalLossAmountRCVAThruD)));

            mergeFields.Add(new mergeField("LOSSAMOUNT_ACV_A", Convert.ToString(lossAmountACVA)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_ACV_B", Convert.ToString(lossAmountACVB)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_ACV_C", Convert.ToString(lossAmountACVC)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_ACV_D", Convert.ToString(lossAmountACVD)));
            mergeFields.Add(new mergeField("TOTAL_LOSS_AMOUNT_A_THRU_DACV", Convert.ToString(totalLossAmountACVAThruD)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_ACV_E", Convert.ToString(lossAmountACVE)));
            mergeFields.Add(new mergeField("LOSSAMOUNT_ACV_F", Convert.ToString(lossAmountACVF)));
            mergeFields.Add(new mergeField("TOTAL_LOSS_AMOUNT_E_THRU_FACV", Convert.ToString(totalLossAmountACVAThruD)));

            mergeFields.Add(new mergeField("DATEOPEN_REPORTED", dateOpenReported));
            mergeFields.Add(new mergeField("DATE_CONTACTED", dateContacted));
            mergeFields.Add(new mergeField("DATE_INSPECTION_COMPLETED", dateInspectionCompleted));
            mergeFields.Add(new mergeField("EFFECTIVE_DATE", ""));
            mergeFields.Add(new mergeField("EXPIRY_DATE", ""));




			// build field name arrays
			fieldNames = mergeFields.Select(x => x.fieldName).ToArray();

			// build field value arrays
			fieldValues = mergeFields.Select(x => x.fieldValue).ToArray();
		}

		static string getZipCode(string zipID) {
			ZipCodeMaster zipMaster = Data.Account.ZipCode.Get(zipID);
			return zipMaster == null ? "" : zipMaster.ZipCode;
		}

		static public void addLetterToDocumentList(int claimID, string documentPath, string documentDescription) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string directoryPath = null;

			ClaimDocument claimDocument = new ClaimDocument();
			string destinationPath = null;

			if (!File.Exists(documentPath))
				return;

			string ext = System.IO.Path.GetExtension(documentPath);

			claimDocument.ClaimID = claimID;
			claimDocument.Description = documentDescription;
			claimDocument.DocumentName = Core.ReportHelper.sanatizeFileName(documentDescription) + ext;

			claimDocument.DocumentDate = DateTime.Now;

			claimDocument = ClaimDocumentManager.Save(claimDocument);

			if (claimDocument.ClaimDocumentID > 0) {
				directoryPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, claimDocument.ClaimDocumentID);

				if (!Directory.Exists(directoryPath))
					Directory.CreateDirectory(directoryPath);

				destinationPath = directoryPath + "/" + claimDocument.DocumentName;

				File.Copy(documentPath, destinationPath);
			}
		}

		static public void autoFillDocument(string templateURL, string filepath, Claim claim, bool isHeader = false) {
			Document document = new Document();

			// load document template
			//document.LoadFromFile(templateURL, FileFormat.Doc);
			document.LoadFromFile(templateURL, FileFormat.Auto);

			// load merge fields
			loadFieldValues(claim);

			//if (isHeader)
			//     setHeader(document, lead.Client);

			document.MailMerge.Execute(fieldNames, fieldValues);

			document.SaveToFile(filepath, FileFormat.Doc);
			//document.SaveToFile(filepath, FileFormat.Auto);

			document.Close();

			document = null;
		}

		static public void autoFillDocument(string templateURL, string filepath, Leads lead, bool isHeader = false) {
			Document document = new Document();

			// load document template
			//document.LoadFromFile(templateURL, FileFormat.Doc);
			document.LoadFromFile(templateURL, FileFormat.Auto);

			// load merge fields
			loadFieldValues(lead);

			//if (isHeader)
			//     setHeader(document, lead.Client);

			document.MailMerge.Execute(fieldNames, fieldValues);

			document.SaveToFile(filepath, FileFormat.Doc);
			//document.SaveToFile(filepath, FileFormat.Auto);

			document.Close();

			document = null;
		}

		static public string mergeLetterTemplate(int clientID, ClientLetterTemplate template, int claimID) {
			string appPath = null;
			Leads lead = null;
			Claim claim = null;
			string letterTemplatePath = null;
			string fileExtension = null;
			string finalDocumentPath = null;

			claim = ClaimsManager.Get(claimID);

			//template = LetterTemplateManager.Get(templateID);

			appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			if (claim != null) {
				lead = claim.LeadPolicy.Leads;

				// get extension
				fileExtension = System.IO.Path.GetExtension(template.Path);

				// fileExtension includes "."
				letterTemplatePath = string.Format("{0}/ClientLetterTemplates/{1}/{2}{3}", appPath, clientID, template.TemplateID, fileExtension);

				finalDocumentPath = string.Format("{0}/Temp/{1}.doc", appPath, Guid.NewGuid());

				try {
					Core.MergeDocumentHelper.autoFillDocument(letterTemplatePath, finalDocumentPath, claim, true);

					//addLetterToDocumentList(claim.ClaimID, finalDocumentPath, template.Description);

					//clientFileName = string.Format("{0}_{1}.doc", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

				//if (!string.IsNullOrEmpty(finalDocumentPath))
				//	Core.ReportHelper.renderToBrowser(finalDocumentPath, clientFileName);
			}

			return finalDocumentPath;
		}

		static public void setHeader(Document document, Client client) {
			string logoPath = null;
			Section section = null;
			string clientHeader = null;
			string state = null;
			string city = null;

			if (client != null) {
				logoPath = HttpContext.Current.Server.MapPath(string.Format("~/ClientLogo/{0}.jpg", client.ClientId));

				state = client.StateMaster == null ? " " : client.StateMaster.StateCode;
				city = client.CityMaster == null ? " " : client.CityMaster.CityName;

				clientHeader = string.Format("{0}\n{1} {2}\n{3}, {4} {5}\nOffcie: {6} Fax: {7} Email: {8}",
					client.BusinessName,
					client.StreetAddress1,
					client.StreetAddress2,
					city,
					state,
					client.ZipCode ?? "",
					client.PrimaryPhoneNo ?? "",
					client.SecondaryPhoneNo ?? "",
					client.PrimaryEmailId ?? ""
					);

			}
			else {
				logoPath = HttpContext.Current.Server.MapPath("~/Images/claim_ruler_logo.jpg");

				clientHeader = "Public Adjusters, Inc\nA Public Adjusting Firm\n1 Anywhere Street, Somewhere, FL 11111\nOffice: (555) 111-1111 Fax: (555) 111-1111 E-Mail: someone@example.com";
			}

			section = document.Sections[0];

			// check we got a valid logo
			if (File.Exists(logoPath)) {
				//if (imageStream != null) {
				//Image logo = Image.FromStream(imageStream);
				System.Drawing.Image logo = System.Drawing.Image.FromFile(logoPath);

				//System.Drawing.Image resizedLogo = resizeImage(logo, 50);

				//Paragraph paragraph = section.Paragraphs[0];
				Paragraph paragraph = section.AddParagraph();
				paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

				//paragraph.AppendPicture(resizedLogo);
				paragraph.AppendPicture(logo);

				Paragraph pheading = section.AddParagraph();
				pheading.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

				pheading.AppendText(clientHeader);
			}

		}



		static public System.Drawing.Image resizeImage(System.Drawing.Image imgPhoto, int Percent) {
			float nPercent = ((float)Percent / 100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0;
			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(destWidth, destHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
			    new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
			    new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
			    System.Drawing.GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

        private static List<vw_ClaimLimit> primePropertyLimits()
        {
            List<Limit> limits = null;
            List<vw_ClaimLimit> propertyLimits = null;

            limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_PROPERTY);

            propertyLimits = (from x in limits
                              select new vw_ClaimLimit
                              {
                                  LimitID = x.LimitID,
                                  LimitLetter = x.LimitLetter,
                                  LimitType = x.LimitType,
                                  LimitDescription = x.LimitDescription
                              }).ToList<vw_ClaimLimit>();

            return propertyLimits;

        }


        private static List<vw_ClaimLimit> primeCasualtyLimits()
        {
            List<Limit> limits = null;
            List<vw_ClaimLimit> casualtyLimits = null;

            limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_CASUALTY);

            casualtyLimits = (from x in limits
                              select new vw_ClaimLimit
                              {
                                  LimitID = x.LimitID,
                                  LimitLetter = x.LimitLetter,
                                  LimitType = x.LimitType,
                                  LimitDescription = x.LimitDescription
                              }).ToList<vw_ClaimLimit>();

            return casualtyLimits;
        }

	}
}
