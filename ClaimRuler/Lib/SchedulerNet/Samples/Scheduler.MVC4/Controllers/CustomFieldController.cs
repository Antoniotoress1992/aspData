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
    public class CustomFieldController : Controller
    {

        public ActionResult Index()
        {
          
            var sched = new DHXScheduler(this);
           
          
            sched.Lightbox.Add(new LightboxText("text", "Description"));

            var check = new LightboxCheckbox("highlighting", "Important");
            check.MapTo = "color";//checkbox will set value of 'color' property of the event
            check.CheckedValue = "#FE7510";
 
            sched.Lightbox.Add(check);

            sched.LoadData = true;
            sched.EnableDataprocessor = true;

            //allows to postback changes from the server
            sched.UpdateFieldsAfterSave();

            sched.InitialDate = new DateTime(2011, 9, 5);
            return View(sched);
        }
     

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData((new CustomFieldsDataContext()).ColoredEvents);
      
            return (data);
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {

            var action = new DataAction(actionValues);
            var changedEvent = (ColoredEvent)DHXEventsHelper.Bind(typeof(ColoredEvent), actionValues);
            var color = "";
            if (actionValues["color"] == "#FE7510")
            {
                color = "#FE7510";
            }
            else
            {
                if (changedEvent.start_date < DateTime.Now)
                    color = "#ccc";
                else
                    color = "#76B007";
            }        
         
            CustomFieldsDataContext data = new CustomFieldsDataContext();
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        changedEvent.color = color;
                        data.ColoredEvents.InsertOnSubmit(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = data.ColoredEvents.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.ColoredEvents.DeleteOnSubmit(changedEvent);
                        break;
                    default:// "update"                          
                        var eventToUpdate = data.ColoredEvents.SingleOrDefault(ev => ev.id == action.SourceId);
                        DHXEventsHelper.Update(eventToUpdate, changedEvent, new List<string>() { "id" });

                        changedEvent.color = color;


                        break;
                }
                data.SubmitChanges();
                action.TargetId = changedEvent.id;
            }
            catch 
            {
                action.Type = DataActionTypes.Error;
            }

            var result = new AjaxSaveResponse(action);
            result.UpdateField("color", color);//property will be updated on the client
            return result;
        }

    }
}
