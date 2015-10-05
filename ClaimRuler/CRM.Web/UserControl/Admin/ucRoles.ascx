<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRoles.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucRoles" %>

<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
<asp:UpdatePanel runat="server" ID="updatePanel1">
    <ContentTemplate>
        <div class="toolbar toolbar-body">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lbnNew" runat="server" Text="" CssClass="toolbar-item" OnClick="lbnNew_Click">
					        <span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New</span>
                        </asp:LinkButton>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <div class="paneContentInner">
            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                <asp:HiddenField runat="server" ID="hdId" />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                            <tr>
                                <td style="width: 200px; text-align: right;">
                                    <label>
                                        Role
                                    </label>
                                </td>
                                <td class="redstar">*
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRoleName" class="login_st" MaxLength="50" /><br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*Please Enter Role Name"
                                        Display="Dynamic" EnableClientScript="true" ValidationGroup="role" ControlToValidate="txtRoleName"
                                        CssClass="validation1" SetFocusOnError="true" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <label>
                                        Role Description &nbsp;</label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRoleDescription" class="login_st" MaxLength="100" /><br />
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px; text-align: right;">
                                    <label>
                                        Status
                                    </label>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlStatus" CssClass="DDLStyles" Style="float: left;">
                                        <asp:ListItem Text="Active" Value="1" />
                                        <asp:ListItem Text="In-Active" Value="0" />
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Button ID="btnSave" runat="server" CssClass="mysubmit"
                                        OnClick="btnSave_Click" Text="Save" ValidationGroup="role" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false"
                                        CssClass="mysubmit" OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>

            <asp:Panel ID="pnlList" runat="server" Style="display: inline-table; table-layout: fixed; width: 100%;">
                <div class="message_area">
                    <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
                    <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
                </div>

                <div class="vendor_list" style="display: table-row">
                    <asp:GridView runat="server" ID="gvData" Width="100%" DataKeyNames="RoleId" AutoGenerateColumns="false"
                        CssClass="gridView" OnRowCommand="gvData_RowCommand" AlternatingRowStyle-BackColor="#e8f2ff" HorizontalAlign="Center"
                        AllowPaging="true" PageSize="15" PagerSettings-PageButtonCount="5" CellPadding="4"
                        OnPageIndexChanging="gvData_PageIndexChanging" OnRowDataBound="gvData_RowDataBound">
                        <PagerStyle CssClass="pager" />
                        <EmptyDataTemplate>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField HeaderText="Role Name" DataField="RoleName"
                                ItemStyle-Width="25%" />
                            <asp:BoundField HeaderText="Role Description" DataField="RoleDescription"
                                ItemStyle-Width="30%" />
                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Literal ID="litStatus" Text='<%#Convert.ToString(Eval("Status"))=="True"?"Active":"In-Active" %>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("RoleId") %>'
                                        ImageUrl="~/Images/edit_icon.png" ToolTip="Edit"
                                        Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' />&nbsp;
                                         
                                        <asp:ImageButton runat="server" ID="imgDelete" CommandName="DoDelete" CommandArgument='<%#Eval("RoleId") %>'
                                            Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'
                                            ImageUrl='<%# Convert.ToString(Eval("RoleId")) == "1" ? "../../Images/delete_icon_gray.png" : "../../Images/delete_icon.png" %>'
                                            OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this role?');"
                                            ToolTip="Delete" />

                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
