<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="LeadEmailBroadcast.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadEmailBroadcast" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

	<div class="paneContent">
		<div class="page-title">
			Client Marketing - Broadcast All Clients
		</div>
		<div class="toolbar toolbar-body">
			<table>
				<tr>
					<td>
						<asp:LinkButton ID="btnSend1" runat="server" Text="Send" CssClass="toolbar-item" OnClick="btnSend_Click" ValidationGroup="EmailGroup" >
									<span class="toolbar-img-and-text" style="background-image: url(../../images/mail_send.png)">Send</span>
						</asp:LinkButton>
					</td>
				</tr>
			</table>
		</div>

		<div class="message_area">
			<asp:Label ID="lblMessage" runat="server" />
		</div>
		<div class="paneContentInner">
			<table style="width: 95%; border-collapse: separate; border-spacing: 2px; padding: 3px;" border="0" class="editForm">
				<tr>
					<td style="width: 15%" class="right">Subject:</td>
					<td>
						<asp:TextBox ID="txtEmailSubject" runat="server" Width="90%" />
						<div>
							<asp:RequiredFieldValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmailSubject"
								Display="Dynamic" SetFocusOnError="true" ErrorMessage="Please enter email subject."
								CssClass="validation1" ValidationGroup="EmailGroup" />
						</div>
					</td>
				</tr>
				<!--
		<tr>
			<td align="right" valign="top">
				Attachments:				
			</td>
			<td valign="top">
				<asp:FileUpload ID="fileUpload" runat="server" CssClass="mysubmit" Width="300px" />
			</td>
		</tr>
		-->
				<tr>
					<td class="right top"></td>
					<td>
						<ighedit:WebHtmlEditor ID="txtEmailText" runat="server" Width="90%" Height="550px"></ighedit:WebHtmlEditor>
					</td>
				</tr>
				
			</table>
		</div>
	</div>
</asp:Content>
