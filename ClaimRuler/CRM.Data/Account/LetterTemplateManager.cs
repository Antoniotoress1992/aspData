using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class LetterTemplateManager {

		public static void Delete(int templateID) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM ClientLetterTemplate WHERE TemplateID = {0}", templateID);

            var template = DbContextHelper.DbContext.ClientLetterTemplate.Where(x => x.TemplateID == templateID).ToList();
            foreach (ClientLetterTemplate objR in template)
            {
                DbContextHelper.DbContext.DeleteObject(objR);
            }
            DbContextHelper.DbContext.SaveChanges();
                        
		}

		public static ClientLetterTemplate Get(int id) {
			return (from x in DbContextHelper.DbContext.ClientLetterTemplate
				   where x.TemplateID == id
				   select x
					 ).FirstOrDefault<ClientLetterTemplate>();

		}

		public static List<ClientLetterTemplate> GetAll(int clientID) {
			List<ClientLetterTemplate> list = (from x in DbContextHelper.DbContext.ClientLetterTemplate
					  where x.ClientID == clientID
					  select x).ToList();

			return list;
		}
		public static List<ClientLetterTemplate> GetAll() {
			List<ClientLetterTemplate> list = (from x in DbContextHelper.DbContext.ClientLetterTemplate										
										select x).ToList();

			return list;
		}

		static public int Save(ClientLetterTemplate template) {
			int templateID = 0;

			if (template.TemplateID == 0) {				
				DbContextHelper.DbContext.Add(template);
			}
			
			DbContextHelper.DbContext.SaveChanges();

			templateID = template.TemplateID;
			

			return templateID;
		}
	}
}
