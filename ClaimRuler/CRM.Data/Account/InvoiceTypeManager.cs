using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CRM.Data.Entities;
using System.Linq.Expressions;
using LinqKit;

namespace CRM.Data.Account
{
    public class InvoiceTypeManager
    {
        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;
        private bool disposed = false;

        public static List<InvoiceType> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.InvoiceType
                       orderby x.InvoiceTypes
                       select x;
            return list.ToList();

        }
        public static InvoiceType GetByID(int invoiceTypeID)
        {
            var list = from x in DbContextHelper.DbContext.InvoiceType
                       where x.InvoiceTypeID == invoiceTypeID
                       orderby x.InvoiceTypes
                       select x;
            return list.Any() ? list.First() : new InvoiceType();
            

        }
    }
}
