﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="BusinessRules.aspx.cs" Inherits="CRM.Web.Protected.Admin.BusinessRules" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<%@ Register Src="~/UserControl/Rules/ucRule1.ascx" TagName="ucRule1" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Rules/ucRule2.ascx" TagName="ucRule2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Rules/ucRule8.ascx" TagName="ucRule8" TagPrefix="uc8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="page-title">
        Business Rules
    </div>
    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnBackToList" runat="server" Text="" CssClass="toolbar-item" OnClick="btnBackToList_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/list.png)">Rules</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="paneContentInner">
        <asp:UpdatePanel ID="udpatePanel" runat="server">
            <ContentTemplate>
                <div class="message_area">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
                <div class="editForm no_min_width">

                    <asp:Panel ID="pnlGrid" runat="server">
                        <div class="paneContentInner">
                            <asp:LinkButton ID="lbtnNewRule" runat="server" Text="New Rule" CssClass="link" OnClick="lbtnNewRule_Click" />
                        </div>
                        <asp:GridView ID="gvBsuinessRules" CssClass="gridView" runat="server"
                            AlternatingRowStyle-BackColor="#e8f2ff"
                            AutoGenerateColumns="False"
                            AllowSorting="true"
                            AllowPaging="true"
                            CellPadding="4"
                            DataKeyNames="BusinessRuleID"
                            HorizontalAlign="Left"
                            OnRowCommand="gvBsuinessRules_RowCommand"
                            OnRowDataBound="gvBsuinessRules_RowDataBound"
                            OnPageIndexChanging="gvBsuinessRules_PageIndexChanging"
                            OnSorting="gvBsuinessRules_Sorting"
                            PageSize="20"
                            RowStyle-HorizontalAlign="Center"
                            PagerSettings-PageButtonCount="10"
                            ShowHeaderWhenEmpty="false"
                            Width="100%">
                            <PagerStyle CssClass="pager" />
                            <EmptyDataTemplate>
                                No rules available.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                                    ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server"
                                            CommandName="DoEdit"
                                            CommandArgument='<%#Eval("BusinessRuleID") %>'
                                            ToolTip="Edit"
                                            ImageUrl="~/Images/edit.png"
                                            Visible='<%# Convert.ToBoolean(Master.hasEditPermission) %>' />
                                        &nbsp;
				                                <asp:ImageButton ID="btnDelete" runat="server"
                                                    CommandName="DoDelete"
                                                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to deactivate this rule?');"
                                                    CommandArgument='<%#Eval("BusinessRuleID") %>'
                                                    ToolTip="Delete"
                                                    ImageUrl="~/Images/delete_icon.png"
                                                    Visible='<%# Convert.ToBoolean(Master.hasDeletePermission) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rule Type"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("Rule.RuleName") %>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Client"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("Client.FirstName") %> <%# Eval("Client.LastName") %>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("Description") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Updated"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# Eval("UpdateDate","{0:MM/dd/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="45px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "Yes" : "No"%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                    <asp:Panel ID="pnlNew" runat="server" Visible="false">
                        <table style="width: 100%; margin: auto;">
                            <tr>
                                <td class="right">Rule Type &nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlRules" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlRules_SelectedIndexChanged" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>



                    <asp:Panel ID="pnlRule1" runat="server" Visible="false" CssClass="rulePanel">
                        <div class="boxContainer">
                            <div class="section-title">
                                <asp:Label ID="lblRuleTitle" runat="server" />
                            </div>
                            <div class="paneContentInner">
                                <uc1:ucRule1 ID="ucRule1" runat="server" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlRule2" runat="server" Visible="false" CssClass="rulePanel">

                        <uc2:ucRule2 ID="ucRule2" runat="server" />

                    </asp:Panel>
                    <asp:Panel ID="pnlRule8" runat="server" Visible="false" CssClass="rulePanel">
                        <uc8:ucRule8 ID="ucRule8" runat="server" />
                    </asp:Panel>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
