<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSelectAdjuster.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucSelectAdjuster" %>
<asp:DropDownList ID="ddlAdjuster" runat="server">
</asp:DropDownList>
<asp:RequiredFieldValidator ID="rfvAdjuster" runat="server" ControlToValidate="ddlAdjuster"
	Display="Dynamic" ForeColor="" ErrorMessage="Please select adjuster" 
	CssClass="validation1" InitialValue="0">
</asp:RequiredFieldValidator>
