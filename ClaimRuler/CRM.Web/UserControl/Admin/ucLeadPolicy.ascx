<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLeadPolicy.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucLeadPolicy" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Src="~/UserControl/Admin/ucPolicyType.ascx" TagName="ucPolicyType" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Admin/ucLeadPolicyContact.ascx" TagName="ucLeadPolicyContact" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Admin/ucPolicyPropertyLimits.ascx" TagName="ucPolicyPropertyLimits" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Admin/ucPolicySubLimit.ascx" TagName="ucPolicySubLimit" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/Admin/ucClaimList.ascx" TagName="ucClaimList" TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/Admin/ucPolicyCasualtyLimits.ascx" TagName="ucPolicyCasualtyLimits" TagPrefix="uc6" %>


<asp:Panel ID="pnlContent" runat="server">
    <div class="paneContentInner">
        <div class="message_area">
            <asp:Label ID="lblMessage" runat="server" />
        </div>

        <ajaxToolkit:TabContainer ID="tabContainerPolicy" runat="server" Width="100%" ActiveTabIndex="1">
            <ajaxToolkit:TabPanel ID="tabPanel2" runat="server">
                <HeaderTemplate>
                    Policy						
                </HeaderTemplate>
                <ContentTemplate>
                    <table style="width: 100%" class="editForm nowrap">
                        <tr>
                            <td class="top left" style="width: 50%;">
                                <div class="boxContainer">
                                    <div class="section-title">
                                        Policy Details
                                    </div>
                                    <table style="width: 100%" class="editForm nowrap">
                                        <tr>
                                            <td style="width: 15%" class="top">Policy Number </td>
                                            <td style="width: 5px" class="redstar top">&nbsp;</td>
                                            <td>
                                                <ig:WebTextEditor runat="server" ID="txtInsurancePolicyNumber" MaxLength="100" TabIndex="1" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%" class="top">Policy Form Type</td>
                                            <td style="width: 5px" class="redstar top">&nbsp;</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtPolicyFormType" runat="server" TabIndex="3" MaxLength="50" />




                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%" class="top">Type of Policy</td>
                                            <td style="width: 5px" class="redstar top"></td>
                                            <td>
                                                <uc1:ucPolicyType ID="ucPolicyType1" runat="server" />




                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%" class="left middle">Effective Date
                                            </td>
                                            <td style="width: 5px" class="redstar"></td>
                                            <td>
                                                <ig:WebDatePicker ID="txtEffectiveDate" runat="server" TabIndex="3" CssClass="date_picker">
                                                    <Buttons>
                                                        <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                                    </Buttons>
                                                </ig:WebDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%" class="left middle">Expiration Date
                                            </td>
                                            <td style="width: 5px" class="redstar"></td>
                                            <td>
                                                <ig:WebDatePicker ID="txtExpirationDate" runat="server" TabIndex="3" CssClass="date_picker">
                                                    <Buttons>
                                                        <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                                    </Buttons>
                                                </ig:WebDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%" class="left middle">Initial Coverage Date
                                            </td>
                                            <td style="width: 5px" class="redstar"></td>
                                            <td>
                                                <ig:WebDatePicker ID="txtInitialCoverageDate" runat="server" TabIndex="4" CssClass="date_picker">
                                                    <Buttons>
                                                        <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                                    </Buttons>
                                                </ig:WebDatePicker>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                             
                            </td>
                            <td class="top left">
                                <div class="boxContainer">
                                    <div class="section-title">
                                        <%--Insurer/Carrier--%>
                                        Client
                                    </div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td class="top left" style="width: 50%;">
                                                <table style="width: 100%" class="editForm nowrap">
                                                    <tr>
                                                        <td class="left">Client</td>
                                                        <td class="redstar"></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCarrier" runat="server" OnSelectedIndexChanged="ddlCarrier_SelectedIndexChanged" AutoPostBack="True" TabIndex="11" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10%;">Name</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceCompanyName" />
                                                    </tr>
                                                    <tr>
                                                        <td>Street Address</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceAddress" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>Street Address 2</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceAddress2" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>State</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceState" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>City</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceCity" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Postal Code
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceZipCode" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Country
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="txtInsuranceCountry" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Phone Number</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="txtInsurancePhoneNumber" runat="server" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Fax Number</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="txtInsuranceFaxNumber" runat="server" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>

                                                </table>
                                            </td>
                                            <td class="left center">
                                                <table style="width: 100%" class="editForm nowrap">

                                                    <tr>
                                                        <td class="left">Primary Contact</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="txtPrimaryContactName" runat="server" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="left">Contact Phone</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="txtPrimaryContactPhone" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="left">Contact Fax</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="txtPrimaryContactFax" runat="server" />


                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="left">Contact Email</td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="txtPrimaryContactEmail" runat="server" />

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="top left" colspan="2">
                                <div class="boxContainer">
                                    <div class="section-title">
                                        Policy Limits
                                    </div>

                                      <div id="divradiobuttonCoverageAdd" runat="server" visible="false">
                                     <table>
                                        <tr>
                                            <td >  
                                                Apply Deductible
                                            </td>
                                            <td>
                                                <asp:RadioButton id="acrossAllCoveragesAdd" runat="server" Text="Across All Coverages" ClientIDMode="Static" GroupName="A"/>
                                                <asp:RadioButton id="coverageSpecificAdd" runat="server" Text="Coverage Specific" ClientIDMode="Static" GroupName="A"/>
                                             <%--<asp:RadioButtonList id="rbtnApplyDetuctible" runat="server" RepeatDirection="Horizontal">
                                                  <asp:ListItem  Selected="True" Text="Across All Coverages" Value="Across All Coverages"/>
                                                 <asp:ListItem  Selected="False" Text="Coverage Specific" Value="Coverage Specific"/>
                                             </asp:RadioButtonList>--%>
                                                <asp:HiddenField id="hdnApplyDeductibleAdd" runat="server" Value="1"/>
                                                <asp:HiddenField id="hdnPolicyIdDetuctibleAdd" runat="server" Value="0"/>
                                            </td>                                           
                                        </tr>
                                    </table>
                                </div>
                                      <div id="divbuttonCoverageAdd" runat="server" visible="false">
                                    <table>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <%--<asp:Button id="btnShowLossTemplateAdd" runat="server" CssClass="mysubmit" Text="Use Template" OnClick="btnShowLossTemplateAdd_Click" OnClientClick="javascript:return ConfirmDialog(this, 'Warning:  If you use template, then it may not be recoverable.  Please do not delete unless absolutely necessary.  Are you sure you want to delete this record forever?');"/>--%>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnLossDetailsPopUpOpenAdd" runat="server" CssClass="mysubmit" Text="Add Coverage" OnClientClick="return LossDetailsPopUpAdd();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                      <div id="divradiobuttonCoverage" runat="server">
                                     <table>
                                        <tr>
                                            <td >  
                                                Apply Deductible
                                            </td>
                                            <td>
                                                <asp:RadioButton id="acrossAllCoverages" runat="server" Text="Across All Coverages" ClientIDMode="Static" GroupName="A"/>
                                                <asp:RadioButton id="coverageSpecific" runat="server" Text="Coverage Specific" ClientIDMode="Static" GroupName="A"/>
                                             <%--<asp:RadioButtonList id="rbtnApplyDetuctible" runat="server" RepeatDirection="Horizontal">
                                                  <asp:ListItem  Selected="True" Text="Across All Coverages" Value="Across All Coverages"/>
                                                 <asp:ListItem  Selected="False" Text="Coverage Specific" Value="Coverage Specific"/>
                                             </asp:RadioButtonList>--%>
                                                <asp:HiddenField id="hdnApplyDeductible" runat="server" Value="0"/>
                                                <asp:HiddenField id="hdnPolicyIdDetuctible" runat="server" Value="0"/>
                                            </td>                                           
                                        </tr>
                                    </table>
                                </div>
                                      <div id="divbuttonCoverage" runat="server">


                                    <table>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <%--<asp:Button id="btnShowLossTemplate" runat="server" CssClass="mysubmit" Text="Use Template" OnClick="btnShowLossTemplate_Click" OnClientClick="javascript:return ConfirmDialog(this, 'Warning:  If you use template, then it may not be recoverable.  Please do not delete unless absolutely necessary.  Are you sure you want to delete this record forever?');"/>--%>
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </div>
                               

                                    <uc3:ucPolicyPropertyLimits ID="propertyLimits" runat="server" />
                                   
                                </div>
                               
                               <%-- <div class="boxContainer" style="margin-top: 5px;">
                                    <div class="section-title">
                                        Casualty Policy Limits
                                    </div>
                                     <div>
                                       <asp:Button ID="btnLossDetailsPopUpOpen" runat="server" CssClass="mysubmit" Text="Add Coverage" OnClientClick="return LossDetailsPopUp();" />
                                     </div>
                                    <%--<uc6:ucPolicyCasualtyLimits ID="casualtyLimits" runat="server" />--%>
                                
                            </td>
                          
                        </tr>
                        <tr>
                              <td class="top left" colspan="2">
                                <div class="boxContainer">
                                    <div class="section-title">
                                        Policy Sub-Limits                                
                                    </div>
                                    <uc4:ucPolicySubLimit ID="propertySubLimits" runat="server" />
                                </div>
                            </td>
                        </tr>


                        <tr>
                            <td colspan="2" class="top left">
                                <asp:Panel ID="pnlClaims" runat="server" Visible="False">
                                    <div class="boxContainer">
                                        <div class="section-title">
                                            Claims                                
                                        </div>
                                        <div style="margin: 5px;">
                                            <asp:LinkButton ID="hlnlNewClaim" runat="server" Text="New Claim" CssClass="link" OnClick="hlnlNewClaim_Click"></asp:LinkButton>
                                        </div>
                                        <uc5:ucClaimList ID="claimLists" runat="server" />
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tabPanelAgent" runat="server">
                <HeaderTemplate>
                    Agent Details						
                </HeaderTemplate>
                <ContentTemplate>
                    <table style="width: 100%" class="editForm nowrap" border="0">
                        <tr>
                            <td class="right" style="width: 35%">Agent</td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlAgent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAgent_SelectedIndexChanged" />
                                &nbsp;
                                <asp:LinkButton ID="lbtnAgentNew" runat="server" Text="New Agent" CssClass="link" OnClick="lbtnAgentNew_Click" />
                            </td>
                        </tr>
                    </table>

                    <table style="width: 100%" class="editForm nowrap" border="0">
                        <tr>
                            <td class="right" style="width: 35%">Agent Entity Name</td>
                            <td></td>
                            <td>
                                <asp:Label ID="lblAgentEntityName" runat="server" Visible="false" CssClass="" />
                                <ig:WebTextEditor ID="txtAgentEntityName" runat="server" MaxLength="100" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">First Name</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentFirstName" MaxLength="50" runat="server" Visible="false"></ig:WebTextEditor>
                                <asp:Label ID="lblAgentFirstName" runat="server" Visible="false" />
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtAgentFirstName" Display="Dynamic"
                                        SetFocusOnError="true" ErrorMessage="Please enter first name." CssClass="validation1" ValidationGroup="Contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Last Name</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentLastName" MaxLength="50" runat="server" Visible="false"></ig:WebTextEditor>
                                <asp:Label ID="lblAgentLastName" runat="server" Visible="false" />
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAgentLastName" Display="Dynamic"
                                        SetFocusOnError="true" ErrorMessage="Please enter last name." CssClass="validation1" ValidationGroup="Contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Address
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentAddress1" runat="server" MaxLength="100" Width="200px" Visible="false" />
                                <asp:Label ID="lblAgentAddress1" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right"></td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentAddress2" runat="server" MaxLength="100" Width="200px" Visible="false" />
                                <asp:Label ID="lblAgentAddress2" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">State
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged" Visible="false">
                                </asp:DropDownList>
                                <asp:Label ID="lblAgentState" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">City
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True" Visible="false"
                                    OnSelectedIndexChanged="dllCity_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="lblAgentCity" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Zip Code
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlAgentZip" runat="server" Visible="false">
                                </asp:DropDownList>
                                <asp:Label ID="lblAgentZip" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">E-mail</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentEmail" runat="server" MaxLength="100" Visible="false" />
                                <asp:Label ID="lblAgentEmail" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Phone #</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentPhoneNumber" runat="server" MaxLength="30" Visible="false" />
                                <asp:Label ID="lblAgentPhone" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Fax #</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentFax" runat="server" MaxLength="30" Visible="false" />
                                <asp:Label ID="lblAgentFax" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Agent Code #</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentCode" runat="server" MaxLength="50" Visible="false" />
                                <asp:Label ID="lblAgentCode" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Agent Sub Code #</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgenctSubcode" runat="server" MaxLength="50" Visible="false" />
                                <asp:Label ID="lblAgentSubCode" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Agent Customer ID</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAgentCustomerID" runat="server" MaxLength="50" Visible="false" />
                                <asp:Label ID="lblAgentCustomerID" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Panel ID="pnlAgentButton" runat="server">
                                    <asp:Button ID="btnAgentNewSave" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnAgentNewSave_Click" />
                                    &nbsp;
                                        <asp:Button ID="btnAgentNewCancel" runat="server" Text="Cancel" CssClass="mysubmit" OnClick="btnAgentNewCancel_Click" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>


                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tabPanelLienHolder" runat="server">
                <HeaderTemplate>
                    Mortgagees		
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="pnlMortgageeAdd" runat="server" Visible="false">

                        <table style="width: 100%" class="editForm nowrap" border="0">
                            <tr>
                                <td class="right">Mortgagee Name</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlMorgagee" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvMortagee1" runat="server" ControlToValidate="ddlMorgagee" InitialValue="0"
                                        SetFocusOnError="true" Display="Dynamic" ValidationGroup="mortgagee" CssClass="validation1" ErrorMessage="Please select mortagee." />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Loan Number</td>
                                <td></td>
                                <td>
                                    <ig:WebTextEditor ID="txtLoanNumber" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="left">
                                    <asp:Button ID="btnMortageeAddSave" runat="server" Text="Save" OnClick="btnMortageeAddSave_Click" CssClass="mysubmit" ValidationGroup="mortgagee" />
                                    &nbsp;
                                    <asp:Button ID="btnMortageeAddCancel" runat="server" Text="Cancel" OnClick="btnMortageeAddCancel_Click" CssClass="mysubmit" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlMortgageeGrid" runat="server">
                        <table style="width: 100%; margin: 0 auto;">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbtnMortgageeAdd" runat="server" Text="Add Mortgagee" CssClass="link" OnClick="lbtnMortgageeAdd_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="left top">

                                    <asp:GridView ID="gvMortgagee" CssClass="gridView" OnRowCommand="gvMortgagee_RowCommand"
                                        Width="100%" runat="server" AutoGenerateColumns="False" CellPadding="2">
                                        <PagerStyle CssClass="pager" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle BackColor="#E8F2FF" />
                                        <EmptyDataTemplate>
                                            No mortgagees found.
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" Width="45px" Wrap="False" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                                                        OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this Lienholder from policy?');"
                                                        CommandArgument='<%#Eval("ID") %>' ToolTip="Delete"
                                                        ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mortgagee Name" SortExpression="Mortgagee.MortageeName">
                                                <ItemTemplate>
                                                    <%#Eval("Mortgagee.MortageeName")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loan Number">
                                                <ItemTemplate>
                                                    <%#Eval("LoanNumber")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact Name" SortExpression="Mortgagee.ContactName">
                                                <ItemTemplate>
                                                    <%#Eval("Mortgagee.ContactName")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone">
                                                <ItemTemplate>
                                                    <%#Eval("Mortgagee.Phone")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fax">
                                                <ItemTemplate>
                                                    <%#Eval("Mortgagee.Fax")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email">
                                                <ItemTemplate>
                                                    <%#Eval("Mortgagee.Email")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>

                                </td>
                                <td class="redstar"></td>
                                <td class="left top"></td>

                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tabPanelPolicyNotes" runat="server">
                <HeaderTemplate>
                    Notes						
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="pnlPolicyNotes" runat="server">
                        <div class="paneContentInner">
                            <asp:LinkButton ID="lbtnPolicyNoteNew" runat="server" Text="New Note" CssClass="link" OnClick="lbtnPolicyNoteNew_Click" />
                        </div>
                        <ig:WebDataGrid ID="wdgNotes" runat="server" Height="350px" Width="100%"
                            AutoGenerateColumns="false"
                            CssClass="gridView"
                            DataSourceID="edsPolicyNotes"
                            OnItemCommand="wdgNotes_ItemCommand">
                            <Columns>
                                <ig:TemplateDataField CssClass="center" Key="commands" Width="40px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server"
                                            ImageAlign="Middle"
                                            ImageUrl="~/Images/edit.png"
                                            ToolTip="Edit"
                                            CommandName="DoEdit"
                                            CommandArgument='<%# Eval("PolicyNoteID") %>' />

                                        <asp:ImageButton ID="btnDelete" runat="server"
                                            ImageAlign="Middle"
                                            ImageUrl="~/Images/delete_icon.png"
                                            ToolTip="Delete"
                                            CommandName="DoRemove"
                                            CommandArgument='<%#Eval("PolicyNoteID") %>'
                                            OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this note?');" />
                                    </ItemTemplate>
                                </ig:TemplateDataField>
                                <ig:TemplateDataField Key="NoteDate" Header-Text="Date" Width="110px">
                                    <ItemTemplate>
                                        <div class="center top">
                                            <%# Eval("NoteDate", "{0:g}") %>
                                        </div>
                                    </ItemTemplate>
                                </ig:TemplateDataField>
                                <ig:TemplateDataField Key="UserName" Header-Text="User Name" Width="100px">
                                    <ItemTemplate>
                                        <div class="center top">
                                            <%# Eval("SecUser.UserName") %>
                                        </div>
                                    </ItemTemplate>
                                </ig:TemplateDataField>
                                <ig:TemplateDataField Key="Notes" Header-Text="Notes">
                                    <ItemTemplate>
                                        <%# Eval("Notes") %>
                                    </ItemTemplate>
                                </ig:TemplateDataField>
                            </Columns>
                            <Behaviors>
                                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="20"
                                    ThresholdFactor="0.5" Enabled="true" />
                                <ig:Sorting Enabled="true" SortingMode="Single" />
                            </Behaviors>
                        </ig:WebDataGrid>
                        <asp:EntityDataSource ID="edsPolicyNotes" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
                            EnableFlattening="False" EntitySetName="PolicyNote" Include="SecUser"
                            Where="it.PolicyID = @policyID"
                            OrderBy="it.NoteDate Desc">
                            <WhereParameters>
                                <asp:ControlParameter Name="policyID" ControlID="hf_policyID" Type="Int32" />
                            </WhereParameters>
                        </asp:EntityDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlPolicyNoteEdit" runat="server" Visible="false">
                        <table style="width: 100%" class="editForm nowrap">
                            <tr>
                                <td>
                                    <ig:WebTextEditor ID="txtPolicyNote" runat="server" TextMode="MultiLine" MultiLine-Rows="10" Width="100%" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvPolicyNote" runat="server" ControlToValidate="txtPolicyNote" Display="Dynamic" SetFocusOnError="true"
                                            CssClass="validation1" ErrorMessage="Please enter note." ValidationGroup="policyNote" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="center">
                                    <asp:Button ID="btnPolicyNoteSave" runat="server" Text="Save" CssClass="mysubmit" CausesValidation="true" ValidationGroup="policyNote" OnClick="btnPolicyNoteSave_Click" />
                                    &nbsp;
                                <asp:Button ID="btnPolicyNoteCancel" runat="server" Text="Cancel" CssClass="mysubmit" CausesValidation="false" OnClick="btnPolicyNoteCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

        </ajaxToolkit:TabContainer>
    </div>
