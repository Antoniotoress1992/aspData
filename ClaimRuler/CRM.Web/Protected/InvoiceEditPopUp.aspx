<%@ Page Title="" Language="C#" MasterPageFile="~/SitePopUp.Master" AutoEventWireup="true" CodeBehind="InvoiceEditPopUp.aspx.cs" Inherits="CRM.Web.Protected.InvoiceEditPopUp" %>

<%@ Register Src="~/UserControl/ucInvoiceEdit.ascx" TagName="ucInvoiceEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        html {
            /* no background image for popup */
            background: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div class="page-title">
        Invoice 
    </div>
    <uc1:ucInvoiceEdit ID="invoiceEdit" runat="server" />
</asp:Content>
