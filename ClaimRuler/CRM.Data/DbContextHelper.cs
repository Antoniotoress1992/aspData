
namespace CRM.Data.Entities
{
   #region Namespace
   using System;
   using System.Web;
   using System.Configuration;
   #endregion

   public static class DbContextHelper
    {
        private const string DATACONTEXT_ITEMS_KEY = "DataContextKey";

        private  static CRMEntities InternalDbContext
        {
            get { 
                return (CRMEntities)HttpContext.Current.Items[DATACONTEXT_ITEMS_KEY]; }
            set { HttpContext.Current.Items[DATACONTEXT_ITEMS_KEY] = value; }
        }

        public static CRMEntities DbContext
        {
            get
            {
                if(InternalDbContext == null)
                {
                    var conString = ConfigurationManager.ConnectionStrings["CRMEntities"].ToString();
                    InternalDbContext = new CRMEntities();
                }
                return InternalDbContext;
            }
        }

        public static void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public  static void CleanUp()
        {
            if (InternalDbContext != null)
            {
                InternalDbContext.Dispose();
                InternalDbContext = null;
            }
        }
    }

  

}
