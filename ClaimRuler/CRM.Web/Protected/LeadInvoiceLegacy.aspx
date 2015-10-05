<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true" CodeBehind="LeadInvoiceLegacy.aspx.cs" Inherits="CRM.Web.Protected.LeadInvoiceLegacy" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>
<%@ Register Src="~/UserControl/Admin/ucPolicyType.ascx" TagName="ucPolicyType" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Admin/ucSelectAdjuster.ascx" TagName="ucSelectAdjuster" TagPrefix="uc2" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    Prepare Invoice
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
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>


            <div class="message_area">
                <asp:Label ID="lblMessage" runat="server" />
            </div>

            <table style="width: 95%; border-collapse: separate; border-spacing: 2px; padding: 3px;" border="0" class="editForm no_min_width">
                <tr id="tr_email" style="display: none;">
                    <td colspan="3">
                        <div class="boxContainer">
                            <div class="section-title">Email Invoice</div>
                            <div class="paneContentInner">
                                <div>
                                    <span>Recipient:</span>
                                    &nbsp;<ig:WebTextEditor ID="txtEmailTo" runat="server" Width="300px" TextMode="Email" />
                                    &nbsp;
                                <asp:Button ID="btnEmailInvoice" runat="server" OnClick="btnEmailInvoice_Click" Text="Send" CssClass="mysubmit" CausesValidation="true" ValidationGroup="emailInvoice" OnClientClick="javascript: return validateEmail();" />
                                    &nbsp;
                              <input type="button" title="Cancel" value="Cancel" class="mysubmit" onclick="javascript: return cancelEmail()" />
                                </div>
                            </div>
                        </div>
                </tr>
                <tr>
                    <td style="width: 45%"></td>
                    <td style="width: 5%"></td>
                    <td style="width: 45%" class="right">Invoice Date&nbsp;<span class="redstar">*</span>&nbsp;
						<div class="float_right">
                            <ig:WebDatePicker ID="txtInvoiceDate" runat="server" StyleSetName="Default" Width="80px"
                                Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" CssClass="date_picker" />
                        </div>
                        <div class="right">
                            <div>
                                <asp:RequiredFieldValidator ID="rfvInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate"
                                    Display="Dynamic" ForeColor="" ErrorMessage="Please enter date." ValidationGroup="invoice"
                                    CssClass="validation1" SetFocusOnError="true" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="right">Due Date&nbsp;<span class="redstar">*</span>&nbsp;
						<div class="float_right">
                            <ig:WebDatePicker ID="txtDueDate" runat="server" StyleSetName="Default" CssClass="date_picker" Width="80px"
                                Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" />
                        </div>
                        <div class="float_right">
                            <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ControlToValidate="txtDueDate"
                                Display="Dynamic" ForeColor="" ErrorMessage="Please enter date." ValidationGroup="invoice"
                                CssClass="validation1" SetFocusOnError="true" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="right">
                        <asp:Panel ID="Panel1" runat="server">
                            Reference #&nbsp;<ig:WebTextEditor ID="txtReferenceNumber" runat="server" MaxLength="20" Width="80px" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="right">
                        <asp:Panel ID="pnlInvoiceNumber" runat="server">
                            Invoice Number&nbsp;&nbsp;<ig:WebTextEditor ID="txtInvoiceNumber" runat="server" ReadOnly="true" BorderStyle="None" Width="80px" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="left">
                        <div style="border: 1px solid grey; padding-top: 5px; padding-bottom: 5px; height: 180px; width: 85%;">
                            <table>
                                <tr>
                                    <td>Bill To&nbsp;<span class="redstar">*</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBillTo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBillTo_SelectedIndexChanged" />
                                        <div>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBillTo"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please select Bill-To" ValidationGroup="invoice"
                                                CssClass="validation1" InitialValue="0" Visible="false">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%">Name
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillTo" runat="server" Width="250px" />
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvBillTo" runat="server" ControlToValidate="txtBillTo"
                                                Display="Dynamic" ForeColor="" ErrorMessage="Please select Bill-To" ValidationGroup="invoice"
                                                CssClass="validation1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Address
                                    </td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillToAddress1" runat="server" Width="250px" />
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
                                    <td>
                                        <ig:WebTextEditor ID="txtBillToAddress2" runat="server" Width="250px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <ig:WebTextEditor ID="txtBillToAddress3" runat="server" Width="250px" />
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
                    </td>
                    <td style="width: 5%"></td>
                    <td style="text-align: left; vertical-align: top;">
                        <div style="border: 1px solid grey; padding-top: 5px; padding-bottom: 5px; height: 180px; width: 80%; float: right;">
                            <table>
                                <tr>
                                    <td style="width: 30%; vertical-align: top;">Policyholder
                                    </td>
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
                    </td>
                </tr>

            </table>
            <asp:GridView ID="gvInvoiceLines" ShowFooter="true" Width="95%" ShowHeaderWhenEmpty="true" CssClass="gridView" HorizontalAlign="left"
                runat="server" DataKeyNames="InvoiceLineID" OnRowCommand="gvInvoiceLines_RowCommand"
                OnRowDataBound="gvInvoiceLines_OnRowDataBound" AutoGenerateColumns="False" CellPadding="4"
                AlternatingRowStyle-BackColor="#e8f2ff" OnDataBound="gvInvoiceLines_DataBound"
                RowStyle-HorizontalAlign="Left">
                <FooterStyle Wrap="False" VerticalAlign="Top" />
                <RowStyle HorizontalAlign="Center" />
                <Columns>
                    <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#e8f2ff">
                        <ItemTemplate>
                            <%# Eval("LineDate", "{0:MM/dd/yyyy}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div>
                                &nbsp;
                            </div>
                            <ig:WebDatePicker ID="txtDate" runat="server" StyleSetName="Default" Width="80px" CssClass="date_picker" Buttons-CustomButton-ImageUrl="~/Images/ig_calendar.gif" />
                        </FooterTemplate>
                        <FooterStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Activity Code" HeaderStyle-BackColor="#e8f2ff"
                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80%" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblLineDescription" runat="server" Text='<%# Eval("LineDescription") %>' />
                            <div style="margin-left:10px;">
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
                            <div style="margin-top: 10px;">
                                Comments:&nbsp;<ig:WebTextEditor ID="txtComments" runat="server" Width="80%" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("Qty", "{0:N2}") %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div>
                                &nbsp;
                            </div>
                            <ig:WebNumericEditor ID="txtQty" runat="server" Width="50px" MaxLength="10" OnTextChanged="txtQty_TextChanged"
                                DataMode="Decimal" AutoPostBackFlags-ValueChanged="On" />
                        </FooterTemplate>
                        <FooterStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("InvoiceServiceType.ServiceRate", "{0:N2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div>
                                &nbsp;
                            </div>
                            <ig:WebNumericEditor ID="txtRate" runat="server" Width="50px" MaxLength="10" AutoPostBack="true"
                                OnTextChanged="txtRate_TextChanged" DataMode="Decimal" />

                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("InvoiceServiceType.InvoiceServiceUnit.UnitDescription") %>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div>
                                &nbsp;
                            </div>
                            <asp:Label ID="lblUnitDescription" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLineAmount" runat="server" Text='<%# Eval("LineAmount", "{0:N2}") %>'
                                BorderStyle="None" Style="text-align: right; width: 80px; background-color: inherit;" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <div>
                                &nbsp;
                            </div>
                            <asp:TextBox ID="txtLineAmount" runat="server"
                                BorderStyle="None" Style="text-align: right; width: 80px; background-color: inherit;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Billable" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
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
                    <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" HeaderStyle-BackColor="#e8f2ff"
                        ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Panel ID="pnlCommands" runat="server" Visible='<%# Convert.ToInt32(Eval("InvoiceLineID")) > 0 %>'>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("InvoiceLineID") %>'
                                    ToolTip="Edit" ImageUrl="~/Images/edit.png"></asp:ImageButton>
                                &nbsp;
								<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this line?');"
                                    CommandArgument='<%#Eval("InvoiceLineID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
                            </asp:Panel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div>
                                &nbsp;
                            </div>
                            <asp:ImageButton ID="ibtnAdd" runat="server" OnClick="ibtnAdd_Click" ImageUrl="~/Images/add.png" ImageAlign="Middle" />
                            &nbsp;
							<asp:ImageButton ID="ibtnCancel" runat="server" OnClick="ibtnCancel_Click" ImageUrl="~/Images/cancel.png" ImageAlign="Middle"
                                Visible="<%# isEditMode() %>" />
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table border="1" style="width: 95%; border-collapse: collapse;">
                <tbody>
                    <tr>
                        <th scope="col" class="right" style="width: 78.5%; height: 24px;">
                            <b>
                                <label>Total Amount&nbsp;</label></b>
                        </th>
                        <th style="text-align: center">
                            <asp:TextBox ID="txtTotalAmount" runat="server" Width="80px" BorderStyle="None" Style="font-weight: bold; font-size: 1.2em;" />
                        </th>
                    </tr>
                </tbody>
            </table>
            <div style="text-align: center; margin-top: 10px;">
                <div style="display: none;">
                    <asp:Button ID="btnSaveInvoiceDetail" runat="server" Text="Save" OnClick="btnSave_Click"
                        class="mysubmit" />
                </div>



            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none;">
        <rsweb:ReportViewer ID="reportViewer" runat="server" Height="100%" Width="100%" ShowToolBar="false">
        </rsweb:ReportViewer>
    </div>
    <script type="text/javascript">
        function validateEmail(obj) {
            var email = $("#<%=txtEmailTo.ClientID%>").val();
            if (email == "") {
                $("#<%=txtEmailTo.ClientID%>").focus();
                return false;
            }

            return true;
        }

        function cancelEmail() {
            $("#tr_email").hide();
        }
        function sendEmail() {
            $("#tr_email").show();
            return false;
        }
    </script>
</asp:Content>
