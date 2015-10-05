<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewLead.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucNewLead" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControl/Admin/ucLeadPolicy.ascx" TagName="ucLeadPolicy" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Admin/ucPolicyList.ascx" TagName="ucPolicyList" TagPrefix="uc4" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>

<div class="paneContent">
    <div class="page-title">
       <%-- Claimant(s) --%> 
        Insured
       
    </div>
    <asp:Panel ID="pnlTasks" runat="server">
        <h2>
            <span><b><%= Session["InsuredName"]%></b> </span>
        </h2>
        <asp:Panel ID="pnlTasksHeader" runat="server" CssClass="section-title">
            <div style="float: left;">
                Tasks
            </div>
            <div style="float: right; padding-right: 5px; vertical-align: middle;">
                <img runat="server" id="img_tasks" src="~/images/expand_blue.jpg" alt="(Show Tasks...)" title="Hide/Show"
                    style="border-width: 0px; vertical-align: middle;" />
            </div>
        </asp:Panel>
        <ajaxToolkit:CollapsiblePanelExtender ID="cpeTasks" runat="Server" TargetControlID="pnlLeadTasks"
            Collapsed="false" ExpandControlID="img_tasks" CollapseControlID="img_tasks"
            AutoCollapse="False" ScrollContents="false" ImageControlID="img_tasks" ExpandedImage="~/images/expand_blue.jpg"
            CollapsedImage="~/images/collapse_blue.jpg" ExpandDirection="Vertical" />
        <!-- 
			Calendar 
		-->
        <asp:Panel ID="pnlLeadTasks" runat="server">
            <div class="toolbar toolbar-body">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnScheduler" runat="server" Text="" CssClass="toolbar-item" PostBackUrl="~/Protected/LeadSchedule.aspx">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/ig_calendar.gif)">Scheduler</span>
                            </asp:LinkButton>
                        </td>
                        <td style="width: 5px;"></td>
                    </tr>
                </table>
            </div>
            <div style="padding-bottom: 5px;">
                <table style="width: 100%; border-collapse: separate; border-spacing: 3px; padding: 0px;" border="0">
                    <tr>
                        <td style="width: 150px; vertical-align: top; text-align: left;">
                            <ig:WebMonthCalendar ID="TaskCalendar" runat="server" ChangeMonthToDateClicked="true" AutoPostBackFlags-SelectionChanged="On"
                                EnableMonthDropDown="True" EnableYearDropDown="True" OnSelectedDateChanged="TaskCalendar_SelectedDateChanged" Font-Size="11px" Width="220px">
                            </ig:WebMonthCalendar>
                            <asp:ImageButton ID="ibtnTasks" runat="server" ImageAlign="Top" PostBackUrl="~/Protected/Admin/Tasks.aspx"
                                ImageUrl="~/Images/scheduler.png" Visible="false" />
                        </td>
                        <td style="vertical-align: top;">

                            <asp:UpdatePanel ID="updatePanelCalendar" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="TaskCalendar" />
                                </Triggers>
                                <ContentTemplate>


                                    <asp:GridView ID="gvTasks" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="gridView"
                                        CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff" AllowSorting="true"
                                        OnSorting="gvTasks_onSorting" RowStyle-HorizontalAlign="Center">
                                        <EmptyDataTemplate>
                                            <a runat="server" href="~/Protected/LeadSchedule.aspx" target="_self" style="color: blue; text-decoration: underline;">New Task</a>
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle BorderStyle="None" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="start_date">
                                                <ItemStyle Font-Size="11px" Width="120px" />
                                                <ItemTemplate>
                                                    <%# Eval("start_date", "{0:MM/dd/yyyy HH:mm tt}") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Event" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Eval("text") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Eval("details")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="User Name" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="owner_name">
                                                <ItemTemplate>
                                                    <%# Eval("owner_name") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Policyholder" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="lead_name">
                                                <ItemTemplate>
                                                    <a href="NewLead.aspx?id=<%#Eval("lead_id") %>">
                                                        <%# Eval("lead_name") %></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                SortExpression="statusName">
                                                <ItemTemplate>
                                                    <%# Eval("statusName") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlLeadHeader" runat="server" CssClass="section-title">
        <div style="float: left;">
            <asp:Label ID="lblHead" runat="server" Text="Add New" />
        </div>
    </asp:Panel>

    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="toolbar-item" PostBackUrl="~/Protected/Admin/AllUsersLeads.aspx">
							<span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Claims</span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="toolbar-item"
                        OnClick="btnSaveAndContinue_Click" ValidationGroup="NewLead" OnClientClick="return validate_page();">
						<span class="toolbar-img-and-text" style="background-image: url(../images/toolbar_save.png)">Save</span>
                    </asp:LinkButton>
                </td>

            </tr>
        </table>
    </div>

    <div style="display: none;">
        <asp:Button ID="btnSaveAndContinue1" runat="server" Text="Save" OnClick="btnSaveAndContinue_Click"
            ValidationGroup="NewLead" CausesValidation="true" />
        <asp:Button ID="btnRefreshTasks" runat="server" OnClick="btnRefreshTasks_Click" />
    </div>

    <table style="width: 100%; border-collapse: separate; border-spacing: 3px; padding: 0px;" border="0" class="new_user">
        <tr>
            <td style="vertical-align: top;">
                <asp:UpdatePanel ID="updatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="message_area" style="margin-bottom: 10px;">
                            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                            <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                            <asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
                        </div>
                        <ajaxToolkit:TabContainer ID="tabContainer" runat="server" Width="100%">
                            <ajaxToolkit:TabPanel ID="tabPanelClaimant" runat="server">
                                <HeaderTemplate>
                                   <%--Claimant(s)--%>
                                    Insured
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="left top" style="width: 33%;">
                                                <div class="boxContainer" <%--style="height: 420px"--%>>
                                                    <div class="section-title">
                                                        <%--Claimant(s)--%>
                                                        Insured
                                                    </div>
                                                    <table style="width: 100%;" class="editForm nowrap">
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <span>Date Record Created</span>
                                                            </td>
                                                            <td class="redstar">*</td>
                                                            <td>
                                                                <ig:WebDatePicker ID="txtOriginalLeadDate" runat="server" TabIndex="1" Height="20px" CssClass="date_picker">
                                                                    <Buttons>
                                                                        <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                                                    </Buttons>
                                                                </ig:WebDatePicker>
                                                                <div>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtOriginalLeadDate" runat="server" ValidationGroup="NewLead"
                                                                        ControlToValidate="txtOriginalLeadDate" SetFocusOnError="True" Display="Dynamic"
                                                                        CssClass="validation1" ErrorMessage="Please enter date." />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Insured</td>
                                                            <td class="redstar">*</td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="100" ID="txtInsuredName" TabIndex="2" />
                                                                <div>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="NewLead"
                                                                        ControlToValidate="txtInsuredName" SetFocusOnError="True" Display="Dynamic"
                                                                        CssClass="validation1" ErrorMessage="Please enter insured name." />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>First Name</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtClaimantName" TabIndex="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Last Name
                                                            </td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtClaimantLastName" TabIndex="3" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Middle Name
                                                            </td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtClaimantMiddleName" TabIndex="4" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Business Name (if any)
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtBusinessName" TabIndex="5" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Salutation
                                                            </td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtSalutation" TabIndex="6" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Home Phone #</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="35" ID="txtPhoneNumber" TabIndex="7" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Business Phone #
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="35" ID="txtSecondaryPhone" TabIndex="8" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>Mobile Phone #
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="35" ID="txtMobilePhone" TabIndex="8" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Primary Email</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" ID="txtEmail" TabIndex="9" MaxLength="100" />
                                                                <div>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                                                        ErrorMessage="Email not valid!" ValidationGroup="NewLead" Display="Dynamic"
                                                                        CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Secondary Email
                                                            </td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="100" ID="txtSecondaryEmail" TabIndex="10" />
                                                                <div>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSecondaryEmail"
                                                                        ErrorMessage="Email not valid!" ValidationGroup="NewLead" Display="Dynamic"
                                                                        CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Birthdate </td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <ig:WebDatePicker ID="txtBirthdate" runat="server" TabIndex="10" CssClass="date_picker">
                                                                    <Buttons>
                                                                        <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                                                    </Buttons>
                                                                </ig:WebDatePicker>


                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Reference</td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtReference" runat="server" TabIndex="10" MaxLength="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Language</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" ID="txtLanguage" MaxLength="100" TabIndex="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Title</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" ID="txtTitle" MaxLength="100" TabIndex="10" />
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </td>
                                            <td class="left top">
                                                <div class="boxContainer" style="height: 416px">
                                                    <div class="section-title">
                                                        Loss Address
                                                    </div>
                                                    <table style="width: 100%;" class="editForm nowrap">
                                                        <tr>
                                                            <td style="width: 20%;">Loss Address 1</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="100" ID="txtLossAddress" TabIndex="20" />
                                                               <%-- <div>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="NewLead"
                                                                        ControlToValidate="txtLossAddress" SetFocusOnError="True" Display="Dynamic"
                                                                        CssClass="validation1" ErrorMessage="Please enter address." />
                                                                </div>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Loss Address 2</td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="100" ID="txtLossAddress2" TabIndex="21" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Loss Country</td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="40" ID="txtlossCountry" TabIndex="21" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Loss State</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlState" runat="server" TabIndex="22" />
                                                              <%--  <div>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="NewLead"
                                                                        ControlToValidate="ddlState" SetFocusOnError="True" Display="Dynamic"
                                                                        CssClass="validation1" ErrorMessage="Please select state." />
                                                                </div>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Loss City</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtCityName" runat="server" MaxLength="50" TabIndex="23" />
                                                               <%-- <div>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="NewLead"
                                                                        ControlToValidate="txtCityName" SetFocusOnError="True" Display="Dynamic"
                                                                        CssClass="validation1" ErrorMessage="Please enter city." />
                                                                </div>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Loss Zip</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtZipCode" runat="server" MaxLength="10" TabIndex="24" />
                                                                <%--<div>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="NewLead"
                                                                        ControlToValidate="txtZipCode" SetFocusOnError="True" Display="Dynamic"
                                                                        CssClass="validation1" ErrorMessage="Please enter zip code." />
                                                                </div>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Loss Location</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtLossLocation" runat="server" MaxLength="100" TabIndex="25" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="3">
                                                                <div class="section-title">
                                                                    3rd Party Claimant
                                                                </div>
                                                                <table style="width: 100%;" class="editForm nowrap">                                                                    
                                                                 <tr>
                                                                    <td style="width: 20%;">Name</td>
                                                                    <td class="redstar"></td>
                                                                    <td>
                                                                        <ig:WebTextEditor runat="server" MaxLength="100" ID="txtThirdPartyName" TabIndex="25" />                                                               
                                                                    </td>
                                                                 </tr>
                                                                   <tr>
                                                                    <td style="width: 20%;">Street Address</td>
                                                                    <td class="redstar"></td>
                                                                    <td>
                                                                        <ig:WebTextEditor runat="server" MaxLength="100" ID="txtThirdPartyStreet" TabIndex="25" />                                                               
                                                                    </td>
                                                                 </tr>
                                                                    <tr>
                                                                    <td style="width: 20%;">City</td>
                                                                    <td class="redstar"></td>
                                                                    <td>
                                                                        <ig:WebTextEditor runat="server" MaxLength="100" ID="txtThirdPartyCity" TabIndex="25" />                                                               
                                                                    </td>
                                                                 </tr>
                                                                    <tr>
                                                                    <td style="width: 20%;">State</td>
                                                                    <td class="redstar"></td>
                                                                    <td>
                                                                        <ig:WebTextEditor runat="server" MaxLength="100" ID="txtThirdPartyState" TabIndex="25" />                                                               
                                                                    </td>
                                                                 </tr>
                                                                    <tr>
                                                                    <td style="width: 20%;">Zip Code</td>
                                                                    <td class="redstar"></td>
                                                                    <td>
                                                                        <ig:WebTextEditor runat="server" MaxLength="6" ID="txtThirdPartyPostalCodes" TabIndex="25" />  
                                                                       <%-- <ig:WebNumericEditor ID="txtThirdPartyPostalCode" runat="server" DataMode="Int" TabIndex="25"  MaxLength="6" HorizontalAlign="Left">
                                                                        </ig:WebNumericEditor>--%>                                                          
                                                                    </td>
                                                                 </tr>
                                                                    <tr>
                                                                    <td style="width: 20%;">Phone(Ext.)</td>
                                                                    <td class="redstar"></td>
                                                                    <td>
                                                                      <%--  <ig:WebTextEditor runat="server" MaxLength="20" ID="txtThirdPartyPhoneNumber" TabIndex="25" /> --%>       
                                                                        <asp:TextBox ID="txtThirdPartyPhoneNumber" runat="server" Width="242px"></asp:TextBox>  
                                                                        <ajaxToolkit:MaskedEditExtender ID="maskMainPhone" runat="server"
                                                                            TargetControlID="txtThirdPartyPhoneNumber"
                                                                            Mask="999-999-9999(999)"
                                                                            MaskType="None"
                                                                            MessageValidatorTip="true" 
                                                                            OnFocusCssClass="editmask"
                                                                            OnInvalidCssClass="invalidmask"
                                                                            InputDirection="LeftToRight"
                                                                            ClearMaskOnLostFocus="false"
                                                                            AutoComplete="false" />                                                                                                                           
                                                                    </td>
                                                                 </tr>
                                                                    </table>




                                                            </td>
                                                        </tr>


                                                    </table>
                                                </div>
                                            </td>
                                            <td class="left top">
                                                <div class="boxContainer" style="height: 310px">
                                                    <div class="section-title">
                                                        Mailing Address
                                                    </div>

                                                    <table style="width: 100%;" class="editForm nowrap">
                                                        <tr>
                                                            <td style="width: 20%;"></td>
                                                            <td></td>
                                                            <td style="vertical-align: middle;">
                                                                <input type="checkbox" onclick="javascript: copyLossAddress();" tabindex="30" />&nbsp;Same as Loss Address
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mailing Address</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtMailingAddress" runat="server" MaxLength="100" TabIndex="31" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mailing Address 2</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtMailingAddress2" runat="server" MaxLength="50" TabIndex="31" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td>Mailing Country</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtMailingCountry" runat="server" MaxLength="40" TabIndex="31" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mailing State</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMailingState" runat="server" TabIndex="32" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mailing City</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtMailingCity" runat="server" MaxLength="50" TabIndex="33" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Mailing Zip</td>
                                                            <td></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtMailingZip" runat="server" MaxLength="10" TabIndex="34" />
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="top left" style="width: 33%;">
                                                <div class="boxContainer" style="height: 160px">
                                                    <div class="section-title">
                                                        Owner Information
                                                    </div>
                                                    <table style="width: 100%;" class="editForm nowrap">
                                                        <tr>
                                                            <td style="width: 20%;">Owner Same?
                                                            </td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOwnerSame" runat="server" TabIndex="40" onclick="copySameAsOwner();">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Owner First Name</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtOwnerFirstName" TabIndex="41" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Owner Last Name</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="50" ID="txtOwnerLastName" TabIndex="42" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Owner Phone</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="35" ID="txtOwnerPhone" TabIndex="43" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Owner Email
                                                            </td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor runat="server" MaxLength="100" ID="txtOwnerEmail" TabIndex="44" />
                                                                <div>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOwnerEmail"
                                                                        ErrorMessage="Email not valid." ValidationGroup="NewLead" Display="Dynamic" CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td class="top left">
                                                <div class="boxContainer" style="height: 160px">
                                                    <div class="section-title">
                                                        Producer Information
                                                    </div>
                                                    <table border="0" style="width: 100%;" class="editForm nowrap">
                                                        <tr>
                                                            <td style="width: 20%;">Primary Source</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlLeadSource" runat="server" CssClass="DDLStyles" TabIndex="50">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Secondary Source</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <ig:WebTextEditor ID="txtSecondaryLeadSource" runat="server" TabIndex="51" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Primary Producer</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPrimaryProducer" CssClass="DDLStyles" runat="server" TabIndex="52">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Secondary Producer</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlSecondaryProducer" CssClass="DDLStyles" runat="server" TabIndex="53">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td class="top left">
                                                <div class="boxContainer" style="height: 160px">
                                                    <div class="section-title">
                                                        Other Information
                                                    </div>
                                                    <table style="width: 100%;" class="editForm nowrap">
                                                        <tr>
                                                            <td style="width: 20%;">Appraiser</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlAppraiser" CssClass="DDLStyles" runat="server" TabIndex="60">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Contractor</td>
                                                            <td class="redstar"></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlContractor" CssClass="DDLStyles" runat="server" TabIndex="61">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Umpire</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlUmpire" CssClass="DDLStyles" runat="server" TabIndex="62">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Last Activity Date:</td>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label ID="lblLastActivityDate" runat="server" />
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>

                            <ajaxToolkit:TabPanel ID="tabPanelPolicies" runat="server">
                                <HeaderTemplate>
                                    Policy
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="pnlPolicyList" runat="server">

                                        <div style="margin-bottom: 5px;">
                                            <asp:LinkButton ID="btnNewPolicy" runat="server" Text="New Policy" OnClick="btnNewPolicy_Click"
                                                Visible='<%#Convert.ToBoolean(CRM.Core.PermissionHelper.checkAddPermission("newlead.aspx")) %>'>
												<span style="text-decoration:underline; font-size:11px;">New Policy</span>
                                            </asp:LinkButton>

                                        </div>
                                        <div>
                                            <uc4:ucPolicyList ID="ucpolicyList" runat="server" />
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPolicyEdit" runat="server" Visible="false">
                                        <div>
                                            <!-- POLICY EDIT SCREEN -->
                                            <asp:LinkButton ID="btnPolicyList" runat="server" Text="Return to Claim" OnClick="btnPolicyList_Click">
												<span style="text-decoration:underline; font-size:11px;">Back to Policies</span>
                                            </asp:LinkButton>

                                        </div>


                                        <uc2:ucLeadPolicy ID="policyEditForm" runat="server" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>

                        </ajaxToolkit:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</div>
