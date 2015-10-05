using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;

using CRM.Core;
using CRM.Core.bingMapImageryService;
using CRM.Core.bingMapGeocodeService;

namespace CRM.Core {
	static public class bingMapHelper {
		private static bool checkRemoteResource(string url) {
			bool exists = false;
			try {
				HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
					exists = response.StatusCode == HttpStatusCode.OK;
				}
			}
			catch {
			}

			return exists;
		}

		private static Stream downloadImage(string resourceUrl) {
			System.IO.Stream stream = null;

			if (checkRemoteResource(resourceUrl)) {				

				System.Net.WebRequest req = System.Net.WebRequest.Create(resourceUrl);
				System.Net.WebResponse response = req.GetResponse();
				stream = response.GetResponseStream();
			}

			return stream;
		}

		public static Stream getMapImage(double? latitude, double? longitude) {
			const string map_credentials = "ApjKKP3xo-UePHYy4EWVj7kzRUAx40rbKSGGCzw-E_jK2YAtyhs5Na0_PunRgr2Y";
			bingMapImageryService.MapUriRequest mapUriRequest = new MapUriRequest();
			Stream imageStream = null;
		

			// Set credentials using a valid Bing Maps key
			mapUriRequest.Credentials = new bingMapImageryService.Credentials();
			mapUriRequest.Credentials.ApplicationId = map_credentials;	// "ApjKKP3xo-UePHYy4EWVj7kzRUAx40rbKSGGCzw-E_jK2YAtyhs5Na0_PunRgr2Y";		

					bingMapImageryService.Pushpin[] pins = new bingMapImageryService.Pushpin[1];
			bingMapImageryService.Pushpin pushpin = new bingMapImageryService.Pushpin();
			pushpin.Location = new bingMapImageryService.Location();
			pushpin.Location.Latitude = (double)latitude;
			pushpin.Location.Longitude = (double)longitude;
			pushpin.IconStyle = "2";
			pins[0] = pushpin;

			mapUriRequest.Pushpins = pins;

			//// Set the map style and zoom level
			bingMapImageryService.MapUriOptions mapUriOptions = new bingMapImageryService.MapUriOptions();
			mapUriOptions.Style = bingMapImageryService.MapStyle.AerialWithLabels;
			mapUriOptions.ZoomLevel = 16;

			// Set the size of the requested image to match the size of the image control
			mapUriOptions.ImageSize = new bingMapImageryService.SizeOfint();
			mapUriOptions.ImageSize.Height = 350;
			mapUriOptions.ImageSize.Width = 600;
			mapUriRequest.Options = mapUriOptions;

			bingMapImageryService.ImageryServiceClient imageryService = new bingMapImageryService.ImageryServiceClient("BasicHttpBinding_IImageryService");

			// Make the image request
			bingMapImageryService.MapUriResponse mapUriResponse = imageryService.GetMapUri(mapUriRequest);
			string mapUri = mapUriResponse.Uri;

			// get image from uri
			imageStream = downloadImage(mapUri);

			//if (imageStream != null) 
			//     mapImage = Image.FromStream(imageStream);

			return imageStream;
		}

		public static geoResponse geocodeAddress(string address) {
			

			geoResponse results = new geoResponse();
			
			results.message = "No Results Found";

			results.status = "fail";
			if (!string.IsNullOrEmpty(address.Trim())) {

				// get key from configuration
				string key = "ApjKKP3xo-UePHYy4EWVj7kzRUAx40rbKSGGCzw-E_jK2YAtyhs5Na0_PunRgr2Y";


				GeocodeRequest geocodeRequest = new GeocodeRequest();

				// Set the credentials using a valid Bing Maps key
				geocodeRequest.Credentials = new bingMapGeocodeService.Credentials();
				geocodeRequest.Credentials.ApplicationId = key;

				// Set the full address query
				geocodeRequest.Query = address;

				// Set the options to only return high confidence results 
				ConfidenceFilter[] filters = new ConfidenceFilter[1];
				filters[0] = new ConfidenceFilter();
				filters[0].MinimumConfidence = bingMapGeocodeService.Confidence.High;

				// Add the filters to the options
				GeocodeOptions geocodeOptions = new GeocodeOptions();
				geocodeOptions.Filters = filters;
				geocodeRequest.Options = geocodeOptions;

				// Make the geocode request
				GeocodeServiceClient geocodeService = new GeocodeServiceClient();
				GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);

				if (geocodeResponse.Results.Length > 0) {
					results.latitude = geocodeResponse.Results[0].Locations[0].Latitude.ToString();

					results.longitude = geocodeResponse.Results[0].Locations[0].Longitude.ToString();

					results.status = "ok";
				}
			}

			return results;
		}


	}
}
