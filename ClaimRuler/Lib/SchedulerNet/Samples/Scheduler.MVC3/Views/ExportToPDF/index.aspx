<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage<DHTMLX.Scheduler.DHXScheduler>" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Export
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Export to PDF
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Export calendar to PDF
</asp:Content>





<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<div style="text-align:center">
    <input type="button" value="Print" onclick="<%= Model.ToPDF("http://dhtmlxscheduler.appspot.com/export/pdf")%>" />
   </div>
    <% Html.RenderPartial("Scheduler"); %> 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Export to PDF
</asp:Content>
