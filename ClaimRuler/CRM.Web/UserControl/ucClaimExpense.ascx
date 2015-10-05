<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimExpense.ascx.cs" Inherits="CRM.Web.UserControl.ucClaimExpense" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>


<asp:Panel ID="pnlEditExpense" runat="server" Visible="false" DefaultButton="btnSaveExpense">
    <div class="boxContainer">
        <div class="section-title">
            Expense Detail
        </div>
        <table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 0px;" border="0" class="editForm no_min_width">
            <tr>
                <td colspan="3" class="center">
                    <asp:Label ID="lblMessage" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Expense Type</td>
                <td class="redstar">*</td>
                <td>
                    <asp:DropDownList ID="ddlExpenseType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExpenseType_SelectedIndexChanged" />
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlExpenseType" InitialValue="0"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select expense type."
                            ValidationGroup="expense" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Adjuster</td>
                <td class="redstar">*</td>
                <td class="nowrap">
                    <ig:WebTextEditor ID="txtExpenseAdjuster" runat="server" Enabled="false" Width="250px" />
                    <a href="javascript:findAdjusterForExpenseDialog();">
                        <asp:Image ID="imgAdjusterFind" runat="server" ImageUrl="~/Images/searchbg.png" ImageAlign="Top" />
                    </a>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvAdjuster" runat="server" ControlToValidate="txtExpenseAdjuster"
                            Display="Dynamic" CssClass="validation1" SetFocusOnError="True" ErrorMessage="Please select adjuster."
                            ValidationGroup="expense" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Expense Date
                </td>
                <td class="redstar">*</td>
                <td>
                    <ig:WebDatePicker ID="txtExpenseDate" runat="server" CssClass="date_picker">
                        <Buttons>
                            <CustomButton ImageUrl="~/Images/ig_calendar.gif" />
                        </Buttons>
                    </ig:WebDatePicker>
                    <div>
                        <asp:RequiredFieldValidator ID="tfvExpenseDate" runat="server" ControlToValidate="txtExpenseDate" ValidationGroup="expense"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter expense date." CssClass="validation1" />
                    </div>
                </td>
            </tr>
            
            <tr>
                <td class="right top">Description</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtExpenseDescription" MaxLength="500" TextMode="MultiLine" MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    <div>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtExpenseDescription" ValidationGroup="expense"
                            SetFocusOnError="True" Display="Dynamic" ErrorMessage="Please enter description." CssClass="validation1" />--%>
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right top">Internal Comments</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" ID="txtMyComments" MaxLength="500" TextMode="MultiLine" MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    <div>
                      
                    </div>
                </td>
            </tr>
            <tr>
                <td class="right">Reimburse</td>
                <td class="redstar"></td>
                <td>
                    <asp:CheckBox ID="cbxExpenseReimburse" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="right">Quantity</td>
                <td class="redstar"></td>
                <td>
                    <ig:WebNumericEditor ID="txtExpenseQty" runat="server" MinDecimalPlaces="2" Width="80px">
                    </ig:WebNumericEditor>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>OR</td>
            </tr>
            <tr>
                <td class="right">
                    <asp:Label ID="lblMyAmount" runat="server" Text="Amount"></asp:Label>
                </td>
                <td class="redstar"></td>
                <td>
                    <asp:Label ID="lblAmount" runat="server" Visible="false"></asp:Label>
                    <ig:WebNumericEditor ID="txtExpenseAmount" runat="server" MinDecimalPlaces="2" Width="80px">
                    </ig:WebNumericEditor>
                    <div>
                        <asp:CustomValidator ID="CustomValidator" runat="server" ValidationGroup="expense"
                            CssClass="validation1" ErrorMessage="Please enter either quantity or amount." Display="Dynamic"
                             OnServerValidate="CustomValidator_Amount_Qty"
                            SetFocusOnError="True" ClientValidationFunction="validate_Amount_Qty">

                        </asp:CustomValidator>
                    </div>
                </td>
               <%-- <td style="font-size:smaller; text-wrap:avoid; color:red; font-style:italic"></td>--%>
                
            </tr>
          
             <tr>
                <td class="right top">Email Adjuster</td>
                <td class="redstar top"></td>
                <td>
                    <asp:CheckBox ID="cbEmailAdjuster" runat="server" />
                    <div>
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right top">Email Client Contact</td>
                <td class="redstar top"></td>
                <td>
                    <asp:CheckBox ID="cbEmailClient" runat="server" />
                    <div>
                    </div>
                </td>
            </tr>
             <tr>
                <td class="right top">Email to:</td>
                <td class="redstar top"></td>
                <td>
                    <ig:WebTextEditor runat="server" TextMode="Email" ID="txtEmailTo" MaxLength="500"  MultiLine-Rows="3" Width="100%"></ig:WebTextEditor>
                    
                    <div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="center">
                    <asp:Button ID="btnSaveExpense" runat="server" Text="Save" CssClass="mysubmit" OnClick="btnSaveExpense_Click" CausesValidation="true" ValidationGroup="expense" />
                    &nbsp;
                    <asp:Button ID="btnCancelExpense" runat="server" Text="Cancel" CssClass="mysubmit" OnClick="btnCancelExpense_Click" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>


