namespace MyWay.Passport.Mobile
{
    public static class Constants
    {
        // TransportCanberra URLs
        public const string BalanceCheckApiUrl = "https://www.transport.act.gov.au/tickets-and-myway/check-myway-balance/check-your-balance?sq_content_src=%2BdXJsPWh0dHAlM0ElMkYlMkZmaWxlcy50cmFuc3BvcnQuYWN0Lmdvdi5hdSUyRkFSVFMlMkZ1c2VfRnVuY3MuYXNwJmFsbD0x";
        public const string RegisterCardUrl = "https://www.transport.act.gov.au/tickets-and-myway/register-myway";

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
