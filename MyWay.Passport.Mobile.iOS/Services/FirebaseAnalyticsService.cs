using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;
using MyWay.Passport.Mobile.iOS.Services;
using MyWay.Passport.Mobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAnalyticsService))]
namespace MyWay.Passport.Mobile.iOS.Services
{
    public class FirebaseAnalyticsService : IFirebaseAnalyticsService
    {
        public void LogEvent(string eventId)
        {
            LogEvent(eventId, (IDictionary<string, string>)null);
        }

        public void LogEvent(string eventId, string message)
        {
            LogEvent(eventId, "message", message);
        }

        public void LogEvent(string eventId, string paramName, string value)
        {
            LogEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void SetUserId(string userId)
        {
            Analytics.SetUserId(userId);
        }

#pragma warning disable CS0162 // Unreachable code detected
        public void LogEvent(string eventId, IDictionary<string, string> parameters)
        {
            // Don't trigger analytics event if disabled
            if (!Constants.EnableAnalytics)
            {
                return;
            }

            if (parameters == null)
            {
                Analytics.LogEvent(eventId, parameters: null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
}
