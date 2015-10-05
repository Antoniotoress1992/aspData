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
	''' using unobtrusive validation
	''' </summary>
	Public Class JQueryValidationController
		Inherits Controller
		Public Function Index() As ActionResult
            Dim sched = New DHXScheduler(Me)

			sched.LoadData = True
			sched.EnableDataprocessor = True

			sched.Lightbox.SetExternalLightbox("JQueryValidation/LightboxControl", 400, 140)

			sched.InitialDate = New DateTime(2011, 9, 5)
			Return View(sched)
		End Function



		Public Function LightboxControl(ev As ValidEvent) As ActionResult
			Dim context = New DHXSchedulerDataContext()
			Dim current = context.ValidEvents.SingleOrDefault(Function(e) e.id = ev.id)
			If current Is Nothing Then
				current = ev
			End If
			Return View(current)
		End Function

		Public Function Data() As ContentResult
			Return (New SchedulerAjaxData((New DHXSchedulerDataContext()).ValidEvents))
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ActionResult
			Dim action = New DataAction(actionValues)
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType(ValidEvent), actionValues), ValidEvent)
			If action.Type <> DataActionTypes.[Error] Then
				'handle changes done without lightbox - dnd, resize..
				Return NativeSave(action, changedEvent, actionValues)
			Else
				Return CustomFormSave(action, changedEvent, actionValues)
			End If



		End Function

		Public Function CustomFormSave(action As DataAction, changedEvent As ValidEvent, actionValues As FormCollection) As ContentResult


			If actionValues("actionType") IsNot Nothing Then
				Dim actionType = actionValues("actionType").ToLower()
				Dim data = New DHXSchedulerDataContext()
				Try
					If actionType = "save" Then

						If data.ValidEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId) IsNot Nothing Then
							'update event
							Dim eventToUpdate = data.ValidEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId)

							DHXEventsHelper.Update(eventToUpdate, changedEvent, New List(Of String)() From { _
								"id" _
							})

							action.Type = DataActionTypes.Update
						Else
							'create event                           
							data.ValidEvents.InsertOnSubmit(changedEvent)
							action.Type = DataActionTypes.Insert
						End If
					ElseIf actionType = "delete" Then

						changedEvent = data.ValidEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						data.ValidEvents.DeleteOnSubmit(changedEvent)

						action.Type = DataActionTypes.Delete
					End If
					data.SubmitChanges()

				Catch e As Exception
					action.Type = DataActionTypes.[Error]
				End Try
			End If



			Return (New SchedulerFormResponseScript(action, changedEvent))
		End Function

		Public Function NativeSave(action As DataAction, changedEvent As ValidEvent, actionValues As FormCollection) As ContentResult

			Dim data = New DHXSchedulerDataContext()
			Try
				Select Case action.Type
					Case DataActionTypes.Insert
						data.ValidEvents.InsertOnSubmit(changedEvent)
						Exit Select
					Case DataActionTypes.Delete
						changedEvent = data.ValidEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						data.ValidEvents.DeleteOnSubmit(changedEvent)
						Exit Select
					Case Else
						' "update"                          
						Dim eventToUpdate = data.ValidEvents.SingleOrDefault(Function(ev) ev.id = action.SourceId)
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
