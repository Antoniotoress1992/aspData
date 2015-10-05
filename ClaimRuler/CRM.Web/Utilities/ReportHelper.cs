﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Microsoft.Reporting.WebForms;

namespace CRM.Web.Utilities {
	static public class ReportHelper {


		static public string sanatizeFileName(string filename) {
			return filename.Replace(",", "")
						.Replace("&", "")
						.Replace("/", "")
						.Replace("#", "")
						.Replace("[", "")
						.Replace("$", "")
						.Replace("@", "")
						.Replace("\\", "")
						.Replace("'", "")
						.Replace("-", "")
						.Replace(" ", "_");	
		}

		static public void renderToBrowser(string filePath, string clientFileName) {
			string downloadFileName = null;

			if (System.IO.File.Exists(filePath)) {
				System.IO.FileInfo file = new System.IO.FileInfo(filePath);

				downloadFileName = string.IsNullOrEmpty(clientFileName) ? file.Name : clientFileName;

				HttpContext.Current.Response.Expires = 0;

				HttpContext.Current.Response.ClearContent();

				HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + downloadFileName);

				HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

				if (filePath.ToLower().Contains(".doc"))
					HttpContext.Current.Response.ContentType = "Application/msword";	// doc
				else if (filePath.ToLower().Contains(".xls"))
					HttpContext.Current.Response.ContentType = "xls";
				else if (filePath.ToLower().Contains(".pdf"))
					HttpContext.Current.Response.ContentType = "Application/pdf";

				HttpContext.Current.Response.TransmitFile(file.FullName);

				HttpContext.Current.Response.End();
			}
		}

		static public void renderToBrowser(string filePath) {

			if (System.IO.File.Exists(filePath)) {
				System.IO.FileInfo file = new System.IO.FileInfo(filePath);

				HttpContext.Current.Response.Expires = 0;

				HttpContext.Current.Response.ClearContent();

				HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + file.Name);

				HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

				if (filePath.ToLower().Contains(".doc"))
					HttpContext.Current.Response.ContentType = "Application/msword";	// doc
				else if (filePath.ToLower().Contains(".xls"))
					HttpContext.Current.Response.ContentType = "xls";
				else if (filePath.ToLower().Contains(".pdf"))
					HttpContext.Current.Response.ContentType = "Application/pdf";

				HttpContext.Current.Response.TransmitFile(file.FullName);

				HttpContext.Current.Response.End();
			}
		}

		static public void renderToBrowser(string filePath, bool isFlush) {

			if (System.IO.File.Exists(filePath)) {
				System.IO.FileInfo file = new System.IO.FileInfo(filePath);

				HttpContext.Current.Response.Expires = 0;

				HttpContext.Current.Response.ClearContent();

				HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + file.Name);

				HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

				if (filePath.ToLower().Contains(".doc"))
					HttpContext.Current.Response.ContentType = "Application/msword";	// doc
				else if (filePath.ToLower().Contains(".xls"))
					HttpContext.Current.Response.ContentType = "xls";
				else if (filePath.ToLower().Contains(".pdf"))
					HttpContext.Current.Response.ContentType = "Application/pdf";

				HttpContext.Current.Response.TransmitFile(file.FullName);

				HttpContext.Current.Response.Flush();
			}
		}

		static public void renderPDFFromLocalReport(LocalReport report, string pdfFileName) {
			//code to render report as pdf document
			string encoding = String.Empty;
			string mimeType = String.Empty;
			string extension = String.Empty;
			Warning[] warnings = null;
			string[] streamids = null;

			byte[] byteArray = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);
		
			HttpContext.Current.Response.ContentType = "Application/pdf";
			HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfFileName);
			HttpContext.Current.Response.AddHeader("Content-Length", byteArray.Length.ToString());
			HttpContext.Current.Response.ContentType = "application/octet-stream";
			HttpContext.Current.Response.BinaryWrite(byteArray);
			
			HttpContext.Current.Response.End();


		}

		static public void savePDFFromLocalReport(LocalReport report, string pdfFileName) {
			//code to render report as pdf document
			string encoding = String.Empty;
			string mimeType = String.Empty;
			string extension = String.Empty;
			Warning[] warnings = null;
			string[] streamids = null;

			byte[] byteArray = report.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

			// write pdf to disk
			using (FileStream fs = new FileStream(pdfFileName, FileMode.Create)) {
				fs.Write(byteArray, 0, byteArray.Length);
			}		
		}
	}
}