using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class DocumentList {
		public int DocumentListId { get; set; }
		public int LeadId { get; set; }
		public string Description { get; set; }

		public string DocumentName { get; set; }
		public string DocumentPath { get; set; }
		public bool isChecked { get; set; }
	}
}
