<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="OwnerSame.aspx.cs" Inherits="CRM.Web.Protected.Admin.OwnerSame" %>
<%@ Register src="../../UserControl/Admin/ucOwnerSame.ascx" tagname="ucOwnerSame" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucOwnerSame ID="ucOwnerSame1" runat="server" />
</asp:Content>
