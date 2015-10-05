<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="WindPolicy.aspx.cs" Inherits="CRM.Web.Protected.Admin.WindPolicy" %>
<%@ Register src="../../UserControl/Admin/ucWindPolicy.ascx" tagname="ucWindPolicy" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucWindPolicy ID="ucWindPolicy1" runat="server" />
</asp:Content>
