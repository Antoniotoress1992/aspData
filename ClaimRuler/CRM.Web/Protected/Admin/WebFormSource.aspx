<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="WebFormSource.aspx.cs" Inherits="CRM.Web.Protected.Admin.WebFormSource" %>
<%@ Register src="../../UserControl/Admin/ucWebFormSource.ascx" tagname="ucWebFormSource" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucWebFormSource ID="ucWebFormSource1" runat="server" />
</asp:Content>
