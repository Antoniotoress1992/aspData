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
	Public Class BasicSchedulerController
		Inherits Controller
		''' <summary>
		''' Default initialization
		''' </summary>
		''' <returns></returns>
		Public Overridable Function Index() As ActionResult

            Dim sched = New DHXScheduler(Me)
           

			sched.InitialDate = New DateTime(2011, 9, 19)
			'load data initially
			sched.LoadData = True

			'save client-side changes
			sched.EnableDataprocessor = True

			Return View(sched)
		End Function

		''' <summary>
		''' Block time areas
		''' </summary>
		''' <returns></returns>
		Public Function Limit() As ActionResult
            Dim sched = New DHXScheduler(Me)
          
			sched.InitialDate = New DateTime(2011, 9, 19)


			' block specific date range
            sched.TimeSpans.Add(New DHXBlockTime() With { _
             .StartDate = New DateTime(2011, 9, 14), _
             .EndDate = New DateTime(2011, 9, 17) _
            })

			' block each tuesday from 12AM to 15PM 
            sched.TimeSpans.Add(New DHXBlockTime() With { _
             .Day = DayOfWeek.Tuesday, _
             .Zones = New List(Of Zone)() From { _
              New Zone() With { _
               .Start = 0, _
               .[End] = 15 * 60 _
              } _
             } _
            })

			' block each sunday
            sched.TimeSpans.Add(New DHXBlockTime() With { _
             .Day = DayOfWeek.Sunday _
            })

			' block each monday from 12am to 8am, and from 18pm to 12am of the next day
            sched.TimeSpans.Add(New DHXBlockTime() With { _
             .Day = DayOfWeek.Monday, _
             .Zones = New List(Of Zone)() From { _
              New Zone() With { _
               .Start = 0, _
               .[End] = 8 * 60 _
              }, _
              New Zone() With { _
               .Start = 18 * 60, _
               .[End] = 24 * 60 _
              } _
             } _
            })



            Dim dc As New DHXSchedulerDataContext()

            Dim rooms = dc.Rooms.ToList()
            Dim unit As New UnitsView("unit", "room_id")
            unit.AddOptions(rooms) ''parse model objects
            sched.Views.Add(unit)

            Dim timeline As New TimelineView("timeline", "room_id")
            timeline.RenderMode = TimelineView.RenderModes.Bar
            timeline.FitEvents = False
            timeline.AddOptions(rooms)
            sched.Views.Add(timeline)

            ''block different sections in timeline and units view
            sched.TimeSpans.Add(New DHXBlockTime() With _
            {
                .FullWeek = True,
                .Sections = New List(Of Section)() From _
                {
                    New Section(unit.Name, rooms.Take(2).Select(Function(r) r.key.ToString()).ToArray()),
                    New Section(timeline.Name, rooms.Skip(2).Select(Function(r) r.key.ToString()).ToArray())
                }
            })

			sched.LoadData = True
			sched.EnableDataprocessor = True
			Return View(sched)

		End Function

		''' <summary>
		''' Colors for time areas
		''' </summary>
		''' <returns></returns>
		Public Function MarkedTimeSpans() As ActionResult
            Dim sched = New DHXScheduler(Me)
            
			sched.InitialDate = New DateTime(2011, 9, 19)


				' apply this css class to the section
				' inner html of the section
				'if specified - user can't create event in this area
            sched.TimeSpans.Add(New DHXMarkTime() With { _
             .Day = DayOfWeek.Thursday, _
             .CssClass = "red_section", _
             .HTML = "Forbidden", _
             .Zones = New List(Of Zone)() From { _
              New Zone() With { _
               .Start = 2 * 60, _
               .[End] = 12 * 60 _
              } _
             }, _
             .SpanType = DHXMarkTime.Type.BlockEvents _
            })
            sched.TimeSpans.Add(New DHXMarkTime() With { _
             .Day = DayOfWeek.Saturday, _
             .CssClass = "green_section" _
            })
            sched.TimeSpans.Add(New DHXMarkTime() With { _
             .StartDate = New DateTime(2011, 9, 25), _
             .EndDate = New DateTime(2011, 9, 29), _
             .CssClass = "green_section" _
            })

			sched.LoadData = True
			sched.EnableDataprocessor = True
			Return View(sched)
		End Function



		''' <summary>
		''' Highligting pointed area
		''' </summary>
		''' <returns></returns>
		Public Function Highlight() As ActionResult
            Dim sched = New DHXScheduler(Me)
           
			sched.InitialDate = New DateTime(2011, 9, 19)

			sched.Highlighter.Enable("highlight_section")
			'use 'highlight_section' class for highlighted area
			sched.LoadData = True
			sched.EnableDataprocessor = True
			Return View(sched)
		End Function

		''' <summary>
		''' Single click event creation
		''' </summary>
		''' <returns></returns>
		Public Function HighlightClickCreate() As ActionResult
            Dim sched = New DHXScheduler(Me)
        
			sched.InitialDate = New DateTime(2011, 9, 19)
			sched.Config.drag_create = False

			'use 'highlight_section' class for highlighted area   
			'size of the area = 120 minutes
			sched.Highlighter.Enable("highlight_section", 120)
			sched.Highlighter.FixedStep = False
			'create event on single click on highlighted area
			sched.Highlighter.CreateOnClick = True
			sched.LoadData = True
			sched.EnableDataprocessor = True
			Return View(sched)
		End Function


		''' <summary>
		''' Custom event containers
		''' </summary>
		''' <returns></returns>
		Public Function CustomEventBox() As ActionResult
			Dim sched = New DHXScheduler(Me)

			'helper for event templates,
			'it also can be defined on the client side
			Dim evBox = New DHXEventTemplate()

			evBox.CssClass = InlineAssignHelper(sched.Templates.event_class, "my_event")
			'class to be applied to event box
			' template will be rendered to the js function - function(ev, start, end){....}
			' where ev - is event object itself
			' template allows to inject js code within asp.net-like tags, and output properties of event with simplified sintax
			' {text} is equivalent to ' + ev.text + '
            evBox.Template = "<div class='my_event_body'>" & vbCr & vbLf & "                    <% if((ev.end_date - ev.start_date) / 60000 > 60) { %>" & vbCr & vbLf & "                        <span class='event_date'>{start_date:date(%H:%i)} - {end_date:date(%H:%i)}</span><br/>" & vbCr & vbLf & "                    <% } else { %>" & vbCr & vbLf & "                        <span class='event_date'>{start_date:date(%H:%i)}</span>" & vbCr & vbLf & "                    <% } %>                  " & vbCr & vbLf & "                    <span>{text}</span>" & vbCr & vbLf & "                    <br>" & vbCr & vbLf & "                    <div style=""padding-top:5px;"">" & vbCr & vbLf & "                        Duration: <b><%= Math.ceil((ev.end_date - ev.start_date) / (60 * 60 * 1000)) %></b> hours" & vbCr & vbLf & "                    </div>" & vbCr & vbLf & "                  </div>"
			sched.Templates.EventBox = evBox
			sched.LoadData = True
			sched.EnableDataprocessor = True
			Return View(sched)
		End Function

		Public Function IndexRazor() As ActionResult
			Return Index()
		End Function



		''' <summary>
		''' custom DataObject-to-JSON function
		''' </summary>
		''' <param name="builder"></param>
		''' <param name="ev"></param>
		Public Sub eventRenderer(builder As System.Text.StringBuilder, ev As Object)
			Dim item = TryCast(ev, [Event])
			'need to escape quotes and other characters, that may break json
			builder.Append(String.Format("{{id:{0}, text:""{1}"", start_date:""{2:MM/dd/yyyy HH:mm}"", end_date:""{3:MM/dd/yyyy HH:mm}""}}", item.id, item.text.Replace("""", "\"""), item.start_date, item.end_date))
		End Sub
		''' <summary>
		''' Rendering data with custom function
		''' </summary>
		''' <returns></returns>
		Public Function CustomData() As ContentResult
			Dim data = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)
			'
'             *  return data;
'             *  is equal to 
'             *  return Content(data.Render());
'             *  SchedulerAjaxData.Render can also take Action<StringBuilder, object> as a parameter
'             

			Return Content(data.Render(AddressOf eventRenderer))
		End Function

		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)
			Return data__1
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
						'update all properties, except for id
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
		Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
			target = value
			Return value
		End Function



	End Class
End Namespace
