<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Basic initialization
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    See how scheduler can be initialized
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Drag or double-click to create new event<br />
    Double-click on existing events to edit them, or drag them to change their time. 
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <% Html.RenderPartial("Scheduler"); %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Basic initialization
</asp:Content>
