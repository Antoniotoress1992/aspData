<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="TypeofProperty.aspx.cs" Inherits="CRM.Web.Protected.Admin.TypeofProperty" %>
<%@ Register src="~/UserControl/Admin/ucTypeofProperty.ascx" tagname="ucTypeofProperty" tagprefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucTypeofProperty ID="ucTypeofProperty1" runat="server" />
</asp:Content>
