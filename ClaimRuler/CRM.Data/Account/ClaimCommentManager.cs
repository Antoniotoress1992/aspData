using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class ClaimCommentManager {
        public static List<ClaimCommented> GetAll(int claimID)
        {
            List<ClaimCommented> comments = null;

            comments = (from x in DbContextHelper.DbContext.ClaimComment
                        join y in DbContextHelper.DbContext.SecUser on x.UserId equals y.UserId
                        where x.ClaimID == claimID
                        orderby x.CommentDate descending
                        select new ClaimCommented
                        {
                         CommentID=x.CommentID,
                         UserName=y.UserName,
                         CommentText=x.CommentText,
                         CommentDate=x.CommentDate,
                         ActivityType=x.ActivityType,
                         InternalComments = x.InternalComments
                        
                        }
                    ).ToList();

			return comments;
		}

		public static ClaimComment Get(int id) {
			ClaimComment comment = null;

			comment = (from x in DbContextHelper.DbContext.ClaimComment
					  where x.CommentID == id
					  select x
					).FirstOrDefault<ClaimComment>();

			return comment;
		}

		public static ClaimComment Save(ClaimComment comment) {

            if (comment.CommentID == 0)
                DbContextHelper.DbContext.Add(comment);

			DbContextHelper.DbContext.SaveChanges();

			return comment;
		}


        public static void Delete(int id)
        {
            // Create an entity to represent the Entity you wish to delete 
            // Notice you don't need to know all the properties, in this 
            // case just the ID will do. 
            ClaimComment comment = Get(id);

            if (comment != null)
            {
                // Do the delete the category 
                DbContextHelper.DbContext.DeleteObject(comment);

                // Apply the delete to the database 
                DbContextHelper.DbContext.SaveChanges();
            }
        }

       
        

	}

    public class ClaimCommented
    {
        public int CommentID { get; set; }
        public int ClaimID { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CommentText { get; set; }
        public string ActivityType { get; set; }
        public string InternalComments { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CommentDate { get; set; }
    }

}
