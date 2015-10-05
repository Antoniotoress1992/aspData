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
	Public Class CustomFieldController
		Inherits Controller

		Public Function Index() As ActionResult

			Dim sched = New DHXScheduler(Me)


			sched.Lightbox.Add(New LightboxText("text", "Description"))

			Dim check = New LightboxCheckbox("highlighting", "Important")
			check.MapTo = "textColor"
			'checkbox will set value of 'textColor' property of the event
			check.CheckedValue = "red"

			sched.Lightbox.Add(check)

			sched.LoadData = True
			sched.EnableDataprocessor = True

			'allows to postback changes from the server
			sched.UpdateFieldsAfterSave()

			sched.InitialDate = New DateTime(2011, 9, 5)
			Return View(sched)
		End Function


		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New CustomFieldsDataContext()).ColoredEvents)

			Return (data__1)
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType(ColoredEvent), actionValues), ColoredEvent)
			Dim color = ""
			If actionValues("textColor") = "red" Then
				color = "red"
			Else
				If changedEvent.start_date < DateTime.Now Then
					color = "gray"
				Else
					color = "blue"
				End If
			End If

			Dim data As New CustomFieldsDataContext()
			Try
				Select Case action.Type
					Case DataActionTypes.Insert
						changedEvent.textColor = color
						data.ColoredEvents.InsertOnSubmit(changedEvent)
						Exit Select
					Case DataActionTypes.Delete
						changedEvent = data.ColoredEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						data.ColoredEvents.DeleteOnSubmit(changedEvent)
						Exit Select
					Case Else
						' "update"                          
						Dim eventToUpdate = data.ColoredEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						DHXEventsHelper.Update(eventToUpdate, changedEvent, New List(Of String)() From { _
							"id" _
						})

						changedEvent.textColor = color


						Exit Select
				End Select
				data.SubmitChanges()
				action.TargetId = changedEvent.id
			Catch a As Exception
				action.Type = DataActionTypes.[Error]
			End Try

			Dim result = New AjaxSaveResponse(action)
			result.UpdateField("textColor", color)
			'property will be updated on the client
			Return result
		End Function

	End Class
End Namespace
