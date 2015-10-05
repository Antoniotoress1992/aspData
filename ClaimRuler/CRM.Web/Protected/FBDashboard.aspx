<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FBDashboard.aspx.cs" MasterPageFile="~/Protected/ClaimRuler.Master" Inherits="CRM.Web.Protected.FBDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMiddArea" runat="server">
	<script type="text/javascript" src="../js/jquery.contextmenu.js"></script>
	<script type="text/javascript" src="../js/facebook.js"></script>
	<script type="text/javascript" src="../js/moment.min.js"></script>
	<script type="text/javascript" src="../js/globalize.js"></script>
	<script type="text/javascript" src="../js/globalize.culture.en-US.js"></script>
	<link rel="stylesheet" href="../Css/jquery.contextmenu.css" />
	<link  rel="stylesheet" href="../Css/fb-buttons.css" />
	<style type="text/css">
		.fb_wrapper
		{
			font-family: Verdana;
			width: 100%;
			font-size: 11px;
			margin: 0px auto;
		}

		.fb_container
		{
			border-radius: 5px;
			-moz-border-radius: 5px;
			-webkit-border-radius: 5px;
			border: 1px solid #d3d6db;
			vertical-align: top;
			font-size: 11px;
		}

		.right
		{
			text-align: right;
			vertical-align:top;
		}

		.friend
		{
			border: 1px solid #d3d6db;
		}


		.fb_container_header
		{
			background: #f2f2f2; /*#f6f7f8  f2f2f2*/
			border-bottom: 1px solid #d3d6db;
			padding: 10px 10px 10px 10px;
		}

		.fb_container_title
		{
			color: #6a7480; /* title color*/
			font-weight: bold;
			font-size: 12px;
		}

		.fb_container_content
		{
			background-color: white;
			padding: 10px 10px 10px 10px;
			color: #6a7480; /* text color */
		}



		.fb_container_box
		{
			border: 1px solid #d3d6db;
			vertical-align: top;
			font-family: Verdana;
			font-size: 11px;
		}

		.activity_textblock
		{
			padding-top: 10px;
			min-height: 58px;
		}

		.activity_thumb
		{
			background: none;
			display: inline-block;
			height: 64px;
			overflow: hidden;
			position: relative;
			vertical-align: middle;
			width: 64px;
		}

		.activity_thumb_empty
		{
			background: none;
			display: inline-block;
			height: 64px;
			overflow: hidden;
			position: relative;
			vertical-align: middle;
			width: 64px;
			border: 1px solid #d3d6db;
		}

		.activity_row
		{
			border-bottom: 1px solid #d3d6db;
		}

		.activity_timestamp
		{
			font-size: 0.8em;
		}

		.fb_table
		{
			border: 1px solid #d3d6db;
		}

		.fb_cell
		{
			vertical-align: top;
		}

		#blueBar
		{
			background-color: #3b5998;
			border-bottom: 1px solid #133783;
			position: relative;
			z-index: 300;
		}

		.shared_story
		{
			margin-left: 10px;
		}

		.error
		{						
			background-color: #ffc;
			border: 1px solid #c00;	
			text-align:left;		
		}
	</style>


	<div id="fb-root"></div>
	<div class="paneContent">
		<div class="page-title">
			Facebook Dashboard
		</div>

		<div class="paneContentInner">
			<div style="margin-top: 3px;">
				<div class="fb-login-button" data-max-rows="1" data-show-faces="true">Login with Facebook</div>
			</div>

			<div id="div_dashboard" class="fb_wrapper" style="display: none;">
				<div class="uibutton-toolbar" style="float: left; width: 100%; margin-top: 2px;">
					<div class="uibutton-group">
						<a class="uibutton icon add" href="javascript:createEvent();">Create Event</a>
						<a class="uibutton" href="javascript:logout();">Logout</a>

					</div>
				</div>


				<table>
					<tr>
						<td class="fb_cell">
							<!-- FRIENDS SECTION -->
							<div style="width: 300px; float: left" class="fb_container_box">
								<div class="fb_container_header">
									<span class="fb_container_title">My Friends</span>
									<span style="font-weight: normal;" id="friend_count"></span>
								</div>
								<div class="fb_container_content">
									<table id="tbl_friends">										
									</table>
								</div>

							</div>
						</td>
						<td class="fb_cell">
							<!-- WALL -->
							<div id="div_wall">
								<div class="fb_container_box" style="width: 400px;">
									<div class="fb_container_header">
										<span class="fb_container_title">Home</span>
										<div id="div_progress" style="float: right; padding-right: 10px;">
											<img runat="server" src="~/Images/loading_small.gif" />
										</div>
									</div>
									<div class="fb_container_content">
										<div id="div_feed">
										</div>
									</div>
								</div>
							</div>
						</td>
						<td class="fb_cell">
							<!-- TIMELINE -->

							<div class="fb_container_box" style="width: 350px;">
								<div class="fb_container_header">
									<span class="fb_container_title">My Timeline</span>
								</div>
								<div class="fb_container_content">
									<div id="div_timeline">
									</div>
								</div>
							</div>
						</td>

					</tr>
				</table>

			</div>
			<div id="edit_dialog" style="display: none;">

				<table style="border-collapse: separate; border-spacing: 9px; padding: 0px;" border="0">
					<tr>
						<td class="right top">Event Name</td>
						<td>
							<textarea rows="1" cols="50" id="txtEventName" name="txtEventName" class="required"></textarea>
						</td>
					</tr>
					<tr>
						<td class="right top">Description</td>
						<td>
							<textarea rows="5" cols="50" id="txtEventDescription" name="txtEventDescription" class="required"></textarea>
						</td>
					</tr>
					<tr>
						<td class="right top">Location</td>
						<td>
							<textarea rows="1" cols="50" id="txtEventLocation" name="txtEventLocation" class="required"></textarea>
						</td>
					</tr>
					<tr>
						<td class="right">Start Date/Time</td>
						<td>
							<input type="datetime" id="txtEventStartDate" name="txtEventStartDate" class="required" />
							&nbsp;
							<input type="datetime" id="txtStartTime" name="txtStartTime" />
						</td>
					</tr>
					<tr>
						<td class="right">End Date/Time</td>
						<td>
							<input type="datetime" id="txtEventEndDate" />
							&nbsp;
							<input type="datetime" id="txtEndTime" name="txtEndTime" />
						</td>
					</tr>
					<tr>
						<td></td>
						<td>
							<p style="font-size: smaller; color: red;">Note: All friends will be invited automatically.</p>
						</td>
					</tr>
				</table>

			</div>
		</div>
	</div>

	<script type="text/javascript">
		// create event

		
		function validateEventForm() {
			var isValid = true;

			if ($("#txtEventName").val() == '') {
				$("#txtEventName").attr('class', 'error');
				isValid = false;
			}
			if ($("#txtEventDescription").val() == '') {
				$("#txtEventDescription").attr('class', 'error');
				isValid = false;
			}
			if ($("#txtEventLocation").val() == '') {
				$("#txtEventLocation").attr('class', 'error');
				isValid = false;
			}
			
			return isValid;
		}
		$.widget("ui.timespinner", $.ui.spinner, {
			options: {
				// seconds
				step: 60 * 1000,
				// hours
				page: 60
			},

			_parse: function (value) {
				if (typeof value === "string") {
					// already a timestamp
					if (Number(value) == value) {
						return Number(value);
					}
					return +Globalize.parseDate(value);
				}
				return value;
			},

			_format: function (value) {
				return Globalize.format(new Date(value), "t");
			}
		});
		function createEvent() {
			$("#txtEventName").val('');
			$("#txtEventDescription").val('');
			$("#txtEventLocation").val('');
			$("#txtEventStartDate").val('');
			$("#txtStartTime").val('');
			$("#txtEventEndDate").val('');
			$("#txtEndTime").val('');

			$("#txtEventStartDate").datepicker();
			$("#txtEventEndDate").datepicker();

			var now = new Date();
			//$("#txtStartTime").val(now.toLocaleTimeString());
			$("#txtStartTime").timespinner();

			//$("#txtEndTime").val(now.toLocaleTimeString());
			$("#txtEndTime").timespinner();


			$('#edit_dialog').dialog
			    ({
			    	height: 'auto',
			    	width: 'auto',
			    	title: 'Create Event',
			    	modal: true,
			    	resizable: false,
			    	draggable: false,
			    	close: function (event, ui) { },
			    	buttons:
                    {
                    	'Post': function () {
                    		

                    		var txtEventName = $("#txtEventName").val();
                    		var txtEventDescription = $("#txtEventDescription").val();
                    		var txtEventLocation = $("#txtEventLocation").val();

                    		var txtEventStartDate = $("#txtEventStartDate").val();
                    		var txtEventStartTime = $("#txtStartTime").val();

                    		var startDateTime = null;
                    		var endDateTime = null;

                    		var txtEventEndDate = $("#txtEventEndDate").val();
                    		var txtEventEndTime = $("#txtEndTime").val();

                    		if ($.trim(txtEventStartTime) != '')
                    			startDateTime = new Date(txtEventStartDate + ' ' + txtEventStartTime);
                    		else
                    			startDateTime = new Date(txtEventStartDate);

                    		if ($.trim(txtEventEndDate) != '' && $.trim(txtEventEndTime) != '')
                    			endDateTime = new Date(txtEventEndDate + ' ' + txtEventEndTime);
                    		else if ($.trim(txtEventEndDate) != '' && $.trim(txtEventEndTime) == '')
                    			endDateTime = new Date(txtEventEndDate);

                    		if (validateEventForm()) {                    			
                    			$(this).dialog('close');
                    			postEvent(txtEventName, startDateTime, endDateTime, txtEventDescription, txtEventLocation)
                    		}                    		
                    	},
                    	'Cancel': function () {
                    		$(this).dialog('close');
                    	}
                    }
		});			
	}



	</script>
	<script>
		// init facebook api
		window.fbAsyncInit = function () {
			FB.init({
				appId: '384926558308677',
				status: true, // check login status
				cookie: true, // enable cookies to allow the server to access the session
				xfbml: true  // parse XFBML
			});

			// Here we subscribe to the auth.authResponseChange JavaScript event. This event is fired
			// for any authentication related change, such as login, logout or session refresh. This means that
			// whenever someone who was previously logged out tries to log in again, the correct case below 
			// will be handled. 
			FB.Event.subscribe('auth.authResponseChange', function (response) {
				// Here we specify what we do with the response anytime this event occurs. 
				if (response.status === 'connected') {
					// The response object is returned with a status field that lets the app know the current
					// login status of the person. In this case, we're handling the situation where they 
					// have logged in to the app.
					loadData();
				} else if (response.status === 'not_authorized') {
					// In this case, the person is logged into Facebook, but not into the app, so we call
					// FB.login() to prompt them to do so. 
					// In real-life usage, you wouldn't want to immediately prompt someone to login 
					// like this, for two reasons:
					// (1) JavaScript created popup windows are blocked by most browsers unless they 
					// result from direct interaction from people using the app (such as a mouse click)
					// (2) it is a bad experience to be continually prompted to login upon page load.

					//FB.login();
				} else {
					// In this case, the person is not logged into Facebook, so we call the login() 
					// function to prompt them to do so. Note that at this stage there is no indication
					// of whether they are logged into the app. If they aren't then they'll see the Login
					// dialog right after they log in to Facebook. 
					// The same caveats as above apply to the FB.login() call here.

					//FB.login();
				}
			});
		};

		// Load the SDK asynchronously
		(function (d) {
			var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
			if (d.getElementById(id)) { return; }
			js = d.createElement('script'); js.id = id; js.async = true;
			js.src = "//connect.facebook.net/en_US/all.js";
			ref.parentNode.insertBefore(js, ref);
		}(document));

		// Here we run a very simple test of the Graph API after login is successful. 
		// This testAPI() function is only called in those cases. 
		function loadData() {
			$("#div_dashboard").show();
			loadFriends();

			loadTimeline();

			loadWall();

		}


		function logout() {
			FB.logout(function (response) {
				window.location.reload();
			});

		}
	</script>

</asp:Content>
