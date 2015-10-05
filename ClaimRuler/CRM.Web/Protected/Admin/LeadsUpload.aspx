<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="LeadsUpload.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadsUpload" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="LeadsUpload.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadsUpload" %>

<%@ Register src="../../UserControl/Admin/UploadPhoto.ascx" tagname="UploadPhoto" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:UploadPhoto ID="UploadPhoto1" runat="server" />
</asp:Content>
