<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
    CodeBehind="StatusReminder.aspx.cs" Inherits="CRM.Web.Protected.Admin.MasterStatusReminder" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="page-title">
        Status Reminders
    </div>

    <div class="toolbar toolbar-body">
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="btnSend1" runat="server" CssClass="toolbar-item" OnClick="btnNew_Click">
					<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Status Reminder</span>
                    </asp:LinkButton>
                </td>

            </tr>
        </table>
    </div>
    <div class="paneContent">
        <div class="paneContentInner">
            <div class="messsage_area">
                <asp:Label ID="lblMessage" runat="server" Visible="false" Text="Session expired. Please re-login." />
            </div>
            <asp:Panel ID="pnlEdit" runat="server" Visible="false">

                <igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="700px" TitleAlignment="Left" Text="Status Reminder Details" CssClass="canvas">
                    <Template>
                        <table style="width: 100%; border-collapse: separate; border-spacing: 8px; padding: 2px; text-align: left;" border="0" class="editForm">
                            <tr>
                                <td class="right">Reminder Type</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlType" runat="server"
                                        AutoPostBack="false">
                                        <asp:ListItem Text="Select One" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Hour(s)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Day(s)" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ErrorMessage="Please enter description." ControlToValidate="ddlType"
                                        CssClass="validation1" ValidationGroup="reminder" InitialValue="0" />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Description of Reminder</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebTextEditor ID="txtDescription" runat="server" MaxLength="100" />
                                    <asp:RequiredFieldValidator ID="rftxtDescription" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ErrorMessage="Please enter description." ControlToValidate="txtDescription"
                                        CssClass="validation1" ValidationGroup="reminder" />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Remind Me In</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebTextEditor ID="txtDuration" runat="server" TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true"
                                        Display="Dynamic" ErrorMessage="Please enter duration." ControlToValidate="txtDuration"
                                        CssClass="validation1" ValidationGroup="reminder" />
                                </td>

                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="mysubmit" OnClick="btnSave_click" ValidationGroup="reminder" CausesValidation="true" />
                                    &nbsp;
									<asp:Button ID="btnCancel" runat="server" Text="Cancel" class="mysubmit" OnClick="btnCancel_click" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </Template>
                </igmisc:WebGroupBox>
            </asp:Panel>
            <asp:Panel ID="pnlList" runat="server">
                <asp:GridView ID="gvReminder" CssClass="gridView" Width="50%" HorizontalAlign="Center"
                    OnRowCommand="gvReminder_RowCommand" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
                    PageSize="10" PagerSettings-PageButtonCount="5" PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt">
                    <PagerStyle CssClass="pager" />
                    <RowStyle HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        No reminders available.
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="Description of Reminder" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                            ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ReminderID") %>'
                                    ToolTip="Edit" 
                                    Visible='<%# Master.hasEditPermission %>'
                                    ImageUrl="~/Images/edit.png"></asp:ImageButton>
                                &nbsp;
						<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                            OnClientClick="javascript:return confirm('Are you sure you want to delete this reminder?');"
                            CommandArgument='<%#Eval("ReminderID") %>'
                            ToolTip="Delete"
                            Visible='<%# Master.hasDeletePermission %>'
                            ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hf_reminderID" runat="server" Value="0" />
</asp:Content>
