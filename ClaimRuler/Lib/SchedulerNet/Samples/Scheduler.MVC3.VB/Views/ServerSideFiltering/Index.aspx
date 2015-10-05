<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHXSchedulerSamples.SchedulerTest.Controllers.ServerSideFilteringController.SchedulerFilterModel)" %>
<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
   Integration with other controls
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Server Side filtering
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Use control in the right corner of the page<br />
    Page will be reloaded and scheduler will load only events of selected room.
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<div style="position:absolute; right:200px; top:10px">
    <% Html.BeginForm("Index", "ServerSideFiltering", FormMethod.Get, Nothing)%>
        <%= Html.DropDownList("rooms", New SelectList(Model.Rooms, "key", "label", ViewData("rooms")))%>
       <input type="submit" name="filter" value="Filter" />
    <% Html.EndForm() %>
</div>
<%= Model.Scheduler.Render() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Server Side filtering
</asp:Content>