<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="form.aspx.cs" Inherits="CRM.Web.Protected.Intake.form" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControl/Admin/ucPolicyType.ascx" TagName="ucPolicyType" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <link type="text/css" rel="Stylesheet" href="../../ig_ui/css/themes/infragistics/infragistics.theme.css" />
    <link type="text/css" rel="Stylesheet" href="../../ig_ui/css/structure/infragistics.css" />
    <script type="text/javascript" src="../../ig_ui/js/infragistics.js"></script>

    <head />
    <div class="paneContent">
        <div class="page-title">
            Claim Intake Form
        </div>


        <div class="message_area">
            <asp:Label ID="lblMessage" runat="server" />
        </div>
        <div class="paneContentInner">
            <div style="border: solid 1px #204D89; width: 70%; margin: 0 auto;">
                <div class="section-title" onclick="expandCollapse('tbl_claim');" style="cursor: pointer;">
                    1. Claim Details
                </div>

                <table id="tbl_claim" style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                    <tr>
                        <td class="right" style="width: 20%;">Loss Date</td>
                        <td class="redstar" style="width: 5px">*</td>
                        <td>
                            <ig:WebDatePicker ID="txtLossDate" runat="server" TabIndex="1" Height="20px"
                                Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" StyleSetName="Default" />
                            <div>
                                <asp:RequiredFieldValidator ID="rfvLossDate" runat="server" ControlToValidate="txtLossDate" CssClass="validation1"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required!" ValidationGroup="intake" InitialValue="" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Claim Number</td>
                        <td class="redstar">*</td>
                        <td>
                            <ig:WebTextEditor ID="txtClaimNumber" runat="server" MaxLength="50" Width="200px"></ig:WebTextEditor>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClaimNumber" InitialValue=""
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required!" ValidationGroup="intake" CssClass="validation1" />
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td class="right">Policy Number</td>
                        <td></td>
                        <td>
                            <ig:WebTextEditor ID="txtPolicyNumber" runat="server" MaxLength="50" Width="200px"></ig:WebTextEditor>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Property Type</td>
                        <td></td>
                        <td>
                            <asp:DropDownList ID="ddlTypeOfProperty" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right top">Damage Type</td>
                        <td></td>
                        <td>
                            <div id="divdamage" runat="server" style="height: 100px; border: solid 1px silver; width: 230px; overflow: auto;">
                                <asp:CheckBoxList ID="chkTypeOfDamage" runat="server" Width="200px" CssClass="checkboxlist">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="right top">Description of Loss/Peril</td>
                        <td class="redstar top">*</td>
                        <td>
                            <ig:WebTextEditor ID="txtLossPerilDescription" runat="server" Width="500px"
                                TextMode="MultiLine" MultiLine-Rows="10">
                            </ig:WebTextEditor>
                            <div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLossPerilDescription"
                                    SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required!" ValidationGroup="intake" CssClass="validation1" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="right top">General Assignment Instructions</td>
                        <td></td>
                        <td>
                            <ig:WebTextEditor ID="txtAssignmentInstructions" runat="server" Width="500px"
                                TextMode="MultiLine" MultiLine-Rows="10">
                            </ig:WebTextEditor>
                        </td>
                    </tr>
                </table>

                <div class="section-title" onclick="expandCollapse('div_carrier');" style="cursor: pointer;">
                    2. Carrier Information                            
                </div>
                <div id="div_carrier">
                    <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                        <tr>
                            <td class="right" style="width: 20%;">Select Carrier</td>
                            <td class="redstar" style="width: 5px"></td>
                            <td>
                                <asp:DropDownList ID="ddlCarrier" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCarrier_SelectedIndexChanged"></asp:DropDownList>
                                <asp:LinkButton ID="lbtnNewCarrier" runat="server" Text="New Carrier" OnClick="lbtnNewCarrier_Click" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlCarrier" runat="server" Visible="false" Width="100%">

                        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                            <tr>
                                <td class="right" style="width: 20%;">Carrier Name</td>
                                <td class="redstar" style="width: 5px">*</td>
                                <td>
                                    <ig:WebTextEditor ID="txtCarrierName" runat="server" MaxLength="100" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right">Address Line</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtCarrierAddressLine1" runat="server" MaxLength="100" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right"></td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtCarrierAddressLine2" runat="server" MaxLength="50" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right carrier">State</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" />
                                </td>

                            </tr>
                            <tr>
                                <td class="right">City</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" />
                                </td>

                            </tr>
                            <tr>
                                <td class="right">Zip Code</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlZipCode" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Contact First Name</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtContactFirstName" runat="server" MaxLength="50" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right">Contact Last Name</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtContactLastName" runat="server" MaxLength="50" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right">Contact Phone Number</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtContactPhone" runat="server" MaxLength="50" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right">Contact Fax Number</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtContactFax" runat="server" MaxLength="50" Width="300px"></ig:WebTextEditor>
                                </td>

                            </tr>
                            <tr>
                                <td class="right">Contact Email</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtContactEmail" runat="server" MaxLength="100" Width="300px"></ig:WebTextEditor>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div class="section-title" onclick="expandCollapse('tbl_insured');" style="cursor: pointer;">
                    3. Insured Information                    
                </div>
                <table id="tbl_insured" style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                    <tr>
                        <td class="right" style="width: 20%;">First Name</td>
                        <td class="redstar" style="width: 5px">*</td>
                        <td>
                            <asp:TextBox ID="txtInsuredFirstName" runat="server" MaxLength="50" Width="300px" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="right">Last Name</td>
                        <td class="redstar">*</td>
                        <td>
                            <asp:TextBox ID="txtInsuredLastName" runat="server" MaxLength="50" Width="300px" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="right">Loss Address</td>
                        <td class="redstar">*</td>
                        <td>
                            <asp:TextBox ID="txtInsuredLossAddressLine" runat="server" MaxLength="100" Width="300px" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Button ID="btnCopyAddress" runat="server" Text="Copy Address" OnClick="btnCopyAddress_Click"
                                CssClass="mysubmit" CausesValidation="false" Font-Size="0.8em" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right"></td>
                        <td class="redstar"></td>
                        <td>
                            <asp:TextBox ID="txtInsuredLossAddressLine2" runat="server" MaxLength="50" Width="300px" />
                        </td>
                        <td class="right">Mailing Address                                  
                        </td>
                        <td>
                            <asp:TextBox ID="txtInsuredMailingAddress" runat="server" MaxLength="100" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">State</td>
                        <td class="redstar">*</td>
                        <td>
                            <asp:DropDownList ID="ddlInsuredLossState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuredState_SelectedIndexChanged" />
                        </td>
                        <td class="right">State</td>
                        <td>
                            <asp:DropDownList ID="ddlInsuredMailingState" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuredMailingState_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">City</td>
                        <td class="redstar">*</td>
                        <td>
                            <asp:DropDownList ID="ddlInsuredLossCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuredCity_SelectedIndexChanged" />
                        </td>
                        <td class="right">City</td>
                        <td>
                            <asp:DropDownList ID="ddlInsuredMailingCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInsuredMailingCity_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Zip Code</td>
                        <td class="redstar">*</td>
                        <td>
                            <asp:DropDownList ID="ddlInsuredLossZipCode" runat="server" />
                        </td>
                        <td class="right">Zip Code</td>
                        <td>
                            <asp:DropDownList ID="ddlInsuredMailingZipCode" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Phone</td>
                        <td class="redstar">*</td>
                        <td>
                            <asp:TextBox ID="txtInsuredPhone" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Secondary Phone</td>
                        <td class="redstar"></td>
                        <td>
                            <asp:TextBox ID="txtInsuredSecondaryPhone" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Fax Number</td>
                        <td class="redstar"></td>
                        <td>
                            <asp:TextBox ID="txtInsuredFax" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="right">Email</td>
                        <td class="redstar"></td>
                        <td>
                            <asp:TextBox ID="txtInsuredEmail" runat="server" MaxLength="100" />
                        </td>
                    </tr>
                </table>
                <div class="section-title" onclick="expandCollapse('tbl_policy');" style="cursor: pointer;">
                    4. Policy Information and Coverage Details
                </div>
                <table id="tbl_policy" style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                    <tr>
                        <td class="right" style="width: 20%;">Coverage Type</td>
                        <td class="redstar" style="width: 5px">*</td>
                        <td>
                            <uc1:ucPolicyType ID="ucPolicyType1" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ucPolicyType1$ddlPolicyType"
                                SetFocusOnError="true" Display="Dynamic" ErrorMessage="Required!" ValidationGroup="intake" CssClass="validation1" InitialValue="0" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>

                            <asp:GridView ID="gvCoverages" CssClass="gridView" ShowFooter="false" Width="100%" runat="server" HorizontalAlign="Center"
                                AutoGenerateColumns="False" CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff" GridLines="Vertical">
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Coverage Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCoverage" runat="server" Text='<%# Bind("Description")%>' MaxLength="100" Width="300px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Limit">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLimit" runat="server" Text='<%# Bind("Limit")%>' MaxLength="50" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deductible">
                                        <ItemTemplate>
                                            <ig:WebTextEditor ID="txtDeductible" runat="server" MaxLength="50" TextMode="Number"></ig:WebTextEditor>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Coinsurance Forms">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCoinsuranceForm" runat="server" Text='<%# Bind("CoInsuranceForm")%>' MaxLength="50" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <div class="section-title" onclick="expandCollapse('tbl_comments');" style="cursor: pointer;">
                    5. Comments
                </div>
                <table id="tbl_comments" style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                    <tr>
                        <td class="right top" style="width: 20%;">Comments</td>
                        <td class="redstar" style="width: 5px"></td>
                        <td>
                            <ig:WebTextEditor ID="txtComments" runat="server" Width="600px"
                                TextMode="MultiLine" MultiLine-Rows="10">
                            </ig:WebTextEditor>
                        </td>
                    </tr>
                </table>

                <div class="section-title" onclick="expandCollapse('tbl_files');" style="cursor: pointer;">
                    6. Upload Files
                </div>
                <table id="tbl_files" style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0">
                    <tr>
                        <td class="right top" style="width: 20%;">Attachment</td>
                        <td class="redstar" style="width: 5px"></td>
                        <td>
                            <asp:FileUpload ID="fileUpload1" runat="server" />
                        </td>
                        <td class="left">
                            <ig:WebTextEditor ID="txtfileUpload1" runat="server" MaxLength="500" Width="400px"></ig:WebTextEditor>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:FileUpload ID="fileUpload2" runat="server" />
                        </td>
                        <td class="left">
                            <ig:WebTextEditor ID="txtfileUpload2" runat="server" MaxLength="500" Width="400px"></ig:WebTextEditor>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:FileUpload ID="fileUpload3" runat="server" />
                        </td>
                        <td class="left">
                            <ig:WebTextEditor ID="txtfileUpload3" runat="server" MaxLength="500" Width="400px"></ig:WebTextEditor>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:FileUpload ID="fileUpload4" runat="server" />
                        </td>
                        <td class="left">
                            <ig:WebTextEditor ID="txtfileUpload4" runat="server" MaxLength="500" Width="400px"></ig:WebTextEditor>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:FileUpload ID="fileUpload5" runat="server" />
                        </td>
                        <td class="left">
                            <ig:WebTextEditor ID="txtfileUpload5" runat="server" MaxLength="500" Width="400px"></ig:WebTextEditor>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="center" style="margin-top: 5px;">
                <asp:Button ID="btnSubmit" runat="server" CssClass="mysubmit" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="intake" />
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hf_carrierID" runat="server" />
    <script type="text/javascript">


        function expandCollapse(controlID) {
            var control = "#" + controlID;


            var display = $(control).css('display');
            if (display == 'none') {
                $(control).show();
            }
            else {
                $(control).hide();
            }
        }

        $(document).ready(function () {
            //$(".carrier").hide();
        });

        function quickAddCarrier() {
            var display = $(".carrier").css('display');

            if (display == 'none') {
                $(".carrier").show();
                $("#span_new").html('Hide New');
            }
            else {
                $(".carrier").hide();
                $("#span_new").html('New Carrier');
            }
        }
    </script>
</asp:Content>
