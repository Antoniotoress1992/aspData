<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="RoleModule.aspx.cs" Inherits="CRM.Web.Protected.Admin.RoleModule" %>

<%@ Register Src="~/UserControl/Admin/ucRoleModule.ascx" TagName="ucRoleModule" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Role Module
        </div>
        <uc1:ucRoleModule ID="ucRoleModule1" runat="server" />
    </div>

</asp:Content>
