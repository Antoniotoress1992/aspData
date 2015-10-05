﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Views;
using CRM.Data.Entities;

namespace CRM.Repository
{
    public class RuleExceptionManager : IDisposable
    {
        private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;
        public RuleExceptionManager()
        {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
        }

        public IQueryable<RuleException> GetAll(int clientID)
        {
            IQueryable<RuleException> exceptions = null;

            exceptions = from x in claimRulerDBContext.RuleException
                         where x.ClientID == clientID
                         select x;

            return exceptions;
        }
        public List<RuleException> GetAllException(int clientId) {


            List<RuleException> rules = (from x in claimRulerDBContext.RuleException
                                where x.IsActive == true &&
                                 x.ClientID == clientId
                                select x
                      ).ToList<RuleException>();

            return rules;
        }
        public void format(int clientID, int userID, int claimId) 
        {
            var query =
                from x in claimRulerDBContext.RuleException
                where 
                      x.ClientID == clientID &&
                      x.UserID == userID &&
                      x.ObjectID == claimId
                select x;



            foreach (RuleException x in query)
            {
                x.IsActive = false;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database. 
            try
            {
                claimRulerDBContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        
        }

        public void update(int BusinessRuleId, int clientID, int userID, int claimId)
        {
             var query =
                from x in claimRulerDBContext.RuleException
                where x.BusinessRuleID == BusinessRuleId &&
                      x.ClientID == clientID &&
                      x.UserID == userID &&
                      x.ObjectID == claimId
                select x;



             foreach (RuleException x  in query)
            {
                x.IsActive = true;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database. 
             try
             {
                 claimRulerDBContext.SaveChanges();
             }
             catch (Exception ex) {
                
             }
        }
        public RuleException Save(RuleException exception)
        {
            if (exception.RuleExceptionID == 0)
            {
                exception.ExceptionDate = DateTime.Now;
                exception.IsActive = true;
                claimRulerDBContext.RuleException.Add(exception);
            }

            claimRulerDBContext.SaveChanges();

            return exception;
        }


        public RuleException GetRuleException(int businessruleId, int clientId, int userId, int claimId) 
        { 
            RuleException ruleExceptionObj = new  RuleException();
            ruleExceptionObj = (from x in claimRulerDBContext.RuleException
                               where x.ClientID == clientId &&
                               x.BusinessRuleID == businessruleId &&
                               x.UserID == userId &&
                               x.ObjectID == claimId 
                              
                                select x).FirstOrDefault<RuleException>();
            return ruleExceptionObj;
            
        }

        public List<RuleExceptionStatisticsView> GetExceptionStatistics(int clientID)
        {
            List<RuleExceptionStatisticsView> statistics = null;

            List<RuleExceptionStatisticsView> exceptionStats = (from x in claimRulerDBContext.RuleException
                                                      .Include("BusinessRule.Rule")
                                                                where x.ClientID == clientID && x.IsActive
                                                                group x by new { x.BusinessRule.RuleID } into g
                                                                select new RuleExceptionStatisticsView
                                                                {
                                                                    RuleID = (int)g.Key.RuleID,
                                                                    ExceptionCount = g.Count()
                                                                }).ToList();

            List<Rule> rules = (from x in claimRulerDBContext.Rule
                                where x.IsActive == true
                                orderby x.RuleID descending
                                select x
                      ).ToList<Rule>();

            statistics = (from x in rules
                          join c in exceptionStats
                          on x.RuleID equals c.RuleID into left_join
                          from leftjoin in left_join.DefaultIfEmpty(new RuleExceptionStatisticsView())	// include row from left side of join					   
                          select new RuleExceptionStatisticsView
                          {
                              RuleID = (int)x.RuleID,
                              RuleName = x.RuleName,
                              ExceptionCount = leftjoin.ExceptionCount
                          }).ToList();

            return statistics;
        }


        public List<RuleExceptionView> GetByRuleID(int clientID, int RuleID)
        {
            List<RuleExceptionView> exceptions = null;

            exceptions = (from x in claimRulerDBContext.RuleException
                          .Include("BusinessRule.Rule")
                          .Include("RuleObject")
                          .Include("Claim")
                          
                          where x.ClientID == clientID &&
                                  x.BusinessRule.Rule.RuleID == RuleID &&
                                  x.IsActive == true 
                                  
                          select new RuleExceptionView
                          {
                              BusinessRuleID = x.BusinessRule.BusinessRuleID,
                              RuleID = x.BusinessRule.RuleID,
                              BusinessRuleDescription = x.BusinessRule.Description,
                              ExceptionDate = x.ExceptionDate,
                              UserID = x.UserID,
                              UserName = x.SecUser.UserName,
                              ObjectName = x.RuleObject.ObjectName,
                              ObjectTypeID = x.ObjectTypeID,
                              ObjectID = x.ObjectID

                          }
                   ).ToList<RuleExceptionView>();

            return exceptions;
        }

        public bool CheckForException(int clientID, int businessRuleID, int objectID, int objectTypeID)
        {
            bool isApplied = false;

            isApplied = claimRulerDBContext.RuleException.Any(
                        x => x.ClientID == clientID &&
                            x.BusinessRuleID == businessRuleID &&
                            x.ObjectID == objectID &&
                            x.ObjectTypeID == objectTypeID &&
                            x.IsActive);

            return isApplied;
        }
        #region ===== memory management =====

        public void Dispose()
        {
            // Perform any object clean up here.

            // If you are inheriting from another class that
            // also implements IDisposable, don't forget to
            // call base.Dispose() as well.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (claimRulerDBContext != null)
                    {

                        claimRulerDBContext.Dispose();
                    }
                }

                disposed = true;
            }
        }
        #endregion
    }
}
