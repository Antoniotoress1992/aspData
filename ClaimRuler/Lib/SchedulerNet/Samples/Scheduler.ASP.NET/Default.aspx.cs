using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DHTMLX.Scheduler;


namespace SchedulerNetAsp
{
    public partial class _Default : System.Web.UI.Page
    {

        public DHXScheduler Scheduler { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Scheduler = new DHXScheduler();
            Scheduler.InitialDate = new DateTime(2011, 11, 24);

            
            Scheduler.Config.first_hour = 8;
            Scheduler.Config.last_hour = 19;
            Scheduler.Config.time_step = 30;
            Scheduler.Config.limit_time_select = true;

            Scheduler.DataAction = this.ResolveUrl("~/Data.ashx");
            Scheduler.SaveAction = this.ResolveUrl("~/Save.ashx");
            Scheduler.LoadData = true;
            Scheduler.EnableDataprocessor = true;
           
        }
    }

}