</asp:Panel>
<div id="survey_reminder" style="display: none;">
    <div style="display: inline-block; float: left;">
        Remind me in&nbsp;display: inline-block; float: left;">
        <input type="button" class="mysubmit" value="Save" />
    </div>
</div>

<asp:HiddenField ID="hf_policyID" runat="server" />
<asp:HiddenField ID="hf_lastStatusID" runat="server" />
<asp:HiddenField ID="hf_leadDocumentId" runat="server" />

<asp:Button ID="btnSaveHidden" runat="server" OnClick="btnSaveHidden_Click" Style="display: none;" />


<div id="divLossDetailsPopUp" style="display: none; width: 90%;" title="Loss Detail">
    <div class="boxContainer">
        <div class="section-title">
            Loss Detail
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">

            <tr>
                <td class="right">Coverage</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtCoverage" TabIndex="1" Width="249px" />
                </td>
            </tr>
            <tr>
                <td class="right">Type</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="Structural" Value="Structural"></asp:ListItem>
                        <asp:ListItem Text="Other Structures" Value="Other Structures"></asp:ListItem>
                        <asp:ListItem Text="Personal Property" Value="Personal Property"></asp:ListItem>
                        <asp:ListItem Text="Loss of Use / ALE" Value="Loss of Use / ALE"></asp:ListItem>
                        <asp:ListItem Text="Personal Liability" Value="Personal Liability"></asp:ListItem>
                        <asp:ListItem Text="Medical Payments" Value="Medical Payments"></asp:ListItem>
                        <asp:ListItem Text="Contacts" Value="Contacts"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">Policy Limit</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtPolicyLimit" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Deductible</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtDeductible" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">CAT Deductible</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtCATDeductible" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
             <tr>
                <td class="right">Co Insurance Limit</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtCoInsuranceLimit" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Apply To</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlApplyTo" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="ACV" Value="ACV"></asp:ListItem>
                        <asp:ListItem Text="RCV" Value="RCV"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">ITV</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtItv" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>

            <tr>
                <td class="right">Reserve</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtReserve" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveLossDetails" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return SaveLossDetails();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>




        </table>


    </div>
