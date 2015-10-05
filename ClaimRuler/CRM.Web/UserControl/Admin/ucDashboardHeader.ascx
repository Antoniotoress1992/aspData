<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDashboardHeader.ascx.cs"
  Inherits="CRM.Web.UserControl.Admin.ucDashboardHeader" %>
<table width="970" border="0" cellspacing="0" cellpadding="00">
  <tr>
    <td align="center">
      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="logo_cont">
        <tr>
          <td width="49%">
            <div class="mylogo">
              <h1>
                <a href="#">
                  <img src="/images/logo.jpg" alt="" width="200" /></a></h1>
            </div>
          </td>
          <td width="51%" align="right" valign="middle">
            <br />
             <asp:LinkButton ID="lbtnSignout" runat="server" CssClass="report fright" Text="Sign Out" OnClick="logOut_Click" />
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
