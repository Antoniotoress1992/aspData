<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage( Of DHXSchedulerSamples.SchedulerTest.Controllers.MultiSchedulerController.[mod])" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Multiple schedulers
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Several calendars can be created on single page
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Each scheduler must have unique name. <br />
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

<div style="height:400px">
<%= Model.sh1.Render() %>
</div>
<div style="height:400px">
<%= Model.sh2.Render() %>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Multiple schedulers
</asp:Content>
