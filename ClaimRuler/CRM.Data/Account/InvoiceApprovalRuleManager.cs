using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class InvoiceApprovalRuleManager {

		static public void Delete(int id) {
			SecRoleInvoiceApprovalPermission rule = new SecRoleInvoiceApprovalPermission { ID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("SecRoleInvoiceApprovalPermissions", rule);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(rule);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		static public List<CRM.Data.Entities.SecRoleInvoiceApprovalPermission> GetAll(int roleID) {
            List<CRM.Data.Entities.SecRoleInvoiceApprovalPermission> rules = null;

			rules = (from x in DbContextHelper.DbContext.SecRoleInvoiceApprovalPermission
				    where x.RoleID == roleID
                     select x).ToList<CRM.Data.Entities.SecRoleInvoiceApprovalPermission>();

			return rules;
		}

		static public SecRoleInvoiceApprovalPermission Get(int id) {
			SecRoleInvoiceApprovalPermission rule = null;

			rule = (from x in DbContextHelper.DbContext.SecRoleInvoiceApprovalPermission
				    where x.ID == id
				    select x).FirstOrDefault<SecRoleInvoiceApprovalPermission>();

			return rule;
		}

		static public SecRoleInvoiceApprovalPermission Save(SecRoleInvoiceApprovalPermission rule) {
			if (rule.ID == 0) {
				DbContextHelper.DbContext.Add(rule);
			}
		
			DbContextHelper.DbContext.SaveChanges();

			return rule;
		}
	}
}
