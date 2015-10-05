<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Highlight pointer
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Mouseover highlighting
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Facilitate time selection for a new event by highlighting the area under cursor. <br /> 
    Double-click to create a new event.
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%= Model.GenerateJS()%>
    <%= Model.GenerateCSS()%>
    <style>
        .highlight_section {
            opacity: 0.25;
            z-index:0;
            filter:alpha(opacity=25);
        } 
        .highlight_section:hover {
		    background-color: #90ee90;
	    }
    </style>
    <%= Model.GenerateHTML() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Mouseover highligt
</asp:Content>
