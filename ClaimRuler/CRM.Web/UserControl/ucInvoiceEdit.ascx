<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInvoiceEdit.ascx.cs" Inherits="CRM.Web.UserControl.ucInvoiceEdit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>

        <div class="toolbar toolbar-body">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="toolbar-item" OnClick="btnSave_Click" CausesValidation="true">
					<span class="toolbar-img-and-text" style="background-image: url(../images/toolbar_save.png)">Save</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnEmail" runat="server" CssClass="toolbar-item" OnClientClick="javascript:return sendEmail();" Visible="false">
					<span class="toolbar-img-and-text" style="background-image: url(../images/mail_send.png)">Email</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnPrint" runat="server" CssClass="toolbar-item" OnClick="btnPrint_Click" Visible="false">
					<span class="toolbar-img-and-text" style="background-image: url(../images/print_icon.png)">Print</span>
                        </asp:LinkButton>
                    </td>

                </tr>
            </table>
        </div>
        <div class="message_area">
            <asp:Label ID="lblMessage" runat="server" />
        </div>

        <table style="width: 100%; border-collapse: separate; border-spacing: 2px; padding: 3px; margin: auto;" border="0" class="editForm no_min_width">
            <tr id="tr_email" style="display: none;">
                <td colspan="3"></td>
            </tr>
            <tr>
                <td class="left top" style="width: 33%;">
                    <div class="boxContainer" style="height: 200px;">
                        <div class="section-title">Invoice</div>
                        <div class="paneContentInner">
                            <table>
                                <tr>
                                    <td class="right nowrap">Invoice Date
                                    </td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <ig:WebDatePicker ID="txtInvoiceDate" runat="server" StyleSetName="Default" Width="100px"
                                            Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" CssClass="date_picker" TabIndex="1" />
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please enter date." ValidationGroup="invoice"
                                                CssClass="validation1" SetFocusOnError="true" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right nowrap">Due Date</td>
                                    <td></td>
                                    <td>
                                        <ig:WebDatePicker ID="txtDueDate" runat="server" StyleSetName="Default" CssClass="date_picker" Width="100px"
                                            Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" TabIndex="2" />
                                        <div class="float_right">
                                            <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ControlToValidate="txtDueDate"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please enter date." ValidationGroup="invoice"
                                                CssClass="validation1" SetFocusOnError="true" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right nowrap">Invoice Number</td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtInvoiceNumber" runat="server" Enabled="false" Width="100px" TabIndex="3" />
                                        &nbsp<span class="redstar nowrap">Auto-generated</span>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="right nowrap">Policy Type</td>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblPolicyType" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Policy Number</td>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblPolicyNumber" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right nowrap">Insurer Claim Number</td>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblInsurerClaimNumber" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right nowrap">Reference #</td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtReferenceNumber" runat="server" MaxLength="20" Width="100px" TabIndex="4" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td class="left top" style="width: 33%;">

                    <div class="boxContainer" style="height: 200px;">
                        <div class="section-title">Bill To</div>
                        <div class="paneContentInner">
                            <table>
                                <tr>
                                    <td class="right nowrap">Bill To</td>
                                    <td class="redstar">*</td>
                                    <td>
                                        <ig:WebDropDown runat="server" ID="ddlBillTo" Width="250px" TabIndex="10"
                                            EnableAnimations="false"
                                            EnableClosingDropDownOnBlur="true"
                                            EnableDropDownAsChild="false"
                                            EnableClosingDropDownOnSelect="true"
                                            DropDownContainerHeight="350px"
                                            DropDownContainerWidth="600px"
                                            EnableViewState="true"
                                            enabledataviewstate="true"
                                            ViewStateMode="Enabled">
                                            <Items>
                                                <ig:DropDownItem>
                                                </ig:DropDownItem>
                                            </Items>
                                            <ItemTemplate>
                                                <ig:WebDataGrid ID="WebDataGrid1" runat="server" CssClass="gridView smallheader" DataSourceID="odsBillTo"
                                                    AutoGenerateColumns="false" EnableViewState="true" EnableDataViewState="true"
                                                    Width="100%">
                                                    <Columns>
                                                        <ig:BoundDataField DataFieldName="billTo" Key="billTo" Header-Text="Bill-To" />
                                                        <ig:BoundDataField DataFieldName="billingName" Key="billingName" Header-Text="Name" />
                                                        <ig:BoundDataField DataFieldName="mailingAddress" Key="mailingAddress" Header-Text="Street Address" />
                                                        <ig:BoundDataField DataFieldName="mailingCity" Key="mailingCity" Header-Text="City Name" />
                                                        <ig:BoundDataField DataFieldName="mailingState" Key="mailingState" Header-Text="State Name" />
                                                        <ig:BoundDataField DataFieldName="mailingZip" Key="mailingZip" Header-Text="Zip Code" />
                                                    </Columns>
                                                    <Behaviors>
                                                        <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                                                            <SelectionClientEvents RowSelectionChanged="rowSelChange" />
                                                        </ig:Selection>
                                                    </Behaviors>
                                                </ig:WebDataGrid>
                                            </ItemTemplate>
                                            <ClientEvents SelectionChanging="cancelDDSel" />
                                        </ig:WebDropDown>
                                        <asp:ObjectDataSource ID="odsBillTo" runat="server" SelectMethod="getBillToCollection" DataObjectTypeName="BillToView" TypeName="CRM.Web.Protected.LeadInvoice">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="claimID" Type="Int32" SessionField="ClaimID" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBillTo"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please select Bill-To." ValidationGroup="invoice"
                                                CssClass="validation1" InitialValue="0" Visible="false">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Name</td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillTo" runat="server" Width="250px" TabIndex="11" />
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvBillTo" runat="server" ControlToValidate="txtBillTo"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please select Bill-To" ValidationGroup="invoice"
                                                CssClass="validation1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="right">Address</td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillToAddress1" runat="server" Width="250px" TabIndex="12" />
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvBillTo2" runat="server" ControlToValidate="txtBillToAddress1"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please enter address" ValidationGroup="invoice"
                                                CssClass="validation1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillToAddress2" runat="server" Width="250px" TabIndex="13" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillToAddress3" runat="server" Width="250px" TabIndex="14" />
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvtxtBillToAddress3" runat="server" ControlToValidate="txtBillToAddress3"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please enter city, state, zip code"
                                                ValidationGroup="invoice" CssClass="validation1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>


                </td>
                <td class="left top" style="width: 33%;">
                    <div class="boxContainer" style="height: 200px;">
                        <div class="section-title">Policyholder To</div>
                        <div class="paneContentInner">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblClient" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>

                </td>
            </tr>

            <tr>
                <td colspan="3">

                    <asp:GridView ID="gvInvoiceLines" runat="server"
                        AlternatingRowStyle-BackColor="#e8f2ff"
                        AutoGenerateColumns="False"
                        CssClass="gridView"
                        CellPadding="2"
                        DataKeyNames="InvoiceLineID"
                        HorizontalAlign="Center"
                        OnRowCommand="gvInvoiceLines_RowCommand"
                        OnRowDataBound="gvInvoiceLines_OnRowDataBound"
                        OnDataBound="gvInvoiceLines_DataBound"
                        ShowFooter="true"
                        ShowHeaderWhenEmpty="true"
                        Width="100%">
                        <FooterStyle Wrap="False" VerticalAlign="Top" />
                        <RowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <%# Eval("LineDate", "{0:MM/dd/yyyy}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <ig:WebDatePicker ID="txtDate" runat="server" StyleSetName="Default" Width="80px" CssClass="date_picker"
                                        Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" />
                                </FooterTemplate>
                                <FooterStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Activity Code"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:Label ID="lblLineDescription" runat="server" Text='<%# Eval("LineDescription") %>' />
                                    <div style="margin-left: 10px;">
                                        <asp:Label ID="lblComments" runat="server" Text='<%# Eval("Comments") %>' />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <div>
                                        <asp:DropDownList ID="cbxServiceDescription" runat="server" AutoCompleteMode="Suggest"
                                            Width="400px" AutoPostBack="true" OnSelectedIndexChanged="cbxServiceDescription_selectedIndexChanged" />
                                    </div>
                                    <div style="margin-top: 3px;">
                                        <ig:WebTextEditor ID="txtComments" runat="server" Width="90%" />
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Qty", "{0:N2}") %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <ig:WebNumericEditor ID="txtQty" runat="server" Width="80px" MaxLength="10" OnTextChanged="txtQty_TextChanged"
                                        DataMode="Decimal" AutoPostBackFlags-ValueChanged="On" />
                                </FooterTemplate>
                                <FooterStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Rate", "{0:N2}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <ig:WebNumericEditor ID="txtRate" runat="server" Width="80px" MaxLength="10" autopostback="true"
                                        OnTextChanged="txtRate_TextChanged" DataMode="Decimal" />

                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtLineAmount" runat="server" Text='<%# Eval("LineAmount", "{0:N2}") %>'
                                        BorderStyle="None" Style="text-align: right; background-color: inherit;" Width="100px" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <asp:TextBox ID="txtLineAmount" runat="server" Width="100px"
                                        BorderStyle="None" Style="text-align: right; background-color: inherit;" />
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Billable" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <%# Eval("isBillable") == null ? "No" : ((bool)Eval("isBillable")) ? "Yes" : "No" %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <asp:CheckBox ID="cbxBillable" runat="server" />
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
                                ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Panel ID="pnlCommands" runat="server" Visible='<%# Convert.ToInt32(Eval("InvoiceLineID")) > 0 %>'>
                                        <asp:ImageButton ID="btnEdit" runat="server"
                                            CommandName="DoEdit"
                                            CommandArgument='<%#Eval("InvoiceLineID") %>'
                                            ToolTip="Edit"
                                            ImageUrl="~/Images/edit.png"></asp:ImageButton>

                                        <asp:ImageButton ID="btnDelete" runat="server"
                                            CommandName="DoDelete"
                                            OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this line?');"
                                            CommandArgument='<%#Eval("InvoiceLineID") %>'
                                            ToolTip="Delete"
                                            ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                                    </asp:Panel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>&nbsp;</div>
                                    <div class="nowrap">
                                        <asp:ImageButton ID="ibtnAdd" runat="server"
                                            CommandName="DoSave"
                                            ImageAlign="Middle"
                                            ImageUrl="~/Images/toolbar_save.png"                                            
                                            Width="16px" />
                                        <asp:ImageButton ID="ibtnCancel" runat="server"
                                            ImageUrl="~/Images/cancel.png"
                                            ImageAlign="Middle"
                                            OnClick="ibtnCancel_Click"
                                            Width="16px"
                                            Visible="<%# isEditMode() %>" />
                                    </div>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table border="1" style="width: 100%; border-collapse: collapse;">
                        <tbody>
                            <tr>
                                <th scope="col" class="right" style="width: 85%; height: 24px;">
                                    <b>
                                        <label>Total Amount&nbsp;</label></b>
                                </th>
                                <th style="text-align: center">
                                    <asp:TextBox ID="txtTotalAmount" runat="server" Width="100px" BorderStyle="None" Style="font-weight: bold; font-size: 1.2em;" />
                                </th>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnEmailInvoice" runat="server" OnClick="btnEmailInvoice_Click" Style="display: none;" />
        <asp:Button ID="btnSaveInvoiceDetail" runat="server" Text="Save" OnClick="btnSave_Click" Style="display: none;" />
    </ContentTemplate>
