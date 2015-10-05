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
	''' Recurring events support
	''' </summary>
	Public Class RecurringEventsController
		Inherits Controller

		Public Function Index() As ActionResult
            Dim scheduler = New DHXScheduler(Me)

			scheduler.InitialDate = New DateTime(2011, 10, 1)
			scheduler.Extensions.Add(SchedulerExtensions.Extension.Recurring)
			scheduler.LoadData = True
			scheduler.EnableDataprocessor = True


			Return View(scheduler)
		End Function
		Public Function Data() As ActionResult
			'recurring events have 3 additional mandatory properties
			' string rec_type
			' Nullable<long> event_length
			' Nullable<int> event_pid
			Return New SchedulerAjaxData(New DHXSchedulerDataContext().Recurrings)
		End Function




		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ActionResult
			Dim action = New DataAction(actionValues)

			Dim data As New DHXSchedulerDataContext()
			Try
				Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType(Recurring), actionValues), Recurring)
				'operations with recurring events require some additional handling
				Dim isFinished As Boolean = deleteRelated(action, changedEvent, data)
				If Not isFinished Then
					Select Case action.Type

						Case DataActionTypes.Insert
							data.Recurrings.InsertOnSubmit(changedEvent)
							If changedEvent.rec_type = "none" Then
								'delete one event from the serie
								action.Type = DataActionTypes.Delete
							End If
							Exit Select
						Case DataActionTypes.Delete
							changedEvent = data.Recurrings.SingleOrDefault(Function(ev) ev.id = action.SourceId)
							data.Recurrings.DeleteOnSubmit(changedEvent)
							Exit Select
						Case Else
							' "update"   
							Dim eventToUpdate = data.Recurrings.SingleOrDefault(Function(ev) ev.id = action.SourceId)
							DHXEventsHelper.Update(eventToUpdate, changedEvent, New List(Of String)() From { _
								"id" _
							})
							Exit Select
					End Select
				End If
				data.SubmitChanges()


				action.TargetId = changedEvent.id
			Catch a As Exception
				action.Type = DataActionTypes.[Error]
			End Try

			Return (New AjaxSaveResponse(action))
		End Function
		Protected Function deleteRelated(action As DataAction, changedEvent As Recurring, context As DHXSchedulerDataContext) As Boolean
			Dim finished As Boolean = False
			If (action.Type = DataActionTypes.Delete OrElse action.Type = DataActionTypes.Update) AndAlso Not String.IsNullOrEmpty(changedEvent.rec_type) Then
                context.Recurrings.DeleteAllOnSubmit(From ev In context.Recurrings Where ev.event_pid = changedEvent.id)
			End If
			If action.Type = DataActionTypes.Delete AndAlso (changedEvent.event_pid <> 0 AndAlso changedEvent.event_pid IsNot Nothing) Then
                Dim changed As Recurring = (From ev In context.Recurrings Where ev.id = action.TargetId).[Single]()
				changed.rec_type = "none"
				finished = True
			End If
			Return finished
		End Function
	End Class
End Namespace
