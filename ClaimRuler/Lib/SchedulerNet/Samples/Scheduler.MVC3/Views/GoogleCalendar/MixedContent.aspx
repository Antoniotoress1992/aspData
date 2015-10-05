<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Integration with Google Calendar
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Loading events from calendar
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Loading events from calendar. <br />
    Events are loaded from google calendar and site database;
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<% Html.RenderPartial("Scheduler"); %> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Google Calendar
</asp:Content>