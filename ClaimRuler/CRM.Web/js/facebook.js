

// Turn off shadows for all context menus  
$.contextMenu.shadow = false;

String.Format = function () {
	var s = arguments[0];
	for (var i = 0; i < arguments.length - 1; i++) {
		var reg = new RegExp("\\{" + i + "\\}", "gm");
		s = s.replace(reg, arguments[i + 1]);
	}
	return s;
}

function postEvent(eventName, startTime, endTime, description, location) {
	var event = {};

	event["name"] = eventName;
	event["description"] = description;
	event["location"] = location;
	event["privacy_type"] = "FRIENDS";
	event["start_time"] = startTime.toISOString();

	if (endTime != null) 
		event["end_time"] = endTime.toISOString();

	// privacy_type {'OPEN','SECRET','FRIENDS','CLOSED'}

	//FB.api('/me/events','post',{ 
	//	name: eventName,
	//	start_time: startTime,
	//	//end_time: endTime,
	//	description: description,
	//	location: location,
	//	privacy_type: "FRIENDS"
	//},	
	FB.api('/me/events', 'post', event,
		function (response) {
			if (response.id > 0) {					
				inviteAllFriendsToEvent(response.id);
			}		
	});
}
function inviteAllFriendsToEvent(eventID) {
	FB.api('/me/friends', function (response) {
		$.each(response.data, function (i, friend) {
			FB.api('/' + eventID + '/invited/' + friend.id, 'post', function (isInvited) {
				if (isInvited) {
				}
				else {
				}
			});
		});
		// refresh screen
		window.location.reload(true);
	});
}

function postToMyWall(txtCaption,  txtLink) {
	var params =
		{
			method: 'feed',
			link: txtLink,
			caption: txtCaption,			
			display: 'iframe'
		}

	FB.ui(params, function (response) {
		if (response) {
			if (response.id) {
				alert("Posted as " + response.post_id);
			}
			else {
				alert("Error");
			}
		}
	});

	// The id of the post will be in response.post_id
	function onPostToWallCompleted(response) {
		// Just show error message if there's an error

		// user cancelled
	}
}


function makePostToFee_old(msg) {
	var params = {};
	params['message'] = msg;
	params['name'] = 'Name';
	params['description'] = 'this is a description';
	params['link'] = 'http://www.somelink.com/page.htm';
	params['picture'] = 'http://www.somelink.com/img/pic.jpg';
	params['caption'] = 'Caption of the Post';

	FB.api('/me/feed', 'post', params, function (response) {
		if (!response || response.error) {
			// an error occured
			alert(JSON.stringify(response.error));
		} else {
			// Done
			alert('Published to stream');
		}
	});
}


/***************************************************************************************
*	Wall/Home section
****************************************************************************************/
function loadWall() {


	var html = "<table><tbody>";
	var feed = new Array();
	
	FB.api('/me?fields=feed,friends.fields(feed)', function (response) {
		$.each(response.feed.data, function (i, o) {
			feed.push(o);
		});

		// get friends' feed
		$.each(response.friends.data, function (i, o) {
			$.each(o.feed.data, function (i, d) {
				feed.push(d);
			});
		});

		// sort all feed by date
		feed.sort(function (a, b) {
			var dateA = new Date(a.created_time), dateB = new Date(b.created_time)
			//return dateA - dateB //sort by date ascending
			return dateB - dateA //sort by date descending
		})
	
	//FB.api('/me?fields=feed,friends.fields(feed)', function (response) {
	//	$.each(response.data, function (i, fb) {
	$.each(feed, function (i, fb) {
			var imgURL = '';
			var msg = '';
			html += "<tr class='activity_row'>";

			var now = new Date();
			var timestamp = moment(new Date(fb.created_time)).format('MMMM Do YYYY, h:mm a');
			//var timestamp = timeDifference(now, new Date(fb.created_time));

			switch (fb.type) {
				case 'link':
				case 'photo':
				case 'video':
					html += "<td>";
					html += String.Format("<a href='{0}' target='_blank' class='activity_thumb'>", fb.link);
					
					if (fb.picture != null)
						imgURL = fb.picture;
					else
						imgURL = String.Format('http://graph.facebook.com/{0}/picture', fb.from.id);

					html += String.Format("<img width='64px' alt='' src='{0}'/>", imgURL);

					html += "</td>";
					if (fb.story != null)
						msg = fb.story;
					if (fb.name != null)
						msg = fb.name;

					
					html += "<td><div class='activity_textblock'>" + msg;
					html += "<p class='activity_timestamp'>" + timestamp + "</p>";
					html += "</div></td>";
					break;
				

				case 'status':
					html += "<td><span class='activity_thumb'>";
					html += String.Format("<img width='64px' alt='' src='{0}/{1}/picture' /></span>", 'http://graph.facebook.com', fb.from.id);
					msg = $.trim(fb.story) != '' ? fb.story : fb.message;
					html += "<td><div class='activity_textblock'>" + msg;
					html += "<p class='activity_timestamp'>" + timestamp + "</p>";
					html += "</div></td>";
					break;

			}


			html += "</tr>";
		});
		html += "</tbody></table>";

		$('#div_feed').html(html);
		feed = null;

		$("#div_progress").hide();
	});

	

}
/***************************************************************************************
*	timeline section
****************************************************************************************/
function loadTimeline() {
	var html = "<table><tbody>";


	FB.api('/me/feed?limit=10', function (response) {
		$.each(response.data, function (i, fb) {
			var imgURL = '';
			var msg = '';
			html += "<tr class='activity_row'>";

			var now = new Date();
			var timestamp = moment(new Date(fb.created_time)).format('MMMM Do YYYY, h:mm a');
			//var timestamp = timeDifference(now, new Date(fb.created_time));

			switch (fb.type) {
				case 'link':
				case 'photo':
				case 'video':
					html += "<td>";
					html += String.Format("<a href='{0}' target='_blank' class='activity_thumb'>", fb.link);

					if (fb.picture != null)
						imgURL = fb.picture;
					else
						imgURL = String.Format('http://graph.facebook.com/{0}/picture', fb.from.id);

					html += String.Format("<img width='64px' alt='' src='{0}'/>", imgURL);

					html += "</td>";
					if (fb.story != null)
						msg = fb.story;
					if (fb.name != null)
						msg = fb.name;


					html += "<td><div class='activity_textblock'>" + msg;
					html += "<p class='activity_timestamp'>" + timestamp + "</p>";
					html += "</div></td>";
					break;


				case 'status':
					html += "<td><span class='activity_thumb'>";
					html += String.Format("<img width='64px' alt='' src='{0}/{1}/picture' /></span>", 'http://graph.facebook.com', fb.from.id);
					msg = $.trim(fb.story) != '' ? fb.story : fb.message;
					html += "<td><div class='activity_textblock'>" + msg;
					html += "<p class='activity_timestamp'>" + timestamp + "</p>";
					html += "</div></td>";
					break;

			}


			html += "</tr>";
		});
		html += "</tbody></table>";

		$('#div_timeline').html(html);
	});
}
/***************************************************************************************
*	friends section
****************************************************************************************/
function sendMessage(menuItem, menu) {
	// get user id from "img id" attribute
	var uid = menu.target.attributes['id'].value;
	FB.ui({
		method: 'send',
		to: uid,
		link: 'http://www.nytimes.com/2011/06/15/arts/people-argue-just-to-win-scholars-assert.html',
	});
}

