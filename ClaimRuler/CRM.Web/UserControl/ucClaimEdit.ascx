<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimEdit.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimEdit" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControl/Admin/ucFeeDesignation.ascx" TagName="ucFeeDesignation" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ucClaimComments.ascx" TagName="ucClaimComments" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/ucClaimContacts.ascx" TagName="ucClaimContacts" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/ucClaimDocuments.ascx" TagName="ucClaimDocuments" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/ucClaimPropertyLimits.ascx" TagName="ucClaimPropertyLimits" TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/Admin/ucPolicySubLimit.ascx" TagName="ucPolicySubLimit" TagPrefix="uc6" %>
<%@ Register Src="~/UserControl/ucClaimCasualtyLimits.ascx" TagName="ucClaimCasualtyLimits" TagPrefix="uc7" %>
<%@ Register Src="~/UserControl/ucClaimSubLimits.ascx" TagName="ucClaimSubLimits" TagPrefix="uc8" %>
<%@ Register Src="~/UserControl/ucContactList.ascx" TagName="ucContactList" TagPrefix="uc9" %>

<%--<script type="text/javascript" src="http://code.jquery.com/jquery-1.7.1.js"></script> --%>

<div class="message_area">
    <asp:Label ID="lblMessage" runat="server" />
</div>
 
