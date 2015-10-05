<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentUpload.aspx.cs"
	Inherits="CRM.Web.Content.DocumentUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Claim Ruler - Industrial Strength Property Claim Management Software</title>
	<link href="../Css/ClaimRuler.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="../js/jquery-1.7.2.js"></script>
	<script type="text/javascript" src="../js/jquery-1.7.2.min.js"></script>
	<script type="text/javascript" src="../js/general.js"></script>
	<script type="text/javascript">
		function closeRefresh() {
			// call function in parent window		
			window.opener.refreshDocuments();
			window.close();

		}
		function getFileInfo() {
			// Mon Aug 26 2013 09:11:27 GMT-0400 (Eastern Daylight Time)
			var ofiles = document.getElementById("FileUpload2").files[0];
			var lastModifiedDate = ofiles.lastModifiedDate;	//.toString().substr(0, 24);

			//var fileDate = new Date(lastModifiedDate.toUTCString());

			//var dateTime = fileDate.toDateString() + " " + fileDate.toLocaleTimeString();
			//alert(fileDate.toTimeString());

			var yyyy = lastModifiedDate.getFullYear();
			var mm = lastModifiedDate.getMonth() + 1;
			var dd = lastModifiedDate.getDate();

			var hh = lastModifiedDate.getHours();
			var min = lastModifiedDate.getMinutes();
			var ss = lastModifiedDate.getSeconds();

			var strDate = mm + '/' + dd + '/' + yyyy + ' ' + hh + ':' + min + ':' + ss + ' ' + (hh > 12 ? 'PM' : 'AM');

			//var dateTime = fileDate.toUTCString();

			$("#<%= hf_lastModifiedDate.ClientID %>").val(strDate);
		}

		$(document).ready(function () {
			$('#<%= txtDescriptionDoc.ClientID %>').keyup(function () {
				var $this = $(this);
				if ($this.val().length > 500) {
					$this.val($this.val().substr(0, 500));
					alert('Maximum number of characters reached!');
				}
			});
		});

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:HiddenField ID="hf_lastModifiedDate" runat="server" />
		<div class="page-title">
			Upload Document 
		</div>

		<div id="dvDocument" style="display: block;">
			<br />
			<table border="0" cellspacing="0" cellpadding="0" width="800px" class="editForm"
				align="center">
				<tr>
					<td colspan="2">&nbsp;
					</td>
				</tr>
				<tr>
					<td class="right top" style="width: 20%; ">Description&nbsp;&nbsp;
					</td>
					<td>&nbsp;&nbsp;<asp:TextBox ID="txtDescriptionDoc" runat="server" TextMode="MultiLine"
						Rows="6" Width="500px" />
						<Br />&nbsp;&nbsp; <span style="color: white; font-size: 10.5px;">(Please be as descriptive as
						possible)</span>
						<div>
							&nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
								ControlToValidate="txtDescriptionDoc" ErrorMessage="Please enter document description."
								Display="Dynamic" SetFocusOnError="true" CssClass="validation1" ValidationGroup="DocUpload" />
						</div>
						<div>
							<asp:RegularExpressionValidator ID="txtValidator1" ControlToValidate="txtDescriptionDoc"
								Text="1000 characters maximum allowed." ValidationExpression="^[\s\S]{0,1000}$" runat="server"
								ValidationGroup="DocUpload" CssClass="validation1" SetFocusOnError="true" />
						</div>					
						
					</td>
				</tr>
				<tr>
					<td colspan="2">&nbsp;
					</td>
				</tr>
				<tr>
					<td class="right top">Select Document&nbsp;&nbsp;
					</td>
					<td class="top">&nbsp;&nbsp;<asp:FileUpload ID="FileUpload2" runat="server" onchange="javascript:getFileInfo();" />
						<div>
							&nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="FileUpload2"
								ErrorMessage="Please select document to upload." Display="Dynamic" SetFocusOnError="true"
								CssClass="validation1" ValidationGroup="DocUpload" />
						</div>
					</td>
				</tr>
				<tr>
					<td colspan="2">&nbsp;
					</td>
				</tr>
				<tr>
					<td class="center top" colspan="2">
						<asp:Button ID="btnUploadDoc" runat="server" ValidationGroup="DocUpload" class="mysubmit"
							Text="Upload" OnClick="btnUploadDoc_Click" />
					</td>
				</tr>
				<tr>
					<td colspan="2" class="center"></td>
				</tr>
			</table>
		</div>
	</form>
</body>
</html>
