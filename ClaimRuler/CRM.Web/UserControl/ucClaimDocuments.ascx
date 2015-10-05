<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimDocuments.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimDocuments" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<style type="text/css">    .auto-style1 {        width: 500px;        height: 26px;    }    .auto-style2 {        height: 26px;    }</style>

<asp:Panel ID="pnlGridPanel" runat="server">
    <div style="margin: 3px;">
        <a class="link" href="javascript:showDocumentUploadDialog();">New Document</a>
        <asp:LinkButton CssClass="link" runat="server" ID="linkSearch" OnClick="linkSearch_Click">Import Outlook Mail</asp:LinkButton>    </div>    <asp:Panel ID="pnlSearch" runat="server" Visible="false">             <div class="boxContainer">        <div class="section-title">            Import Outlook Mail        </div>          <div class="center">              <br />        <table style="width:100%">                         <tr>                <td style="text-align:right" class="auto-style1">                    <div>Enter Claim Number or Insured Name :</div>                </td>                <td style="text-align:left" class="auto-style2">                    <%--<asp:TextBox ID="txtActivityTypes" runat="server" TabIndex="1"></asp:TextBox>--%>                     <asp:TextBox ID="txtKeyword" runat="server" Width="250px" />                </td>            </tr>                  <tr>                <td style="width:500px;text-align:right">                   <asp:Button ID="btnDocumentSearch" runat="server" Text="Search" OnClick="btnDocumentsSearch_Click" CssClass="mysubmit" ValidationGroup="comment" CausesValidation="true" />                </td>                <td style="text-align:left">                    <%--<asp:TextBox ID="txtActivityTypes" runat="server" TabIndex="1"></asp:TextBox>--%>                    <asp:Button ID="btnDocumentCancel" runat="server" Text="Cancel" OnClick="btnDocumentsCancel_Click" CssClass="mysubmit" CausesValidation="false" />                </td>            </tr>                                     </table>              <asp:Label ID="lblErrorMSg" runat="server" Text="Please enter valid Claim Number or Insured Name to Search" ForeColor="Red" Visible="False" AutoPostBack="true" />        <br /><br />
    </div>
                 <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">          <div class="boxContainer">        <div class="section-title">            Search Result        </div>        <asp:GridView Width="100%" ID="gvSearchResult" runat="server"                        CssClass="gridView"            HorizontalAlign="Center"            CellPadding="4">            <RowStyle HorizontalAlign="Center" />            <Columns>                                <asp:TemplateField HeaderText="Save">                    <ItemStyle Width="30px" HorizontalAlign="Center" />                    <ItemTemplate>                        <asp:CheckBox ID="cbxSave" runat="server" Checked="true" AutoPostBack="true"/>                    </ItemTemplate>                </asp:TemplateField>                            </Columns>        </asp:GridView>              <table style="width:100%">                   <tr>                <td style="width:500px;text-align:right">                   <asp:Button ID="btnDocumentsSave" runat="server" Text="Save" CssClass="mysubmit" ValidationGroup="comment" CausesValidation="true" OnClick="btnDocumentsSave_Click" />                </td>                <td style="text-align:left">                    <%--<asp:TextBox ID="txtActivityTypes" runat="server" TabIndex="1"></asp:TextBox>--%>                    <asp:Button ID="btnDocumentsClear" runat="server" Text="Clear" CssClass="mysubmit" CausesValidation="false" OnClick="btnDocumentsClear_Click" />                </td>            </tr>                 </table>    </div>                                </asp:Panel>    </div>        </asp:Panel>
    <div class="boxContainer">
        <div class="section-title">
            Current Documents
        </div>
        <asp:GridView Width="100%" ID="gvDocuments" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="ClaimDocumentID"
            CssClass="gridView"
            HorizontalAlign="Center"
            CellPadding="4"
            OnRowCommand="gvDocuments_RowCommand"
            OnRowDataBound="gvDocuments_RowDataBound">
            <RowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="btnEdit" Text="Edit"
                            CommandName="DoEdit"
                            ImageUrl="~/Images/edit_icon.png"
                            CommandArgument='<%# Eval("ClaimDocumentID") %>'
                            ToolTip="Edit"
                            Visible='<%# CRM.Core.PermissionHelper.checkEditPermission("UsersLeads.aspx") %>' />
                        &nbsp;
						<asp:ImageButton runat="server" ID="btnDelete" Text="Delete" CommandName="DoDelete"
                            ImageUrl="~/Images/delete_icon.png"
                            CommandArgument='<%# Eval("ClaimDocumentID") %>'
                            OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this document?')"
                            ToolTip="Delete"
                            Visible='<%# CRM.Core.PermissionHelper.checkEditPermission("UsersLeads.aspx") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Print">
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                    <ItemTemplate>

                        <asp:CheckBox ID="cbxPrint" runat="server" Checked='<%# Eval("IsPrint") == null ? true : Eval("IsPrint") %>' AutoPostBack="true" OnCheckedChanged="cbxPrint_CheckedChanged" />

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="30%" HeaderText="Document" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlnkDocument" runat="server" Target="_blank" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category">
                    <ItemTemplate>
                        <%#Eval("DocumentCategory.CategoryName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" ItemStyle-Width="40%">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Date" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%#Eval("DocumentDate", "{0:g}")%>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
    <div class="boxContainer">
        <div class="section-title">
            Previous Documents
        </div>
        <asp:GridView Width="100%" ID="gvHistoricalDocuments" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="LeadDocumentId"
            CssClass="gridView"
            HorizontalAlign="Center"
            CellPadding="4"
            OnRowDataBound="gvHistoricalDocuments_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-Width="30%" HeaderText="Document" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlnkDocument" runat="server" Target="_blank" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" ItemStyle-Width="50%">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfLeadId" runat="server" Value='<%#Eval("LeadId") %>' />
                        <%#Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%#Eval("InsertDate")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>
<asp:Panel ID="pnlEditClaimDocument" runat="server" Visible="false">
    <table style="width: 80%" class="editForm">
        <tr>
            <td class="right">Category</td>
            <td class="redstar"></td>
            <td>
                <asp:DropDownList ID="ddlDocumentCategoryEdit" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="tfvCategory" runat="server" ControlToValidate="ddlDocumentCategoryEdit"
                        Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select category." InitialValue="0"
                        ValidationGroup="document" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="right" style="width: 30%">Description</td>
            <td class="redstar"></td>
            <td>
                <ig:WebTextEditor ID="txtDcoumentDescription" runat="server" MaxLength="500" TextMode="MultiLine" MultiLine-Rows="5" Width="100%" />
                <div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="center">
                <asp:Button ID="btnSaveDocumentEdit" runat="server" Text="Save" CssClass="mysubmit" ValidationGroup="document" CausesValidation="true" OnClick="btnSaveDocumentEdit_Click" />
                &nbsp;
                <asp:Button ID="btnCanelDocumentEdit" runat="server" Text="Cancel" CssClass="mysubmit" CausesValidation="false" OnClick="btnCanelDocumentEdit_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<div id="documentUpload" style="display: none;" title="Document Upload">
    <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td class="right">Category</td>
            <td class="redstar"></td>
            <td>
                <asp:DropDownList ID="ddlDocumentCategory" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 10%;" class="top">Description</td>
            <td class="redstar top"></td>
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
<asp:HiddenField ID="hf_claimID" runat="server" Value="0" />
<asp:Button ID="btnHiddenRefreshDocument" runat="server" OnClick="btnRefresh_Click" Style="display: none;" CausesValidation="false" />