<asp:Panel ID="pnlContent" runat="server">
    <ajaxToolkit:TabContainer ID="tabContainerClaim" runat="server" Width="100%" ActiveTabIndex="0" OnClientActiveTabChanged="clientActiveTabChanged">
        <ajaxToolkit:TabPanel ID="tabPanelClaim" runat="server">
            <HeaderTemplate>
                Claim						            
            </HeaderTemplate>
            <ContentTemplate>
                <table style="width: 100%" class="editForm no_min_width">
                    <tr>
                        <td class="top left">
                            <table style="width: 100%">
                                <tr id="f_1" runat="server">
                                    <td class="right" runat="server">Policy Number</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <ig:WebTextEditor runat="server" ID="txtPolicyNumber" TabIndex="1" Enabled="False" />
                                    </td>
                                </tr>
                                <tr id="f_2" runat="server">
                                    <td class="right" runat="server">Insurer Claim #</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtInsurerClaimNumber" runat="server" TabIndex="2" MaxLength="50" />

                                    </td>

                                </tr>
                                <tr id="f_3" runat="server">
                                    <td class="right" runat="server">Adjuster File #</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtClaimNumber" runat="server" TabIndex="2" MaxLength="50" />

                                        <span class="redstar nowrap">(Leave blank to auto generate)</span>
                                    </td>

                                </tr>
                                 <!-- Added two new fields--->
                                 <tr id="Tr2" runat="server">
                                    <td id="Td1" class="right" runat="server">Adjuster Branch</td>
                                    <td id="Td2" runat="server">&nbsp;</td>
                                    <td id="Td3" runat="server">
                                        <ig:WebTextEditor ID="txtAdjusterBranch" Enabled="False" runat="server" TabIndex="2" MaxLength="100" />

                                        
                                    </td>

                                </tr>

                                 <tr id="Tr3" runat="server">
                                    <td id="Td4" class="right" runat="server">Branch Code</td>
                                    <td id="Td5" runat="server">&nbsp;</td>
                                    <td id="Td6" runat="server">
                                        <ig:WebTextEditor ID="txtBranchCode" runat="server" Enabled="False" TabIndex="2" MaxLength="50" />                                        
                                    </td>
                                </tr>

                                <tr id="f_4" runat="server">
                                    <td class="right" runat="server">Type of Policy</td>
                                    <td runat="server"></td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtPolicyType" runat="server" Enabled="False" TabIndex="3" Width="250px" />

                                    </td>
                                </tr>
                                <tr id="f_5" runat="server">
                                    <td class="right" runat="server">Client</td>
                                    <td runat="server"></td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtInsuranceCarrier" runat="server" Enabled="False" TabIndex="4" Width="250px" />

                                    </td>
                                </tr>
                                 <tr id="f_11" runat="server">
                                    <td class="right" runat="server">Insurer/Branch</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlOwnerManagerEntityName" runat="server" TabIndex="9" />
                                    </td>
                                </tr>
                                <tr id="f_6" runat="server">
                                    <td class="right" runat="server">Insured</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtPolicyHolder" runat="server" Enabled="False" TabIndex="5" Width="250px" />

                                    </td>
                                </tr>
                              <%--  <tr id="f_7" runat="server">
                                    <td class="right" runat="server">Claimant Entity (if any)</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtBusinessName" runat="server" Enabled="False" TabIndex="5" Width="250px" />

                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="right">Adjuster
                                    </td>
                                    <td class="redstar"></td>
                                    <td class="nowrap">
                                        <ig:WebTextEditor ID="txtAdjuster" runat="server" Enabled="False" Width="250px" />

                                        <a href="javascript:findAdjusterDialog();">
                                            <asp:Image ID="imgAdjusterFind" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />

                                        </a>
                                        <div>
                                           <%-- <asp:RequiredFieldValidator ID="rfvAdjuster" runat="server" ControlToValidate="txtAdjuster"
                                                Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select adjuster."
                                                ValidationGroup="claim" />--%>

                                        </div>
                                    </td>
                                </tr>
                                <tr id="f_35" runat="server">
                                    <td class="right" runat="server">Inside Adjuster</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlOutsideAdjuster" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_36" runat="server">
                                    <td class="right" runat="server">Contents Adjuster</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlContentsAdjuster" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_37" runat="server">
                                    <td class="right" runat="server">Examiner</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlExaminer" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                 <tr id="Tr4" runat="server">
                                    <td class="right" runat="server">Estimator</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlEstimator" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                 <tr id="Tr5" runat="server">
                                    <td class="right" runat="server">Desk Adjuster</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlDeskAdjuster" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_38" runat="server">
                                    <td class="right" runat="server">Company Builder</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlCompanyBuilder" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_39" runat="server">
                                    <td class="right" runat="server">Company Inventory</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlCompanyInventory" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_40" runat="server">
                                    <td class="right" runat="server">Our Builder</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlOurBuilder" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_41" runat="server">
                                    <td class="right" runat="server">Inventory Company</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlInventoryCompany" runat="server" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr id="f_8" runat="server">
                                    <td class="right" runat="server">Supervisor</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlSupervisors" runat="server" TabIndex="7" />
                                    </td>
                                </tr>
                                <tr id="f_9" runat="server">
                                    <td class="right" runat="server">Team Lead</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlTeamLeader" runat="server" TabIndex="8" />
                                    </td>
                                </tr>
                                <tr id="f_10" runat="server">
                                    <td class="right" runat="server">Carrier Owner/Manager Name</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlManager" runat="server" TabIndex="9" />
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td class="right">Adj Comm % Override</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ig:WebPercentEditor ID="txtAdjCommOveride" runat="server" MinDecimalPlaces="2" onblur="disabletxtAdjCommFlatFeeOveride()" DataMode="Decimal" TabIndex="10" Width="250px" />

                                    </td>
                                </tr>
                                <tr>
                                    <td class="right"></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <span class="redstar nowrap">(Please enter data in ONLY the Adjuster Commission %
                                            <br />
                                            Override OR the Adjuster Commission Flat Fee )</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Adj Comm Flat Fee Override</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ig:WebCurrencyEditor ID="txtAdjCommFlatFeeOveride" runat="server" onblur="disabletxtAdjCommOveride()" TabIndex="10" Width="250px" />


                                    </td>
                                </tr>
                                <tr id="f_12" runat="server">
                                    <td class="right" runat="server">Severity</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebNumericEditor ID="txtSeverity" runat="server" DataMode="Int" TabIndex="11" MinValue="1" MaxValue="9">
                                            <Buttons SpinButtonsDisplay="OnRight"
                                                SpinOnReadOnly="True" SpinWrap="True" />
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr id="f_13" runat="server">
                                    <td class="right" runat="server">Event Type</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtEventType" runat="server" MaxLength="50" TabIndex="11" Width="245px" />
                                    </td>
                                </tr>
                                <tr id="f_14" runat="server">
                                    <td class="right" runat="server">Event Name</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtEventName" runat="server" MaxLength="50" TabIndex="12" Width="245px" />
                                    </td>
                                </tr>

                                <tr id="c_15" runat="server">
                                    <td class="right">Cat ID #</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <ig:WebTextEditor ID="txtCat" runat="server" MaxLength="12" TabIndex="13" Width="245px" />
                                    </td>
                                </tr>
                                <tr id="f_15" runat="server">
                                    <td class="right" runat="server">Claim Workflow Type</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtClaimWorkflowType" runat="server" MaxLength="20" TabIndex="13" Width="245px" />
                                    </td>
                                </tr>

                             

                              
                                 <tr>
                                    <td class="right">
                                        <span>Loss Of Use Amount</span>
                                    </td>
                                    <td></td>
                                    <td>
                                         <ig:WebNumericEditor ID="txtLossAmount" runat="server" DataMode="decimal" TabIndex="15" MaxDecimalPlaces="2" MaxLength="9" Width="242px">
                                                </ig:WebNumericEditor>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                         <span>Loss Of Use Reserve</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <ig:WebNumericEditor ID="txtLossReserve" runat="server" DataMode="decimal" TabIndex="15" MaxDecimalPlaces="2" MaxLength="9" Width="242px">
                                                </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="right">
                                         <span>Tax Jurisdiction</span>
                                    </td>
                                    <td></td>
                                    <td>                                       
                                         <ig:WebTextEditor ID="txttaxJurisdiction" runat="server" MaxLength="50" TabIndex="15" Width="245px" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="right">
                                         <span>Overhead</span>
                                    </td>
                                    <td></td>
                                    <td>      
                                         <ig:WebNumericEditor ID="txtOverHead" runat="server" DataMode="decimal" TabIndex="15" MaxDecimalPlaces="2" MaxLength="5" Width="242px">
                                        </ig:WebNumericEditor>                                     
                                        <%-- <ig:WebTextEditor ID="txtOverHead" runat="server" MaxLength="5" TabIndex="15" Width="245px" />--%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="right">
                                         <span>Profit</span>
                                    </td>
                                    <td></td>
                                    <td>       
                                        <ig:WebNumericEditor ID="txtProfit" runat="server" DataMode="decimal" TabIndex="15" MaxDecimalPlaces="2" MaxLength="5" Width="242px">
                                        </ig:WebNumericEditor>                                  
                                        <%-- <ig:WebTextEditor ID="txtProfit" runat="server" MaxLength="5" TabIndex="15" Width="245px" />--%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="right">
                                         <span>Cumulative OP</span>
                                    </td>
                                    <td></td>
                                    <td>
                                          <ig:WebTextEditor ID="txtCumulative" runat="server" MaxLength="50" TabIndex="15" Width="245px" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="right">
                                         <span>Default Repaired By</span>
                                    </td>
                                    <td></td>
                                    <td>
                                          <ig:WebTextEditor ID="txtDefaultRepairedBY" runat="server" MaxLength="80" TabIndex="15" Width="245px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                         <span>Depreciation Material</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox id="chkDepriationMat" runat="server" TabIndex="15"/>
                                          
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                         <span>Depreciation Non Material</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox id="chkDepriationNonMat" runat="server" TabIndex="15"/>
                                         
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                         <span> Depreciation Taxes</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox id="chkDepriationTaxes" runat="server" TabIndex="15"/>
                                         
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                         <span>Max Depreciation</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <ig:WebNumericEditor ID="txtMaxDepriation" runat="server" DataMode="decimal" TabIndex="15" MaxDecimalPlaces="2" MaxLength="9" Width="242px">
                                        </ig:WebNumericEditor>                                         
                                    </td>
                                </tr>

                            </table>
                        </td>
                        <td class="top left">
                            <table style="width: 100%">
                                <tr>
                                    <td class="right middle">Progress</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlProgressStatus" runat="server" TabIndex="16" />
                                        <div>
                                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlProgressStatus" ValidationGroup="claim"
                                                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please select progress status." CssClass="validation1" InitialValue="0" />--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right middle">Status</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlLeadStatus" runat="server" TabIndex="16" onclick="return HandleStatusClick();" />
                                        <div>
                                            <%--<asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlLeadStatus" ValidationGroup="claim"
                                                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please select status." CssClass="validation1" InitialValue="0" />--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="f_16" runat="server">
                                    <td class="right" runat="server">Sub Status
                                    </td>
                                    <td runat="server"></td>
                                    <td runat="server">
                                        <asp:DropDownList ID="ddlSubStatus" runat="server" TabIndex="17" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"></td>
                                </tr>
                                   <tr id="Tr1" runat="server">
                                    <td class="right">Type of Loss </td>
                                    <td>&nbsp;</td>
                                    <td>

                                        <asp:DropDownList ID="ddlTypeOfLoss" runat="server" TabIndex="15">
                                            <asp:ListItem Text="None" Value="None"></asp:ListItem>
                                            <asp:ListItem Text="Backup of Sewer or Drain " Value="Backup of Sewer or Drain"></asp:ListItem>
                                            <asp:ListItem Text="Collapse" Value="Collapse"></asp:ListItem>
                                            <asp:ListItem Text="EarthQuake" Value="EarthQuake"></asp:ListItem>
                                            <asp:ListItem Text="Fire" Value="Fire"></asp:ListItem>
                                            <asp:ListItem Text="Flood" Value="Flood"></asp:ListItem>
                                            <asp:ListItem Text="Freeze" Value="Freeze"></asp:ListItem>
                                            <asp:ListItem Text="Hail" Value="Hail"></asp:ListItem>
                                            <asp:ListItem Text="Hurricane" Value="Hurricane"></asp:ListItem>
                                            <asp:ListItem Text="Ice/Snow-Weight of" Value="Ice/Snow-Weight of"></asp:ListItem>
                                            <asp:ListItem Text="Lightning" Value="Lightning"></asp:ListItem>
                                            <asp:ListItem Text="other" Value="other"></asp:ListItem>
                                            <asp:ListItem Text="Sewage" Value="Sewage"></asp:ListItem>
                                            <asp:ListItem Text="Smoke" Value="Smoke"></asp:ListItem>
                                            <asp:ListItem Text="Theft" Value="Theft"></asp:ListItem>
                                            <asp:ListItem Text="Tornado" Value="Tornado"></asp:ListItem>
                                            <asp:ListItem Text="Vandalism" Value="Vandalism"></asp:ListItem>
                                            <asp:ListItem Text="Vehicle" Value="Vehicle"></asp:ListItem>
                                            <asp:ListItem Text="Water Damage" Value="Water Damage"></asp:ListItem>
                                            <asp:ListItem Text="Wind Damage" Value="Wind Damage"></asp:ListItem>

                                        </asp:DropDownList>
                                        <%--<ig:WebTextEditor ID="txtTypeofLoss" runat="server" MaxLength="50" TabIndex="15" Width="245px" />--%>
                                    </td>
                                </tr>
                                  <tr>
                                    <td class="right">Cause of Loss</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <div id="divdamage" style="height: 100px; width: 250px; overflow: auto;" class="checkboxlist_container">
                                            <asp:CheckBoxList ID="chkLossType" runat="server" Width="230px" TabIndex="15"></asp:CheckBoxList>
                                        </div>
                                        <%--<div>
                                            <asp:CustomValidator ID="CustomValidator" runat="server" ValidationGroup="Policy"
                                                CssClass="validation1" ErrorMessage="Please select loss type." Display="Dynamic" OnServerValidate="CustomValidator_chkLossType"
                                                SetFocusOnError="True" ClientValidationFunction="validateCheckBoxList"></asp:CustomValidator>
                                        </div>--%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="right">Loss Description</td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtLossDescription" runat="server" TextMode="MultiLine" Height="50px" Width="250px"></ig:WebTextEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Date of Loss
                                    </td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtLossDate" runat="server" TabIndex="18" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                        <%--<div>
                                            <asp:RequiredFieldValidator ID="rfvLossDate" runat="server" ControlToValidate="txtLossDate" ValidationGroup="claim"
                                                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter loss date." CssClass="validation1" />
                                        </div>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Date Received</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtDateOpened" runat="server" TabIndex="19" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                        <%--<div>
                                            <asp:RequiredFieldValidator ID="rfvDateOpen" runat="server" ControlToValidate="txtDateOpened" ValidationGroup="claim"
                                                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter date open." CssClass="validation1" />
                                        </div>--%>
                                    </td>
                                </tr>
                                   <tr>
                                    <td class="right">Date Entered</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtDateEntered" runat="server" TabIndex="20" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                        <%--<div>
                                            <asp:RequiredFieldValidator ID="rfvDateOpen" runat="server" ControlToValidate="txtDateOpened" ValidationGroup="claim"
                                                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter date open." CssClass="validation1" />
                                        </div>--%>
                                    </td>
                                </tr>
                                <tr id="f_17" runat="server">
                                    <td class="right" runat="server">Date of Initial Reserve Changed</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateInitialReservedChanged" runat="server" TabIndex="21" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_18" runat="server">
                                    <td class="right" runat="server">Date Assigned</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateAssigned" runat="server" TabIndex="21" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <ClientSideEvents ValueChanged="calculateCycleTime" />
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_19" runat="server">
                                    <td class="right" runat="server">Date Acknowledged</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateAcknowledge" runat="server" TabIndex="22" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_20" runat="server">
                                    <td class="right" runat="server">Date First Contact Attempted</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateFirstContactAttempted" runat="server" TabIndex="23" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_21" runat="server">
                                    <td class="right" runat="server">Date Contacted</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateContacted" runat="server" TabIndex="24" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_22" runat="server">
                                    <td class="right" runat="server">Date Inspection Scheduled</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateInspectionSchedule" runat="server" TabIndex="25" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_23" runat="server">
                                    <td class="right" runat="server">Date Inspection Completed</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateInspectionCompleted" runat="server" TabIndex="26" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                   <tr>
                                    <td class="right">Date Project Completed</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtDateProjCompleted" runat="server" TabIndex="19" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                        <%--<div>
                                            <asp:RequiredFieldValidator ID="rfvDateOpen" runat="server" ControlToValidate="txtDateOpened" ValidationGroup="claim"
                                                SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter date open." CssClass="validation1" />
                                        </div>--%>
                                    </td>
                                </tr>
                                <tr id="f_24" runat="server">
                                    <td class="right" runat="server">Date Submitted</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateSubmitted" runat="server" TabIndex="27" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr id="f_25" runat="server">
                                    <td class="right" runat="server">Date Indemnity Payment Requested</td>
                                    <td class="redstar" runat="server"></td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateIndemnityRequetsted" runat="server" TabIndex="28" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>
                                </tr>
                                <tr id="f_26" runat="server">
                                    <td class="right" runat="server">
                                        <div>
                                            Date Indemnity Payment Approved
                                        </div>
                                    </td>
                                    <td runat="server"></td>
                                    <td runat="server">

                                        <ig:WebDatePicker ID="txtDateIndemnityPaymentApproved" runat="server" TabIndex="29" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>
                                </tr>
                                <tr id="f_27" runat="server">
                                    <td class="right" runat="server">Date Indemnity Payment Issued</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">

                                        <ig:WebDatePicker ID="txtDateIndemnityPaymentIssued" runat="server" TabIndex="30" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Date Closed</td>
                                    <td class="redstar">&nbsp;</td>
                                    <td>
                                        <ig:WebDatePicker ID="txtDateClosed" runat="server" TabIndex="31" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <ClientSideEvents ValueChanged="calculateCycleTime" />

                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>

                                </tr>
                                <tr id="f_28" runat="server">
                                    <td class="right" runat="server">Date First Reopen</td>
                                    <td class="redstar" runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateFirstReopen" runat="server" TabIndex="32" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <ClientSideEvents ValueChanged="calculateReopenCycleTime" />

                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>
                                </tr>
                                <tr id="f_29" runat="server">
                                    <td class="right" runat="server">Date <span class="red">Reopen</span> Completed</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">
                                        <ig:WebDatePicker ID="txtDateReopenCompleted" runat="server" TabIndex="33" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <ClientSideEvents ValueChanged="calculateReopenCycleTime" />

                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>
                                </tr>
                                <tr id="f_30" runat="server">
                                    <td class="right" runat="server">Date <span class="red">Final</span> Closed</td>
                                    <td runat="server">&nbsp;</td>
                                    <td runat="server">

                                        <ig:WebDatePicker ID="txtDateFinalClosed" runat="server" TabIndex="34" CssClass="date_picker" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                            <Buttons>
                                                <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                            </Buttons>
                                        </ig:WebDatePicker>

                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="right">Cycle Time</td>
                                    <td>
                                        <div class="helptip">
                                            <img runat="server" src="~/images/help.png" title="Claim Ruler calculates cycle times for you.  Please make sure the Date Opened and Date Closed have dates in them, not Final Closed since that is used for Reopen Cycle Time." />

                                            &nbsp;&nbsp; &nbsp;
                                        </div>
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtCycleTime" runat="server" Enabled="False" CssClass="no_min_width" TabIndex="35" Width="125px" />


                                    </td>
                                </tr>
                                <tr id="f_31" runat="server">
                                    <td class="right" runat="server">
                                        <span>Reopen Cycle Time</span>

                                    </td>
                                    <td runat="server"></td>
                                    <td runat="server">
                                        <ig:WebTextEditor ID="txtReopenCycleTime" runat="server" Enabled="False"
                                            CssClass="no_min_width" TabIndex="36" Width="125px" />

                                    </td>
                                </tr>

                                <tr>
                                    <td class="right">
                                        <span>Job Size Code</span>

                                    </td>
                                    <td></td>
                                    <td>

                                        <asp:DropDownList ID="ddlJobSideCode" runat="server" TabIndex="37">
                                            <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Small" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Mod" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Medium" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Large" Value="4"></asp:ListItem>
                                             <asp:ListItem Text="Very Large" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="XL" Value="6"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                        <span>Estimate Count</span>

                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList ID="ddlEstimateCount" runat="server" TabIndex="38">
                                            <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Assignment" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Redo" Value="1"></asp:ListItem>
                                        </asp:DropDownList>


                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="right">
                                        <span>Carrier Name</span>

                                    </td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtCarrierName" runat="server" Width="241px"
                                            TabIndex="39" />

                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="right">
                                        <span>Mitigation</span>

                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:DropDownList ID="ddlMitigation" runat="server" TabIndex="39">
                                            <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">
                                        <span>Profile Code</span>
                                    </td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtProfileCode" runat="server" Width="241px"
                                            TabIndex="40" />

                                    </td>
                                </tr>
                               

                            </table>
                        </td>
                        <td class="top left">
                            <div id="f_32" runat="server" class="boxContainer">
                                <div class="section-title">
                                    Loss Totals
                                </div>
                                <table style="width: 100%">
                                     <tr>
                                        <td class="right">Invoice Type</td>
                                        <td></td>
                                        <td tabindex="41">
                                            <asp:DropDownList ID="ddlInvoiceType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Carrier Invoice Profile</td>
                                        <td></td>
                                        <td tabindex="41">
                                            <asp:DropDownList ID="ddlCarrierInvoiceProfile" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Fee Invoice Designation</td>
                                        <td></td>
                                        <td tabindex="42">
                                            <uc1:ucFeeDesignation ID="ddlPropertyFeeInvoiceDesignation" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Gross Claim Payable</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtGrossLossPayable" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="43">
                                                <ClientEvents ValueChanged="calculateNetClaimPayable" />
                                            </ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Recoverable Depreciation</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtDepreciation" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="44">
                                                <ClientEvents ValueChanged="calculateNetClaimPayable" />
                                            </ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Non Recoverable Depreciation</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtNonDepreciation" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="45">
                                                <ClientEvents ValueChanged="calculateNetClaimPayable" />
                                            </ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Deductible</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtPolicyDeductible" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="46">
                                                <ClientEvents ValueChanged="calculateNetClaimPayable" />
                                            </ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Net Claim Payable</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtNetClaimPayable" runat="server" MinDecimalPlaces="2" DataMode="Decimal" Enabled="False" TabIndex="47" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right"><%--Auto-Invoice Claim--%></td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="cbxInvoiceReady" runat="server" TabIndex="48" OnCheckedChanged="cbxInvoiceReady_CheckedChanged" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div runat="server" class="boxContainer" visible="false">
                                <div class="section-title">
                                    Casualty Loss Totals
                                </div>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="right">Fee Invoice Designation</td>
                                        <td class="redstar"></td>
                                        <td tabindex="43">
                                            <uc1:ucFeeDesignation ID="ddlCasualtyFeeInvoiceDesignation" runat="server" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Gross Claim Payable</td>
                                        <td class="redstar"></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtCasualtyGrossClaimPayable" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="49">
                                                <ClientEvents ValueChanged="calculateNetClaimPayable" />
                                            </ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="f_34" runat="server" class="boxContainer">
                                <div class="section-title">
                                    Carrier Finance Details(Optional)
                                </div>
                                <table style="width: 100%">
                                    <tr>
                                        <td class="right">Outstanding Indemnity Reserve</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtOutstandingIndemnityReserve" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="50"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Outstanding LAE Reserves</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtOutstandingLAEReserves" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="51"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Total Indemnity Paid</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtTotalIndemnityPaid" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="52">
                                                <ClientEvents ValueChanged="calculateNetClaimPayable" />
                                            </ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Coverage A Paid</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtCoverageAPaid" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="53"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Coverage B Paid</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtCoverageBPaid" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="54"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Coverage C Paid</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtCoverageCPaid" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="55"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Coverage D Paid</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtCoverageDPaid" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="56"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="right">Total Expense Paid</td>
                                        <td></td>
                                        <td>
                                            <ig:WebCurrencyEditor ID="txtTotalExpensePaid" runat="server" MinDecimalPlaces="2" DataMode="Decimal" TabIndex="57"></ig:WebCurrencyEditor>

                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="top left" colspan="3">
                            <div class="boxContainer">
                                <div class="section-title">
                                    Loss Details
                                </div>
                                  <%-- <div>
                                     <table>
                                        <tr>
                                            <td >  
                                                Apply Deductible
                                            </td>
                                            <td>
                                                <asp:RadioButton id="acrossAllCoverages" runat="server" Text="Across All Coverages" ClientIDMode="Static" GroupName="A"/>
                                                <asp:RadioButton id="coverageSpecific" runat="server" Text="Coverage Specific" ClientIDMode="Static" GroupName="A"/>
                                            
                                                <asp:HiddenField id="hdnApplyDeductible" runat="server" Value="0"/>
                                            </td>                                           
                                        </tr>
                                    </table>
                                </div>--%>
                                  <%-- <div>


                                    <table>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button id="btnShowLossTemplate" runat="server" CssClass="mysubmit" Text="Use Template" OnClick="btnShowLossTemplate_Click" OnClientClick="javascript:return ConfirmDialog(this, 'Warning:  If you use template, then it may not be recoverable.  Please do not delete unless absolutely necessary.  Are you sure you want to delete this record forever?');"/>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnLossDetailsPopUpOpen" runat="server" CssClass="mysubmit" Text="Add Loss Details" OnClientClick="return LossDetailsPopUp();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                               

                                <uc5:ucClaimPropertyLimits ID="propertyLimits" runat="server" />
                               <%-- <div id="divLossAmount" runat="server">
                                    <table>
                                        <tr>
                                            <td>Loss Of Use:</td>
                                            <td>Amount</td>
                                            <td>
                                               

                                            </td>
                                            <td>Reserve</td>
                                            <td>
                                               

                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>


                            </div>

                        </td>
                       
                    </tr>
                  <%--  <tr>
                         <td class="top left" colspan="3">
                            <div class="boxContainer">
                                <div class="section-title">
                                    Casualty Loss Details
                                </div>
                                <uc7:ucClaimCasualtyLimits ID="casualtyLimits" runat="server" />

                            </div>
                        </td>
                    </tr>--%>

                    <tr>
                        <td class="top left" colspan="3">
                            <div class="boxContainer">
                                <div class="section-title">
                                    Sub-Limits
                                </div>
                                <uc8:ucClaimSubLimits ID="propertySubLimits" runat="server" />

                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>

            </ContentTemplate>

        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tabPanelComments" runat="server">
            <HeaderTemplate>
                <%--Diary	--%>	
                Claim Log				            
            </HeaderTemplate>
            <ContentTemplate>
                <asp:Button ID="btnShowDiary" runat="server" Style="display: none;"   OnClick="btnShowDiary_Click" />
                <asp:Panel ID="pnlDiary" runat="server" Visible="false">
                    <uc2:ucClaimComments ID="claimComments" runat="server" />
                </asp:Panel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tabPanelContacts" runat="server">
            <HeaderTemplate>
                Claim-Level Contacts						            
            </HeaderTemplate>

            <ContentTemplate>
                <asp:Button ID="btnHiddenShowContact" runat="server" Style="display: none;" OnClick="btnHiddenShowContact_Click" />
                <asp:Panel ID="pnlContacts" runat="server" Visible="false">
                    <uc3:ucClaimContacts ID="claimContacts" runat="server" />
                </asp:Panel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tabPanelDocuments" runat="server">
            <HeaderTemplate>
                Documents						            
            </HeaderTemplate>
            <ContentTemplate>
                <asp:Button ID="btnHiddenShowDocuments" runat="server" Style="display: none;" OnClick="btnHiddenShowDocuments_Click" />
                <asp:Panel ID="pnlDocument" runat="server" Visible="false">
                    <uc4:ucClaimDocuments ID="claimDocuments" runat="server" />
                </asp:Panel>

            </ContentTemplate>

        </ajaxToolkit:TabPanel>

    </ajaxToolkit:TabContainer>
