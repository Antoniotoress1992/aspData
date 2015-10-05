<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ClientRolesManager.aspx.cs" Inherits="CRM.Web.Protected.Admin.ClientRolesManager" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>


<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="paneContent">
        <div class="page-title">
            Client Roles
        </div>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>

                <div class="toolbar toolbar-body">
                    <table>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btnNewRole" runat="server" CssClass="toolbar-item" OnClick="btnNewRole_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Role</span>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="paneContentInner">
                    <asp:Panel ID="pnlGridPanel" runat="server">

                        <asp:GridView ID="gvClientRoles" Width="80%" runat="server" OnRowCommand="gvClientRoles_RowCommand" CssClass="gridView" HorizontalAlign="Center"
                            AutoGenerateColumns="False"
                            AlternatingRowStyle-BackColor="#e8f2ff"
                            CellPadding="4">
                            <RowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                No roles available.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit"
                                            CommandArgument='<%#Eval("RoleId") %>'
                                            ToolTip="Edit" ImageUrl="~/Images/edit.png"></asp:ImageButton>
                                        &nbsp;
							<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete" Visible='<%# Eval("ClientID") != null %>'
                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this role?');"
                                CommandArgument='<%#Eval("RoleId") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Name">
                                    <ItemTemplate>
                                        <%#Eval("RoleName") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Description">
                                    <ItemTemplate>
                                        <%#Eval("RoleDescription") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="pnlEditPanel" runat="server" Visible="false" DefaultButton="btnSave">
                        <table style="width: 100%;">
                            <tr>
                                <td class="top left" style="width: 30%;">
                                    <table style="border-collapse: separate; border-spacing: 3px; padding: 2px; width: 100%;" border="0" class="editForm">
                                        <tr>
                                            <td class="right">Role Name</td>
                                            <td class="redstar">*</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtRoleName" runat="server"></ig:WebTextEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="right">Role Description</td>
                                            <td class="redstar">*</td>
                                            <td>
                                                <ig:WebTextEditor ID="txtRoleDescription" runat="server"></ig:WebTextEditor>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" class="center">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" CausesValidation="true" ValidationGroup="Role" OnClick="btnSave_Click" />
                                                &nbsp;
										        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="mysubmit" CausesValidation="false" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="top left">
                                    <ajaxToolkit:TabContainer ID="tabContainer" runat="server" Width="100%" ActiveTabIndex="0">
                                        <ajaxToolkit:TabPanel ID="tabPanelDeploymentAddress" runat="server">
                                            <HeaderTemplate>Access Permissions</HeaderTemplate>
                                            <ContentTemplate>
                                                <div class="paneContentInner">
                                                    <asp:LinkButton ID="lbtnSelectAll" runat="server" CssClass="link" Text="Select All" OnClick="lbtnSelectAll_Click" />
                                                </div>
                                                <div style="height: 600px; overflow: auto;">
                                                    <asp:GridView ID="gvModules" runat="server"
                                                        AutoGenerateColumns="False"
                                                        AlternatingRowStyle-BackColor="#e8f2ff"
                                                        CellPadding="4"
                                                        CssClass="gridView"
                                                        DataKeyNames="ModuleID"
                                                        OnRowDataBound="gvModules_RowDataBound"
                                                        Width="100%">
                                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Module Name">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbxModule" runat="server" Text='<%#Eval("ModuleName") %>' TextAlign="Right" Checked='<%# Bind("ViewPermssion") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Granted Permissions">
                                                                <ItemTemplate>
                                                                    <asp:GridView ID="gvModulePermission" runat="server"
                                                                        AutoGenerateColumns="False"
                                                                        CellPadding="2"
                                                                        CssClass="gridView"
                                                                        DataKeyNames="ModuleID"
                                                                        Width="100%">
                                                                        <RowStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Module Name">
                                                                                <ItemStyle Width="300px" />
                                                                                <ItemTemplate>
                                                                                    <%#Eval("ModuleName") %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="View">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="cbxViewPermission" runat="server" Checked='<%# Bind("ViewPermssion") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Add">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="cbxAddPermission" runat="server" Checked='<%# Bind("AddPermssion") %>' Visible='<%# Eval("HasNew") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Edit">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="cbxEditPermission" runat="server" Checked='<%# Bind("EditPermission") %>' Visible='<%# Eval("HasEdit") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="cbxDeletePermission" runat="server" Checked='<%# Bind("DeletePermission") %>' Visible='<%# Eval("HasDelete") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel ID="tabPanel1" runat="server">
                                            <HeaderTemplate>Actions</HeaderTemplate>
                                            <ContentTemplate>
                                                <div class="editForm">
                                                    <asp:CheckBoxList ID="cblRoleActions" runat="server" Width="400px"></asp:CheckBoxList>
                                                </div>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>

                                    </ajaxToolkit:TabContainer>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
