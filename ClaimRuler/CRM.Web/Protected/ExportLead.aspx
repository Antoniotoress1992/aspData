<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true"
	CodeBehind="ExportLead.aspx.cs" Inherits="CRM.Web.Protected.ExportLead" %>

<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>

<%@ Register Src="~/UserControl/Admin/ucPolicyType.ascx" TagName="ucPolicyType"
	TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
	Export
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
			<td>
				<asp:LinkButton ID="btnExport" runat="server" Text="Export" CssClass="toolbar-item" OnClick="btnExport_Click">
					<span class="toolbar-img-and-text" style="background-image: url(../images/export.png)">Export</span>
				</asp:LinkButton>
			</td>
	
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

	<asp:UpdatePanel ID="updatePanel" runat="server">
		<ContentTemplate>
			<div class="message_area">
				<asp:Label ID="lblMessage" runat="server" />
			</div>
			<igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="800px" TitleAlignment="Left" Text="Export Details" CssClass="canvas">
				<Template>
					<table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px;" border="0" class="editForm">

						<tr>
							<td class="right">Email To</td>
							<td class="redstar" style="width: 5px;">*</td>
							<td>
								<asp:TextBox ID="txtEmailTo" runat="server" Width="90%" ToolTip="Use comma or semi-colon when entering multiple recipients." />
								<span>
									<asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmailTo"
										ErrorMessage="Invalid email address." ValidationGroup="register" Display="Dynamic"
										EnableClientScript="true" CssClass="validation1" ValidationExpression="^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4};?)+$"
										SetFocusOnError="true"></asp:RegularExpressionValidator>
								</span>
							</td>
						</tr>

						<tr>
							<td style="width: 20%" class="right">Select Coverage Type:</td>
							<td class="redstar">*</td>
							<td class="left">
								<asp:DropDownList ID="ddlPolicyType" runat="server"></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td class="left">
								<asp:CheckBox ID="cbxClaimLog" runat="server" Text=" Claim Log" />
							</td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td class="left">
								<asp:CheckBox ID="cbxPhotos" runat="server" Text=" Photos" />
							</td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td class="left">
								<asp:CheckBox ID="cbxDocuments" runat="server" Text=" Documents" />
							</td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td class="left">
								<asp:CheckBox ID="cbxDemographics" runat="server" Text=" Demographics" />
							</td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td class="left">
								<asp:CheckBox ID="cbxCoverage" runat="server" Text=" Coverage" />
							</td>
						</tr>

					</table>
				</Template>
			</igmisc:WebGroupBox>
			
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
