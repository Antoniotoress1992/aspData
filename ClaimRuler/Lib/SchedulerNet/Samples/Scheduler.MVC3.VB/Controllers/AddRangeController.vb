Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc


Imports System.Web.Security

Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Common
Imports DHTMLX.Scheduler.Authentication

Namespace SchedulerTest.Controllers
	Public Class AddRangeController
		Inherits Controller
		Public Function Details() As ActionResult
            Dim sched = New DHXScheduler(Me)

			sched.XY.scroll_width = 0
			sched.Config.first_hour = 8
			sched.Config.last_hour = 19
			sched.Config.time_step = 30
			sched.Config.limit_time_select = True


			Dim text = New LightboxText("text", "Description")
			text.Height = 50
			sched.Lightbox.Add(text)
			Dim [select] = New LightboxSelect("textColor", "Priority")
			Dim items = New List(Of Object)()
            items.Add(New With { _
             .key = "gray", _
             .label = "Low" _
            })
            items.Add(New With { _
             .key = "blue", _
             .label = "Medium" _
            })
            items.Add(New With { _
             .key = "red", _
             .label = "Hight" _
            })
			[select].AddOptions(items)
			Dim check = New LightboxRadio("category", "Category")
			check.AddOption(New LightboxSelectOption("job", "Job"))
			check.AddOption(New LightboxSelectOption("family", "Family"))
			check.AddOption(New LightboxSelectOption("other", "Other"))
			sched.Lightbox.Add(check)
			sched.Lightbox.Add([select])
			sched.Lightbox.Add(New LightboxMiniCalendar("time", "Time:"))

			sched.Lightbox.Add(New LightboxCheckbox("remind", "Remind"))
			sched.LoadData = True
			sched.EnableDataprocessor = True
			sched.InitialDate = New DateTime(2011, 9, 11)

			Return View(sched)
		End Function

		Public Function Wide() As ActionResult
            Dim sched = New DHXScheduler(Me)
            sched.Skin = DHXScheduler.Skins.Glossy
			sched.Config.wide_form = True


			sched.XY.scroll_width = 0
			sched.Config.first_hour = 8
			sched.Config.last_hour = 19
			sched.Config.time_step = 30
			sched.Config.limit_time_select = True


			Dim text = New LightboxText("text", "Description")
			text.Height = 50
			sched.Lightbox.Add(text)
			Dim [select] = New LightboxSelect("textColor", "Priority")
			Dim items = New List(Of Object)()
            items.Add(New With { _
             .key = "gray", _
             .label = "Low" _
            })
            items.Add(New With { _
             .key = "blue", _
             .label = "Medium" _
            })
            items.Add(New With { _
             .key = "red", _
             .label = "Hight" _
            })
			[select].AddOptions(items)
			Dim check = New LightboxRadio("category", "Category")
			check.AddOption(New LightboxSelectOption("job", "Job"))
			check.AddOption(New LightboxSelectOption("family", "Family"))
			check.AddOption(New LightboxSelectOption("other", "Other"))
			sched.Lightbox.Add(check)
			sched.Lightbox.Add([select])
			sched.Lightbox.Add(New LightboxMiniCalendar("time", "Time:"))

			sched.Lightbox.Add(New LightboxCheckbox("remind", "Remind"))
			sched.LoadData = True
			sched.EnableDataprocessor = True
			sched.InitialDate = New DateTime(2011, 9, 11)

			Return View(sched)
		End Function



		Public Function Index() As ActionResult


            Dim sched = New DHXScheduler(Me)

			Dim unit = New UnitsView("unit1", "room_id")
			sched.Views.Add(unit)


			Dim context = New DHXSchedulerDataContext()

			'can add IEnumerable of objects, native units or dictionary
			unit.AddOptions(context.Rooms)
			'parse model objects
			sched.Config.details_on_create = True
			sched.Config.details_on_dblclick = True

			Dim timeline = New TimelineView("timeline", "room_id")

			Dim items = New List(Of Object)()
			timeline.FitEvents = False
			sched.Views.Add(timeline)
			timeline.AddOptions(context.Rooms)


			Dim [select] = New LightboxSelect("textColor", "Priority")
			items = New List(Of Object)()
            items.Add(New With { _
             .key = "gray", _
             .label = "Low" _
            })
            items.Add(New With { _
             .key = "blue", _
             .label = "Medium" _
            })
            items.Add(New With { _
             .key = "red", _
             .label = "Hight" _
            })
			[select].AddOptions(items)
			sched.Lightbox.Add([select])


			[select] = New LightboxSelect("room_id", "Room id")
			[select].AddOptions(context.Rooms)
			sched.Lightbox.Add([select])

			sched.LoadData = True
			sched.EnableDataprocessor = True
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
			Dim data As New CustomFieldsDataContext()
			Try
				Select Case action.Type
					Case DataActionTypes.Insert
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
