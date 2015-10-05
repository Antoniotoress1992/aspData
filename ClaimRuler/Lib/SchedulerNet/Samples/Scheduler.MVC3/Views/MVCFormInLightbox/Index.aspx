<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    MVC Form in lightbox
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Custom lightbox form
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Try to edit event
</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
	    .custom_lightbox
	    {

	    }
	</style>

    <% Html.RenderPartial("Scheduler"); %>


   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | MVC Form in lightbox
</asp:Content>
