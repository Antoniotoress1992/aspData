<%@ Page Title="" Language="C#"  MasterPageFile="~/Protected/AdminSite.master" AutoEventWireup="true"
	CodeBehind="LeadsImagesUploadDescription.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadsImagesUploadDescription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<h2>
		<asp:Label ID="lblHead" runat="server" Text="Property Damage Photos" />
	</h2>
	<br />
	<br />
	<asp:DataList ID="dlImagesLocationDescription" runat="server" DataKeyField="LeadImageId"
		OnItemDataBound="dlImagesLocationDescription_ItemDataBound">
		<ItemTemplate>
			<table border="0" cellspacing="0" cellpadding="0" width="80%" class="new_user" align="center">
				<tr>
					<td align="right" style="width: 35%; vertical-align: top;" nowrap="nowrap">
						Location of Photo/Damage in Property&nbsp;
					</td>
					<td align="left" valign="top">
						<asp:TextBox ID="txtLocation" runat="server" class="myinput1" MaxLength="250" Width="350px"
							Text='<%# Bind("Location") %>'></asp:TextBox>						
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td align="right" style="width: 35%; vertical-align: top; padding-top: 5px;">
						Full Description of Photo/Damage&nbsp;
					</td>
					<td align="left" valign="top" style="padding-top: 5px;">
						<asp:TextBox ID="txtDescription" runat="server" class="myinput1" MaxLength="1000"
							TextMode="MultiLine" Height="65px" Width="357px" Text='<%# Bind("Description") %>' />
						<br />
						&nbsp;&nbsp; <span style="color: Red; font-size: 10.5px;">(Please be as descriptive as
							possible)</span>						
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td align="right" valign="top">
						Image&nbsp;&nbsp;
					</td>
					<td align="left" valign="top">
						<div class="example" style="height: 380px; width: 480px; border: 3px solid #184687;
							border-radius: 5px;">
							<asp:Image ID="photoImage" class="imgzoom" runat="server" Width="480px" Height="380px" />
						</div>
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
			</table>
		</ItemTemplate>
	</asp:DataList>
	<div style="text-align: center;">
		<asp:Button ID="btnSaveContinue" runat="server" class="mysubmit" Text="Save and Continue"
			OnClick="btnSaveContinue_Click" />
	</div>
</asp:Content>

