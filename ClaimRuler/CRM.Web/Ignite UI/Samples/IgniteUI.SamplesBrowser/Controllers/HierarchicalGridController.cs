using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;
using Infragistics.Web.Mvc;
using IgniteUI.SamplesBrowser.Models.Repositories;
using IgniteUI.SamplesBrowser.Application.Data;
using System.Collections.Generic;
using IgniteUI.SamplesBrowser.Models.Northwind;
using System;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class HierarchicalGridController : Controller
    {      
        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspMvcHelper()
        {
            GridModel gridModel = GetGridModel();
            return View("aspnet-mvc-helper", gridModel);
        }
        
        [GridDataSourceAction]
        [ActionName("dataset-binding")]
        public ActionResult DataSetMvcHelper()
        {
            var categoriesProducts = this.CategoriesProducts;
            return View("dataset-binding", categoriesProducts);
        }

        [GridDataSourceAction]
        [ActionName("editing-dataset")]
        public ActionResult EditingDataSetMvcHelper()
        {
            var categoriesProducts = this.CategoriesProducts;
            return View("editing-dataset", categoriesProducts);
        }

        [ActionName("EditingSaveChanges")]
        public ActionResult EditingSaveChanges()
        {
            ViewData["GenerateCompactJSONResponse"] = false;
            GridModel m = new GridModel();
            List<Transaction<Category>> categoryTransactions = m.LoadTransactions<Category>(HttpContext.Request.Form["ig_transactions"]);
            foreach (Transaction<Category> t in categoryTransactions)
            {
                DataRow dr = this.CategoriesProducts.Tables["Categories"].Rows.Find(Int32.Parse(t.rowId));
                if ((t.layoutKey == null) && (dr != null))
                {
                    if (t.type == "row")
                    {
                        Category categoryRow = (Category)t.row;
                        if (categoryRow.CategoryName != null) {
                            dr["CategoryName"] = categoryRow.CategoryName;
                        }
                        if (categoryRow.Description != null){
                            dr["Description"] = categoryRow.Description;
                        }
                    } else if (t.type == "deleterow")
                    {
                        this.CategoriesProducts.Tables["Categories"].Rows.Remove(dr);
                    }
                }
            }
            List<Transaction<Product>> productTransactions = m.LoadTransactions<Product>(HttpContext.Request.Form["ig_transactions"]);
            foreach (Transaction<Product> t in productTransactions)
            {
                DataRow dr = this.CategoriesProducts.Tables["Products"].Rows.Find(Int32.Parse(t.rowId));
                if ((t.layoutKey == "Products") && (dr != null))
                {
                    if (t.type == "deleterow")
                    {
                        this.CategoriesProducts.Tables["Products"].Rows.Remove(dr);
                    }
                    else if (t.type == "row")
                    {
                        Product productRow = (Product)t.row;
                        if (productRow.ProductName != null)
                        {
                            dr["ProductName"] = productRow.ProductName;
                        }
                        if (productRow.CategoryID != null)
                        {
                            dr["CategoryID"] = productRow.CategoryID;
                        }
                        if (productRow.UnitPrice != null)
                        {
                            dr["UnitPrice"] = productRow.UnitPrice;
                        }
                        if (productRow.UnitsInStock != null)
                        {
                            dr["UnitsInStock"] = productRow.UnitsInStock;
                        }
                        dr["Discontinued"] = productRow.Discontinued;
                    }
                }
            }
            JsonResult result = new JsonResult();
            Dictionary<string, bool> response = new Dictionary<string, bool>();
            response.Add("Success", true);
            result.Data = response;
            return result;
        }

        [ActionName("load-on-demand")]
        public ActionResult DataSetWithLoadOnDemand()
        {
            GridModel grid = GridLoadOnDemandModel();
            grid.ID = "gridLoadOnDemand";
            grid.LoadOnDemand = true;
            grid.DataSourceUrl = this.Url.Action("BindParent");
            grid.ColumnLayouts[0].DataSourceUrl = this.Url.Action("BindChild");
            return View("load-on-demand", grid);
        }

        #region Grid Models
        private GridModel GetGridModel()
        {
            GridModel gridModel = new GridModel();
            gridModel.ID = "gridModel";
            gridModel.AutoGenerateColumns = false;
            gridModel.AutoGenerateLayouts = false;
            gridModel.PrimaryKey = "ID";
            gridModel.LoadOnDemand = false;
            gridModel.Width = "100%";
            gridModel.Columns = new List<GridColumn>();
            gridModel.Columns.Add(new GridColumn() { HeaderText = "Category ID", Key = "ID", DataType = "number", Width = "15%" });
            gridModel.Columns.Add(new GridColumn() { HeaderText = "Category Name", Key = "CategoryName", DataType = "string", Width = "30%" });
            gridModel.Columns.Add(new GridColumn() { HeaderText = "Description", Key = "Description", DataType = "string", Width = "40%" });
            gridModel.Columns.Add(new GridColumn() { HeaderText = "Products Count", Key = "ProductCount", DataType = "number", Width = "15%" });
        
            gridModel.DataSourceUrl = this.Url.Action("GetCategoriesData");
            gridModel.DataSource = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products).Get().AsQueryable();

            GridColumnLayoutModel productsModel = GetProductsLayout();
            gridModel.ColumnLayouts.Add(productsModel);

            gridModel.Features.Add(new GridFiltering() { Type = OpType.Remote, Inherit = true, Persist = false });
            gridModel.Features.Add(new GridSorting() { Type = OpType.Remote, Inherit = true, Persist = false });
            gridModel.Features.Add(new GridSummaries() { Type = OpType.Remote, Inherit = true });
            gridModel.Features.Add(new GridPaging() { Type = OpType.Remote, Inherit = true , PageSize = 5});
            gridModel.Features.Add(new GridResponsive() { 
                EnableVerticalRendering = false, 
                ColumnSettings = new List<ResponsiveColumnSetting>() { 
                    new ResponsiveColumnSetting() { ColumnKey = "ID", Classes = "ui-hidden-phone" },
                    new ResponsiveColumnSetting() { ColumnKey = "Description", Classes = "ui-hidden-phone" }
                } 
            });
    
            return gridModel;
        }

        private GridModel GridLoadOnDemandModel()
        {
            // Define the Categories layout
            GridModel grid = new GridModel();
            grid.AutoGenerateLayouts = false;
            grid.AutoGenerateColumns = false;
            grid.PrimaryKey = "ID";
            grid.Width = "100%";
            grid.Columns.Add(new GridColumn() { HeaderText = "Category ID", Key = "ID", DataType = "number", Width = "0%", Hidden = true});
            grid.Columns.Add(new GridColumn() { HeaderText = "Category Name", Key = "CategoryName", DataType = "string", Width = "30%" });
            grid.Columns.Add(new GridColumn() { HeaderText = "Description", Key = "Description", DataType = "string", Width = "50%" });
            grid.Columns.Add(new GridColumn() { HeaderText = "Products Count", Key = "ProductCount", DataType = "number", Width = "20%" });

            // Define the Products layout
            GridColumnLayoutModel layout = new GridColumnLayoutModel();
            layout.Key = "Products";
            layout.ForeignKey = "CategoryID";
            layout.PrimaryKey = "ID";
            layout.AutoGenerateColumns = false;
            layout.Columns.Add(new GridColumn() { HeaderText = "Product ID", Key = "ID", DataType = "number", Width = "0%", Hidden = true });
            layout.Columns.Add(new GridColumn() { HeaderText = "Category ID", Key = "CategoryID", DataType = "number",Width = "0%", Hidden = true});
            layout.Columns.Add(new GridColumn() { HeaderText = "Product Name", Key = "ProductName", DataType = "string",Width = "40%"});
            layout.Columns.Add(new GridColumn() { HeaderText = "Unit Price", Key = "UnitPrice", DataType = "number", Width = "30%" });
            layout.Columns.Add(new GridColumn() { HeaderText = "Units In Stock", Key = "UnitsInStock", DataType = "number", Width = "30%" });
            
            layout.Width = "100%";
            grid.ColumnLayouts.Add(layout);

            GridPaging paging = new GridPaging();
            paging.Type = OpType.Remote;
            paging.Inherit = true;
            paging.PageSize = 5;
            grid.Features.Add(paging);

            return grid;
        }
        #endregion // Grid Models

        #region Layouts

        private GridColumnLayoutModel GetProductsLayout()
        {
            GridColumnLayoutModel layout = new GridColumnLayoutModel();
            layout.Key = "Products";
            layout.PrimaryKey = "ID";
            layout.ForeignKey = "CategoryID";
            layout.Width = "100%";
            layout.AutoGenerateColumns = false;
            //layout.AutofitLastColumn = false;
            layout.Columns = new List<GridColumn>();
            layout.Columns.Add(new GridColumn() { HeaderText = "Product ID", Key = "ID", DataType = "number", Width = "15%"});
            layout.Columns.Add(new GridColumn() { HeaderText = "Product Name", Key = "ProductName", DataType = "string", Width = "20%"});
            layout.Columns.Add(new GridColumn() { HeaderText = "Unit Price", Key = "UnitPrice", DataType = "number", Width = "15%"});
            layout.Columns.Add(new GridColumn() { HeaderText = "Units In Stock", Key = "UnitsInStock", DataType = "number", Width = "15%"});
            layout.Columns.Add(new GridColumn() { HeaderText = "Category ID", Key = "CategoryID", DataType = "number", Width = "0%", Hidden = true });
            layout.Columns.Add(new GridColumn() { HeaderText = "Discontinued", Key = "Discontinued", DataType = "bool", Width = "15%" });
            layout.DataSourceUrl = this.Url.Action("GetProductsData");
            layout.Features.Add(new GridResponsive()
            {
                EnableVerticalRendering = false,
                ColumnSettings = new List<ResponsiveColumnSetting>() { 
                    new ResponsiveColumnSetting() { ColumnKey = "ID", Classes = "ui-hidden-phone ui-hidden-tablet" },
                    new ResponsiveColumnSetting() { ColumnKey = "UnitsInStock", Classes = "ui-hidden-phone ui-hidden-tablet" },
                    new ResponsiveColumnSetting() { ColumnKey = "UnitPrice", Classes = "ui-hidden-phone ui-hidden-tablet" }
                }
            });
            return layout;
        }

        #endregion // Layouts

        #region DataSourceUrls

        public JsonResult GetCategoriesData()
        {
            GridModel model = GetGridModel();
            return model.GetData();
        }
      
        public JsonResult GetProductsData(string path, string layout)
        {
            GridModel gridModel = GetGridModel();
            gridModel.DataSource = RepositoryFactory.GetProductRepository().Get();
            return gridModel.GetData(path, layout);
        }

        public JsonResult BindParent()
        {
            GridModel model = GridLoadOnDemandModel();
            model.DataSource = RepositoryFactory.GetCategoryRepository().Get();
            return model.GetData();
        }

        public JsonResult BindChild(string path, string layout)
        {
            GridModel model = GridLoadOnDemandModel();
            model.DataSource = RepositoryFactory.GetProductRepository().Get();
            return model.GetData(path, layout);
        }

        #endregion // DataSourceUrls

        #region SampleData

        private DataSet CategoriesProducts
        {
            get
            {
                if (Session["CategoriesProducts"] == null)
                {
                    Session["CategoriesProducts"] = GetCategoriesProductDataSet();
                }
                return (DataSet)Session["CategoriesProducts"];
            }
        }

        private DataSet GetCategoriesProductDataSet()
        {
            DataSet ds = new DataSet("CategoriesProductRecords");

            // Get the tables to use in the DataSet
            DataTable categories = this.GetCategoriesDataTable();
            DataTable products = this.GetProductsDataTable();

            // Add the tables to the DataSet
            ds.Tables.Add(categories);
            ds.Tables.Add(products);

            // Create the relationship between the tables.
            ds.Relations.Add("CategoryProductRel", categories.Columns["ID"], products.Columns["CategoryID"]);
            return ds;
        }

        private DataTable GetCategoriesDataTable()
        {
            string connString = new NorthwindContext().Database.Connection.ConnectionString;
            string selectCategoriesStmt = "Select CategoryID as ID, CategoryName, Description from Categories";

            SqlDataAdapter da = new SqlDataAdapter(selectCategoriesStmt, connString);
            DataTable dtData = new DataTable("Categories");
            da.Fill(dtData);

            dtData.PrimaryKey = new DataColumn[] { dtData.Columns["ID"] };
            return dtData;
        }

        private DataTable GetProductsDataTable()
        {
            string connString = new NorthwindContext().Database.Connection.ConnectionString;
            string selectProductsStmt = "Select ProductID as ID, ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued  from Products";

            SqlDataAdapter da = new SqlDataAdapter(selectProductsStmt, connString);
            DataTable dtData = new DataTable("Products");
            da.Fill(dtData);

            dtData.PrimaryKey = new DataColumn[] { dtData.Columns["ID"] };
            return dtData;
        }

        #endregion SampleData
    }

}
