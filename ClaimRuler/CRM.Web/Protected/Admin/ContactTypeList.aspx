<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="ContactTypeList.aspx.cs" Inherits="CRM.Web.Protected.Admin.ContactTypeList" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnNew" runat="server" CssClass="toolbar-item" OnClick="btnNew_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Contact Type</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pnlContactTypeGrid" runat="server">
        <asp:GridView ID="gvContactType" runat="server"
            AutoGenerateColumns="False"
            AlternatingRowStyle-BackColor="#e8f2ff"
            CellPadding="4"
            CssClass="gridView"
            HorizontalAlign="Center"
            OnRowCommand="gvContactType_RowCommand"
            Width="60%">
            <RowStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" />
            <EmptyDataTemplate>
                No contact types available.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Contact Types">
                    <ItemTemplate>
                        <%#Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ID") %>'
                            ToolTip="Edit"
                            Visible='<%# Master.hasEditPermission && Eval("ClientID") != null %>'
                            ImageUrl="~/Images/edit.png"></asp:ImageButton>
                        &nbsp;
				
				        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                            OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this contact type?');"
                            CommandArgument='<%#Eval("ID") %>'
                            ToolTip="Delete Carrier"
                            Visible='<%# Master.hasDeletePermission && Eval("ClientID") != null %>'
                            ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlContactTypeEdit" runat="server" Visible="false">
        <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
            <tr>
                <td class="right">Contact Type</td>
                <td class="redstar">*</td>
                <td>
                    <ig:WebTextEditor ID="txtContactType" runat="server" MaxLength="100"></ig:WebTextEditor>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSave_Click" ValidationGroup="ContactType" CausesValidation="true" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="mysubmit" OnClick="btnCancel_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>


</asp:Content>
