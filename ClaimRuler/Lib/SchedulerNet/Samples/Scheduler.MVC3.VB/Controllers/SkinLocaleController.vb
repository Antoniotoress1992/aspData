Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DHTMLX.Scheduler
Imports DHTMLX.Scheduler.Data
Imports DHTMLX.Scheduler.Controls
Imports DHTMLX.Common


Namespace SchedulerTest.Controllers

	''' <summary>
	''' check available skins and localizations
	''' </summary>
	Public Class SkinLocaleController
		Inherits Controller
		'
		' GET: /SkinLocale/
		Public Class LocaleData
			Public scheduler As DHXScheduler
			Public locale As String
			Public skin As String
			Public Sub New(sched As DHXScheduler, loc As String, sk As String)
				scheduler = sched
				locale = loc
				skin = sk
			End Sub
		End Class
		Public Function Index() As ActionResult
			Dim lang As SchedulerLocalization.Localizations
			Dim language = Me.Request.QueryString("language")
			Dim skn = Me.Request.QueryString("skin")
			Dim skin As DHXScheduler.Skins
			'#Region "language"
			Select Case Me.Request.QueryString("language")
				Case "ar"
					lang = SchedulerLocalization.Localizations.Arabic
					Exit Select
				Case "be"
					lang = SchedulerLocalization.Localizations.Belarusian
					Exit Select
				Case "ca"
					lang = SchedulerLocalization.Localizations.Catalan
					Exit Select
				Case "cn"
					lang = SchedulerLocalization.Localizations.Chinese
					Exit Select
				Case "cs"
					lang = SchedulerLocalization.Localizations.Czech
					Exit Select
				Case "da"
					lang = SchedulerLocalization.Localizations.Danish
					Exit Select
				Case "nl"
					lang = SchedulerLocalization.Localizations.Dutch
					Exit Select
				Case "fi"
					lang = SchedulerLocalization.Localizations.Finnish
					Exit Select
				Case "fr"
					lang = SchedulerLocalization.Localizations.French
					Exit Select
				Case "de"
					lang = SchedulerLocalization.Localizations.German
					Exit Select
				Case "el"
					lang = SchedulerLocalization.Localizations.Greek
					Exit Select
				Case "he"
					lang = SchedulerLocalization.Localizations.Hebrew
					Exit Select
				Case "hu"
					lang = SchedulerLocalization.Localizations.Hungarian
					Exit Select
				Case "id"
					lang = SchedulerLocalization.Localizations.Indonesia
					Exit Select
				Case "it"
					lang = SchedulerLocalization.Localizations.Italian
					Exit Select
				Case "jp"
					lang = SchedulerLocalization.Localizations.Japanese
					Exit Select
				Case "no"
					lang = SchedulerLocalization.Localizations.Norwegian
					Exit Select
				Case "pl"
					lang = SchedulerLocalization.Localizations.Polish
					Exit Select
				Case "pt"
					lang = SchedulerLocalization.Localizations.Portuguese
					Exit Select
				Case "ru"
					lang = SchedulerLocalization.Localizations.Russian
					Exit Select
				Case "ro"
					lang = SchedulerLocalization.Localizations.Romanian
					Exit Select
				Case "si"
					lang = SchedulerLocalization.Localizations.Slovenian
					Exit Select
				Case "es"
					lang = SchedulerLocalization.Localizations.Spanish
					Exit Select
				Case "sv"
					lang = SchedulerLocalization.Localizations.Swedish
					Exit Select
				Case "tr"
					lang = SchedulerLocalization.Localizations.Turkish
					Exit Select
				Case "ua"
					lang = SchedulerLocalization.Localizations.Ukrainian
					Exit Select
				Case Else
					lang = SchedulerLocalization.Localizations.English
					language = "en"
					Exit Select
			End Select
			'#End Region

			'#Region "skin"
			Select Case Me.Request.QueryString("skin")
				Case "glossy"
					skin = DHXScheduler.Skins.Glossy
					Exit Select
				Case "terrace"
					skin = DHXScheduler.Skins.Terrace
					Exit Select
				Case Else
					skin = DHXScheduler.Skins.Standart
					skn = "classic"
					Exit Select
			End Select
			'#End Region
			Dim scheduler = New DHXScheduler(Me)

			scheduler.InitialDate = New DateTime(2011, 11, 24)

			scheduler.XY.scroll_width = 0
			scheduler.Config.first_hour = 8
			scheduler.Config.last_hour = 19
			scheduler.Config.time_step = 30
			scheduler.Config.multi_day = True
			scheduler.Config.limit_time_select = True
			scheduler.Skin = skin
			scheduler.Localization.Directory = "locale"
			scheduler.Localization.[Set](lang, False)

			Dim rooms = New DHXSchedulerDataContext().Rooms.ToList()

			Dim unit = New UnitsView("unit1", "room_id")
			unit.AddOptions(rooms)
			'parse model objects
			scheduler.Views.Add(unit)

			Dim timeline = New TimelineView("timeline", "room_id")
			timeline.RenderMode = TimelineView.RenderModes.Bar
			timeline.FitEvents = False
			timeline.AddOptions(rooms)
			scheduler.Views.Add(timeline)


			scheduler.EnableDataprocessor = True
			scheduler.LoadData = True
			scheduler.InitialDate = New DateTime(2011, 9, 19)
			Return View(New LocaleData(scheduler, language, skn))
		End Function
		Public Function Data() As ContentResult

			Dim data__1 = New SchedulerAjaxData((New DHXSchedulerDataContext()).Events)


			Return data__1
		End Function

		Public Function Save(id As System.Nullable(Of Integer), actionValues As FormCollection) As ContentResult

			Dim action = New DataAction(actionValues)

			Dim data As New DHXSchedulerDataContext()
			Dim changedEvent = DirectCast(DHXEventsHelper.Bind(GetType([Event]), actionValues), [Event])
			Try
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
