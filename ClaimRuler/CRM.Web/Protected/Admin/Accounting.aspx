<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true" CodeBehind="Accounting.aspx.cs" Inherits="CRM.Web.Protected.Admin.Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="paneContent">
		<div class="page-title">
			Accounting
		</div>
		<asp:GridView ID="gvInvoices" Width="100%" runat="server" OnRowCommand="gv_RowCommand" CssClass="gridView" HorizontalAlign="Center"
			AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff" ShowFooter="true"
			OnRowDataBound="gv_RowDataBound" PageSize="20"
			PagerSettings-PageButtonCount="5"
			PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt">
			<PagerStyle CssClass="pager" />
			<RowStyle HorizontalAlign="Center" />
			<EmptyDataTemplate>
				No info available.
			</EmptyDataTemplate>
			<FooterStyle HorizontalAlign="Right" />
			<Columns>
				<asp:TemplateField HeaderText="Claim #" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<asp:Label ID="lblClaimNumber" runat="server" />
					</ItemTemplate>
				</asp:TemplateField>

				<asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("LeadInvoice.InvoiceNumber", "{0:N0}")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Invoice Date" SortExpression="InvoiceDate"
					ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("LeadInvoice.InvoiceDate", "{0:MM-dd-yyyy}")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Client Name" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("ClientName")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Client Paid Date" SortExpression="InvoiceDate"
					ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("ClientPaid", "{0:MM-dd-yyyy}")%>
					</ItemTemplate>
				</asp:TemplateField>

				<asp:TemplateField HeaderText="Adjuster Name" SortExpression="InvoiceDate"
					ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("AdjusterMaster.AdjusterName")%>
						
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Total Commission" ItemStyle-HorizontalAlign="Right">
					<ItemTemplate>
						<%# Eval("CommissionTotal", "{0:N2}")%>
					</ItemTemplate>					
					<FooterTemplate>						
						<asp:Label ID="lblCommissionTotal" runat="server" />
					</FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Total Expenses" ItemStyle-HorizontalAlign="Right">
					<ItemTemplate>
						<%# Eval("TotalExpenses", "{0:N2}")%>
					</ItemTemplate>
					<FooterTemplate>
						<asp:Label ID="lblTotalExpenses" runat="server" />
					</FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Adjuster Net" ItemStyle-HorizontalAlign="Right">
					<ItemTemplate>
						<%# Eval("AdjusterNet", "{0:N2}")%>
					</ItemTemplate>
					<FooterTemplate>
						<asp:Label ID="lblAdjusterNet" runat="server" />
					</FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Invoice Total" ItemStyle-HorizontalAlign="Right">
					<ItemTemplate>
						<%# Eval("InvoiceTotal", "{0:N2}")%>
					</ItemTemplate>
					<FooterTemplate>
						<asp:Label ID="lblInvoiceTotal" runat="server" />
					</FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px"
					ItemStyle-Wrap="false">
					<ItemTemplate>
						<asp:ImageButton ID="btnEdit" runat="server" ToolTip="Edit" ImageUrl="~/Images/edit.png"
							CausesValidation="false" />
						&nbsp;
					<asp:ImageButton ID="btnPrint" runat="server" ToolTip="Print" ImageUrl="~/Images/print_icon.png"
						CausesValidation="false" />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</div>
	<asp:HiddenField ID="hfNewPermission" runat="server" Value="0" />
	<asp:HiddenField ID="hfEditPermission" runat="server" Value="0" />
	<asp:HiddenField ID="hfDeletePermission" runat="server" Value="0" />
	<asp:HiddenField ID="hfViewPermission" runat="server" Value="0" />
	<asp:HiddenField ID="hfKeywordSearch" runat="server" Value="" />
	<script type="text/javascript">
		function editInvoice(invoiceID) {
			PopupCenter("../Admin/LeadInvoice.aspx?id=" + invoiceID.toString(), "Invoice", 1200, 600);
			return false;
		}
		function printInvoice(invoiceID) {
			PopupCenter("../../Content/PrintInvoice.aspx?id=" + invoiceID.toString(), "Invoice", 800, 800);
			return false;
		}
	</script>
</asp:Content>
