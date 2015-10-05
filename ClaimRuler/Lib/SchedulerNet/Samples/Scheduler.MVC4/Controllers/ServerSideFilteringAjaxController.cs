using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Common;
using DHXScheduler_MVC4.Models;

namespace SchedulerTest.Controllers
{
    public class ServerSideFilteringAjaxController : Controller
    {
        //
        // GET: /ServerSideFilteringAjax/
        public class SchedulerFilterModel
        {
            public DHXScheduler Scheduler{get;set;}
            public IEnumerable<Room> Rooms { get; set; }
            public SchedulerFilterModel(DHXScheduler sched, IEnumerable<Room> rooms)
            {
                this.Scheduler = sched;
                this.Rooms = rooms;
            }
        }
        public ActionResult Index(FormCollection data)
        {

            var sched = new DHXScheduler(this);
           
            var rooms = new DHXSchedulerModelsDataContext().Rooms.ToList();

            var unit = new UnitsView("rooms", "room_id");          
            unit.Label = "Rooms";

            unit.AddOptions(rooms);
            sched.Views.Add(unit);

            sched.InitialView = unit.Name;
            sched.InitialDate = new DateTime(2011, 9, 7);
            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            return View(new SchedulerFilterModel(sched, rooms));

        }

        public ContentResult Data()
        {
            var dc = new DHXSchedulerModelsDataContext(); 
   
            IEnumerable<Event> dataset;

            if (this.Request.QueryString["rooms"] == null)
                dataset = dc.Events;
            else
            {
                var current_room = int.Parse(this.Request.QueryString["rooms"]);
                dataset = from ev in dc.Events where ev.room_id == current_room select ev;
            }

            var data = new SchedulerAjaxData(dataset);

   
            return (data);
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);
            var changedEvent = (Event)DHXEventsHelper.Bind(typeof(Event), actionValues);
            DHXSchedulerModelsDataContext data = new DHXSchedulerModelsDataContext();
            try
            {
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
