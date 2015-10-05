using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler.GoogleCalendar;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using SchedulerTest.Models;
namespace SchedulerTest.Controllers
{
    public class GoogleCalendarController : Controller
    {
        //
        // GET: /GoogleCalendar/

        public ActionResult Index()
        {
            var sched = new DHXScheduler();

   
            sched.InitialDate = new DateTime(2011, 7, 19);
            sched.LoadData = true;
            
            sched.InitialView = sched.Views[0].Name;
            sched.Config.isReadonly = true;
            sched.DataFormat = SchedulerDataLoader.DataFormats.iCal;
            sched.DataAction = Url.Action("Data", "GoogleCalendar");
            return View(sched);
        }

        public ActionResult MixedContent()
        {
            var sched = new DHXScheduler(this);
            sched.Config.multi_day = true;
            sched.InitialDate = new DateTime(2011, 7, 19);
            sched.LoadData = true;
            sched.InitialView = sched.Views[0].Name;
            sched.DataAction = "Mixed";
            return View(sched);
        }
        public ContentResult Data()
        {
            var data = new SchedulerAjaxData();  
                    
            return Content(data.FromUrl("https://www.google.com/calendar/ical/b0prga519c0g0t3crcnc0g9in0@group.calendar.google.com/public/basic.ics"));

        }
        public ContentResult Mixed()
        {
         //   var helper = new 
        //    var helper = new ICalHelper();
        //    var events = helper.GetFromFeed("https://www.google.com/calendar/ical/b0prga519c0g0t3crcnc0g9in0@group.calendar.google.com/public/basic.ics");

            var data = new SchedulerAjaxData();
            data.FromICal("https://www.google.com/calendar/ical/b0prga519c0g0t3crcnc0g9in0@group.calendar.google.com/public/basic.ics");
            data.Add(new DHXSchedulerDataContext().Events);
            return data;
        }
        public ActionResult ExportToGoogleCalendar()
        {
            var help = new GoogleCalendarHelper("login", "password");
            help.ExportToGoogleCalendar("https://www.google.com/calendar/feeds/default/private/full", new DHXSchedulerDataContext().Events);

            
            return Content("");
        }
    }
}
