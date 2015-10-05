<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="EmailSettings.aspx.cs" Inherits="CRM.Web.Protected.Admin.EmailSettings" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">	

	<div class="paneContent">
		<div class="page-title">
			Email Settings
		</div>
		<div class="toolbar toolbar-body">
			<table>
				<tr>
					<td>
						<asp:LinkButton ID="btnSave" runat="server" Text="Send" CssClass="toolbar-item" OnClick="btnSave_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_save.png)">Save</span>
						</asp:LinkButton>
					</td>
					<td></td>
				</tr>
			</table>
		</div>
		<igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="800px" TitleAlignment="Left" Text="Current Settings" CssClass="canvas">

			<Template>

				<table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px;" border="0" class="editForm nowrap">
					<tr>
						<td colspan="7" style="text-align: center;">
							<asp:Label ID="lblMessage" runat="server" CssClass="ok" Visible="false" />
						</td>
					</tr>
					<tr>
						<td class="right">
							<label>Email Provider</label></td>
						<td></td>
						<td>
							<select id="email_provider">
								<option value="">Select One</option>
								<option value="gmail">Gmail</option>
								<option value="hotmail">HotMail</option>
								<option value="comcast">Comcast</option>
								<option value="godaddy">GoDaddy</option>
								<option value="securenetsystems">SecureNet Systems</option>
							</select>
						</td>
					</tr>
					<tr>
						<td></td>
					</tr>
					<tr>
						<td class="right">
							<label>
								Email Host Server
							</label>
						</td>
						<td></td>
						<td>
                            
							 <ig:WebTextEditor ID="txtEmailHost" runat="server" Width="200px" />
						</td>
					</tr>
					<tr>
						<td class="right">
							<label>Email Host Port</label></td>
						<td></td>
						<td>                           
							<ig:WebNumericEditor ID="txtHostPostNumber" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="right">
							<label>Use Secured Connection</label></td>
						<td></td>
						<td>
							<asp:CheckBox ID="cbxSSL" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="right">
							<label>
								Email
							</label>
						</td>
						<td></td>
						<td>
							<ig:WebTextEditor ID="txtEmail" runat="server" Width="200px" />
							<span class="validationSpan">
								<asp:RequiredFieldValidator ID="r1" runat="server" ErrorMessage="*Please enter email."
									Display="Dynamic" EnableClientScript="true" ValidationGroup="register" ControlToValidate="txtEmail"
									CssClass="validation" SetFocusOnError="true" />
								<asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
									ErrorMessage="*Email not valid!" ValidationGroup="register" Display="Dynamic"
									EnableClientScript="true" CssClass="validation" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
									SetFocusOnError="true"></asp:RegularExpressionValidator></span>
						</td>
					</tr>
					<tr>
						<td class="right">
							<label>Email Password</label></td>
						<td></td>
						<td>
							<ig:WebTextEditor ID="txtEmailPassword" runat="server" TextMode="Password" MaxLength="20"
								autocomplete="off" />
							<asp:HiddenField ID="hf_pswd" runat="server" />
						</td>
					</tr>
					<tr>
						<td class="right top">
							<label>Signature</label></td>
						<td></td>
						<td colspan="6">
							<ighedit:WebHtmlEditor ID="txtEmailSignature" runat="server"></ighedit:WebHtmlEditor>
						</td>
					</tr>
				</table>


			</Template>
		</igmisc:WebGroupBox>
	</div>
	<script type="text/javascript">
		$(document).ready(function () {

			$("#email_provider").change(function () {
				var provider = $('#email_provider').val();
				switch (provider) {
					case "gmail":
						$("#<%= txtEmailHost.ClientID %>").val("smtp.gmail.com");
						$("#<%= txtHostPostNumber.ClientID %>").val("587");
						break;

					case "hotmail":
						$("#<%= txtEmailHost.ClientID %>").val("smtp.live.com");
						$("#<%= txtHostPostNumber.ClientID %>").val("587");
						$("#<%= cbxSSL.ClientID %>").prop('checked', true);
						break;

					case "comcast":
						$("#<%= txtEmailHost.ClientID %>").val("smtp.comcast.net");
						$("#<%= txtHostPostNumber.ClientID %>").val("587");
						$("#<%= cbxSSL.ClientID %>").prop('checked', true);
						break;

					case "godaddy":
						$("#<%= txtEmailHost.ClientID %>").val("smtpout.secureserver.net");
						$("#<%= txtHostPostNumber.ClientID %>").val("80");
						$("#<%= cbxSSL.ClientID %>").prop('checked', false);
						break;

					case "securenetsystems":
						$("#<%= txtEmailHost.ClientID %>").val("mail.securenetsystems.net");
						$("#<%= txtHostPostNumber.ClientID %>").val("587");
						$("#<%= cbxSSL.ClientID %>").prop('checked', false);
						break;

					default:
						$("#<%= txtEmailHost.ClientID %>").val("");
						$("#<%= txtHostPostNumber.ClientID %>").val("");
						$("#<%= cbxSSL.ClientID %>").prop('checked', false);
						break;
				}

			});
		});


	</script>
</asp:Content>
