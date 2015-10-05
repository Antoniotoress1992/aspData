using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

using SchedulerTest.Models;

namespace SchedulerTest.Controllers
{
    /// <summary>
    /// Recurring events support
    /// </summary>
    public class RecurringEventsController : Controller
    {

        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this);
 

            scheduler.InitialDate = new DateTime(2011, 10, 1);
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Recurring);
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;
         

            return View(scheduler);
        }
        public ActionResult Data()
        {
            //recurring events have 3 additional mandatory properties
            // string rec_type
            // Nullable<long> event_length
            // Nullable<int> event_pid
            return new SchedulerAjaxData(new DHXSchedulerDataContext().Recurrings);
        }

        
       

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);

            DHXSchedulerDataContext data = new DHXSchedulerDataContext();
            try
            {
                var changedEvent = DHXEventsHelper.Bind<Recurring>(actionValues);
                //operations with recurring events require some additional handling
                bool isFinished = deleteRelated(action, changedEvent, data);
                if (!isFinished)
                {
                    switch (action.Type)
                    {

                        case DataActionTypes.Insert:
                            data.Recurrings.InsertOnSubmit(changedEvent);
                            if (changedEvent.rec_type == "none")//delete one event from the serie
                                action.Type = DataActionTypes.Delete;
                            break;
                        case DataActionTypes.Delete:
                            changedEvent = data.Recurrings.SingleOrDefault(ev => ev.id == action.SourceId);
                            data.Recurrings.DeleteOnSubmit(changedEvent);
                            break;
                        default:// "update"   
                            var eventToUpdate = data.Recurrings.SingleOrDefault(ev => ev.id == action.SourceId);
                            DHXEventsHelper.Update(eventToUpdate, changedEvent, new List<string>() { "id" });
                            break;
                    }
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
        protected bool deleteRelated(DataAction action, Recurring changedEvent, DHXSchedulerDataContext context)
        {
            bool finished = false;
            if ((action.Type == DataActionTypes.Delete || action.Type == DataActionTypes.Update) && !string.IsNullOrEmpty(changedEvent.rec_type))
            {
                context.Recurrings.DeleteAllOnSubmit(from ev in context.Recurrings where ev.event_pid == changedEvent.id select ev);
            }
            if (action.Type == DataActionTypes.Delete && (changedEvent.event_pid != 0 && changedEvent.event_pid != null))
            {
                Recurring changed  = (from ev in context.Recurrings where ev.id == action.TargetId select ev).Single() ;
                changed.rec_type = "none";
                finished = true;
            }
            return finished;
        }
    }
}
