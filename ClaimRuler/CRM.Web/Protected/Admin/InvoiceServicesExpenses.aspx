<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="InvoiceServicesExpenses.aspx.cs" Inherits="CRM.Web.Protected.Admin.InvoiceServicesExpenses" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<%@ Register Src="~/UserControl/Admin/ucServiceList.ascx" TagName="ucServiceList" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Admin/ucExpenseList.ascx" TagName="ucExpenseList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Invoice Services 
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <div class="paneContentInner">

                    <table style="width: 100%;" class="editForm no_min_width">
                        <tr>
                            <td class="top left">
                                <div runat="server" class="boxContainer">
                                    <div class="section-title">
                                        List of Services
                                    </div>
                                    <div>
                                        <uc1:ucServiceList ID="invoiceServices" runat="server" />
                                    </div>
                                </div>
                            </td>
                            <td class="top left" style="width: 50%">
                                <div runat="server" class="boxContainer">
                                    <div class="section-title">
                                        List of Expenses
                                    </div>
                                    <div class="paneContentInner">
                                        <uc2:ucExpenseList ID="invoiceExpenses" runat="server" />
                                    </div>
                                </div>
                            </td>

                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
