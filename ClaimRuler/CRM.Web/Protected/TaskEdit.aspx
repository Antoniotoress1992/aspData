<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="TaskEdit.aspx.cs" Inherits="CRM.Web.Protected.TaskEdit" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControl/ucContactLookup.ascx" TagName="ucContactLookup" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            <asp:Label ID="lblTitle" runat="server"></asp:Label>
        </div>

        <asp:UpdatePanel ID="updatepanel" runat="server">
            <ContentTemplate>
                <div class="toolbar toolbar-body">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnBackToActivities" runat="server" CssClass="toolbar-item" PostBackUrl="~/Protected/tasks.aspx">
					                <span class="toolbar-img-and-text" style="background-image: url(../images/back.png)">Activities</span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <div class="paneContentInner">
                    <div class="boxContainer" style="width: 50%; margin: auto;">
                        <div class="section-title">Task Details</div>
                        <table style="width: 100%;" class="editForm no_min_width">
                            <tr>
                                <td class="right" style="width: 30%;">Subject</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebTextEditor ID="txtSubject" runat="server" MaxLength="250" Width="300px" TabIndex="1"></ig:WebTextEditor>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvsubejct" runat="server" ControlToValidate="txtSubject" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter subject." CssClass="validation1" />

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Description</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtDescription" runat="server" MaxLength="500" Width="305px" TextMode="MultiLine" MultiLine-Rows="3" TabIndex="2"></ig:WebTextEditor>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Due Date</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebDatePicker ID="txtDateDue" runat="server" CssClass="date_picker" TabIndex="3" DisplayModeFormat="g" EditModeFormat="MM/dd/yyyy h:mm tt">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>

                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvLossDate" runat="server" ControlToValidate="txtDateDue" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter due date." CssClass="validation1" />

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Priority</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlPriority" runat="server" Width="200px" TabIndex="4" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPriority" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please select priority." CssClass="validation1" />

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Owner</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlOwner" runat="server" Width="200px" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlOwner" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please select owner." CssClass="validation1" InitialValue="0" />

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Status</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlTaskStatus" runat="server" Width="200px" TabIndex="5" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTaskStatus" ValidationGroup="task"
                                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please select status." CssClass="validation1" />

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Contact</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebTextEditor ID="txtContact" runat="server" Width="300px" ReadOnly="true" CssClass="watermark_search" TabIndex="6">
                                        <ClientEvents MouseUp="txtContact_focus" />
                                    </ig:WebTextEditor>
                                </td>
                            </tr>
                        </table>

                        <div style="display:none" id="divRecurrence">
                        <h3>Task Recurrence
                            <span>&nbsp;<asp:LinkButton ID="lbtnClearRecurrence" runat="server" Text="Delete Repeat Series"
                                OnClientClick="javascript:return ConfirmDialog(this, 'Deleting this repeat series will delete all associated tasks. Are you sure?');"
                                OnClick="lbtnClearRecurrence_Click" CssClass="link" Visible="false" /></span>
                        </h3>
                 
                        <table style="width: 100%;" class="editForm no_min_width" border="0" >
                            <tr>
                                <td class="right" style="width: 30%;">Start Date</td>
                                <td class="redstar"></td>
                                <td class="left nowrap">

                                    <ig:WebDatePicker ID="txtRecurrenceStartDate" runat="server" Width="135px" TabIndex="7">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="right">End Date</td>
                                <td class="redstar"></td>
                                <td>
                                    <ig:WebDatePicker ID="txtRecurrenceEndDate" runat="server" Width="135px" TabIndex="8">
                                        <Buttons>
                                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                                        </Buttons>
                                    </ig:WebDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="right top">Repeat</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlRecurringRepeatFrequency" runat="server" Width="135px" AutoPostBack="true" TabIndex="9"
                                        OnSelectedIndexChanged="ddlRecurringRepeatFrequency_SelectedIndexChanged">
                                        <asp:ListItem Text="None" Value="0" />
                                        <asp:ListItem Text="Daily" Value="1" />
                                        <asp:ListItem Text="Weekly" Value="2" />
                                        <asp:ListItem Text="Monthly" Value="3" />
                                        <asp:ListItem Text="Yearly" Value="4" />
                                    </asp:DropDownList>

                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td></td>
                                <td colspan="2">
                                    <asp:Panel ID="pnlRepeatDaily" runat="server" Visible="false">
                                        <div class="paneContentInner">
                                            <table>
                                                <tr>
                                                    <td class="left">
                                                        <asp:CheckBox ID="cbxRecurringDailyEveryDay" runat="server" Text="Every day" TextAlign="Right" TabIndex="20" />
                                                        <ajaxToolkit:MutuallyExclusiveCheckBoxExtender runat="server"
                                                            ID="MustHaveGuestBedroomCheckBoxEx"
                                                            TargetControlID="cbxRecurringDailyEveryDay"
                                                            Key="RecurringEveryDay" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="left nowrap">
                                                        <asp:CheckBox ID="cbxRecurringDailyEveryNDay" runat="server" Text="Repeat for every" TextAlign="Right" TabIndex="21" />
                                                        <ajaxToolkit:MutuallyExclusiveCheckBoxExtender runat="server"
                                                            ID="MutuallyExclusiveCheckBoxExtender1"
                                                            TargetControlID="cbxRecurringDailyEveryNDay"
                                                            Key="RecurringEveryDay" />

                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="txtRecurringDailyEveryNDays" runat="server" TextMode="Number" TabIndex="22"
                                                            MaxLength="3" MinValue="1"
                                                            Width="50px" Buttons-SpinButtonsDisplay="OnRight" />
                                                    </td>
                                                    <td>days</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="task" CssClass="validation1"
                                                            ValidateEmptyText="true"
                                                            ErrorMessage="Please select repeat series."
                                                            OnServerValidate="cbxRecurringDailyEveryNDay_Validate" Display="Dynamic">
                                                        </asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>

                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="pnlRepeatWeekly" runat="server" Visible="false">
                                        <div class="paneContentInner">
                                            <div class="left nowrap">
                                                Repeat for every 
                                                <ig:WebNumericEditor ID="txtRepeatWeeklyEveryNWeeks" runat="server" TextMode="Number" MaxLength="3" Width="50px" TabIndex="30" />
                                                &nbsp;weeks
                                            </div>
                                            <div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRepeatWeeklyEveryNWeeks" ValidationGroup="task"
                                                    SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter number of weeks." CssClass="validation1" />
                                            </div>
                                            <div class="left">
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekSun" runat="server" Text="Sunday" TextAlign="Right" TabIndex="31" />
                                                </div>
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekMon" runat="server" Text="Monday" TextAlign="Right" TabIndex="32" />
                                                </div>
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekTue" runat="server" Text="Tuesday" TextAlign="Right" TabIndex="33" />
                                                </div>
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekWed" runat="server" Text="Wednesday" TextAlign="Right" TabIndex="34" />
                                                </div>
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekThu" runat="server" Text="Thursday" TextAlign="Right" TabIndex="35" />
                                                </div>
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekFri" runat="server" Text="Friday" TextAlign="Right" TabIndex="36" />
                                                </div>
                                                <div class="left">
                                                    <asp:CheckBox ID="cbxEveryWeekSat" runat="server" Text="Saturday" TextAlign="Right" TabIndex="37" />
                                                </div>
                                            </div>
                                            <div>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="task" CssClass="validation1"
                                                    ValidateEmptyText="true"
                                                    ErrorMessage="Please select repeat series."
                                                    OnServerValidate="RepeatWeeklyEveryNWeeks_Validate" Display="Dynamic">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="pnlRepeatMonthly" runat="server" Visible="false">
                                        <div class="paneContentInner">
                                            <table border="0">
                                                <tr>
                                                    <td class="left nowrap">
                                                        <asp:CheckBox ID="cbxRecurringMonthlyOnDay" runat="server" TabIndex="40" />
                                                        <ajaxToolkit:MutuallyExclusiveCheckBoxExtender runat="server"
                                                            ID="MutuallyExclusiveCheckBoxExtender2" TargetControlID="cbxRecurringMonthlyOnDay" Key="RecurringMonthlyOnDay" />
                                                    </td>
                                                    <td class="left nowrap">On day</td>
                                                    <td class="left nowrap">
                                                        <ig:WebNumericEditor ID="txtRecurringMonthlyOnDay" runat="server" TextMode="Number" TabIndex="41"
                                                            MinValue="1" MaxValue="31"
                                                            MaxLength="2" Width="50px">
                                                            <Buttons SpinButtonsDisplay="OnRight" ListOfValues="1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31"
                                                                SpinOnArrowKeys="true"
                                                                SpinOnReadOnly="true" SpinWrap="true" />
                                                        </ig:WebNumericEditor>
                                                    </td>
                                                    <td>&nbsp;of every
                                                        <ig:WebNumericEditor ID="txtRecurringMonthlyOnDayEvery" runat="server" TextMode="Number" TabIndex="42"
                                                            MaxLength="3" Width="40px"
                                                            Buttons-SpinButtonsDisplay="None" />

                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td class="left">
                                                        <asp:CheckBox ID="cbxRecurringMonthlyOn" runat="server" TabIndex="43" />
                                                        <ajaxToolkit:MutuallyExclusiveCheckBoxExtender runat="server"
                                                            ID="MutuallyExclusiveCheckBoxExtender3" TargetControlID="cbxRecurringMonthlyOn" Key="RecurringMonthlyOnDay" />
                                                    </td>
                                                    <td class="left nowrap">On</td>
                                                    <td class="left nowrap">
                                                        <asp:DropDownList ID="ddlRecurringMonthlyOn" runat="server" Width="50px" CssClass="no_min_width" TabIndex="44">
                                                            <asp:ListItem Text="First" Value="1" />
                                                            <asp:ListItem Text="Second" Value="2" />
                                                            <asp:ListItem Text="Third" Value="3" />
                                                            <asp:ListItem Text="Fourth" Value="4" />
                                                            <asp:ListItem Text="Last" Value="5" />
                                                        </asp:DropDownList>

                                                    </td>
                                                    <td class="left nowrap">
                                                        <asp:DropDownList ID="ddlRecurringMonthlyWeekDay" runat="server" Width="50px" CssClass="no_min_width" TabIndex="45">
                                                            <asp:ListItem Text="Sunday" Value="0" />
                                                            <asp:ListItem Text="Monday" Value="1" />
                                                            <asp:ListItem Text="Tuesday" Value="2" />
                                                            <asp:ListItem Text="Wednesday" Value="3" />
                                                            <asp:ListItem Text="Thursday" Value="4" />
                                                            <asp:ListItem Text="Friday" Value="5" />
                                                            <asp:ListItem Text="Saturday" Value="6" />
                                                        </asp:DropDownList>

                                                        of every &nbsp;
                                                        <ig:WebNumericEditor ID="txtRecurringMonthlyWeekDayOfEveryMonth" runat="server" TextMode="Number" TabIndex="46"
                                                            MaxLength="3" Width="40px"
                                                            Buttons-SpinButtonsDisplay="None" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ValidationGroup="task" CssClass="validation1"
                                                    ValidateEmptyText="true"
                                                    ErrorMessage="Please select and complete repeat series."
                                                    OnServerValidate="RepeatMonthly_Validate" Display="Dynamic">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Panel ID="pnlRepeatYearly" runat="server" Visible="false">
                                        <table border="0">
                                            <tr>
                                                <td class="left nowrap">
                                                    <asp:CheckBox ID="cbxRecurringYearlyOnEvery" runat="server" TabIndex="50" />
                                                    <ajaxToolkit:MutuallyExclusiveCheckBoxExtender runat="server"
                                                        ID="MutuallyExclusiveCheckBoxExtender4" TargetControlID="cbxRecurringYearlyOnEvery" Key="RecurringYearlyOnEvery" />
                                                </td>
                                                <td class="left nowrap">On every</td>
                                                <td class="left nowrap">
                                                    <asp:DropDownList ID="ddlRepeatYearlyOnEveryMonth" runat="server" Width="50px" CssClass="no_min_width" TabIndex="51">
                                                        <asp:ListItem Text="January" Value="1" />
                                                        <asp:ListItem Text="February" Value="2" />
                                                        <asp:ListItem Text="March" Value="3" />
                                                        <asp:ListItem Text="April" Value="4" />
                                                        <asp:ListItem Text="May" Value="5" />
                                                        <asp:ListItem Text="June" Value="6" />
                                                        <asp:ListItem Text="July" Value="7" />
                                                        <asp:ListItem Text="August" Value="8" />
                                                        <asp:ListItem Text="September" Value="9" />
                                                        <asp:ListItem Text="October" Value="10" />
                                                        <asp:ListItem Text="November" Value="11" />
                                                        <asp:ListItem Text="December" Value="12" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="left nowrap">
                                                    <ig:WebNumericEditor ID="txtRecurringYearlyOnEveryMonthDay" runat="server" TextMode="Number" TabIndex="52"
                                                        MinValue="1" MaxValue="31" Height="24px" DataMode="Int"
                                                        MaxLength="2" Width="50px">
                                                        <Buttons SpinButtonsDisplay="OnRight" ListOfValues="1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31"
                                                            SpinOnArrowKeys="true"
                                                            SpinOnReadOnly="true" SpinWrap="true" />
                                                    </ig:WebNumericEditor>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="cbxRecurringYearlyOn" runat="server" TabIndex="53" />
                                                    <ajaxToolkit:MutuallyExclusiveCheckBoxExtender runat="server"
                                                        ID="MutuallyExclusiveCheckBoxExtender5" TargetControlID="cbxRecurringYearlyOn" Key="RecurringYearlyOnEvery" />
                                                </td>
                                                <td>On</td>
                                                <td class="left nowrap">
                                                    <asp:DropDownList ID="ddlRecurringYearlyOn" runat="server" Width="50px" CssClass="no_min_width" TabIndex="54">
                                                        <asp:ListItem Text="First" Value="1" />
                                                        <asp:ListItem Text="Second" Value="2" />
                                                        <asp:ListItem Text="Third" Value="3" />
                                                        <asp:ListItem Text="Fourth" Value="4" />
                                                        <asp:ListItem Text="Last" Value="5" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="left nowrap">
                                                    <asp:DropDownList ID="ddlRecurringYearlyWeekDay" runat="server" Width="50px" CssClass="no_min_width" TabIndex="55">
                                                        <asp:ListItem Text="Sunday" Value="0" />
                                                        <asp:ListItem Text="Monday" Value="1" />
                                                        <asp:ListItem Text="Tuesday" Value="2" />
                                                        <asp:ListItem Text="Wednesday" Value="3" />
                                                        <asp:ListItem Text="Thursday" Value="4" />
                                                        <asp:ListItem Text="Friday" Value="5" />
                                                        <asp:ListItem Text="Saturday" Value="6" />
                                                    </asp:DropDownList>
                                                    of 
                                                     <asp:DropDownList ID="ddlRecurringYearlyMonth" runat="server" Width="50px" CssClass="no_min_width" TabIndex="56">
                                                         <asp:ListItem Text="January" Value="1" />
                                                         <asp:ListItem Text="February" Value="2" />
                                                         <asp:ListItem Text="March" Value="3" />
                                                         <asp:ListItem Text="April" Value="4" />
                                                         <asp:ListItem Text="May" Value="5" />
                                                         <asp:ListItem Text="June" Value="6" />
                                                         <asp:ListItem Text="July" Value="7" />
                                                         <asp:ListItem Text="August" Value="8" />
                                                         <asp:ListItem Text="September" Value="9" />
                                                         <asp:ListItem Text="October" Value="10" />
                                                         <asp:ListItem Text="November" Value="11" />
                                                         <asp:ListItem Text="December" Value="12" />
                                                     </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <div>
                                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidationGroup="task" CssClass="validation1"
                                                ValidateEmptyText="true"
                                                ErrorMessage="Please select and complete repeat series."
                                                OnServerValidate="RepeatYearly_Validate" Display="Dynamic">
                                            </asp:CustomValidator>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                            </div>
                        </panel>
                       <%-- <h3>Reminder</h3>

                        <table style="width: 100%;" class="editForm no_min_width">
                            <tr>
                                <td class="right" style="width: 30%">When</td>
                                <td class="redstar"></td>
                                <td style="width: 20%;">
                                    <asp:DropDownList ID="ddlReminderWhen" runat="server" Width="135px" TabIndex="60">
                                        <asp:ListItem Text="--- Select ---" Value="0" />
                                        <asp:ListItem Text="On Time" Value="0" />
                                        <asp:ListItem Text="15 mins" Value="15" />
                                        <asp:ListItem Text="30 mins" Value="30" />
                                        <asp:ListItem Text="1 hour" Value="60" />
                                        <asp:ListItem Text="2 hours" Value="120" />
                                        <asp:ListItem Text="6 hours" Value="360" />
                                        <asp:ListItem Text="12 hours" Value="720" />
                                        <asp:ListItem Text="1 day" Value="1440" />
                                        <asp:ListItem Text="2 days" Value="2880" />
                                        <asp:ListItem Text="3 days" Value="4320" />
                                        <asp:ListItem Text="4 days" Value="5760" />
                                        <asp:ListItem Text="5 days" Value="7200" />
                                        <asp:ListItem Text="6 days" Value="8640" />
                                        <asp:ListItem Text="1 week" Value="10080" />
                                        <asp:ListItem Text="2 weeks" Value="20160" />
                                        <asp:ListItem Text="3 weeks" Value="30240" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnClearReminder" runat="server" Text="Delete" OnClick="lbtnClearReminder_Click" CssClass="link" TabIndex="61" />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Repeat</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlReminderRepeatFrequency" runat="server" Width="135px" TabIndex="62">
                                        <asp:ListItem Text="None" Value="0" />
                                        <asp:ListItem Text="Daily" Value="1" />
                                        <asp:ListItem Text="Weekly" Value="2" />
                                        <asp:ListItem Text="Monthly" Value="3" />
                                        <asp:ListItem Text="Yearly" Value="4" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Alert</td>
                                <td class="redstar"></td>
                                <td>
                                    <asp:DropDownList ID="ddlAlertType" runat="server" Width="135px" TabIndex="63">
                                        <asp:ListItem Text="Email" Value="1" />
                                        <asp:ListItem Text="Pop-up" Value="2" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>--%>


                    </div>


                </div>







                <div style="margin-top: 15px" class="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="task" TabIndex="70" />
                    &nbsp;
                    <asp:Button ID="btnSaveNew" runat="server" Text="Save & New " CssClass="mysubmit" OnClick="btnSaveNew_Click" CausesValidation="true" ValidationGroup="task" TabIndex="71" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" CssClass="mysubmit" TabIndex="72" />
                </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <uc1:ucContactLookup ID="contactLookup" runat="server" />

    <asp:HiddenField ID="hf_contactID" runat="server" Value="0" />

    <script type="text/javascript">

        // step 1
        function txtContact_focus(sender, args) {
            showContactDialog();
        }

        function gvContacts_rowsSelected(sender, args) {
            var selectedRows = args.getSelectedRows();

            var contactID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
            var contactName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

            // save id in hidden field
            $("#<%= hf_contactID.ClientID %>").val(contactID);

            // update textbox
            $find("<%= txtContact.ClientID %>").set_value(contactName);
        }

    </script>
</asp:Content>
