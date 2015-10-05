<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMenu.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucMenu" %>
<style>
	a
	{
		text-decoration: none;
	}
	
	a img
	{
		border: none;
	}
</style>
<script langugae="javascript">
<!--
	$(document).ready(function () {
		var hv = $('#<%=hfMenu.ClientID %>').val();
	});
-->
</script>
<table align="center" border="0" cellspacing="0" cellpadding="00">
	<tr>
		<td>
			<table width="970" border="0" cellspacing="0" cellpadding="00">
				<tr>
					<td align="center">
						<table width="100%" border="0" cellspacing="0" cellpadding="0" class="logo_cont">
							<tr>
								<td align="right">
									<div class="mylogo">
										<h1>
											<a href="#">
												<img src="../../images/logo.jpg" alt="" width="150" /></a></h1>
									</div>
								</td>
								<td width="31%" align="right" valign="middle">
									<br />
									<span style="line-height: 45px; margin-right: 10px; font-weight: bold;">
										<asp:Label ID="lblUserWelcome" runat="server"></asp:Label></span>
									<asp:LinkButton ID="lbtnSignout" runat="server" CssClass="report fright" Text="Sign Out"
										OnClick="logOut_Click" />
								</td>
							</tr>
							<tr>
								<td width="49%">
								</td>
								
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
		</td>
	</tr>
	<tr>
		<tr>
			<td valign="top">
				<input id="hfMenu" type="hidden" runat="server" value="" />
				<div id="dvMenu" runat="server" class="ddsmoothmenu">
				</div>
			</td>
		</tr>
</table>
