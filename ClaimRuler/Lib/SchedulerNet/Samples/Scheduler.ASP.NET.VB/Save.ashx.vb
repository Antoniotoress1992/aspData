Imports System.Web
Imports System.Web.Services
Imports DHTMLX.Common

Public Class Save
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        context.Response.ContentType = "text/xml"
        Dim action = New DataAction(context.Request.Form)

        Dim data As New SchedulerDataContext()
        Try

            Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), context.Request.Form), [Event])
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
                    DHXEventsHelper.Update(eventToUpdate, changedEvent, New List(Of String)() From {"id"})
                    Exit Select
            End Select
            data.SubmitChanges()
            action.TargetId = changedEvent.id
        Catch a As Exception
            action.Type = DataActionTypes.[Error]
        End Try

        context.Response.Write(New AjaxSaveResponse(action).ToString())

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class