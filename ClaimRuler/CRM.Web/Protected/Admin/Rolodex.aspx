<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" 
	CodeBehind="Rolodex.aspx.cs" Inherits="CRM.Web.Protected.Admin.Rolodex" %>

<%@ Register Src="../../UserControl/Admin/ucRolodex.ascx" TagName="ucRolodex" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>			
			<uc1:ucRolodex ID="ucRolodex1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
