using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core {
	static public class CSVHelper {
		static private StreamReader file = null;

		static public DataTable ReadCSVFile(string filename) {
			DataTable table = CreateDataTable(filename);

			return table;
		}

		static public DataTable CreateDataTable(string filePath) {
			DataTable table = new DataTable();
			string[] fields = null;

			List<List<object>> rows = new List<List<object>>();
			string textLine = null;

			file = new StreamReader(filePath);

			buildTableHeader(table);

			while ((textLine = file.ReadLine()) != null) {
				if (!string.IsNullOrEmpty(textLine)) {
					fields = textLine.Split(new char[] { ',' });

					DataRow dataRow = table.NewRow();
					for (int i = 0; i < fields.Length; i++) {
						try {
							dataRow[i] = fields[i];
						}
						catch (Exception ex) {
						}
					}
					table.Rows.Add(dataRow);
				}
			}
			file.Close();

			return table;
		}

		static public void buildTableHeader(DataTable table) {
			string[] headers = null;

			string headerLine = file.ReadLine();

			if (!string.IsNullOrEmpty(headerLine)) {
				headers = headerLine.Split(new char[] { ',' });

				for (int i = 0; i < headers.Length; i++) {
					table.Columns.Add(new DataColumn(headers[i], typeof(string)));
				}
			}
		}

		static public bool columnExists(DataTable datatable, string columnName) {
			bool exists = false;

			exists = datatable.Columns.Cast<DataColumn>()
							.Any(c => c.ColumnName == columnName);


			return exists;
		}

		static public CRM.Data.Entities.ImportMapField[] readHeaders(string filePath) {
			string[] headers = null;
			string headerLine = null;
			CRM.Data.Entities.ImportMapField[] fieldHeaders = null;

			file = new StreamReader(filePath);

			headerLine = file.ReadLine();

			if (!string.IsNullOrEmpty(headerLine)) {
				headers = headerLine.Split(new char[] { ',' }).ToArray();

				fieldHeaders = (from x in headers
							 select new CRM.Data.Entities.ImportMapField {
								 UserFieldName = x
							 }).ToArray();
			}

			file.Close();

			return fieldHeaders;
		}

		static public string getUserFieldName(List<CRM.Data.Entities.ImportMapField> mappedFields, string claimRulerFieldName) {
			string userFieldName = null;

			userFieldName = (from x in mappedFields
						  where x.ClaimRulerFieldName == claimRulerFieldName
						  select x.UserFieldName
					).FirstOrDefault<string>();

			return userFieldName;
		}
	}
}
