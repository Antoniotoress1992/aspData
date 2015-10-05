Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc


Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Common

Namespace SchedulerTest.Controllers
	''' <summary>
	''' It's not necessary to load data with ajax,
	''' it can be rendered initially.
	''' </summary>
	Public Class StaticLoadingController
		Inherits Controller
		Public Function Index() As ActionResult

			Dim sched = New DHXScheduler(Me)


			Dim unit = New UnitsView("unit1", "room_id")


			sched.Views.Add(unit)
			Dim context = New DHXSchedulerDataContext()
			unit.AddOptions(context.Rooms.ToList())

			sched.Data.Parse(context.Events)
			sched.EnableDataprocessor = True
			sched.InitialDate = New DateTime(2011, 9, 5)

			Return View(sched)
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)

			Dim data As New DHXSchedulerDataContext()
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), actionValues), [Event])
			Try
				Select Case action.Type
					Case DataActionTypes.Insert
						data.Events.InsertOnSubmit(changedEvent)
						Exit Select
					Case DataActionTypes.Delete
						changedEvent = data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						data.Events.DeleteOnSubmit(changedEvent)
						Exit Select
					Case Else
						' "update"                          
						Dim eventToUpdate = data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						DHXEventsHelper.Update(eventToUpdate, changedEvent, New List(Of String)() From { _
							"id" _
						})
						Exit Select
				End Select
				data.SubmitChanges()
				action.TargetId = changedEvent.id
			Catch a As Exception
				action.Type = DataActionTypes.[Error]
			End Try

			Return (New AjaxSaveResponse(action))
		End Function

	End Class
End Namespace
