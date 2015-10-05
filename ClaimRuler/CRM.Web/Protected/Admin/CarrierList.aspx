<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="CarrierList.aspx.cs" Inherits="CRM.Web.Protected.Admin.CarrierList" %>
<%@ Register Src="~/UserControl/Admin/ucCarrierList.ascx" TagName="ucCarrierList" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="paneContent">
		<div class="page-title">
			<%--Insurers/Carriers--%> 
            Clients
		</div>

		
		<asp:UpdatePanel ID="updatePanel" runat="server">
			<ContentTemplate>
				<uc1:ucCarrierList ID="ucCarrierList1" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
</asp:Content>
