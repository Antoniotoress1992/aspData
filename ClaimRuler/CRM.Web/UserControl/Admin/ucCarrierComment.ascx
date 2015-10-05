<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierComment.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierComment" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<div class="message_area">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
</div>
<div style="margin-bottom: 5px;">
    <asp:LinkButton ID="btnNewComment" runat="server" Text="New Comment" OnClick="btnNewComment_Click" CssClass="link" />
</div>
<asp:Panel ID="pnlGrid" runat="server">
    <asp:GridView Width="99%" ID="gvComments" runat="server" AutoGenerateColumns="False"
        CellPadding="4" AllowPaging="true" CssClass="gridView" HorizontalAlign="Center"
        PageSize="20" PagerSettings-PageButtonCount="5" 
        OnPageIndexChanging="gvComments_PageIndexChanging"
        OnRowCommand="gvComments_RowCommand" DataKeyNames="CommentId"      
        HeaderStyle-BackColor="#e8f2ff">
        <PagerStyle CssClass="pager" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="50px">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemTemplate>
                    <asp:ImageButton runat="server" ID="btnEdit" CommandName="DoEdit"
                        ImageUrl="~/Images/edit_icon.png" CommandArgument='<%# Eval("CommentId") %>' ToolTip="Edit" />
                    &nbsp;
				<asp:ImageButton runat="server" ID="btnDelete" Text="Delete" CommandName="DoDelete"
                    ImageUrl="~/Images/delete_icon.png" CommandArgument='<%# Eval("CommentId") %>'
                    OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this comment?')"
                    ToolTip="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="false" />
                <ItemTemplate>
                    <%# Eval("CommentDate", "{0:g}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="User Name" ItemStyle-Width="100px">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemTemplate>
                    <%# Eval("SecUser.UserName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Comments">
                <ItemTemplate>
                    <asp:Label ID="lblComments" runat="server"
                        Text='<%#Eval("CommentText") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server" Visible="false" DefaultButton="btnSave">
    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td>
                <ig:WebTextEditor ID="txtComment" runat="server" TextMode="MultiLine" MultiLine-Rows="20" Width="100%"></ig:WebTextEditor>
            </td>
        </tr>
        <tr>
            <td class="center">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="mysubmit" CausesValidation="true" ValidationGroup="comment" OnClick="btnSave_Click" />
                &nbsp;
				<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="mysubmit" CausesValidation="false" ValidationGroup="comment" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
