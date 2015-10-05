<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="Modules.aspx.cs" Inherits="CRM.Web.Protected.Admin.Modules" %><%-- EnableEventValidation="false"--%>
<%@ Register src="~/UserControl/Admin/ucModule.ascx" tagname="ucModule" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucModule ID="ucModule1" runat="server" />
</asp:Content>
