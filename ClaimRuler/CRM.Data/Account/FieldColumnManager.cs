using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class FieldColumnManager {
		public static void DeleteAll(int clientID) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM ClientFieldColumn WHERE ClientID = {0}", clientID);
           var client= DbContextHelper.DbContext.ClientFieldColumn.Where(x => x.ClientID == clientID).ToList();
           foreach (ClientFieldColumn objR in client)
           {
               DbContextHelper.DbContext.DeleteObject(objR);
           }
            DbContextHelper.DbContext.SaveChanges();

		}



		public static List<FieldColumn> GetAll() {
			List<FieldColumn> list = (from x in DbContextHelper.DbContext.FieldColumn								
								 select x).ToList();

			return list;
		}

        public static List<CRM.Data.Entities.vw_FieldColumn> GetAll(int clientID)
        {
            List<CRM.Data.Entities.vw_FieldColumn> list = (from x in DbContextHelper.DbContext.vw_FieldColumn
								 where (x.clientid == clientID || x.clientid == null)
								 orderby x.ColumnName
								 select x).ToList();

			return list;
		}

		public static List<vw_FieldColumn> GetInitialFieldList(int clientID) {
			List<vw_FieldColumn> fields = null;

			List<FieldColumn> list = GetAll();

			if (list != null) {
				fields = (from x in list
						select new vw_FieldColumn {
							clientid = clientID,
							ColumnID = x.ColumnID,
							ColumnName = x.ColumnName,
							isVisible = true,
						}).ToList();
			}

			return fields;
		}

		public static void Save(ClientFieldColumn column) {
			DbContextHelper.DbContext.Add(column);

			DbContextHelper.DbContext.SaveChanges();
		}
	}
}