</div>


<div id="divLossDetailsPopUpAdd" style="display: none; width: 90%;" title="Loss Detail">
    <div class="boxContainer">
        <div class="section-title">
            Loss Detail
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">

            <tr>
                <td class="right">Coverage</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtCoverageAdd" TabIndex="1" Width="249px" />
                </td>
            </tr>
            <tr>
                <td class="right">Type</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlTypeAdd" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="Structural" Value="Structural"></asp:ListItem>
                        <asp:ListItem Text="Other Structures" Value="Other Structures"></asp:ListItem>
                        <asp:ListItem Text="Personal Property" Value="Personal Property"></asp:ListItem>
                        <asp:ListItem Text="Loss of Use / ALE" Value="Loss of Use / ALE"></asp:ListItem>
                        <asp:ListItem Text="Personal Liability" Value="Personal Liability"></asp:ListItem>
                        <asp:ListItem Text="Medical Payments" Value="Medical Payments"></asp:ListItem>
                        <asp:ListItem Text="Contacts" Value="Contacts"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">Policy Limit</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtPolicyLimitAdd" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Deductible</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtDeductibleAdd" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
             <tr>
                <td class="right">CAT Deductible</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtCATDeductibleAdd" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
             <tr>
                <td class="right">Co Insurance Limit</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtCoInsuranceLimitAdd" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Apply To</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlApplyToAdd" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="ACV" Value="ACV"></asp:ListItem>
                        <asp:ListItem Text="RCV" Value="RCV"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">ITV</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtItvAdd" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>

            <tr>
                <td class="right">Reserve</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtReserveAdd" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveLossDetailsAdd" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return SaveLossDetailsAdd();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>




        </table>


    </div>
