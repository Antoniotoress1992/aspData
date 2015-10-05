using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class LeadDocumentListManager {

		public static LeadDocumentList Get(int leadID, int id) {
			return (from x in DbContextHelper.DbContext.LeadDocumentList
				   where x.LeadId == leadID &&
					x.DocumentListId == id
				   select x
					).FirstOrDefault<LeadDocumentList>();
		}

		public static void Delete(int leadID, int id) {
			LeadDocumentList document = (from x in DbContextHelper.DbContext.LeadDocumentList
				   where x.LeadId == leadID &&
					x.DocumentListId == id
				   select x
					).FirstOrDefault<LeadDocumentList>();

			if (document != null) {
				DbContextHelper.DbContext.DeleteObject(document);

				DbContextHelper.DbContext.SaveChanges();
			}
		}
		
		static public List<DocumentList> GetDocumentList(int clientID, int leadID, int policyTypeID) {
			List<DocumentList> milestone = null;

			milestone = (from a in DbContextHelper.DbContext.DocumentListMaster
					   join b in
						   (from m in DbContextHelper.DbContext.LeadDocumentList
						    where m.LeadId == leadID
						    select m)
						on a.DocumentListId equals b.DocumentListId
						into c
					   from d in c.DefaultIfEmpty()
					   where a.IsActive == true && a.clientID == clientID && a.PolicyTypeId == policyTypeID
					   select new DocumentList {
						   LeadId = leadID,
						   DocumentListId = a.DocumentListId,
						   DocumentName = a.DocumentName,
						   isChecked = d.LeadId != null ? true : false
					   }).ToList<DocumentList>();

			return milestone;
		}

		static public void Save(LeadDocumentList document) {
			if (document.ID == 0)
				DbContextHelper.DbContext.Add(document);

			DbContextHelper.DbContext.SaveChanges();
		}
	}
}
