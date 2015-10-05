<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Integration with Google Calendar
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Loading events from calendar
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Events get loaded from the public google calendar. <br />
    . 
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<% Html.RenderPartial("Scheduler")%> 
<script>
    //apply local time offset
    scheduler.attachEvent("onEventLoading", function (event_object) {
        var offset = new Date(event_object.start_date).getTimezoneOffset();
        event_object.start_date = new Date(event_object.start_date.getTime() - offset * 60 * 1000);
        event_object.end_date = new Date(event_object.end_date.getTime() - offset * 60 * 1000);
        return true;
    });
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Google Calendar
</asp:Content>