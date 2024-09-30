using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CalculateDistance;
using CalculateDistance.DistanceMatrix;
using CalculateDistance.Geocoding;
using CalculateDistance.StaticMaps;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Annotations;

namespace SearchAddressMap
{
	public partial class Window1 : Window
	{
		private const string GoogleApiKey = "AIzaSyBPKSA5nsy139rMFU27BhUzABDWtY6roNk"; // Use a secure method to store API key
		private List<Location> locations = new List<Location>(); 
		private Location destinationLocation;
		private List<Location> delocation = new List<Location>();
		private List<HQItem> resultList = new List<HQItem>();

		public Window1()
		{
			InitializeComponent();
			ShowHQList();
			GoogleSigned.AssignAllServices(new GoogleSigned(GoogleApiKey));
			loadingImage.Visibility = Visibility.Visible;
			_ = GetAddressesAsync();
			//loadingImage.Visibility = Visibility.Collapsed;
		}

		private void ShowHQList(int distanceValue = 0, int feeValue = 0)
		{
			HQListView.ItemsSource = null;
			HQListView.Items.Clear();
			var items = new List<HQItem>
			{
                // Add HQ items here...
                new HQItem { HQName = "700 Franklin Avenue, Franklin Square NY 11010", Distance = 0, Fee = 0 },
				new HQItem { HQName = "9 Forts Ferry Rd, Latham, NY 1211", Distance = 0, Fee = 0 },
				new HQItem { HQName = "661 Pleasant St, Southington CT 06489", Distance = 0, Fee = 0 },
				new HQItem { HQName = "13 Peck St, Attleboro, MA 02703", Distance = 0, Fee = 0 },
				new HQItem { HQName = "1919 Woodbridge Ave, Edison, NJ 08817", Distance = 0, Fee = 0 },
				new HQItem { HQName = "860 Candia Rd, Manchester, NH 03109", Distance = 0, Fee = 0 },
				new HQItem { HQName = "3300 Wilmington Rd, New Castle, PA 16105", Distance = 0, Fee = 0 },
				new HQItem { HQName = "123 W Washington St Oswego, IL  60543", Distance = 0, Fee = 0 },
				new HQItem { HQName = "701 N Market St, Wilmington, DE 19801", Distance = 0, Fee = 0 },
				new HQItem { HQName = "1916 Willow ln Woodbridge VA 22191", Distance = 0, Fee = 0 },
				new HQItem { HQName = "28 Prospect St Barre, VT 05649", Distance = 0, Fee = 0 },
				new HQItem { HQName = "22 Monument Square Portland, ME  04101", Distance = 0, Fee = 0 },
				new HQItem { HQName = "127 S Illinois St, Indianapolis, IN 46225", Distance = 0, Fee = 0 },
				new HQItem { HQName = "2328 E Main St, Springfield, OH 45503", Distance = 0, Fee = 0 },
				new HQItem { HQName = "3709 Payne Ave, Cleveland, OH 44114", Distance = 0, Fee = 0 },
				new HQItem { HQName = "2416 W State St, Milwaukee, WI 53233", Distance = 0, Fee = 0 },
				new HQItem { HQName = "804 Town Blvd A1010 Atlanta GA 30319", Distance = 0, Fee = 0 },
				new HQItem { HQName = "726 N Cumberland St, Metairie, LA, 70003", Distance = 0, Fee = 0 },
				new HQItem { HQName = "900 E 11th street Austin Texas 7870", Distance = 0, Fee = 0 },
				new HQItem { HQName = "7120 Bramblebush Dr Frisco TX 75033", Distance = 0, Fee = 0 },
				new HQItem { HQName = "2502 N 50 St, Tampa, FL, 33619", Distance = 0, Fee = 0 },
				new HQItem { HQName = "4120 Hollister Pl Jacksonville, FL, 32257", Distance = 0, Fee = 0 },
				new HQItem { HQName = "1121 Holland Drive Boca Raton 33487", Distance = 0, Fee = 0 },
				new HQItem { HQName = "144 E 23rd St, Panama City, FL 32405", Distance = 0, Fee = 0 },
				new HQItem { HQName = "1119 Lily Loch Way, Durham NC 27703", Distance = 0, Fee = 0 },
				new HQItem { HQName = "5727 N Sharon Amity Rd, Charlotte, NC 28215", Distance = 0, Fee = 0 },
				new HQItem { HQName = "2149 Young Ave, Memphis, TN 38104", Distance = 0, Fee = 0 },
				new HQItem { HQName = "1560 S Lewis St, Anaheim, CA 92805", Distance = 0, Fee = 0 },
				new HQItem { HQName = "4369 Fairwood Street, Fremont, CA 94538", Distance = 0, Fee = 0 },
				new HQItem { HQName = "3245 El Cajon Blvd, San Diego, CA, 92104", Distance = 0, Fee = 0 },
				new HQItem { HQName = "4220 Florin rd Sacramento CA  95823", Distance = 0, Fee = 0 },
				new HQItem { HQName = "6446 E Albany St, Mesa, AZ 85205", Distance = 0, Fee = 0 },
				new HQItem { HQName = "8230 SE Harrison St #345, Portland, OR 97216", Distance = 0, Fee = 0 }
                // ... other items ...
            };

			HQListView.ItemsSource = items;
		}

