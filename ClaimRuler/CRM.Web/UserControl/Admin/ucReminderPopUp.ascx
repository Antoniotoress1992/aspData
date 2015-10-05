<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReminderPopUp.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucReminderPopUp" %>


<%@ Register Src="~/UserControl/Admin/ucReminderInterval.ascx" TagName="ucReminderInterval" TagPrefix="uc2" %>

<div class="paneContent">

	<div class="paneContentInner">
		<asp:Panel ID="pnlSnooze" runat="server" DefaultButton="btnSnooze">
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
						<ItemStyle Width="80px" />
						<ItemTemplate>
							<asp:Label ID="lblDueIn" runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField ItemStyle-Width="10px" ItemStyle-Wrap="true">
						<ItemTemplate>
							<asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("id") %>'
								ToolTip="Open" ImageUrl="~/Images/clock_edit.png"></asp:ImageButton>
							<asp:ImageButton ID="btnDismiss" runat="server" CommandName="DoDismiss"
									CommandArgument='<%#Eval("id") %>' ToolTip="Dismiss" ImageUrl="~/Images/clock_delete.png"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>

			</asp:GridView>

			<div style="margin-top: 10px; margin-bottom: 10px;">
				Click Snooze to be reminded again in:
			</div>
			<div style="float:left;">
				<uc2:ucReminderInterval ID="ucReminderInterval" runat="server" />
				&nbsp;
				<asp:Button ID="btnSnooze" runat="server" Text="Snooze" CssClass="mysubmit" OnClick="btnSnooze_Click" ValidationGroup="snooze" />
			</div>
			<div style="float:right;">
				<asp:Button ID="btnDismissAll" runat="server" Text="Dismiss All" CssClass="mysubmit" OnClick="btnDismissAll_Click" />
			</div>
		</asp:Panel>

	</div>

</div>
