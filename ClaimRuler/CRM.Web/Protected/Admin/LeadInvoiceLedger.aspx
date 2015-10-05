<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
    CodeBehind="LeadInvoiceLedger.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadInvoiceLedger" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Invoice Ledger
        </div>
        <div class="paneContentInner">
            <div>
                <asp:LinkButton ID="lbtrnSearchPanel" runat="server" CssClass="toolbar-item" OnClick="lbtrnSearchPanel_Click" Width="66px">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/toolbar_search.png)">Search</span>
                    </asp:LinkButton>                
            </div>
            <div class="message_area">
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <table style="width: 100%;" class="editForm no_min_width">
                <tr>
                    <td class="top lef" style="width: 200px;" id="tdsearch" runat="server">
                        <div class="boxContainer">
                            <div class="section-title">
                                Filters
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <div style="float: left;">
                                            <asp:ImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" ImageUrl="~/Images/searchbg.png" />
                                        </div>
                                        <div style="float: right;">
                                            <asp:LinkButton ID="lbtnClear" runat="server" OnClick="lbtnClear_Click" Text="Clear" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>Create Date From</div>
                                        <ig:WebDatePicker ID="txtDateFrom" runat="server" Width="100px" TabIndex="1"
                                            Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif">
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            Create Date To
                                        </div>
                                        <ig:WebDatePicker ID="txtDateTo" runat="server" Width="100px" TabIndex="2"
                                            Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif">
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td class="top lef">
                        <asp:GridView ID="gvInvoices" Width="99%" runat="server"
                            AutoGenerateColumns="False"
                            AllowSorting="true"
                            AlternatingRowStyle-BackColor="#e8f2ff"
                            CssClass="gridView"
                            CellPadding="4"
                            OnRowDataBound="gvInvoices_RowDataBound"
                            OnRowCommand="gvInvoices_RowCommand"
                            PageSize="20"
                            PagerSettings-PageButtonCount="5">
                            <PagerStyle CssClass="pager" />
                            <RowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                No invoices available.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                                    ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server"
                                            ToolTip="Edit" 
                                            ImageUrl="~/Images/edit.png"
                                            />
                                        &nbsp;
					                    <asp:ImageButton ID="btnPrint" runat="server" 
                                            ToolTip="Print" 
                                            ImageUrl="~/Images/print_icon.png"                                          
                                            />
                                        &nbsp;
					                    <asp:ImageButton ID="btnDelete" runat="server"
                                            CommandName="DoDelete"
                                            OnClientClick="javascript:return ConfirmDialog(this,'Are you sure you want to void this invoice?');"
                                            CommandArgument='<%#Eval("InvoiceID") %>'
                                            ToolTip="Delete"
                                            ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Date" 
                                    ItemStyle-HorizontalAlign="Center"> <%--SortExpression="InvoiceDate"--%>
                                    <ItemTemplate>
                                        <%# Eval("InvoiceDate", "{0:MM-dd-yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due Date" ItemStyle-HorizontalAlign="Center"
                                    >   <%--SortExpression="DueDate"--%>
                                    <ItemTemplate>
                                        <%# Eval("DueDate", "{0:MM-dd-yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Right"
                                    > <%--SortExpression="AdjusterInvoiceNumber"--%>
                                    <ItemTemplate>
                                        <%# Eval("InvoiceNumber", "{0:N0}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Insurer Claim ID #" ItemStyle-HorizontalAlign="Center"
                                    >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnClaim" runat="server" Text=' <%# Eval("Claim.InsurerClaimNumber")%>' OnClick="lbtnClaim_Click" ValidationGroup=' <%# Eval("Claim.AdjusterClaimNumber")%>' />
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payee Name" 
                                    ItemStyle-HorizontalAlign="Center"> <%--SortExpression="BillToName"--%>
                                    <ItemTemplate>
                                        <%# Eval("BillToName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount" ItemStyle-HorizontalAlign="right"
                                    > <%--SortExpression="TotalAmount"--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("TotalAmount", "{0:N2}")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            


        </div>
    </div>
    <script type="text/javascript">    
        function printInvoice(invoiceID) {
            PopupCenter("../../Content/PrintInvoice.aspx?q=" + invoiceID.toString(), "Invoice", 800, 800);
            return false;
        }
</script>
</asp:Content>