</asp:Panel>

<asp:HiddenField ID="hf_adjusterID" runat="server" Value="0" />
<div id="divAdjustersList" style="display: none;" title="Select Adjuster">

    <ig:WebDataGrid ID="adjusterGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsAdjusters"
        AutoGenerateColumns="false" Height="300px"
        Width="100%">
        <Columns>
            <ig:BoundDataField DataFieldName="AdjusterId" Key="AdjusterId" Header-Text="ID" Width="50px" Hidden="true" />
            <ig:BoundDataField DataFieldName="AdjusterName" Key="AdjusterName" Header-Text="Adjuster Name" />
            <ig:BoundDataField DataFieldName="email" Key="email" Header-Text="E-mail Address" />
        </Columns>
        <Behaviors>

            <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                ThresholdFactor="0.5" Enabled="true" />

            <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                <SelectionClientEvents RowSelectionChanged="adjusterGrid_rowsSelected" />
            </ig:Selection>
        </Behaviors>
    </ig:WebDataGrid>

</div>


<asp:EntityDataSource ID="edsAdjusters" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
    EnableFlattening="False" EntitySetName="AdjusterMaster"
    Where="it.ClientId = @ClientID && it.Status = true"
    OrderBy="it.AdjusterName Asc">
    <WhereParameters>
        <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<div id="divAutomaticInvoiceMethodSelectionAlert" title="Automatic Invoice Method Alert" style="display: none">
    <p class="center red">Warning</p>
    <p>Please Configure an Automatic Invoice Method under Invoice Services to proceed with automated invoicing.  Your invoice was not created.  For manual invoicing, press Prepare Invoice above.</p>
