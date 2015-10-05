<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Integration with Google Maps
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Display Google Maps in calendar
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Integrate calendar with Google Maps to display event location on the map.<br />
    Location of a new event can be selected directly on the map.
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   
    <% Html.RenderPartial("Scheduler")%> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Google map support
</asp:Content>
