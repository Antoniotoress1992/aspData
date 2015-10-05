<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="OtherSource.aspx.cs" Inherits="CRM.Web.Protected.Admin.OtherSource" %>
<%@ Register src="../../UserControl/Admin/ucOtherSource.ascx" tagname="ucOtherSource" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucOtherSource ID="ucOtherSource1" runat="server" />
</asp:Content>
