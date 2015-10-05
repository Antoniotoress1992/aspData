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
	''' use server-side form instead of default details form
	''' </summary>
	Public Class MVCFormInLightboxController
		Inherits Controller
		Public Function Index() As ActionResult
            Dim sched = New DHXScheduler(Me)

			sched.LoadData = True
			sched.EnableDataprocessor = True

			'view, width, height
			Dim box = sched.Lightbox.SetExternalLightbox("MVCFormInLightbox/LightboxControl", 420, 195)
			'css class to be applied to the form
			box.ClassName = "custom_lightbox"
			sched.InitialDate = New DateTime(2011, 9, 5)
			Return View(sched)
		End Function



		Public Function LightboxControl(ev As [Event]) As ActionResult
			Dim context = New DHXSchedulerDataContext()
			Dim current = context.Events.SingleOrDefault(Function(e) e.id = ev.id)
			If current Is Nothing Then
				current = ev
			End If
			Return View(current)
		End Function

		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)

			Return (data__1)
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ActionResult
			Dim action = New DataAction(actionValues)
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), actionValues), [Event])
			If action.Type <> DataActionTypes.[Error] Then
				'process resize, d'n'd operations...
				Return NativeSave(changedEvent, actionValues)
			Else
				'custom form operation
				Return CustomSave(changedEvent, actionValues)
			End If

		End Function

		Public Function CustomSave(changedEvent As [Event], actionValues As FormCollection) As ActionResult

			Dim action = New DataAction(DataActionTypes.Update, changedEvent.id, changedEvent.id)
			If actionValues("actionButton") IsNot Nothing Then
				Dim data As New DHXSchedulerDataContext()
				Try
					If actionValues("actionButton") = "Save" Then

						If data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId) IsNot Nothing Then
							Dim eventToUpdate = data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId)
							DHXEventsHelper.Update(eventToUpdate, changedEvent, New List(Of String)() From { _
								"id" _
							})
						Else
							action.Type = DataActionTypes.Insert
							data.Events.InsertOnSubmit(changedEvent)
						End If
					ElseIf actionValues("actionButton") = "Delete" Then
						action.Type = DataActionTypes.Delete
						changedEvent = data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						data.Events.DeleteOnSubmit(changedEvent)
					End If
					data.SubmitChanges()

				Catch e As Exception
					action.Type = DataActionTypes.[Error]
				End Try
			Else
				action.Type = DataActionTypes.[Error]
			End If


			Return (New SchedulerFormResponseScript(action, changedEvent))

		End Function

		Public Function NativeSave(changedEvent As [Event], actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)

			Dim data As New DHXSchedulerDataContext()
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
