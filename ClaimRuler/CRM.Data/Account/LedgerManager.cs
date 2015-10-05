using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class LedgerManager {
		public static List<Ledger> GetAll() {

            List<Ledger> list = DbContextHelper.DbContext.Ledger.Select(x=>x).ToList();
            return list;

            //return (from x in DbContextHelper.DbContext.Ledger
            //       orderby x.InvoiceID
            //       select x
            //             ).ToList();
		}

	}
}
