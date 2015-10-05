<%@ Page Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="InvoiceApprovalRule.aspx.cs" Inherits="CRM.Web.Protected.Admin.InvoiceApprovalRule" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Invoice Approval Rules
        </div>
    </div>
    <div class="paneContentInner">
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <table style="width: 100%;" class="editForm no_min_width">
                    <tr>
                        <td style="width: 10%;" class="right">Role Name</td>
                        <td>
                            <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlEditRule" runat="server" Visible="false" DefaultButton="btnSave">
                                <div class="boxContainer" style="width: 600px">
                                    <div class="section-title">
                                        Approval Rule
                                    </div>
                                    <div class="paneContentInner">
                                        <table class="editForm">
                                            <tr>
                                                <td class="right">From Amount
                                                </td>
                                                <td>
                                                    <ig:WebNumericEditor ID="txtAmountFrom" runat="server" DataMode="Decimal" MinDecimalPlaces="2"></ig:WebNumericEditor>
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvtxtAmountFrom" runat="server" ControlToValidate="txtAmountFrom"
                                                            SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please enter amount." ValidationGroup="Rule" Display="Dynamic" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="right">To Amount
                                                </td>
                                                <td>
                                                    <ig:WebNumericEditor ID="txtAmountTo" runat="server" DataMode="Decimal" MinDecimalPlaces="2"></ig:WebNumericEditor>
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="tfvtxtAmountTo" runat="server" ControlToValidate="txtAmountTo"
                                                            SetFocusOnError="true" CssClass="validation1" ErrorMessage="Please enter amount." ValidationGroup="Rule" Display="Dynamic" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSave_Click" ValidationGroup="Rule" CausesValidation="true" />
                                                    &nbsp;
											<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="mysubmit" OnClick="btnCancel_Click" CausesValidation="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                                <div style="margin-bottom: 5px;">
                                    <asp:LinkButton ID="lbtnNew" runat="server" CssClass="link" OnClick="lbtnNew_Click" Text="New Rule" />
                                </div>
                                <asp:GridView ID="gvInvoiceApprovalRules" runat="server" Width="50%" AutoGenerateColumns="False" CssClass="gridView"
                                    HorizontalAlign="Left" CellPadding="2" OnRowCommand="gvInvoiceApprovalRules_RowCommand">
                                    <RowStyle HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        No rules defined.
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" HeaderStyle-BackColor="#e8f2ff"
                                            ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEdit" runat="server"
                                                    CommandName="DoEdit"
                                                    CommandArgument='<%#Eval("ID") %>'
                                                    ToolTip="Edit"
                                                    ImageUrl="~/Images/edit.png"
                                                    Visible='<%# Master.hasEditPermission %>' />
                                                &nbsp;
												<asp:ImageButton ID="btnDelete" runat="server" 
                                                    CommandName="DoDelete" 
                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this permission?');"
                                                    CommandArgument='<%#Eval("ID") %>' 
                                                    ToolTip="Delete" 
                                                    ImageUrl="~/Images/delete_icon.png"
                                                    Visible='<%# Master.hasDeletePermission %>'></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Amount">
                                            <ItemTemplate>
                                                <%#Eval("AmountFrom", "{0:N2}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Amount">
                                            <ItemTemplate>
                                                <%#Eval("AmountTo", "{0:N2}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>


            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
    <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
</asp:Content>
