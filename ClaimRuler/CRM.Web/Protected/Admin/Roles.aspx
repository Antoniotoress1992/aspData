<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="CRM.Web.Protected.Admin.Roles" %>

<%@ Register Src="~/UserControl/Admin/ucRoles.ascx" TagName="ucRoles" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <div class="paneContent">
        <div class="page-title">
            Roles
        </div>

        <uc1:ucRoles ID="ucRoles1" runat="server" />

    </div>

</asp:Content>
