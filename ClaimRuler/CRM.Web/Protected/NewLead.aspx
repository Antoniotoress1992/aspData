<%@ Page Title="" Language="C#" MasterPageFile="~/Protected/ClaimRuler.Master" AutoEventWireup="true"
	CodeBehind="NewLead.aspx.cs" Inherits="CRM.Web.Protected.NewLead" %>

<%@ MasterType VirtualPath="~/Protected/ClaimRuler.Master" %>

<%@ Register Src="~/UserControl/Admin/ucNewLead.ascx" TagName="ucNewLead" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">

	<uc1:ucNewLead ID="ucNewLead1" runat="server" />

	<script type="text/javascript">
		$(document).ready(function () {
			copySameAsOwner();
		});

		

		function WebForm_OnSubmit() {
			if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) {


				for (var i = 0; i < Page_Validators.length; i++) {
					try {
						var control = document.getElementById(Page_Validators[i].controltovalidate);
						if (!Page_Validators[i].isvalid) {
							control.className = "ErrorControl";

						} else {
							control.className = "";
						}
					} catch (e) { }
				}
				return false;
			}
			return true;
		}
	</script>

</asp:Content>
