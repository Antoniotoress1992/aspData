<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ClaimsClosedListing.aspx.cs" Inherits="CRM.Web.Reports.ClaimClosed.ClaimsClosedListing" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div style="margin:auto;">
        <rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="true"
            Font-Names="Tahoma"
            ProcessingMode="Local" ShowExportControls="true" ShowFindControls="true"
            ShowPageNavigationControls="true" ShowPrintButton="true" ShowReportBody="true"
            ShowZoomControl="true" SizeToReportContent="true" KeepSessionAlive="true" Visible="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
