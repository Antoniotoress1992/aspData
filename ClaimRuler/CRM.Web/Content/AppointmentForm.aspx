<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AppointmentForm.aspx.cs" Inherits="CRM.Web.Content.AppointmentForm" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControl/Admin/ucPolicyType.ascx" TagName="ucPolicyType"
	TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Admin/ucReminderInterval.ascx" TagName="ucReminderInterval"
	TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			window.focus();
			var startDateTime = new Date(window.opener.form1.startDateTime.value);

			var datePart = getDatePart(startDateTime);
			document.getElementById("<%= startDate.ClientID%>").value = datePart;

			var timePart = getTimePart(startDateTime);
			document.getElementById("<%= startTime.ClientID%>").value = timePart;
		});
	</script>
	<div class="paneContent">
		<div class="page-title">
			Task
		</div>
		<div class="toolbar toolbar-body">
			<table>
				<tr>
					<td>
						<asp:LinkButton ID="btnSend1" runat="server" Text="" CssClass="toolbar-item" ValidationGroup="appointment"
							OnClick="btnSave_Click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/toolbar_save.png)">Save and Close</span>
						</asp:LinkButton>
					</td>
					<td>
						<asp:LinkButton ID="btnDelete" runat="server" Text="Save and Close" CssClass="toolbar-item"
							OnClick="btnDelete_Click" CausesValidation="false"
							OnClientClick="javascript:return ConfirmDialog(this, 'Would you like to delete this task?');">
								<span class="toolbar-img-and-text" style="background-image: url(../images/delete_icon.png)">Delete</span>
						</asp:LinkButton>
					</td>
				</tr>
			</table>
		</div>
		<div class="paneContentInner" style="height: 450px;">
			<table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 1px;" border="0">
				<tr>
					<td style="width: 25%;" class="right">Subject</td>
					<td class="redstar" style="width: 5px">*</td>
					<td>
						<ig:WebTextEditor ID="txtSubject" runat="server" MaxLength="250" Width="400px" Height="18px"></ig:WebTextEditor>
						<div>
							<asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" SetFocusOnError="true" Display="Dynamic"
								ErrorMessage="Required!" ValidationGroup="appointment" CssClass="validation1" />
						</div>
					</td>

				</tr>
				<tr>
					<td></td>
					<td></td>
					<td>
						<hr />
					</td>
				</tr>
				<tr>
					<td class="right middle">Start Date</td>
					<td class="redstar">*</td>
					<td>
						<table>
							<tr>
								<td>
									<ig:WebDatePicker ID="startDate" runat="server" CssClass="date_picker" EditModeFormat="MM/dd/yyyy" DataMode="Date" ></ig:WebDatePicker>
									<div>
										<asp:RequiredFieldValidator ID="rfvstartdate" runat="server" ControlToValidate="startDate" SetFocusOnError="true" Display="Dynamic"
											ErrorMessage="Required!" ValidationGroup="appointment" CssClass="validation1" />
										<asp:CompareValidator ID="cvstatedate" runat="server" ControlToValidate="startDate" Operator="DataTypeCheck" Display="Dynamic"
											CssClass="validation1" SetFocusOnError="true" ErrorMessage="Invalid date!" ValidationGroup="appointment" />
									</div>
								</td>
								<td class="right">Start Time</td>
								<td class="redstar">*</td>
								<td>
									<ig:WebTextEditor ID="startTime" runat="server" MaxLength="8" Width="80px" Height="18px" TextMode="SingleLine"></ig:WebTextEditor>
									<div>
										<asp:CustomValidator ID="cvStartDate" runat="server" EnableClientScript="true"
											ErrorMessage="Invalid time" ClientValidationFunction="IsValidTime"
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
									<ig:WebDatePicker ID="endDate" runat="server" CssClass="date_picker" EditModeFormat="MM/dd/yyyy" DataMode="Date"></ig:WebDatePicker>
									<div>
										<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="endDate" SetFocusOnError="true" Display="Dynamic"
											ErrorMessage="Required!" ValidationGroup="appointment" CssClass="validation1" />
										<asp:CompareValidator ID="cpenddate" runat="server" ControlToValidate="endDate" Operator="DataTypeCheck" Display="Dynamic"
											CssClass="validation1" SetFocusOnError="true" ErrorMessage="Invalid date!" ValidationGroup="appointment" />
									</div>
								</td>
								<td class="right">End Time</td>
								<td class="redstar">&nbsp;*</td>
								<td>
									<ig:WebTextEditor ID="endTime" runat="server" MaxLength="8" Width="80px" Height="18px"></ig:WebTextEditor>
									<div>
										<asp:CustomValidator ID="cvEndTime" runat="server" EnableClientScript="true"
											ErrorMessage="Invalid time" ClientValidationFunction="IsValidTime"
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
						<asp:CheckBox ID="cbxAllDayEvent" runat="server" Text=" All day event" TextAlign="Right" />
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
					<td></td>
					<td></td>
					<td>
						<hr />
					</td>
				</tr>
				
				<tr>
					<td class="top right">Details</td>
					<td></td>
					<td>
						<ig:WebTextEditor ID="txtDetails" runat="server" TextMode="MultiLine" MultiLine-Wrap="true"
							MaxLength="250" Width="400px" Height="100px">
						</ig:WebTextEditor>
					</td>
				</tr>
				<tr>
					<td class="right">Priority</td>
					<td></td>
					<td>
						<asp:DropDownList ID="ddlPriority" runat="server" DataTextField="PriorityName" DataValueField="PriorityID" />
					</td>
				</tr>
				<tr>
					<td class="right">User</td>
					<td class="redstar"></td>
					<td>
						<asp:DropDownList ID="ddlUsers" runat="server" />

					</td>
				</tr>
				<%--<tr>
					<td class="right">Coverage Type</td>
					<td class="redstar">*</td>
					<td>
						<uc1:ucPolicyType ID="ucPolicyType" runat="server" />
					</td>
				</tr>--%>
			</table>
		</div>
	</div>
</asp:Content>
