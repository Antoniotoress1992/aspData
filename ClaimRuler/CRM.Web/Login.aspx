<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="CRM.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Claim Ruler - Industrial Strength Property Claim Management Software</title>
    <link href="Css/login.css" rel="stylesheet" type="text/css" />
    <link href="Css/new.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="Css/ddsmooth.css" />
    <link type="text/css" rel="stylesheet" href="Css/jscal2.css" />
    <link type="text/css" rel="stylesheet" href="Css/border-radius.css" />
    <link id="skin-steel" title="Steel" type="text/css" rel="alternate stylesheet" href="Css/steel.css" />
    <style type="text/css">
		.redstar
		{
			font-family: "Times New Roman" , Times, serif;
			font-size: 10px;
			color: red; /*width:10px;*/
		}
	</style>
    <link rel="shortcut icon" href="/favicon.ico" />
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
</head>
<body>
    <div class="mybox reverse">

        <div style="text-align: center; margin: auto;">
            <h1>
                <a href="#">
                    <img src="images/logo.jpg" alt="" width="200" /></a></h1>
        </div>

        <div class="line">
        </div>
    </div>
    <form id="loginform" name="loginform" runat="server">
        <div class="content">
            <div class="all_admin_boxs">
                <div class="all_feld">
                    <div id="user_name" class="username">
                        <div class="head">
                            Login
                        </div>
                        <div class="" style="width: 420px;">
                            <asp:Label runat="server" ID="lblError" Width="420px" CssClass="error" Visible="false" />
                        </div>
                        <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">
                            <table width="430" border="0" cellspacing="0" cellpadding="00">
                                <tr>
                                    <td width="88px">&nbsp;
                                    </td>
                                    <td width="12px">&nbsp;
                                    </td>
                                    <td width="261px">&nbsp;
                                    </td>
                                    <td width="39px">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 5px;">Username:
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" class="myinput" TabIndex="1" />
                                        <asp:RequiredFieldValidator runat="server" ID="req1" ControlToValidate="txtUserName"
                                            ErrorMessage="*Please Enter Login" ValidationGroup="Login" Display="Dynamic"
                                            CssClass="redstar" />
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 5px;">Password:
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" class="myinput" TextMode="Password" TabIndex="2" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPWD" ControlToValidate="txtPassword"
                                            ErrorMessage="*Please Enter Password" ValidationGroup="Login" Display="Dynamic"
                                            CssClass="redstar" />
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <table width="226" border="0" cellspacing="0" cellpadding="00">
                                            <tr>
                                                <td width="226" align="center">
                                                    <asp:Button ID="btnLogin" runat="server" class="mysubmit" Text="I Agree" ValidationGroup="Login"
                                                        OnClick="btnSubmit_click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="226" align="center"></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="mybox">
            <div class="text_box_all">
                <br>
                Security Warning: You are attempting to access a private computer system. Unauthorized
			access is prohibited. System use is monitored and recorded, therefore users should
			have no expectation of personal privacy when using this system. Anyone accessing
			this system agrees to use the system only as authorized and expressly consents to
			such monitoring. ITSG reserves the right to access, use and disclose any and all
			information on the system as provided or allowed by federal or state law.
			<br />
                <br />
                Confidentiality Notice: This system contains confidential information belonging
			to ITSG. It may also be privileged or otherwise protected by work product immunity
			or other legal rules. This information is intended only for the use of the individual
			or entity named above. If you are not the intended user, you are hereby notified
			that any disclosure, copying, distribution or the taking of any action in reliance
			on the contents of this confidential information is strictly prohibited.
			<br />
                <br />
                You agree to comply with <a href="https://www.claimruler.com/uploads/image/Claim_Ruler_Online_User_Agreement_and_Terms_-_June_2013_Creation_Date-updated.pdf" target="_blank">all terms of use</a>,
			confidentiality and security requirements that ITSG may impose for use of their
			implementation of the ITSG Claim Ruler Claims Management System, and you agree not
			to attempt to circumvent such security requirements. By clicking "Log In" you are
			agreeing to these terms.
			<br />
                <br />
            </div>
        </div>
        <asp:HiddenField ID="hfmember_id" runat="server" Value="" />
        <asp:HiddenField ID="hflogin_name" runat="server" Value="" />
        <asp:HiddenField ID="hfadmin_user" runat="server" Value="" />
        <asp:HiddenField ID="hfdisable_user_editing" runat="server" Value="" />
    </form>
</body>
</html>
