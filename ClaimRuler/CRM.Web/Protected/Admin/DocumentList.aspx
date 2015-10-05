<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true"
	CodeBehind="DocumentList.aspx.cs" Inherits="CRM.Web.Protected.Admin.DocumentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<h2>
		Document List Management</h2>
	<div>
		<asp:Label ID="lblMessage" runat="server" Visible="false" Text="Session expired. Please re-login." />
	</div>
	<asp:Panel ID="pnlList" runat="server">
		<br />
		<table cellpadding="2" cellspacing="3" style="width: 100%;" border="0">
			<tr>
				<td style="width: 15%">
					Document Type
				</td>
				<td align="left">
					<asp:DropDownList ID="ddlPolicyType" runat="server" OnSelectedIndexChanged="ddlPolicyType_SelectedIndex"
						AutoPostBack="true">
						<asp:ListItem Text="Select One" Value="0"></asp:ListItem>
						<asp:ListItem Text="Homeowners" Value="1"></asp:ListItem>
						<asp:ListItem Text="Commercial" Value="2"></asp:ListItem>
						<asp:ListItem Text="Flood" Value="3"></asp:ListItem>
						<asp:ListItem Text="Earthquake" Value="4"></asp:ListItem>
					</asp:DropDownList>
				</td>
				<td>
				</td>
				<td align="right">
					
						Document Name &nbsp;
						<asp:TextBox ID="txtDocument" runat="server" MaxLength="100" Width="300px" />
				</td>
				<td>
						<asp:Button ID="btnSave" runat="server" Text="Save" class="mysubmit" OnClick="btnSave_click" />
																									
				</td>
			</tr>
			<tr>
				<td></td>
				<td></td>
				<td></td>
				<td align="right">
					<asp:RequiredFieldValidator ID="rftxtDocument" runat="server" SetFocusOnError="true"
							Display="Dynamic" ErrorMessage="Please enter document name." ControlToValidate="txtDocument"
							ValidationGroup="NewDocument" CssClass="validation1" />
				</td>
			</tr>
		</table>
		<asp:GridView ID="gvDocumentList" CssClass="Tables" ShowFooter="true" Width="99%"
			OnRowCommand="gvDocumentList_RowCommand" runat="server" AutoGenerateColumns="False"
			CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff" HeaderStyle-Font-Size="11px"
			PageSize="10" RowStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="5"
			PagerStyle-Font-Bold="true" PagerStyle-Font-Size="9pt">
			<PagerStyle CssClass="pager" />
			<EmptyDataTemplate>
				No documents available.
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField HeaderText="Document Name" HeaderStyle-BackColor="#e8f2ff" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<asp:Label ID="lblDocumentName" runat="server" Text='<%# Eval("DocumentName") %>' />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" HeaderStyle-BackColor="#e8f2ff"
					ItemStyle-Wrap="false">
					<ItemTemplate>
						<asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("DocumentListId") %>'
							ToolTip="Edit" ImageUrl="~/Images/edit.png"></asp:ImageButton>
						&nbsp;
						<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this document?');"
							CommandArgument='<%#Eval("DocumentListId") %>' ToolTip="Delete" ImageUrl="~/Images/delete_icon.png">
						</asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</asp:Panel>
</asp:Content>