<asp:HiddenField ID="hfAdminView" Value="0" runat="server" />
<asp:HiddenField ID="hf_letterPath" runat="server" />
<asp:HiddenField ID="hf_taskDate" runat="server" />
<asp:HiddenField ID="hf_Latitude" runat="server" Value="0" />
<asp:HiddenField ID="hf_Longitude" runat="server" Value="0" />
<style type="text/css">
    .InvisibleCol {
        display: none;
    }
</style>
<script type="text/javascript">
    function validate_page() {
        if (Page_ClientValidate("NewLead"))
            return true;
        else
            return false;
    }

</script>
<script type="text/javascript">
    function copySameAsOwner() {
        var sameAsOwner = $("#<%= ddlOwnerSame.ClientID %> option:selected").text();

        var firstName = $find("<%=txtClaimantName.ClientID %>").get_value();
        var lastName = $find("<%=txtClaimantLastName.ClientID %>").get_value();
        var phone = $find("<%=txtPhoneNumber.ClientID %>").get_value();
        var mobile = $find("<%=txtMobilePhone.ClientID %>").get_value();
        var email = $find("<%=txtEmail.ClientID %>").get_value();

        var ownerFirstName = $find("<%=txtOwnerFirstName.ClientID %>");
        var ownerLastName = $find("<%=txtOwnerLastName.ClientID %>");
        var ownerPhone = $find("<%=txtOwnerPhone.ClientID %>");
        var Owneremail = $find("<%=txtOwnerEmail.ClientID %>");

        if ($.trim(sameAsOwner) == "Yes") {
            ownerFirstName.set_value(firstName);
            ownerLastName.set_value(lastName);
            ownerPhone.set_value(phone);
            Owneremail.set_value(email);
            $("#<%=txtOwnerFirstName.ClientID %>").attr("disabled", "disabled");
	        $("#<%=txtOwnerLastName.ClientID %>").attr("disabled", "disabled");
	        $("#<%=txtOwnerPhone.ClientID %>").attr("disabled", "disabled");
	        $("#<%=txtOwnerEmail.ClientID %>").attr("disabled", "disabled");
	    }
	    else {
	        // make fields editable
	        ownerFirstName.set_value('');
	        ownerLastName.set_value('');
	        ownerPhone.set_value('');
	        Owneremail.set_value('');
	        $("#<%=txtOwnerFirstName.ClientID %>").removeAttr("disabled");
		    $("#<%=txtOwnerLastName.ClientID %>").removeAttr("disabled");
	        $("#<%=txtOwnerPhone.ClientID %>").removeAttr("disabled");
	        $("#<%=txtOwnerEmail.ClientID %>").removeAttr("disabled");
	    }
    }

