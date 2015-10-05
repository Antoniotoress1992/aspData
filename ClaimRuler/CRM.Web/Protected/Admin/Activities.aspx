<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Activities.aspx.cs" Inherits="CRM.Web.Protected.Admin.Activities" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebSchedule.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="page-title">
        Activities 
    </div>
   


            <div class="toolbar toolbar-body">
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnNewTask" runat="server" CssClass="toolbar-item" PostBackUrl="~/protected/TaskEdit.aspx" CausesValidation="false">
					        <span class="toolbar-img-and-text" style="background-image: url(../../images/tasks.png)">New Task</span>
                            </asp:LinkButton>
                        </td>

                        <td>
                            <asp:LinkButton ID="btnNewEvent" runat="server" CssClass="toolbar-item" PostBackUrl="~/protected/EventEdit.aspx" CausesValidation="false">
					        <span class="toolbar-img-and-text" style="background-image: url(../../images/event.png)">New Event</span>
                            </asp:LinkButton>
                        </td>

                        <td>
                            <asp:LinkButton ID="btnLogCall" runat="server" CssClass="toolbar-item" CausesValidation="false" Visible="false">
					        <span class="toolbar-img-and-text" style="background-image: url(../../images/phone.png)">Log Call</span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="paneContentInner">

                <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 0px;" border="0">
                    <tr>
                        <td class="top left" runat="server" id="td_tasks">
                            <div class="section-title">
                                Tasks
                            </div>
                            <ig:WebDataGrid ID="gvTasks" runat="server"
                                AutoGenerateColumns="false"
                                CssClass="gridView smallheader"
                                DataSourceID="edsTasks"
                                Height="650px"
                                OnInitializeRow="gvTasks_InitializeRow"
                                OnItemCommand="gvTasks_ItemCommand"
                                Width="100%">
                                <EmptyRowsTemplate>
                                    No Activities found
                                </EmptyRowsTemplate>
                                <Columns>
                                    <ig:TemplateDataField CssClass="center" Key="commands" Width="40px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server"
                                                ImageAlign="Middle"
                                                ImageUrl="~/Images/edit.png"
                                                ToolTip="Edit"
                                                CommandName="DoEdit"
                                                Visible='<%# CRM.Core.PermissionHelper.checkEditPermission("Tasks.aspx") %>'
                                                CommandArgument='<%# string.Format("{0}|{1}", Eval("TaskID"),Eval("Activity")) %>' />

                                            <asp:ImageButton ID="btnDelete" runat="server"
                                                ImageAlign="Middle"
                                                ImageUrl="~/Images/delete_icon.png"
                                                ToolTip="Delete"
                                                CommandName="DoDelete"
                                                CommandArgument='<%#Eval("TaskID") %>'
                                                Visible='<%# CRM.Core.PermissionHelper.checkDeletePermission("Tasks.aspx") %>'
                                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this activity?');" />
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Header-Text="Date Due" Key="DateDue" Width="65px">
                                        <ItemTemplate>
                                            <div class="center">
                                                <%# Eval("DateDue", "{0:MM/dd/yyyy}") %>
                                            </div>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Header-Text="Status" Key="TaskStatusName" Width="60px">
                                        <ItemTemplate>
                                            <div class="center">
                                                <%# Eval("TaskStatusName") %>
                                            </div>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Header-Text="Priority" Key="PriorityName" Width="60px">
                                        <ItemTemplate>
                                            <div class="center">
                                                <%# Eval("PriorityName") %>
                                            </div>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Header-Text="Subject / Description" Key="Subject">
                                        <ItemTemplate>
                                            <div class="center">
                                                <%# Eval("Subject") %>
                                            </div>
                                            <div class="center">
                                                <asp:HyperLink ID="hlnkLead" runat="server" />
                                            </div>
                                            <div class="center">
                                                <%# Eval("Description") %>
                                            </div>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Header-Text="User Name" Key="UserName" Width="100px">
                                        <ItemTemplate>
                                            <div class="center">
                                                <%# Eval("UserName") %>
                                            </div>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                </Columns>
                                <EditorProviders>
                                    <ig:TextBoxProvider ID="TextBoxProvider" />
                                </EditorProviders>

                                <Behaviors>

                                    <ig:Selection RowSelectType="Single" CellClickAction="Row" Enabled="true">
                                    </ig:Selection>
                                    <ig:Sorting Enabled="true" SortingMode="Single">
                                        <ColumnSettings>
                                            <ig:SortingColumnSetting ColumnKey="commands" Sortable="false" />
                                            <ig:SortingColumnSetting ColumnKey="Description" Sortable="false" />
                                        </ColumnSettings>
                                    </ig:Sorting>
                                    <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                                        ThresholdFactor="0.5" Enabled="true" />

                                </Behaviors>
                            </ig:WebDataGrid>
                        </td>
                        <td class="top left" style="width: 50%">
                            <div class="boxContainer">
                                <div class="section-title">
                                    Events
                                </div>
                                <table class="editForm" style="width: 100%">
                                    <tr>
                                        <td style="width: 15%">
                                            <label>View Events for: &nbsp;</label>
                                        </td>
                                        <td class="left">
                                            <asp:DropDownList ID="ddlUsers" runat="server" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged" />
                                        </td>
                                        <td class="right">
                                            <asp:ImageButton ID="ibtn1Day" runat="server" ImageUrl="~/Images/1day.png" ImageAlign="Middle" OnClick="ibtn1Day_Click" ToolTip="View 1 day" />
                                            <asp:ImageButton ID="ibtn5Days" runat="server" ImageUrl="~/Images/5days.png" ImageAlign="Middle" OnClick="ibtn5Days_Click" ToolTip="View 5 days" />
                                            <asp:ImageButton ID="ibtnMonth" runat="server" ImageUrl="~/Images/ig_calendar.gif" ImageAlign="Middle" OnClick="ibtnMonth_Click" ToolTip="View 1 month" />
                                        </td>
                                    </tr>
                                </table>


                            </div>
                            <asp:Panel ID="pnlDayView" runat="server">
                                <igsch:WebDayView ID="WebDayView1" runat="server" WebScheduleInfoID="WebScheduleInfo1"
                                    Width="100%"
                                    VisibleDays="1"
                                    EnableAppStyling="True"
                                    Height="615px">
                                </igsch:WebDayView>
                            </asp:Panel>
                            <asp:Panel ID="pnlMonthView" runat="server" Visible="false">
                                <igsch:WebMonthView ID="WebMonthView1" runat="server"
                                    EnableAppStyling="True"
                                    EnableMultiResourceCaption="true"
                                    NavigationAnimation="None"
                                    NavigationButtonsVisible="true"
                                    WeekNumbersVisible="false"
                                    WeekendDisplayFormat="Full"
                                    WebScheduleInfoID="WebScheduleInfo1"
                                    Width="100%"
                                    Height="650px">
                                </igsch:WebMonthView>
                            </asp:Panel>
                            <igsch:WebScheduleInfo ID="WebScheduleInfo1" runat="server"
                                AllowAllDayEvents="false"
                                EnableAutoActivityDialog="true"
                                EnableRecurringActivities="true"
                                EnableEmailReminders="true"
                                EnableMultiResourceCaption="true"
                                EnableMultiResourceView="true"
                                EnableMultiDayEventBanner="true"
                                EnableReminders="true"
                                EnableMultiDayEventArrows="true"
                                EnableUnassignedResource="false"
                                EnableSmartCallbacks="true"
                                OnActiveDayChanged="WebScheduleInfo1_ActiveDayChanged"                              
                                FirstDayOfWeek="Sunday">
                                <ClientEvents ActivityDialogOpening="openAppointmentDialog" />
                            </igsch:WebScheduleInfo>



                        </td>
                    </tr>

                </table>


            </div>

            <!-- Exclude task status 2 = closed, 3 = completed -->
            <asp:EntityDataSource ID="edsTasks" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
                EnableFlattening="False" EntitySetName="vw_Task"
                Where="it.ClientId = @ClientID && it.Activity = 1 && it.TaskStatusID != 2 && it.TaskStatusID != 3"
                OrderBy="it.DateDue Asc"
                ViewStateMode="Enabled">
                <WhereParameters>
                    <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
                </WhereParameters>
            </asp:EntityDataSource>
        
    <script type="text/javascript">

              
        function openAppointmentDialog(scheduleInfo, evnt, dialog, activity) {
            evnt.cancel = true;
            // pass to appointment form
            $("#startDateTime").val(activity.getStartDateTime());


            var id = activity.getDataKey();
            var strID = (id == null ? "0" : id.toString());

            PopupCenter("../../Protected/EventEditPopUp.aspx?&id=" + strID, "Event", 900, 650);
            //var url = "../../Protected/EventEdit.aspx?&id=" + strID;
            //window.location.href = url;
           
        }
              
    </script>
</asp:Content>
