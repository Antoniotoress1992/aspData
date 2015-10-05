<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>



<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <% Html.RenderPartial("Scheduler"); %>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Scheduler | JQuery validation
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SampleTitle" runat="server">
     JQuery validation
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ShortDescription" runat="server">
    Using MVC unobtrusive validatiom
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="LongDescription" runat="server">
     Try to edit event<br> Text and Email fields are required
</asp:Content>
