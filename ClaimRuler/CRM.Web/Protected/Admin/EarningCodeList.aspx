<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="EarningCodeList.aspx.cs" Inherits="CRM.Web.Protected.Admin.EarningCodeList" %>

<%@ Register Assembly="Infragistics45.Web.v13.2, Version=13.2.20132.2077, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Earning Code
        </div>

        <div class="toolbar toolbar-body">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnNew" runat="server" Text="" CssClass="toolbar-item"
                            OnClick="btnNew_Click"
                            Visible='<%# Master.hasAddPermission %>'>
						    <span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Earning Code</span>
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
                        <asp:GridView ID="gvEarningCode" CssClass="gridView"
                            AllowPaging="true"
                            AllowSorting="true"
                            AutoGenerateColumns="False"
                            AlternatingRowStyle-BackColor="#e8f2ff"
                            CellPadding="2"
                            HorizontalAlign="Center"
                            OnPageIndexChanging="gvEarningCode_PageIndexChanging"
                            OnSorting="gvEarningCode_Sorting"
                            OnRowCommand="gvEarningCode_RowCommand"
                            PageSize="20"
                            Width="80%" runat="server"
                            PagerSettings-PageButtonCount="10">
                            <PagerStyle CssClass="pager" />
                            <RowStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                No Earning Codes defined.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Earning Code" SortExpression="EarningCode">
                                    <ItemTemplate>
                                        <%#Eval("Code")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <%#Eval("CodeDescription")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server"
                                            CommandName="DoEdit"
                                            CommandArgument='<%#Eval("EarningCodeID") %>'
                                            ToolTip="Edit"
                                            ImageUrl="~/Images/edit.png"
                                            Visible='<%# Master.hasEditPermission %>' />
                                        &nbsp;
				
                               
				                <asp:ImageButton ID="btnDelete" runat="server"
                                    CommandName="DoDelete"
                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this code?');"
                                    CommandArgument='<%#Eval("EarningCodeID") %>'
                                    ToolTip="Delete"
                                    ImageUrl="~/Images/delete_icon.png"
                                    Visible='<%# Master.hasDeletePermission %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                    <asp:Panel ID="pnlEdit" runat="server" Visible="false" DefaultButton="btnSave">
                        <div class="boxContainer" style="width: 50%; margin: auto;">
                            <div class="section-title">
                                Earning Code Details
                            </div>
                            <table class="editForm" style="border-collapse: separate; border-spacing: 7px; padding: 2px; width: 100%;" border="0">
                                <tr>
                                    <td class="right">Earning Code</td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <ig:WebTextEditor ID="txtEarningCode" runat="server" Width="250px" MaxLength="50"></ig:WebTextEditor>
                                        <div>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtEarningCode"
                                                ErrorMessage="Please enter earning code." ValidationGroup="earningcode" Display="Dynamic"
                                                CssClass="validation1" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Earning Code Description</td>
                                    <td class="redstar"></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtEarningDescription" runat="server" Width="250px" MaxLength="100"></ig:WebTextEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" CausesValidation="true" ValidationGroup="earningcode" OnClick="btnSave_Click" />
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
