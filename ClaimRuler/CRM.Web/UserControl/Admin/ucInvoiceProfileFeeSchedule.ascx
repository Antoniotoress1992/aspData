<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInvoiceProfileFeeSchedule.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucInvoiceProfileFeeSchedule" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div style="margin-bottom: 5px;">
	<asp:LinkButton CssClass="link" ID="blnkNewInvoiceFee" runat="server" Text="New Profile Fee" OnClick="blnkNewInvoiceFee_Click" />
</div>
<asp:GridView ID="gvInvoiceFees" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="gridView"
	HorizontalAlign="Center" CellPadding="2" OnRowCommand="gvInvoiceFees_RowCommand">
	<RowStyle HorizontalAlign="Center" />
	<FooterStyle HorizontalAlign="Center" />
	<Columns>
		<asp:TemplateField HeaderText="From Amount">
			<ItemStyle HorizontalAlign="Right" />
			<ItemTemplate>
				<%# Eval("RangeAmountFrom", "{0:N2}") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="To Amount">
			<ItemStyle HorizontalAlign="Right" />
			<ItemTemplate>
				<%# Eval("RangeAmountTo", "{0:N2}") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Flat Fee">
			<ItemStyle HorizontalAlign="Right" />
			<ItemTemplate>
				<%# Eval("FlatFee", "{0:N2}") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Percent Fee">
			<ItemStyle HorizontalAlign="Right" />
			<ItemTemplate>
				<%# string.Format("{0:N2}%", Convert.ToDecimal(Eval("PercentFee")) * 100) %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Min. Amount">
			<ItemStyle HorizontalAlign="Right" />
			<ItemTemplate>
				<%# Eval("MinimumFee", "{0:N2}") %>
			</ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField HeaderText="Adjuster Payroll % for Flat/CAT">
		    <ItemStyle HorizontalAlign="Right" />
		    <ItemTemplate>
			    <%# Eval("FlatCatPercent") %>
		    </ItemTemplate>
	    </asp:TemplateField>
        <asp:TemplateField HeaderText="Adjuster Flat Fee for Flat/CAT">
		    <ItemStyle HorizontalAlign="Right" />
		    <ItemTemplate>
			    <%# Eval("FlatCatFee") %>
		    </ItemTemplate>
	    </asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ID") %>'
					ToolTip="Edit" ImageUrl="~/Images/edit.png" />
				&nbsp;
				                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
												 OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this fee?');"
												 CommandArgument='<%#Eval("ID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:Panel ID="pnlInvoiceFee" runat="server" Visible="false" DefaultButton="btnFeeSave">
	<table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm nowrap">
		<tr>
			<td class="right" style="width: 15%;">From Amount
			</td>
			<td>
				<ig:WebNumericEditor ID="txtAmountFrom" runat="server" DataMode="Decimal" MinDecimalPlaces="0" MinValue="0" />

			</td>
		</tr>
		<tr>
			<td class="right">To Amount
			</td>
			<td>
				<ig:WebNumericEditor ID="txtAmountTo" runat="server" DataMode="Decimal" MinDecimalPlaces="0" MinValue="0"></ig:WebNumericEditor>
			</td>
		</tr>
		<tr>
			<td class="right">Flat Fee
			</td>
			<td>
				<ig:WebNumericEditor ID="txtFlatFee" runat="server" DataMode="Decimal" MinDecimalPlaces="0" MinValue="0"></ig:WebNumericEditor>
			</td>
		</tr>
		<tr>
			<td class="right">Percentage Fee
			</td>
			<td>
				<ig:WebPercentEditor ID="txtPercentFee" runat="server"></ig:WebPercentEditor>
			</td>
		</tr>
		<tr>
			<td class="right">Minimum Amount
			</td>
			<td>
				<ig:WebNumericEditor ID="txtMinimumAmount" runat="server" DataMode="Decimal" MinDecimalPlaces="0" MinValue="0"></ig:WebNumericEditor>
			</td>
		</tr>
         <tr>
            <td class="right">Adjuster Payroll % for Flat/CAT</td>
          
            <td>
                <ig:WebPercentEditor ID="txtFlatCatPercent" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" MaxLength="100" Width="80px" />
                                                        
            </td>
        </tr>
        <tr>
                
                <td></td>
                <td>OR</td>
        </tr>
        <tr>
            <td class="right"> Adjuster Flat Fee for Flat/CAT</td>
          
            <td>
                <ig:WebNumericEditor ID="txtFlatCatFee" runat="server" DataMode="Decimal" MinDecimalPlaces="2" MinValue="0" MaxLength="100" Width="80px" />
                                                        
            </td>
        </tr>
		<tr>
			<td colspan="2"></td>
		</tr>
		<tr>
			<td></td>
			<td class="left">
				<asp:Button ID="btnFeeSave" runat="server" OnClick="btnSave_Click" Text="Save" CausesValidation="true" ValidationGroup="Fee" CssClass="mysubmit" />
				&nbsp;
                   <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="mysubmit" CausesValidation="false" />
			</td>
		</tr>
	</table>
</asp:Panel>
