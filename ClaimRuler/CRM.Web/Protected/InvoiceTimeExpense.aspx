<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true" CodeBehind="InvoiceTimeExpense.aspx.cs" Inherits="CRM.Web.Protected.InvoiceTimeExpense" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Invoice Claim Time & Expense
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
    <div class="message_area">
        <asp:Label ID="lblMessage" runat="server" />
    </div>

    <table style="width: 80%; border-collapse: separate; border-spacing: 2px; padding: 3px; margin: auto;" border="0" class="editForm no_min_width">
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
                                <td class="right">Invoice Date
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
                                <td class="right">Due Date</td>
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
                                <td class="right">Invoice Number</td>
                                <td></td>
                                <td>
                                    <ig:WebTextEditor ID="txtInvoiceNumber" runat="server" Enabled="false" Width="100px" TabIndex="3" />
                                    &nbsp<span class="redstar">Auto-generated</span>
                                </td>
                            </tr>

                            <tr>
                                <td class="right">Policy Type</td>
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
                                <td class="right">Insurer Claim Number</td>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblInsurerClaimNumber" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">Reference #</td>
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
                                <td class="right">Bill To</td>
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

                <asp:GridView ID="gvTimeExpense"
                    AutoGenerateColumns="False"
                    AlternatingRowStyle-BackColor="#e8f2ff"
                    CellPadding="4"
                    CssClass="gridView"
                    DataKeyNames="InvoiceLineID,ServiceTypeID,ExpenseTypeID"
                    HorizontalAlign="Center"
                    OnRowDataBound="gvTimeExpense_RowDataBound"
                    runat="server"
                    ShowFooter="true"
                    Width="100%">
                    <RowStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="Include">
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbxAll" runat="server"  OnCheckedChanged="cbxAll_CheckedChanged" AutoPostBack="true" Checked="true"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbxInclude" runat="server" Checked="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                               <asp:Label ID="lblLineDate" runat="server" Text='<%# Eval("LineDate", "{0:MM/dd/yyyy}")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Activity Code"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                            <ItemStyle Width="60%" />
                            <ItemTemplate>
                                <asp:Label ID="lblLineDescription" runat="server" Text='<%# Eval("LineDescription") %>' />
                                <div style="margin-left: 10px;">
                                    <asp:Label ID="lblComments" runat="server" Text='<%# Eval("Comments") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right">
                            <ItemStyle Width="200px" />
                            <ItemTemplate>
                                 <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty", "{0:N2}") %>' />
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate" ItemStyle-HorizontalAlign="Right">
                            <ItemStyle Width="200px" />
                            <ItemTemplate>
                                <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate", "{0:N2}")%>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <span class="right"><b>Total Amount</b></span>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="200px" HorizontalAlign="Right" />
                            <ItemTemplate>
                                
                                <asp:Label ID="lblLineTotal" runat="server" Text='<%# Eval("LineAmount", "{0:N2}") %>' />
                                  
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                <asp:Label ID="lblInvoiceTotal" runat="server" /></b>
                            </FooterTemplate>
                            <FooterStyle Width="200px" HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
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
            OrderBy="it.FirstName, it.LastName Asc">
            <WhereParameters>
                <asp:SessionParameter Name="ClientID" SessionField="ClientId" Type="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
        <asp:Button ID="btnEmailInvoice" runat="server" OnClick="btnEmailInvoice_Click" Style="display: none;" />
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
            var selectedEmails = $('#<%= txtEmailTo.ClientID %>').val();

            var selectedRows = args.getSelectedRows();
            var email = args.getSelectedRows().getItem(0).get_cell(3).get_text();
            selectedEmails += email + ";";
            $('#<%= txtEmailTo.ClientID %>').val(selectedEmails);
        }

        function validateEmail() {
            var email = $("#<%=txtEmailTo.ClientID%>").val();
            $("#<%=txtEmailTo.ClientID%>").attr('class', '');
            if ($.trim(email) == "") {
                $("#<%=txtEmailTo.ClientID%>").focus();
                $("#<%=txtEmailTo.ClientID%>").attr('class', 'ErrorControl');
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
</asp:Content>
