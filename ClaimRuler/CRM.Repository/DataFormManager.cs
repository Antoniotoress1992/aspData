using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class DataFormManager : IDisposable {

		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public DataFormManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public List<DataForm> GetScreenNames() {
			List<DataForm> dataForms = null;

			dataForms = (from x in claimRulerDBContext.DataForm
					   orderby x.FormName
					   select x
				).ToList();

			return dataForms;
		}

		public DataFormFieldTemplate GetTemplate(int id) {
			DataFormFieldTemplate template = null;

			template = (from x in claimRulerDBContext.DataFormFieldTemplate
					  where x.TemplateID == id
					  select x
				 ).FirstOrDefault();

			return template;
		}
		//public List<vw_FormField> GetFields(int formID, int clientID) {
		//	List<vw_FormField> fields = null;

		//	fields = (from x in claimRulerDBContext.vw_FormFields
		//			where x.ClientID == clientID && x.FormID == formID
		//			select x
		//		).ToList();

		//	return fields;
		//}
		//public List<FormFieldView> GetFormFields_bak(int formID, int clientID) {
		//	List<FormFieldView> fields = null;

		//	// get fields for client
		//	fields = (from x in claimRulerDBContext.vw_FormFields
		//			where x.ClientID == clientID && x.FormID == formID
		//			select new FormFieldView {
		//				FieldID = x.FieldID,
		//				FieldPrompt = x.FieldPrompt,
		//				FormID = (int)x.FormID,
		//				IsVisible = x.IsSelected,
		//				TemplateID = x.TemplateID
		//			}).ToList<FormFieldView>();

		//	if (fields != null && fields.Count == 0) {
		//		// client has not defined fields. get all fields for this form
		//		fields = GetFormFields(formID);
		//	}

		//	return fields;
		//}

		public List<FormFieldView> GetFormFields(int formID) {
			List<FormFieldView> fields = null;

			fields = (from x in claimRulerDBContext.DataFormField
					where x.FormID == formID
					orderby x.FieldPrompt
					select new FormFieldView {
						FieldID = x.FieldID,
						FieldPrompt = x.FieldPrompt,
						FormID = x.FormID,
						IsVisible = true,
						TemplateID = 0			// new 
					}).ToList();

			return fields;
		}

		public List<FormFieldView> GetFormFields(int formID, int clientID) {
			List<FormFieldView> formFields = null;


			formFields = (from df in claimRulerDBContext.DataFormField
					    join dft in claimRulerDBContext.DataFormFieldTemplate
							on new {
								df.FieldID,
								df.FormID,
								ClientID = (int)clientID
							}
							equals new {
								dft.FieldID,
								dft.FormID,
								ClientID = (int)dft.ClientID
							} into dft_join

					    from dft in dft_join.DefaultIfEmpty()
					    
					    where df.FormID == formID

					    select new FormFieldView {
						    FieldID = df.FieldID,
						    FieldPrompt = df.FieldPrompt,
						    FormID = df.FormID,
						    IsVisible = dft.IsSelected ?? true,
						    TemplateID = (dft.TemplateID == null ? 0 : dft.TemplateID)

					    }).ToList<FormFieldView>();

			return formFields;
		}

		public void DeleteTemplateFields(int formID, int clientID) {

            //claimRulerDBContext.ExecuteStoreCommand("DELETE FROM DataFormFieldTemplate WHERE ClientID = {0} AND FormId = {1}", clientID, formID);


            var template = DbContextHelper.DbContext.DataFormFieldTemplate.Where(x => x.ClientID == clientID && x.FormID == formID).ToList();
            foreach(DataFormFieldTemplate objR in template)
            {
                DbContextHelper.DbContext.DataFormFieldTemplate.Remove(objR);
            }
            DbContextHelper.DbContext.SaveChanges();




		}
		public void SaveTemplateField(DataFormFieldTemplate fieldTemplate) {
			if (fieldTemplate.TemplateID == 0)
				claimRulerDBContext.DataFormFieldTemplate.Add(fieldTemplate);

			claimRulerDBContext.SaveChanges();
		}

		#region ===== memory management =====

		public void Dispose() {
			// Perform any object clean up here.

			// If you are inheriting from another class that
			// also implements IDisposable, don't forget to
			// call base.Dispose() as well.
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					if (claimRulerDBContext != null) {

						claimRulerDBContext.Dispose();
					}
				}

				disposed = true;
			}
		}
		#endregion
	}
}
