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
using DHTMLX.Scheduler.Authentication;

namespace SchedulerTest.Controllers
{
    public class SchedulerAuthorizationController : Controller
    {
        //
        // GET: /SchedulerAuthorization/

        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
           
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
         

            var context = new DHXSchedulerModelsDataContext();
            
            if (Request.IsAuthenticated)
            {
                var user = context.UserDetails.SingleOrDefault(u => u.UserName == this.HttpContext.User.Identity.Name);
                sched.SetUserDetails(user, "UserId");//pass dictionary<string, object> or any object which can be serialized to json(without circular references)
                sched.Authentication.EventUserIdKey = "user_id";//set field in event which will be compared to user id(same as sched.Authentication.UserIdKey by default)    
            }
            sched.SetEditMode(EditModes.OwnEventsOnly, EditModes.AuthenticatedOnly);

            sched.InitialDate = new DateTime(2011, 9, 26);
            return View(sched);
        }

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData((new DHXSchedulerModelsDataContext()).Events);
          
            return (data);
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);
            var changedEvent = (Event)DHXEventsHelper.Bind(typeof(Event), actionValues);
            DHXSchedulerModelsDataContext data = new DHXSchedulerModelsDataContext();
            if (this.Request.IsAuthenticated && changedEvent.user_id == data.UserDetails.SingleOrDefault(u => u.UserName == this.HttpContext.User.Identity.Name).UserId)
            {
               
                try
                {
                    switch (action.Type)
                    {
                        case DataActionTypes.Insert:
                            changedEvent.room_id = data.Rooms.First().key;
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
            }
            else
            {
                action.Type = DataActionTypes.Error;
            }
            return (new AjaxSaveResponse(action));
        }

    }
}
