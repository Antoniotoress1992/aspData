using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class BillToView {
		public string billTo { get; set; }
		public string billingName { get; set; }
		public string mailingAddress { get; set; }
		public string mailingCity { get; set; }
		public string mailingState { get; set; }
		public string mailingZip { get; set; }
	}
}
