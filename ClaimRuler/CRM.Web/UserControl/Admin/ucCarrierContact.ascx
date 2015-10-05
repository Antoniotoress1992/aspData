<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierContact.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierContact" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Panel ID="pnlContactEdit" runat="server" Visible="false">
    <div class="message_area">
        <asp:Label ID="lblMessage" runat="server" />
    </div>
    <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td class="top left">
                <div class="boxContainer">
                    <div class="section-title">
                        Contact Information
                    </div>
                    <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm">
                        <tr>
                            <td class="right">Contact Type
                            </td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddlContactType" runat="server" />
                                <div>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvtype" ControlToValidate="ddlContactType" InitialValue="0"
                                        ErrorMessage="Please select contact type." Display="Dynamic" CssClass="validation1" ValidationGroup="contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">First Name</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtFirstName" MaxLength="50" runat="server"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic"
                                        SetFocusOnError="true" ErrorMessage="Please enter first name." CssClass="validation1" ValidationGroup="Contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Last Name</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtLastName" MaxLength="50" runat="server"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLastName" Display="Dynamic"
                                        SetFocusOnError="true" ErrorMessage="Please enter last name." CssClass="validation1" ValidationGroup="Contact" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Address
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress1" runat="server" MaxLength="100" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right"></td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtAddress2" runat="server" MaxLength="100" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="right">State
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">City
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="dllCity_SelectedIndexChanged">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Zip Code
                            </td>
                            <td class="redstar"></td>
                            <td>
                                <asp:DropDownList ID="ddlLossZip" runat="server">
                                    <asp:ListItem Text="--- Select ---" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Company Name</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtCompanyName" MaxLength="50" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>

                        <tr>
                            <td class="right">Title</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtContactTile" MaxLength="50" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Department Name</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtDepartmentName" MaxLength="50" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Phone Number</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtContactPhone" MaxLength="20" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Mobile Phone</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtMobilePhone" MaxLength="20" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Fax</td>
                            <td class="redstar"></td>
                            <td>
                                <ig:WebTextEditor ID="txtContactFax" MaxLength="20" runat="server"></ig:WebTextEditor>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Email</td>
                            <td class="redstar">*</td>
                            <td>
                                <ig:WebTextEditor ID="txtContactEmail" MaxLength="100" runat="server"></ig:WebTextEditor>
                                <div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtContactEmail" Display="Dynamic"
                                        SetFocusOnError="true" ErrorMessage="Please enter email." CssClass="validation1" ValidationGroup="Contact" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContactEmail"
                                        ErrorMessage="Email address not valid." ValidationGroup="Contact" Display="Dynamic"
                                        CssClass="validation1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="right">Is Primary</td>
                            <td></td>
                            <td>
                                <asp:CheckBox ID="cbxPrimary" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Contact" CssClass="mysubmit" CausesValidation="true" />
                                &nbsp;
				                <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_Click" CssClass="mysubmit" CausesValidation="false" />
                                &nbsp;
                                <asp:Button ID="btnShowCreateAccount" runat="server" CssClass="mysubmit" OnClick="btnShowCreateAccount_Click"
                                    CausesValidation="true" ValidationGroup="Contact" Text="Create User Account" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td class="top left">
                <asp:Panel ID="pnlUserAccount" runat="server" Visible="false">
                    <div class="boxContainer">
                        <div class="section-title">
                            User Account Information
                        </div>
                        <table style="width: 100%;" border="0" class="editForm">
                            <tr>
                                <td class="right">User Name</td>
                                <td class="redstar">*</td>
                                <td>
                                    <ig:WebTextEditor ID="txtUserName" runat="server" MaxLength="50" Width="245px" Height="24px" />
                                    <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserName" Display="Dynamic"
                                            SetFocusOnError="true" ErrorMessage="Please enter user name." CssClass="validation1" ValidationGroup="Account" />
                                    </div>
                                </td>
                            </tr>
                             <tr>
                                <td class="right">Password Name</td>
                                <td class="redstar"></td>
                                <td>System generated.</td>
                            </tr>
                            <tr>
                                <td class="right">Role</td>
                                <td class="redstar">*</td>
                                <td>
                                    <asp:DropDownList ID="ddlClientRoles" runat="server" Width="250px" />
                                     <div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlClientRoles" Display="Dynamic"
                                            SetFocusOnError="true" ErrorMessage="Please select role." InitialValue="0" CssClass="validation1" ValidationGroup="Account" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="center">
                                    <asp:Button ID="btnCreateCarrierAccount" runat="server" CssClass="mysubmit" OnClick="btnCreateCarrierAccount_Click"
                                        CausesValidation="true" ValidationGroup="Account" Text="Create Account"  />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>

   
</asp:Panel>
<asp:Panel ID="pnlContactGrid" runat="server">
    <div style="margin-bottom: 5px;">
        <asp:LinkButton CssClass="link" ID="lbtnNewContact" runat="server" Text="New Contact" OnClick="lbtnNewContact_Click" />
    </div>
    <asp:GridView ID="gvContacts" runat="server" Width="100%"
        AutoGenerateColumns="False"
        CssClass="gridView"
        HorizontalAlign="Center"
        CellPadding="2"
        OnRowCommand="gvContacts_RowCommand">
        <RowStyle HorizontalAlign="Center" />
        <FooterStyle HorizontalAlign="Center" />
        <Columns>

            <asp:TemplateField HeaderText="First Name">
                <ItemTemplate>
                    <%#Eval("Contact.FirstName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Name">
                <ItemTemplate>
                    <%#Eval("Contact.LastName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Title">
                <ItemTemplate>
                    <%#Eval("Contact.ContactTitle") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Department Name">
                <ItemTemplate>
                    <%#Eval("Contact.DepartmentName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <%#Eval("Contact.Mobile") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mobile Phone">
                <ItemTemplate>
                    <%#Eval("Contact.Mobile") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate>
                    <%#Eval("Contact.Email") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" ImageAlign="Middle" ImageUrl="~/Images/edit.png"
                        ToolTip="Edit Contact" CommandName="DoEdit" CommandArgument='<%#Eval("Contact.ContactID") %>' />
                    &nbsp;
				<asp:ImageButton ID="btnDelete" runat="server" ImageAlign="Middle" ImageUrl="~/Images/delete_icon.png"
                    ToolTip="Remove Contact" CommandName="DoRemove" CommandArgument='<%#Eval("CarrierContactID") %>'
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this contact?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>

