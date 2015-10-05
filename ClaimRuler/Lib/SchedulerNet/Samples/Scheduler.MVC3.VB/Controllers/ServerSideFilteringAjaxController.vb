Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Common


Namespace SchedulerTest.Controllers
	Public Class ServerSideFilteringAjaxController
		Inherits Controller
		'
		' GET: /ServerSideFilteringAjax/
		Public Class SchedulerFilterModel
			Public Property Scheduler() As DHXScheduler
				Get
					Return m_Scheduler
				End Get
				Set
					m_Scheduler = Value
				End Set
			End Property
			Private m_Scheduler As DHXScheduler
			Public Property Rooms() As IEnumerable(Of Room)
				Get
					Return m_Rooms
				End Get
				Set
					m_Rooms = Value
				End Set
			End Property
			Private m_Rooms As IEnumerable(Of Room)
			Public Sub New(sched As DHXScheduler, rooms As IEnumerable(Of Room))
				Me.Scheduler = sched
				Me.Rooms = rooms
			End Sub
		End Class
		Public Function Index(data As FormCollection) As ActionResult

            Dim sched = New DHXScheduler(Me)

			Dim rooms = New DHXSchedulerDataContext().Rooms.ToList()

			Dim unit = New UnitsView("rooms", "room_id")
			unit.Label = "Rooms"

			unit.AddOptions(rooms)
			sched.Views.Add(unit)

			sched.InitialView = unit.Name
			sched.InitialDate = New DateTime(2011, 9, 7)
			sched.LoadData = True
			sched.EnableDataprocessor = True

			Return View(New SchedulerFilterModel(sched, rooms))

		End Function

		Public Function Data() As ContentResult
			Dim dc = New DHXSchedulerDataContext()

			Dim dataset As IEnumerable(Of [Event])

			If Me.Request.QueryString("rooms") Is Nothing Then
				dataset = dc.Events
			Else
				Dim current_room = Integer.Parse(Me.Request.QueryString("rooms"))
                dataset = From ev In dc.Events Where ev.room_id = current_room
			End If

			Dim data__1 = New SchedulerAjaxData(dataset)


			Return (data__1)
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), actionValues), [Event])
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
