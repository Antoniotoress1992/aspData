<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FB.aspx.cs" Inherits="CRM.Web.Content.FB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Claim Ruler - Industrial Strength Property Claim Management Software</title>
	<link type="text/css" rel="stylesheet" href="../Css/ClaimRuler.css" />
	<link type="text/css" rel="Stylesheet" href="../Css/cupertino/jquery-ui-1.9.2.custom.css" />

	<link rel="shortcut icon" href="/favicon.ico" />
	<link rel="icon" href="favicon.ico" type="image/x-icon" />

</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" EnablePageMethods="true">
			<Scripts>
				<asp:ScriptReference Path="~/js/jquery-1.8.3.js" />
				<asp:ScriptReference Path="~/js/jquery-ui-1.9.2.custom.js" />
				<asp:ScriptReference Path="~/js/reminder.js" />
				<asp:ScriptReference Path="~/js/general.js" />
				<asp:ScriptReference Path="~/js/facebook.js" />
			</Scripts>
		</asp:ScriptManager>
		<script type="text/javascript">
			$(function () {
				InitialiseFacebook('1438260343072387');


			});

			function postLike() {
				var objectToLike = 'http://techcrunch.com/2013/02/06/facebook-launches-developers-live-video-channel-to-keep-its-developer-ecosystem-up-to-date/';

				FB.api(
				   'https://graph.facebook.com/me/og.likes',
				   'post',
				   {
				   	object: objectToLike,
				   	privacy: { 'value': 'SELF' }
				   },
				   function (response) {
				   	if (!response) {
				   		alert('Error occurred.');
				   	} else if (response.error) {
				   		document.getElementById('result').innerHTML =
						  'Error: ' + response.error.message;
				   	} else {
				   		document.getElementById('result').innerHTML =
						  '<a href=\"https://www.facebook.com/me/activity/' +
						  response.id + '\">' +
						  'Story created.  ID is ' +
						  response.id + '</a>';
				   	}
				   }
				);
			}
		</script>
		<div id="fb-root"></div>
		<div>
			<fb:login-button autologoutlink="true" perms="read_friendlists, create_event, email, publish_stream">Login to Facebook</fb:login-button>
			<asp:Label ID="token" runat="server" />
		</div>
		<br />
		<fb:user id="1305661220"></fb:user>
		<div class="fb-post" data-href="https://www.facebook.com/FacebookDevelopers/posts/10151471074398553"></div>
		<br />
		<div class="fb-activity" data-app-id="1438260343072387" data-site="claimruler.com" data-action="likes, recommends" data-colorscheme="light" data-header="true"></div>
		<br />
		<div>
			This example creates a story on Facebook using the
		<a href="https://developers.facebook.com/docs/reference/ogaction/og.likes">
			<code>og.likes</code></a> API.  That story will just say that you like an
		<a href="http://techcrunch.com/2013/02/06/facebook-launches-developers-live-video-channel-to-keep-its-developer-ecosystem-up-to-date/">article on TechCrunch</a>.  The story should onlybe visible to you.
		</div>
		<div>
			<input
				type="button"
				value="Create a story with an og.likes action"
				onclick="postLike();" />
		</div>

		<div id="result"></div>
	</form>
</body>
</html>
