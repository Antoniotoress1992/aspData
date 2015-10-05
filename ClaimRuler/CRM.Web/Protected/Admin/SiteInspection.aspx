<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="SiteInspection.aspx.cs" Inherits="CRM.Web.Protected.Admin.SiteInspection" %>
<%@ Register src="../../UserControl/Admin/ucSiteInspection.ascx" tagname="ucSiteInspection" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucSiteInspection ID="ucSiteInspection1" runat="server" />
</asp:Content>
