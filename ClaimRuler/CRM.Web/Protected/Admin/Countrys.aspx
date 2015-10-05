<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="Countrys.aspx.cs" Inherits="CRM.Web.Protected.Admin.Countrys" %>
<%@ Register src="../../UserControl/Admin/ucCountry.ascx" tagname="ucCountry" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucCountry ID="ucCountry1" runat="server" />
</asp:Content>
