<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucApplicationConfiguration.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucApplicationConfiguration" %>
<script type="text/javascript">
    function isNumberKeyConfig(evt, ctr, exp, dec) {
        if (isTextSelected(ctr)) ctr.value = "";
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode == 46 && ctr.value.indexOf('.') < 0 && Number(dec) > 0)
            return true;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        if (ctr.value.indexOf('.') < 0 && String(ctr.value).length == Number(exp))
            return false;
        if (ctr.value.indexOf('.') >= 0) {
            var decindex = ctr.value.indexOf('.');
            var decstr = String(ctr.value).substr(decindex + 1, ctr.value.length - decindex);
            if (decstr.length == dec)
                return false;
        }
        return true;
    }

    function isTextSelected(input) {
        if (typeof input.selectionStart == "number") {
            return input.selectionStart == 0 && input.selectionEnd == input.value.length;
        }
        else if (typeof document.selection != "undefined") {
            input.focus();
            return document.selection.createRange().text == input.value;
        }
    }


</script>
<div id="mainboxs">
    <div class="all_box">
        <div class="box1">
            <div class="mainbox">
                <h2>
                    Application Configuration</h2>
                <div class="warrape">
                    <table style="width: 100%; padding-left: 20px; padding-top: 20px;" border="0" cellspacing="0"
                        cellpadding="00" class="new_user">
                        <tr>
                            <td>
                                Primery Producer Commission(%)
                            </td>
                            <td class="redstar">
                                *
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtPrimeryProducer" class="login_st" AutoComplete="off"
                                    onkeypress="javascript:return isNumberKeyConfig(event,this,5,2);" />
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtPrimeryProducer"
                                    ErrorMessage="*Please Enter" CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig"
                                    Display="Dynamic" EnableClientScript="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="redstar">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Secondary Producer Commission(%)
                            </td>
                            <td class="redstar">
                                *
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSecondaryProducer" class="login_st" AutoComplete="off"
                                    onkeypress="javascript:return isNumberKeyConfig(event,this,5,2);" />
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtSecondaryProducer"
                                    ErrorMessage="*Please Enter" CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig"
                                    Display="Dynamic" EnableClientScript="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="redstar">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div style="padding-bottom: 20px;">
                                    <h4 style="border-bottom: 1px dashed #999; color: #184687; font-family: 'Trebuchet MS',Arial,Helvetica,sans-serif;
                                        font-size: 18px; line-height: 23px; margin: 0 0 5px; padding: 0px 0px 0px 0px;
                                        text-align: left; text-decoration: none; text-shadow: 0 1px 0 white; padding: 3px;">
                                        Notification Email IDs:-
                                    </h4>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px; align: left;">
                                Default
                            </td>
                            <td class="redstar" style="width: 10px; align: left;">
                                *
                            </td>
                            <td style="align: left;">
                                <asp:TextBox runat="server" ID="txtEmailSite1" TextMode="MultiLine" Width="470px"
                                    Height="55px" AutoComplete="off" />
                                <asp:RequiredFieldValidator runat="server" ID="reqoldPassword" ControlToValidate="txtEmailSite1"
                                    ErrorMessage="*Please Enter" CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig"
                                    Display="Dynamic" EnableClientScript="true" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtEmailSite1"
                                    CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig" Text="Invalid address"
                                    ValidationExpression="^(\s*,?\s*[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})+\s*$"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="align: left;">
                                &nbsp;
                            </td>
                            <td class="redstar" style="width: 10px; align: left;">
                                &nbsp;
                            </td>
                            <td style="align: left;">
                                &nbsp;
                            </td>
                        </tr>
				    <!--
                        <tr>
                            <td>
                                Atlantic
                            </td>
                            <td class="redstar">
                                *
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmailSite2" TextMode="MultiLine" Width="470px"
                                    Height="55px" AutoComplete="off" />
                                <asp:RequiredFieldValidator runat="server" ID="reqPassword" ControlToValidate="txtEmailSite2"
                                    ErrorMessage="*Please Enter" CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig"
                                    Display="Dynamic" EnableClientScript="true" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtEmailSite2"
                                    CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig" Text="Invalid address"
                                    ValidationExpression="^(\s*,?\s*[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})+\s*$"
                                    runat="server" />
                            </td>
                        </tr>
				    
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="redstar">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Rainlaw
                            </td>
                            <td class="redstar">
                                *
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtEmailSite3" TextMode="MultiLine" Width="470px"
                                    Height="55px" AutoComplete="off" />
                                <asp:RequiredFieldValidator runat="server" ID="reqRePassword" ControlToValidate="txtEmailSite3"
                                    ErrorMessage="*Please Enter" CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig"
                                    Display="Dynamic" EnableClientScript="true" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtEmailSite3"
                                    CssClass="validation1" SetFocusOnError="true" ValidationGroup="appConfig" Text="Invalid address"
                                    ValidationExpression="^(\s*,?\s*[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})+\s*$"
                                    runat="server" />
                            </td>
                        </tr>
				    -->
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td class="redstar">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td width="112">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" TabIndex="16"
                                    ValidationGroup="appConfig" OnClick="btnSave_Click" />&nbsp;
                                <asp:Button ID="btnCancel" CausesValidation="false" Text="Cancel" runat="server"
                                    TabIndex="17" CssClass="mysubmit" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="1">
                                <div style="padding-top: 10px; padding-bottom: 10px;">
                                    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                                    <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                                    <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
