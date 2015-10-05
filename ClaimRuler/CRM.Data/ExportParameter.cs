using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class ExportParameter {
		public bool isAll { get; set; }
		public bool isClaimLogo { get; set; }
		public bool isPhotos { get; set; }
		public bool isDocuments { get; set; }
		public bool isDemographics { get; set; }
		public bool isCoverage { get; set; }
		public int policyTypeID {get; set;}
	}
}
