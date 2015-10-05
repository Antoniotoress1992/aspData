<%@ Page Language="C#" AutoEventWireup="true"
	MasterPageFile="~/SitePopUp.Master"
	CodeBehind="ReminderPopUp.aspx.cs" Inherits="CRM.Web.Content.ReminderPopUp" %>

<%@ Register Src="~/UserControl/Admin/ucReminderInterval.ascx" TagName="ucReminderInterval" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/SitePopUp.Master" %>
<asp:Content ID="header" runat="server" ContentPlaceHolderID="head">
     <style type="text/css">
        html {
            /* no background image for popup */
            background: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">

	<div class="paneContent">
		<div class="paneContentInner">
			<asp:GridView ID="gvReminder" CssClass="gridView" ShowFooter="false" Width="100%" runat="server"
				HorizontalAlign="Center" DataKeyNames="id"
				OnRowCommand="gv_RowCommand" AutoGenerateColumns="False" CellPadding="4"
				AlternatingRowStyle-BackColor="#e8f2ff"
				OnRowDataBound="gvReminder_RowDataBound">
				<RowStyle HorizontalAlign="Center" />
				<EmptyDataTemplate>
					No reminders available
				</EmptyDataTemplate>
				<Columns>
					<asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Right">
						<ItemStyle Width="20px" />
						<ItemTemplate>
							<%# Container.DataItemIndex + 1 %>
						</ItemTemplate>

					</asp:TemplateField>
					<asp:TemplateField HeaderText="Subject" ItemStyle-HorizontalAlign="Left">
						<ItemStyle Width="70%" />
						<ItemTemplate>
							<div><%# Eval("start_date", "{0:f}") %></div>
							<div>
								<b><%# Eval("text") %></b>
							</div>
							<div>
								<%# Eval("details") %>
							</div>
						</ItemTemplate>

					</asp:TemplateField>
					<asp:TemplateField HeaderText="Due In">
						<ItemStyle Width="80px" HorizontalAlign="Center" />
						<ItemTemplate>
							<asp:Label ID="lblDueIn" runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField ItemStyle-Width="10px" ItemStyle-Wrap="true">
						<ItemTemplate>
							<a href='javascript:openAppointmentForm(<%#Eval("id") %>);' target="_blank">
								<asp:Image ImageUrl="~/Images/clock_edit.png" runat="server" />
							</a>
							<asp:ImageButton ID="btnDismiss" runat="server" CommandName="DoDismiss"
								CommandArgument='<%#Eval("id") %>' ToolTip="Dismiss" ImageUrl="~/Images/clock_delete.png"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>

			</asp:GridView>

			<div style="margin-top: 10px; margin-bottom: 10px;">
				Click Snooze to be reminded again in:
			</div>
			<div style="float: left;">
				<uc2:ucReminderInterval ID="ucReminderInterval" runat="server" />
				&nbsp;
				<asp:Button ID="btnSnooze" runat="server" Text="Snooze" CssClass="mysubmit" OnClick="btnSnooze_Click" ValidationGroup="snooze" />
			</div>
			<div style="float: right;">
				<asp:Button ID="btnDismissAll" runat="server" Text="Dismiss All" CssClass="mysubmit" OnClick="btnDismissAll_Click" />
			</div>

		</div>

	</div>


	<asp:Panel ID="pnlAlarm" runat="server">
		<embed hidden="hidden" width="0" height="0" type="audio/mpeg" src="../Sounds/alarm.mp3" />
	</asp:Panel>
	<asp:Panel ID="PnlAlarmOverDue" runat="server" Visible="false">
		<embed hidden="hidden" width="0" height="0" type="audio/mpeg" src="../Sounds/alarm_overdue.mp3" />
	</asp:Panel>


</asp:Content>
