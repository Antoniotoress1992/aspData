<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Recurring events
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Enable event recurring support
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    You can enable/disable recurring events support and<br /> 
    select a recurring pattern for the series of events.
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%= Model.Render()%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Recurring events
</asp:Content>
