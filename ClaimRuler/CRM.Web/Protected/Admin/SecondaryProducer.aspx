<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="SecondaryProducer.aspx.cs" Inherits="CRM.Web.Protected.Admin.SecondaryProducer" %>

<%@ Register Src="~/UserControl/Admin/ucSecondaryProducer.ascx" TagName="ucSecondaryProducer" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="paneContent">
		<div class="page-title">
			Secondary Producer
		</div>
		<uc1:ucSecondaryProducer ID="ucSecondaryProducer1" runat="server" />

	</div>
</asp:Content>
