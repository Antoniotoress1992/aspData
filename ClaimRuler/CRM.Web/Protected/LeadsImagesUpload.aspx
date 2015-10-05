<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRulerClaim.master" AutoEventWireup="true"
	CodeBehind="LeadsImagesUpload.aspx.cs" Inherits="CRM.Web.Protected.Admin.LeadsImagesUpload" %>

<%@ Register Assembly="Infragistics45.Web.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v14.1, Version=14.1.20141.2011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register Src="~/UserControl/Admin/UploadImages.ascx" TagName="UploadImages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ucClaimPhotos.ascx" TagName="ucClaimPhotos" TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/Protected/ClaimRulerClaim.master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
	Claim Photos
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<div class="center">
		<ig:WebUpload ID="WebUpload1" runat="server" Mode="Multiple" MultipleFiles="true"
			OnUploadFinishing="webUpload1_OnUploadFinishing" EnableTheming="true"
			>
			<ClientEvents FileSelected="webUpload_fileSelected" FileUploaded="webUpload_fileUploaded" />			
		</ig:WebUpload>
	</div>

	<div class="paneContentInner">
		<ajaxtoolkit:TabContainer ID="tabContainerPhotos" runat="server" Width="100%" ActiveTabIndex="0">
			<ajaxtoolkit:TabPanel ID="tabPanel1" runat="server">
				<HeaderTemplate>
					Current Photos						
				</HeaderTemplate>
				<ContentTemplate>
					<uc2:ucClaimPhotos ID="claimPhotos" runat="server" />
				</ContentTemplate>
			</ajaxtoolkit:TabPanel>
			<ajaxtoolkit:TabPanel ID="tabPanel2" runat="server">
				<HeaderTemplate>
					Previous Photos						
				</HeaderTemplate>
				<ContentTemplate>
					<uc1:UploadImages ID="legacyPhotos" runat="server" />
				</ContentTemplate>
			</ajaxtoolkit:TabPanel>
		</ajaxtoolkit:TabContainer>
	</div>
	<div id="div_photo_description" title="Photo Description" style="display: none;" class="editForm">
		<table style="width: 600px">
			<tr>
				<td class="right nowrap">Location of Photo/Damage in Property</td>
				<td>
					<ig:WebTextEditor ID="txtPhotoLocation" runat="server" MaxLength="100" Width="450px"></ig:WebTextEditor>

				</td>
			</tr>
			<tr>
				<td class="right top">Full Description of Photo/Damage</td>
				<td>
					<ig:WebTextEditor ID="txtPhotoDescription" runat="server" MaxLength="500" TextMode="MultiLine" Width="450px" MultiLine-Rows="10"></ig:WebTextEditor>

				</td>
			</tr>
		</table>

	</div>
	<script type="text/javascript">
		var fileCount = 0;
		
		
		//#region claim photo function
		function getClaimPhoto(claimImageID) {
			var params = "{'claimImageID' :'" + claimImageID + "'}";

			$.ajax({
				url: "LeadsImagesUpload.aspx/getClaimPhoto",   // Current Page, Method  
				data: params, 
				type: "POST", 
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					// convert to json object
					var claim = JSON.parse(result.d);
					// show data on UI
					$find("<%= txtPhotoLocation.ClientID%>").set_value(claim.location);
					$find("<%= txtPhotoDescription.ClientID%>").set_value(claim.description);
				},
				error: function (xhr, status) {
					alert(status + " - " + xhr.responseText);
				}
			});
		}

		function claimPhotoDescription(claimImageID) {
			getClaimPhoto(claimImageID);

			// show  dialog
			$("#div_photo_description").dialog({
				modal: true,
				width: 800,
				close: function (e, ui) {
					$(this).dialog('destroy');
				},
				buttons:
				   {
				   	'Save': function () {
				   		saveClaimPhotoDescription(claimImageID);
				   	},
				   	'Close': function () {
				   		$(this).dialog('close');
				   	}
				   }
			});
			return false;
		}

		// claim photo
		function saveClaimPhotoDescription(claimImageID) {
			var location = $find("<%= txtPhotoLocation.ClientID%>").get_value();
			var description = $find("<%= txtPhotoDescription.ClientID%>").get_value();

			if ($.trim(location) == '' && $.trim(description) == '') {
				$find("<%= txtPhotoLocation.ClientID%>").focus();
			}

			PageMethods.saveClaimPhotoDescription(claimImageID, location, description);

			$("#div_photo_description").dialog('close');

			// refresh
			window.location.reload(true);
		}
		//#endregion

		//#region legacy photo
		function getLeadPhoto(leadImageID) {
			var params = "{'leadImageID' :'" + leadImageID + "'}";

			$.ajax({
				url: "LeadsImagesUpload.aspx/getLeadPhoto",   // Current Page, Method  
				data: params,
				type: "POST",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					// convert to json object
					var claim = JSON.parse(result.d);
					// show data on UI
					$find("<%= txtPhotoLocation.ClientID%>").set_value(claim.location);
					$find("<%= txtPhotoDescription.ClientID%>").set_value(claim.description);
				},
				error: function (xhr, status) {
					alert(status + " - " + xhr.responseText);
				}
			});
		}

		function leadPhotoDescription(leadImageID) {
			getLeadPhoto(leadImageID);

			// show  dialog
			$("#div_photo_description").dialog({
				modal: true,
				width: 800,
				close: function (e, ui) {
					$(this).dialog('destroy');
				},
				buttons:
				   {
				   	'Save': function() {
				   		saveLeadPhotoDescription(leadImageID);
				   	},
				   	'Close': function () {
				   		$(this).dialog('close');
				   	}
				   }
			});
			return false;
		}

		function saveLeadPhotoDescription(leadImageID) {
			var location = $find("<%= txtPhotoLocation.ClientID%>").get_value();
			var description = $find("<%= txtPhotoDescription.ClientID%>").get_value();

			if ($.trim(location) == '' && $.trim(description) == '') {
				$find("<%= txtPhotoLocation.ClientID%>").focus();
			}

			PageMethods.saveLeadPhotoDescription(leadImageID, location, description);

			$("#div_photo_description").dialog('close');

			// refresh
			window.location.reload(true);
		}
		//#endregion

		function togglePrintOption(cbx, leadImageID) {
			try {
				PageMethods.togglePrintFlag(cbx.checked, leadImageID);
			}
			catch (err) {
				alert('Unable to set print flag on/off.');
			}
		}

		function webUpload_fileSelected(fileID, filePath) {
			++fileCount;
		}
		function webUpload_fileUploaded(fileID, filePath, totalSize) {
			--fileCount;

			if (fileCount == 0)
				RebindImages();
		}
	</script>
</asp:Content>
