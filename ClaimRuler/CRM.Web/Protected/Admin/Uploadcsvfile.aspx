<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Uploadcsvfile.aspx.cs" Inherits="CRM.Web.Protected.Admin.Uploadcsvfile" %>
<%@ Register src="../../UserControl/Admin/ucUploadCSV.ascx" tagname="ucUploadCSV" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <uc1:ucUploadCSV ID="ucUploadCSV2" runat="server" />
</asp:Content>
