<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="LienholderList.aspx.cs" Inherits="CRM.Web.Protected.Admin.LienholderList" %>

<%@ Register Src="~/UserControl/Admin/ucLienHolder.ascx" TagName="ucLienHolder" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

	<asp:UpdatePanel ID="udpatepanel" runat="server">
		<ContentTemplate>
			<uc1:ucLienHolder ID="ucLienHolder1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>

</asp:Content>
