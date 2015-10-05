<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="CRM.Web.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div style="margin: auto; text-align: center;">
        <asp:Image runat="server" ID="imgError" ImageUrl="~/Images/error.png" />
        <h4>Whoops! An error has occurred.<br />
            System Administors have been notified.</h4>
        <br />
        <br />
        <asp:Button ID="btnContinue" runat="server" Text="Continue" PostBackUrl="~/Protected/dashboard.aspx" CssClass="mysubmit" />
    </div>
</asp:Content>
