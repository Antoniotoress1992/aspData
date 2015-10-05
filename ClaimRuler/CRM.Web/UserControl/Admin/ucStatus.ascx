<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucStatus.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucStatus" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Src="ucUploadCSV.ascx" TagName="ucUploadCSV" TagPrefix="uc1" %>


<div class="paneContent">
    <div class="page-title">
        Claim Status 
    </div>

    
    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnSend1" runat="server" Text="New Status" CssClass="toolbar-item" OnClick="btnNew_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Status</span>
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="btnReturnToClaim" runat="server" Text="Return to Claim" CssClass="toolbar-item" OnClick="btnReturnToClaim_Click">
						<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Return to Claim</span>
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

        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <igmisc:WebGroupBox ID="wgbEdit" runat="server" Width="700px" TitleAlignment="Left" Text="Status Details" CssClass="canvas">
                <Template>
                    <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">Status</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtStatus" MaxLength="100" runat="server" />
                                <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtStatus"
                                    ErrorMessage="Please Enter Status" ValidationGroup="status" Display="Dynamic"
                                    CssClass="validation1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Reminder</td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddlReminder" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Include in Count</td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="cbxIsCountable" runat="server" />

                            </td>
                        </tr>
                        <tr>
                            <td class="right">Count as Open</td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="cbxIsCountAsOpen" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center;">
                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
                                    ValidationGroup="status" />
                                &nbsp;
								<asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" runat="server"
                                    CssClass="mysubmit" OnClick="btnCancel_Click" />
                            </td>
                        </tr>

                    </table>
                </Template>
            </igmisc:WebGroupBox>
        </asp:Panel>

        <asp:Panel ID="pnlGrid" runat="server">
            <asp:GridView ID="gvStatus" CssClass="gridView" OnRowCommand="gv_RowCommand" Width="80%" HorizontalAlign="Center"
                runat="server" AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
                AllowPaging="true" PageSize="20" OnPageIndexChanging="gv_PageIndexChanging"
                PagerSettings-PageButtonCount="5"
                PagerStyle-Font-Size="9pt">
                <PagerStyle CssClass="pager" />
                <RowStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    No status available.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex+1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status Name">
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("StatusName")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reminder">
                        <ItemTemplate>
                            <asp:Label ID="lblReminder" runat="server" Text='<%#Eval("ReminderMaster.Description") == null ? "" : Eval("ReminderMaster.Description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Include in Count">
                        <ItemTemplate>
                            <asp:Label ID="lblIsCountable" runat="server" Text='<%#Eval("isCountable") == null ? "False" : Eval("isCountable").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Count as Open">
                        <ItemTemplate>
                            <asp:Label ID="lblCountAsOpen" runat="server" Text='<%#Eval("isCountAsOpen") == null ? "False" : Eval("isCountAsOpen").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgEdit"
                                CommandName="DoEdit"
                                CommandArgument='<%#Eval("StatusId") %>'
                                ToolTip="Edit" ImageUrl="~/Images/edit_icon.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>' />
                            &nbsp;
						    <asp:ImageButton runat="server" ID="imgDelete"
                                title="Are you sure you want to delete this record?"
                                CommandName="DoDelete"
                                CommandArgument='<%#Eval("StatusId") %>'
                                ToolTip="Delete"
                                ImageUrl="~/Images/delete_icon.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'
                                OnClientClick="javascript:return ConfirmDialog(this, 'Do you want to delete this status?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
</div>

<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
<asp:HiddenField ID="hfStatus" runat="server" Value="0" />
<asp:HiddenField ID="hdId" runat="server" Value="0" />