</asp:UpdatePanel>
<div style="display: none;">
    <rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="false">
    </rsweb:ReportViewer>
</div>
<div id="email_invoice" title="Email Invoice" style="display: none;">
    <div class="boxContainer">
        <div class="section-title">Recipients</div>
        <div class="paneContentInner">
            <ig:WebTextEditor ID="txtEmailTo" runat="server" Width="100%" TextMode="MultiLine" MultiLine-Rows="3" MultiLine-Wrap="true" MultiLine-Overflow="Visible" />
        </div>
    </div>
    <ig:WebDataGrid ID="contractGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsContacts"
        AutoGenerateColumns="false" Height="300px"
        Width="100%">
        <Columns>
            <ig:BoundDataField DataFieldName="FirstName" Key="FirstName" Header-Text="First Name" />
            <ig:BoundDataField DataFieldName="LastName" Key="LastName" Header-Text="Last Name" />
            <ig:BoundDataField DataFieldName="CompanyName" Key="CompanyName" Header-Text="Company Name" />
            <ig:BoundDataField DataFieldName="Email" Key="Email" Header-Text="Email" />
            <ig:BoundDataField DataFieldName="ContactType" Key="ContactType" Header-Text="Contact Type" />
        </Columns>
        <Behaviors>

            <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                ThresholdFactor="0.5" Enabled="true" />

            <ig:Selection RowSelectType="Multiple" Enabled="True" CellClickAction="Row">
                <SelectionClientEvents RowSelectionChanged="contractGrid_rowsSelected" />
            </ig:Selection>

        </Behaviors>
    </ig:WebDataGrid>
    <asp:EntityDataSource ID="edsContacts" runat="server" ConnectionString="name=CRMEntities" DefaultContainerName="CRMEntities"
        EnableFlattening="False" EntitySetName="vw_Contact"
        Where="it.ClientId = @ClientID"
        OrderBy="it.ContactName Asc">
        <WhereParameters>
            <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
