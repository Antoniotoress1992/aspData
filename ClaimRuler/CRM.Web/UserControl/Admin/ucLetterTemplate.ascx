<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLetterTemplate.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.ucLetterTemplate" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>

<div class="page-title">
	Letter Templates
</div>
<div class="toolbar toolbar-body">
	<table>
		<tr>
			<td>
				<asp:LinkButton ID="btnNew" runat="server" Text="Save" CssClass="toolbar-item" OnClick="btnNew_Click" 
                        Visible='<%# Convert.ToBoolean(masterPage.hasAddPermission) %>'>
					<span class="toolbar-img-and-text" style="background-image: url(../../images/add.png)">New Letter Template</span>
				</asp:LinkButton>
			</td>
			<td></td>
		</tr>
	</table>
</div>
<div class="paneContentInner">
	<asp:Panel ID="pnlGrid" runat="server" Visible="true">
		<div>
			<a href="../../Content/CRMFields.xlsx">Download MailMerge Fields</a>
		</div>


		<asp:GridView ID="gvLetterTemplate" Width="99%" runat="server" OnRowCommand="gvLetterTemplate_RowCommand" CssClass="gridView"
			AutoGenerateColumns="False" CellPadding="4" AlternatingRowStyle-BackColor="#e8f2ff"
			PageSize="10" RowStyle-HorizontalAlign="Center">
			<EmptyDataTemplate>
				No letter templates available.
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField HeaderText="Template File Name" HeaderStyle-BackColor="#e8f2ff"
					ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("Path")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Template Description" HeaderStyle-BackColor="#e8f2ff"
					ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# Eval("Description")%>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="45px" HeaderStyle-BackColor="#e8f2ff"
					ItemStyle-Wrap="false">
					<ItemTemplate>
						<asp:ImageButton ID="btnEdit" runat="server" 
                            CommandName="DoEdit" 
                            CommandArgument='<%#Eval("TemplateID") %>'
							ToolTip="Edit" 
                            Visible='<%# masterPage.hasEditPermission %>'
                            ImageUrl="~/Images/edit.png"></asp:ImageButton>
						&nbsp;
					<asp:ImageButton ID="btnDelete" runat="server" 
                        CommandName="DoDelete" 
                        OnClientClick="javascript:return ConfirmDialog(this,  'Are you sure you want to delete this template?');"
						CommandArgument='<%#Eval("TemplateID") %>' 
                        ToolTip="Delete" 
                        Visible='<%# masterPage.hasDeletePermission %>'
                        ImageUrl="~/Images/delete_icon.png"></asp:ImageButton>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</asp:Panel>
	<asp:Panel ID="pnlUpload" runat="server" Visible="false">
		<igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="700px" TitleAlignment="Left" Text="Template Details" CssClass="canvas">			
			<Template>
					<table style="width: 100%;  border:none; border-spacing: 5px; padding: 2px;" border="0" class="editForm">
					<tr>
						<td>Template Description
						</td>
						<td>
							<span class="redstar">*</span>
						</td>
						<td >
							<asp:TextBox ID="txtDescription" runat="server" MaxLength="100" Width="400px" />
							<asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
									ErrorMessage="Required" ValidationGroup="newTemplate" />
						</td>
					</tr>
					<tr>
						<td>Template File Name
						</td>
						<td style="width: 5px"></td>
						<td>
							<asp:Label ID="lblFilename" runat="server" />
						</td>
					</tr>
					<tr>
						<td></td>
						<td></td>
						<td></td>
					</tr>
					<tr>
						<td></td>
						<td></td>
						<td>
							<asp:FileUpload ID="fileUpload" runat="server" Width="300px" />
							<div>
								<asp:CustomValidator ID="cvalAttachment" runat="server" ControlToValidate="fileUpload"
									ValidationGroup="newTemplate" SetFocusOnError="true" ErrorMessage="Only MS Word document files are allowed."
									CssClass="validation1" ClientValidationFunction="UploadFileCheck">
								</asp:CustomValidator>
							</div>
						</td>
					</tr>
					<tr>
						<td colspan="3" style="text-align:center;">
							<asp:Button ID="btnUpload" runat="server" Text="Save" OnClick="btnUpload_Click"
								class="mysubmit" ValidationGroup="newTemplate" />
							&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_click"
								class="mysubmit" CausesValidation="false" />
						</td>
					</tr>
				</table>
			</Template>
		</igmisc:WebGroupBox>

	</asp:Panel>
</div>
<asp:HiddenField ID="hfID" runat="server" Value="0" />
<script type="text/javascript">
	function UploadFileCheck(source, arguments) {
		var sFile = arguments.Value;
		arguments.IsValid = sFile.endsWith('.doc') || sFile.endsWith('.docx');
	}
</script>
