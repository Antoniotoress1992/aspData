<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="CRM.Web.Protected.Admin.UserList" %>
<%@ Register src="~/UserControl/Admin/ucUserList.ascx" tagname="ucUserList" tagprefix="uc1" %>


<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucUserList ID="ucUserList1" runat="server" />
</asp:Content>
