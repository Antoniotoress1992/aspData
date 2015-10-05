using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Rules {
	public interface IRule {
		void bindData(BusinessRule businessRule);

		void clearFields();

		XDocument buildRule();
	}
}