</div>

<div id="div_ClaimStatusReview" style="display: none; width: 90%;" title="Claim Status/Review">
    <div class="boxContainer">
        <div class="section-title">
            Claim Status/Review
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td colspan="3" class="center">
                    <asp:Label ID="lblMsgSaveReview" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Update Status To</td>
                <td class="redstar">*</td>
                <td>
                    <asp:DropDownList ID="ddlClaimStatusReview" runat="server" Width="258px" TabIndex="53" />
                    <div>
                        <asp:Label ID="lblClaimStatusReview" runat="server" Text="Please select service type." Font-Size="Small" ForeColor="Red"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlClaimStatusReview" InitialValue="0"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select service type."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Insurer Claim ID #</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtInsurerClaimId" Width="250px" onblur="return BlankControl()" TabIndex="54" />
                    <div>
                        <asp:Label ID="lblInsurerClaimId" runat="server" Text="Please enter insurer claim #" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>

                </td>
            </tr>
            <tr>
                <td class="right top">Claimant</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtInsurerName" TabIndex="55" Width="250px" onblur="return BlankControl()" />
                    <div>
                        <asp:Label ID="lblInsurerName" runat="server" Text="Please enter insurer name." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right top">Client</td>
                <td class="redstar top">*</td>
                <td>
                    <%--<ig:WebTextEditor runat="server" ID="txtCarrier" TabIndex="1" Width="250px" />--%>
                    <asp:DropDownList ID="ddlClaimCarrier" runat="server" Width="258px" TabIndex="56" />
                    <div>
                        <asp:Label ID="lblClaimCarrier" runat="server" Text="Please select carrier." Font-Size="Small" ForeColor="Red"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Adjuster
                </td>
                <td class="redstar top">*</td>
                <td class="nowrap">
                    <ig:WebTextEditor ID="txtClaimAdjuster" runat="server" Enabled="false" Width="250px" onblur="return BlankControl()" TabIndex="57" />
                    <a href="javascript:findAdjusterForClaimDialog();">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />
                    </a>
                    <div>
                        <asp:Label ID="lblClaimAdjuster" runat="server" Text="Please select adjuster." Font-Size="Small" ForeColor="Red"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClaimAdjuster"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select adjuster."
                            ValidationGroup="service" />
                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">Adjuster Company Name</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtAdjusterComapnyName" TabIndex="58" Width="250px" onblur="return BlankControl()" ReadOnly="true" />
                    <div>
                      <%--  <asp:Label ID="lblAdjusterComapnyName" runat="server" Text="Please enter company name." Font-Size="Small" ForeColor="Red"></asp:Label>--%>
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td class="right top">Updated By</td>
                <td class="redstar top">*</td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtUpdatedby" TabIndex="1"  Width="250px"/>
                  
                </td>
            </tr>--%>
            <tr>
                <td class="right top">Comment/Note</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtCommentNote" MaxLength="500" TabIndex="59" TextMode="MultiLine" MultiLine-Rows="3" Width="100%" onblur="return BlankControl()"></ig:WebTextEditor>
                    <div>

                        <asp:Label ID="lblCommentNote" runat="server" Text="Please enter comment." Font-Size="Small" ForeColor="Red"></asp:Label>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCommentNote" ValidationGroup="service"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter description." CssClass="validation1" />
                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">E-mail To</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor ID="txtEmailTo" runat="server" Width="100%" TabIndex="60" disabled="true" />
                    <%-- onblur="return CheckEmailTo()"--%>
                    <div>
                        <%--<asp:Label ID="lblEmailTo" runat="server" Text="Please select any recipient." Font-Size="Small" ForeColor="Red"></asp:Label>--%>

                    </div>
                </td>
            </tr>

            <tr>
                <td class="right top">Select Recipients</td>
                <td class="redstar top"></td>
                <td>
                    <div style="width: 552px; height: 250px; overflow-y: auto; border: 1px solid;">
                        <asp:GridView Width="100%" ID="gvSelectRecipients" CssClass="gridView csk-box" runat="server" AutoGenerateColumns="False" CellPadding="2">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectRecipients" runat="server" Class="gvSelectRecipientsClass checkbox-align-center" onclick="return HandleCheckBox(this);" />
                                        <asp:HiddenField ID="hdnSelectRecipients" runat="server" Value='<%#Eval("ContactID") %>' />
                                        <asp:HiddenField ID="hdn2SelectRecipients" runat="server" Value='<%#Eval("IdOf") %>' />
                                        <asp:HiddenField ID="hdnEmailSelectRecipients" runat="server" Value='<%#Eval("Email") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name">
                                    <ItemTemplate>
                                        <%#Eval("FirstName") %>
                                        <%-- <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("SecUser.UserName") %>' />--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name">
                                    <ItemTemplate>
                                        <%#Eval("LastName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <ItemTemplate>
                                        <%#Eval("CompanyName") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <%#Eval("Email") %>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Contact Type">
                    <ItemTemplate>
                        <%#Eval("ContactType") %>                       
                    </ItemTemplate>
                    <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>

                    </div>
                </td>
            </tr>

            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveClaimNote" runat="server" Text="Save" CssClass="mysubmit"  OnClientClick="return SaveClaimStatus();" />
                    &nbsp;
                    <%--<asp:Button ID="btnCancelClaimService" runat="server" Text="Cancel" CssClass="mysubmit"   CausesValidation="false" />--%>
                </td>
            </tr>
        </table>


    </div>
