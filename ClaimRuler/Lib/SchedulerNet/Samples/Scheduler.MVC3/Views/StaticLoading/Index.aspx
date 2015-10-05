<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Static Loading
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Static Loading
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
   Data was parsed during scheduler configuration and rendered to html.
</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <% Html.RenderPartial("Scheduler"); %> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Static Loading
</asp:Content>
