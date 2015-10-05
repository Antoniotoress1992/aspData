<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="Contractor.aspx.cs" Inherits="CRM.Web.Protected.Admin.Contractor" %>

<%@ Register Src="../../UserControl/Admin/ucContractor.ascx" TagName="ucContractor" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<uc1:ucContractor ID="ucContractor1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
