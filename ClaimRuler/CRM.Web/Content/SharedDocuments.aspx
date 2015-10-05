<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
	CodeBehind="SharedDocuments.aspx.cs" Inherits="CRM.Web.Content.SharedDocuments" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
	<div class="page-title">
		Shared Documents 
	</div>
	<h2>
		<asp:Label ID="lblName" runat="server" />
	</h2>
	<asp:GridView ID="gvDocument" runat="server" AutoGenerateColumns="false" CssClass="gridView" ShowHeader="true" RowStyle-BackColor="White" AlternatingRowStyle-BackColor="#e8f2ff"
		OnRowDataBound="gvDocument_RowDataBound" HorizontalAlign="Center" CellPadding="2" Width="100%" GridLines="Both" Height="100%">
		<Columns>

			<asp:TemplateField>
				<ItemStyle Width="32px" HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:Image ID="imgDocumentType" runat="server" Width="24px" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Document Name">
				<ItemTemplate>
					<asp:HyperLink ID="lnkDocument" runat="server" Target="_blank" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Description">
				<ItemTemplate>
					<%# Eval("Description") %>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
