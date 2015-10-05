using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository
{
    static public class InvoiceServiceUnitManager {
	    static public InvoiceServiceUnit Save(InvoiceServiceUnit serviceUnit) {
		    if (serviceUnit.UnitID == 0)
			    CRM.Data.Entities.DbContextHelper.DbContext.InvoiceServiceUnit.Add(serviceUnit);

		    DbContextHelper.DbContext.SaveChanges();

		    return serviceUnit;
	    }
    }
}
