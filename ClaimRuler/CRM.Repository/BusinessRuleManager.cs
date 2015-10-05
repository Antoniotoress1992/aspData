using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class BusinessRuleManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public BusinessRuleManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}


		public List<Rule> GetAll() {
			List<Rule> rules = null;

			rules = (from x in claimRulerDBContext.Rule
				    where x.IsActive == true
				    select x
					  ).ToList<Rule>();

			return rules;
		}
		public Rule GetRule(int id) {
			Rule rule = null;

			rule = (from x in claimRulerDBContext.Rule
				   where x.RuleID == id
				   select x
					  ).FirstOrDefault<Rule>();

			return rule;
		}

		public BusinessRule GetBusinessRule(int id) {
			BusinessRule rule = null;

			rule = (from x in claimRulerDBContext.BusinessRule
				   .Include("Rule")

				   where x.BusinessRuleID == id
				   select x
					  ).FirstOrDefault<BusinessRule>();

			return rule;
		}

         

		public List<BusinessRule> GetBusinessRules(int clientID) {
			List<BusinessRule> rules = null;

            rules = (from x in claimRulerDBContext.BusinessRule
                    .Include("Rule")
                    .Include("Client")
                     where x.ClientID == clientID &&
                           x.Client.ClientId == clientID
                     orderby x.Rule.RuleID
                     select x
                      ).ToList<BusinessRule>();


			return rules;
		}

		public List<BusinessRule> GetBusinessRules(int clientID, Globals.RuleType ruleType) {
			List<BusinessRule> rules = null;
			int ruleTypeID = (int)ruleType;

			rules = (from x in claimRulerDBContext.BusinessRule
				    .Include("Rule")
				    where x.ClientID == clientID &&
						x.RuleID == ruleTypeID				    
				    select x
				).ToList<BusinessRule>();

			return rules;
		}

		public List<BusinessRule> GetBusinessRules(Globals.RuleType ruleType) {
			List<BusinessRule> rules = null;
			int ruleTypeID = (int)ruleType;

			rules = (from x in claimRulerDBContext.BusinessRule
				    .Include("Rule")
				    where x.RuleID == ruleTypeID
				    select x
				).ToList<BusinessRule>();

			return rules;
		}

        public List<BusinessRule> GetBusinessRuleThread(int clinetId, int ruleId) 
        {
            List<BusinessRule> rules = null;
            int ruleTypeID = ruleId;

            rules = (from x in claimRulerDBContext.BusinessRule
                    .Include("Rule")
                     where x.ClientID == clinetId &&
                         x.RuleID == ruleTypeID &&
                         x.IsActive == true
                     select x
                ).ToList<BusinessRule>();

            return rules;
            
        }

		public BusinessRule Save(BusinessRule rule) {
			if (rule.BusinessRuleID == 0)
				claimRulerDBContext.BusinessRule.Add(rule);

			rule.UpdateDate = DateTime.Now;

			claimRulerDBContext.SaveChanges();

			return rule;
		}


        public AdjusterMaster GetAdjuster(int adjusterID)
        {
            AdjusterMaster rule = null;

            rule = (from x in claimRulerDBContext.AdjusterMaster
                    where x.AdjusterId == adjusterID
                    select x
                      ).FirstOrDefault<AdjusterMaster>();

            return rule;
        }

        public CRM.Data.Entities.SecUser GetSupervisor(int supervisorId)
        {
            CRM.Data.Entities.SecUser user = null;

            user = (from x in claimRulerDBContext.SecUser
                    where x.UserId == supervisorId
                    select x
                      ).FirstOrDefault<CRM.Data.Entities.SecUser>();

            return user;
        }

        public Claim GetClaim(int claimId)
        {
            Claim claim = null;

            claim = (from x in claimRulerDBContext.Claim
                     where x.ClaimID == claimId 
                    select x
                      ).FirstOrDefault<Claim>();

            return claim;
        }

		#region ===== memory management =====

		public void Dispose() {
			// Perform any object clean up here.

			// If you are inheriting from another class that
			// also implements IDisposable, don't forget to
			// call base.Dispose() as well.
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					if (claimRulerDBContext != null) {

						claimRulerDBContext.Dispose();
					}
				}

				disposed = true;
			}
		}
		#endregion
	}
}
