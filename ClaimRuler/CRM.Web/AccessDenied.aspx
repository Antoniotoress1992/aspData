<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="CRM.Web.AccessDenied" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
   
        <table style="width: 100%; text-align:center;">
            <tr>               
                <td>
                    <img runat="server" src="~/Images/access_denied.gif" height="150" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="mysubmit" OnClick="btnContinue_Click" />
                </td>
            </tr>
        </table>
</asp:Content>