</script>
<script type="text/javascript">
    var map = null;

    $(document).ready(function () {
        copySameAsOwner();
    });


    function copyLossAddress() {
        var lossAddress = $find("<%=txtLossAddress.ClientID %>").get_value();
        var lossAddress2 = $find("<%=txtLossAddress2.ClientID %>").get_value();
        var lossState = $("#<%=ddlState.ClientID %>").val();
        var lossCity = $find("<%=txtCityName.ClientID %>").get_value();
        var lossZip = $find("<%=txtZipCode.ClientID %>").get_value();

        $find("<%=txtMailingAddress.ClientID %>").set_value(lossAddress);
        $find("<%=txtMailingAddress2.ClientID %>").set_value(lossAddress2);
        $("#<%=ddlMailingState.ClientID %>").val(lossState);
        $find("<%=txtMailingCity.ClientID %>").set_value(lossCity);
        $find("<%=txtMailingZip.ClientID %>").set_value(lossZip);
    }

    function isNumberKey(evt, ctr, exp, dec) {
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

    function checkLength(s, v) {
        var v = s.id;
        var len = document.getElementById(v).value;
        if (len.length > Number(v)) {
            return false;
        }
        return false;
    }


    function getCurrentPolicyTab() {
        var policyTypeID = 0;
        var currentTab = $find('<%=tabContainer.ClientID %>').get_activeTab();

	    var tabIndex = currentTab.get_tabIndex();
	    switch (tabIndex) {
	        case 2:
	            policyTypeID = 1; // homeowners
	            break;
	        case 3:
	            policyTypeID = 2; // commercial
	            break;
	        case 4:
	            policyTypeID = 3; // flood
	            break;
	        case 5:
	            policyTypeID = 4; // earthquake
	            break;
	        default:
	            break;
	    }

	    return policyTypeID;
    }

    //	function openEmail() {

    //		var policyTypeID = getCurrentPolicyTab();

    //		var w = window.open("../Admin/LeadEmail.aspx?t=" + policyTypeID, "Email", "status=1, toolbar=1, width=1100, height=800");
    //		w.moveTo(100, 0);
    //	}
    //	function openEmailImport() {
    //		var w = window.open("../Admin/LeadImportEmail.aspx", "Email Import", "status=1, toolbar=1, width=1100, height=800");
    //		w.moveTo(100, 0);
    //	}
    //	function openInvoice() {
    //		PopupCenter("../Admin/LeadInvoice.aspx", "Invoice", 1200, 600);
    //	}

</script>
<script type="text/javascript">
    function WebForm_OnSubmit() {
        if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) {


            for (var i = 0; i < Page_Validators.length; i++) {
                try {
                    var control = document.getElementById(Page_Validators[i].controltovalidate);
                    if (!Page_Validators[i].isvalid) {
                        control.className = "ErrorControl";

                    } else {
                        control.className = "";
                    }
                } catch (e) { }
            }
            return false;
        }
        return true;
    }

    //function Validate_Required(sender, args) {
    //	if (document.getElementById(sender.controltovalidate).value != "") {
    //		args.IsValid = true;
    //		document.getElementById(sender.controltovalidate).className = "";
    //	} else {
    //		args.IsValid = false;
    //		document.getElementById(sender.controltovalidate).className = "ErrorControl";
    //	}
    //}
    function addNewComment(policyType) {
        PopupCenter("../../Content/Comment.aspx?t=" + policyType, "New Comment", 700, 600);
    }

