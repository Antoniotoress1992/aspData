using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SchedulerTest.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

namespace SchedulerTest.Controllers
{
    /// <summary>
    /// It's not necessary to load data with ajax,
    /// it can be rendered initially.
    /// </summary>
    public class StaticLoadingController : Controller
    {
        public ActionResult Index()
        {
        
            var sched = new DHXScheduler(this);
       
            var unit = new UnitsView("unit1", "room_id");


            sched.Views.Add(unit);
            var context = new DHXSchedulerDataContext();
            unit.AddOptions(context.Rooms.ToList());

            sched.Data.Parse(context.Events);
            sched.EnableDataprocessor = true;
            sched.InitialDate = new DateTime(2011, 9, 5);

            return View(sched);
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
