
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
	Public Class DifferentModesController
		Inherits Controller

		''' <summary>
		''' Week agenda and year view
		''' </summary>
		''' <returns></returns>
		Public Function YearNWeek() As ActionResult
			Dim scheduler = New DHXScheduler(Me)

			scheduler.Views.Clear()
			scheduler.Views.Add(New WeekView())

			Dim year = New YearView()

			scheduler.Views.Add(year)
			scheduler.InitialView = year.Name
			scheduler.LoadData = True
			scheduler.EnableDataprocessor = True
			Return View(scheduler)
		End Function

		''' <summary>
		''' Google map in calendar
		''' </summary>
		''' <returns></returns>
		Public Function GoogleMap() As ActionResult
			Dim scheduler = New DHXScheduler(Me)

			scheduler.Views.Clear()
			scheduler.Views.Add(New WeekView())
			scheduler.Views.Add(New MapView())
			scheduler.InitialView = (New MapView()).Name
			scheduler.LoadData = True
			'scheduler.EnableDataprocessor = true;
			scheduler.DataAction = "MapEvents"
			Return View(scheduler)
		End Function


		''' <summary>
		''' Add resources views - units and timeline
		''' </summary>
		''' <returns></returns>
		Public Function MultipleResources() As ActionResult
			Dim scheduler = New DHXScheduler(Me)

			Dim rooms = New DHXSchedulerDataContext().Rooms.ToList()

			scheduler.Views.Clear()
			scheduler.Views.Add(New MonthView())

			Dim unit = New UnitsView("unit1", "room_id")
			unit.AddOptions(rooms)
			'parse model objects
			scheduler.Views.Add(unit)

			Dim timeline = New TimelineView("timeline", "room_id")
			timeline.RenderMode = TimelineView.RenderModes.Bar
			timeline.FitEvents = False
			timeline.AddOptions(rooms)
			scheduler.Views.Add(timeline)

			'show timeline view by default
			scheduler.InitialView = timeline.Name

			scheduler.Lightbox.AddDefaults()
			'add default set of options - text and timepicker
			'add controls to the details form
			Dim [select] = New LightboxSelect("room_id", "Room id")
			[select].AddOptions(rooms)
			scheduler.Lightbox.Add([select])


			scheduler.LoadData = True
			scheduler.InitialDate = New DateTime(2011, 9, 7)
			scheduler.EnableDataprocessor = True

			Return View(scheduler)
		End Function


		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)

			Return data__1
		End Function

		Public Function OtherData() As ContentResult
			Dim data = New SchedulerAjaxData(New CustomFieldsDataContext().ColoredEvents)

			Return data
		End Function
		Public Function MapEvents() As ContentResult
			Dim today = DateTime.Today

            Dim data = New SchedulerAjaxData(New List(Of Object)() From { _
             New With { _
              .id = 2, _
              .text = "Kurtzenhouse", _
              .start_date = today.AddDays(1).AddHours(13), _
              .end_date = today.AddDays(1).AddHours(16), _
              .lat = 48.7396839, _
              .lng = 7.81336809999993, _
              .event_location = "D37, 67240 Kurtzenhouse, France" _
             }, _
             New With { _
              .id = 3, _
              .text = "Forêt Domaniale", _
              .start_date = today.AddDays(2).AddHours(10), _
              .end_date = today.AddDays(2).AddHours(12), _
              .lat = 48.767333, _
              .lng = 5.79325800000004, _
              .event_location = "Forêt Domaniale de la Reine, Véry, 54200 Royaumeix, France" _
             }, _
             New With { _
              .id = 4, _
              .text = "Windstein", _
              .start_date = today.AddDays(3).AddHours(7), _
              .end_date = today.AddDays(3).AddHours(8), _
              .lat = 49.0003477, _
              .lng = 7.68730649999998, _
              .event_location = "1 Rue du Nagelsthal, 67110 Windstein, France" _
             } _
            })


			Return data
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
