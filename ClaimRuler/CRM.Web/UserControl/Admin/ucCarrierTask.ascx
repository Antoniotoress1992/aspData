<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierTask.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierTask" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Src="~/UserControl/Admin/ucReminderInterval.ascx" TagName="ucReminderInterval" TagPrefix="uc2" %>

<div class="message_area">
	<asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>
<asp:Panel ID="pnlGrid" runat="server">
	<div style="margin-bottom: 5px;">
		<asp:LinkButton ID="btnNewTask" runat="server" Text="New Task" OnClick="btnNewTask_Click" CssClass="link" />
	</div>
	<asp:GridView ID="gvTasks" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="gridView"
		CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff" OnRowCommand="gvTasks_RowCommand"
		RowStyle-HorizontalAlign="Center">
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:ImageButton runat="server" ID="btnEdit" CommandName="DoEdit"
						ImageUrl="~/Images/edit_icon.png" CommandArgument='<%# Eval("id") %>' ToolTip="Edit" />
					&nbsp;
                    <asp:ImageButton runat="server" ID="btnDelete" Text="Delete" CommandName="DoDelete"
					ImageUrl="~/Images/delete_icon.png" CommandArgument='<%# Eval("id") %>'
					OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this task?')"
					ToolTip="Delete" />
				</ItemTemplate>
				<ItemStyle HorizontalAlign="Center" Width="50px" />
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Start Date" ItemStyle-HorizontalAlign="Center"
				SortExpression="start_date">
				<ItemStyle Width="120px" />
				<ItemTemplate>
					<%# Eval("start_date", "{0:g}") %> 
				</ItemTemplate>
			</asp:TemplateField>
            <asp:TemplateField HeaderText="End Date" ItemStyle-HorizontalAlign="Center"
				SortExpression="start_date">
				<ItemStyle Width="120px" />
				<ItemTemplate>
					<%# Eval("end_date", "{0:g}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Event" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("text") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("details")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="User Name" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("SecUser.UserName") %>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server" Visible="false" DefaultButton="btnSave">
	<table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 1px;" border="0" class="editForm no_min_width">
		<tr>
			<td style="width: 25%;" class="right">Task Description</td>
			<td class="redstar" style="width: 5px">*</td>
			<td>
				<ig:WebTextEditor ID="txtSubject" runat="server" MaxLength="250" Width="400px" TabIndex="1"></ig:WebTextEditor>
				<div>
					<asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true" Display="Dynamic"
						ErrorMessage="Please enter subject." ValidationGroup="appointment" CssClass="validation1" />
				</div>
			</td>

		</tr>
		<tr>
			<td class="right middle">Start Date</td>
			<td class="redstar">*</td>
			<td>
				<table class="editForm no_min_width">
					<tr>
						<td>
							<ig:WebDatePicker ID="startDate" runat="server" CssClass="date_picker" TabIndex="2">
								<Buttons>
									<CustomButton ImageUrl="~/Images/ig_calendar.gif" />
								</Buttons>
							</ig:WebDatePicker>
							<div>
								<%--<asp:RequiredFieldValidator ID="rfvstartdate" runat="server" ControlToValidate="startDate" SetFocusOnError="true" Display="Dynamic"
									ErrorMessage="Please enter date." ValidationGroup="appointment" CssClass="validation1" />--%>
							</div>
						</td>
						<td class="right">Start Time</td>
						<td class="redstar">*</td>
						<td>							
							<ig:WebTextEditor ID="startTime" runat="server" MaxLength="8" Width="80px" TabIndex="3"/>								
							<div>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="startTime" SetFocusOnError="true" Display="Dynamic"
									ErrorMessage="Please enter time." ValidationGroup="appointment" CssClass="validation1" />
								<asp:CustomValidator ID="CustomValidator1" runat="server" 
									ErrorMessage="Invalid time" OnServerValidate="IsTimeValid" ValidationGroup="appointment"
									ControlToValidate="startTime" Display="Dynamic" CssClass="validation1">
								</asp:CustomValidator>
							</div>
						</td>

					</tr>
				</table>

			</td>
		</tr>
		<tr>
			<td class="right middle">End Date</td>
			<td class="redstar">*</td>
			<td>
				<table>
					<tr>
						<td>
							<ig:WebDatePicker ID="endDate" runat="server" TabIndex="4" CssClass="date_picker">
								<Buttons>
									<CustomButton ImageUrl="~/Images/ig_calendar.gif" />
								</Buttons>
							</ig:WebDatePicker>
							<div>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="endDate" SetFocusOnError="true" Display="Dynamic"
									ErrorMessage="Please enter date." ValidationGroup="appointment" CssClass="validation1" />
							</div>
						</td>
						<td class="right">End Time</td>
						<td class="redstar">&nbsp;*</td>
						<td>
							<ig:WebTextEditor ID="endTime" runat="server" MaxLength="8" Width="80px" TabIndex="5"></ig:WebTextEditor>
							<div>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="endTime" SetFocusOnError="true" Display="Dynamic"
									ErrorMessage="Please enter time." ValidationGroup="appointment" CssClass="validation1" />
								<asp:CustomValidator ID="CustomValidator2" runat="server" 
									ErrorMessage="Invalid time" OnServerValidate="IsTimeValid" ValidationGroup="appointment"
									ControlToValidate="endTime" Display="Dynamic" CssClass="validation1">
								</asp:CustomValidator>
							</div>
						</td>

					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td></td>
			<td></td>
			<td class="top">
				<asp:CheckBox ID="cbxAllDayEvent" runat="server" Text=" All day event" TextAlign="Right" TabIndex="6" />
			</td>
		</tr>
		<tr>
			<td class="right">Reminder</td>
			<td></td>
			<td class="top">
				<uc2:ucReminderInterval ID="ucReminderInterval" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="top right">Details</td>
			<td></td>
			<td>
				<ig:WebTextEditor ID="txtDetails" runat="server" TextMode="MultiLine" MultiLine-Wrap="true" TabIndex="8"
					MaxLength="250" Width="400px" Height="100px">
				</ig:WebTextEditor>
			</td>
		</tr>
		<tr>
			<td class="right">Priority</td>
			<td></td>
			<td>
				<asp:DropDownList ID="ddlPriority" runat="server"  TabIndex="9"/>
			</td>
		</tr>
		<tr>
			<td colspan="3" class="center">
				<div style="margin-top: 15px;">
					<asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="appointment" OnClick="btnSave_Click" CssClass="mysubmit" />
					&nbsp;
					<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" CssClass="mysubmit" />
				</div>
			</td>
		</tr>
	</table>
</asp:Panel>
<%--<script type="text/javascript">
	function validateTime(sender, args) {
		var isValid = true;

		var timeStr =  args.get_value();
				
		var regEx = /^(\d{1,2}):(\d{2})(:(\d{2}))?(\s?(AM|am|PM|pm))?$/;
	
		var matchArray = timeStr.match(regEx);

		var control = $("#" + sender._id);

		if (matchArray != null) {

			hour = matchArray[1];
			minute = matchArray[2];
			second = matchArray[4];
			ampm = matchArray[6];

			if (second == "") { second = null; }

			if (ampm == "") {
				alert('Invalid Time!');
				control.focus();
			}

			if (hour < 0 || hour > 12) {
				alert('Invalid Time!');
				control.focus();
			}

			if (minute < 0 || minute > 59) {
				alert('Invalid Time!');
				control.focus();
			}
		}
		else {
			alert('Invalid Time!');
			control.focus();
		}
	}

</script>--%>