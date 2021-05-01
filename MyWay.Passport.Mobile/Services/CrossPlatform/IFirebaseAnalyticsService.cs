using System.Collections.Generic;

namespace MyWay.Passport.Mobile.Services
{
    public interface IFirebaseAnalyticsService
    {
        void LogEvent(string eventId);
        void LogEvent(string eventId, string message);
        void LogEvent(string eventId, string paramName, string value);
        void LogEvent(string eventId, IDictionary<string, string> parameters);
        void SetUserId(string userId);
    }
}
