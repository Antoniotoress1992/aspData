<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Authorization
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Managing users rights
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Different permissions for guests and logged in users. Scheduler is readonly for anonymous users, 
    logged in users can modify only they own events.<br />
    You can use "test" as login and password
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div style="position:absolute;top:10px;right:60px; background-color:White; text-align:center;"><% Html.RenderPartial("LogOnUserControl") %></div>

    <%= Model.Render() %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Managing users rights
</asp:Content>