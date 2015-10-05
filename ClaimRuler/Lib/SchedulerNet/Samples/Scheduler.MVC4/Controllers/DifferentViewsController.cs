
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHXScheduler_MVC4.Models;
using System.Web.Security;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
namespace SchedulerTest.Controllers
{
    public class DifferentModesController : Controller
    {
        
        /// <summary>
        /// Week agenda and year view
        /// </summary>
        /// <returns></returns>
        public ActionResult YearNWeek()
        {
            var scheduler = new DHXScheduler(this);
        
            scheduler.Views.Clear();
            scheduler.Views.Add(new WeekView());

            var year = new YearView();

            scheduler.Views.Add(year);
            scheduler.InitialView = year.Name;
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;
            return View(scheduler);
        }

        /// <summary>
        /// Google map in calendar
        /// </summary>
        /// <returns></returns>
        public ActionResult GoogleMap()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Terrace;
            scheduler.Views.Clear();        
            scheduler.Views.Add(new WeekView());
            scheduler.Views.Add(new MapView());
            scheduler.InitialView = (new MapView()).Name;
            scheduler.LoadData = true;
            scheduler.DataAction = "MapEvents";
            return View(scheduler);
        }


        /// <summary>
        /// Add resources views - units and timeline
        /// </summary>
        /// <returns></returns>
        public ActionResult MultipleResources()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Terrace;    
            var rooms = new DHXSchedulerModelsDataContext().Rooms.ToList();

            scheduler.Views.Clear();
            scheduler.Views.Add(new MonthView());
            
            var unit = new UnitsView("unit", "room_id");
            unit.AddOptions(rooms);//parse model objects
            scheduler.Views.Add(unit);

            


            var timeline = new TimelineView("timeline", "room_id");
            timeline.RenderMode = TimelineView.RenderModes.Bar;
            timeline.FitEvents = false;
            timeline.AddOptions(rooms);
            scheduler.Views.Add(timeline);



            //show timeline view by default
            scheduler.InitialView = timeline.Name;

            scheduler.Lightbox.AddDefaults();//add default set of options - text and timepicker
            //add controls to the details form
            var select = new LightboxSelect("room_id", "Room id");
            select.AddOptions(rooms);
            scheduler.Lightbox.Add(select);

 
            scheduler.LoadData = true;
            scheduler.InitialDate = new DateTime(2011, 9, 7);
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }


        public ContentResult Data()
        {
            var data = new SchedulerAjaxData((new DHXSchedulerModelsDataContext()).Events);
    
            return data;
        }

        public ContentResult OtherData()
        {
            var data = new SchedulerAjaxData(new CustomFieldsDataContext().ColoredEvents);
        
            return data;
        }
        public ContentResult MapEvents()
        {
            var today = DateTime.Today;

            var data = new SchedulerAjaxData(new List<object>() {
                new {id=2, text="Kurtzenhouse", start_date=today.AddDays(1).AddHours(13), end_date=today.AddDays(1).AddHours(16), lat=48.7396839, lng=7.813368099999934, event_location="D37, 67240 Kurtzenhouse, France"},
                new {id=3, text="Forêt Domaniale", start_date=today.AddDays(2).AddHours(10), end_date=today.AddDays(2).AddHours(12), lat=48.767333, lng=5.793258000000037, event_location="Forêt Domaniale de la Reine, Véry, 54200 Royaumeix, France"},         
                new {id=4, text="Windstein", start_date=today.AddDays(3).AddHours(7), end_date=today.AddDays(3).AddHours(8), lat=49.0003477, lng=7.687306499999977, event_location="1 Rue du Nagelsthal, 67110 Windstein, France"}
                });


            return data;
        }
       

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            DHXSchedulerModelsDataContext data = new DHXSchedulerModelsDataContext();
            try
            {
                var changedEvent = (Event)DHXEventsHelper.Bind(typeof(Event), actionValues);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        data.Events.InsertOnSubmit(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = data.Events.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.Events.DeleteOnSubmit(changedEvent);
                        break;
                    default:// "update"                          
                        var eventToUpdate = data.Events.SingleOrDefault(ev => ev.id == action.SourceId);
                        DHXEventsHelper.Update(eventToUpdate, changedEvent, new List<string>() { "id" });
                        break;
                }
                data.SubmitChanges();
                action.TargetId = changedEvent.id;
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (new AjaxSaveResponse(action));
        }
    }
}
