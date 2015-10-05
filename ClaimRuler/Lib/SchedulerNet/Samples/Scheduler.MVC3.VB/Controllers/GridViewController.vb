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
	''' using 'table' view
	''' </summary>
	Public Class GridViewController
		Inherits Controller

		Public Function Index() As ActionResult
			Dim scheduler = New DHXScheduler(Me)

			Dim grid = New GridView("grid")

			'adding the columns
				'data property, label
            grid.Columns.Add(New GridViewColumn("text", "Event") With { _
             .Width = 300 _
            })

				'can assign template for column contents
				' for more info about templates syntax see http://scheduler-net.com/docs/dhxscheduler.templates.html
            grid.Columns.Add(New GridViewColumn("start_date", "Date") With { _
             .Template = "<% if((ev.end_date - ev.start_date)/1440000 > 1){%>" & vbCr & vbLf & "                                {start_date:date(%d %M %Y)} - {end_date:date(%d %M %Y)} " & vbCr & vbLf & "                            <% }else{ %> " & vbCr & vbLf & "                                {start_date:date(%d %M %Y %H:%i)} " & vbCr & vbLf & "                            <% } %>", _
             .Align = GridViewColumn.Aligns.Left, _
             .Width = 200 _
            })



            grid.Columns.Add(New GridViewColumn("details", "Details") With { _
             .Align = GridViewColumn.Aligns.Left _
            })

			scheduler.Views.Add(grid)

			scheduler.Lightbox.Add(New LightboxText("text", "Text"))
			scheduler.Lightbox.Add(New LightboxText("details", "Details"))
			scheduler.Lightbox.Add(New LightboxTime("time"))

			scheduler.InitialView = grid.Name
			scheduler.InitialDate = New DateTime(2011, 9, 19)
			scheduler.LoadData = True
			scheduler.EnableDataprocessor = True
			Return View(scheduler)
		End Function

		Public Function Data() As ContentResult
			Dim data__1 = New SchedulerAjaxData((New CustomFieldsDataContext().Grids))

			Return data__1
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult
			Dim action = New DataAction(actionValues)
			Dim data As New CustomFieldsDataContext()
			Try
				Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType(Grid), actionValues), Grid)
				Select Case action.Type
					Case DataActionTypes.Insert
						data.Grids.InsertOnSubmit(changedEvent)
						Exit Select
					Case DataActionTypes.Delete
						changedEvent = data.Grids.SingleOrDefault(Function(ev) ev.id = action.SourceId)
						data.Grids.DeleteOnSubmit(changedEvent)
						Exit Select
					Case Else
						' "update"                          
						Dim eventToUpdate = data.Grids.SingleOrDefault(Function(ev) ev.id = action.SourceId)
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
			Return (New AjaxSaveResponse(action))
		End Function

	End Class
End Namespace
