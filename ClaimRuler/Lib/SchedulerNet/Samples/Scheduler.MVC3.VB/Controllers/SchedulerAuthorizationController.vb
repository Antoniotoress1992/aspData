Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc


Imports System.Web.Security

Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Common
Imports DHTMLX.Scheduler.Authentication

Namespace SchedulerTest.Controllers
	Public Class SchedulerAuthorizationController
		Inherits Controller
		'
		' GET: /SchedulerAuthorization/

		Public Function Index() As ActionResult
			Dim sched = New DHXScheduler(Me)

			sched.LoadData = True
			sched.EnableDataprocessor = True


			Dim context = New DHXSchedulerDataContext()

			If Request.IsAuthenticated Then
				Dim user = context.Users.SingleOrDefault(Function(u) u.UserId = CType(Membership.GetUser().ProviderUserKey, Guid))
				sched.SetUserDetails(user, "UserId")
				'pass dictionary<string, object> or any object which can be serialized to json(without circular references)
					'set field in event which will be compared to user id(same as sched.Authentication.UserIdKey by default)    
				sched.Authentication.EventUserIdKey = "user_id"
			End If
			sched.SetEditMode(EditModes.OwnEventsOnly, EditModes.AuthenticatedOnly)

			sched.InitialDate = New DateTime(2011, 9, 26)
			Return View(sched)
		End Function

		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)

			Return (data__1)
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), actionValues), [Event])

			If Me.Request.IsAuthenticated AndAlso changedEvent.user_id = CType(Membership.GetUser().ProviderUserKey, Guid) Then
				Dim data As New DHXSchedulerDataContext()
				Try
					Select Case action.Type
						Case DataActionTypes.Insert
							changedEvent.room_id = data.Rooms.First().key
							data.Events.InsertOnSubmit(changedEvent)
							Exit Select
						Case DataActionTypes.Delete
							changedEvent = data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId)
							data.Events.DeleteOnSubmit(changedEvent)
							Exit Select
						Case Else
							' "update"                          
							Dim eventToUpdate = data.Events.SingleOrDefault(Function(ev) ev.id = action.SourceId)
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
			Else
				action.Type = DataActionTypes.[Error]
			End If
			Return (New AjaxSaveResponse(action))
		End Function

	End Class
End Namespace
