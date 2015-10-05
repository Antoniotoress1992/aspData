<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="ApplicationConfiguration.aspx.cs" Inherits="CRM.Web.Protected.Admin.ApplicationConfiguration" %>
<%@ Register src="../../UserControl/Admin/ucApplicationConfiguration.ascx" tagname="ucApplicationConfiguration" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucApplicationConfiguration ID="ucApplicationConfiguration1" 
        runat="server" />
</asp:Content>
