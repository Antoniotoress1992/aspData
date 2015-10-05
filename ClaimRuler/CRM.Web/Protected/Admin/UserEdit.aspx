<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
    CodeBehind="UserEdit.aspx.cs" Inherits="CRM.Web.Protected.Admin.NewUser" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<%@ Register Src="~/UserControl/Admin/ucUserEdit.ascx" TagName="ucUserEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
        <uc1:ucUserEdit ID="ucUserEdit1" runat="server" />
</asp:Content>
