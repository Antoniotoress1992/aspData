<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFieldColumn.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.ucFieldColumn" %>
<div class="page-title">
	Field Columns
</div>
<div class="toolbar toolbar-body">
	<table>
		<tr>
			<td>
				<asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="toolbar-item" OnClick="btnSave_click">
								<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_save.png)">Save</span>
				</asp:LinkButton>
			</td>
			<td></td>
		</tr>
	</table>
</div>
<div class="paneContentInner">

	<asp:GridView ID="gvFieldColumn" Width="65%" runat="server" CssClass="gridView"
		AutoGenerateColumns="False"
		DataKeyNames="ColumnID"
		HorizontalAlign="Center"
		CellPadding="4"
		AlternatingRowStyle-BackColor="#e8f2ff"
		RowStyle-HorizontalAlign="Center">
		<RowStyle HorizontalAlign="Center" />
		<Columns>
			<asp:TemplateField HeaderText="Show" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<asp:CheckBox ID="cbx" runat="server" Checked='<%# Eval("isVisible") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Column Name" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("ColumnName") %>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>

</div>
