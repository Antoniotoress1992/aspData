<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Custom field
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Updating from the server side
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Event gets updated from server response after save. <br />
    Dbl-click to open details form and change the checkbox state there - after saving, color will be updated from server side. 
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<% Html.RenderPartial("Scheduler"); %> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Custom field
</asp:Content>