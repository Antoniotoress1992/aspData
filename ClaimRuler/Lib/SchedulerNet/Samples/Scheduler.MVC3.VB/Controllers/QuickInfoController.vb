
Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data

Namespace DHXSchedulerSamples
    Public Class QuickInfoController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /QuickInfo

        Function Index() As ActionResult
            Dim sched = New DHXScheduler(Me)

            sched.InitialDate = New DateTime(2011, 9, 19)
            sched.Data.Parse((New DHXSchedulerDataContext()).Events)

            sched.Extensions.Add(SchedulerExtensions.Extension.QuickInfo)

            Return View(sched)
        End Function

    End Class
End Namespace