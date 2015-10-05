using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class CarrierTaskManager {
		
		public static List<Task> GetAll(int carrierID) {
			List<Task> comments = null;

			comments = (from x in DbContextHelper.DbContext.Task.Include("SecUser")
					  where x.CarrierID == carrierID
					  orderby x.start_date ascending
					  select x).ToList<Task>();

			return comments;
		}


        public static List<TaskPriority> GetAllPriority()
        {
            var list = from x in DbContextHelper.DbContext.TaskPriority
                       where x.IsActive == true
                       select x;
            return list.ToList();
            
        }



	}
}
