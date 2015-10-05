<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="ClientList.aspx.cs" Inherits="CRM.Web.Protected.Admin.ClientList" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="paneContent">
		<div class="page-title">
			Clients
		</div>

		<div class="toolbar toolbar-body">
			<table>
				<tr>
					<td>
						<asp:LinkButton ID="btnNew" runat="server" Text="" CssClass="toolbar-item" OnClick="btnNew_Click">
									<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Client</span>
						</asp:LinkButton>
					</td>
				</tr>
			</table>
		</div>
		<div class="paneContentInner">
			<asp:GridView ID="gvClients" CssClass="gridView" OnRowCommand="gvClients_RowCommand"
				Width="99%" runat="server" AutoGenerateColumns="False" CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff"
				AllowPaging="true" PageSize="100" OnPageIndexChanging="gvClients_PageIndexChanging"
				PagerSettings-PageButtonCount="5" PagerStyle-Font-Bold="true"
				AllowSorting="true"
				OnSorting="gvClients_onSorting">
				<PagerStyle CssClass="pager" />
				<RowStyle HorizontalAlign="Center" />
				<HeaderStyle HorizontalAlign="Center" />
				<EmptyDataTemplate>
					No clients available.
				</EmptyDataTemplate>
				<Columns>
                    <asp:BoundField HeaderText="ID" DataField="ClientId"  SortExpression="ClientId" />
					<asp:BoundField HeaderText="First Name" DataField="FirstName"  SortExpression="FirstName" />
					<asp:BoundField HeaderText="Last Name" DataField="LastName"  SortExpression="LastName" />
					<asp:TemplateField HeaderText="Company"  SortExpression="BusinessName">
						<ItemTemplate>
							<%#Eval("BusinessName")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Phone"  SortExpression="PrimaryPhoneNo">
						<ItemTemplate>
							<%#Eval("PrimaryPhoneNo")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Email"  SortExpression="PrimaryEmailId">
						<ItemTemplate>
							<%#Eval("PrimaryEmailId")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Is Trail" >
						<ItemTemplate>
							<%# Eval("isTrial") == null ? false : Eval("isTrial")%>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField ItemStyle-Width="45px"  ItemStyle-Wrap="false">
						<ItemTemplate>
							<asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ClientId") %>'
								ToolTip="Edit" ImageUrl="~/Images/people_edit.png"></asp:ImageButton>
							&nbsp;
					<asp:ImageButton ID="btnNew" runat="server" CommandName="DoNew" ToolTip="New Client"
						ImageUrl="~/Images/people_new.png"></asp:ImageButton>
							&nbsp;
					<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete" OnClientClick="javascript:return confirm('Are you sure you want to delete this client?');"
						CommandArgument='<%#Eval("ClientId") %>' ToolTip="Delete Client" ImageUrl="~/Images/people_delete.png"></asp:ImageButton>
							&nbsp;
					<asp:ImageButton ID="btnImpresonateClient" runat="server" CommandName="DoImpersonate" ToolTip="Login as client"
						CommandArgument='<%#Eval("ClientId") %>' ImageUrl="~/Images/SearchPng.png"></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</div>
	</div>
</asp:Content>
