<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLeadPolicyContact.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucLeadPolicyContact" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Panel ID="pnlContactEdit" runat="server" Visible="false">
    <div class="message_area">
        <asp:Label ID="lblContactMessage" runat="server" Visible="false" />
    </div>
    <table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 0px; text-align: left;" border="0" class="editForm">
        <tr>
            <td style="width: 15%" class="right">Contact Name
            </td>
            <td class="redstar" style="width:5px;">*</td>
            <td>
                <ig:WebTextEditor ID="txtContactName" runat="server" Width="200px" MaxLength="100" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ControlToValidate="txtContactName"
                         SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please enter contact name." ValidationGroup="Contact"
                         CssClass="validation1" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="right">Contact Type
            </td>
            <td></td>
            <td>
                <asp:DropDownList ID="ddlContactType" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="right">Email
            </td>
            <td></td>
            <td>
                <ig:WebTextEditor ID="txtContactEmail" runat="server" Width="200px" MaxLength="100" />
                <div>
                    <asp:RegularExpressionValidator ID="regContactEmail" runat="server" ControlToValidate="txtContactEmail"
                        ErrorMessage="Email is not valid." ValidationGroup="Contact" Display="Dynamic"
                        CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        SetFocusOnError="True" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="right">Mobile Phone
            </td>
            <td></td>
            <td>
                <ig:WebTextEditor ID="txtContactPhone" runat="server" Width="200px" MaxLength="20" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>
                <asp:Button ID="btnSaveContact" runat="server" Text="Save" class="mysubmit" CausesValidation="true" ValidationGroup="Contact"
                    OnClick="btnSaveContact_Click" />
                &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="mysubmit" CausesValidation="false"
                    OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlContacGrid" runat="server">
    <div>
        <asp:LinkButton ID="lbtnNewContact" runat="server" Text="New Contact" OnClick="lbtnNewContact_Click" CssClass="link" />
    </div>
    <asp:GridView ID="gvContacts" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="gridView"
        HorizontalAlign="Center" CellPadding="2" OnRowCommand="gvContacts_RowCommand">
        <RowStyle HorizontalAlign="Center" />
        <Columns>
            <asp:TemplateField HeaderText="Contact Type">
                <ItemTemplate>
                    <%#Eval("LeadContactType.Description") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Contact Name">
                <ItemTemplate>
                    <%#Eval("ContactName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mobile Phone">
                <ItemTemplate>
                    <%#Eval("Mobile") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate>
                    <%#Eval("Email") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" ImageAlign="Middle" ImageUrl="~/Images/edit.png"
                        ToolTip="Edit Contact" CommandName="DoEdit" CommandArgument='<%#Eval("ID") %>' />
                    &nbsp;
								<asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Middle" ImageUrl="~/Images/delete_icon.png"
                                    ToolTip="Remove Contact" CommandName="DoRemove" CommandArgument='<%#Eval("ID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>
