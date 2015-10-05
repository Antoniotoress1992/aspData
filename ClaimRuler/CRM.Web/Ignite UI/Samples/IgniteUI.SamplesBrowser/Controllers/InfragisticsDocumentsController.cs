using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#region PDF_XPS Document Exporting

using Infragistics.Web.Mvc;
using Infragistics.Documents.Reports.Graphics;
using Infragistics.Documents.Reports.Report;
using Infragistics.Documents.Reports.Report.Band;
using Infragistics.Documents.Reports.Report.Preferences.Printing;
using Infragistics.Documents.Reports.Report.Section;
using Infragistics.Documents.Reports.Report.Table;
using Infragistics.Documents.Reports.Report.Text;
using IgniteUI.SamplesBrowser.Models.Repositories;
using IgniteUI.SamplesBrowser.Models.Northwind;

#endregion

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class InfragisticsDocumentsController : Controller
    {
        private IEnumerable<Order> orders = null;

        // Orders data
        private IEnumerable<Order> Orders{
            get {
                try
                {
                    if (orders == null)
                        orders = RepositoryFactory.GetOrderRepository().Get().Take(100).AsQueryable();
                    return orders;
                }
                catch (Exception)
                {
                    return new List<Order>().AsQueryable();
                }
            }
        }

        //
        // GET: /InfragisticsDocuments/
        [ActionName("create-pdf-or-xps")]
        public ActionResult CreatePDFOrXPS()
        {
            return View("create-pdf-or-xps", Orders);
        }

        [GridDataSourceAction]
        [ActionName("PagingGetData")]
        public ActionResult PagingGetData()
        {
            return View("create-pdf-or-xps", Orders);
        }

        [HttpPost]
        [ActionName("create-pdf-or-xps")]        
        public void CreatePDFOrXPS(string pageFormat, string pageOrientation, string exportType, int pageSize, int pageNumber)
        {
            //make the currentPageNumber start from 1
            pageNumber++;
            List<Order> newOrders = Orders.ToList();
            bool exportAllPages = false;

            if (exportType == "currentPage")
            {
                newOrders = newOrders
                    .Skip<Order>(pageSize * (pageNumber - 1))
                    .Take<Order>(pageSize)
                    .Select(c => new Order
                    {
                        OrderID = c.OrderID,
                        ContactName = c.ContactName,
                        ShipAddress = c.ShipAddress,
                        OrderDate = c.OrderDate
                    })
                    .ToList();
            }
            else
            {
                newOrders = newOrders
                    .Select(c => new Order
                    {
                        OrderID = c.OrderID,
                        ContactName = c.ContactName,
                        ShipAddress = c.ShipAddress,
                        OrderDate = c.OrderDate
                    })
                    .ToList();
                exportAllPages = true;
            }

            ExportHelper exporter = new ExportHelper(newOrders, pageOrientation, pageNumber, exportAllPages);
            Report igReport = exporter.Report();

            SendForDownload(igReport, pageFormat);
        }

        private void SendForDownload(Report report, string pageFormat)
        {
            string documentFileNameRoot;
            FileFormat exportFileFormat;

            documentFileNameRoot = string.Format("Document.{0}", pageFormat.ToLower());
            exportFileFormat = (pageFormat.ToLower() == "pdf") ? FileFormat.PDF : FileFormat.XPS;

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            Response.ContentType = "application/octet-stream";
            report.Publish(Response.OutputStream, exportFileFormat);
            Response.End();
        }

        #region Report
        private class ExportHelper
        {
            #region Private
            private string Author = "Infragistics, Inc.";
            private string Creator = "Infragistics Report Writer";
            private string Copyright = string.Format("Copyright © 2003-{0} by Infragistics, Inc.", DateTime.Now.Year.ToString());

            private Report report;

            // Report Section
            private static ISection section;

            // Styles
            private static Borders bordersStyle;
            private static Font fontStyle;
            private static Font headerFont;

            //Report options
            private List<Order> ordersData;
            private string pageOrientation;
            private int currentPageNumber;
            private bool exportAllPages;
            #endregion

            /// <summary>
            /// The ReportExporter creates an sample report based igGrid and the data source attached to it.
            /// </summary>
            /// <param name="data">A sample data.</param>
            /// <param name="pageOrientation">Landscape or Portrait.</param>
            /// <param name="currentPageNumber">The selected page in the grid. </param>
            public ExportHelper(List<Order> data, string pageOrientation,
                                    int currentPageNumber, bool exportAllPages)
            {
                this.ordersData = data;
                this.pageOrientation = pageOrientation;
                this.currentPageNumber = currentPageNumber;
                this.exportAllPages = exportAllPages;
            }

            /// <summary>
            /// Creates an report object with the passed data.
            /// </summary>
            /// <returns>Returns a Report object.</returns>
            public Report Report()
            {                
                SetupStyles();
                SetupReportInfo("Document Exporter");

                IBand reportHeader = section.AddBand();
                IText reportHeaderText = reportHeader.AddText();
                reportHeaderText.Height = new FixedHeight(30);
                reportHeaderText.Style.Font = headerFont;

                if (this.exportAllPages)
                {
                    reportHeaderText.AddContent("Congratulations! You have exported all pages from the report successfully!");
                }
                else
                {
                    reportHeaderText.AddContent(string.Format("Congratulations! You have exported page {0} successfully!", this.currentPageNumber));
                }

                ITable table = section.AddTable();

                table.Borders = bordersStyle;
                table.Margins.Top = 5;
                table.Margins.Bottom = 5;
                table.Width = new RelativeWidth(100);
                table.Margins.Left = 30;

                // Header
                ITableHeader header = table.Header;
                header.Height = new FixedHeight(24);
                header.Repeat = true;

                this.AddHeaderCell(header, "Order ID");
                this.AddHeaderCell(header, "Contact Name");
                this.AddHeaderCell(header, "Shipping Address");
                this.AddHeaderCell(header, "Order Date");

                //Here we add all the rows of the table
                this.AddTableRows(table);

                return report;
            }

            private void AddHeaderCell(ITableHeader header, string text, Width width = null)
            {
                IText headerText;
                ITableCell cell = header.AddCell();

                Color startColor = new Color(117, 117, 117);
                Color endColor = new Color(84, 84, 84);
                cell.Borders = bordersStyle;
                cell.Background = new Background(new LinearGradientBrush(startColor, endColor, 90));
                cell.Alignment.Vertical = Alignment.Middle;

                if (width != null)
                {
                    cell.Width = width;
                }

                headerText = cell.AddText();

                headerText.Style.Font = headerFont;
                headerText.Style.Brush = Brushes.White;
                headerText.Alignment = TextAlignment.Center;
                headerText.AddContent(text);
            }

            private void AddTableRows(ITable table)
            {
                ITableRow tableRow;
                ITableCell tableCell;
                IText cellText;
                int i = 0;
                Paddings cellPaddings = new Paddings(3, 0, 5, 3);
                string modifiedDate;

                foreach (Order order in this.ordersData)
                {
                    tableRow = table.AddRow();
                    tableRow.Height = new FixedHeight(30);

                    Background cellBackgroundColor = (i % 2 == 0) ? new Background(Colors.White) : new Background(new Color(235, 235, 235));

                    tableCell = tableRow.AddCell();
                    tableCell.Paddings = cellPaddings;
                    tableCell.Background = cellBackgroundColor;
                    tableCell.Borders = bordersStyle;

                    cellText = tableCell.AddText();
                    cellText.Style.Font = fontStyle;
                    cellText.AddContent(order.OrderID.ToString());

                    tableCell = tableRow.AddCell();
                    tableCell.Paddings = cellPaddings;
                    tableCell.Background = cellBackgroundColor;
                    tableCell.Borders = bordersStyle;

                    cellText = tableCell.AddText();
                    cellText.Style.Font = fontStyle;
                    cellText.AddContent(order.ContactName.ToString());

                    tableCell = tableRow.AddCell();
                    tableCell.Paddings = cellPaddings;
                    tableCell.Background = cellBackgroundColor;
                    tableCell.Borders = bordersStyle;

                    cellText = tableCell.AddText();
                    cellText.Style.Font = fontStyle;
                    cellText.AddContent(order.ShipAddress.ToString());

                    tableCell = tableRow.AddCell();
                    tableCell.Paddings = cellPaddings;
                    tableCell.Background = cellBackgroundColor;
                    tableCell.Borders = bordersStyle;

                    cellText = tableCell.AddText();
                    cellText.Style.Font = fontStyle;

                    modifiedDate = order.OrderDate != null ? string.Format("{0:d}", order.OrderDate) : "";
                    
                    cellText.AddContent(modifiedDate);
                    i++;
                }
            }

            private void SetupReportInfo(string title)
            {
                report = new Report();

                report.Info.Title = title;
                report.Info.Author = Author;
                report.Info.Creator = Creator;
                report.Info.Copyright = Copyright;

                report.Preferences.Printing.PaperSize = PaperSize.Auto;
                report.Preferences.Printing.PaperOrientation = PaperOrientation.Auto;
                report.Preferences.Printing.FitToMargins = true;

                SetupDefaultSection();
            }

            private void SetupDefaultSection()
            {
                section = report.AddSection();
                section.PageSize = PageSizes.Letter;
                section.PageMargins.All = 35;

                if (pageOrientation == "portrait")
                {
                    section.PageOrientation = PageOrientation.Portrait;
                }
                else
                {
                    section.PageOrientation = PageOrientation.Landscape;
                }
            }

            private void SetupStyles()
            {
                bordersStyle = new Borders(new Pen(new Color(117, 117, 117), (float)0.5, DashStyle.Solid));
                fontStyle = new Font("Arial Unicode MS", 9);
                headerFont = new Font("Arial Unicode MS", 11, 0);
            }
        }
        #endregion

    }
}
