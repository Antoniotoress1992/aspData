<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="SubStatus.aspx.cs" Inherits="CRM.Web.Protected.Admin.SubStatus" %>
<%@ Register src="~/UserControl/Admin/ucSubStatus.ascx" tagname="ucSubStatus" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<uc1:ucSubStatus ID="ucSubStatus1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
    
</asp:Content>
