<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierDocument.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierDocument" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div style="margin-bottom: 5px;">
    <a class="link" href="javascript:showUploadDocumentDialog();">New Document</a>
</div>
<asp:Panel ID="pnlGrid" runat="server">
    <asp:GridView Width="99%" ID="gvDocuments" runat="server" AutoGenerateColumns="False"
        CellPadding="4" AllowPaging="true" CssClass="gridView" HorizontalAlign="Center"
        PageSize="20" PagerSettings-PageButtonCount="5"
        OnPageIndexChanging="gvDocuments_PageIndexChanging"
        OnRowCommand="gvDocuments_RowCommand"
        OnRowDataBound="gvDocuments_RowDataBound"
        HeaderStyle-BackColor="#e8f2ff">
        <PagerStyle CssClass="pager" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>                    
                    <asp:ImageButton runat="server" ID="btnDelete" Text="Delete" CommandName="DoDelete"
                        ImageUrl="~/Images/delete_icon.png" CommandArgument='<%# Eval("CarrierDocumentID") %>'
                        OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this document?')"
                        ToolTip="Delete" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="20px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <%# Eval("DocumentDate", "{0:g}") %>
                </ItemTemplate>
                <ItemStyle Width="125px" Wrap="False" VerticalAlign="Top" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Document Name">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:HyperLink ID="hlnkDocument" runat="server" Text='<%# Eval("DocumentName") %>' Target="_blank" CssClass="link" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Document Description">
                <ItemStyle HorizontalAlign="Left" />
                <ItemTemplate>
                    <asp:Label ID="lblDocumentDescription" runat="server" Text='<%#Eval("DocumentDescription") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
</asp:Panel>
<div id="documentUpload" style="display: none;" title="Document Upload">
    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td style="width: 10%;" class="top">Description</td>
            <td class="redstar top">*</td>
            <td>
                <textarea id="txtDocumentDescription" maxlength="500" rows="5" style="width: 100%;"></textarea>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="webUpload">
                </div>
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hf_carrierID" runat="server" Value="0" />

<asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Style="display: none;" CausesValidation="false" />