</div>
<script type="text/javascript">
    function sendEmail() {

        // handles email invoice functions
        $("#email_invoice").dialog({
            modal: false,
            width: 800,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
            {
                'Send': function () {
                    if (validateEmail()) {
                        $(this).dialog('close');
                        $("#<%= btnEmailInvoice.ClientID %>").click();
                    }
                },
                'Cancel': function () {
                    $(this).dialog('close');
                }
            }
        });

        return false;
    }

    function contractGrid_rowsSelected(sender, args) {
        var selectedEmails = $find('<%= txtEmailTo.ClientID %>').get_value();

        var selectedRows = args.getSelectedRows();
        var email = args.getSelectedRows().getItem(0).get_cell(3).get_text();
        selectedEmails += email + ";";

        $find('<%= txtEmailTo.ClientID %>').set_value(selectedEmails);
    }

    function validateEmail() {
        var email = $find("<%= txtEmailTo.ClientID %>").get_value();
        //$.find("<%=txtEmailTo.ClientID%>").stylattr('class', '');
        if ($.trim(email) == "") {
            $find("<%=txtEmailTo.ClientID%>").focus();
            return false;
        }

        return true;
    }
</script>

<script type="text/javascript">
    // handle Bill-to functions
    function setMailingAddress(billingName, mailingAddress, cityStateZip) {
        $find("<%= txtBillTo.ClientID %>").set_value(billingName);
        $find("<%= txtBillToAddress1.ClientID %>").set_value(mailingAddress);
        $find("<%= txtBillToAddress3.ClientID %>").set_value(cityStateZip);
    }

    function rowSelChange(sender, args) {
        var dd = $find('<%= ddlBillTo.ClientID %>');
        // get billing name column
        var billingName = args.getSelectedRows().getItem(0).get_cell(1).get_text();
        // set row value in ddl
        dd.set_currentValue(billingName, true);

        // get mailing address to show on UI
        var mailingAddress = args.getSelectedRows().getItem(0).get_cell(2).get_text();
        var cityname = args.getSelectedRows().getItem(0).get_cell(3).get_text();
        var statename = args.getSelectedRows().getItem(0).get_cell(4).get_text();
        var zipcode = args.getSelectedRows().getItem(0).get_cell(5).get_text();

        var cityStateZip = cityname + ", " + statename + " " + zipcode;

        setMailingAddress(billingName, mailingAddress, cityStateZip);

        // set focus back to ddl
        dd._elements["Input"].focus();
        dd.closeDropDown();
    }

    function cancelDDSel(sender, args) {
        args.set_cancel(true);
    }
</script>

