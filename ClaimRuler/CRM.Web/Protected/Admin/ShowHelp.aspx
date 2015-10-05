<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true" CodeBehind="ShowHelp.aspx.cs" Inherits="CRM.Web.Protected.Admin.ShowHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<script type="text/javascript">
		function openHelpWindow(path) {
			window.open(path, "Help");
		}

	</script>
</asp:Content>
