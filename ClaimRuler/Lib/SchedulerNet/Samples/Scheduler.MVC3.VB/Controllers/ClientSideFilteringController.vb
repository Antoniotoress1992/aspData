Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Configuration

Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Namespace SchedulerTest.Controllers
	Public Class ClientSideFilteringController
		Inherits Controller

		''' <summary>
		''' applying client side filters for each view
		''' </summary>
		''' <returns></returns>
		Public Function Index() As ActionResult
			Dim scheduler = New DHXScheduler()

			Dim rooms = New DHXSchedulerDataContext().Rooms.ToList()

			'each view can have multiple rules
			'they also can be added on the client
			'month
			scheduler.Views(1).Filter.Rules.Add(New Rule("start_date", [Operator].GreaterOrEqual, New DateTime(2011, 9, 6)))
			scheduler.Views(1).Filter.Rules.Add(New Rule("end_date", [Operator].LowerOrEqual, New DateTime(2011, 9, 14)))

			'day
			scheduler.Views(2).Filter.Rules.Add(New Rule("room_id", [Operator].Equals, rooms.First().key))
			'day
			scheduler.Views(2).Filter.Rules.Add(New ExpressionRule("{text}.length > 4 && {text}.length < 20"))



			Dim [select] = New LightboxSelect("room_id", "Room")
			[select].AddOptions(rooms)
			scheduler.Lightbox.AddDefaults()
			scheduler.Lightbox.Add([select])


			scheduler.Controller = "BasicScheduler"
			'using BasicSchedulerController to load data
			scheduler.LoadData = True
			scheduler.InitialDate = New DateTime(2011, 9, 7)
			Return View(scheduler)
		End Function

	End Class
End Namespace
