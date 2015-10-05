<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucChangePassword.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.ucChangePassword" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link rel="stylesheet" type="text/css" href="../../js/password_strength/style.css" />
<script type="text/javascript">
    $(document).ready(function () {

        $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ucChangePassword1_WebGroupBox1_confirmPasswordTextbox").hide();

		var txtps = document.getElementById("<%=txtNewPassword.ClientID %>").id;

		$("#" + txtps).keyup(function () {
			$('#result').html(checkStrength($('#' + txtps).val()))
		})

		function checkStrength(password) {
			//initial strength
			var strength = 0

			if (password.length == 0) {
				$('#result').removeClass()
				//$('#result').addClass('weak')
				return ''
			}
			//if the password length is less than 6, return message.
			if (password.length < 6) {
				$('#result').removeClass()
				$('#result').addClass('short')
				return 'Too short'
			}

			//length is ok, lets continue.

			//if length is 8 characters or more, increase strength value
			if (password.length > 7) strength += 1

			//if password contains both lower and uppercase characters, increase strength value
			if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1

			//if it has numbers and characters, increase strength value
			if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1

			//if it has one special character, increase strength value
			if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

			//if it has two special characters, increase strength value
			if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1

			//now we have calculated strength value, we can return messages

			//if value is less than 2
			if (strength < 2) {
				$('#result').removeClass()
				$('#result').addClass('weak')
				return 'Weak'
			} else if (strength == 2) {
				$('#result').removeClass()
				$('#result').addClass('good')
				return 'Good'
			} else {
				$('#result').removeClass()
				$('#result').addClass('strong')
				return 'Strong'
			}

		}

	});
</script>
<script type="text/javascript">

	function isBlankSpace(evt, ctr) {

		var charCode = (evt.which) ? evt.which : event.keyCode
		if (charCode == 32)
			return false;
		return true;
	}
</script>
<div class="page-title">
	Change Password
</div>
<div class="paneContentInner">
	<div style="padding-top: 10px; padding-bottom: 10px;">
		<asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
		<asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
		<asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
	</div>
	<igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="600px" TitleAlignment="Left" Text="Change Password" CssClass="canvas">

		<Template>

			<table style="width: 100%; border: none; border-spacing: 8px; padding: 2px;" border="0" class="editForm">
				<tr>
					<td class="right">Old Password
					</td>
					<td class="redstar">*
					</td>
					<td>
						<asp:TextBox runat="server" class="login_st" ID="txtOldPassword" TextMode="Password"
							AutoComplete="off" MaxLength="10" />
						<asp:RequiredFieldValidator runat="server" ID="reqoldPassword" ControlToValidate="txtOldPassword"
							ErrorMessage="*Please Enter Old  Password" CssClass="validation1" SetFocusOnError="true"
							ValidationGroup="resetPassword" Display="Dynamic" EnableClientScript="true" />
					</td>
				</tr>

				<tr>
					<td class="right">New Password</td>
					<td class="redstar">*</td>
					<td>
						<asp:TextBox runat="server" class="login_st" ID="txtNewPassword" TextMode="Password"
							AutoComplete="off" MaxLength="10" />
						<asp:RequiredFieldValidator runat="server" ID="reqPassword" ControlToValidate="txtNewPassword"
							ErrorMessage="*Please enter Password" CssClass="validation1" SetFocusOnError="true"
							ValidationGroup="resetPassword" Display="Dynamic" EnableClientScript="true" />
					</td>
					<td>
						<span id="result" class="short"></span>
					</td>
				</tr>
				<tr>
					<td class="right">Confirm Password</td>
					<td class="redstar">*
					</td>
					<td>
						<asp:TextBox runat="server" ID="txtConPassword" class="login_st" TextMode="Password"
							AutoComplete="off" MaxLength="10" />
						<asp:RequiredFieldValidator runat="server" ID="reqRePassword" ControlToValidate="txtConPassword"
							ErrorMessage="*Please enter Password" CssClass="validation1" SetFocusOnError="true"
							ValidationGroup="resetPassword" Display="Dynamic" EnableClientScript="true" />
						<asp:CompareValidator runat="server" ID="confirmPasswordTextbox" ControlToValidate="txtConPassword"
							CssClass="validation1" ValidateEmptyText="true" ControlToCompare="txtNewPassword"
							ErrorMessage="Password must match!" ValidationGroup="resetPassword" Display="Dynamic" />
					</td>
				</tr>
				<tr>
					<td colspan="4" style="text-align: center;">
						<br />
						<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" TabIndex="16"
							ValidationGroup="resetPassword" OnClick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnCancel" CausesValidation="false" Text="Cancel" runat="server"
					TabIndex="17" CssClass="mysubmit" OnClick="btnCancel_Click" />
					</td>

				</tr>
			</table>
		</Template>
	</igmisc:WebGroupBox>
</div>
