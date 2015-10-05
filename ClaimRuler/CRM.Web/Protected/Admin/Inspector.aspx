<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="Inspector.aspx.cs" Inherits="CRM.Web.Protected.Admin.Inspector" %>
<%@ Register src="../../UserControl/Admin/ucInspector.ascx" tagname="ucInspector" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucInspector ID="ucInspector1" runat="server" />
</asp:Content>
