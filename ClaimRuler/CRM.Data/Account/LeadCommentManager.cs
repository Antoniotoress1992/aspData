using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public class LeadCommentManager {

		public static void Delete(int id) {
			LeadComment comment = new LeadComment();

			comment.CommentId = id;
			
			DbContextHelper.DbContext.LeadComment.Attach(comment);
			
			DbContextHelper.DbContext.DeleteObject(comment);

			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<LeadComment> getLeadCommentByLeadID(int LeadID) {
			var list = from li in DbContextHelper.DbContext.LeadComment
					 //where li.Status == 1 && li.LeadId == LeadID
					 where li.Status == 1 && li.LeadId == LeadID
					 orderby li.InsertDate descending
					 select li;

			return list.ToList();
		}

		public static List<LeadComment> getLeadCommentByLeadID(int LeadID, int policyTypeID) {
			List<LeadComment> comments = (from li in DbContextHelper.DbContext.LeadComment

									where li.Status == 1 && li.LeadId == LeadID && li.PolicyType == policyTypeID
								 orderby li.InsertDate descending
								 select li).ToList<LeadComment>();

			return comments;
		}

		public static LeadComment GetLeadCommentById(int Id) {
			var list = from x in DbContextHelper.DbContext.LeadComment
                       where x.CommentId == Id
					 select x;

			return list.Any() ? list.First() : new LeadComment();
		}

		public static LeadComment GetLeadCommentByReferenceId(int Id) {
			LeadComment comment = (from x in DbContextHelper.DbContext.LeadComment
							   where x.ReferenceID == Id
							   select x).FirstOrDefault();
			return comment;
		}

		public static void DeleteLeadCommentByReferenceId(int Id) {
			LeadComment comment = (from x in DbContextHelper.DbContext.LeadComment
							   where x.ReferenceID == Id
							   select x).FirstOrDefault();

			if (comment != null) {
				DbContextHelper.DbContext.DeleteObject(comment);

				DbContextHelper.DbContext.SaveChanges();
			}
					
		}

		public static LeadComment Save(LeadComment objLeadComment) {
			if (objLeadComment.CommentId == 0) {

				objLeadComment.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

				// insert now time when date null
				objLeadComment.InsertDate = objLeadComment.InsertDate ?? DateTime.Now;

				objLeadComment.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objLeadComment);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objLeadComment.UpdateMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objLeadComment.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objLeadComment;
		}
	}
}
