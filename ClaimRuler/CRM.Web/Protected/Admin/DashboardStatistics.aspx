<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true"
	CodeBehind="DashboardStatistics.aspx.cs" Inherits="CRM.Web.Protected.Admin.DashboardStatistics" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">	
	<h2>
		Dashboard</h2>
	<asp:Panel ID="pnlStatistics" runat="server" Width="400px" Height="200px" CssClass="panel_roundcorner">
		<div class="panel_header">Claim Statistics</div>
		<table cellpadding="2" cellspacing="3" width="90%">
			<tr>
				<td style="width: 30%">
					Open Claims
				</td>
				<td align="right">
					<asp:Label ID="lblOpenLeadCount" runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					Closed Claims
				</td>
				<td align="right">
					<asp:Label ID="lblCloseLeadCount" runat="server" />
				</td>
			</tr>
		</table>
	</asp:Panel>
	<ajaxtoolkit:RoundedCornersExtender ID="rce" runat="server" TargetControlID="pnlStatistics"
		Radius="8" Corners="All"  />
</asp:Content>
