<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="TaskExport.aspx.cs" Inherits="CRM.Web.Protected.TaskExport" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="page-title">
		Export Tasks to Calendar 
	</div>
	<div class="toolbar toolbar-body">
		<table>
			<tr>
				<td>
					<asp:LinkButton ID="btnReturnToClaim" runat="server" Text="" CssClass="toolbar-item" OnClick="btnClose_click" CausesValidation="false">
					<span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Tasks</span>
					</asp:LinkButton>
				</td>
				<td>
					<asp:LinkButton ID="btnExport" runat="server" Text="Days" CssClass="toolbar-item" OnClick="btnExport_click">
								<span class="toolbar-img-and-text" style="background-image: url(../images/export.png)">Export</span>
					</asp:LinkButton>
				</td>

			</tr>
		</table>
	</div>
	<div class="message_area">
		<asp:Label ID="lblMessage" runat="server" />
	</div>
	<asp:GridView ID="gvTasks" CssClass="gridView" Width="99%" runat="server"
		AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
		HeaderStyle-Font-Size="11px" RowStyle-HorizontalAlign="Center" HorizontalAlign="Center">
		<EmptyDataTemplate>
			No tasks available.
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:CheckBox ID="cbxExport" runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Start Date" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="start_date" runat="server" Text='<%# Eval("start_date", "{0:g}") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="End Date" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="end_date" runat="server" Text='<%# Eval("end_date", "{0:g}") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Event" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="event" runat="server" Text='<%# Eval("text") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="details" runat="server" Text='<%# Eval("details") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="User Name" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="owner_name" runat="server" Text='<%# Eval("owner_name") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Client/Claim" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="lead_name" runat="server" Text='<%# Eval("lead_name") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:Label ID="statusName" runat="server" Text='<%# Eval("statusName") %>' />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>


</asp:Content>