</div>

<div id="divLossDetailsPopUpAddEditProperty" style="display: none; width: 90%;" title="Loss Detail">
    <div class="boxContainer">
        <div class="section-title">
            Loss Detail
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">

            <tr>
                <td class="right">Coverage</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtCoverageAddEditProperty" TabIndex="1" Width="249px" />
                </td>
            </tr>
            <tr>
                <td class="right">Type</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlTypeAddEditProperty" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="Structural" Value="Structural"></asp:ListItem>
                        <asp:ListItem Text="Other Structures" Value="Other Structures"></asp:ListItem>
                        <asp:ListItem Text="Personal Property" Value="Personal Property"></asp:ListItem>
                        <asp:ListItem Text="Loss of Use / ALE" Value="Loss of Use / ALE"></asp:ListItem>
                        <asp:ListItem Text="Personal Liability" Value="Personal Liability"></asp:ListItem>
                        <asp:ListItem Text="Medical Payments" Value="Medical Payments"></asp:ListItem>
                        <asp:ListItem Text="Contacts" Value="Contacts"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">Policy Limit</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtPolicyLimitAddEditProperty" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Deductible</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtDeductibleAddEditProperty" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
             <tr>
                <td class="right">CAT Deductible</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtCATDeductibleAddEditProperty" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
             <tr>
                <td class="right">Co Insurance Limit</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtCoInsuranceLimitAddEditProperty" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td class="right">Apply To</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlApplyToAddEditProperty" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="ACV" Value="ACV"></asp:ListItem>
                        <asp:ListItem Text="RCV" Value="RCV"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
                        <asp:ListItem Text="None" Value="None"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">ITV</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtItvAddEditProperty" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>

            <tr>
                <td class="right">Reserve</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtReserveAddEditProperty" runat="server" DataMode="decimal" TabIndex="11" MaxDecimalPlaces="2" MaxLength="9" Width="249px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveLossDetailsAddEditProperty" runat="server" Text="Save" CssClass="mysubmit" OnClientClick="return SaveLossDetailsAddEditProperty();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>




        </table>


    </div>
