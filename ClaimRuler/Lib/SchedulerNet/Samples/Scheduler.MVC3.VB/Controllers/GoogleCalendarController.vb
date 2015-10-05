Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DHTMLX.Scheduler.GoogleCalendar
Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Common

Namespace SchedulerTest.Controllers
	Public Class GoogleCalendarController
		Inherits Controller
		'
		' GET: /GoogleCalendar/

		Public Function Index() As ActionResult
            Dim sched = New DHXScheduler()
          
			sched.InitialDate = New DateTime(2011, 7, 19)
			sched.LoadData = True

			sched.InitialView = sched.Views(0).Name
			sched.Config.isReadonly = True
			sched.DataFormat = SchedulerDataLoader.DataFormats.iCal
            sched.DataAction = Url.Action("Data", "GoogleCalendar")
			Return View(sched)
		End Function

		Public Function MixedContent() As ActionResult
            Dim sched = New DHXScheduler(Me)
           
			sched.InitialDate = New DateTime(2011, 7, 19)
			sched.LoadData = True
			sched.InitialView = sched.Views(0).Name
			sched.DataAction = "Mixed"
			Return View(sched)
		End Function
		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData()

			Return Content(data__1.FromUrl("https://www.google.com/calendar/ical/b0prga519c0g0t3crcnc0g9in0@group.calendar.google.com/public/basic.ics"))

		End Function
		Public Function Mixed() As ContentResult
			'   var helper = new 
			'    var helper = new ICalHelper();
			'    var events = helper.GetFromFeed("https://www.google.com/calendar/ical/b0prga519c0g0t3crcnc0g9in0@group.calendar.google.com/public/basic.ics");

			Dim data = New SchedulerAjaxData()
			data.FromICal("https://www.google.com/calendar/ical/b0prga519c0g0t3crcnc0g9in0@group.calendar.google.com/public/basic.ics")
			data.Add(New DHXSchedulerDataContext().Events)
			Return data
		End Function
		Public Function ExportToGoogleCalendar() As ActionResult
			Dim help = New GoogleCalendarHelper("login", "password")
			help.ExportToGoogleCalendar("https://www.google.com/calendar/feeds/default/private/full", New DHXSchedulerDataContext().Events)


			Return Content("")
		End Function
	End Class
End Namespace
