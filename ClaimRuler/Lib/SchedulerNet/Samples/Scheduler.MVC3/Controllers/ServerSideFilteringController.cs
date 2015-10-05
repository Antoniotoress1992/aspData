using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Common;
using SchedulerTest.Models;

namespace SchedulerTest.Controllers
{
    public class ServerSideFilteringController : Controller
    {
        //
        // GET: /ServerSideFiltering/
        public class SchedulerFilterModel
        {
            public DHXScheduler Scheduler { get; set; }
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

            sched.Extensions.Add(SchedulerExtensions.Extension.Cookie);

            var rooms = new DHXSchedulerDataContext().Rooms.ToList();
                    
            int selectedRoom;
            if (this.Request.QueryString["filter"] != null)
            {
                // parameters will be added to data url
                sched.Data.Loader.AddParameters(this.Request.QueryString);
                selectedRoom = int.Parse(this.Request.QueryString["rooms"]);
            }
            else
            {
                selectedRoom = rooms.First().key;               
            }


            var unit = new UnitsView("rooms", "room_id");
            unit.Label = "Rooms";
            unit.AddOptions(rooms);
            sched.Views.Add(unit);


            sched.Lightbox.AddDefaults();
            var select = new LightboxSelect("room_id", "Room");
            select.AddOptions(rooms);
            sched.Lightbox.Add(select); 


            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialView = unit.Name;
            ViewData["rooms"] = selectedRoom;
            sched.InitialDate = new DateTime(2011, 9, 7);
            return View(new SchedulerFilterModel(sched, rooms));

        }
        public ContentResult Data()
        {
            var dc = new DHXSchedulerDataContext();

            IEnumerable<Event> dataset;

            if (this.Request.QueryString["rooms"] == null)
                dataset = dc.Events;
            else{
                var current_room = int.Parse(this.Request.QueryString["rooms"]);
                dataset = from ev in dc.Events where ev.room_id == current_room select ev;
            }


            var data = new SchedulerAjaxData(dataset);

   
            return (data);
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);

            DHXSchedulerDataContext data = new DHXSchedulerDataContext();
            var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
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
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }

    }
}