</div>

<div id="div_ClaimAdjustersList" style="display: none; width: 90%;" title="Select Adjuster">
    <div class="boxContainer">
        <div class="section-title">
            Adjusters
        </div>
        <ig:WebDataGrid ID="claimAdjusterGrid" runat="server" CssClass="gridView smallheader" DataSourceID="EntityDataSource1"
            AutoGenerateColumns="false" Height="300px"
            Width="100%">
            <Columns>
                <ig:BoundDataField DataFieldName="AdjusterId" Key="AdjusterId" Header-Text="ID" Width="50px" />
                <ig:BoundDataField DataFieldName="AdjusterName" Key="AdjusterName" Header-Text="Adjuster Name" />
                <ig:BoundDataField DataFieldName="email" Key="email" Header-Text="E-mail Address" />
            </Columns>
            <Behaviors>

                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                    ThresholdFactor="0.5" Enabled="true" />

                <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                    <SelectionClientEvents RowSelectionChanged="claimAdjusterGrid_rowsSelected" />
                </ig:Selection>
            </Behaviors>
        </ig:WebDataGrid>
    </div>
</div>

<div id="div_StatusSave" style="display: none; width: 90%;" title="Status Change">
    <div class="boxContainer">
        Status Changed Successfully.
        
        
    </div>