<asp:GridView ID="gvExpense" CssClass="gridView"
    AutoGenerateColumns="False"
    AlternatingRowStyle-BackColor="#e8f2ff"
    CellPadding="2"
    HorizontalAlign="Center"
    OnRowCommand="gvExpense_RowCommand"
     OnRowDataBound="gvExpense_RowDataBound"
    ShowHeaderWhenEmpty="true"
    Width="100%" runat="server">
    <RowStyle HorizontalAlign="Center" />
    <HeaderStyle HorizontalAlign="Center" />
    <EmptyDataTemplate>
        No expenses have been defined.
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
            ItemStyle-Wrap="false">
            <ItemTemplate>
                <asp:ImageButton ID="btnEdit" runat="server"
                    CommandName="DoEdit"
                    CommandArgument='<%#Eval("ClaimExpenseID") %>'
                    ToolTip="Edit"
                    ImageUrl="~/Images/edit.png"
                    Visible="true" />
                &nbsp;
				            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
                                OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this expense?');"
                                CommandArgument='<%#Eval("ClaimExpenseID") %>'
                                ToolTip="Delete"
                                ImageUrl="~/Images/delete_icon.png"
                                Visible="true" />

            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date">
            <ItemStyle Width="80px" />
            <ItemTemplate>
                <%#Eval("ExpenseDate", "{0:MM/dd/yyyy}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Expense">
            <ItemTemplate>
                <%#Eval("ExpenseType.ExpenseName")%>
                <asp:HiddenField ID="hfBilled" runat="server" Value='<%# Eval("Billed") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Adjuster">
            <ItemTemplate>
                <%# Eval("AdjusterMaster") == null ? "" : Eval("AdjusterMaster.adjusterName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ExpenseDescription")%>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Comments">
            <ItemTemplate>
                <%#Eval("InternalComments")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Reimb.">
            <ItemStyle Width="50px" />
            <ItemTemplate>
                <%#Convert.ToBoolean(Eval("IsReimbursable")) ?  "Yes" : "No" %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Quantity">
            <ItemStyle Width="80px" HorizontalAlign="Right" />
            <ItemTemplate>
                <%#Eval("ExpenseQty", "{0:N2}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amount">
            <ItemStyle Width="80px" HorizontalAlign="Right" />
            <ItemTemplate>
                <%#Eval("ExpenseAmount", "{0:N2}")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:HiddenField ID="hf_expenseAdjusterID" runat="server" Value="0" />

<div id="div_ClaimExpenseAdjustersList" style="display: none; width: 90%;" title="Select Adjuster">
    <div class="boxContainer">
        <div class="section-title">Adjusters</div>
        <ig:WebDataGrid ID="adjusterGrid" runat="server" CssClass="gridView smallheader" DataSourceID="edsAdjusters"
            AutoGenerateColumns="false" Height="300px"
            Width="100%">
            <Columns>
                <ig:BoundDataField DataFieldName="AdjusterId" Key="AdjusterId" Header-Text="ID" Width="50px" />
                <ig:BoundDataField DataFieldName="AdjusterName" Key="AdjusterName" Header-Text="Adjuster Name" />
                <ig:BoundDataField DataFieldName="email" Key="email" Header-Text="E-mail Address" />
            </Columns>
            <Behaviors>

                <ig:VirtualScrolling ScrollingMode="Virtual" DataFetchDelay="500" RowCacheFactor="10"
                    ThresholdFactor="0.5" Enabled="true" />

                <ig:Selection RowSelectType="Single" Enabled="True" CellClickAction="Row">
                    <SelectionClientEvents RowSelectionChanged="claimExpenseAdjusterGrid_rowsSelected" />
                </ig:Selection>
            </Behaviors>
        </ig:WebDataGrid>
    </div>
</div>
<script type="text/javascript">
    function validate_Amount_Qty(sender, args) {
        var txtExpenseAmount = $find("<%= txtExpenseAmount.ClientID %>").get_value();
        var txtExpenseQty = $find("<%= txtExpenseQty.ClientID %>").get_value();

        if (txtExpenseAmount > 0 && txtExpenseQty > 0) {
            args.IsValid = false;
        }
        else {
            args.IsValid = true;
        }
    }
</script>
<script type="text/javascript">
    // handle events for claim expenses
    function findAdjusterForExpenseDialog() {

        // show dialog
        $("#div_ClaimExpenseAdjustersList").dialog({
            modal: false,
            width: 600,
            close: function (e, ui) {
                $(this).dialog('destroy');
            },
            buttons:
               {
                   'Done': function () {
                       $(this).dialog('close');
                   }
               }
        });
    }
    function claimExpenseAdjusterGrid_rowsSelected(sender, args) {
        var selectedRows = args.getSelectedRows();

        var adjusterID = args.getSelectedRows().getItem(0).get_cell(0).get_text();
        var adjusterName = args.getSelectedRows().getItem(0).get_cell(1).get_text();

        $("#<%= hf_expenseAdjusterID.ClientID %>").val(adjusterID);
        $find("<%= txtExpenseAdjuster.ClientID %>").set_value(adjusterName);
    }
</script>
