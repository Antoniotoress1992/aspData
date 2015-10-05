using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CRM.Core {
	public class PDFPageEventHelper : PdfPageEventHelper {
		string _footerText = null;

		public PDFPageEventHelper() {
		}
		public PDFPageEventHelper(string footerText) {
			_footerText = footerText;
		}
		public override void OnEndPage(PdfWriter writer, Document document) {
			//PdfPTable table = new PdfPTable(1);
			////table.WidthPercentage = 100; //PdfPTable.writeselectedrows below didn't like this
			//table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]
			//PdfPTable table2 = new PdfPTable(2);

			////logo
			////PdfPCell cell2 = new PdfPCell("Tan");
			////cell2.Colspan = 2;
			////table2.AddCell(cell2);

			////title
			//Font black9BoldFont = FontFactory.GetFont("swis721 cn BT", 9f, Font.BOLD, BaseColor.BLACK);
			//PdfPCell cell2 = new PdfPCell(new Phrase("\nTITLE", black9BoldFont));
			//cell2.HorizontalAlignment = Element.ALIGN_CENTER;
			//cell2.Colspan = 2;
			//table2.AddCell(cell2);

			//PdfPCell cell = new PdfPCell(table2);
			//table.AddCell(cell);

			//table.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 36, writer.DirectContent);

			if (!string.IsNullOrEmpty(_footerText)) {
				Paragraph footer = new Paragraph(_footerText, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));

				footer.Alignment = Element.ALIGN_CENTER;

				PdfPTable footerTbl = new PdfPTable(1);

				footerTbl.TotalWidth = 300;

				footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

				PdfPCell cell = new PdfPCell(footer);

				cell.Border = 0;

				cell.PaddingLeft = 10;

				footerTbl.AddCell(cell);

				footerTbl.WriteSelectedRows(0, -1, 415, 30, writer.DirectContent);
			}
		}
	}
}
