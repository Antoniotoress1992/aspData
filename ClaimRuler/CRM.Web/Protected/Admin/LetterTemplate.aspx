<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="LetterTemplate.aspx.cs" Inherits="CRM.Web.Protected.Admin.LetterTemplate" %>

<%@ Register Src="~/UserControl/Admin/ucLetterTemplate.ascx" TagName="ucLetterTemplate"
	TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<uc1:ucLetterTemplate ID="ucLetterTemplate1" runat="server" />
</asp:Content>
