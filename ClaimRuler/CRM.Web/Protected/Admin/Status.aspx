<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="CRM.Web.Protected.Admin.Status" %>
<%@ Register src="../../UserControl/Admin/ucStatus.ascx" tagname="ucStatus" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			 <uc1:ucStatus ID="ucStatus1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
   
</asp:Content>
