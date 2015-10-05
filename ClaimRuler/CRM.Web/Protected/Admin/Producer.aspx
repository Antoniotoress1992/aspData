<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Producer.aspx.cs" Inherits="CRM.Web.Protected.Admin.Producer" %>
<%@ Register src="~/UserControl/Admin/ucProducer.ascx" tagname="ucProducer" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<uc1:ucProducer ID="ucProducer1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
    
</asp:Content>
