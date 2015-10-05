<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="ReportedToInsurer.aspx.cs" Inherits="CRM.Web.Protected.Admin.ReportedToInsurer" %>
<%@ Register src="../../UserControl/Admin/ucReportedToInsurer.ascx" tagname="ucReportedToInsurer" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucReportedToInsurer ID="ucReportedToInsurer1" runat="server" />
</asp:Content>
