<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUmpire.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucUmpire" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>

<div class="page-title">
    Umpires
</div>

<div class="toolbar toolbar-body">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnNew" runat="server" CssClass="toolbar-item" OnClick="btnNew_Click" Visible='<%# Convert.ToBoolean(masterPage.hasAddPermission) %>'>
							<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Umpire</span>
                </asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="btnReturnToClaim" runat="server" CssClass="toolbar-item" OnClick="btnReturnToClaim_Click">
							<span class="toolbar-img-and-text" style="background-image: url(../../images/back.png)">Return to Client</span>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>
<div class="paneContent">

    <div class="paneContentInner">
        <div class="message_area">
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
            <asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
            <asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
        </div>
        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="700px" TitleAlignment="Left" Text="Umpire Details" CssClass="canvas">
                <Template>
                    <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">Umpire</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtName" MaxLength="100" runat="server" Width="250px" />
                                <asp:RequiredFieldValidator runat="server" ID="reqddlPrimaryProducer" ControlToValidate="txtName"
                                    ErrorMessage="Please enter umpire name" ValidationGroup="contractor" Display="Dynamic"
                                    CssClass="validation1" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Company Name</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtBusinessName" MaxLength="100" runat="server" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Address</td>
                            <td class="redstar"></td>
                            <td><ig:WebTextEditor ID="txtAddress" runat="server" MaxLength="100" Width="250px" />
                                
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress2" runat="server" MaxLength="50" Width="250px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">State</td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlState" CssClass="DDLStyles" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                </asp:DropDownList>
                               
                            </td>
                        </tr>
                        <tr>
                            <td class="right">City</td>
                            <td class="redstar"></td>

                            <td>
                                <asp:DropDownList ID="ddlCity" CssClass="DDLStyles" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="dllCity_SelectedIndexChanged">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                              
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Zip Code</td>
                            <td class="redstar"></td>

                            <td>
                                <asp:DropDownList ID="ddlLossZip" CssClass="DDLStyles" runat="server">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                              
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Federal ID No.</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtFedID" MaxLength="20" runat="server" Width="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Email</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtEmail" MaxLength="100" runat="server" Width="250px" TextMode="Email" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Phone</td>
                            <td></td>
                            <td>
                                <ig:WebTextEditor ID="txtPhone" MaxLength="50" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center;">
                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="mysubmit" OnClick="btnSave_Click"
                                    ValidationGroup="contractor" />
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

            <asp:GridView ID="gvUmpire" CssClass="gridView" ShowFooter="false" Width="99%" runat="server" HorizontalAlign="Center"
                OnRowCommand="gvUmpire_RowCommand" AutoGenerateColumns="False" CellPadding="4"
                AlternatingRowStyle-BackColor="#e8f2ff" PageSize="10"
                PagerSettings-PageButtonCount="5" PagerStyle-Font-Bold="true"
                AllowSorting="true" OnSorting="gv_onSorting"
                PagerStyle-Font-Size="9pt">
                <PagerStyle CssClass="pager" />
                <RowStyle HorizontalAlign="Center" />
                <EmptyDataTemplate>
                    No contractors available.
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Umpire Name" ItemStyle-HorizontalAlign="Center" SortExpression="UmpireName">
                        <ItemTemplate>
                            <%# Eval("UmpireName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" ItemStyle-HorizontalAlign="Center" SortExpression="BusinessName">
                        <ItemTemplate>
                            <%# Eval("BusinessName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone" ItemStyle-HorizontalAlign="Center" SortExpression="Phone">
                        <ItemTemplate>
                            <%# Eval("Phone") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" SortExpression="Email"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("Email")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" SortExpression="Status">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("Status")) ? "Active" : "Inactive" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("UmpireId") %>'
                                ToolTip="Edit" ImageUrl="~/Images/edit.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasEditPermission) %>'></asp:ImageButton>
                            &nbsp;
							<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this umpire?');"
                                CommandArgument='<%#Eval("UmpireId") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png"
                                Visible='<%# Convert.ToBoolean(masterPage.hasDeletePermission) %>'></asp:ImageButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
</div>

<asp:HiddenField ID="hdId" runat="server" Value="0" />

