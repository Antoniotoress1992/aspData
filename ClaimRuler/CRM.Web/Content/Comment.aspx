<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Comment.aspx.cs" Inherits="CRM.Web.Content.Comment" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
	<head />
	<script type="text/javascript">
		function closeRefresh() {
			// call function in parent window		
			window.opener.refreshComments();
			window.close();

		}
	</script>

	<div class="paneContent">
		<div class="section-title">
			New Comment
		</div>
		<h2>
			<%= Session["ClaimantFirstName"]%>
			<%= Session["ClaimantLastName"]%>
		</h2>
		<div class="toolbar toolbar-body">
			<table>
				<tr>
					<td>
						<asp:LinkButton ID="btnSend1" runat="server" Text="" CssClass="toolbar-item" OnClick="btnSave_click">
									<span class="toolbar-img-and-text" style="background-image: url(../images/toolbar_save.png)">Save</span>
						</asp:LinkButton>
					</td>
					<td>
						<asp:LinkButton ID="btnCancel1" runat="server" Text="Return to Claim" CssClass="toolbar-item" OnClientClick="javascript:window.close();">
									<span class="toolbar-img-and-text" style="background-image: url(../images/cancel.png)">Cancel</span>
						</asp:LinkButton>
					</td>
				</tr>
			</table>
		</div>
		<ighedit:WebHtmlEditor ID="txtComment" runat="server" Width="100%" Height="400px">
			<TabStrip Enabled="false" />
		</ighedit:WebHtmlEditor>


	</div>
</asp:Content>

