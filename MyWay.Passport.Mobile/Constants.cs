namespace MyWay.Passport.Mobile
{
    public static class Constants
    {
        // Disable API on developer devices
#if DEBUG
        public const bool EnableAnalytics = false;
#else
        public const bool EnableAnalytics = true;
#endif

        // TransportCanberra URLs
        public const string TransportCanberraDomain = "www.transport.act.gov.au";
        public const string BalanceCheckApiUrl = "https://" + TransportCanberraDomain + "/tickets-and-myway/check-myway-balance/check-your-balance?sq_content_src=%2BdXJsPWh0dHAlM0ElMkYlMkZmaWxlcy50cmFuc3BvcnQuYWN0Lmdvdi5hdSUyRkFSVFMlMkZ1c2VfRnVuY3MuYXNwJmFsbD0x";
        public const string RegisterCardUrl = "https://" + TransportCanberraDomain + "/tickets-and-myway/register-myway";
        public const string PasswordHelpUrl = "https://" + TransportCanberraDomain + "/tickets-and-myway/check-myway-balance#Protect";

        // Susfusion Licence
        public const string SysfusionLicenceKey = "NDQzNzIxQDMxMzkyZTMxMmUzMG0wRTFBcTBKVkkxbXlwbmJUSmZYbFMxRWpJa0REbHVjTDJzdzVDU0ZRdkU9";

        // Internal URLs
        public const string PrivacyPolicyUrl = "https://www.jamespetty.com.au/privacy.html";

        // Background app refresh frequency (seconds). Minimum 15 minutes.
        public const double BackgroundRefreshFrequency = 1800;

        // Balance to send out warning at
        public const double BalanceWarningLimit = 5.0;

        public static class SettingNames
        {
            public const string CardDetails = "CardDetails";
        }

        public static class EventNames
        {
            public const string OnResume = "OnResume";
        }

        public static class ErrorMessages
        {
            public const string BalanceCheckFailure = "Failed to retrieve balance.\n\n\nCheck your card details.";
            public const string BalanceCheckMissingCardDetails = "Enter MyWay card details to view balance.";

            public const string RecentTripsFailure = "Failed to retrieve trip history.\n\n\nCheck your card details.";
            public const string RecentTripsMissingCardDetails = "Enter MyWay card details to view recent trips.";
        }

        public static class AnalyticsEvents
        {
            public const string BalanceRefershSuccess = "BalanceRefershSuccess";
            public const string BalanceRefershFailure = "BalanceRefershFailure";

            public const string RecentTripRefreshSuccess = "RecentTripRefreshSuccess";
            public const string RecentTripRefreshFailure = "RecentTripRefreshFailure";
        }
    }
}
