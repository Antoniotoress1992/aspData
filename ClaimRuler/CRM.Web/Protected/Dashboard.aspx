<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CRM.Web.Protected.Dashboard" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics45.WebUI.WebSchedule.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<%@ Register Src="~/UserControl/Admin/ucReminderPopUp.ascx" TagName="ucReminderPopUp" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/ucClaimProgressChart.ascx" TagName="ucClaimProgressChart" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/ucRuleExceptionChart.ascx" TagName="ucRuleExceptionChart" TagPrefix="uc4" %>
<asp:Content ID="contentheader" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <%--<asp:Timer ID="timer1" runat="server" Interval="60000" OnTick="timer1_Tick" />--%>
    <div class="page-title">
        Dashboard 
    </div>

    <%--  <Triggers>
            <asp:AsyncPostBackTrigger ControlID="timer1" EventName="Tick" />
            
        </Triggers>--%>

    <div class="paneContentInner">
        <table style="width: 100%; border-collapse: separate; border-spacing: 0px; padding: 0px;" border="0">
            <tr>
                <td class="top">
                    <table>
                        <tr>
                            <td>
                                <div class="boxContainer">
                                    <div class="section-title">
                                        Claim Progress
                                    </div>

                                    <uc3:ucClaimProgressChart ID="claimProgressChart" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="boxContainer">
                                    <div class="section-title">
                                        Red Flag Exceptions
                                    </div>
                                    <uc4:ucRuleExceptionChart ID="ruleExceptionChart" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>

                </td>

                <td class="top">
                    <table>
                        <tr>
                            <td>

                                <div class="boxContainer" style="height:680px">
                                     <div class="section-title">
                                          <table>                                             <tr>                                                 <td width="300px">                                        Tasks</td>                                                 <td>                                                     Load Outlook Tasks                                                 </td>                                                 <td>                                         <asp:RadioButton ID="rbOn" runat="server" Text="On" OnCheckedChanged="rbOn_CheckedChanged" AutoPostBack="True" />                                                </td>                                                <td>                                         <asp:RadioButton ID="rbOff" runat="server" Text="Off" OnCheckedChanged="rbOff_CheckedChanged" AutoPostBack="True"/>                                                </td>                                                 </tr>                                             </table>
                                       
                                    </div>
                                    <ig:WebDataGrid ID="gvTasks" runat="server" Height="660px" Width="100%" AutoGenerateColumns="false"
                                        OnInitializeRow="gvTasks_InitializeRow" CssClass="gridView">
                                        <Columns>
                                            <ig:BoundDataField DataFieldName="start_date" Key="start_date" DataFormatString="{0:MM/dd/yyyy HH:mm tt}" Width="125px">
                                                <Header Text="Date" />
                                            </ig:BoundDataField>

                                            <ig:BoundDataField DataFieldName="statusName" Key="statusName" Width="60px">
                                                <Header Text="Status" />
                                            </ig:BoundDataField>

                                            <ig:BoundDataField DataFieldName="priorityName" Key="priorityName" Width="60px">
                                                <Header Text="Priority" />
                                            </ig:BoundDataField>

                                            <ig:TemplateDataField Header-Text="Event" Key="text" CssClass="center">
                                                <ItemTemplate>
                                                    <div>
                                                        <%# Eval("text") %>
                                                    </div>
                                                    <div>
                                                        <%# Eval("details") %>
                                                    </div>
                                                </ItemTemplate>
                                            </ig:TemplateDataField>

                                            <ig:TemplateDataField Key="lead_name">
                                                <Header Text="Policyholder" />
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hlnkLead" runat="server" />
                                                </ItemTemplate>
                                            </ig:TemplateDataField>

                                            <ig:BoundDataField DataFieldName="owner_name" Key="owner_name">
                                                <Header Text="User Name" />
                                            </ig:BoundDataField>


                                        </Columns>
                                        <Behaviors>
                                            <ig:ColumnResizing Enabled="true" />
                                            <ig:Filtering Alignment="Top" Visibility="Visible" Enabled="true" FilterType="ExcelStyleFilter">
                                                <ColumnSettings>
                                                    <ig:ColumnFilteringSetting ColumnKey="lead_name" Enabled="false" />
                                                    <ig:ColumnFilteringSetting ColumnKey="statusName" Enabled="true" />
                                                </ColumnSettings>
                                            </ig:Filtering>
                                            <ig:Sorting Enabled="true" SortingMode="Single" />
                                        </Behaviors>
                                    </ig:WebDataGrid>
                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="div_scheduler" style="display:none">
                                    <div class="section-title" style="margin-top: 5px;">
                                        Scheduler
                                    </div>
                                    <div class="toolbar toolbar-body">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="btn1Day" runat="server" Text="Day" CssClass="toolbar-item"
                                                        OnClick="changeCalendarView_Click">
													<span class="toolbar-img-and-text" style="background-image: url(../images/1day.png)">Day</span>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="btn5Days" runat="server" Text="Days" CssClass="toolbar-item"
                                                        OnClick="changeCalendarView_Click">
													<span class="toolbar-img-and-text" style="background-image: url(../images/5days.png)">Days</span>
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <td>
                                                        <asp:LinkButton ID="btnResizeFull" runat="server" Text="" CssClass="toolbar-item" href="#"
                                                            OnClientClick="resize_fullscreen();">
														<span class="toolbar-img-and-text" style="background-image: url(../images/resize_full.png)"></span>
                                                        </asp:LinkButton>
                                                    </td>

                                                </td>
                                                <td>
                                                    <td>
                                                        <asp:LinkButton ID="btnResizeNormal" runat="server" Text="" CssClass="toolbar-item" href="#"
                                                            OnClientClick="resize_normal();" Style="display: none;">
														<span class="toolbar-img-and-text" style="background-image: url(../images/resize_normal.png)"></span>
                                                        </asp:LinkButton>
                                                    </td>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <igsch:WebDayView ID="WebDayView1" runat="server" WebScheduleInfoID="WebScheduleInfo1"
                                        Width="100%" VisibleDays="5" EnableAppStyling="True">
                                    </igsch:WebDayView>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <igsch:WebScheduleInfo ID="WebScheduleInfo1" runat="server"
                        AllowAllDayEvents="true"
                        EnableRecurringActivities="True"
                        EnableEmailReminders="false"
                        EnableMultiResourceCaption="true"
                        EnableMultiResourceView="true"
                        EnableMultiDayEventBanner="true"
                        EnableReminders="true"
                        EnableMultiDayEventArrows="True"
                        EnableUnassignedResource="false"
                        EnableSmartCallbacks="true"
                        WorkDayStartTime="8:00 AM"
                        WorkDayEndTime="6:00 PM"
                        OnActiveResourceChanged="WebScheduleInfo1_ActiveResourceChanged"
                        OnActiveDayChanged="WebScheduleInfo1_ActiveDayChanged"
                        EnableProgressIndicator="true"
                        ReminderFormPath="~/Content/Reminder.aspx"
                        FirstDayOfWeek="Monday">
                        <ClientEvents ActivityDialogOpening="openAppointmentDialog" />
                    </igsch:WebScheduleInfo>
                </td>
            </tr>

        </table>

    </div>
    <asp:Button ID="btnRefreshTasksFromCalendar" OnClick="btnRefreshTasksFromCalendar_Click"
        runat="server" Style="display: none;" />

    <asp:HiddenField ID="hf_taskDate" runat="server" />
    <asp:HiddenField ID="hf_clientID" runat="server" />

    <div id="chart_drilldown_dialog" style="display: none;">
        <table id="grid_progress"></table>
    </div>
    <div id="ruleException_drilldown_dialog" style="display: none;">
        <table id="grid_ruleException"></table>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
        });
    </script>
    <script type="text/javascript">

        function resize_normal() {

            $("[id$='btnResizeFull']").show();
            $("[id$='btnResizeNormal']").hide();

            var pos_left = $("#pos_left").val();
            var pos_top = $("#pos_top").val();
            $("#isFullScreen").val("0");


            $('#div_scheduler').css({
                position: 'relative',
                top: pos_left,
                left: pos_top,
                width: '100%',
                height: '100%'
            });
        }

        function resize_fullscreen() {
            var pos_left = $("#div_scheduler").offset().left;
            var pos_top = $("#div_scheduler").offset().top;
            $("#isFullScreen").val("1");

            $("#pos_left").val(pos_left);
            $("#pos_top").val(pos_top);


            $('#div_scheduler').css({
                position: 'absolute',
                top: 0,
                left: 0,
                width: $(window).width(),
                height: $(window).height()
            });

            $("[id$='WebDayView1']").css({
                height: '99%'
            });
            $("[id$='WebDayView1_divScroll']").css({
                height: '99%'
            });

            $("[id$='btnResizeFull']").hide();
            $("[id$='btnResizeNormal']").show();
        }

        function openAppointmentDialog(scheduleInfo, evnt, dialog, activity) {
            evnt.cancel = true;

            // pass to appointment form
            $("#startDateTime").val(activity.getStartDateTime());


            var id = activity.getDataKey();
            var strID = (id == null ? "0" : id.toString());

            PopupCenter("../Content/AppointmentForm.aspx?id=" + strID, "Appointment", 700, 650);
        }

        function set_calendar_task_date(sender, args) {
            var selectedDate = args.get_dates()[0].toString().substr(0, 24);

            var ldate = new Date(selectedDate);


            $("#<%= hf_taskDate.ClientID %>").val(ldate.toUTCString());
            $("#<%= btnRefreshTasksFromCalendar.ClientID %>").click();
        }
    </script>

    <script type="text/javascript">
        // handles click on bar from rule exception chart
        function ruleExceptionDrilldown(ruleID, ruleName) {
            var clientID = $("#<%= hf_clientID.ClientID %>").val();
            var myParams = "{ 'clientID':'" + clientID + "', 'ruleID':'" + ruleID + "'}";


            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/getExceptionsForRule") %>',
                success: function (response) {
                    // refresh user photo
                    showExceptionsForRule(ruleName, response.d);
                },
                error: function (xhr, err) {
                    showAlert(xhr.responseText);
                }
            });
        }

        function showExceptionsForRule(ruleName, data) {
            // 1. prepare datagrid inside div
            var ruleExceptions = $.parseJSON(data);
            $("#grid_ruleException").igGrid({
                autoGenerateColumns: false,
                width: "100%",
                columns: [
                             { headerText: "Carrier/Client", key: "Carrier", dataType: "string", width: "20%" },//Business Rule
                             { headerText: "Red Flag Triggered", key: "BusinessRuleDescription", dataType: "string", width: "30%" },//Business Rule
                             { headerText: "User Name", key: "UserName", dataType: "string", width: "10%" },
                             { headerText: "What", key: "ObjectName", dataType: "string", width: "10%" },
                             { headerText: "Who", key: "url", dataType: "string", width: "30%" },
                             { headerText: "When", key: "ExceptionDate", dataType: "date", width: "15%", format: "dateTime" },
                             { headerText: "Insurer Claim #", key: "InsureClaim", dataType: "string", width: "20%", }

                ],
                features: [
                    {
                        name: "Responsive",
                    },
                    {
                        name: 'Paging',
                        type: "local",
                        pageSize: 20,
                        showPageSizeDropDown: false
                    },
                    {
                        name: "Sorting",
                        type: "local"
                    }
                ],
                dataSource: ruleExceptions
            });

            // 2. show dialog
            $("#ruleException_drilldown_dialog").dialog({
                modal: false,
                width: "50%",
                title: ruleName,
                close: function () {
                    $("#grid_ruleException").igGrid("destroy");
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

        function progressDrilldown(progressID, progressDescription) {
            var clientID = $("#<%= hf_clientID.ClientID %>").val();

            var carrierid = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_hdnCarrier").val();
            var adjusterid = $("#ctl00_WebSplitter1_tmpl1_ContentPlaceHolderMiddArea_claimProgressChart_hdnAdjuster").val();

            var myParams = "{ 'clientID':'" + clientID + "', 'progressID':'" + progressID + "','carrierid':'" + carrierid + "','adjusterid':'" + adjusterid + "'}";
            
            // set dialog title to progress description

            // get claims for progress ID
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/getClaimForProgress") %>',
                success: function (response) {
                    // refresh user photo
                    showClaimsForProgress(progressDescription, response.d);
                },
                error: function (xhr, err) {
                    showAlert(xhr.responseText);
                }
            });

        }

        function showClaimsForProgress(progressDescription, data) {
            // 1. prepare datagrid inside div
            var claims = $.parseJSON(data);
            $("#grid_progress").igGrid({
                autoGenerateColumns: false,
                width: "100%",
                columns: [
                             { headerText: "", key: "url", dataType: "string", width: "5%" },
                             { headerText: "Insured Name", key: "insuredName", dataType: "string", width: "30%" },
                             { headerText: "Loss Date", key: "lossDate", dataType: "date", width: "15%", format: "dateTime" },
                             { headerText: "Adjuster Name", key: "adjusterName", dataType: "string", width: "25%" },
                             { headerText: "Policy Type", key: "policyType", dataType: "string", width: "10%" },
                             { headerText: "Status", key: "claimStatus", dataType: "string", width: "10%" }

                ],
                features: [
                    {
                        name: "Responsive",
                    },
                    {
                        name: 'Paging',
                        type: "local",
                        pageSize: 20,
                        showPageSizeDropDown: false
                    },
                    {
                        name: "Sorting",
                        type: "local"
                    }
                ],
                dataSource: claims
            });



            // 2. show dialog
            $("#chart_drilldown_dialog").dialog({
                modal: false,
                width: "50%",
                title: progressDescription,
                close: function () {
                    $("#grid_progress").igGrid("destroy");
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
       <%-- function getExceptionChartData() {
            var clientID = $("#<%= hf_clientID.ClientID %>").val();
            var myParams = "{ 'clientID':'" + clientID + "'}";

            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                data: myParams,
                url: '<%= ResolveUrl("~/protected/AjaxClientServices.aspx/getRuleExceptionStatistics") %>',
                success: function (response) {
                    fillExceptionChart(response.d);
                },
                error: function (xhr, err) {
                    showAlert(xhr.responseText);
                }
            });

        }--%>
        //function fillExceptionChart(exceptionData) {
        //    var stats = $.parseJSON(exceptionData);
        //    $("#barChartException").igDataChart({
        //        seriesMouseLeftButtonDown: function (evt, ui) {
        //            alert(ui.item)
        //        },
        //        width: "98%",
        //        height: "300px",
        //        dataSource: stats,

        //        axes: [{
        //            name: "xAxis",
        //            type: "numericX"
        //        }, {
        //            name: "yAxis",
        //            type: "categoryY",
        //            label: "RuleName"
        //        }],
        //        series: [{
        //            name: "series1",
        //            title: "RuleName",
        //            type: "bar",
        //            isHighlightingEnabled: true,
        //            isTransitionInEnabled: true,
        //            xAxis: "xAxis",
        //            yAxis: "yAxis",
        //            valueMemberPath: "ExceptionCount"

        //        }]
        //    });
        //}
    </script>
</asp:Content>

 