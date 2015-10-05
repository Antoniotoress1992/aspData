<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="LeadSource.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadSource" %>
<%@ Register src="~/UserControl/Admin/ucLeadSource.ascx" tagname="ucLeadSource" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<uc1:ucLeadSource ID="ucLeadSource1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
    
</asp:Content>
