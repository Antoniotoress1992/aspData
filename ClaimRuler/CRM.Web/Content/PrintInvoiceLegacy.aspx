﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintInvoiceLegacy.aspx.cs" Inherits="CRM.Web.Content.PrintInvoiceLegacy" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
	Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content1">

	<rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="false">
	</rsweb:ReportViewer>

</asp:Content>
