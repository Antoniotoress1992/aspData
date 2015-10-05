

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
    using CRM.Data.Entities;
	using System.Linq.Expressions;
	using LinqKit;

	public class LeadsUploadManager {

		//public static List<LeadsUpload> getLeadsUplod(int LeadId)
		//{
		//    LeadsUpload obj = null;
		//var result = from ld in DbContextHelper.DbContext.Leads
		//             join leadsimage in DbContextHelper.DbContext.LeadsImages on ld.LeadId equals leadsimage.LeadId
		//             join leadsDocument in DbContextHelper.DbContext.LeadsDocuments on ld.LeadId equals leadsDocument.LeadId
		//             where (ld.LeadId == LeadId && leadsimage.Status == 1 && leadsDocument.Status == 1)
		//             orderby ld.LeadId
		//             select new
		//             {

		//             };

		//var list1 = (from ld in DbContextHelper.DbContext.Leads
		//            join leadsimage in DbContextHelper.DbContext.LeadsImages on ld.LeadId equals leadsimage.LeadId
		//            where ld.Status == 1 && ld.LeadId == LeadId && leadsimage.Status == 1
		//            select new
		//            {
		//                leadsimage.LeadId,
		//                leadsimage.LeadImageId,
		//                leadsimage.ImageName,
		//                leadsimage.Description

		//            }).ToList();
		// var list2=(from ld in DbContextHelper.DbContext.Leads
		//               join leadsDocument in DbContextHelper.DbContext.LeadsDocuments on ld.LeadId equals leadsDocument.LeadId
		//               where ld.Status == 1 && ld.LeadId == LeadId && leadsDocument.Status == 1
		//               select new
		//               {
		//                   leadsDocument.LeadId,
		//                   leadsDocument.LeadDocumentId,
		//                   leadsDocument.DocumentName,
		//                   leadsDocument.Description 

		//               }).ToList();

		// var list3 = list1.Union(list2);//.OrderBy(p => p.id);


		//}


		public static List<LeadsImage> getLeadsImageByLeadID(int LeadID) {
			var list = from li in DbContextHelper.DbContext.LeadsImage
					 where li.Status == 1 && li.LeadId == LeadID
					 select li;

			return list.ToList();
		}

		public static List<LeadsImage> getLeadsImageForReport(int LeadID) {
			var list = from li in DbContextHelper.DbContext.LeadsImage
					 where li.Status == 1 && li.LeadId == LeadID && li.isPrint == true
					 select li;

			return list.ToList();
		}

		public static List<LeadsImage> getLeadsImageByLeadID(int LeadID, int policyTypeID) {
			List<LeadsImage> list = (from li in DbContextHelper.DbContext.LeadsImage
								where li.Status == 1 && li.LeadId == LeadID && li.policyTypeID == policyTypeID
								select li).ToList<LeadsImage>();

			return list;
		}

		public static LeadsImage GetLeadsImageById(int Id) {
			var users = from x in DbContextHelper.DbContext.LeadsImage
					  where x.LeadImageId == Id && x.Status == 1
					  select x;

			return users.Any() ? users.First() : new LeadsImage();
		}


		public static LeadsImage SaveImage(LeadsImage objLeadsImage) {
			if (objLeadsImage.LeadImageId == 0) {				
				objLeadsImage.InsertDate = DateTime.Now;
				objLeadsImage.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objLeadsImage);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objLeadsImage.UpdateMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objLeadsImage.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objLeadsImage;
		}

		public static bool checkForLocationDescriptionBlank(int LeadID) {
			bool isBlank = DbContextHelper.DbContext.LeadsImage.Any(li =>
						li.Status == 1 &&
						li.LeadId == LeadID &&
						(li.Description == null || li.Location == null)
						);

			return isBlank;

		}

		public static List<LeadsImage> getLeadsImagesMissingLocationDescription(int LeadID) {
			var list = from li in DbContextHelper.DbContext.LeadsImage
					 where li.Status == 1 && 
						li.LeadId == LeadID &&
						li.Description == null
					 select li;

			return list.ToList();
		}

		public static List<LeadsDocument> getLeadsDocumentByLeadID(int LeadID) {
			var list = from li in DbContextHelper.DbContext.LeadsDocument
					 where li.Status == 1 && li.LeadId == LeadID
					 select li;

			return list.ToList();
		}

		public static List<LeadsDocument> getLeadsDocumentForExportByLeadID(int LeadID) {
			var list = from li in DbContextHelper.DbContext.LeadsDocument
					 where li.Status == 1 && li.LeadId == LeadID && (li.IsPrint == null || li.IsPrint == true)
					 select li;

			return list.ToList();
		}

		public static List<LeadsDocument> getLeadsDocumentByLeadID(int LeadID, int policyTypeID) {
			List<LeadsDocument> list = (from li in DbContextHelper.DbContext.LeadsDocument
								   where li.Status == 1 && li.LeadId == LeadID && li.PolicyTypeID == policyTypeID
								   select li
					 ).ToList<LeadsDocument>();

			return list;
		}

	

		public static LeadsDocument GetLeadsDocumentById(int Id) {
			var users = from x in DbContextHelper.DbContext.LeadsDocument
					  where x.LeadDocumentId == Id
					  select x;

			return users.Any() ? users.First() : new LeadsDocument();
		}

		public static LeadsDocument SaveDocument(LeadsDocument objLeadsDocument) {
			if (objLeadsDocument.LeadDocumentId == 0) {

				objLeadsDocument.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				
				if (objLeadsDocument.InsertDate == null)
					objLeadsDocument.InsertDate = DateTime.Now;

				objLeadsDocument.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objLeadsDocument);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objLeadsDocument.UpdateMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objLeadsDocument.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objLeadsDocument;
		}
	}

	public class LeadsUpload {
		public int Id {
			get;
			set;
		}
		public int LeadID {
			get;
			set;
		}
		public string Name {
			get;
			set;
		}
		public string Location {
			get;
			set;
		}
		public string Description {
			get;
			set;
		}
	}
}
