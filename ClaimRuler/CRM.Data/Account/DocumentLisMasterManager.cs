using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class DocumentLisMasterManager {

		public static List<DocumentListMaster> GetDocumentListMaster(int clientID, int policyTypeID) {
			return (from x in DbContextHelper.DbContext.DocumentListMaster
				   orderby x.DocumentName
				   where x.IsActive == true &&
						x.clientID == clientID &&
						x.PolicyTypeId == policyTypeID
				   select x
					).ToList<DocumentListMaster>();
		}

		public static DocumentListMaster GetDocumentMaster(int DocumentListId) {
			return (from x in DbContextHelper.DbContext.DocumentListMaster
				   where x.IsActive == true &&
					x.DocumentListId == DocumentListId
				   select x
					).FirstOrDefault<DocumentListMaster>();
		}

		static public void Save(DocumentListMaster document) {
			if (document.DocumentListId == 0)
				DbContextHelper.DbContext.Add(document);
		
			DbContextHelper.DbContext.SaveChanges();
		}

		
	}
}
