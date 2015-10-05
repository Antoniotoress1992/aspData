<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="AdjusterEdit.aspx.cs" Inherits="CRM.Web.Protected.Admin.AdjusterEdit" %>

<%@ Register Src="~/UserControl/Admin/ucAdjuster.ascx" TagName="ucAdjuster" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Adjuster Detail 
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <uc1:ucAdjuster ID="ucAdjuster1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <script type="text/javascript">      
      
    </script>
</asp:Content>
