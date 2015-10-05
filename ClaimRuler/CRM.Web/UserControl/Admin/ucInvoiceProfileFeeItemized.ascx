<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInvoiceProfileFeeItemized.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucInvoiceProfileFeeItemized" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<asp:Panel ID="pnlGrid" runat="server">
    <div class="paneContentInner">
        <asp:LinkButton CssClass="link" ID="lbtnAddService" runat="server" Text="Add Service" OnClick="lbtnAddService_Click" />
        <asp:LinkButton CssClass="link" ID="lbtnAddExpense" runat="server" Text="Add Expense" OnClick="lbtnAddExpense_Click" />
    </div>
    <asp:GridView ID="gvItems" runat="server"  AutoGenerateColumns="False"
        CellPadding="2" 
        CssClass="gridView"
        HorizontalAlign="Center"  
        OnRowCommand="gvItems_RowCommand"
        OnRowDataBound="gvItems_RowDataBound"
        Width="100%">
        <RowStyle HorizontalAlign="Center" />
        <FooterStyle HorizontalAlign="Center" />
        <Columns>
            <asp:TemplateField HeaderText="Item Description">
                <ItemTemplate>
                   <asp:Label ID="lblServiceDescription" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rate Amount">
                <ItemStyle HorizontalAlign="Right" Width="80px" />
                <ItemTemplate>
                    <%# Eval("ItemRate", "{0:N2}") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Percentage">
                <ItemStyle HorizontalAlign="Right" Width="80px" />
                <ItemTemplate>
                    <%# string.Format("{0:N2}", (Decimal)(Eval("ItemPercentage") ?? 0) * 100 )%> %
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" Width="50px" />
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" 
                        CommandName="DoEdit" 
                        CommandArgument='<%#Eval("ID") %>'
                        ToolTip="Edit" 
                        ImageUrl="~/Images/edit.png" />
                    &nbsp;
				<asp:ImageButton ID="btnDelete" runat="server" 
                    CommandName="DoDelete"
                    OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this item?');"
                    CommandArgument='<%#Eval("ID") %>' 
                    ToolTip="Delete" 
                    ImageUrl="~/Images/delete_icon.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="pnlAddExpense" runat="server" Visible="false" DefaultButton="btnAddExpense_save">
    <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm no_min_width">
        <tr>
            <td class="right" style="width: 15%;">Expense</td>
            <td class="redstar">*</td>
            <td>
                <asp:DropDownList ID="ddlExpenses" runat="server" Width="300px" />
                <div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlExpenses"
                        Display="Dynamic" ErrorMessage="Please select expense." SetFocusOnError="True" ValidationGroup="expense"
                        CssClass="validation1" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td class="right">
                Description
            </td>
            <td></td>
            <td>
                <ig:WebTextEditor ID="txtExpenseDescription" runat="server" MaxLength="100" Width="250px"></ig:WebTextEditor>
            </td>
        </tr>
          <tr>
            <td class="right">Condition</td>
            <td></td>
            <td class="nowrap">
                <asp:DropDownList ID="ddlConditionalOperator" runat="server" 
                    AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlConditionalOperator_SelectedIndexChanged"
                    Width="150px"
                    >
                    <asp:ListItem Text="--- Select ---" Value="0" />
                    <asp:ListItem Text="Equals" Value="1"/>
                    <asp:ListItem Text="Less than" Value="2"/>
                    <asp:ListItem Text="Less than or equal" Value="3"/>
                    <asp:ListItem Text="Greater than" Value="4"/>
                    <asp:ListItem Text="Greater than or equal" Value="5"/>
                </asp:DropDownList>
              
                <ig:WebNumericEditor ID="txtOperand" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" Visible="false" />                                           
                
            </td>
        </tr>
        <tr>
            <td class="right">Rate</td>
            <td></td>
            <td>
                <ig:WebNumericEditor ID="txtExpenseRate" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
        <tr runat="server" id="tr_percentage">
            <td class="right">Percentage</td>
            <td></td>
            <td>
                <ig:WebPercentEditor ID="txtExpensePercentage" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
         <tr>
            <td class="right">Adjuster Payroll %</td>
            <td></td>
            <td>
                <ig:WebPercentEditor ID="txtExpPayroll" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
        <tr>
                <td></td>
                <td></td>
                <td>OR</td>
        </tr>
         <tr>
            <td class="right">Adjuster Payroll Flat Fee</td>
            <td></td>
            <td>
                <ig:WebNumericEditor ID="txtExpPayrollFee" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>
                <asp:Button ID="btnAddExpense_save" runat="server" Text="Save" OnClick="btnAddExpense_save_Click" CssClass="mysubmit" CausesValidation="true" ValidationGroup="expense" />
                &nbsp;
			    <asp:Button ID="btnAddExpense_cancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="mysubmit" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnlAddService" runat="server" Visible="false" DefaultButton="btnAddService_save">
    <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm no_min_width">
        <tr>
            <td class="right" style="width: 15%;">Service</td>
            <td class="redstar">*</td>
            <td>
                <asp:DropDownList ID="ddlServices" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvInvoiceTYpe" runat="server" ControlToValidate="ddlServices"
                        Display="Dynamic" ErrorMessage="Please select service." SetFocusOnError="True" ValidationGroup="service"
                        CssClass="validation1" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td class="right">
                Description
            </td>
            <td></td>
            <td>
                <ig:WebTextEditor ID="txtItemDescription" runat="server" MaxLength="100" Width="250px"></ig:WebTextEditor>
            </td>
        </tr>
        <tr>
            <td class="right">Amount</td>
            <td></td>
            <td>
                <ig:WebNumericEditor ID="txtServiceRate" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
        <tr>
            <td class="right">Percentage</td>
            <td></td>
            <td>
                <ig:WebPercentEditor ID="txtServicePercentage" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
         <tr>
            <td class="right">Adjuster Payroll %</td>
            <td></td>
            <td>
                <ig:WebPercentEditor ID="txtSerPayroll" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
        <tr>
                <td></td>
                <td></td>
                <td>OR</td>
        </tr>
         <tr>
            <td class="right">Adjuster Payroll Flat Fee</td>
            <td></td>
            <td>
                <ig:WebNumericEditor ID="txtSerPayrollFee" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" Width="80px" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>
                <asp:Button ID="btnAddService_save" runat="server" Text="Save" OnClick="btnAddService_save_Click" CssClass="mysubmit" CausesValidation="true" ValidationGroup="service" />
                &nbsp;
			    <asp:Button ID="btnAddService_cancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="mysubmit" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
