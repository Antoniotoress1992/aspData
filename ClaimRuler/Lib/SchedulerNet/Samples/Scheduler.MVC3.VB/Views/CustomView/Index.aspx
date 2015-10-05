<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Custom view
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Extend scheduler functionality
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    You can create your own views.<br />
    See custom "Decade" and "Work Week" views
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<%= Model.GenerateJS()%> 
<%= Model.GenerateCSS()%> 
<script>

    scheduler.attachEvent("onTemplatesReady", function () {
        //work week
        scheduler.date.workweek_start = scheduler.date.week_start;
        scheduler.templates.workweek_date = scheduler.templates.week_date;
        scheduler.templates.workweek_scale_date = scheduler.templates.week_scale_date;
        scheduler.date.add_workweek = function (date, inc) { return scheduler.date.add(date, inc * 7, "day"); }
        scheduler.date.get_workweek_end = function (date) { return scheduler.date.add(date, 5, "day"); }


        //decade
        scheduler.date.decade_start = function (date) {
            var ndate = new Date(date.valueOf());
            ndate.setDate(Math.floor(date.getDate() / 10) * 10 + 1);
            return this.date_part(ndate);
        }
        scheduler.templates.decade_date = scheduler.templates.week_date;
        scheduler.templates.decade_scale_date = scheduler.templates.week_scale_date;
        scheduler.date.add_decade = function (date, inc) { return scheduler.date.add(date, inc * 10, "day"); }
    });
</script>

<%= Model.GenerateHTML() %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Custom view
</asp:Content>