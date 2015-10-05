namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class AdjusterManager {
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			AdjusterMaster adjuster = new AdjusterMaster { AdjusterId = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("AdjusterMaster", adjuster);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(adjuster);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static IQueryable<AdjusterMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.AdjusterMaster
					 where x.Status == true
					 orderby x.AdjusterName
					 select x;

			return list;
		}

		public static IQueryable<AdjusterMaster> Search(Expression<Func<AdjusterMaster, bool>> predicate) {

			return DbContextHelper.DbContext.AdjusterMaster
						.AsExpandable()
						.Where(predicate);

		}

		public static IQueryable<AdjusterMaster> GetAll(int clientID) {
			IOrderedQueryable<AdjusterMaster> adjusters = null;

			adjusters = from x in DbContextHelper.DbContext.AdjusterMaster
						  //.Include("DeploymentStateMaster")
						  .Include("AdjusterHandleClaimType")
						  .Include("AdjusterHandleClaimType.LeadPolicyType")
						   .Include("AdjusterServiceArea")
					  where x.Status == true && x.ClientId == clientID
					  orderby x.AdjusterName
					  select x;

			return adjusters;
		}

		public static AdjusterMaster GetByAdjusterName(string adjusterName) {

			var adjuster = from x in DbContextHelper.DbContext.AdjusterMaster
						where x.AdjusterName == adjusterName && x.Status == true
						select x;

			return adjuster.Any() ? adjuster.First() : new AdjusterMaster();
		}
		public static AdjusterMaster GetByAdjusterName(int clientID, string adjusterName) {

			var adjuster = from x in DbContextHelper.DbContext.AdjusterMaster
						where x.AdjusterName == adjusterName && x.Status == true && x.ClientId == clientID
						select x;

			return adjuster.Any() ? adjuster.First() : new AdjusterMaster();
		}

		public static AdjusterMaster GetAdjusterId(int adjusterId) {
			var adjuster = from x in DbContextHelper.DbContext.AdjusterMaster
						where x.AdjusterId == adjusterId && x.Status == true
						select x;

			return adjuster.Any() ? adjuster.First() : new AdjusterMaster();
		}

        public static  int GetAdjusterIdForNewUser()// OC - 9/4/14
        {
            var adjuster = (from x in DbContextHelper.DbContext.AdjusterMaster
                           orderby x.AdjusterId descending
                           select x.AdjusterId).Take(1);

            return adjuster.SingleOrDefault();
        }

		public static string GetAdjusterName(int adjusterId) {
			string adjusterName = (from x in DbContextHelper.DbContext.AdjusterMaster
							   where x.AdjusterId == adjusterId && x.Status == true
							   select x.AdjusterName
						).FirstOrDefault();

			return adjusterName;
		}

		public static AdjusterMaster GetAdjusterByUserID(int userID) {
			AdjusterMaster adjuster = (from x in DbContextHelper.DbContext.AdjusterMaster
								  where x.userID == userID && x.Status == true
								  select x
						).FirstOrDefault();

			return adjuster;
		}

		public static int? GetAdjusterSupervisor(int adjusterId) {
			return (from x in DbContextHelper.DbContext.AdjusterMaster
				   where x.AdjusterId == adjusterId
				   select x.SupervisorID
						).FirstOrDefault();

		}

		/// <summary>
		/// Returns complete adjuster object
		/// </summary>
		/// <param name="adjusterId"></param>
		/// <returns></returns>
		public static AdjusterMaster Get(int adjusterId) {
			var adjuster = from x in DbContextHelper.DbContext.AdjusterMaster.Include("Client")
						where x.AdjusterId == adjusterId && x.Status == true
						select x;

			return adjuster.Any() ? adjuster.First() : new AdjusterMaster();
		}

		public static List<AdjusterView> GetClaimAssigned(int clientID) {
			List<AdjusterView> adjusters = null;

			adjusters = (from x in DbContextHelper.DbContext.AdjusterMaster
					   where x.Status == true && x.ClientId == clientID
					   orderby x.AdjusterName
					   select new AdjusterView {
						   AdjusterID = x.AdjusterId,
						   AdjusterName = x.AdjusterName,
						   MumberOfClaims = (
									DbContextHelper.DbContext.Claim.Where(p => p.AdjusterID == x.AdjusterId && p.IsActive == true).Count()
									)
					   }).ToList<AdjusterView>();

			return adjusters;

		}
		public static bool IsExist(string adjusterName, int clientID) {
			var status = from x in DbContextHelper.DbContext.AdjusterMaster
					   where x.AdjusterName == adjusterName && x.ClientId == clientID && x.Status == true
					   select x;

			return status.Any();
		}

		public static AdjusterMaster Save(AdjusterMaster objAdjuster) {
			if (objAdjuster.AdjusterId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objAdjuster.InsertDate = DateTime.Now;
				DbContextHelper.DbContext.Add(objAdjuster);
			}

			objAdjuster.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objAdjuster;
		}

        public static AdjusterMaster GetByClientID(int clientID)
        {

            var adjuster = from x in DbContextHelper.DbContext.AdjusterMaster
                           where  x.Status == true && x.ClientId == clientID
                           select x;

            return adjuster.Any() ? adjuster.First() : new AdjusterMaster();
        }

        public static bool GetAdjusterSttingPayroll(int adjusterId)
        {
            var adjuster = from x in DbContextHelper.DbContext.AdjusterSettingsPayroll
                           where x.AdjusterId == adjusterId 
                           select x;

            return adjuster.Any() ? true: false;
        }
        public static AdjusterSettingsPayroll GetAdjusterSttingPayrollData(int adjusterId)
        {
            var adjusterSetting = from x in DbContextHelper.DbContext.AdjusterSettingsPayroll
                           where x.AdjusterId == adjusterId
                           select x;

            return adjusterSetting.Any() ? adjusterSetting.First() : new AdjusterSettingsPayroll();
        }

        public static AdjusterSettingsPayroll SavePayrollSetting(AdjusterSettingsPayroll objSettingsPayroll)
        {
           
                DbContextHelper.DbContext.Add(objSettingsPayroll);              
                DbContextHelper.DbContext.SaveChanges();
                return objSettingsPayroll;
        }

        public static void UpdatePayrollSetting(AdjusterSettingsPayroll objSettingsPayroll)
        {
            AdjusterSettingsPayroll objSettingsPayroll2 = DbContextHelper.DbContext.AdjusterSettingsPayroll.First(x => x.AdjusterId == objSettingsPayroll.AdjusterId);
            objSettingsPayroll2.AdjusterBranch = objSettingsPayroll.AdjusterBranch;
            objSettingsPayroll2.BranchCode = objSettingsPayroll.BranchCode;

            objSettingsPayroll2.Rating = objSettingsPayroll.Rating;
            objSettingsPayroll2.QAScore = objSettingsPayroll.QAScore;
            objSettingsPayroll2.Designation = objSettingsPayroll.Designation;
            objSettingsPayroll2.AdjustingExperience = objSettingsPayroll.AdjustingExperience;
            objSettingsPayroll2.MaximumNumberofClaims = objSettingsPayroll.MaximumNumberofClaims;
            objSettingsPayroll2.MaximumReserves = objSettingsPayroll.MaximumReserves;
            objSettingsPayroll2.NationalProducerId = objSettingsPayroll.NationalProducerId;
            objSettingsPayroll2.GeographicalAreaofService = objSettingsPayroll.GeographicalAreaofService;
            objSettingsPayroll2.IsActive = objSettingsPayroll.IsActive;
            objSettingsPayroll2.Supervisor = objSettingsPayroll.Supervisor;
            objSettingsPayroll2.EmployeeType = objSettingsPayroll.EmployeeType;
            objSettingsPayroll2.DefaultAdjusterHourlyRate = objSettingsPayroll.DefaultAdjusterHourlyRate;
            objSettingsPayroll2.DefaultAdjusterCommissionRate = objSettingsPayroll.DefaultAdjusterCommissionRate;
            objSettingsPayroll2.IndependenContractorAgreementOnFile = objSettingsPayroll.IndependenContractorAgreementOnFile;
            objSettingsPayroll2.LastYear1099AgreementonFile = objSettingsPayroll.LastYear1099AgreementonFile;
            objSettingsPayroll2.ResumeOnFile = objSettingsPayroll.ResumeOnFile;

            DbContextHelper.DbContext.SaveChanges();


        }

        public static List<DeployementAddressData> GetAllDeployementAddress(int userId)
        {
            var list = from x in DbContextHelper.DbContext.AdjusterDeploymentAddress
                       join y in DbContextHelper.DbContext.StateMaster on x.States equals y.StateId
                       where x.UserId == userId && x.IsActive==true
                       select new DeployementAddressData
                       {
                           Id=x.Id,
                           DeploymentAddress=x.DeploymentAddress+" "+x.DeploymentAddress2+" "+x.City+" "+y.StateName+" "+x.ZipCode,
                       };


            return list.ToList();
            //return null;
        }

        public static AdjusterDeploymentAddress GetDeployementAddress(int id)
        {
            var list = from x in DbContextHelper.DbContext.AdjusterDeploymentAddress
                                              where x.Id == id
                                           select x;

            return list.Any() ? list.First() : new AdjusterDeploymentAddress();         
           
        }

        public static AdjusterDeploymentAddress SaveDeploymentAddress(AdjusterDeploymentAddress objAdjusterDeploymentAddress)
        {

            DbContextHelper.DbContext.Add(objAdjusterDeploymentAddress);           
            DbContextHelper.DbContext.SaveChanges();

            return objAdjusterDeploymentAddress;
        }

        public static List<AdjusterDeploymentEvent> GetDeploymentEvent(int adjusterId)
        {
            List<AdjusterDeploymentEvent> limits = null;

            limits = (from x in DbContextHelper.DbContext.AdjusterDeploymentEvent
                      where x.AdjusterId == adjusterId
                      select x).ToList<AdjusterDeploymentEvent>();

            return limits;
        }

        public static AdjusterDeploymentEvent SaveEvent(AdjusterDeploymentEvent objAdjusterDeploymentEvent)
        {
            //if (objAdjusterDeploymentEvent.Id == 0)
            //{
                DbContextHelper.DbContext.Add(objAdjusterDeploymentEvent);
           // }            
            DbContextHelper.DbContext.SaveChanges();
            return objAdjusterDeploymentEvent;
        }

        /// <summary>
        /// Select all adjuster by branch code
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public static IQueryable<AdjusterMaster> GetAllByBranch(int clientID, string branchId)
        {
            IQueryable<AdjusterMaster> adjusters = null;

            adjusters = from x in DbContextHelper.DbContext.AdjusterMaster
                            //.Include("DeploymentStateMaster")
                          .Include("AdjusterHandleClaimType")
                          .Include("AdjusterHandleClaimType.LeadPolicyType")
                           .Include("AdjusterServiceArea")
                        join asp in DbContextHelper.DbContext.AdjusterSettingsPayroll
                        on x.AdjusterId equals asp.AdjusterId
                        where x.Status == true && x.ClientId == clientID && asp.BranchCode == branchId
                        orderby x.AdjusterName
                        select x;

            return adjusters;
        }

        public static IQueryable<AdjusterSettingsPayroll> GetGetAllAdjusterPayrollSetting(int clientId)
        {
            IQueryable<AdjusterSettingsPayroll> objSettingsPayroll = null;

            objSettingsPayroll = from x in DbContextHelper.DbContext.AdjusterSettingsPayroll
                                 join master in DbContextHelper.DbContext.AdjusterMaster
                                 on x.AdjusterId equals master.AdjusterId
                                 where master.Status == true && master.ClientId == clientId && x.AdjusterBranch != null
                                 orderby x.AdjusterBranch
                                 select x;
            return objSettingsPayroll;
        }

	}
}
