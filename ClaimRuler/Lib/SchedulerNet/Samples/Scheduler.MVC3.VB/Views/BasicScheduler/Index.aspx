<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Basic initialization
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <%= Model.Render() %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SampleTitle" runat="server">
    Basic initialization
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ShortDescription" runat="server">
See how scheduler can be initialized
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="LongDescription" runat="server">
    Drag or double-click to create new event<br />
    Double-click on existing events to edit them, or drag them to change their time. 
</asp:Content>
