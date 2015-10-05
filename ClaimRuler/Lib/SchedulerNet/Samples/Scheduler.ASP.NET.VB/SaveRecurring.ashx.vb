Imports System.Web
Imports System.Web.Services
Imports DHTMLX.Common

Public Class SaveRecurring
    Implements System.Web.IHttpHandler


    Protected Function deleteRelated(ByVal action As DataAction, ByVal changedEvent As Recurring, ByVal context As SchedulerDataContext) As Boolean
        Dim finished As Boolean = False
        If (action.Type = DataActionTypes.Delete OrElse action.Type = DataActionTypes.Update) AndAlso Not String.IsNullOrEmpty(changedEvent.rec_type) Then
            context.Recurrings.DeleteAllOnSubmit(From ev In context.Recurrings Where ev.event_pid = changedEvent.id)
        End If
        If action.Type = DataActionTypes.Delete AndAlso changedEvent.event_pid <> 0 Then
            Dim changed As Recurring = (From ev In context.Recurrings Where ev.id = action.TargetId).[Single]()
            changed.rec_type = "none"
            finished = True
        End If
        Return finished
    End Function

    Protected Function insertRelated(ByVal action As DataAction, ByVal changedEvent As Recurring, ByVal context As SchedulerDataContext) As DataAction
        If action.Type = DataActionTypes.Insert AndAlso changedEvent.rec_type = "none" Then
            'insert_related
            action.Type = DataActionTypes.Delete
        End If
        Return action
    End Function

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/xml"
        Dim action = New DataAction(context.Request.Form)
        Dim data = New SchedulerDataContext()

        Try
            Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType(Recurring), context.Request.Form), Recurring)
            Dim isFinished As Boolean = deleteRelated(action, changedEvent, data)
            If Not isFinished Then
                Select Case action.Type
                    Case DataActionTypes.Insert
                        ' define here your Insert logic
                        data.Recurrings.InsertOnSubmit(changedEvent)

                        Exit Select
                    Case DataActionTypes.Delete
                        ' define here your Delete logic
                        changedEvent = data.Recurrings.SingleOrDefault(Function(ev) ev.id = action.SourceId)
                        data.Recurrings.DeleteOnSubmit(changedEvent)
                        Exit Select
                    Case Else
                        ' "update" // define here your Update logic
                        Dim updated = data.Recurrings.SingleOrDefault(Function(ev) ev.id = action.SourceId)
                        DHXEventsHelper.Update(updated, changedEvent, New List(Of String)() From {"id"})


                        Exit Select
                End Select
            End If
            data.SubmitChanges()
            action.TargetId = changedEvent.id
            action = insertRelated(action, changedEvent, data)
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

