using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class FormFieldView {
		public int FieldID { get; set; }

		public int FormID { get; set; }

		public bool IsVisible { get; set; }

		public int? TemplateID { get; set; }

		public string FieldPrompt { get; set; }
	}
}