		private void RefreshImage()
		{
			if (locations != null && locations.Any())
			{
				RefreshMap(locations, image1);

			}
			//else
			//{
			//	MessageBox.Show("No locations available to display on the map.");
			//}
		}

		private void RefreshMap(IEnumerable<Location> locations, Image imageControl)
		{
			if (!locations.Any()) throw new ArgumentException("At least one location must be provided.");

			// Validate and convert zoomSlider.Value
			int zoomValue;
			try
			{
				zoomValue = Convert.ToInt32(zoomSlider.Value);
			}
			catch (OverflowException)
			{
				throw new ArgumentException("Zoom value is out of range for an Int32.");
			}

			// Validate and convert imageControl.Width
			int widthValue;
			try
			{
				widthValue = Convert.ToInt32(imageControl.Width);
			}
			catch (OverflowException)
			{
				throw new ArgumentException("Image control width is out of range for an Int32.");
			}

			// Validate and convert imageControl.Height
			int heightValue;
			try
			{
				heightValue = Convert.ToInt32(imageControl.Height);
			}
			catch (OverflowException)
			{
				throw new ArgumentException("Image control height is out of range for an Int32.");
			}

			var request = new StaticMapRequest
			{
				Center = locations.First(),
				Zoom = zoomValue,
				Size = new MapSize(widthValue, heightValue),
				MapType = (MapTypes)Enum.Parse(typeof(MapTypes), ((ComboBoxItem)mapTypeComboBox.SelectedItem).Content.ToString(), true)
			};

			foreach (var location in locations) request.Markers.Add(location);

			var mapSvc = new StaticMapService();
			var imageSource = new BitmapImage();
			imageSource.BeginInit();
			imageSource.StreamSource = mapSvc.GetStream(request);
			imageSource.CacheOption = BitmapCacheOption.OnLoad;
			imageSource.EndInit();

			imageControl.Source = imageSource;

			// Force a layout update
			imageControl.UpdateLayout();
		}


		private async Task GetAddressesAsync()
		{
			var destinationAddress = destinationTextBox.Text;
			var hqAddresses = new List<string>
			{
                // Add HQ addresses here...
                "700 Franklin Avenue, Franklin Square NY 11010",
				"9 Forts Ferry Rd, Latham, NY 1211",
				"661 Pleasant St, Southington CT 06489",
				"13 Peck St, Attleboro, MA 02703",
				"1919 Woodbridge Ave, Edison, NJ 08817",
				"860 Candia Rd, Manchester, NH 03109",
				"3300 Wilmington Rd, New Castle, PA 16105",
				"123 W Washington St Oswego, IL  60543",
				"701 N Market St, Wilmington, DE 19801",
				"1916 Willow ln Woodbridge VA 22191",
				"28 Prospect St Barre, VT 05649",
				"22 Monument Square Portland, ME  04101",
				"127 S Illinois St, Indianapolis, IN 46225",
				"2328 E Main St, Springfield, OH 45503",
				"3709 Payne Ave, Cleveland, OH 44114",
				"2416 W State St, Milwaukee, WI 53233",
				"804 Town Blvd A1010 Atlanta GA 30319",
				"726 N Cumberland St, Metairie, LA, 70003",
				"900 E 11th street Austin Texas 7870",
				"7120 Bramblebush Dr Frisco TX 75033",
				"2502 N 50 St, Tampa, FL, 33619",
				"4120 Hollister Pl Jacksonville, FL, 32257",
				"1121 Holland Drive Boca Raton 33487",
				"144 E 23rd St, Panama City, FL 32405",
				"1119 Lily Loch Way, Durham NC 27703",
				"5727 N Sharon Amity Rd, Charlotte, NC 28215",
				"2149 Young Ave, Memphis, TN 38104",
				"1560 S Lewis St, Anaheim, CA 92805",
				"4369 Fairwood Street, Fremont, CA 94538",
				"3245 El Cajon Blvd, San Diego, CA, 92104",
				"4220 Florin rd Sacramento CA  95823",
				"6446 E Albany St, Mesa, AZ 85205",
				"8230 SE Harrison St #345, Portland, OR 97216"
                // ... other addresses ...
            };

			var geocodingService = new GeocodingService();
			foreach (var address in hqAddresses)
			{
				var request = new GeocodingRequest { Address = address };
				var response = await geocodingService.GetResponseAsync(request);

				if (response.Status == ServiceResponseStatus.Ok)
				{
					foreach (var result in response.Results)
					{
						var newLocation = result.Geometry.Location;
						locations.Add(newLocation);
						Console.WriteLine($"{result.FormattedAddress}: ({result.Geometry.Location.Latitude}, {result.Geometry.Location.Longitude})");
					}
				}
				else
				{
					Console.WriteLine($"Geocoding failed for {address}: {response.Status}");
				}
			}
			
			RefreshMap(locations, image1);
		}

