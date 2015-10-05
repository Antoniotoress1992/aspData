<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="CRM.Web.Protected.Admin.UserDetail" %>
<%@ Register src="../../UserControl/Admin/ucUserDetail.ascx" tagname="ucUserDetail" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucUserDetail ID="ucUserDetail1" runat="server" />
</asp:Content>
