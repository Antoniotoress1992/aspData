<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Grid view
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Grid view
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Grid view can be used to display events as a table.
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Check different configurations of Grid view. Double click on the area to create events. 
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">  
    <% Html.RenderPartial("Scheduler")%> 
</asp:Content>
