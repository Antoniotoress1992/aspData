using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#region Excel Exporting

using System.Drawing;
using System.Security;
using Infragistics.Web.Mvc;
using Infragistics.Documents.Excel;
using IgniteUI.SamplesBrowser.Models.Northwind;
using IgniteUI.SamplesBrowser.Models.Repositories;

#endregion

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class InfragisticsExcelController : Controller
    {
        private IEnumerable<Order> orders = null;

        // Orders data
        private IEnumerable<Order> Orders
        {
            get
            {
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
        // GET: /InfragisticsExcel/
        [ActionName("create-excel-worksheet")]
        public ActionResult ExcelExporter()
        {
            return View("create-excel-worksheet", Orders);
        }

        [GridDataSourceAction]
        [ActionName("PagingGetData")]
        public ActionResult PagingGetData()
        {
            return View("create-excel-worksheet", Orders);
        }

        [HttpPost]
        [ActionName("create-excel-worksheet")]
        public void ExcelExporter(int pageNumber, int pageSize, bool exportType, bool exportFormat)
        {
            pageNumber++;
            List<Order> newOrders = Orders.ToList();
            bool exportAllPages = exportType;
            WorkbookFormat excelFormat;

            if(exportFormat)
                excelFormat = WorkbookFormat.Excel2007;
            else
                excelFormat = WorkbookFormat.Excel97To2003;

            if(exportAllPages)
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
            }
            else
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

            ExcelExportingModel exportModel = new ExcelExportingModel(excelFormat);
            exportModel.PopulateExcelWorkbook(newOrders);
            SendForDownload(exportModel.ExcelWorkbook, excelFormat);
        }
        [SecuritySafeCritical]
        private void SendForDownload(Workbook document, WorkbookFormat excelFormat)
        {
            string documentFileNameRoot;
            documentFileNameRoot = string.Format("Document.{0}", excelFormat == WorkbookFormat.Excel97To2003 ? "xls" : "xlsx");
             
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            Response.ContentType = "application/octet-stream";
            document.SetCurrentFormat(excelFormat);
            document.Save(Response.OutputStream);
            Response.End();
        }

        #region Excel Report Model

        class ExcelExportingModel
        {
            #region Members

            #region Private Members

            private Workbook excelWorkbook;

            #endregion

            #endregion

            #region Constructor

            public ExcelExportingModel()
            {
                this.excelWorkbook = new Workbook();
            }

            public ExcelExportingModel(WorkbookFormat exportFormat)
            {
                this.excelWorkbook = new Workbook(exportFormat);
            }

            #endregion
            
            #region Properties

            public Workbook ExcelWorkbook
            {
                get
                {
                    return this.excelWorkbook;
                }
            }

            #endregion

            #region Public Methods

            public void PopulateExcelWorkbook(List<Order> data)
            {
                Worksheet currentWorksheet = this.excelWorkbook.Worksheets.Add("WorkSheet1");

                foreach (var cell in currentWorksheet.GetRegion("A1:D1"))
                {
                    cell.CellFormat.Fill = CellFill.CreateSolidFill(Color.Gray);
                    cell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.White);
                }

                currentWorksheet.Rows[0].Cells[0].Value = "Order ID";
                currentWorksheet.Rows[0].Cells[1].Value = "Contact Name";
                currentWorksheet.Rows[0].Cells[2].Value = "Shipping Address";
                currentWorksheet.Rows[0].Cells[3].Value = "Order Date";
                
                currentWorksheet.Columns[0].Width = 3000;
                currentWorksheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
                currentWorksheet.Columns[1].Width = 7100;
                currentWorksheet.Columns[2].Width = 3000;
                currentWorksheet.Columns[2].CellFormat.Alignment = HorizontalCellAlignment.Left;
                currentWorksheet.Columns[3].Width = 6100;

                int i = 1;
                foreach (Order order in data)
                {
                    currentWorksheet.Rows[i].Cells[0].Value = order.OrderID;
                    currentWorksheet.Rows[i].Cells[1].Value = order.ContactName;
                    currentWorksheet.Rows[i].Cells[2].Value = order.ShipAddress;
                    currentWorksheet.Rows[i].Cells[3].Value = order.OrderDate != null ? string.Format("{0:d}", order.OrderDate) : "";
                    i++;
                }
            }

            #endregion

        }


        #endregion

    }
}
