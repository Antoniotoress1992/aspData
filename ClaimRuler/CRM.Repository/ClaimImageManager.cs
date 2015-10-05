using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	static public class ClaimImageManager {
		public static List<ClaimImage> GetAll(int claimID) {
			List<ClaimImage> images = (from x in DbContextHelper.DbContext.ClaimImage
								  where x.ClaimID == claimID && x.IsActive == true
								  select x).ToList<ClaimImage>();

			return images;
		}
		public static ClaimImage Get(int id) {
			ClaimImage image = (from x in DbContextHelper.DbContext.ClaimImage
								  where x.ClaimImageID == id
								  select x).FirstOrDefault<ClaimImage>();

			return image;
		}

		public static ClaimImage Save(ClaimImage claimImage) {
			if (claimImage.ClaimImageID == 0) {
				claimImage.ImageDate = DateTime.Now;
				DbContextHelper.DbContext.ClaimImage.Add(claimImage);
			}

			DbContextHelper.DbContext.SaveChanges();

			return claimImage;
		}

	}
}
