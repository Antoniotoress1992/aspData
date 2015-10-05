<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProducer.ascx.cs"
    Inherits="CRM.Web.UserControl.Admin.ucProducer" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div class="page-title">
   <%-- Primary Producers--%>
    Activities
</div>

<div class="paneContent">
    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnNew" runat="server" CssClass="toolbar-item"
                        Visible='<%# masterPage.hasAddPermission %>'
                        OnClick="btnNew_Click">
							<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New</span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="btnReturnToClaim" runat="server" CssClass="toolbar-item"
                        OnClick="btnReturnToClaim_Click">
							<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Client</span>
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="message_area">
        <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
        <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
        <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
    </div>
    <div class="paneContentInner">
        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <table style="border-collapse: separate; border-spacing: 5px; padding: 2px; margin:auto;" border="0" class="editForm">
                <tr>
                    <td class="right">Activity Name</td>
                    <%--<td class="right">Producer Name</td>--%>
                    <td class="redstar"></td>
                    <td>
                        <ig:WebTextEditor ID="txtProducer" Visible="false" MaxLength="250" runat="server" />
                        <ig:WebTextEditor ID="txtActivity"  MaxLength="250" runat="server" />
                        <div>
                            <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtActivity"
                                ErrorMessage="Please Enter Activity Name" ValidationGroup="producer" Display="Dynamic"
                                CssClass="validation1" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="center">
                        <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click" CausesValidation="true"
                            ValidationGroup="producer" />
                        &nbsp;
					    <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                            CssClass="mysubmit" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:GridView ID="gvProducer" CssClass="gridView" runat="server" HorizontalAlign="Center" DataKeyNames="ActivityID"
            AutoGenerateColumns="False"
            AllowPaging="false"
            AllowSorting="true"
            AlternatingRowStyle-BackColor="#e8f2ff"
            CellPadding="4"
            OnRowCommand="gv_RowCommand"
            OnPageIndexChanging="gvProducer_PageIndexChanging"
         
            PagerSettings-PageButtonCount="10"
            OnSorting="gv_onSorting">
            <PagerStyle CssClass="pager" />
            <RowStyle HorizontalAlign="Center" />
            <EmptyDataRowStyle BorderStyle="None" />
            <EmptyDataTemplate>
                No producers available.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Activity" SortExpression="Activity"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblProducer" runat="server" Text='<%# Eval("Activity1") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Width="100px" />
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="imgEdit"
                            CommandName="DoEdit"
                            CommandArgument='<%#Eval("ActivityID") %>'
                            ToolTip="Edit"
                            ImageUrl="~/Images/edit_icon.png"
                            Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' />
                        &nbsp;
									<asp:ImageButton runat="server" ID="imgDelete"
                                        title="Are you sure you want to delete this record?"
                                        CommandName="DoDelete"
                                        CommandArgument='<%#Eval("ActivityID") %>'
                                        ToolTip="Delete"
                                        ImageUrl="~/Images/delete_icon.png"
                                        OnClientClick="javascript:return ConfirmDialog(this,  'Do you want to delete this activity ?');"
                                        Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>' />

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>



    </div>

</div>

<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />
