<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUploadCSV.ascx.cs"
	Inherits="CRM.Web.UserControl.Admin.ucUploadCSV" %>
<div class="paneContent">
	<div class="page-title">
		Upload File 
	</div>
	<asp:HiddenField ID="hfLeadsId" runat="server" Value="0" />
	<div class="message_area">
		<asp:Label ID="lblError" runat="server" CssClass="error" Visible="false" />
		<asp:Label ID="lblSave" runat="server" CssClass="ok" Visible="false" />
		<asp:Label ID="lblMessage" runat="server" CssClass="info" Visible="false" />
	</div>
	<div class="paneContentInner">
		<table style="width: 100%; border-collapse: separate; border-spacing: 5px; padding: 2px; text-align: left;" border="0">
			<tr>
				<td colspan="2" class="left top" style="height: 40px;">
					<div style="margin: 10px 0px 10px 0px;">
						<p>
							Please follow these steps to import your data:<br />
							<br />
						</p>

						<ul>
							<li>1. <a href="../../Content/New_Import_Template.xls">Download Import Template.</a></li>
							<li>2. Fill in your data.</li>
							<li>3. Save spreadsheet as CSV(Comma delimited).</li>
							<li>4. Choose file saved in Step 3.</li>
							<li>5. Click Upload button.</li>
						</ul>

					</div>

				</td>
			</tr>
			<tr>
				<td colspan="2" class="right"></td>
			</tr>

			<tr>
				<td colspan="2" class="right"></td>
			</tr>
			<tr>

				<td colspan="2" class="center top">&nbsp;&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" ValidationGroup="propImages" />
					<div>
						<asp:RequiredFieldValidator Display="Dynamic" CssClass="validation1"
							ID="RequiredFieldValidator2" runat="server" ErrorMessage="You have not uploaded your file"
							ValidationGroup="ImgUpload" ControlToValidate="FileUpload1"></asp:RequiredFieldValidator>
					</div>					
				</td>
			</tr>
			<tr>
				<td colspan="2" class="center top">
					<br />
						<br />
						<asp:Button ID="btnUpload" runat="server" ValidationGroup="ImgUpload" class="mysubmit"
							Text="Upload" OnClick="btnUpload_Click" />
				</td>
			</tr>			
		</table>
	</div>
</div>
