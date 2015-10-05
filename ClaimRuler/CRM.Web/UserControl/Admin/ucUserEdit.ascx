<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUserEdit.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucNewUser" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:HiddenField ID="hfUserId" runat="server" Value="0" />




<div class="page-title">
    <asp:Label ID="lblheading" runat="server" />
</div>


<div class="toolbar toolbar-body">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnUserList" runat="server" CssClass="toolbar-item" PostBackUrl="~/protected/admin/UserList.aspx" Visible='<%# Convert.ToBoolean(masterPage.hasViewPermission) %>'>
						<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">User List</span>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<div class="paneContentInner">
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>


            <div class="message_area">
                <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                <asp:Label ID="lblMessage" runat="server" CssClass="redstar" Visible="false" />
            </div>
            <igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="600px" TitleAlignment="Left" Text="User Settings" CssClass="canvas">

                <Template>


                    <table style="width: 100%; border: none; border-spacing: 5px; padding: 2px;" border="0" class="editForm">
                        <tr>
                            <td class="right" style="width: 20%; border: none;">
                                <label>
                                    First Name
                                </label>
                            </td>
                            <td class="redstar">*</td>
                            <td class="disabletxt">
                                <ig:WebTextEditor runat="server" ID="txtFirstName" MaxLength="50" />
                                <br />
                                <asp:RequiredFieldValidator runat="server" ID="reqFirstName" ControlToValidate="txtFirstName"
                                    ErrorMessage="Please enter first Name" CssClass="validation1" ValidationGroup="register"
                                    Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" />
                            </td>
                            <td rowspan="4" class="top left">
                                <div class="boxContainer" style="width:85px;">
                                    <div class="paneContentInner">
                                        <asp:Image ID="userPhoto" runat="server" Width="75px" Height="75px"
                                            ImageUrl="~/Images/user-thumbnail.png" ImageAlign="Middle" />

                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">
                                <label>
                                    Last Name
                                </label>
                            </td>
                            <td class="redstar"></td>
                            <td class="disabletxt">
                                <ig:WebTextEditor runat="server" ID="txtLastName" MaxLength="50" />
                                <br />
                                <asp:RequiredFieldValidator runat="server" ID="rfvLastName" ControlToValidate="txtLastName"
                                    ErrorMessage="Please enter last Name" CssClass="validation1" ValidationGroup="register"
                                    Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px; text-align: right;">
                                <label>
                                    Status
                                </label>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>

                                <asp:DropDownList runat="server" ID="ddlStatus">
                                    <asp:ListItem Text="Active" Value="1" />
                                    <asp:ListItem Text="In-Active" Value="0" />
                                </asp:DropDownList>

                            </td>

                        </tr>
                        <tr>
                            <td class="right">
                                <label>
                                    Role
                                </label>
                            </td>
                            <td class="redstar">*
                            </td>
                            <td>

                                <asp:DropDownList ID="ddlRole" runat="server">
                                </asp:DropDownList>

                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="requiredFieldValidator6" ControlToValidate="ddlRole"
                                        ErrorMessage="Please select Role." ValidationGroup="register" Display="Dynamic"
                                        CssClass="validation1" InitialValue="0" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">View All Claims</td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="cbxViewAllClaims" runat="server" />
                            </td>
                        </tr>
                        <tr id="tr_client" runat="server" visible="false">
                            <td class="right">Client ID</td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddlClient" runat="server" />
                            </td>
                        </tr>



                        <tr>
                            <td class="right">
                                <label>
                                    Email
                                </label>
                            </td>
                            <td class="redstar">*
                            </td>
                            <td class="disabletxt">
                                <ig:WebTextEditor runat="server" ID="txtEmail" MaxLength="100" />
                                <div>
                                    <span class="validationSpan">
                                        <asp:RequiredFieldValidator ID="r1" runat="server" ErrorMessage="*Please Enter Email"
                                            Display="Dynamic" EnableClientScript="true" ValidationGroup="register" ControlToValidate="txtEmail"
                                            CssClass="validation1" SetFocusOnError="true" />
                                        <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="Email not valid!" ValidationGroup="register" Display="Dynamic"
                                            EnableClientScript="true" CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            SetFocusOnError="true"></asp:RegularExpressionValidator></span>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td class="right">
                                <label>
                                    User Name
                                </label>
                            </td>
                            <td class="redstar">*
                            </td>
                            <td class="disabletxt">
                                <ig:WebTextEditor runat="server" ID="txtUserName" MaxLength="50" autocomplete="off" onkeypress="javascript:return isBlankSpace(event,this);" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="reqUserName" ControlToValidate="txtUserName"
                                        ErrorMessage="Please enter user name." CssClass="validation1" ValidationGroup="register"
                                        Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" />
                                </div>
                            </td>

                        </tr>
                        <tr>
                            <td class="right">
                                <label>
                                    Password
                                </label>
                            </td>
                            <td class="redstar">*
                            </td>
                            <td class="disabletxt">
                                <asp:TextBox runat="server" ID="txtPassWord" onkeypress="javascript:show();" MaxLength="50"
                                    autocomplete="off" TextMode="Password"></asp:TextBox>
                                <br />
                                <span class="validationSpan">
                                    <asp:RequiredFieldValidator runat="server" ID="reqPassword" ControlToValidate="txtPassWord"
                                        ErrorMessage="Please enter password" CssClass="validation1" ValidationGroup="register"
                                        Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" /></span>
                                <div id="strength" runat="server">
                                    <span id="result" class="validation1"></span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">
                                <label>
                                    Confirm-Password
                                </label>
                            </td>
                            <td class="redstar">*
                            </td>
                            <td class="disabletxt">
                                <asp:TextBox runat="server" ID="txtRePassWord" MaxLength="50" autocomplete="off"
                                    TextMode="Password"></asp:TextBox><br />
                                <span class="validationSpan">
                                    <asp:RequiredFieldValidator runat="server" ID="reqrePassword" CssClass="validation1"
                                        ControlToValidate="txtRePassWord" ErrorMessage="Please confirm password."
                                        ValidationGroup="register" Display="Dynamic" SetFocusOnError="true" />
                                    <asp:CompareValidator runat="server" ID="comparePassword" ControlToValidate="txtPassWord"
                                        CssClass="validation1" ValidateEmptyText="true" ControlToCompare="txtRePassWord"
                                        ErrorMessage="Password must match!" ValidationGroup="register" EnableClientScript="true"
                                        Display="Dynamic" /></span>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPassword" Checked="false" Text="Set Password" Enabled="false"
                                    runat="server" onClick="javascript:return checkPass(this);" />
                            </td>


                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center;">
                                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" CssClass="mysubmit" />
                                &nbsp;
								<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="mysubmit" ValidationGroup="register" CausesValidation="true" />
                                &nbsp;                                
				                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="mysubmit" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </Template>
            </igmisc:WebGroupBox>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>



