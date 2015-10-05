<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Recurring.aspx.cs" Inherits="SchedulerNetAsp.Recurring1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
        <div style="height:509px; width: 100%;">
            <%= this.Scheduler.Render()%>
        </div>	
</asp:Content>