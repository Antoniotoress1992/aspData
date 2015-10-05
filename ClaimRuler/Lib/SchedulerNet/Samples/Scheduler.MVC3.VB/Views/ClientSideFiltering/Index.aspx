<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Client side filtering
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Filtering events on the client side
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Different filters can be attached to each view. <br />
    Month view - default,
    Week view - date between 09.06.2011 and 09.14.2011, 
    Day view - show events of the first room  
 
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<% Html.RenderPartial("Scheduler") %> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Client-side filter
</asp:Content>