<script type="text/javascript">
	$(document).ready(function () {

		var txtps = document.getElementById("<%=txtPassWord.ClientID %>").id;

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
	

    function checkPass(val) {
    	if (val.checked == true) {
    		if (document.getElementById("<%=txtPassWord.ClientID %>").value != "*****") {
        		document.getElementById("<%=strength.ClientID %>").style.display = "block";
            }
	  	document.getElementById("<%=txtPassWord.ClientID %>").disabled = false;
        	document.getElementById("<%=txtRePassWord.ClientID %>").disabled = false;
        	document.getElementById("<%=txtPassWord.ClientID %>").value = "";
        	document.getElementById("<%=txtRePassWord.ClientID %>").value = "";
        	document.getElementById("<%=txtPassWord.ClientID %>").focus();
        	ValidatorEnable(document.getElementById('<%= reqPassword.ClientID %>'), true);
        	ValidatorEnable(document.getElementById('<%= reqrePassword.ClientID %>'), true);
        	ValidatorEnable(document.getElementById('<%= comparePassword.ClientID %>'), true);
        }
        else {
        	document.getElementById("<%=strength.ClientID %>").style.display = "none";
        	document.getElementById("<%=txtPassWord.ClientID %>").disabled = true;
        	document.getElementById("<%=txtRePassWord.ClientID %>").disabled = true;
        	ValidatorEnable(document.getElementById('<%= reqPassword.ClientID %>'), false);
        	ValidatorEnable(document.getElementById('<%= reqrePassword.ClientID %>'), false);
        	ValidatorEnable(document.getElementById('<%= comparePassword.ClientID %>'), false);
        	document.getElementById("<%=txtPassWord.ClientID %>").value = "*****";
    	    document.getElementById("<%=txtRePassWord.ClientID %>").value = "*****";
    	}
        return true;
    }

    function show() {
        document.getElementById("<%=strength.ClientID %>").style.display = "block";
   }

</script>
<script type="text/javascript">

    function isBlankSpace(evt, ctr) {


        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode == 32)
            return false;
        return true;
    }

</script>