		private async void searchButton_Click(object sender, RoutedEventArgs e)
		{
			var destinationAddress = destinationTextBox.Text;
			if (string.IsNullOrEmpty(destinationAddress))
			{
				MessageBox.Show("Please enter a destination address.");
				return;
			}

			var request = new GeocodingRequest { Address = destinationAddress };
			var response = await new GeocodingService().GetResponseAsync(request);

			if (response.Status == ServiceResponseStatus.Ok)
			{
				destinationLocation = response.Results.First().Geometry.Location;
				//Console.WriteLine($"{response.Results.First().FormattedAddress}: ({destinationLocation.Latitude}, {destinationLocation.Longitude})");
			}
			else
			{
				Console.WriteLine($"Geocoding failed for {destinationAddress}: {response.Status}");
				return;
			}

			await CalculateDistancesAsync();

		}
		private double CalculateFee(double distanceInMiles)
		{
			if (distanceInMiles <= 50)
			{
				return 0;
			}
			else if (distanceInMiles <= 74)
			{
				return 50;
			}
			else if (distanceInMiles <= 99)
			{
				return 75;
			}
			else if (distanceInMiles <= 149)
			{
				return 100;
			}
			else
			{
				return 150;
			}
		}
		private async Task CalculateDistancesAsync()
		{
			HQListView.ItemsSource = null;
			resultList.Clear();

			var distanceMatrixService = new DistanceMatrixService(new GoogleSigned(GoogleApiKey));
			foreach (var address in locations)
			{
				var requestDistance = new DistanceMatrixRequest();
				requestDistance.AddOrigin(address);
				requestDistance.AddDestination(destinationLocation);
				requestDistance.Mode = TravelMode.driving;

				var responseDistance = await distanceMatrixService.GetResponseAsync(requestDistance);

				if (responseDistance.Status == ServiceResponseStatus.Ok)
				{
					try
					{
						long distanceText = responseDistance.Rows.First().Elements.First().distance.Value;
						//double distanceInMeters = ConvertDistanceTextToDouble(distanceText);
						double distanceInMiles = ConvertMetersToMiles(distanceText);

						var calculatedItem = new HQItem
						{
							HQName = responseDistance.OriginAddresses.First().ToString(),
							Distance = distanceInMiles,
							Fee = CalculateFee(distanceInMiles)// Replace with actual fee calculation
						};

						resultList.Add(calculatedItem);
					}
					catch (FormatException ex)
					{
						Console.WriteLine($"Failed to parse distance: {ex.Message}");
					}
				}

				else
				{
					Console.WriteLine($"Distance calculation failed for {address}: {responseDistance.Status}");
				}
			}

			// Update UI with results
			HQListView.ItemsSource = resultList;
		}
		// Convert the distance from meters to miles
		double ConvertMetersToMiles(long meters)
		{
			return Math.Round(meters * 0.000621371, 2); // 1 meter = 0.000621371 miles
		}

		// Convert distance text to double
		double ConvertDistanceTextToDouble(string distanceText)
		{
			// Use a regular expression to extract the numeric part
			var numericPart = Regex.Match(distanceText, @"\d+(\.\d+)?").Value;

			// Replace any commas with dots for decimal values
			numericPart = numericPart.Replace(',', '.');

			if (double.TryParse(numericPart, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
			{
				return result;
			}
			else
			{
				throw new FormatException($"Unable to parse the distance text '{distanceText}' to a double.");
			}
		}
		//public static string ConvertMetersToMiles(double meters)
		//{
		//	const double MetersToMilesConversionFactor = 0.000621371;
		//	double miles = meters * MetersToMilesConversionFactor;
		//	return miles.ToString("0.##"); // Formats the result to 2 decimal places if needed
		//}

		private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (zoomLabel != null)
			{
				zoomLabel.Content = zoomSlider.Value.ToString("0x");
				RefreshImage();
			}
		}

		private void mapTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RefreshImage();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			zoomLabel.Content = zoomSlider.Value.ToString("0x");
		}

		public class HQItem
		{
			public string HQName { get; set; }
			public double Distance { get; set; }
			public double Fee { get; set; }
		}

		private void destinationTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			destinationLocation = destinationTextBox.Text;
		}
	}
}
