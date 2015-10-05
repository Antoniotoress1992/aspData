<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCarrierChannel.ascx.cs" Inherits="CRM.Web.UserControl.Admin.ucCarrierChannel" %>

<asp:GridView ID="gvChannels" CssClass="gridView" 
	Width="100%" runat="server" AutoGenerateColumns="False" CellPadding="2" AlternatingRowStyle-BackColor="#e8f2ff">

	<RowStyle HorizontalAlign="Center" />
	<HeaderStyle HorizontalAlign="Center" />
	<EmptyDataTemplate>
		No channels available.
	</EmptyDataTemplate>
	<Columns>
		<asp:TemplateField HeaderText="Channel Type">
			<ItemTemplate>
				<%#Eval("ChannelType.ChannelName")%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="">
			<ItemTemplate>
				<%#Eval("ChannelData")%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField ItemStyle-Width="45px" ItemStyle-Wrap="false">
			<ItemTemplate>
				<asp:ImageButton ID="btnEdit" runat="server" CommandName="DoEdit" CommandArgument='<%#Eval("ChannelID") %>'
					ToolTip="Edit" ImageUrl="~/Images/edit.png"></asp:ImageButton>			
				&nbsp;
					<asp:ImageButton ID="btnDelete" runat="server" CommandName="DoDelete" OnClientClick="javascript:return confirm(this, 'Are you sure you want to delete this channel?');"
						CommandArgument='<%#Eval("ChannelID") %>' ToolTip="Delete Client" ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>

