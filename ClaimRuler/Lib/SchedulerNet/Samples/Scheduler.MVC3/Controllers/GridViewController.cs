using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using SchedulerTest.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;

namespace SchedulerTest.Controllers
{
    /// <summary>
    /// using 'table' view
    /// </summary>
    public class GridViewController : Controller
    {

        public ActionResult Index()
        {
            var scheduler = new DHXScheduler(this);

            var grid = new GridView("grid");

            //adding the columns
            grid.Columns.Add(new GridViewColumn("text", "Event")//data property, label
            {
                Width = 300
            });

            grid.Columns.Add(new GridViewColumn("start_date", "Date")
            {   //can assign template for column contents
                // for more info about templates syntax see http://scheduler-net.com/docs/dhxscheduler.templates.html
                Template = @"<% if((ev.end_date - ev.start_date)/1440000 > 1){%>
                                {start_date:date(%d %M %Y)} - {end_date:date(%d %M %Y)} 
                            <% }else{ %> 
                                {start_date:date(%d %M %Y %H:%i)} 
                            <% } %>",
                Align = GridViewColumn.Aligns.Left,
                Width = 200
            });



            grid.Columns.Add(new GridViewColumn("details", "Details")
            {
                Align = GridViewColumn.Aligns.Left
            });

            scheduler.Views.Add(grid);

            scheduler.Lightbox.Add(new LightboxText("text", "Text"));
            scheduler.Lightbox.Add(new LightboxText("details", "Details"));
            scheduler.Lightbox.Add(new LightboxTime("time"));

            scheduler.InitialView = grid.Name;
            scheduler.InitialDate = new DateTime(2011, 9, 19);
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;
            return View(scheduler);
        }

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData((new CustomFieldsDataContext().Grids));
           
            return data;
        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            CustomFieldsDataContext data = new CustomFieldsDataContext();
            try
            {
                var changedEvent = DHXEventsHelper.Bind<Grid>(actionValues);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        data.Grids.InsertOnSubmit(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = data.Grids.SingleOrDefault(ev => ev.id == action.SourceId);
                        data.Grids.DeleteOnSubmit(changedEvent);
                        break;
                    default:// "update"                          
                        var eventToUpdate = data.Grids.SingleOrDefault(ev => ev.id == action.SourceId);
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