</script>
<script type="text/javascript">
    function uploadDocument(policyType) {
        PopupCenter("../../Content/DocumentUpload.aspx?t=" + policyType, "Upload Document", 700, 400);
    }

    function printLetterOfRep() {
        window.open("../../Content/PrintLetterOfRep.aspx");
    }
    function printPhotoReport() {
        window.open("../../Content/PrintPhotoReport.aspx");
    }



</script>
<script type="text/javascript">
    // this is called whenever user changes status
    function saveForm() {
        //$("#<%= btnRefreshTasks.ClientID %>").click();
        $("#<%= btnSaveAndContinue1.ClientID %>").click();
    }
</script>
<script type="text/javascript">
    function LoadMap() {
        // Define the address on which to centre the map
        var addressLine = $("#<%= txtLossAddress.ClientID %>").val(); 	// Street Address
        var locality = $("#<%= txtCityName.ClientID %>").val(); 			// City or town name
        var adminDistrict = $("#<%= ddlState.ClientID %>").val();  			// County
        var country = "US"; 										// Country
        var postalCode = $("#<%= txtZipCode.ClientID %>").val(); 			//Postcode

        // return if blank
	    if ($.trim(addressLine) == '')
	        return;

        // Construct a request to the REST geocode service
	    var geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations"
					    + "?countryRegion=" + country
					    + "&addressLine=" + addressLine
					    + "&locality=" + locality
					    + "&adminDistrict=" + adminDistrict
					    + "&postalCode=" + postalCode
					    + "&key=" + credentials
					    + "&jsonp=GeocodeCallback"; // This function will be called after the geocode service returns its results

        // Call the service
	    CallRestService(geocodeRequest);
    }

    function GeocodeCallback(result) {
        // Check that we have a valid response
        if (result && result.resourceSets && result.resourceSets.length > 0 && result.resourceSets[0].resources && result.resourceSets[0].resources.length > 0) {

            // Create a Location based on the geocoded coordinates
            var coords = result.resourceSets[0].resources[0].point.coordinates;
            centerPoint = new Microsoft.Maps.Location(coords[0], coords[1]);

            $("#<%= hf_Latitude.ClientID %>").val(coords[0]);
            $("#<%= hf_Longitude.ClientID %>").val(coords[1]);

            // Create a map centred on the location
            map = new Microsoft.Maps.Map(document.getElementById("divLossMap"),
						{
						    credentials: credentials,
						    center: centerPoint,
						    mapTypeId: Microsoft.Maps.MapTypeId.aerial,
						    zoom: 20
						});

            // Add a pushpin as well
            var pushpin = new Microsoft.Maps.Pushpin(map.getCenter());
            map.entities.push(pushpin);
        }
    }

    function CallRestService(request) {
        var script = document.createElement("script");
        script.setAttribute("type", "text/javascript");
        script.setAttribute("src", request);
        var dochead = document.getElementsByTagName("head").item(0);
        dochead.appendChild(script);
    }
</script>
