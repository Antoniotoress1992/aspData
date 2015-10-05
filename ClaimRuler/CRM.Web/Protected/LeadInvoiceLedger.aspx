<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true"
    CodeBehind="LeadInvoiceLedger.aspx.cs" Inherits="CRM.Web.Protected.LeadInvoiceLedger" %>

<%@ Register Src="~/UserControl/Admin/ucInvoiceLedger.ascx" TagName="ucInvoiceLedger" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Admin/ucInvoiceLedgerLegacy.ascx" TagName="ucInvoiceLedgerLegacy" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Invoice Ledger
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">


    <td>
        <asp:LinkButton ID="btnNewInvoice" runat="server" Text="Return to Claim" CssClass="toolbar-item" PostBackUrl="~/Protected/LeadInvoice.aspx">
					<span class="toolbar-img-and-text" style="background-image: url(../images/add.png)">New Invoice</span>
        </asp:LinkButton>
    </td>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="boxContainer">
                <div class="section-title">
                    Current Invoice Ledger
                </div>
                <uc1:ucInvoiceLedger ID="ucInvoiceLedger1" runat="server" />

            </div>
             <div class="boxContainer">
                <div class="section-title">
                    Previous Invoices 
                </div>
                <uc2:ucInvoiceLedgerLegacy ID="legacyInvoices" runat="server" />

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