</div>

<div id="div_StatusNotSave" style="display: none; width: 90%;" title="Status Change">
    <div class="boxContainer">
        Status Not Changed Successfully.
        
        
    </div>
</div>

<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
    EnableFlattening="False" EntitySetName="AdjusterMaster"
    Where="it.ClientId = @ClientID && it.Status = true"
    OrderBy="it.AdjusterName Asc">
    <WhereParameters>
        <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="hf_ClaimAdjusterID" runat="server" Value="0" />
<asp:HiddenField ID="hf_ClaimIdForStatus" runat="server" Value="0" />

<asp:HiddenField ID="hdnEmailToList" runat="server" Value="0" />



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
                <td class="right">Apply To</td>
                <td class="redstar"></td>
                <td>
                    <asp:DropDownList ID="ddlApplyTo" runat="server" Width="258px" TabIndex="53">
                        <asp:ListItem Text="ACV" Value="ACV"></asp:ListItem>
                        <asp:ListItem Text="RCV" Value="RCV"></asp:ListItem>
                        <asp:ListItem Text="Both" Value="Both"></asp:ListItem>
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



<script type="text/javascript">
    function automaticInvoiceMethodSelectionAlert() {
        $("#divAutomaticInvoiceMethodSelectionAlert").dialog({
            modal: true,
            width: 600,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
           {
               'Close': function () {
                   $(this).dialog('close');
               }
           }
        });
    }
</script>
<script type="text/javascript">
    function clientActiveTabChanged(sender, args) {
        switch (sender.get_activeTabIndex()) {
            case 1:     // diary
                $("#<%= btnShowDiary.ClientID %>").click();
                break;
            case 2:     // contacts
                $("#<%= btnHiddenShowContact.ClientID %>").click();
                break;
            case 3:     // documents
                $("#<%= btnHiddenShowDocuments.ClientID %>").click();
                break;
            default:
                break;
        }
    }
</script>
<script type="text/javascript">


    function findAdjusterDialog() {
        showHideDiv('div_adjusters');

        // show upload dialog
        $("#divAdjustersList").dialog({
            modal: false,
            width: 600,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
			{
			    'Done': function () {
			        $(this).dialog('close');
			    }
			}
        });
    }
    function adjusterGrid_rowsSelected(sender, args) {
        var selectedRows = args.getSelectedRows();

        var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
        var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();


        $('#<%= hf_adjusterID.ClientID %>').val(adjusterID);
        $find('<%= txtAdjuster.ClientID %>').set_value(adjusterName);
    }
</script>
<script type="text/javascript">
    function calculateCycleTime(sender, args) {
        var dateAssigned;
        var dateClosed;

        if ($find("<%= txtDateOpened.ClientID %>").get_value() != null)
            dateOpened = new Date($find("<%= txtDateOpened.ClientID %>").get_value().format('yyyy/MM/dd'));

        if ($find("<%= txtDateClosed.ClientID %>").get_value() != null)
            dateClosed = new Date($find("<%= txtDateClosed.ClientID %>").get_value().format('yyyy/MM/dd'));

        if (dateOpened != null && dateClosed != null) {
            var days = dateDiffInDays(dateOpened, dateClosed);
            $find("<%= txtCycleTime.ClientID %>").set_value(days);
        }
    }

    function calculateReopenCycleTime(sender, args) {
        var dateFirstReopen;
        var dateReopenCompleted;

        if ($find("<%= txtDateFirstReopen.ClientID %>").get_value() != null)
            dateFirstReopen = new Date($find("<%= txtDateFirstReopen.ClientID %>").get_value().format('yyyy/MM/dd'));

        if ($find("<%= txtDateReopenCompleted.ClientID %>").get_value() != null)
            dateReopenCompleted = new Date($find("<%= txtDateReopenCompleted.ClientID %>").get_value().format('yyyy/MM/dd'));

        if (dateFirstReopen != null && dateReopenCompleted != null) {
            var days = dateDiffInDays(dateFirstReopen, dateReopenCompleted);
            $find("<%= txtReopenCycleTime.ClientID %>").set_value(days);
        }
    }
    function dateDiffInDays(date1, date2) {
        var days = 0;


        date1 = Date.UTC(date1.getFullYear(), date1.getMonth(), date1.getDate());
        date2 = Date.UTC(date2.getFullYear(), date2.getMonth(), date2.getDate());
        if (parseFloat(date1) < parseFloat(date2)) {
            var ms = Math.abs(date1 - date2);
            days = Math.floor(ms / 1000 / 60 / 60 / 24);
        }
        return days;
    }

    function calculateNetClaimPayable(sender, e) {
        var txtNetClaimPayable = 0;

        var txtGrossLossPayable = $find("<%= txtGrossLossPayable.ClientID %>").get_value();
        var txtDepreciation = $find("<%= txtDepreciation.ClientID %>").get_value();

        var txtNonDepreciation = $find("<%= txtNonDepreciation.ClientID %>").get_value();
        var txtPolicyDeductible = $find("<%= txtPolicyDeductible.ClientID %>").get_value();

        if (txtGrossLossPayable > 0) {
            var netClaimPayable = txtGrossLossPayable - txtDepreciation - txtNonDepreciation - txtPolicyDeductible;

            $find("<%= txtNetClaimPayable.ClientID %>").set_value(netClaimPayable);
        }
    }
    function calculateGrossClaimPayable() {
        // calculate gross claim payable
        var lblTotalLossAmountACV = $("input[id$='lblTotalLossAmountACV']").val();
        var lblTotalLossAmountRCV = $("input[id$='lblTotalLossAmountRCV']").val();

        var totalLossAmountACV = 0;
        totalLossAmountACV = deformatAmount(lblTotalLossAmountACV);
        totalLossAmountACV = isNaN(totalLossAmountACV) ? 0 : totalLossAmountACV;

        var totalLossAmountRCV = 0;
        totalLossAmountRCV = deformatAmount(lblTotalLossAmountRCV);
        totalLossAmountRCV = isNaN(totalLossAmountRCV) ? 0 : totalLossAmountRCV;

        var grossLossPayable = totalLossAmountACV + totalLossAmountRCV;

        $find("<%= txtGrossLossPayable.ClientID %>").set_value(grossLossPayable);

        // recalculate net payable based on changes made at policy acv/rcv level
        calculateNetClaimPayable();
    }

    function totalGridColumn(sourceID, targetID) {
        var totalAmount = 0;
        $("input[id$='" + sourceID + "']").each(function () {
            var strAmount = $(this).val();
            var amount = deformatAmount(strAmount);
            if (isNaN(amount))
                amount = 0;

            totalAmount += amount;
        });

        //$("input[id$='" + targetID + "']").val(totalAmount);

        var controls = $("input[id$='" + targetID + "']");

        var ctrl_id = controls[0].id;

        $find(ctrl_id).set_value(totalAmount);

    }

    function IsCheckBoxChecked() {
        var isChecked = false;
        var list = document.getElementById('<%= chkLossType.ClientID %>');
        if (list != null) {
            for (var i = 0; i < list.rows.length; i++) {
                for (var j = 0; j < list.rows[i].cells.length; j++) {
                    var listControl = list.rows[i].cells[j].childNodes[0];
                    if (listControl.checked) {
                        isChecked = true;
                    }
                }
            }
        }
        return isChecked;

    }
