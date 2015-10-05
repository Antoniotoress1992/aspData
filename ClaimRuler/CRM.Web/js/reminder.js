function getURL(){
	var url = '';
	if (document.location.host == 'localhost')
		url = document.location.origin + "/" + document.location.pathname.split('/')[1];
	else
		url = document.location.origin;	// http://stage.claimruler.com

	return url;
}
function checkForReminders() {
	var url = getURL() + '/Content/ReminderPopUp.aspx/checkForReminders';
	$.ajax({
		type: "POST",
		url: url,		//"../Content/ReminderPopUp.aspx/checkForReminders",
		data: "{}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (hasReminders) {
			if (hasReminders.d == "1")
			    openReminderDialog();

			
		}
	});
}

function clearSession() {
	var url = getURL() + '/Content/ReminderPopUp.aspx/clearReminderFromSession';
	$.ajax({
		type: "POST",
		url: url,	//"../Content/ReminderPopUp.aspx/clearReminderFromSession",
		contentType: "application/json; charset=utf-8",
		dataType: "json"		
	});
}

function closeReminderDialog() {
	if ($("#div_reminder").dialog("isOpen")) {
		window.parent.$('#div_reminder').dialog('close');
	}
}

function openAppointmentForm(id) {
	// 1. open appointment form
	PopupCenter("../Content/AppointmentForm.aspx?id=" + id, "Appointment", 700, 650);

	// 2. then close dialog
	closeReminderDialog();
}

function openReminderDialog() {
	var url = getURL() + '/Content/ReminderPopUp.aspx';
	try {
		$('body').find('#div_reminder').remove();		
		$('body').append("<div id='div_reminder' style='display:none;'><iframe src='" + url + "' seamless='seamless' width='600' height='400'></iframe></div>");

		var dialog1 = $("#div_reminder").dialog({
			title: 'Reminder',
			autoOpen: false,
			width: 625,
			modal: true,
			close: function (e) {
				$('body').find('#div_reminder').remove();
				clearSession();
			}
		});
		dialog1.dialog("open");
	}
	catch (ex) {
	}
}

var soundObject = null;
function playAlarmSound() {
	if (soundObject != null) {
		document.body.removeChild(soundObject);
		soundObject.removed = true;
		soundObject = null;
	}
	soundObject = document.createElement("embed");
	soundObject.setAttribute("src", "http://stage.claimruler.com/Sounds/alarm.mp3");
	soundObject.setAttribute("hidden", true);
	soundObject.setAttribute("autostart", true);
	document.body.appendChild(soundObject);
}