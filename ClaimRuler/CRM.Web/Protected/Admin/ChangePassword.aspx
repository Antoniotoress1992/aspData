<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CRM.Web.Protected.Admin.ChangePassword" %>
<%@ Register src="../../UserControl/Admin/ucChangePassword.ascx" tagname="ucChangePassword" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucChangePassword ID="ucChangePassword1" runat="server" />
</asp:Content>
