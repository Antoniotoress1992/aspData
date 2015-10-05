Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Namespace SchedulerTest.Controllers
	Public Class IndexController
		Inherits Controller
		'
		' GET: /Index/

		Public Function Index() As ActionResult
			Return View()
		End Function

	End Class
End Namespace
