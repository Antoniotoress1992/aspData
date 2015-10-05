<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Umpire.aspx.cs" Inherits="CRM.Web.Protected.Admin.Umpire" %>

<%@ Register Src="~/UserControl/Admin/ucUmpire.ascx" TagName="ucUmpire" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<uc1:ucUmpire ID="ucUmpire1" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