</div>


<asp:HiddenField ID="hdnLimitIdForEdit" runat="server" />









<script type="text/javascript">


    function createSurveyReminder(tbxDate) {
        //$("#survey_reminder").dialog();
    }


    function validateCheckBoxList(sender, args) {
        var isValid = IsCheckBoxChecked();

        args.IsValid = isValid;
    }

</script>




<script>
    function LossDetailsPopUp() {
        $("#divLossDetailsPopUp").dialog({
            modal: false,
            width: 700,
            close: function (e, ui) {
                $(this).dialog('destroy');
                window.location.href = window.location.href;

            },
            buttons:
                       {
                           //'Done': function () {
                           //    $(this).dialog('close');
                           //}
                       }
        });

        return false;
    }

    function SaveLossDetails() {

        var coverage = $("#<%= txtCoverage.ClientID %>").val();
        var type = $("#<%= ddlType.ClientID %>").val();
        var policyLimit = $("#<%= txtPolicyLimit.ClientID %>").val();
        var deductible = $("#<%= txtDeductible.ClientID %>").val();
        var applyTo = $("#<%= ddlApplyTo.ClientID %>").val();
        var itv = $("#<%= txtItv.ClientID %>").val();
        var reserve = $("#<%= txtReserve.ClientID %>").val();

        //var claimID = parseInt($("[id$='hf_ClaimIdForStatus']").val());
        var policyID = parseInt($("[id$='hdnPolicyIdDetuctible']").val());
        
        
        var acrossall = parseInt($("[id$='hdnApplyDeductible']").val());

        var catDeductible = $("#<%= txtCATDeductible.ClientID %>").val();
        var coInsuranceLimit = $("#<%= txtCoInsuranceLimit.ClientID %>").val();
        

        var myParams = "{'policyID':'" + policyID + "', 'coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "','catDeductible':'" + catDeductible + "','coInsuranceLimit':'" + coInsuranceLimit + "'}";

        //var myParams = "{'claimID':'" + claimID + "', 'coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "'}";
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            data: myParams,
            url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveLossDetails") %>',
            success: function (data) {
                window.location.href = window.location.href;
            },
            error: function () {

            }
        });



    }


