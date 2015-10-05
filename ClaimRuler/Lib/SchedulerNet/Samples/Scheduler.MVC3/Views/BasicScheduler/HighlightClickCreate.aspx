<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage<DHTMLX.Scheduler.DHXScheduler>" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Create on click
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
   Create events in highlighted slots
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
   Create an event on click in highlighted slots.<br />
   The default event duration determines the size of highlighted area.  
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%= Model.GenerateJS() %>
    <%= Model.GenerateCSS() %>
    <style>
        .dhx_cal_event {
            z-index:1;
        }
        .highlight_section {
            opacity: 0.25;
            filter:alpha(opacity=25);
            z-index:0;
        } 
        .highlight_section:hover {
		    background-color: #90ee90;
	    }
    </style>
    <%= Model.GenerateHTML() %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Create on click
</asp:Content>
