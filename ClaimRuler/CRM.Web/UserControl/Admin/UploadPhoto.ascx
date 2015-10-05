<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadPhoto.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.UploadPhoto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<script type="text/javascript" src="../../js/jquery-1.7.2.js"></script>

<link rel="stylesheet" href="../../css/default.css">
<style type="text/css">
	.style1
	{
		width: 92px;
	}
</style>
<script type="text/javascript">

</script>
<%--<head id="Head1" runat="server" />--%>
<div id="mainbox" style="font-size: small;">
	<!--
    <h2>
        <asp:Label ID="lblheading" runat="server" Text="Upload Document" />
	</h2>
	-->
	<h2>
		Claim Photo Manager, Report Printing, Letter Printing
	</h2>
	<div align="center" style="display: inline-block; width: 100%;">
		<asp:Button ID="btnComments" runat="server" Text="Return to Claim" class="mysubmit"
			OnClick="btnReturnToClaim_Click" />&nbsp;&nbsp; &nbsp;&nbsp;
		<asp:Button ID="btnPhotos" runat="server" Text="Photo Manager" class="mysubmit" Visible="True"
			OnClick="btnPhotos_Click" />
		&nbsp;&nbsp; &nbsp;&nbsp;
		<asp:Button ID="btnGenerateReport" runat="server" Text="Print Photo Report" class="mysubmit"
			OnClick="btnGenerateReport_Click" Visible="false" />
		&nbsp;&nbsp; &nbsp;&nbsp;
		<asp:Button ID="btnGenerateRepLetter" runat="server" Text="Print Letter of Rep" class="mysubmit"
			Visible="True" OnClick="btnGenerateRepLetter_Click" />
	</div>
	<h2>
		<asp:Label ID="lblHead" runat="server" Text="Claim Documents<br/>Policies, Estimates, Repair Contracts, Signed Adjusting Contracts, Signed Retainers,<br/> Scope of Loss, Completed Estimates, Etc." />
		<asp:HiddenField ID="hfLeadsId" runat="server" Value="0" />
		<asp:HiddenField ID="hfView" runat="server" Value="0" />
	</h2>
	<div style="height: 2px;">
	</div>
	<div align="center" style="border: 1px solid #e8f2ff">
		<div style="padding-top: 10px; padding-bottom: 10px;">
			<asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
			<asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
			<asp:Label ID="lblMessage" runat="server" CssClass="error" Visible="false" />
		</div>
		<div id="dvEdit" runat="server">
			<br />
			<div id="dvDocument" style="display: block;">
				<table border="0" cellspacing="0" cellpadding="0" width="800px" class="new_user"
					align="center">
					<tr>
						<td colspan="2">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td align="right" style="width: 30%;">
							Description&nbsp;&nbsp;
						</td>
						<td align="left" valign="top">
							&nbsp;&nbsp;<asp:TextBox ID="txtDescriptionDoc" MaxLength="1000" runat="server" TextMode="MultiLine"
								Height="65px" Width="350px"></asp:TextBox>
							<br />
							&nbsp;&nbsp; <span style="color: Red; font-size: 10.5px;">(Please be as descriptive as
								possible)</span>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							Select Document&nbsp;&nbsp;
						</td>
						<td align="left" valign="top">
							&nbsp;&nbsp;<asp:FileUpload ID="FileUpload2" runat="server" ValidationGroup="propImages" />
							&nbsp;&nbsp;<asp:RequiredFieldValidator Display="Dynamic" CssClass="validation1"
								ID="RequiredFieldValidator1" runat="server" ErrorMessage="You have not uploaded your file"
								ValidationGroup="DocUpload" ControlToValidate="FileUpload2"></asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td valign="top" colspan="2" align="center">
							<asp:Button ID="btnUploadDoc" runat="server" ValidationGroup="DocUpload" class="mysubmit"
								Text="Upload Documents" OnClick="btnUploadDoc_Click" />
							&nbsp;&nbsp;<asp:Button ID="btnCancelDoc" runat="server" CausesValidation="false"
								class="mysubmit" Text="Cancel" OnClientClick="javascript:return canceldoc();" />
						</td>
					</tr>
					<tr>
						<td colspan="2" align="center">
						</td>
					</tr>
				</table>
			</div>
			<br />
		</div>		
		<br />
	</div>
	<div align="left" style="border: 1px solid #e8f2ff">
		<table border="0" cellspacing="0" cellpadding="0" width="95%" class="new_user">
			<tr>
				<td colspan="2" align="center">
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:Panel ID="Panel1" runat="server">
						<div style="height: 25px; font-weight: bold; text-align: left; font-size: 20px;">
							<h2>
								Property Insurance Claim Documents</h2>
						</div>
						<div style="height: 15px;">
						</div>
						<asp:GridView Width="99%" ID="gvDocuments" BorderColor="#e8f2ff" runat="server" AutoGenerateColumns="False"
							ShowFooter="true" EmptyDataText="No Record Found !!!" DataKeyNames="LeadDocumentId"
							CellPadding="4" OnRowCommand="gvDocuments_RowCommand">
							<Columns>
								<asp:TemplateField ItemStyle-Width="30%" HeaderText="Document">
									<ItemTemplate>
										<a id="A1" href='<%# "\\LeadsDocument\\" + Convert.ToString(Eval("LeadID")) + "\\"+ Convert.ToString(Eval("LeadDocumentId")) + "\\" +Convert.ToString(Eval("DocumentName"))  %>'
											target='_blank'>
											<asp:Label ID="lblDoc" runat="server" Text='<%#Eval("DocumentName") %>' />
										</a>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Description" ItemStyle-Width="50%">
									<ItemTemplate>
										<asp:HiddenField ID="hfLeadId" runat="server" Value='<%#Eval("LeadId") %>' />
										<%#Eval("Description")%>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Date" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<%#Eval("InsertDate")%>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
									<ItemTemplate>
										<asp:ImageButton runat="server" ID="btnDelete" Text="Delete" CommandName="DoDelete"
											ImageUrl="../../Images/DeleteRed.png" CommandArgument='<%# Eval("LeadDocumentId") %>'
											OnClientClick="javascript:return confirm('Do you want to delete this record ?')"
											ToolTip="Delete" />
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</asp:Panel>
				</td>
			</tr>
		</table>
		<br />
	</div>
</div>
