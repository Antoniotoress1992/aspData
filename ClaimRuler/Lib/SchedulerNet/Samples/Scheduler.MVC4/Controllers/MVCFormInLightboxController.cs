using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHXScheduler_MVC4.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;

using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

namespace SchedulerTest.Controllers
{
    /// <summary>
    /// use server-side form instead of default details form
    /// </summary>
    public class MVCFormInLightboxController : Controller
    {
        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
            
            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            //view, width, height
            var box = sched.Lightbox.SetExternalLightbox("MVCFormInLightbox/LightboxControl", 420, 195);
            //css class to be applied to the form
            box.ClassName = "custom_lightbox";
            sched.InitialDate = new DateTime(2011, 9, 5);

            //try in new skin
            //sched.Skin = DHXScheduler.Skins.Terrace;

            return View(sched);
        }



        public ActionResult LightboxControl(Event ev)
        {
            var context = new DHXSchedulerModelsDataContext();
            var current = context.Events.SingleOrDefault(e => e.id == ev.id);
            if (current == null)
                current = ev;
            return View(current);
        }

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData((new DHXSchedulerModelsDataContext()).Events);
        
            return (data);
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = (Event)DHXEventsHelper.Bind(typeof(Event), actionValues);
            if (action.Type != DataActionTypes.Error)
            {
                //process resize, d'n'd operations...
                return NativeSave(changedEvent, actionValues);
            }
            else
            {
                //custom form operation
                return CustomSave(changedEvent, actionValues);
            }

        }

        public ActionResult CustomSave(Event changedEvent, FormCollection actionValues)
        {

            var action = new DataAction(DataActionTypes.Update, changedEvent.id, changedEvent.id);
            if (actionValues["actionButton"] != null)
            {
                DHXSchedulerModelsDataContext data = new DHXSchedulerModelsDataContext();
                try
                {
                    if (actionValues["actionButton"] == "Save")
                    {

                        if (data.Events.SingleOrDefault(ev => ev.id == action.SourceId) != null)
                        {
                            var eventToUpdate = data.Events.SingleOrDefault(ev => ev.id == action.SourceId);
                            DHXEventsHelper.Update(eventToUpdate, changedEvent, new List<string>() { "id" });                           
                        }
                        else
                        {
                            action.Type = DataActionTypes.Insert;
                            data.Events.InsertOnSubmit(changedEvent);
                        }
                    }else if(actionValues["actionButton"] == "Delete"){
                        action.Type = DataActionTypes.Delete;
                        changedEvent = data.Events.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.Events.DeleteOnSubmit(changedEvent);
                    }                       
                    data.SubmitChanges();
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


            return (new SchedulerFormResponseScript(action, changedEvent));
           
        }

        public ContentResult NativeSave(Event changedEvent, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);

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
