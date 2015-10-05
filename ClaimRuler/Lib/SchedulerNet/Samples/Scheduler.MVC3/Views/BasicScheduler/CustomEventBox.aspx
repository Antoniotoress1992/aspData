<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage<DHTMLX.Scheduler.DHXScheduler>" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Custom event boxes
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Customize the looks of events
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Customize the looks of events to render them in rectangular containers.<br />
    Drag or double-click to create a new event. Event duration period can be displayed in event boxes.
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%= Model.GenerateLinks() %>
    <style>
	    /* background color for whole container and it's border*/
        .my_event {
            background-color: #add8e6;
            border: 1px solid #778899;
        }
        .my_event div {
            background-color: transparent !important;
            border: none !important;
        }
          
        div.dhx_cal_editor
        {
            background-color:transparent;
        }
        
        /* styles for event content */
        .dhx_cal_event.my_event .my_event_body {
            padding-top: 3px;
            padding-left: 5px;
        }
        /* event's date information */
        .my_event .event_date {
            font-weight: bold;
            padding-right: 5px;
        }
		
    </style>
    <%= Model.GenerateHTML() %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Custom containers
</asp:Content>
