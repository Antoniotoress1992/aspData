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
    /// using unobtrusive validation
    /// </summary>
    public class JQueryValidationController : Controller
    {
        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);

            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            sched.Lightbox.SetExternalLightbox("JQueryValidation/LightboxControl", 400, 140);

            sched.InitialDate = new DateTime(2011, 9, 5);

            //try in new skin
            //sched.Skin = DHXScheduler.Skins.Terrace;

            return View(sched);
        }



        public ActionResult LightboxControl(ValidEvent ev)
        {
            var context = new DHXSchedulerDataContext();
            var current = context.ValidEvents.SingleOrDefault(e => e.id == ev.id);
            if (current == null)
                current = ev;
            return View(current);
        }

        public ContentResult Data()
        {
            return (new SchedulerAjaxData((new DHXSchedulerDataContext()).ValidEvents));
        }

        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<ValidEvent>(actionValues);
            if (action.Type != DataActionTypes.Error)
            {
                //handle changes done without lightbox - dnd, resize..
                return NativeSave(action, changedEvent, actionValues);
            }
            else
            {
                return CustomFormSave(action, changedEvent, actionValues);
            }



        }

        public ContentResult CustomFormSave(DataAction action, ValidEvent changedEvent, FormCollection actionValues)
        {


            if (actionValues["actionType"] != null)
            {
                var actionType = actionValues["actionType"].ToLower();
                var data = new DHXSchedulerDataContext();
                try
                {
                    if (actionType == "save")
                    {

                        if (data.ValidEvents.SingleOrDefault(ev => ev.id == action.SourceId) != null)
                        {
                            //update event
                            var eventToUpdate = data.ValidEvents.SingleOrDefault(ev => ev.id == action.SourceId);

                            DHXEventsHelper.Update(eventToUpdate, changedEvent, new List<string>() { "id" });

                            action.Type = DataActionTypes.Update;
                        }
                        else
                        {
                            //create event                           
                            data.ValidEvents.InsertOnSubmit(changedEvent);
                            action.Type = DataActionTypes.Insert;
                        }
                    }
                    else if (actionType == "delete")
                    {

                        changedEvent = data.ValidEvents.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.ValidEvents.DeleteOnSubmit(changedEvent);

                        action.Type = DataActionTypes.Delete;
                    }
                    data.SubmitChanges();
                }

                catch (Exception e)
                {
                    action.Type = DataActionTypes.Error;
                }
            }



            return (new SchedulerFormResponseScript(action, changedEvent));
        }

        public ContentResult NativeSave(DataAction action, ValidEvent changedEvent, FormCollection actionValues)
        {

            var data = new DHXSchedulerDataContext();
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        data.ValidEvents.InsertOnSubmit(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = data.ValidEvents.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.ValidEvents.DeleteOnSubmit(changedEvent);
                        break;
                    default:// "update"                          
                        var eventToUpdate = data.ValidEvents.SingleOrDefault(ev => ev.id == action.SourceId);
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
