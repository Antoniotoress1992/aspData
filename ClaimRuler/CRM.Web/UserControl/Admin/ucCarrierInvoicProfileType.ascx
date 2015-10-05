<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierInvoicProfileType.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierInvoicProfileType" %>
<asp:DropDownList ID="ddlInvoicProfileType" runat="server">	
	<asp:ListItem Text="--- Select One ---" Value="0" />
	<asp:ListItem Text="Flat Rate per File" Value="1" />
	<asp:ListItem Text="CAT Rate per File" Value="2" />
	<asp:ListItem Text="Time and Expense Billing" Value="3" />
</asp:DropDownList>

