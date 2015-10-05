using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class FunnelChartController : Controller
    {
        //
        // GET: /FunnelChart/
        [ActionName("server-binding")]
        public ActionResult BindCollection()
        {
            var data = new List<BudgetData>()
            {
                new BudgetData { Budget = 30, Department = "Administration" },
                new BudgetData { Budget = 50, Department = "Sales" },
                new BudgetData { Budget = 60, Department = "IT" },
                new BudgetData { Budget = 50, Department = "Marketing" },
                new BudgetData { Budget = 100, Department = "Development" },
                new BudgetData { Budget = 20, Department = "Support" }
            };
            return View("server-binding", data.AsQueryable());            
        }
        
        [ActionName("getBudget")]
        public JsonResult GetBudget()
        {
            var data = new List<BudgetData>()
            {
                new BudgetData { Budget = 30, Department = "Administration" },
                new BudgetData { Budget = 50, Department = "Sales" },
                new BudgetData { Budget = 60, Department = "IT" },
                new BudgetData { Budget = 50, Department = "Marketing" },
                new BudgetData { Budget = 100, Department = "Development" },
                new BudgetData { Budget = 20, Department = "Support" }
            };

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

    public class BudgetData
    {
        public decimal Budget { get; set; }
        public string Department { get; set; }
    }
}
