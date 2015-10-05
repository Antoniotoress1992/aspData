using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CRM.Data.Entities;
using System.Linq.Expressions;
using LinqKit;

namespace CRM.Data.Account
{
    public class ActivityManager 
    {
        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;
        private bool disposed = false;
        public static List<Activity> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.Activity
                       orderby x.Activity1
                       select x;
            return list.ToList();
            
        }
        public static Activity GetActivityById(int activityId)
        {

            var activities = from x in DbContextHelper.DbContext.Activity
                        where x.ActivityID == activityId 
                        select x;

            return activities.Any() ? activities.First() : new Activity();
        }
        public static Activity GetByAcctivity(string myActivity)
        {

            var activities = from x in DbContextHelper.DbContext.Activity
                             where x.Activity1 == myActivity
                             select x;

            return activities.Any() ? activities.First() : new Activity();
        }
        public static Activity Delete(int activityId)
        {
            
            Activity act = DbContextHelper.DbContext.Activity.Single(x => x.ActivityID == activityId);
            DbContextHelper.DbContext.Activity.Remove(act);
            DbContextHelper.DbContext.SaveChanges();

            var activities = from x in DbContextHelper.DbContext.Activity
                             where x.ActivityID == activityId
                             //               
                             select x;
            
           // DbContextHelper.DbContext.Activity.Remove(activities);

            return activities.Any() ? activities.First() : new Activity();
        }
        public static Activity Save(Activity myActivity)
        {
            //objLead.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            if (myActivity.ActivityID == 0)
            {
                DbContextHelper.DbContext.Add(myActivity);
                //claimRulerDBContext.Activity.Add(myActivity);
            }
            DbContextHelper.DbContext.SaveChanges();
            //claimRulerDBContext.SaveChanges();
            return myActivity;
        }

 
			
    }
}
