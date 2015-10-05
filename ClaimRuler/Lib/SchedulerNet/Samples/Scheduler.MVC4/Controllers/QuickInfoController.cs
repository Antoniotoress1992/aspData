using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHXScheduler_MVC4.Models;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;


namespace SchedulerTest.Controllers
{
    public class QuickInfoController : Controller
    {
        public virtual ActionResult Index()
        {

            var sched = new DHXScheduler(this);
          
            sched.InitialDate = new DateTime(2011, 9, 19);

            sched.Data.Parse(new DHXSchedulerModelsDataContext().Events);

            sched.Extensions.Add(SchedulerExtensions.Extension.QuickInfo);
            return View(sched);
        }

    }
}