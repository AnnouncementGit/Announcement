using System;
using System.Threading.Tasks;
using Android.Locations;
using Android.App;
using Android.Content;
using Android.OS;
using System.Collections.Generic;

namespace Announcement.Android
{
    public static class LocationService
    {
        public static bool IsLocationEnabled
        {
            get
            {
                return locationManager.IsProviderEnabled(LocationManager.GpsProvider) || locationManager.IsProviderEnabled(LocationManager.NetworkProvider);
            }
        }
        
        static LocationService()
        {
            locationManager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);

            locationListener = new LocationListener(locationManager);
        }

        public static async Task<Location> GetLocationAsync()
        {
            return await locationListener.GetCurrentLocation();
        }

		public static async Task<string> GetCityAsync()
		{
			return await locationListener.GetCurrentCity();
		}

        private static LocationManager locationManager;

        private static LocationListener locationListener;
    }

    public class LocationListener : Java.Lang.Object, ILocationListener
    {
        public LocationListener(LocationManager locationManager)
        {
            this.locationManager = locationManager;
        }

        public async Task<global::Android.Locations.Location> GetCurrentLocation()
        {
			isGetCity = false;

            tskCompletionSource = new TaskCompletionSource<global::Android.Locations.Location>();

            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, LOCATION_REFRESH_TIME, LOCATION_REFRESH_DISTANCE, this);

            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, LOCATION_REFRESH_TIME, LOCATION_REFRESH_DISTANCE, this);

            return await tskCompletionSource.Task;
        }

        public void OnLocationChanged(global::Android.Locations.Location location)
        {
            locationManager.RemoveUpdates(this);

            MakeUseOfNewLocation(location);

            if(currentBestLocation == null)
            {
                currentBestLocation = location;
            }

			if (!isGetCity) {
				if (tskCompletionSource != null && tskCompletionSource.Task != null && !tskCompletionSource.Task.IsCompleted) {
					tskCompletionSource.TrySetResult (currentBestLocation);
				}
			} else
				GetCity ();
        }

		public async Task<string> GetCurrentCity()
		{
			isGetCity = true;

			tskCompletionSourceCity = new TaskCompletionSource<string>();

			locationManager.RequestLocationUpdates(LocationManager.GpsProvider, LOCATION_REFRESH_TIME, LOCATION_REFRESH_DISTANCE, this);

			locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, LOCATION_REFRESH_TIME, LOCATION_REFRESH_DISTANCE, this);					

			return await tskCompletionSourceCity.Task;

		}


		private async void GetCity()
		{
			Geocoder coder = new Geocoder(Application.Context, new Java.Util.Locale("uk"));
			string city = string.Empty;
			try {
				var result = await coder.GetFromLocationAsync(currentBestLocation.Latitude, currentBestLocation.Longitude, 1);
				if(result != null && result.Count > 0)
					city = result[0].Locality;
			} catch (Exception e) {
				// nothing
			}

			if (tskCompletionSourceCity != null && tskCompletionSourceCity.Task != null && !tskCompletionSourceCity.Task.IsCompleted) {
				isGetCity = false;
				tskCompletionSourceCity.TrySetResult (city);
			}
		}

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        } 

        protected global::Android.Locations.Location LastBestLocation() 
        {
            var locationGPS = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);

            var locationNet = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);

            long GPSLocationTime = 0;

            if (null != locationGPS) 
            { 
                GPSLocationTime = locationGPS.Time; 
            }

            long NetLocationTime = 0;

            if (null != locationNet) 
            {
                NetLocationTime = locationNet.Time;
            }

            if ( 0 < (GPSLocationTime - NetLocationTime) ) 
            {
                return locationGPS;
            }
            else 
            {
                return locationNet;
            }         
        }

        protected void MakeUseOfNewLocation(global::Android.Locations.Location location) 
        {
            if ( IsBetterLocation(location, currentBestLocation) )
            {
                currentBestLocation = location;
            }
        }

        protected bool IsBetterLocation(global::Android.Locations.Location location, global::Android.Locations.Location currentBestLocation) 
        {
            if (currentBestLocation == null) 
            {
                return true;
            }

            long timeDelta = location.Time - currentBestLocation.Time;

            var isSignificantlyNewer = timeDelta > LOCATION_REFRESH_TIME;

            var isSignificantlyOlder = timeDelta < - LOCATION_REFRESH_TIME;

            var isNewer = timeDelta > 0;

            if (isSignificantlyNewer) 
            {
                return true;
            } 
            else if (isSignificantlyOlder) 
            {
                return false;
            }

            int accuracyDelta = (int) (location.Accuracy - currentBestLocation.Accuracy);

            var isLessAccurate = accuracyDelta > 0;

            var isMoreAccurate = accuracyDelta < 0;

            var isSignificantlyLessAccurate = accuracyDelta > 200;

            var isFromSameProvider = IsSameProvider(location.Provider, currentBestLocation.Provider);

            if (isMoreAccurate) 
            {
                return true;
            } 
            else if (isNewer && !isLessAccurate) 
            {
                return true;
            } 
            else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider) 
            {
                return true;
            }
            return false;
        }

        protected bool IsSameProvider(string provider1, string provider2)
        {
            if (provider1 == null)
            {
                return provider2 == null;
            }

            return provider1.Equals(provider2);
        }

        private LocationManager locationManager;

        private global::Android.Locations.Location currentBestLocation;

        private TaskCompletionSource<global::Android.Locations.Location> tskCompletionSource;

		private TaskCompletionSource<string> tskCompletionSourceCity;

		private bool isGetCity = false;

        private const int LOCATION_REFRESH_TIME = 1000;

        private const int LOCATION_REFRESH_DISTANCE = 10;
    }
}