</script>

<script>
    $("#acrossAllCoverages").change(function () {
        var value = $(this).val();
        $("#<%= hdnApplyDeductible.ClientID %>").val("1");
        $("#<%=txtDeductible.ClientID %>").prop("disabled", true);
        if ($('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_tabContainerPolicy_tabPanel2_propertyLimits_gvLimits2').length > 0) {
            $("#<%=txtDeductible.ClientID %>").prop("disabled", true);
        }
        else { $("#<%=txtDeductible.ClientID %>").prop("disabled", false); }


    });
    $("#coverageSpecific").change(function () {
        var value = $(this).val();
        $("#<%= hdnApplyDeductible.ClientID %>").val("0");
        $("#<%=txtDeductible.ClientID %>").prop("disabled", false);

    });
</script>




<script>
    function LossDetailsPopUpAdd() {
        $("#divLossDetailsPopUpAdd").dialog({
            modal: false,
            width: 700,
            close: function (e, ui) {
                $(this).dialog('destroy');
                window.location.href = window.location.href;

            },
            buttons:
                       {
                           //'Done': function () {
                           //    $(this).dialog('close');
                           //}
                       }
        });

        return false;
    }

    function SaveLossDetailsAdd() {

        var coverage = $("#<%= txtCoverageAdd.ClientID %>").val();
        var type = $("#<%= ddlTypeAdd.ClientID %>").val();
        var policyLimit = $("#<%= txtPolicyLimitAdd.ClientID %>").val();
        var deductible = $("#<%= txtDeductibleAdd.ClientID %>").val();
        var applyTo = $("#<%= ddlApplyToAdd.ClientID %>").val();
        var itv = $("#<%= txtItvAdd.ClientID %>").val();
        var reserve = $("#<%= txtReserveAdd.ClientID %>").val();

        //var claimID = parseInt($("[id$='hf_ClaimIdForStatus']").val());
        var policyID = parseInt($("[id$='hdnPolicyIdDetuctibleAdd']").val());


        var acrossall = parseInt($("[id$='hdnApplyDeductibleAdd']").val());
        var catDeductible = $("#<%= txtCATDeductibleAdd.ClientID %>").val();
        var coInsuranceLimit = $("#<%= txtCoInsuranceLimitAdd.ClientID %>").val();
        

        var myParams = "{'policyID':'" + policyID + "', 'coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "','catDeductible':'" + catDeductible + "','coInsuranceLimit':'" + coInsuranceLimit + "'}";

        //var myParams = "{'claimID':'" + claimID + "', 'coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "'}";
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: 'POST',
            data: myParams,
            url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveLossDetailsAdd") %>',
            success: function (data) {
                window.location.href = window.location.href;
            },
            error: function () {

            }
        });



    }


