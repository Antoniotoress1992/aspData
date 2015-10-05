<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimContacts.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimContacts" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<asp:Panel ID="pnlContactEdit" runat="server" Visible="false">
    <div class="message_area">
        <asp:Label ID="lblMessage" runat="server" />
    </div>
    <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm">
        <tr>
            <td class="right">Contact Type
            </td>
            <td></td>
            <td>
                <asp:DropDownList ID="ddlContactType" runat="server" />
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
            <td class="redstar"></td>
            <td>
                <ig:WebTextEditor ID="txtContactEmail" MaxLength="100" runat="server"></ig:WebTextEditor>
                <div>
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
				<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="mysubmit" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnlContactGrid" runat="server">
    <div style="margin: 5px;">
        <asp:LinkButton ID="lbtnNewContact" runat="server" Text="New Contact"
            OnClick="lbtnNewContact_Click" CssClass="link" />

        <asp:LinkButton ID="lbtnImportContact" runat="server" Text="Import Contact" OnClick="lbtnImportContact_Click" CssClass="link" />
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
            <asp:TemplateField>
                <ItemStyle Width="45px" />
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server"
                        ImageAlign="Middle"
                        ImageUrl="~/Images/edit.png"
                        ToolTip="Edit Contact"
                        CommandName="DoEdit"
                        Visible='<%# CRM.Core.PermissionHelper.checkEditPermission("UsersLeads.aspx") %>'
                        CommandArgument='<%#Eval("Contact.ContactID") %>' />
                    &nbsp;
				<asp:ImageButton ID="btnDelete" runat="server"
                    ImageAlign="Middle"
                    ImageUrl="~/Images/delete_icon.png"
                    ToolTip="Remove Contact"
                    CommandName="DoRemove"
                    CommandArgument='<%#Eval("ClaimContactID") %>'
                    Visible='<%# CRM.Core.PermissionHelper.checkDeletePermission("UsersLeads.aspx") %>'
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this contact?');" />
                </ItemTemplate>
            </asp:TemplateField>
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
            <asp:TemplateField HeaderText="Type">
                <ItemTemplate>
                    <%# Eval("Contact.LeadContactType") == null ? null : Eval("Contact.LeadContactType.Description") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Company Name">
                <ItemTemplate>
                    <%#Eval("Contact.CompanyName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Department Name">
                <ItemTemplate>
                    <%#Eval("Contact.DepartmentName") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Title">
                <ItemTemplate>
                    <%#Eval("Contact.ContactTitle") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <%#Eval("Contact.Phone") %>
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

        </Columns>
    </asp:GridView>
</asp:Panel>

<asp:Panel ID="pnlImportContact" runat="server" Visible="false">
    <div class="message_area">
        <asp:Label ID="lblImportContactMessage" runat="server" />
    </div>
    <div style="margin-top: 10px;">
        <div class="section-title">Import Contact</div>
        <ig:WebDataGrid ID="wdgContacts" runat="server" Height="350px" Width="100%"
            AutoGenerateColumns="false"
            CssClass="gridView"
            OnItemCommand="wdgContacts_ItemCommand">
            <Columns>
                <ig:TemplateDataField Key="command" Header-Text="" Width="45px">
                    <ItemTemplate>
                        <asp:LinkButton ID="ibtnImportContactFromGrid" runat="server" Text="Import" CommandName="DoImport" CommandArgument='<%#Eval("ContactID") %>' CssClass="link" />
                    </ItemTemplate>
                </ig:TemplateDataField>
                <ig:BoundDataField DataFieldName="FirstName" Key="FirstName" Header-Text="First Name" />
                <ig:BoundDataField DataFieldName="LastName" Key="LastName" Header-Text="Last Name" />
                <ig:BoundDataField DataFieldName="CompanyName" Key="CompanyName" Header-Text="Company Name" />
                <ig:BoundDataField DataFieldName="DepartmentName" Key="DepartmentName" Header-Text="Department" />
                <ig:BoundDataField DataFieldName="ContactTitle" Key="ContactTitle" Header-Text="Title" />
                <ig:TemplateDataField Key="LeadContactType" Header-Text="Type">
                    <ItemTemplate>
                        <%# Eval("LeadContactType.Description") %>
                    </ItemTemplate>
                </ig:TemplateDataField>
                <ig:BoundDataField DataFieldName="Email" Key="Email" Header-Text="Email" />
                <ig:BoundDataField DataFieldName="Phone" Key="Phone" Header-Text="Phone" />
                <ig:BoundDataField DataFieldName="Mobile" Key="Mobile" Header-Text="Mobile" />
                <ig:BoundDataField DataFieldName="Fax" Key="Fax" Header-Text="Fax" />
            </Columns>
            <Behaviors>
                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="20"
                    ThresholdFactor="0.5" Enabled="true" />
                <ig:Sorting Enabled="true" SortingMode="Single" />
            </Behaviors>
        </ig:WebDataGrid>
        <asp:EntityDataSource ID="edsContacts" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
            EnableFlattening="False" EntitySetName="Contacts" Include="LeadContactType"
            Where="it.ClientId = @ClientID && it.IsActive = true"
            OrderBy="it.FirstName, it.LastName Asc">
            <WhereParameters>
                <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
        <div class="center">
            <br />
            <asp:Button ID="btnCloseImportContact" runat="server" Text="Close" OnClick="btnCloseImportContact_Click" CssClass="mysubmit" />
        </div>
    </div>

</asp:Panel>

<script type="text/javascript">
    function wdgContacts_RowSelectionChanged(sender, args) {
        var selectedRows = args.getSelectedRows();

        var contactID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
    }

    function findGlobalContactDialog() {
        // show upload dialog
        $("#div_globalContacts").dialog({
            modal: false,
            width: 800,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
			{
			    'Import': function () {

			        $(this).dialog('close');
			    },
			    'Close': function () {
			        $(this).dialog('close');
			    }
			}
        });
    }

</script>
