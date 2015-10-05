using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DHTMLX.Common;
using DHTMLX.Helpers;
using System.Globalization;

namespace SchedulerNetAsp
{
    /// <summary>
    /// </summary>
    public class SaveRecurring : IHttpHandler
    {


        protected bool deleteRelated(DataAction action, Recurring changedEvent, SchedulerDataContext context)
        {
            bool finished = false;
            if ((action.Type == DataActionTypes.Delete || action.Type == DataActionTypes.Update) && !string.IsNullOrEmpty(changedEvent.rec_type))
            {
                context.Recurrings.DeleteAllOnSubmit(from ev in context.Recurrings where ev.event_pid == changedEvent.id select ev);
            }
            if (action.Type == DataActionTypes.Delete && changedEvent.event_pid != 0)
            {
                Recurring changed = (from ev in context.Recurrings where ev.id == action.TargetId select ev).Single();
                changed.rec_type = "none";
                finished = true;
            }
            return finished;
        }

        protected DataAction insertRelated(DataAction action, Recurring changedEvent, SchedulerDataContext context)
        {
            if (action.Type == DataActionTypes.Insert && changedEvent.rec_type == "none")
            {//insert_related
                 action.Type = DataActionTypes.Delete;
            }
            return action;
        }
        public void ProcessRequest(HttpContext context)
        {
            var action = new DataAction(context.Request.Form);
            var data = new SchedulerDataContext();

            try
            {

                var changedEvent = (Recurring)DHXEventsHelper.Bind(typeof(Recurring), context.Request.Form);//create model object from the request fields

                bool isFinished = deleteRelated(action, changedEvent, data);
                if (!isFinished)
                {
                    switch (action.Type)
                    {
                        case DataActionTypes.Insert: // define here your Insert logic
                            data.Recurrings.InsertOnSubmit(changedEvent);
                            
                            break;
                        case DataActionTypes.Delete: // define here your Delete logic
                            changedEvent = data.Recurrings.SingleOrDefault(ev => ev.id == action.SourceId);
                            data.Recurrings.DeleteOnSubmit(changedEvent);
                            break;
                        default:// "update" // define here your Update logic
                            var updated = data.Recurrings.SingleOrDefault(ev => ev.id == action.SourceId);
                            DHXEventsHelper.Update(updated, changedEvent, new List<string>() { "id" });
                            

                            break;
                    }
                }
                data.SubmitChanges();
                action.TargetId = changedEvent.id;
                action = insertRelated(action, changedEvent, data);
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            context.Response.ContentType = "text/xml";
            context.Response.Write(new AjaxSaveResponse(action).ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}