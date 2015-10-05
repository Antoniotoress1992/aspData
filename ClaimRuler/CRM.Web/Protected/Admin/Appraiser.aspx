<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Appraiser.aspx.cs" Inherits="CRM.Web.Protected.Admin.Appraiser" %>

<%@ Register Src="../../UserControl/Admin/ucAppraiser.ascx" TagName="ucAppraiser" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<uc1:ucAppraiser ID="ucAppraiser1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
