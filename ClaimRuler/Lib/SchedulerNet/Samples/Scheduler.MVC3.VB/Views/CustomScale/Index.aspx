<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Customized time scale
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
	<style>
		.day_split {
			  background-color: #77ff77;
			  opacity: 0.25;
		}
	</style>
    <%= Model.Render() %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SampleTitle" runat="server">
    Customized time scale
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ShortDescription" runat="server">
    Remove hours and days from time scale
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="LongDescription" runat="server">
    Drag or double-click to create new event<br />
    Double-click on existing events to edit them, or drag them to change their time. 
</asp:Content>
