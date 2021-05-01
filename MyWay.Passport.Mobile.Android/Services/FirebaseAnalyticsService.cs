using System.Collections.Generic;
using Android.OS;
using Firebase.Analytics;
using MyWay.Passport.Mobile.Droid.Services;
using MyWay.Passport.Mobile.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAnalyticsService))]
namespace MyWay.Passport.Mobile.Droid.Services
{
	public class FirebaseAnalyticsService : IFirebaseAnalyticsService
	{
		public void LogEvent(string eventId)
		{
			LogEvent(eventId, parameters: null);
		}

		public void LogEvent(string eventId, string message)
		{
			LogEvent(eventId, "message", message);
		}

		public void LogEvent(string eventId, string paramName, string value)
		{
			LogEvent(eventId, new Dictionary<string, string>
			{
				{paramName, value}
			});
		}

		public void SetUserId(string userId)
		{
			var fireBaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.AppContext);

			fireBaseAnalytics.SetUserId(userId);
		}

		public void LogEvent(string eventId, IDictionary<string, string> parameters)
		{
			var fireBaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.AppContext);

			if (parameters == null)
			{
				fireBaseAnalytics.LogEvent(eventId, null);
				return;
			}

			var bundle = new Bundle();

			foreach (var item in parameters)
			{
				bundle.PutString(item.Key, item.Value);
			}

			fireBaseAnalytics.LogEvent(eventId, bundle);
		}
	}
}
