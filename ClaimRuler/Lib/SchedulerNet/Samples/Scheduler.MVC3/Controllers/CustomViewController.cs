using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
namespace SchedulerTest.Controllers
{
    /// <summary>
    /// See how custom views can be created
    /// </summary>
    public class CustomViewController : Controller
    {

        //real functionality is defined on the client side
        public class DecadeView : SchedulerView
        {
            public DecadeView()
                : base()
            {
                Name = ViewType = "decade";//view type must be equal the one defined on the client
            }
        }
        public class WorkWeekView : SchedulerView
        {
            public WorkWeekView():base()
            {
                Name = ViewType = "workweek";
            }
        }
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler();
            scheduler.Views.Add(new WorkWeekView() { 
                Label = "W-Week"           
            });
            scheduler.Views.Add(new DecadeView()
            {
                Label = "Decade"
            });
            return View(scheduler);
        }

    }
}
