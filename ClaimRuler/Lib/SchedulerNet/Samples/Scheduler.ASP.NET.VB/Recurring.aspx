<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Recurring.aspx.vb" Inherits="SchedulerAspNetVB.Recurring1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div style="height:509px; width: 100%;">
            <%= Me.Scheduler.Render()%>
        </div>	
</asp:Content>
