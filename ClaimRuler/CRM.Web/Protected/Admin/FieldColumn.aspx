<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="FieldColumn.aspx.cs" Inherits="CRM.Web.Protected.Admin.FieldColumn" %>

<%@ Register Src="../../UserControl/Admin/ucFieldColumn.ascx" TagName="ucFieldColumn"
	TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel1" runat="server">
		<ContentTemplate>
			<uc1:ucFieldColumn ID="ucFieldColumn1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
