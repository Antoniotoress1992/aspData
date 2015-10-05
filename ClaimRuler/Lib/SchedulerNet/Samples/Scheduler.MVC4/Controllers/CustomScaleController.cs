using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

using DHXScheduler_MVC4.Models;

namespace SchedulerTest.Controllers
{
    public class CustomScaleController : Controller
    {
        //
        // GET: /CustomizeScale/

        public ActionResult Index()
        {

            var sched = new DHXScheduler(this);
        
            sched.Config.first_hour = 8;
            sched.Config.last_hour = 18;
            sched.InitialDate = new DateTime(2011, 9, 19);

            var rooms = new DHXSchedulerModelsDataContext().Rooms.ToList();
            var timeline = new TimelineView("timeline", "room_id");
            timeline.X_Unit = TimelineView.XScaleUnits.Hour;
            timeline.X_Size = 72;

            timeline.Scale.IgnoreRange(19, 23);
            timeline.Scale.IgnoreRange(0, 7);

            timeline.RenderMode = TimelineView.RenderModes.Bar;
            timeline.FitEvents = false;


            sched.TimeSpans.Add(new DHXMarkTime
            {
                FullWeek = true,
                Zones = new List<Zone> { new Zone(8 * 60 + 10, 19 * 60 - 10) },
                CssClass = "day_split",
                InvertZones = true,
                Sections = new List<Section>{
                    new Section(timeline.Name, rooms.Select(r => r.key.ToString()).ToList())
                
                }
            });

            timeline.AddOptions(rooms);
            sched.Views.Add(timeline);
            sched.InitialView = timeline.Name;
            var week = sched.Views[1];
            week.Scale.Ignore((int)DayOfWeek.Saturday, (int)DayOfWeek.Sunday);
            

            sched.LoadData = true;

            return View(sched);
        }
        public ContentResult Data()
        {
            var data = new SchedulerAjaxData(new DHXSchedulerModelsDataContext().Events);
            return data;
        }
    }
}
