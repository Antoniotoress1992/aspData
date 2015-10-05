<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFeeDesignation.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucFeeDesignation" %>
<asp:DropDownList ID="ddlFeeDesignation" runat="server" TabIndex="20">
	<asp:ListItem Text="Flat Rate" Value="1"></asp:ListItem>
	<asp:ListItem Text="CAT" Value="2"></asp:ListItem>
	<asp:ListItem Text="Daily" Value="3"></asp:ListItem>
    <asp:ListItem Text="Loss Percentage Fee" Value="4"></asp:ListItem>
    <asp:ListItem Text="Time & Expense Invoice Only" Value="5"></asp:ListItem>
</asp:DropDownList>
