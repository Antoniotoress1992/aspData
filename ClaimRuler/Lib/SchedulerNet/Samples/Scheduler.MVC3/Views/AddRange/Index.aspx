<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Add range  
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Add range  
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Timeline and units view can be used to organize taks per resource.
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Check timeline and unit views. You can create and edit events in the normal way. 
</asp:Content>




 

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
   
    <% Html.RenderPartial("Scheduler"); %> 
</asp:Content>
