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
	''' See how custom views can be created
	''' </summary>
	Public Class CustomViewController
		Inherits Controller

		'real functionality is defined on the client side
		Public Class DecadeView
			Inherits SchedulerView
			Public Sub New()
				MyBase.New()
					'view type must be equal the one defined on the client
				Name = InlineAssignHelper(ViewType, "decade")
			End Sub
			Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
				target = value
				Return value
			End Function
		End Class
		Public Class WorkWeekView
			Inherits SchedulerView
			Public Sub New()
				MyBase.New()
				Name = InlineAssignHelper(ViewType, "workweek")
			End Sub
			Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
				target = value
				Return value
			End Function
		End Class
		Public Function Index() As ActionResult
            Dim scheduler = New DHXScheduler()
            scheduler.Skin = DHXScheduler.Skins.Terrace

            scheduler.Views.Add(New WorkWeekView() With { _
             .Label = "W-Week" _
            })
            scheduler.Views.Add(New DecadeView() With { _
             .Label = "Decade" _
            })
			Return View(scheduler)
		End Function

	End Class
End Namespace
