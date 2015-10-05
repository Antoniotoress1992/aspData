<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Quick Info
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    View event details
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Click on event to open qick info popup<br />
    Replace standard sidebar buttons and simplified edit form (which are quite small and hard-to-target on touch devices) with new ones, bigger and handier. 
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <%= Model.Render()%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Quick Info
</asp:Content>