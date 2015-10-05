<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="CarrierInvoiceApproval.aspx.cs" Inherits="CRM.Web.Protected.Admin.CarrierInvoiceApproval" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div class="paneContent">
                <div class="page-title">
                    Carrier Invoice Approval 
                </div>
                <div class="toolbar toolbar-body">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnEmailInvoice" runat="server" CssClass="toolbar-item" OnClick="btnEmailInvoice_Click">
					                <span class="toolbar-img-and-text" style="background-image: url(../../images/mail_send.png)">Email Invoices</span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="paneContentInner">
                    <div class="message_area">
                        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
                    </div>
                    <asp:GridView ID="gvInvoice" CssClass="gridView" Width="90%" runat="server" HorizontalAlign="Center"
                        AutoGenerateColumns="False" CellPadding="2" DataKeyNames="InvoiceID"
                        OnSorting="gvInvoice_Sorting" AllowSorting="true"
                        OnRowDataBound="gvInvoice_RowDataBound"
                        OnRowCommand="gvInvoice_RowCommand"
                        AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
                        RowStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="50px" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbxApproved" runat="server" />
                                    &nbsp;
						            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoVoid" OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to void this invoice?');"
                                        CommandArgument='<%#Eval("InvoiceID") %>' ToolTip="Void Invoice" ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Number" SortExpression="InvoiceNumber">
                                <ItemStyle Width="100px" />
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlnkInvoice" runat="server" Target="_blank" Text='<%# Eval("InvoiceNumber") %>'
                                        NavigateUrl='<%# string.Format("javascript:showInvoice({0});", Eval("InvoiceID")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" SortExpression="InvoiceDate">
                                <ItemStyle Width="100px" />
                                <ItemTemplate>
                                    <%# Eval("InvoiceDate", "{0:MM/dd/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Type">
                                <ItemTemplate>
                                    <%# Eval("CarrierInvoiceType.InvoiceType") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Amount" SortExpression="TotalAmount">
                                <ItemStyle HorizontalAlign="Right" Width="200px" />
                                <ItemTemplate>
                                    <%# Eval("TotalAmount", "{0:N2}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Carrier">
                                <ItemTemplate>
                                    <%# Eval("Claim.LeadPolicy.Carrier.CarrierName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Policyholder">
                                <ItemTemplate>
                                    <%# string.IsNullOrEmpty(Eval("Lead.BusinessName").ToString()) ?  Eval("Lead.ClaimantLastName").ToString() + ", " + Eval("Lead.ClaimantFirstName").ToString() :  Eval("Lead.BusinessName")  %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div style="display:none;">
        <rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="false">
        </rsweb:ReportViewer>
    </div>
    <asp:HiddenField ID="hf_approvalRangeAmount" runat="server" />
    <script type="text/javascript">
        function showInvoice(invoiceID) {
            var url = '../../Content/PrintInvoice.aspx?id=' + invoiceID.toString();
            PopupCenter(url, 'Invoice', 800, 800);
        }
    </script>
</asp:Content>