</script>
<script type="text/javascript">
    // handle document upload
    function showDocumentUploadDialog() {
        // clear fields
        $("#txtDocumentDescription").val('');

        // build upload control
        $("#webUpload").igUpload({
            mode: 'single',
            //progressUrl: "http://appv3.claimruler.com/IGUploadStatusHandler.ashx",
            //progressUrl: "/samplesbrowser/IGUploadStatusHandler.ashx",
            progressUrl: "http://localhost:6375/IGUploadStatusHandler.ashx",
            onError: function (e, args) {
                
                showAlert(e);
                //System.Diagnostics.Debug.WriteLine("test -" + e);
            },
            fileUploading: function (e, args) {
                if (!validateDocumentUploadFields()) {
                    $("#txtDocumentDescription").focus();
                    return false;   // cancel upload
                }
            },
            fileUploaded: function (e, args) {
                // alert(args.fileID + " " + args.filePath);     
                // find hf_claimID in ucClaimDocuments.ascx
                var claimID = parseInt($("[id$='hf_claimID']").val());

                var documentDescription = $("#txtDocumentDescription").val();

                var categoryID = $("[id$='ddlDocumentCategory']").val();
                var documentCategoryID = parseInt(categoryID);

                PageMethods.saveFile(claimID, args.filePath, documentDescription, documentCategoryID);

                $("#documentUpload").dialog('close');

                $("[id$='btnHiddenRefreshDocument']").click();
            }
        });

        // show upload dialog
        $("#documentUpload").dialog({
            modal: false,
            width: 600,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
			{
			    'Close': function () {
			        $(this).dialog('close');
			    }
			}
        });
        // return false;
    }
    function validateDocumentUploadFields() {
        var isValid = true;
        var documentCategoryID = $("#ddlDocumentCategory").val();

        if (parseInt(documentCategoryID) < 1)
            isValid = false;

        return isValid;
    }

    function disabletxtAdjCommFlatFeeOveride() {
        // 
        var adjcommoveride = $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommOveride").val();
        //alert(adjcommoveride);
        if (adjcommoveride != "") {
            $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommFlatFeeOveride").prop('disabled', true);
            $(".Office2007BlueEditInContainer").focus();

        }
        else {
            $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommFlatFeeOveride").prop('disabled', false);
            $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommFlatFeeOveride").focus();
        }
    }

    function disabletxtAdjCommOveride() {
        // 
        var adjcommfeeoveride = $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommFlatFeeOveride").val();
        //alert(adjcommoveride);
        if (adjcommfeeoveride != "") {
            $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommOveride").prop('disabled', true);

        }
        else {
            $("#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_tabContainerClaim_tabPanelClaim_txtAdjCommOveride").prop('disabled', false);
        }
    }
</script>

