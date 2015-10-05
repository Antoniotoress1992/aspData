Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Imports DHTMLX.Scheduler

Namespace SchedulerTest.Controllers
	''' <summary>
	''' export to pdf,
	''' 'ToPDF' function is rendered on View/ExportToPDF/Index.aspx.
	''' Using online service to generate pdf document,
	''' For more details see http://scheduler-net.com/docs/data_export.html#export_to_pdf
	''' </summary>
	Public Class ExportToPDFController
		Inherits BasicSchedulerController

		Public Overrides Function Index() As ActionResult
			Dim sched = New DHXScheduler()

			sched.Controller = "BasicScheduler"
			sched.Config.first_hour = 8
			sched.LoadData = True
			sched.EnableDataprocessor = True


			' 'PDF' extension is required
			sched.Extensions.Add(SchedulerExtensions.Extension.PDF)


			sched.InitialDate = New DateTime(2011, 9, 19)
			Return View(sched)
		End Function

	End Class
End Namespace
