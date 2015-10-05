<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Color time sections
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Mark time sections
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    You can mark certain calendar sections with color<br />
    (e.g., days off, holidays or blocked time sections).
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%= Model.GenerateJS()%>
    <%= Model.GenerateCSS()%>
    <style>
        .red_section {
            background-color: red;
            text-align: center;
            color:#E2B8AC;
            font-size:24px;
            opacity: 0.25;
            filter:alpha(opacity=25);
        } 
        .green_section {
            background-color: #77ff77;
            font-size:24px;
            opacity: 0.25;
            filter:alpha(opacity=25);
        } 
    </style>
    <%= Model.GenerateHTML() %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Hour areas color
</asp:Content>

