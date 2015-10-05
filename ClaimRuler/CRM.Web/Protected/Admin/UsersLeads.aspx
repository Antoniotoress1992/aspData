<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="UsersLeads.aspx.cs" Inherits="CRM.Web.Protected.Admin.UsersLeads" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<%@ Register Src="~/UserControl/Admin/ucAllUserLeads.ascx" TagName="ucAllUserLeads" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <uc1:ucAllUserLeads ID="ucAllUserLeads1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
	
</asp:Content>
