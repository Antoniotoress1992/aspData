<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSubStatus.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucSubStatus" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>


<div class="paneContent">
    <div class="page-title">
        Lead/Claim Sub-Status 
    </div>
    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnReturnToClaim" runat="server" Text="Return to Claim" CssClass="toolbar-item" OnClick="btnReturnToClaim_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Client</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="paneContentInner">
        <div class="message_area">
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
            <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
        </div>
        <div style="text-align: center; margin-bottom: 10px;">
            <asp:Panel ID="pnlSubstatus" runat="server" DefaultButton="btnSave">
                Sub Status&nbsp;
			<ig:WebTextEditor ID="txtSubStatus" MaxLength="100" runat="server" Width="300px" />
                &nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
                    ValidationGroup="substatus" />
                &nbsp;<asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                    CssClass="mysubmit" OnClick="btnCancel_Click" />

                <div>
                    <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtSubStatus"
                        ErrorMessage="Please Enter Sub Status" ValidationGroup="substatus" Display="Dynamic"
                        CssClass="validation1" />
                </div>
            </asp:Panel>
        </div>
        <asp:GridView ID="gvSubStatus" CssClass="gridView" OnRowCommand="gv_RowCommand" Width="80%" HorizontalAlign="Center"
            runat="server" AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
            AllowPaging="true" PageSize="20" OnPageIndexChanging="gv_PageIndexChanging"
            PagerSettings-PageButtonCount="5">
            <PagerStyle CssClass="pager" />
            <RowStyle HorizontalAlign="Center" />
            <HeaderStyle HorizontalAlign="Center" />
            <EmptyDataTemplate>
                No sub-status available.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="No.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub-Status Name">
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("SubStatusName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgEdit" CommandName="DoEdit" CommandArgument='<%#Eval("SubStatusId") %>'
                            ToolTip="Edit" ImageUrl="../../Images/edit_icon.png"
                            Enabled='<%# Convert.ToBoolean(Convert.ToInt32(this.hfEditPermission.Value) == 0) ? false : true %>' />
                        &nbsp;
						<asp:ImageButton runat="server" ID="imgDelete" title="Are you sure you want to delete this record?"
                            CommandName="DoDelete" CommandArgument='<%#Eval("SubStatusId") %>' ToolTip="Delete"
                            ImageUrl="../../Images/delete_icon.png"
                            Enabled='<%# (this.hfDeletePermission.Value == "0" || Convert.ToString(Eval("Status"))=="False") ? false : true %>'
                            OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this status?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
        <asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
        <asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
        <asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
        <asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
        <asp:HiddenField ID="hfStatus" runat="server" Value="0" />
        <asp:HiddenField ID="hdId" runat="server" Value="0" />
    </div>
</div>
