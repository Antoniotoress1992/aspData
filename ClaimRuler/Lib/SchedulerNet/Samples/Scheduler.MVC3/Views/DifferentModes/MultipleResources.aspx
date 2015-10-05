<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Multiple Resources
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Load data from different resources 
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    You can display events of different resources (e.g. employees, office locations, rooms)<br /> 
    on one calendar page.
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   
    <% Html.RenderPartial("Scheduler"); %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Multiple resources mode
</asp:Content>
