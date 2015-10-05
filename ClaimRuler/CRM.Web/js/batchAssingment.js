var map = null, infobox, dataLayer;
var pinCount = 0;
var pos_left = 0;
var pos_top = 0;
var isFullsize = false;
var layer_claims;
var layer_adjusters;

function LoadMap() {
	// Initialize the map
	map = new Microsoft.Maps.Map(document.getElementById("myMap"),
			 {
			 	credentials: credentials,
			 	showDashboard: false,
			 	center: new Microsoft.Maps.Location(35.471303, -96.228675),
			 	zoom: 4
			 });

	// create layers
	layer_claims = new Microsoft.Maps.EntityCollection();
	layer_adjusters = new Microsoft.Maps.EntityCollection();

	map.entities.push(layer_claims);
	map.entities.push(layer_adjusters);

	// handle double click on map
	attachdblclick = Microsoft.Maps.Events.addHandler(map, 'dblclick', resize_fullscreen);
	ShowAdjusters();
	ShowLossAddresses();
}

function ShowAdjusters() {
	var hf_adjusters = $("[id$='hf_adjusters']").val().split('$');
	if (hf_adjusters.length > 0) {
		
		adjusters = new Array(hf_adjusters.length - 1);

		// compensate for last $ in string
		for (var i = 0; i < hf_adjusters.length - 1; i++) {
			var nameAddressFields = hf_adjusters[i].split('|');

			
			var adjusterName = nameAddressFields[0];
			var adjusterStreetAddress = nameAddressFields[1];
			
			var latlong = nameAddressFields[2].split(',');

			var latitude = latlong[0];
			var longitude = latlong[1];

			var handlePolicyTypes = '';
			var infoboxHeight = 0;

			if (nameAddressFields[3].length > 0) {
				var policyType = nameAddressFields[3].split(':');

				handlePolicyTypes = '<br/><b>Claim Types Handled:</b></br>';
				
				if (policyType.length > 0) {
					handlePolicyTypes += "<div style='height: 80px; border:1px solid silver; padding:2px; overflow: auto;'>"
					for (var p = 0; p < policyType.length; p++) {
						if (policyType[p] != '') {
							handlePolicyTypes += policyType[p] + '<br/>';
							infoboxHeight += 20;
						}
					}
					handlePolicyTypes += "</div>";
				}
			}

			// Add pushpin 
			// coordinates
			var position = new Microsoft.Maps.Location(latitude, longitude);
			var pushpin = new Microsoft.Maps.Pushpin(position, { icon: 'http://app.claimruler.com/images/pushpin_adjuster.png' });

			pushpin.Title = adjusterName;
			pushpin.Description = adjusterStreetAddress + '<br/>' + handlePolicyTypes;

			//map.entities.push(pushpin);
			layer_adjusters.push(pushpin);

			// InfoBox
			var infoboxOptions = {
				offset: new Microsoft.Maps.Point(0, 32),
				height: 80 + infoboxHeight,
				visible: false
			};
			var myInfobox = new Microsoft.Maps.Infobox(position, infoboxOptions);
			layer_adjusters.push(myInfobox);

			//Set event handler to handle pushpin show/hide
			Microsoft.Maps.Events.addHandler(pushpin, 'click', function (e) {
				myInfobox.setLocation(e.target.getLocation());
				myInfobox.setOptions({
					visible: true,
					title: e.target.Title,
					description: e.target.Description 
				});
			});
		}
	}
}


function ShowLossAddresses() {
	// get loss address
	var lossLocations = $("[id$='lossLocation']");

	var protocol = window.location.protocol;
		pinCount = 0;
		for (var i = 0; i < lossLocations.length; i++) {
			var lossAddress =lossLocations[i].value.split('|');			

			var geocodeRequest = protocol + "//dev.virtualearth.net/REST/v1/Locations?query=" + encodeURI(lossAddress[1]) + "&maxResults=1&output=json&jsonp=GeocodeCallback&key=" + credentials;

			// Call the service
			CallRestService(geocodeRequest);

			++pinCount;
		}
	
}

function GeocodeCallback(result) {
	// Check that we have a valid response
	if (result && result.resourceSets && result.resourceSets.length > 0 && result.resourceSets[0].resources && result.resourceSets[0].resources.length > 0) {
		

		// Create a Location based on the geocoded coordinates
		var coords = result.resourceSets[0].resources[0].point.coordinates;
		var lossAddress = result.resourceSets[0].resources[0].address.formattedAddress;

		// coordinates
		var position = new Microsoft.Maps.Location(coords[0], coords[1]);
		//var title = insureds[pinCount];

		// Add pushpin 
		var pushpin = new Microsoft.Maps.Pushpin(position);
		//pushpin.Title = title;
		pushpin.Description = lossAddress;
		layer_claims.push(pushpin);

		// InfoBox
		var infoboxOptions = {			
			offset: new Microsoft.Maps.Point(0, 32),
			height:80,
			visible: false
		};
		var myInfobox = new Microsoft.Maps.Infobox(position, infoboxOptions);
		//map.entities.push(myInfobox);
		layer_claims.push(myInfobox);

		//Set event handler to handle pushpin show/hide
		Microsoft.Maps.Events.addHandler(pushpin, 'click', function (e) {
			myInfobox.setLocation(e.target.getLocation());
			myInfobox.setOptions({
				visible: true,
				title: e.target.Title,
				description: e.target.Description
			});
		});

		
	}
}


function pinMouseOver(e) {
	var pin = e.target;
	if (pin != null) {
		boxes[pin.getText()].setOptions({ visible: true });
	}
}

function hideInfobox(e) {
	var pin = e.target;
	var infobox = boxes[pin.getText()];
	if (pin != null) {
		infobox.setOptions({ visible: false });
	}
}

function CallRestService(request) {
	var script = document.createElement("script");
	script.setAttribute("type", "text/javascript");
	script.setAttribute("src", request);
	var dochead = document.getElementsByTagName("head").item(0);
	dochead.appendChild(script);
}

function resize_fullscreen() {
	pos_left = $("#myMap").offset().left;
	pos_top = $("#myMap").offset().top;	

	$('#myMap').css({
		position: 'absolute',
		top: 0,
		left: 0,
		width: $(window).width(),
		height: $(window).height()
	});
	isFullsize = true;

	$("#myMap").attr("title", "Press ESC to exit");

	map.setView({ zoom: 4, center: new Microsoft.Maps.Location(31.220388, -109.687660) });
}
function resetMapSize() {
	$('#myMap').css({
		position: 'relative',
		width: '700px',
		height: '450px'
	});

	$("#myMap").appendTo("#div_mapPlaceHolder");

	$("myMap").title = "Double click for full screen";
	map.setView({ zoom: 4, center: new Microsoft.Maps.Location(37.779294, -97.533697) });
}
	