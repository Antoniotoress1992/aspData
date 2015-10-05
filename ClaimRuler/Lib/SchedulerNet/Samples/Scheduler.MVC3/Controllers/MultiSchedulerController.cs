using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler;
namespace SchedulerTest.Controllers
{
    /// <summary>
    /// you can create multiple scheduler on page
    /// </summary>
    public class MultiSchedulerController : Controller
    {
        //
        // GET: /MultiScheduler/
        public class mod
        {
            public DHXScheduler sh1 { get; set; }
            public DHXScheduler sh2 { get; set; }
        }
        public ActionResult Index()
        {
            //each scheduler must have unique name
            var scheduler = new DHXScheduler("sched1");
  
            scheduler.InitialView = scheduler.Views[scheduler.Views.Count - 1].Name;
            var scheduler2 = new DHXScheduler("sched2");
   
            return View(new mod() { sh1 = scheduler, sh2 = scheduler2 });
        }

    }
}
