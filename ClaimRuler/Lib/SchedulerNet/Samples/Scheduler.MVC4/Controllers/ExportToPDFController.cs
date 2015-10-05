using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHTMLX.Scheduler;

namespace SchedulerTest.Controllers
{
    /// <summary>
    /// export to pdf,
    /// 'ToPDF' function is rendered on View/ExportToPDF/Index.aspx.
    /// Using online service to generate pdf document,
    /// For more details see http://scheduler-net.com/docs/data_export.html#export_to_pdf
    /// </summary>
    public class ExportToPDFController : BasicSchedulerController
    {

        public override ActionResult Index()
        {
            var sched = new DHXScheduler();
           
            sched.Controller = "BasicScheduler";
            sched.Config.first_hour = 8;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
           

            // 'PDF' extension is required
            sched.Extensions.Add(SchedulerExtensions.Extension.PDF);


            sched.InitialDate = new DateTime(2011, 9, 19);
            return View(sched);
        }

    }
}
