Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Data

Imports System.Text
Namespace SchedulerTest.Controllers

	''' <summary>
	''' Exporting events to iCal format
	''' </summary>
	Public Class ExportToICalController
		Inherits BasicSchedulerController

		Public Overrides Function Index() As ActionResult
			Dim sched = New DHXScheduler()

			sched.Controller = "BasicScheduler"
			sched.Config.first_hour = 8

			' 'Serialize' extension is required
			sched.Extensions.Add(SchedulerExtensions.Extension.Serialize)
			sched.InitialDate = New DateTime(2011, 9, 19)

			Return View(sched)
		End Function

		''' <summary>
		''' client-side serialization - client sends data in ical format, see Views/ExportToICal/Index.aspx
		''' </summary>
		''' <returns></returns>
		Public Function Export() As ActionResult

			Response.ContentType = "text/plain"
			Response.AppendHeader("content-disposition", "attachment; filename=dhtmlxScheduler.ics")
			Return Content(Request.Form("data").ToString())
		End Function


		''' <summary>
		''' Serializing data
		''' </summary>
		''' <returns></returns>
		Public Function ExportServerSide() As ActionResult
			Response.ContentType = "text/plain"
			Response.AppendHeader("content-disposition", "attachment; filename=dhtmlxScheduler.ics")

			Dim renderer = New ICalRenderer()
			Dim events = New DHXSchedulerDataContext().Events

			Return Content(renderer.ToICal(events))
			'you can also use custom function for rendering of the events
			'renderer.ToICal(events, RenderItem);
		End Function


		Public Sub RenderItem(builder As StringBuilder, item As Object)
			Dim ev = TryCast(item, [Event])
			builder.AppendLine("BEGIN:VEVENT")
			builder.AppendLine(String.Format("DTSTART:{0:yyyyMMddTHmmss}", ev.start_date))
			builder.AppendLine(String.Format("DTEND:{0:yyyyMMddTHmmss}", ev.end_date))
			builder.AppendLine(String.Format("SUMMARY:{0}", ev.text))
			builder.AppendLine("END:VEVENT")
		End Sub

	End Class
End Namespace