<script type="text/javascript">
    var listEmail = [];

    function HandleCheckBox(elem) {
        var id = jQuery(elem).attr('id');
        var hdnfield = id.replace("chk", "hdnEmail");
        if (elem.checked) {
            var result = validateEmail($("#" + hdnfield).val());
            if (result) {
                if (!IsExists(listEmail, $("#" + hdnfield).val())) {
                    listEmail.push($("#" + hdnfield).val());
                }
                PopulateEmailTo(listEmail);
            } else {
                alert('You can not select this recipient as no email available for this recipient');
                elem.checked = false;
            }
        }
        else {
            listEmail = RemoveElementFromArray(listEmail, $("#" + hdnfield).val());
            PopulateEmailTo(listEmail);
        }

        var emailto = $("#<%= txtEmailTo.ClientID %>").val(stringEmailTo);
       // if (emailto == "") {
            //$("# lblEmailTo.ClientID %>").show();
      //  }
       // else {
         //   $("# lblEmailTo.ClientID %>").hide();
       // }

    }

    function PopulateEmailTo(listArray) {
        var stringEmailTo = ''
        for (counter = 0; counter < listArray.length; counter++) {
            if (counter != 0) {
                stringEmailTo += ";";
            }
            stringEmailTo += listArray[counter];
        }
        $("#<%= txtEmailTo.ClientID %>").val(stringEmailTo);

        var emailto = $("#<%= txtEmailTo.ClientID %>").val(stringEmailTo);
       // if (emailto == "") {
         //   $("# lblEmailTo.ClientID %>").show();
      //  }
      //  else {
         //   $("#lblEmailTo.ClientID %>").hide();
       // }
    }

    function IsExists(array, element) {
        for (var counter = 0; counter < array.length; counter++) {
            if (array[counter] == element) {
                return true;
            }
        }
        return false;
    }

    function RemoveElementFromArray(array, element) {
        var listArray = [];
        if (IsExists(array, element)) {
            for (var counter = 0; counter < array.length; counter++) {
                if (array[counter] != element) {
                    listArray.push(array[counter]);
                }
            }
        }
        return listArray;
    }

    function HandleStatusClick() {

        $("#<%= lblClaimStatusReview.ClientID %>").hide();
        $("#<%= lblInsurerClaimId.ClientID %>").hide();
        $("#<%= lblInsurerName.ClientID %>").hide();
        $("#<%= lblClaimAdjuster.ClientID %>").hide();
       // $("# lblAdjusterComapnyName.ClientID").hide();
        $("#<%= lblCommentNote.ClientID %>").hide();
        $("#<%= lblClaimCarrier.ClientID %>").hide();
       // $("#lblEmailTo.ClientID %>").hide();

        $("#div_ClaimStatusReview").dialog({
            modal: false,
            width: 761,
            close: function (e, ui) {
                $(this).dialog('destroy');

            },
            //buttons:
            //   {
            //       'Done': function () {                       
            //           $(this).dialog('close');
            //       }
            //   }
        });


        return false;
    }

    function findAdjusterForClaimDialog() {

        // show dialog
        $("#div_ClaimAdjustersList").dialog({
            modal: false,
            width: 600,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
               {
                   'Done': function () {
                       $(this).dialog('close');
                   }
               }
        });
    }






    function SaveClaimStatus() {
        var idOf = [];
        var recipientId = [];
        var recipientEmail = [];
        $('#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_gvSelectRecipients').find('input[type=checkbox]:checked').each(function () {
            var id = jQuery(this).attr('id');
            var hdnfield = id.replace("chk", "hdn");
            recipientId.push($("#" + hdnfield).val());

            var hdnfield2 = id.replace("chk", "hdn2");
            idOf.push($("#" + hdnfield2).val());
        })

        //$("# lblEmailTo.ClientID %>").hide();

        var claimStatus = $("#<%= ddlClaimStatusReview.ClientID %>").val();
        var claimStatusName = $("#<%= ddlClaimStatusReview.ClientID %> :selected").text();
        var insurerClaimId = $("#<%= txtInsurerClaimId.ClientID %>").val();

        var insurerName = $("#<%= txtInsurerName.ClientID %>").val();
        var claimAdjuster = $("#<%= txtClaimAdjuster.ClientID %>").val();
        var claimAdjusterId = $("#<%= hf_ClaimAdjusterID.ClientID %>").val();
        var adjusterComapnyName = $("#<%= txtAdjusterComapnyName.ClientID %>").val();
        var updatedby = "";
        var commentNote = $("#<%= txtCommentNote.ClientID %>").val();
        var emailTo = $("#<%= txtEmailTo.ClientID %>").val();
        emailTo = '';

        var carrier = $("#<%= ddlClaimCarrier.ClientID %> :selected").text();
        var carrierID = $("#<%= ddlClaimCarrier.ClientID %>").val();
        var claimID = $("#<%= hf_ClaimIdForStatus.ClientID %>").val();


        if (claimStatus == 0) {
            $("#<%= lblClaimStatusReview.ClientID %>").show();
        }
        if (insurerClaimId == "") {
            $("#<%= lblInsurerClaimId.ClientID %>").show();
        }
        if (insurerName == "") {
            $("#<%= lblInsurerName.ClientID %>").show();
        }
        if (claimAdjuster == "") {
            $("#<%= lblClaimAdjuster.ClientID %>").show();
        }
        //if (adjusterComapnyName == "") {
            //$("# //lblAdjusterComapnyName.ClientID").show();
       // }
       // if (commentNote == "") {
         //   $("#//lblCommentNote.ClientID %>").show();
        //}
        if (carrierID == 0) {
            $("#<%= lblClaimCarrier.ClientID %>").show();
        }
      //  if (recipientId.length == 0) {
        //    $("#lblEmailTo.ClientID %>").show();
       // }


        emailTo = "";
         if (claimStatus != 0 && insurerClaimId != "" && insurerName != "" && claimAdjusterId != 0  && carrierID != 0 ) //&& claimID != 0 && commentNote != "" && emailcheck==true ---------- remove by client     && adjusterComapnyName != ""  ----------- && recipientId.length > 0
        {
            var myParams = "{ 'claimStatus':'" + claimStatus + "', 'insurerClaimId':'" + insurerClaimId + "','insurerName':'" + insurerName + "','claimAdjusterId':'" + claimAdjusterId + "', 'adjusterComapnyName':'" + adjusterComapnyName + "','updatedby':'" + updatedby + "', 'commentNote':'" + commentNote + "','emailTo':'" + emailTo + "','carrierID':'" + carrierID + "','claimID':'" + claimID + "','recipientId':'" + recipientId + "','claimAdjuster':'" + claimAdjuster + "','claimStatusName':'" + claimStatusName + "','carrier':'" + carrier + "','idOf':'" + idOf + "'}";


            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/SaveClaimStatus") %>',
                success: function (data) {
                    $("#<%= hf_adjusterID.ClientID %>").val(claimAdjusterId);
                    $find("<%= txtAdjuster.ClientID %>").set_value(claimAdjuster);
                    $("#<%= txtInsurerClaimNumber.ClientID %>").val(insurerClaimId);

                    var leadStatus = $("#<%= ddlClaimStatusReview.ClientID %>").get(0).selectedIndex;
                    $("#<%= ddlLeadStatus.ClientID %>").prop('selectedIndex', leadStatus);

                    $("#div_ClaimStatusReview").dialog('close');
                    //control blank

                    $('#ctl00_ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_ContentPlaceHolderMiddArea_claimEdit_gvSelectRecipients').find('input[type=checkbox]:checked').each(function () {
                        var id = jQuery(this).attr('id');
                        $("#" + id).prop('checked', false);
                    })

                    //alert('hi');

                    $("#div_StatusSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');
                            $("#div_ClaimStatusReview").dialog('close');

                        },
                        buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                    });



                },
                error: function () {

                    $("#div_StatusNotSave").dialog({
                        modal: false,
                        width: 300,
                        close: function (e, ui) {
                            $(this).dialog('destroy');

                        },
                        buttons:
                                   {
                                       //'Done': function () {
                                       //    $(this).dialog('close');
                                       //}
                                   }
                    });

                }
            });

        }

        return false;
    }



    function BlankControl() {



        var insurerClaimId = $("#<%= txtInsurerClaimId.ClientID %>").val();

        var insurerName = $("#<%= txtInsurerName.ClientID %>").val();




        var commentNote = $("#<%= txtCommentNote.ClientID %>").val();


        if (insurerClaimId != "") {
            $("#<%= lblInsurerClaimId.ClientID %>").hide();
        }
        if (insurerName != "") {
            $("#<%= lblInsurerName.ClientID %>").hide();
    }


    if (commentNote != "") {
        $("#<%= lblCommentNote.ClientID %>").hide();
    }

    return false;
}



$("#<%= ddlClaimStatusReview.ClientID %>").change(function () {
        var claimStatus = $("#<%= ddlClaimStatusReview.ClientID %>").val();

        if (claimStatus != 0) {

            $("#<%= lblClaimStatusReview.ClientID %>").hide();
        }
    });

    $("#<%= ddlClaimCarrier.ClientID %>").change(function () {
        var claimCarrier = $("#<%= ddlClaimCarrier.ClientID %>").val();

    if (claimCarrier != 0) {

        $("#<%= lblClaimCarrier.ClientID %>").hide();
    }
});

$("#<%= ddlLeadStatus.ClientID %>").change(function () {
        var leadStatus = $("#<%= ddlLeadStatus.ClientID %>").get(0).selectedIndex;
        $("#<%= ddlClaimStatusReview.ClientID %>").prop('selectedIndex', leadStatus);

    });



    function CheckEmailTo() {
        var emailTo = $("#<%= txtEmailTo.ClientID %>").val();
       // if (!validateEmail(emailTo)) {
          //  $("# lblEmailTo.ClientID %>").show();
      //  }
      //  else {
        //    $("# lblEmailTo.ClientID %>").hide();
       // }

      //  if (emailTo == "") {
        //    $("# lblEmailTo.ClientID %>").show();
       // }
        return false;
    }


    function validateEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }


    function claimAdjusterGrid_rowsSelected(sender, args) {
        var selectedRows = args.getSelectedRows();

        var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
        var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

        $("#<%= hf_ClaimAdjusterID.ClientID %>").val(adjusterID);
        $find("<%= txtClaimAdjuster.ClientID %>").set_value(adjusterName);

        // $("#<%= hf_adjusterID.ClientID %>").val(adjusterID);
        // $find("<%= txtAdjuster.ClientID %>").set_value(adjusterName);

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

        var claimID = parseInt($("[id$='hf_ClaimIdForStatus']").val());
        //$("#<%= ddlClaimStatusReview.ClientID %> :selected").text();
        //var acrossall = parseInt($("[id$='hdnApplyDeductible']").val()); 

        //ConfirmDialog(this, 'Warning:  If you use loss details, then it template data not be recoverable.  Please do not delete unless absolutely necessary.  Are you sure you want to delete template record forever?')




        var myParams = "{'claimID':'" + claimID + "', 'coverage':'" + coverage + "','type':'" + type + "','policyLimit':'" + policyLimit + "','deductible':'" + deductible + "','applyTo':'" + applyTo + "','itv':'" + itv + "','reserve':'" + reserve + "','acrossall':'" + acrossall + "'}";
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


<style>
    table.csk-box td input[type="checkbox"], table.csk-box th input[type="checkbox"] {
        margin-left: 0!important;
    }
</style>
