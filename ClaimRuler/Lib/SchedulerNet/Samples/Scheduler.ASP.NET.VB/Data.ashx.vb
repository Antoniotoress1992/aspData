Imports System.Web
Imports System.Web.Services
Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Common


Public Class Data
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest


        Dim dataResult As SchedulerAjaxData
        If context.Request.QueryString("recurring") IsNot Nothing Then
            dataResult = New SchedulerAjaxData((New SchedulerDataContext()).Recurrings)
        Else
            dataResult = New SchedulerAjaxData((New SchedulerDataContext()).Events)
        End If
        context.Response.ContentType = "application/json"
        context.Response.Write(dataResult.ToString())

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class