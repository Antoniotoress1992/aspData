<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage(Of DHTMLX.Scheduler.DHXScheduler)" %>

<asp:Content ID="Content6" ContentPlaceHolderID="SampleTitle" runat="server">
    Export
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ShortDescription" runat="server">
    Export to iCal
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="LongDescription" runat="server">
    Export calendar to iCal
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="Content" runat="server">
   <div style="text-align:center">
   <input type="button" value="Print" onclick="<%= Model.ToICal("/ExportToICal/Export", "data")%>" />
   </div>
    <%= Model.Render()%>

</asp:Content>

<asp:Content ID="Content10" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Export to iCal
</asp:Content>