</script>

<script>
    $("#acrossAllCoveragesAdd").change(function () {
        var value = $(this).val();
        $("#<%= hdnApplyDeductibleAdd.ClientID %>").val("1");
        $("#<%=txtDeductibleAdd.ClientID %>").prop("disabled", true);
        if ($('#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_tabContainerPolicy_tabPanel2_propertyLimits_gvLimits3').length > 0) {
            $("#<%=txtDeductibleAdd.ClientID %>").prop("disabled", true);
        }
        else { $("#<%=txtDeductibleAdd.ClientID %>").prop("disabled", false); }


    });
    $("#coverageSpecificAdd").change(function () {
        var value = $(this).val();
        $("#<%= hdnApplyDeductibleAdd.ClientID %>").val("0");
        $("#<%=txtDeductibleAdd.ClientID %>").prop("disabled", false);

    });
</script>


<%--for edit in add mode--%>
<script>
    function LossDetailsPopUpAddEditProperty(elem) {

        var id = jQuery(elem).attr('id');
        var hdnFieldLimitId = id.replace("btn", "hdn");
        var limitId = $("#" + hdnFieldLimitId).val();
        $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_hdnLimitIdForEdit").val(limitId);
        var myParams = "{ 'limitId':'" + limitId + "'}";


        var policyID = parseInt($("[id$='hdnPolicyIdDetuctible']").val());
       

        if (policyID > 0)//edit mode
        {
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/GetLossDataEditPropertyEdit") %>',
            success: function (data) {
                $("#<%= txtCoverageAddEditProperty.ClientID %>").val(data.d[0]);
                $("#<%= ddlTypeAddEditProperty.ClientID %>").val(data.d[2]);
                $("#<%= txtPolicyLimitAddEditProperty.ClientID %>").val(data.d[3]);
                $("#<%= txtDeductibleAddEditProperty.ClientID %>").val(data.d[4]);
                $("#<%= ddlApplyToAddEditProperty.ClientID %>").val(data.d[7]);
                $("#<%= txtItvAddEditProperty.ClientID %>").val(data.d[5]);
                $("#<%= txtReserveAddEditProperty.ClientID %>").val(data.d[6]);

                $("#<%= txtCATDeductibleAddEditProperty.ClientID  %>").val(data.d[10]);
                $("#<%= txtCoInsuranceLimitAddEditProperty.ClientID %>").val(data.d[11]);

                var acrossll = data.d[8];
                var id = data.d[9];
                if (acrossll == 'True' && limitId != id) {
                    $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_txtDeductibleAddEditProperty").prop("disabled", true);
                }
                else {
                    $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_txtDeductibleAddEditProperty").prop("disabled", false);
                }




            },
                 error: function () {

                 }
             });



        }
        else {  //add mode
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/GetLossDataAddEditProperty") %>',
                success: function (data) {
                    $("#<%= txtCoverageAddEditProperty.ClientID %>").val(data.d[0]);
                    $("#<%= ddlTypeAddEditProperty.ClientID %>").val(data.d[2]);
                    $("#<%= txtPolicyLimitAddEditProperty.ClientID %>").val(data.d[3]);
                    $("#<%= txtDeductibleAddEditProperty.ClientID %>").val(data.d[4]);
                    $("#<%= ddlApplyToAddEditProperty.ClientID %>").val(data.d[7]);
                    $("#<%= txtItvAddEditProperty.ClientID %>").val(data.d[5]);
                    $("#<%= txtReserveAddEditProperty.ClientID %>").val(data.d[6]);
                    $("#<%= txtCATDeductibleAddEditProperty.ClientID  %>").val(data.d[10]);
                    $("#<%= txtCoInsuranceLimitAddEditProperty.ClientID %>").val(data.d[11]);
                    var acrossll = data.d[8];
                    var id = data.d[9];



                    if (acrossll == 'True' && limitId != id) {
                        $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_txtDeductibleAddEditProperty").prop("disabled", true);
                    }
                    else {
                        $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_txtDeductibleAddEditProperty").prop("disabled", false);
                    }
                },
                error: function () {

                }
            });


        }



        $("#divLossDetailsPopUpAddEditProperty").dialog({
            modal: false,
            width: 700,
            close: function (e, ui) {
                $(this).dialog('destroy');
                window.location.href = window.location.href;
            },
            buttons:
                       {
                           //'Done': function () {
                           //    $(this).dialog('close');
                           //}
                       }
        });

        return false;
    }












    function SaveLossDetailsAddEditProperty() {

        var policyID = parseInt($("[id$='hdnPolicyIdDetuctible']").val());       
        if(policyID>0)
        {
            //for edit mode
            var coverage = $("#<%= txtCoverageAddEditProperty.ClientID %>").val();
            var type = $("#<%= ddlTypeAddEditProperty.ClientID %>").val();
            var policyLimit = $("#<%= txtPolicyLimitAddEditProperty.ClientID %>").val();
            var deductible = $("#<%= txtDeductibleAddEditProperty.ClientID %>").val();
            var applyTo = $("#<%= ddlApplyToAddEditProperty.ClientID %>").val();
            var itv = $("#<%= txtItvAddEditProperty.ClientID %>").val();
            var reserve = $("#<%= txtReserveAddEditProperty.ClientID %>").val();   

            var acrossall = parseInt($("[id$='hdnApplyDeductible']").val());
            var limitId = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_hdnLimitIdForEdit").val();           

            var catDeductible = $("#<%= txtCATDeductibleAddEditProperty.ClientID %>").val();
            var coInsuranceLimit = $("#<%= txtCoInsuranceLimitAddEditProperty.ClientID %>").val();



            var myParams = "{'policyID':'" + policyID + "','coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "','limitId':'" + limitId + "','catDeductible':'" + catDeductible + "','coInsuranceLimit':'" + coInsuranceLimit + "'}";


            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveLossDetailsEditPropertyEdit") %>',
            success: function (data) {
                window.location.href = window.location.href;
            },
            error: function () {

            }
             });

        }
        else {
            //for add mode
            var coverage = $("#<%= txtCoverageAddEditProperty.ClientID %>").val();
            var type = $("#<%= ddlTypeAddEditProperty.ClientID %>").val();
            var policyLimit = $("#<%= txtPolicyLimitAddEditProperty.ClientID %>").val();
            var deductible = $("#<%= txtDeductibleAddEditProperty.ClientID %>").val();
            var applyTo = $("#<%= ddlApplyToAddEditProperty.ClientID %>").val();
            var itv = $("#<%= txtItvAddEditProperty.ClientID %>").val();
            var reserve = $("#<%= txtReserveAddEditProperty.ClientID %>").val();

            var acrossall = parseInt($("[id$='hdnApplyDeductibleAdd']").val());
            var limitId = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_policyEditForm_hdnLimitIdForEdit").val();

            var catDeductible = $("#<%= txtCATDeductibleAddEditProperty.ClientID %>").val();
            var coInsuranceLimit = $("#<%= txtCoInsuranceLimitAddEditProperty.ClientID %>").val();





            var myParams = "{'coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "','limitId':'" + limitId + "','catDeductible':'" + catDeductible + "','coInsuranceLimit':'" + coInsuranceLimit + "'}";
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveLossDetailsAddEditProperty") %>',
                success: function (data) {
                    window.location.href = window.location.href;
                },
                error: function () {

                }
           });




        }

    }




</script>



