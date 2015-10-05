<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="TypeOfDamage.aspx.cs" Inherits="CRM.Web.Protected.Admin.TypeOfDamage" %>

<%@ Register Src="~/UserControl/Admin/ucTypeofDamage.ascx" TagName="ucTypeofDamage" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Type of Damage
        </div>
    </div>
    <uc1:ucTypeofDamage ID="ucTypeofDamage1" runat="server" />
</asp:Content>
