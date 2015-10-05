Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc


Imports System.Web.Security

Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Common

Namespace SchedulerTest.Controllers
	''' <summary>
	''' Use custom form instead of native details form.
	''' 
	''' </summary>
	Public Class PartialViewInLightboxController
		Inherits Controller

		Public Function Index() As ActionResult

			Dim sched = New DHXScheduler(Me)


			sched.LoadData = True
			sched.EnableDataprocessor = True
			sched.Views.Add(New AgendaView())

			'Need to implement setValue/getValue interface, and form will be fully integrated into the scheduler
			Dim box = sched.Lightbox.SetExternalLightboxForm("PartialViewInLightbox/Form", 500, 150)
			box.ClassName = "custom_lightbox"
			sched.InitialDate = New DateTime(2011, 9, 5)
			Return View(sched)
		End Function

		Public Function Form() As ActionResult
			Return View()
		End Function
		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)

			Return (data__1)
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)

			Dim data As New DHXSchedulerDataContext()
			Try
				Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), actionValues), [Event])
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
