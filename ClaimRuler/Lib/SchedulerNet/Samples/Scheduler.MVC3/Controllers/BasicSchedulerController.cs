using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SchedulerTest.Models;
using System.Web.Security;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;


namespace SchedulerTest.Controllers
{
    public class BasicSchedulerController : Controller
    {
        /// <summary>
        /// Default initialization
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {

            var sched = new DHXScheduler(this);

		sched.InitialDate = new DateTime(2011, 9, 19);

           


            //load data initially
            sched.LoadData = true;

            //save client-side changes
            sched.EnableDataprocessor = true; 
            return View(sched);
        }
        
        /// <summary>
        /// Block time areas
        /// </summary>
        /// <returns></returns>
        public ActionResult Limit()
        {
            var sched = new DHXScheduler(this);

            sched.InitialDate = new DateTime(2011, 9, 19);

            
            // block specific date range
            sched.TimeSpans.Add(new DHXBlockTime()
            {
                StartDate = new DateTime(2011, 9, 14),
                EndDate = new DateTime(2011, 9, 17)
            });

            // block each tuesday from 12AM to 15PM 
            sched.TimeSpans.Add(new DHXBlockTime()
            {
                Day = DayOfWeek.Tuesday,
                Zones = new List<Zone>() { new Zone { Start = 0, End = 15 * 60 } }
            });
            
            // block each sunday
            sched.TimeSpans.Add(new DHXBlockTime()
            {
                Day = DayOfWeek.Sunday
            });

            // block each monday from 12am to 8am, and from 18pm to 12am of the next day
            sched.TimeSpans.Add(new DHXBlockTime()
            {
                Day = DayOfWeek.Monday,
                Zones = new List<Zone>() { new Zone { Start = 0, End = 8 * 60 }, new Zone { Start = 18 * 60, End = 24 * 60 } }
            });
            #region add units and timeline
            var rooms = new DHXSchedulerDataContext().Rooms.ToList();
            var unit = new UnitsView("unit", "room_id");
            unit.AddOptions(rooms);//parse model objects
            sched.Views.Add(unit);

            var timeline = new TimelineView("timeline", "room_id");
            timeline.RenderMode = TimelineView.RenderModes.Bar;
            timeline.FitEvents = false;
            timeline.AddOptions(rooms);
            sched.Views.Add(timeline);
            #endregion
            //block different sections in timeline and units view
            sched.TimeSpans.Add(new DHXBlockTime()
            {
                FullWeek = true,
                Sections = new List<Section>() {  
                    new Section(unit.Name, rooms.Take(2).Select(r => r.key.ToString()).ToArray()),
                    new Section(timeline.Name, rooms.Skip(2).Select(r => r.key.ToString()).ToArray())
                }
            });


            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            return View(sched);

        }

        /// <summary>
        /// Colors for time areas
        /// </summary>
        /// <returns></returns>
        public ActionResult MarkedTimeSpans()
        {
            var sched = new DHXScheduler(this);

            sched.InitialDate = new DateTime(2011, 9, 19);

           
            sched.TimeSpans.Add(new DHXMarkTime()
            {
                Day = DayOfWeek.Thursday,
                CssClass = "red_section",// apply this css class to the section
                HTML = "Forbidden", // inner html of the section
                Zones = new List<Zone>() { new Zone { Start = 2 * 60, End = 12 * 60 } },
                SpanType = DHXMarkTime.Type.BlockEvents//if specified - user can't create event in this area
            });
            sched.TimeSpans.Add(new DHXMarkTime()
            {
                Day = DayOfWeek.Saturday,
                CssClass = "green_section"
            });
            sched.TimeSpans.Add(new DHXMarkTime()
            {
                StartDate = new DateTime(2011, 9, 25),
                EndDate = new DateTime(2011, 9, 29),
                CssClass = "green_section"
            });
            
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            return View(sched);
        }

     

        /// <summary>
        /// Highligting pointed area
        /// </summary>
        /// <returns></returns>
        public ActionResult Highlight()
        {
            var sched = new DHXScheduler(this);

            sched.InitialDate = new DateTime(2011, 9, 19);

            sched.Highlighter.Enable("highlight_section");//use 'highlight_section' class for highlighted area
            
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            return View(sched);
        }

        /// <summary>
        /// Single click event creation
        /// </summary>
        /// <returns></returns>
        public ActionResult HighlightClickCreate()
        {
            var sched = new DHXScheduler(this);

            sched.InitialDate = new DateTime(2011, 9, 19);
            sched.Config.drag_create = false;



            //use 'highlight_section' class for highlighted area   
            //size of the area = 120 minutes
            sched.Highlighter.Enable("highlight_section", 120);
            sched.Highlighter.FixedStep = false;
            //create event on single click on highlighted area
            sched.Highlighter.CreateOnClick = true;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            return View(sched);
        }


        /// <summary>
        /// Custom event containers
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomEventBox()
        {
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;

            //helper for event templates,
            //it also can be defined on the client side
            var evBox = new DHXEventTemplate();

            evBox.CssClass = sched.Templates.event_class = "my_event";//class to be applied to event box
            // template will be rendered to the js function - function(ev, start, end){....}
            // where ev - is event object itself
            // template allows to inject js code within asp.net-like tags, and output properties of event with simplified sintax
            // {text} is equivalent to ' + ev.text + '
            evBox.Template =
                @"<div class='my_event_body'>
                    <% if((ev.end_date - ev.start_date) / 60000 > 60) { %>
                        <span class='event_date'>{start_date:date(%H:%i)} - {end_date:date(%H:%i)}</span><br/>
                    <% } else { %>
                        <span class='event_date'>{start_date:date(%H:%i)}</span>
                    <% } %>                  
                    <span>{text}</span>
                    <br>
                    <div style=""padding-top:5px;"">
                        Duration: <b><%= Math.ceil((ev.end_date - ev.start_date) / (60 * 60 * 1000)) %></b> hours
                    </div>
                  </div>";
            sched.Templates.EventBox = evBox;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            
            return View(sched);
        }

        public ActionResult IndexRazor()
        {
            return Index();
        }



        /// <summary>
        /// custom DataObject-to-JSON function
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="ev"></param>
        public void eventRenderer(System.Text.StringBuilder builder, object ev)
        {
            var item = ev as Event;
            builder.Append(
                string.Format("{{id:{0}, text:\"{1}\", start_date:\"{2:MM/dd/yyyy HH:mm}\", end_date:\"{3:MM/dd/yyyy HH:mm}\"}}",
                item.id,
                item.text.Replace("\"", "\\\""),//need to escape quotes and other characters, that may break json
                item.start_date,
                item.end_date));
        }
        /// <summary>
        /// Rendering data with custom function
        /// </summary>
        /// <returns></returns>
        public ContentResult CustomData()
        {
            var data = new SchedulerAjaxData((new DHXSchedulerDataContext()).Events);
            /*
             *  return data;
             *  is equal to 
             *  return Content(data.Render());
             *  SchedulerAjaxData.Render can also take Action<StringBuilder, object> as a parameter
             */
            return Content( data.Render(eventRenderer) );
        }

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData(new DHXSchedulerDataContext().Events);
            return data;
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            DHXSchedulerDataContext data = new DHXSchedulerDataContext();
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
                        //update all properties, except for id
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
