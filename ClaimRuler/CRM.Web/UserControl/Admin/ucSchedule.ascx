<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSchedule.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucSchedule" %>

<%@ Register Assembly="Infragistics45.WebUI.WebSchedule.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<div id="div_scheduler">

	<div class="toolbar toolbar-body">
		<table>
			<tr>
				<td>
					<asp:LinkButton ID="btn1Day" runat="server" Text="Day" CssClass="toolbar-item"
						OnClick="changeCalendarView_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/1day.png)">Day</span>
					</asp:LinkButton>
				</td>
				<td>
					<asp:LinkButton ID="btn5Days" runat="server" Text="Days" CssClass="toolbar-item"
						OnClick="changeCalendarView_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/5days.png)">Days</span>
					</asp:LinkButton>
				</td>
				<td>
					<asp:LinkButton ID="btnExport" runat="server" Text="Export to Gmail" CssClass="toolbar-item" Visible="false"
						PostBackUrl="~/Protected/TaskExport.aspx">
							<span class="toolbar-img-and-text" style="background-image: url(../images/gmail_export.png)">Export</span>
					</asp:LinkButton>
				</td>
				<td>
					<asp:LinkButton ID="btnResizeFull" runat="server" Text="" CssClass="toolbar-item" href="#" Visible="false"
						OnClientClick="resize_fullscreen();">
							<span class="toolbar-img-and-text" style="background-image: url(../images/resize_full.png)">Full Screen</span>
					</asp:LinkButton>
				</td>
				<td>
					<asp:LinkButton ID="btnResizeNormal" runat="server" Text="" CssClass="toolbar-item" href="#"
						OnClientClick="resize_normal();" Style="display: none;">
							<span class="toolbar-img-and-text" style="background-image: url(../images/resize_normal.png)">Normal</span>
					</asp:LinkButton>
				</td>
			</tr>
		</table>
	</div>

	<igsch:WebDayView ID="WebDayView1" runat="server" WebScheduleInfoID="WebScheduleInfo1"
		Width="100%" VisibleDays="30" EnableAppStyling="True" Height="650px" DayHeaderFormatString="dddd, MMMM dd yyyy">
	</igsch:WebDayView>
</div>
<igsch:WebScheduleInfo ID="WebScheduleInfo1" runat="server"
	AllowAllDayEvents="true"
	EnableRecurringActivities="True"
	EnableEmailReminders="true"
	EnableMultiResourceCaption="true"
	EnableMultiResourceView="true"
	EnableMultiDayEventBanner="true"
	EnableReminders="true"
	EnableMultiDayEventArrows="true"
	EnableUnassignedResource="false"
	EnableSmartCallbacks="true"
	WorkDayStartTime="8:00 AM"
	WorkDayEndTime="6:00 PM"
	OnActiveResourceChanged="WebScheduleInfo1_ActiveResourceChanged"
	OnActiveDayChanged="WebScheduleInfo1_ActiveDayChanged"
	 ReminderFormPath="~/Content/Reminder.aspx"
	FirstDayOfWeek="Monday">
	<ClientEvents ActivityDialogOpening="openAppointmentDialog" ReminderDialogOpening="openReminderDialog" />
</igsch:WebScheduleInfo>
<script type="text/javascript">
	

	function resize_normal() {
		
		$("[id$='btnResizeFull']").show();
		$("[id$='btnResizeNormal']").hide();

		var pos_left = $("#pos_left").val();
		var pos_top = $("#pos_top").val();
		$("#isFullScreen").val("0");


		$('#div_scheduler').css({
			position: 'relative',
			top: pos_left,
			left: pos_top,
			width: '100%',
			height: '100%'
		});
	}

	function resize_fullscreen() {
		var pos_left = $("#div_scheduler").offset().left;
		var pos_top = $("#div_scheduler").offset().top;
		$("#isFullScreen").val("1");

		$("#pos_left").val(pos_left);
		$("#pos_top").val(pos_top);


		$('#div_scheduler').css({
			position: 'absolute',
			top: 0,
			left: 0,
			width: $(window).width(),
			height: $(window).height()
		});

		$("[id$='WebDayView1']").css({
			height: '99%'
		});
		$("[id$='WebDayView1_divScroll']").css({
			height: '99%'
		});

		$("[id$='btnResizeFull']").hide();
		$("[id$='btnResizeNormal']").show();

	}
</script>
<script type="text/javascript" src="../js/ig_addAppointmentDialog.js"></script>
<script type="text/javascript" src="../js/ig_addAppointmentDialogForm.js"></script>
