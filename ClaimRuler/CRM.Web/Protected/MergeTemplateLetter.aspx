<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true"
	CodeBehind="MergeTemplateLetter.aspx.cs" Inherits="CRM.Web.Protected.MergeTemplateLetter" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
	Letters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
			
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

	<asp:GridView ID="gvLetter" Width="50%" runat="server" 
        AllowSorting="true"
        AlternatingRowStyle-BackColor="#e8f2ff"
		AutoGenerateColumns="False" 
        CssClass="gridView"
        CellPadding="4" 
        HorizontalAlign="Center"
        OnRowCommand="gvLetter_RowCommand" 
		PageSize="15"
		PagerSettings-PageButtonCount="5" 
        PagerStyle-Font-Bold="true" 
		PagerStyle-Font-Size="9pt" >
		<PagerStyle CssClass="pager" />
		<RowStyle HorizontalAlign="Center" />
		<EmptyDataTemplate>
			No letters available.
		</EmptyDataTemplate>
		<Columns>
			<asp:TemplateField HeaderText="Letter Description" HeaderStyle-BackColor="#e8f2ff"
				ItemStyle-HorizontalAlign="Center">
				<ItemTemplate>
					<%# Eval("Description")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" HeaderStyle-BackColor="#e8f2ff"
				ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton ID="btnPrint" runat="server" CommandName="DoPrint" CommandArgument='<%#Eval("TemplateID") %>'
						ToolTip="Print" ImageUrl="~/Images/print_icon.png"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>

</asp:Content>
