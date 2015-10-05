<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/scheduler.Master" Inherits="System.Web.Mvc.ViewPage<SchedulerTest.Controllers.ServerSideFilteringAjaxController.SchedulerFilterModel>" %>


<asp:Content ID="Content5" ContentPlaceHolderID="SampleTitle" runat="server">
    Integration with other controls
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ShortDescription" runat="server">
    Server Side filtering, ajax loading
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="LongDescription" runat="server">
    Use control in the right corner of the page<br />
    Scheduler will load only events of selected room.
</asp:Content>




<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<div style="position:absolute;  right:200px; top:10px"><%= Html.DropDownListFor(c => c.Rooms, new SelectList(Model.Rooms, "key", "label"), new { onchange = "reload()"})%>
    </div>

    <script>
        function reload() {
            
            scheduler.clearAll();
            scheduler.load("<%= Model.Scheduler.DataUrl %>rooms=" + document.getElementById("Rooms").value);
           
        }
    </script> 
     <%= Model.Scheduler.Render() %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
Scheduler | Server Side filtering, ajax loading
</asp:Content>