function visitFriendsPage(menuItem, menu) {
	// get user id from "img id" attribute
	var uid = menu.target.attributes['id'].value;
	var friendURL = "https://www.facebook.com/" + uid.toString();

	window.open(friendURL);
}

function loadFriends() {
	var FriendMenu = [
				  { "Visit Friend's Page": function (menuItem, menu) { visitFriendsPage(menuItem, menu); } },
				  { 'Send Message': function (menuItem, menu) { sendMessage(menuItem, menu); } }
	];

	FB.api('/me/friends', function (response) {
		var friend_count = " . " + response.data.length.toString();

		//remove existing tr
		$("#tbl_friends tr").remove();

		for (var i = 0; i < response.data.length; i++) {
			var imgUrl = String.Format("<img id='{1}' alt='picture' src='https://graph.facebook.com/{0}/picture' class='friend'></img>", response.data[i].id.toString(), response.data[i].id.toString());

			var friendLink = String.Format("href='{0}/{1}'", "https://www.facebook.com", response.data[i].id.toString());

			var tr = "<tr>"
				+ "<td><a target='_blank' " + friendLink + ">" + imgUrl + "</a></td>"
				+ "<td>" + response.data[i].name + "</td>"
				+ "</tr>";
			$("#tbl_friends").append(tr);

			var friendControlID = "#" + response.data[i].id.toString();

			$(friendControlID).contextMenu(FriendMenu, { theme: 'xp' });

			//var uid = response.data[i].id;
			//$("#" + response.data[i].id).contextMenu(function () {
			//	var o1 = {}; o1["Visit Friend's Page"] = function () { visitFriedsPage(uid); };
			//	var o2 = {}; o1["Send Message"] = function () { sendMessage(uid); };
			//	return [o1, o2];
			//	}
			//	, { theme: 'xp' }
			//);
		}
		$("#friend_count").html(friend_count);
	});
}

function timeDifference(laterdate,earlierdate) {
	var elapsedTime = '';

	var difference = laterdate.getTime() - earlierdate.getTime();
 
	var daysDifference = Math.floor(difference/1000/60/60/24);
	difference -= daysDifference*1000*60*60*24
 
	var hoursDifference = Math.floor(difference/1000/60/60);
	difference -= hoursDifference*1000*60*60
 
	var minutesDifference = Math.floor(difference/1000/60);
	difference -= minutesDifference*1000*60
 
	var secondsDifference = Math.floor(difference/1000);

	if (daysDifference > 0)
		elapsedTime += daysDifference + ((daysDifference > 1)? ' days ' : ' day ');

	if (hoursDifference > 0)
		elapsedTime += hoursDifference + ' hours ';

	if (minutesDifference > 0)
		elapsedTime += minutesDifference + ' minutes ';

	if (secondsDifference > 0)
		elapsedTime += secondsDifference + ' seconds';


	return elapsedTime;
	//return ('difference = ' + daysDifference + ' day/s ' + hoursDifference + ' hour/s ' + minutesDifference + ' minute/s ' + secondsDifference + ' second/s ');
}