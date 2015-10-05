

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;

	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

	public static class LeadsManager {

		public static T Clone<T>(this T source) {
			var dcs = new System.Runtime.Serialization
			  .DataContractSerializer(typeof(T));
			using (var ms = new System.IO.MemoryStream()) {
				dcs.WriteObject(ms, source);
				ms.Seek(0, System.IO.SeekOrigin.Begin);
				return (T)dcs.ReadObject(ms);
			}
		}

		public static bool CheckForDuplicate(Leads lead) {
			bool isDuplicate = false;
		
			var duplicates = from x in DbContextHelper.DbContext.Leads
						  where
							 (x.InsuredName == lead.InsuredName.Trim()) &&
							 (x.MailingAddress == lead.MailingAddress.Trim()) &&
							 (x.MailingAddress2 == lead.MailingAddress2.Trim()) &&
							 (x.MailingState == lead.MailingState.Trim()) &&		// state abbrev, FL
							 (x.MailingCity == lead.MailingCity.Trim()) &&
							 (x.MailingZip == lead.MailingZip.Trim())
						  group x by new {
							  x.InsuredName,
							  x.MailingAddress,
							  x.MailingAddress2,
							  x.MailingState,
							  x.MailingCity,
							  x.MailingZip
						  } into g
						  select g;

			isDuplicate = duplicates.Any(x => x.Count() > 1);

			return isDuplicate;
		}

		public static void Detach(Leads lead) {
			//DbContextHelper.DbContext.Detach(lead);
            ObjectContext objectContext =
                ((IObjectContextAdapter)lead).ObjectContext;
            objectContext.Detach(lead);
		}
		public static IQueryable<LeadView> SearchClaim(Expression<Func<Claim, bool>> predicate) {
			IQueryable<LeadView> leads = null;

			var query = DbContextHelper.DbContext.Claim
						.AsExpandable()
						.Where(predicate);


			leads = (from x in query
				    select new LeadView {
					    LeadId = x.LeadPolicy.LeadId ?? 0,

					    ClaimantLastName = x.LeadPolicy.Leads.ClaimantLastName,
					    ClaimantFirstName = x.LeadPolicy.Leads.ClaimantFirstName,

					    ClaimNumber = x.AdjusterClaimNumber,
					    LossDate = x.LossDate,

					    InsuranceCompanyName = x.LeadPolicy.Carrier == null ? "" : x.LeadPolicy.Carrier.CarrierName,


					    PolicyType = x.LeadPolicy.LeadPolicyType == null ? "" : x.LeadPolicy.LeadPolicyType.Description,

					    // loss 
					    LossAddress = x.LeadPolicy.Leads.LossAddress ?? "",
					    CityName = x.LeadPolicy.Leads.CityName ?? "",
					    StateName = x.LeadPolicy.Leads.StateName ?? "",
					    Zip = x.LeadPolicy.Leads.Zip ?? "",

					    policyID = x.LeadPolicy.Id

				    });

			return leads;
		}

		public static List<Leads> GetPredicate(Expression<Func<Leads, bool>> predicate) {
            return DbContextHelper.DbContext.Leads.
                AsExpandable().
                Where(predicate).
                OrderBy(o => o.ClaimantLastName).ToList();
		}
		public static List<Claim> GetPredicate(Expression<Func<Claim, bool>> predicate) {
			return DbContextHelper.DbContext.Claim
				.AsExpandable()
				.Where(predicate)
				.OrderBy(o => o.LeadPolicy.Leads.ClaimantLastName)
				.ToList();
		}
		public static List<LeadView> GetLeads(Expression<Func<Leads, bool>> predicate) {
			List<Leads> leads = GetPredicate(predicate);

			List<LeadView> listView = null;
			LeadView leadView = null;
			string[] claimNumbers = null;
			string[] companynames = null;
			string[] statusNames = null;
			string[] statusCodes = null;

			if (leads != null) {
				listView = new List<LeadView>();

				foreach (Leads x in leads) {
					leadView = new LeadView();
					leadView.LeadId = x.LeadId;

					leadView.ClaimantLastName = x.ClaimantLastName;
					leadView.ClaimantFirstName = x.ClaimantFirstName;
					leadView.InsuredName = x.InsuredName;
					leadView.BusinessName = x.BusinessName;

					leadView.Email = x.EmailAddress;

					leadView.OriginalLeadDate = x.OriginalLeadDate;

					// get claim numbers
					claimNumbers = (from p in x.LeadPolicy
								 from c in p.Claim
								 where c.IsActive == true
								 select c.AdjusterClaimNumber).ToArray();

					leadView.ClaimNumber = string.Join("<br>", claimNumbers);

					// get claim status
					statusNames = (from p in x.LeadPolicy
								from c in p.Claim
								where c.IsActive == true && c.StatusMaster != null
								select c.StatusMaster.StatusName).ToArray();

					leadView.StatusName = string.Join("<br>", statusNames);

					// get claim status code
					statusCodes = (from p in x.LeadPolicy
								from c in p.Claim
								where c.IsActive == true && c.StatusMaster != null
								select c.StatusMaster.StatusId.ToString()).ToArray();

					leadView.StatusCodes = string.Join("<br>", statusNames);

					// loss date
					leadView.LossDates = (from p in x.LeadPolicy
									  from c in p.Claim
									  where c.IsActive == true && c.LossDate != null
									  select c.LossDate).ToArray();

                    string[] adjusterNames = (from p in x.LeadPolicy
										 from c in p.Claim
										 where c.IsActive == true && c.AdjusterMaster != null
										 select c.AdjusterMaster.adjusterName).ToArray();

					leadView.AdjusterName = string.Join("<br>", adjusterNames);

					string[] coverages = (from e in x.LeadPolicy where e.IsActive && e.PolicyType != null select e.LeadPolicyType.Description).ToArray();
					leadView.Coverage = string.Join("<br>", coverages);


					string[] claimSubStatues = (from p in x.LeadPolicy
										   from c in p.Claim
										   where c.IsActive == true && c.SubStatusMaster != null
										   select c.SubStatusMaster.SubStatusName).ToArray();
					leadView.SubStatusName = string.Join(" ", claimSubStatues);


					//companynames = (from p in x.LeadPolicies where p.IsActive && p.Carrier != null select p.Carrier.CarrierName).ToArray();
					companynames = (from p in x.LeadPolicy where p.IsActive && p.InsuranceCompanyName != null select p.InsuranceCompanyName).ToArray();

					leadView.InsuranceCompanyName = string.Join("<br>", companynames);

                    string[] claimWorkFlowTypes = (from p in x.LeadPolicy
											from c in p.Claim
											where c.IsActive == true && c.ClaimWorkflowType != null
											select c.ClaimWorkflowType).ToArray();
					leadView.ClaimWorkflowType = string.Join("<br>", claimWorkFlowTypes);

                    string[] eventTypes = (from p in x.LeadPolicy
											from c in p.Claim
											where c.IsActive == true && c.EventType != null
											select c.EventType).ToArray();
					leadView.EventType = string.Join("<br>", eventTypes);

                    string[] eventNames = (from p in x.LeadPolicy
									   from c in p.Claim
									   where c.IsActive == true && c.EventName != null
									   select c.EventName).ToArray();
					leadView.EventName = string.Join("<br>", eventNames);

                    int[] severities = (from p in x.LeadPolicy
									   from c in p.Claim
									   where c.IsActive == true && c.SeverityNumber != null
									   select c.SeverityNumber).Cast<int>().ToArray();

					//leadView.Severity = string.Join("<br>", severities.Select(s => s.ToString()).ToArray());


					// get it from claim
                    string[] causeOfLoss = (from p in x.LeadPolicy
									    from c in p.Claim
									    where c.IsActive == true && c.CauseOfLoss != null
									    select c.CauseOfLoss).ToArray();

					if (causeOfLoss != null && causeOfLoss.Length > 0) {
						// get it from claim
						leadView.TypeofDamageText = "";
						foreach (string idList in causeOfLoss) {
							if (!string.IsNullOrEmpty(idList)) {
								string[] damageDescriptions = TypeofDamageManager.GetDescriptions(idList);
								leadView.TypeofDamageText = string.Join("<br>", damageDescriptions);
							}
						}
					}
					else if (!string.IsNullOrEmpty(x.TypeOfDamage)) {
						// get it from Lead
						leadView.TypeofDamageText = string.Join("<br>", x.TypeofDamageText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
					}


					leadView.CityName = x.CityName;
					leadView.StateName = x.StateName;
					leadView.Zip = x.Zip;
					leadView.MailingCity = x.MailingCity;
					leadView.MailingState = x.MailingState;
					leadView.MailingZip = x.MailingZip;
					leadView.LeadSourceName = x.LeadSourceMaster == null ? "" : x.LeadSourceMaster.LeadSourceName;

					leadView.TypeOfProperty = x.TypeOfPropertyMaster == null ? "" : x.TypeOfPropertyMaster.TypeOfProperty;
					leadView.ContractorName = x.ContractorMaster == null ? "" : x.ContractorMaster.ContractorName;
					leadView.AppraiserName = x.AppraiserMaster == null ? "" : x.AppraiserMaster.AppraiserName;
					leadView.UmpireName = x.UmpireMaster == null ? "" : x.UmpireMaster.UmpireName;
					leadView.ProducerName = x.ProducerMaster == null ? "" : x.ProducerMaster.ProducerName;
					leadView.UserName = x.SecUser == null ? "" : x.SecUser.UserName;

					//leadView.LossDate = x.LossDate;
					leadView.SiteSurveyDate = x.SiteSurveyDate;

					leadView.LastActivityDate = x.LastActivityDate;

					listView.Add(leadView);
				}
			}
			return listView;

		}

		public static List<LeadView> GetLeads(Expression<Func<Leads, bool>> predicate, string sortExpression, bool descending) {
			string sortClause = null;

			List<LeadView> listView = GetLeads(predicate);

			if (listView != null) {
				sortClause = sortExpression + (descending ? " Desc " : " Asc ");

				listView = listView.sort(sortClause).ToList();

			}

			return listView;

		}

		public static Leads Save(Leads objLead) {
			if (objLead.LeadId == 0) {

				//objLead.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objLead.InsertDate = DateTime.Now;
				objLead.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objLead);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objLead.UpdateMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objLead.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objLead;
		}

		//public static LeadLog SaveLeadLog(LeadLog objLeadLog) {
		//	if (objLeadLog.LeadLogId == 0) {

		//		objLeadLog.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
		//		objLeadLog.InsertDate = DateTime.Now;
		//		objLeadLog.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
		//		DbContextHelper.DbContext.AddToLeadLogs(objLeadLog);
		//	}

		//	////secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
		//	//objLeadLog.UpdateMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
		//	//objLeadLog.UpdateDate = DateTime.Now;
		//	DbContextHelper.DbContext.SaveChanges();

		//	return objLeadLog;
		//}

		public static Leads GetByLeadId(int LeadId) {
			var users = from x in DbContextHelper.DbContext.Leads
					  where x.LeadId == LeadId
					  select x;

			return users.Any() ? users.First() : new Leads();
		}

		public static string GetLeadName(int LeadId) {
			string claimantName = (from x in DbContextHelper.DbContext.Leads
							   where x.LeadId == LeadId
							   select x.ClaimantFirstName.Trim() + " " + x.ClaimantLastName.Trim()
					  ).FirstOrDefault();

			return claimantName;
		}
		public static string GetBusinessName(int LeadId) {
			string businessName = (from x in DbContextHelper.DbContext.Leads
							   where x.LeadId == LeadId
							   select x.BusinessName
					  ).FirstOrDefault();

			return businessName;
		}
		/// <summary>
		/// Returns Lead (Full object)
		/// </summary>
		/// <param name="LeadId"></param>
		/// <returns></returns>
		public static Leads Get(int LeadId) {
			Leads lead = (from x in DbContextHelper.DbContext.Leads
						   //.Include("StateMaster")
						   //.Include("CityMaster")
							.Include("LeadPolicy")
							.Include("LeadPolicy.StateMaster")
							.Include("LeadPolicy.CityMaster")
							.Include("LeadPolicy.LeadPolicyType")
							.Include("LeadPolicy.Carrier")
							.Include("LeadSourceMaster")
						   //.Include("AdjusterMaster")
							.Include("AppraiserMaster")
							.Include("ContractorMaster")
							.Include("UmpireMaster")
							.Include("Client")
					   where x.LeadId == LeadId
					   select x).FirstOrDefault();

			return lead;
		}

		public static Leads GetByLeadByUserId(int UserId) {
			var users = from x in DbContextHelper.DbContext.Leads
					  where x.UserId == UserId
					  select x;

			return users.Any() ? users.First() : new Leads();
		}

		public static Leads GetByLeadByEmailPhoneLastName(string email, string phone, string lastName) {
			Leads lead = (from x in DbContextHelper.DbContext.Leads
					   where x.EmailAddress == email &&
							 x.PhoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") == phone &&
							 x.ClaimantLastName == lastName
					   select x).FirstOrDefault();

			return lead;
		}

		public static Leads GetByLeadByLastNameClaimNumber(string lastName, string claimNumber) {
			Leads lead = (from x in DbContextHelper.DbContext.Leads
							.Include("Client")
							.Include("LeadPolicy")
							.Include("LeadPolicy.AdjusterMaster")
							.Include("LeadPolicy.StatusMaster")
							.Include("LeadPolicy.SubStatusMaster")
					   where x.ClaimantLastName == lastName.Trim() &&
							x.LeadPolicy.Any(y => y.ClaimNumber.Contains(claimNumber.Trim()))
					   select x).FirstOrDefault();

			return lead;
		}


		public static List<Leads> GetAll() {
			var list = from x in DbContextHelper.DbContext.Leads
					 where x.Status == 1
					 select x;

			return list.ToList();
		}

		static public List<CurrentLeadPolicy> GetCurrentPolicy(int leadID) {
			List<CurrentLeadPolicy> currentPolicyTypes = null;


			currentPolicyTypes = (from x in DbContextHelper.DbContext.LeadPolicy
							  where x.IsActive && x.LeadId == leadID
							  select new CurrentLeadPolicy {
								  policyTypeDescription = x.LeadPolicyType.Description,
								  policyTypeID = x.LeadPolicyType.LeadPolicyTypeID
							  }).ToList();


			return currentPolicyTypes;
		}

		public static List<string> GetLeadEmails() {
			List<string> emails;

			emails = (from x in DbContextHelper.DbContext.Leads
					where x.Status == 1 &&
					x.EmailAddress != null
					select x.EmailAddress
					).ToList<string>();

			return emails;
		}

		public static List<string> GetLeadEmails(int clientID) {
			List<string> emails;

			emails = (from x in DbContextHelper.DbContext.Leads
					where x.Status == 1 &&
					x.EmailAddress != null &&
					x.ClientID == clientID
					select x.EmailAddress
					).ToList<string>();

			return emails;
		}

		public static int GetOpenLeadCount(int clientID) {
			int count = 0;
			Expression<Func<Leads, bool>> predicate = PredicateBuilder.True<Leads>();
			predicate = predicate.And(Lead => Lead.Status != 0);
			predicate = predicate.And(Lead => Lead.UserId != 0);
			predicate = predicate.And(Lead => Lead.ClientID == clientID);

			int[] statusMasterIDs = StatusManager.GetOpenStatusIDs(clientID);

			count = DbContextHelper.DbContext.Leads.Include("LeadPolicy").Count(x => (int)x.ClientID == clientID
				&& x.Status == 1
				&& x.LeadPolicy.Any(q => q.IsActive && q.LeadStatus != null && statusMasterIDs.Contains(q.StatusMaster.StatusId)));

			//List<vw_openLeadClaim> stat = DbContextHelper.DbContext.vw_openLeadClaims.Where(x => x.clientid == clientID).ToList();
			//if (stat != null)
			//     count = stat.Sum(s => s.itemcount);

			return count;
		}

		public static int GetCloseLeadCount(int clientID) {
			int count = 0;

			int[] statusMasterIDs = StatusManager.GetClosedStatusIDs(clientID);

			count = DbContextHelper.DbContext.Leads.Include("LeadPolicy").Count(x => x.ClientID == clientID
				&& x.Status == 1
				&& x.LeadPolicy.Any(q => q.IsActive && q.LeadStatus != null && statusMasterIDs.Contains(q.StatusMaster.StatusId)));

			//List<vw_closeLeadClaim> stat = DbContextHelper.DbContext.vw_closeLeadClaims.Where(x => x.clientid == clientID).ToList();
			//if (stat != null)
			//     count = stat.Sum(s => s.itemcount);

			return count;
		}

		public static void UpdateLastActivityDate(int leadID) {
            Leads lead=DbContextHelper.DbContext.Leads.Where(x => x.LeadId == leadID).FirstOrDefault();
            lead.LastActivityDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

			//DbContextHelper.DbContext.ExecuteStoreCommand("UPDATE Leads SET LastActivityDate = {0} WHERE LeadId = {1}", DateTime.Now, leadID);
		}

        public static void UpdateLossAmount(Leads objLeads)
        {
            Leads lead = DbContextHelper.DbContext.Leads.Where(x => x.LeadId == objLeads.LeadId).FirstOrDefault();
            lead.LossOfUseAmount = objLeads.LossOfUseAmount;
            lead.LossOfUseReserve = objLeads.LossOfUseReserve;
            DbContextHelper.DbContext.SaveChanges();          
        }
		//public static bool IsSubmittedExist(int leadId)
		//{
		//    var users = from x in DbContextHelper.DbContext.Leads
		//                where x.UserName == userName
		//                select x;

		//    return users.Any();
		//}
	}
}
