<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ExpenseList.aspx.cs" Inherits="CRM.Web.Protected.Admin.ExpenseList" %>

<%@ Register Assembly="Infragistics45.Web.v13.2, Version=13.2.20132.2077, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Expense Type List
        </div>

        <div class="toolbar toolbar-body">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnNew" runat="server" Text="" CssClass="toolbar-item"
                            OnClick="btnNew_Click"
                            Visible='<%# Master.hasAddPermission %>'>
						    <span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Expense Type</span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>

        <div class="paneContentInner">
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>

                    <div class="message_area">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </div>
                    <asp:Panel ID="pnlGrid" runat="server">
                        <asp:GridView ID="gvExpenseType" CssClass="gridView"
                            AllowPaging="true"
                            AllowSorting="true"
                            AutoGenerateColumns="False"
                            AlternatingRowStyle-BackColor="#e8f2ff"
                            CellPadding="2"
                            HorizontalAlign="Center"
                            OnPageIndexChanging="gvExpenseType_PageIndexChanging"
                            OnSorting="gvExpenseType_Sorting"
                            OnRowCommand="gvExpenseType_RowCommand"
                            PageSize="20"
                            Width="80%" runat="server"
                            PagerSettings-PageButtonCount="10">
                            <PagerStyle CssClass="pager" />
                            <RowStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                No Expenses defined.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Expense Name" SortExpression="ExpenseName">
                                    <ItemTemplate>
                                        <%#Eval("ExpenseName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expense Description">
                                    <ItemTemplate>
                                        <%#Eval("ExpenseDescription")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server"
                                            CommandName="DoEdit"
                                            CommandArgument='<%#Eval("ExpenseTypeID") %>'
                                            ToolTip="Edit"
                                            ImageUrl="~/Images/edit.png"
                                            Visible='<%# Master.hasEditPermission %>' />
                                        &nbsp;
				
                               
				                <asp:ImageButton ID="btnDelete" runat="server"
                                    CommandName="DoDelete"
                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this expense?');"
                                    CommandArgument='<%#Eval("ExpenseTypeID") %>'
                                    ToolTip="Delete"
                                    ImageUrl="~/Images/delete_icon.png"
                                    Visible='<%# Master.hasDeletePermission %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                    <asp:Panel ID="pnlEditExpense" runat="server" Visible="false" DefaultButton="btnSave">
                        <div class="boxContainer" style="width: 50%; margin: auto;">
                            <div class="section-title">
                                Expense Type Details
                            </div>
                            <table class="editForm" style="border-collapse: separate; border-spacing: 7px; padding: 2px; width: 100%;" border="0">
                                <tr>
                                    <td class="right">Expense Name</td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <ig:WebTextEditor ID="txtExpenseName" runat="server" Width="250px" MaxLength="100"></ig:WebTextEditor>
                                        <div>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtExpenseName"
                                                ErrorMessage="Please enter expense name." ValidationGroup="expense" Display="Dynamic"
                                                CssClass="validation1" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Expense Description</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtExpenseDescription" runat="server" Width="250px" MaxLength="100"></ig:WebTextEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" CausesValidation="true" ValidationGroup="expense" OnClick="btnSave_Click" />
                                        &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="mysubmit" CausesValidation="false" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

    </div>
</asp:Content>
