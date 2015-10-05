<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="AllUsersLeads.aspx.cs" Inherits="CRM.Web.Protected.Admin.AllUsersLeads" %>
<%@ Register src="~/UserControl/Admin/ucAllUserLeads.ascx" tagname="ucAllUserLeads" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucAllUserLeads ID="ucAllUserLeads1" runat="server" />
</asp:Content>
