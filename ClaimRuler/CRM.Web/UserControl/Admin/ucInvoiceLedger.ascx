<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInvoiceLedger.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.ucInvoiceLedger" %>
<script type="text/javascript">
	
	function editInvoice(invoiceID) {
		PopupCenter("../LeadInvoice.aspx?q=" + invoiceID.toString(), "Invoice", 1200, 600);
		return false;
	}
	function printInvoice(invoiceID) {
		PopupCenter("../Content/PrintInvoice.aspx?q=" + invoiceID.toString(), "Invoice", 800, 800);
		return false;
	}
</script>
<asp:Panel ID="pnlGrid" runat="server">
	<asp:GridView ID="gvInvoices"  Width="99%" runat="server" OnRowCommand="gv_RowCommand" CssClass="gridView"
		AutoGenerateColumns="False" 
        AlternatingRowStyle-BackColor="#e8f2ff"
        CellPadding="4"         
		OnRowDataBound="gv_RowDataBound"  
        PageSize="20"
		PagerSettings-PageButtonCount="5"
		PagerStyle-Font-Bold="true" 
        OnSorting="gv_onSorting" 
        AllowSorting="true" 
        PagerStyle-Font-Size="9pt">
		<PagerStyle CssClass="pager" />
		<RowStyle HorizontalAlign="Center" />
		<EmptyDataTemplate>
			No invoices available.
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField HeaderText="Invoice Date"  SortExpression="InvoiceDate"
				ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("InvoiceDate", "{0:MM-dd-yyyy}")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Due Date"  ItemStyle-HorizontalAlign="Center"
				SortExpression="DueDate">
				<ItemTemplate>
					<%# Eval("DueDate", "{0:MM-dd-yyyy}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Invoice Number"  ItemStyle-HorizontalAlign="Right"
				SortExpression="AdjusterInvoiceNumber">
				<ItemTemplate>
					<%# Eval("InvoiceNumber", "{0:N0}")%>
				</ItemTemplate>
			</asp:TemplateField>			
			<asp:TemplateField HeaderText="Bill To"  SortExpression="BillToName"
				ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("BillToName")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Total Amount"  ItemStyle-HorizontalAlign="right"
				SortExpression="TotalAmount">
				<ItemTemplate>
					<asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("TotalAmount", "{0:N2}")%>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" 
				ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton ID="btnEdit" runat="server" ToolTip="Edit" ImageUrl="~/Images/edit.png"
						CausesValidation="false"  />
					&nbsp;
					<asp:ImageButton ID="btnPrint" runat="server" ToolTip="Print" ImageUrl="~/Images/print_icon.png"
						CausesValidation="false"  />
					&nbsp;
					<asp:ImageButton ID="btnDelete" runat="server" 
                        CommandName="DoDelete" 
                        OnClientClick="javascript:return ConfirmDialog(this,'Are you sure you want to void this invoice?');"
						CommandArgument='<%#Eval("InvoiceID") %>' 
                        ToolTip="Delete" 
                        ImageUrl="~/Images/delete_icon.png">
					</asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Panel>
<asp:HiddenField ID="hfId" runat="server" Value="0" />
