Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DHTMLX.Scheduler
Namespace SchedulerTest.Controllers
    ''' <summary>
    ''' you can create multiple scheduler on page
    ''' </summary>
    Public Class MultiSchedulerController
        Inherits Controller
        '
        ' GET: /MultiScheduler/
        Public Class [mod]

            Public sh1 As DHXScheduler
            Public sh2 As DHXScheduler
        End Class
        Public Function Index() As ActionResult
            'each scheduler must have unique name
            Dim scheduler = New DHXScheduler("sched1")

            Dim scheduler2 = New DHXScheduler("sched2")

            Return View(New [mod]() With { _
             .sh1 = scheduler, _
             .sh2 = scheduler2 _
            })
        End Function

    End Class
End Namespace
