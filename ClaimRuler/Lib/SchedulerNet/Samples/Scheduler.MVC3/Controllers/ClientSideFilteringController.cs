using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SchedulerTest.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
namespace SchedulerTest.Controllers
{
    public class ClientSideFilteringController : Controller
    {
        
        /// <summary>
        /// applying client side filters for each view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var scheduler = new DHXScheduler();
 
            var rooms = new DHXSchedulerDataContext().Rooms.ToList();

            //each view can have multiple rules
            //they also can be added on the client
            scheduler.Views[1].Filter.Rules.Add(//month
                new Rule("start_date", Operator.GreaterOrEqual, new DateTime(2011, 9, 6))
            );
            scheduler.Views[1].Filter.Rules.Add(
                new Rule("end_date", Operator.LowerOrEqual, new DateTime(2011, 9, 14))
            );
           
            scheduler.Views[2].Filter.Rules.Add(//day
                new Rule("room_id", Operator.Equals, rooms.First().key)
            );
            scheduler.Views[2].Filter.Rules.Add(//day
                new ExpressionRule("{text}.length > 4 && {text}.length < 20")
            );

        
            
            var select = new LightboxSelect("room_id", "Room");
            select.AddOptions(rooms);
            scheduler.Lightbox.AddDefaults();
            scheduler.Lightbox.Add(select);


            scheduler.Controller = "BasicScheduler";//using BasicSchedulerController to load data
            scheduler.LoadData = true;
            scheduler.InitialDate = new DateTime(2011, 9, 7);
            return View(scheduler);
        }

    }
}
