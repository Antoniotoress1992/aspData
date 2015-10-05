<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Year view
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Display events in year view
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    You can see assigned events in the year view. <br />
    Tooltips to show assigned events popup on mouseover.
</asp:Content>



<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   
    <% Html.RenderPartial("Scheduler"); %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Year view
</asp:Content>
