using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Scheduler.Controls;

using DHTMLX.Common;
using DHTMLX.Helpers;

using CRM.Data.Account;

namespace CRM.Web.Utilities {
	static public class SchedulerHelper {

		
		static public void BuildGrid(DHXScheduler scheduler) {
		
			var grid = new DHTMLX.Scheduler.Controls.GridView("grid");//initializes the view
			
			

			// adds the columns to the grid
			grid.Columns.Add(
			   new GridViewColumn("start_date", "Date") {
				   Template = "{start_date:date(%m-%d-%Y %H:%i)}"//sets the template for the column
			   });

			grid.Columns.Add(
					new GridViewColumn("text", "Event") {	//initializes a column  
						Width = 200					// sets the width of the column
						 
					});

			grid.Columns.Add(
			    new GridViewColumn("details", "Details") {
				    Align = GridViewColumn.Aligns.Left,	// sets the alignment in the colum
				    Width = 200
			    });

			grid.Columns.Add(
			    new GridViewColumn("owner_name", "User Name") {
				    Align = GridViewColumn.Aligns.Center// sets the alignment in the colum
			    });

			grid.Columns.Add(
			    new GridViewColumn("lead_name", "Lead/Claim") {
				    Align = GridViewColumn.Aligns.Center// sets the alignment in the colum
			    });

			grid.Columns.Add(
			    new GridViewColumn("statusName", "Status") {
				    Align = GridViewColumn.Aligns.Center// sets the alignment in the colum
			    });
			

			scheduler.Views.Clear();

			scheduler.Views.Add(new WeekView());
			scheduler.Views.Add(new DayView());
			scheduler.Views.Add(new MonthView());

			grid.Label = "Agenda";

			grid.Select = true;
			grid.Paging = true;

			scheduler.Views.Add(grid);//adds the view to the scheduler
			//scheduler.Views[1].Label = "Agenda";
			scheduler.InitialView = "grid";

		}

		static public void Initialize(DHXScheduler scheduler) {
			int roleID = 0;
			int userID = 0;

			int.TryParse(System.Web.HttpContext.Current.Session["RoleId"].ToString(), out roleID);

			scheduler.Codebase = "../../js/dhtmlxScheduler";

			//scheduler.DataFormat = SchedulerDataLoader.DataFormats.iCal;

			scheduler.Config.first_hour = 5;//sets the minimum value for the hour scale (Y-Axis)
			scheduler.Config.last_hour = 19;//sets the maximum value for the hour scale (Y-Axis)
			scheduler.Config.time_step = 60;//sets the scale interval for the time selector in the lightbox. 
			//scheduler.Config.full_day = true;// blocks entry fields in the 'Time period' section of the lightbox and sets time period to a full day from 00.00 the current cell date untill 00.00 next day. 
			scheduler.Config.multi_day = true;
			scheduler.Config.limit_time_select = true;

			scheduler.Skin = DHXScheduler.Skins.Terrace;

			scheduler.Extensions.Add(SchedulerExtensions.Extension.Collision);
			scheduler.Extensions.Add(SchedulerExtensions.Extension.Limit);

			

			#region lightbox configuration
			var text = new LightboxText("text", "Task");// initializes a text input with the label 'Task'
			text.Height = 20;// sets the height of the control
			text.Focus = true;// set focus to the control
			scheduler.Lightbox.Add(text);// adds the control to the lightbox

			var description = new LightboxText("details", "Details");// initializes a text input with the label 'Task'
			description.Height = 20;
			scheduler.Lightbox.Add(description);

			var status = new LightboxSelect("status_id", "Status");// initializes a dropdown list with the label 'Status'
			status.AddOptions(TasksManager.GetStatuses());// populates the list with values from the 'Statuses' table
			scheduler.Lightbox.Add(status);

			var owners = new LightboxSelect("owner_id", "Owner");

			if (roleID == (int)UserRole.Administrator)
				owners.AddOptions(TasksManager.GetOwners());
			else {
				int.TryParse(System.Web.HttpContext.Current.Session["UserId"].ToString(), out userID);

				owners.AddOptions(TasksManager.GetOwners(userID));
			}

			scheduler.Lightbox.Add(owners);
			

			// poplicy types
			var policyTypes = new LightboxSelect("policy_type", "Policy Type");
			policyTypes.AddOptions(LeadPolicyTypeManager.GetPolicyTypesForScheduler());
			scheduler.Lightbox.Add(policyTypes);

			//var lead_name = new LightboxText("lead_name", "Lead Name");// initializes a text input with the label 'Task'
			//lead_name.Height = 20;
			//scheduler.Lightbox.Add(lead_name);

			// initializes and adds a control area for setting start and end times of a task
			//scheduler.Lightbox.Add(new LightboxTime("Time"));
			scheduler.Lightbox.Add(new LightboxMiniCalendar("cal", "Time"));
			#endregion

			scheduler.LoadData = true;
			scheduler.EnableDataprocessor = true;
			scheduler.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Week);
			scheduler.UpdateFieldsAfterSave();			
		}
	}
}