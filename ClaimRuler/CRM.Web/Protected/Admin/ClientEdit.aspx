<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
    CodeBehind="ClientEdit.aspx.cs" Inherits="CRM.Web.Protected.Admin.ClientEdit" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <div class="paneContent">
        <div class="page-title">
            Edit Client
        </div>
        <asp:Panel ID="pnlClient" runat="server" DefaultButton="btnSave">
            <div class="toolbar toolbar-body">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="toolbar-item" OnClick="btnSave_Click" ValidationGroup="Client" CausesValidation="true"> 
									<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_save.png)">Save</span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="toolbar-item" OnClick="btnCancel_Click" CausesValidation="false">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/cancel.png)">Cancel</span>
                            </asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnImportSettings" runat="server" CssClass="toolbar-item" OnClick="btnImportSettings_Click" CausesValidation="true">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/settings.png)">Import Settings</span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="paneContentInner">
                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <table style="width: 100%;">
                    <tr>
                        <td class="left top" style="width: 50%;">
                            <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                <tr>
                                    <td class="right">Client Type</td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <asp:DropDownList ID="ddlClientType" runat="server">
                                            <asp:ListItem Value="0" Text="--- Select ---"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Public Adjuster"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Independent Adjuster"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                        <div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlClientType" ValidationGroup="Client"
                                                CssClass="validation1" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please select client type." />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">First Name
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtFirstName" runat="server" Width="200px" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="rfvClientname" runat="server" ControlToValidate="txtFirstName" ValidationGroup="Client"
                                            CssClass="validation1" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please enter first name." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Last Name
                                    </td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <ig:WebTextEditor ID="txtLastName" runat="server" Width="200px" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName" ValidationGroup="Client"
                                            CssClass="validation1" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please enter last name." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Company Name
                                    </td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBusinessName" runat="server" Width="400px" MaxLength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Address
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtAddress" runat="server" Width="400px" MaxLength="250" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">State
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_selectedIndex"
                                            AutoPostBack="true" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvstate" ControlToValidate="ddlState" ValidationGroup="Client"
                                            ErrorMessage="Please select state" Display="Dynamic" CssClass="validation1"
                                            InitialValue="0" SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">City
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dllCity_SelectedIndexChanged" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlCity" ValidationGroup="Client"
                                            ErrorMessage="Please select city" Display="Dynamic" CssClass="validation1" InitialValue="0"
                                            SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Zip Code
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlZipCode" runat="server" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlZipCode"
                                            ErrorMessage="Please select zip code" Display="Dynamic" CssClass="validation1" ValidationGroup="Client"
                                            InitialValue="0" SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Phone Number
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtPhone" runat="server" MaxLength="20" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtPhone"
                                            ErrorMessage="Please enter phone number." Display="Dynamic" CssClass="validation1" ValidationGroup="Client"
                                            SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Fax Number
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtFaxNumber" runat="server" MaxLength="35" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Email
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtEmail" runat="server" MaxLength="200" Width="400px" />
                                        <span>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtEmail"
                                                ErrorMessage="Please enter email." Display="Dynamic" CssClass="validation1" ValidationGroup="Client"
                                                SetFocusOnError="True" />
                                            <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
                                                ErrorMessage="Invalid email address." ValidationGroup="Client" Display="Dynamic"
                                                EnableClientScript="true" CssClass="validation" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                SetFocusOnError="true"></asp:RegularExpressionValidator>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">User Name
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtUserName" runat="server" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtUserName" ValidationGroup="Client"
                                            ErrorMessage="Please user name." Display="Dynamic" CssClass="validation1" SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">User Password
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtUserPassword" runat="server" TextMode="Password" MaxLength="20" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Maximum Number of Users Allowed
                                    </td>
                                    <td class="redstar">*
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtNumberUsers" runat="server" TextMode="Number" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="txtNumberUsers"
                                            ErrorMessage="Please enter maximum number of users allowed." Display="Dynamic" ValidationGroup="Client"
                                            CssClass="validation1" SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Federal ID No.
                                    </td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <ig:WebTextEditor ID="txtFederalTaxID" runat="server" MaxLength="20" />
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtFederalTaxID"
                                            ErrorMessage="Please enter EFIN." Display="Dynamic" CssClass="validation1" ValidationGroup="Client"
                                            SetFocusOnError="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Fee per Contract
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtFeePerContract" runat="server" />
                                        <asp:CompareValidator ID="CompareValidatorFee" runat="server" ControlToValidate="txtFeePerContract"
                                            Display="Dynamic" CssClass="validation1" ErrorMessage="Numeric data allowed only." ValidationGroup="Client"
                                            SetFocusOnError="True" Type="Double" Operator="DataTypeCheck" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Is Trial Account?
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <asp:CheckBox ID="cbxTrial" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Maximum Number of Leads Allowed
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtMaxLeads" runat="server" TextMode="Number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Show Tasks?
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <asp:CheckBox ID="cbxShowTasks" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Inactivity Period
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtInactivityPeriod" runat="server" TextMode="Number" />
                                        &nbsp;in Days
						               
                                    </td>
                                </tr>


                            </table>
                        </td>
                        <td class="left top">
                            <ajaxToolkit:TabContainer ID="tabContainer" runat="server" Width="100%">
                                <ajaxToolkit:TabPanel ID="tabPanel1" runat="server">
                                    <HeaderTemplate>
                                        Logo
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                            <tr>
                                                <td>
                                                    <!-- logo -->

                                                    <asp:Image ID="clientLogo" runat="server" Width="100px" ImageAlign="Middle" CssClass="photo" />

                                                </td>
                                                <td class="top">
                                                    <asp:FileUpload ID="fileUpload" runat="server" Width="300px" />
                                                    &nbsp;<asp:Button ID="btnUpload" runat="server" Text="Upload Logo" OnClick="btnUpload_Click"
                                                        class="mysubmit" ValidationGroup="newLogo" />
                                                    <div>
                                                        <asp:CustomValidator ID="cvalAttachment" runat="server" ControlToValidate="fileUpload"
                                                            ValidationGroup="newLogo" SetFocusOnError="true" ErrorMessage="Only JPG images are allowed."
                                                            CssClass="validation1" ClientValidationFunction="UploadFileCheck">
                                                        </asp:CustomValidator>
                                                    </div>
                                                    <div class="validation1">
                                                        For best results, please ensure JPG has a width of 600px.
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="tabPanelNotes" runat="server">
                                    <HeaderTemplate>
                                        Invoice Settings
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                            <tr>
                                                <td class="right">Invoice Settings
                                                </td>
                                                <td class="redstar">*</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlInvoiceSetting" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceSetting_SelectedIndexChanged">
                                                        <asp:ListItem Text="Contingency Fee" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Carrier Invoice Settings" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="tr_percentage" runat="server">
                                                <td class="right">Percentage</td>
                                                <td class="redstar"></td>
                                                <td>
                                                    <ig:WebPercentEditor ID="txtContingencyFee" runat="server"></ig:WebPercentEditor>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                                <ajaxToolkit:TabPanel ID="tabPanelImap" runat="server">
                                    <HeaderTemplate>
                                        Import Email Settings
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                                            <tr>
                                                <td class="right">IMap Host
                                                </td>
                                                <td class="redstar"></td>
                                                <td>
                                                    <ig:WebTextEditor ID="txtImapHost" runat="server" MaxLength="100" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="right">IMap Host Post
                                                </td>
                                                <td class="redstar"></td>
                                                <td>
                                                    <ig:WebTextEditor ID="txtImapHostPort" runat="server" MaxLength="5" />
                                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtImapHostPort" ValidationGroup="Client"
                                                        Display="Dynamic" CssClass="validation1" ErrorMessage="Numeric data allowed only."
                                                        SetFocusOnError="True" Type="Integer" Operator="DataTypeCheck" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="right">Use Secured Connection
                                                </td>
                                                <td class="redstar"></td>
                                                <td>
                                                    <asp:CheckBox ID="cbxImapUseSSL" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </ajaxToolkit:TabPanel>
                            </ajaxToolkit:TabContainer>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hfuserid" runat="server" />
    <asp:HiddenField ID="hfclientid" runat="server" />


    <script type="text/javascript">
        function UploadFileCheck(source, arguments) {
            var sFile = arguments.Value;
            arguments.IsValid = sFile.endsWith('.jpg');
        }
    </script>
</asp:Content>
