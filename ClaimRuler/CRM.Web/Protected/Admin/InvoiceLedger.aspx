<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="InvoiceLedger.aspx.cs" Inherits="CRM.Web.Protected.Admin.InvoiceLedger" %>

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
            <table style="width: 100%;" class="editForm no_min_width">
                <tr>
                    <td class="top lef" style="width: 200px;">
                        <div class="boxContainer">
                            <div class="section-title">
                                Filters
                            </div>
                            <table style="width:100%;">
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
                            PageSize="20"
                            PagerSettings-PageButtonCount="5">
                            <PagerStyle CssClass="pager" />
                            <RowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                No invoices available.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Invoice Date" SortExpression="InvoiceDate"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("InvoiceDate", "{0:MM-dd-yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due Date" ItemStyle-HorizontalAlign="Center"
                                    SortExpression="DueDate">
                                    <ItemTemplate>
                                        <%# Eval("DueDate", "{0:MM-dd-yyyy}") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Right"
                                    SortExpression="AdjusterInvoiceNumber">
                                    <ItemTemplate>
                                        <%# Eval("InvoiceNumber", "{0:N0}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payee Name" SortExpression="BillToName"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("BillToName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount" ItemStyle-HorizontalAlign="right"
                                    SortExpression="TotalAmount">
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
</asp:Content>
