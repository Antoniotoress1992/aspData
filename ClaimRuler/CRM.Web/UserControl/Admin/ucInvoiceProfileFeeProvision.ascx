<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInvoiceProfileFeeProvision.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucInvoiceProfileFeeProvision" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<div style="margin-bottom: 5px;">
	<asp:LinkButton CssClass="link" ID="lbtnNewProvision" runat="server" Text="New Pricing Provision" OnClick="lbtnNewProvision_Click" />
</div>
<asp:GridView ID="gvFeeProvisions" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="gridView"
	HorizontalAlign="Center" CellPadding="2" OnRowCommand="gvFeeProvisions_RowCommand">
	<RowStyle HorizontalAlign="Center" />
	<FooterStyle HorizontalAlign="Center" />
	<Columns>
		<asp:TemplateField HeaderText="Provision Text">
			<ItemTemplate>
				<%# Eval("ProvisionText") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Fee Amount">
			<ItemStyle HorizontalAlign="Right" Width="80px" />
			<ItemTemplate>
				<%# Eval("ProvisionAmount", "{0:N2}") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemStyle HorizontalAlign="Center" Width="50px" />
			<ItemTemplate>
				<asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ID") %>'
					ToolTip="Edit" ImageUrl="~/Images/edit.png" />
				&nbsp;
				<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete"
					OnClientClick="javascript:return ConfirmDialog(this, 'Are you sure you want to delete this provision?');"
					CommandArgument='<%#Eval("ID") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:Panel ID="pnlFeeProvision" runat="server" Visible="false" DefaultButton="btnProvisionSave">
	<table style="width: 100%; border-collapse: separate; border-spacing: 7px; padding: 2px; text-align: left;" border="0" class="editForm nowrap">
		<tr>
			<td></td>
			<td>
				<asp:Label ID="lblProvisionMessage" runat="server" CssClass="error" Visible="false" />
			</td>
		</tr>
		<tr>
			<td></td>
			<td></td>
		</tr>
		<tr>
			<td class="right top" style="width: 15%;">Provision Text
			</td>
			<td style="width: 5px" class="redstar top">*</td>
			<td>
				<ig:WebTextEditor ID="txtProvisionName" runat="server" TextMode="MultiLine" MultiLine-Rows="5" Width="400px"></ig:WebTextEditor>
				<div>
					<asp:RequiredFieldValidator ID="rfvProvisionName" runat="server" ControlToValidate="txtProvisionName" CssClass="validation1"
						SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please enter provision text." ValidationGroup="Provision" />
				</div>
			</td>
			<tr>
				<td class="right">Fee Amount
				</td>
				<td class="redstar">*</td>
				<td>
					<ig:WebNumericEditor ID="txtProvisionAmount" runat="server" DataMode="Decimal" MinDecimalPlaces="0" MinValue="0"></ig:WebNumericEditor>
					<div>
						<asp:RequiredFieldValidator ID="rfvProvisionAmount" runat="server" ControlToValidate="txtProvisionAmount" CssClass="validation1"
							SetFocusOnError="true" Display="Dynamic" ErrorMessage="Please enter amount." ValidationGroup="Provision" />
					</div>
				</td>
			</tr>
		<tr>
			<td></td>
			<td></td>
		</tr>
		<tr>
			<td></td>
			<td></td>
			<td>
				<asp:Button ID="btnProvisionSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="Provision" CssClass="mysubmit" OnClick="btnProvisionSave_Click" />
				&nbsp;
				<asp:Button ID="btnProvisionCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="mysubmit" OnClick="btnProvisionCancel_Click" />
			</td>
		</tr>
	</table>
</asp:Panel>
