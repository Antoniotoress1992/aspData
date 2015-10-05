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
    Public Class CustomScaleController
        Inherits Controller
        ''' <summary>
        ''' Default initialization
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function Index() As ActionResult

            Dim sched = New DHXScheduler(Me)

            sched.Config.first_hour = 8
            sched.Config.last_hour = 18
            sched.InitialDate = New DateTime(2011, 9, 19)

            Dim rooms = New DHXSchedulerDataContext().Rooms.ToList()
            Dim timeline = New TimelineView("timeline", "room_id")
            timeline.X_Unit = TimelineView.XScaleUnits.Hour
            timeline.X_Size = 72

            timeline.Scale.IgnoreRange(19, 23)
            timeline.Scale.IgnoreRange(0, 7)

            timeline.RenderMode = TimelineView.RenderModes.Bar
            timeline.FitEvents = False


            sched.TimeSpans.Add(New DHXMarkTime With
            {
                .FullWeek = True,
                .Zones = New List(Of Zone) From {New Zone(8 * 60 + 10, 19 * 60 - 10)},
                .CssClass = "day_split",
                .InvertZones = True,
                .Sections = New List(Of Section) From {
                    New Section(timeline.Name, rooms.Select(Function(r) r.key.ToString()).ToList()) _
                }
            })

            timeline.AddOptions(rooms)
            sched.Views.Add(timeline)
            sched.InitialView = timeline.Name
            Dim week = sched.Views(1)
            week.Scale.Ignore(DirectCast(DayOfWeek.Saturday, Integer), DirectCast(DayOfWeek.Sunday, Integer))


            sched.LoadData = True

            Return View(sched)
        End Function

       
        Public Function Data() As ContentResult
            Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)
            Return data__1
        End Function

    End Class
End Namespace
